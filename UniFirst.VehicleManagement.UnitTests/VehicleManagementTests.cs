using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniFirst.VehicleManagement.DataAccess;
using UniFirst.VehicleManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace UniFirst.VehicleManagement.UnitTests
{
    [TestClass]
    public class VehicleManagementTests
    {
        private SimpleDependencyInjection _di;

        [TestInitialize]
        public void Init()
        {
            _di = new SimpleDependencyInjection();
            _di.Register<IVehicleDAO, SampleVehicleDAO>(ObjectLifeTime.Transient);
            _di.Register<IVehicleService, VehicleService>(ObjectLifeTime.Transient);
        }

        [TestMethod]
        public void TestValidVinFormat()
        {
            var svc = _di.Resolve<IVehicleService>();

            SetupLocations(svc);
            var location = svc.LookupLocation("BR1");
            var validVehicle = SetupNewVehicle();

            svc.RegisterVehicle(validVehicle, location);
        }

        [TestMethod]
        public void TestInvalidVinLast5()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var location = svc.LookupLocation("BR1");
                var invalidVinLast5NotNumeric = SetupNewVehicle("ABCDEFGH1111111111100X00");

                svc.RegisterVehicle(invalidVinLast5NotNumeric, location);
                Assert.Fail("Exception expected.");
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual("VIN must end with 5 digits and have at least 8 characters.", ex.Message);
            }
        }

        [TestMethod]
        public void TestInvalidVinNoChars()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var location = svc.LookupLocation("BR1");
                var invalidVinNoChars = SetupNewVehicle("111111111111111111100000");

                svc.RegisterVehicle(invalidVinNoChars, location);
                Assert.Fail("Exception expected.");
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual("VIN must end with 5 digits and have at least 8 characters.", ex.Message);
            }
        }

        [TestMethod]
        public void TestInvalidTooShort()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var location = svc.LookupLocation("BR1");
                var invalidVinTooShort = SetupNewVehicle("2CNDXYL63F8661692");

                svc.RegisterVehicle(invalidVinTooShort, location);
                Assert.Fail("Exception expected.");
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual("VIN should be 24 characters in length.", ex.Message);
            }
        }

        [TestMethod]
        public void TestYearInvalid()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var location = svc.LookupLocation("BR1");
                var invalidBadYear = SetupNewVehicle(year: "200X");

                svc.RegisterVehicle(invalidBadYear, location);
                Assert.Fail("Exception expected.");
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual("Four digit year", ex.Message);
            }
        }

        [TestMethod]
        public void TestSuccessfulVanTransfer()
        {
            var svc = _di.Resolve<IVehicleService>();

            SetupLocations(svc);
            var distCenter = svc.LookupLocation("DC1");
            var branch = svc.LookupLocation("BR1");
            var van = SetupNewVehicle();

            Assert.IsFalse(distCenter.Vehicles.Contains(van));
            Assert.IsFalse(branch.Vehicles.Contains(van));

            svc.RegisterVehicle(van, distCenter);

            Assert.AreSame(distCenter, van.CurrentLocation);
            Assert.IsTrue(distCenter.Vehicles.Contains(van));
            Assert.AreNotSame(branch, van.CurrentLocation);
            Assert.IsFalse(branch.Vehicles.Contains(van));

            svc.TransferVehicle(van, branch);

            Assert.AreNotSame(distCenter, van.CurrentLocation);
            Assert.IsFalse(distCenter.Vehicles.Contains(van));
            Assert.AreSame(branch, van.CurrentLocation);
            Assert.IsTrue(branch.Vehicles.Contains(van));
        }

        [TestMethod]
        public void TestSemiTransferFailure()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var distCenter = svc.LookupLocation("DC1");
                var branch = svc.LookupLocation("BR1");
                var semi = SetupNewVehicle(type: VehicleType.Semi);

                Assert.IsFalse(distCenter.Vehicles.Contains(semi));
                Assert.IsFalse(branch.Vehicles.Contains(semi));

                svc.RegisterVehicle(semi, distCenter);

                Assert.AreSame(distCenter, semi.CurrentLocation);
                Assert.IsTrue(distCenter.Vehicles.Contains(semi));
                Assert.AreNotSame(branch, semi.CurrentLocation);
                Assert.IsFalse(branch.Vehicles.Contains(semi));

                svc.TransferVehicle(semi, branch);
                Assert.Fail("Exception expected.");
            }
            catch (VehicleTransferException ex)
            {
                Assert.AreEqual("A semi can be transferred between distribution centers, but not to branches.", ex.Message);
            }
        }

        [TestMethod]
        public void TestNonStandByTransferFailure()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var distCenter = svc.LookupLocation("DC1");
                var branch = svc.LookupLocation("BR1");
                var van = SetupNewVehicle();

                Assert.IsFalse(distCenter.Vehicles.Contains(van));
                Assert.IsFalse(branch.Vehicles.Contains(van));

                svc.RegisterVehicle(van, distCenter);

                van.Status = VehicleStatus.InRepair;

                Assert.AreSame(distCenter, van.CurrentLocation);
                Assert.IsTrue(distCenter.Vehicles.Contains(van));
                Assert.AreNotSame(branch, van.CurrentLocation);
                Assert.IsFalse(branch.Vehicles.Contains(van));

                svc.TransferVehicle(van, branch);
                Assert.Fail("Exception expected.");
            }
            catch (VehicleTransferException ex)
            {
                Assert.AreEqual("Only vehicles in stand-by can be transferred.", ex.Message);
            }
        }

        [TestMethod]
        public void TestSameLocationTransferFailure()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var distCenter = svc.LookupLocation("DC1");
                var van = SetupNewVehicle();

                Assert.IsFalse(distCenter.Vehicles.Contains(van));

                svc.RegisterVehicle(van, distCenter);

                Assert.AreSame(distCenter, van.CurrentLocation);
                Assert.IsTrue(distCenter.Vehicles.Contains(van));

                svc.TransferVehicle(van, distCenter);
                Assert.Fail("Exception expected.");
            }
            catch (VehicleTransferException ex)
            {
                Assert.AreEqual("The target location must be different than the current location.", ex.Message);
            }
        }
        [TestMethod]
        public void TestNonStandByAndSemiTransferFailure()
        {
            try
            {
                var svc = _di.Resolve<IVehicleService>();

                SetupLocations(svc);
                var distCenter = svc.LookupLocation("DC1");
                var branch = svc.LookupLocation("BR1");
                var semi = SetupNewVehicle(type: VehicleType.Semi);

                Assert.IsFalse(distCenter.Vehicles.Contains(semi));
                Assert.IsFalse(branch.Vehicles.Contains(semi));

                svc.RegisterVehicle(semi, distCenter);
                semi.Status = VehicleStatus.InService;

                Assert.AreSame(distCenter, semi.CurrentLocation);
                Assert.IsTrue(distCenter.Vehicles.Contains(semi));
                Assert.AreNotSame(branch, semi.CurrentLocation);
                Assert.IsFalse(branch.Vehicles.Contains(semi));

                svc.TransferVehicle(semi, branch);
                Assert.Fail("Exception expected.");
            }
            catch (VehicleTransferException ex)
            {
                Assert.AreEqual(ex.FailureReasonFlags, 
                    (VehicleTransferFailureReasonFlags.SemiCannotTransferToBranches | VehicleTransferFailureReasonFlags.VehiclesCannotTransferUnlessOnStandBy));
            }
        }

        private Vehicle SetupNewVehicle(
            string vin = "52CNDL63ABCR21F866169285",
            string year = "1988",
            VehicleType type = VehicleType.Van,
            VehicleStatus status = VehicleStatus.StandBy)
        {
            return new Vehicle
            {
                Make = "GMC",
                VIN = vin,
                Year = year,
                Model = "Brigadier",
                Status = status,
                Type = type
            };
        }

        private void SetupLocations(IVehicleService svc)
        {
            // Add sample data
            svc.RegisterLocation(
                new Location
                {
                    Code = "BR1",
                    Name = "Branch 001",
                    Type = LocationType.Branch
                });
            svc.RegisterLocation(
                new Location
                {
                    Code = "BR2",
                    Name = "Branch 002",
                    Type = LocationType.Branch
                });
            svc.RegisterLocation(
                new Location
                {
                    Code = "DC1",
                    Name = "Distribution Center 001",
                    Type = LocationType.DistributionCenter
                });
            svc.RegisterLocation(
                new Location
                {
                    Code = "DC2",
                    Name = "Distribution Center 002",
                    Type = LocationType.DistributionCenter
                });

        }
    }
}
