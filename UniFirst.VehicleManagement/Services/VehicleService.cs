using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UniFirst.VehicleManagement.DataAccess;
using UniFirst.VehicleManagement.Model;

namespace UniFirst.VehicleManagement
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleDAO _dao;

        public VehicleService(IVehicleDAO dao)
        {
            _dao = dao;
        }

        public Location LookupLocation(string locationCode)
        {
            return _dao.Locations.FirstOrDefault(l => l.Code == locationCode);
        }

        public Vehicle LookupVehicle(string vin)
        {
            return _dao.Vehicles.FirstOrDefault(l => l.VIN == vin);
        }

        public void RegisterLocation(Location location)
        {
            _dao.AddLocation(location);
        }

        public void RegisterVehicle(Vehicle vehicle, Location initialLocation)
        {
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(vehicle, new ValidationContext(vehicle), results, validateAllProperties: true))
            {
                // add the vehicle to the data repository
                _dao.AddVehicle(vehicle);
                TransferVehicle(vehicle, initialLocation);
            }
            else
            {
                throw new ValidationException(
                    results
                        .Where(r => r != ValidationResult.Success)
                        .Select(r => r.ErrorMessage ?? "Error")
                        .Aggregate((i, j) => i + Environment.NewLine + j));
            }
        }

        public void TransferVehicle(Vehicle vehicle, Location targetLocation)
        {
            var currentLocation = vehicle.CurrentLocation;
            var transferFailureFlags = VehicleTransferFailureReasonFlags.None;

            if (currentLocation != null && currentLocation == targetLocation)
            {
                transferFailureFlags |= VehicleTransferFailureReasonFlags.VehiclesCannotTransferToSameLocation;
            }

            if (vehicle.Status != VehicleStatus.StandBy)
            {
                transferFailureFlags |= VehicleTransferFailureReasonFlags.VehiclesCannotTransferUnlessOnStandBy;
            }

            if (vehicle.Type == VehicleType.Semi && targetLocation.Type == LocationType.Branch)
            {
                transferFailureFlags |= VehicleTransferFailureReasonFlags.SemiCannotTransferToBranches;
            }

            if (transferFailureFlags == VehicleTransferFailureReasonFlags.None)
            {
                if (currentLocation?.Vehicles.Contains(vehicle) ?? false)
                {
                    currentLocation.Vehicles.Remove(vehicle);
                }

                if (!targetLocation.Vehicles.Contains(vehicle))
                {
                    targetLocation.Vehicles.Add(vehicle);
                }
                vehicle.CurrentLocation = targetLocation;
            }
            else
            {
                throw new VehicleTransferException(transferFailureFlags);
            }
        }
    }
}