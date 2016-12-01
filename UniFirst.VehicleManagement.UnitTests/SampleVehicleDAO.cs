using System.Collections.Generic;
using System.Linq;
using UniFirst.VehicleManagement.DataAccess;
using UniFirst.VehicleManagement.Model;

namespace UniFirst.VehicleManagement.UnitTests
{
    public class SampleVehicleDAO : IVehicleDAO
    {
        public readonly List<Vehicle> _vehicles = new List<Vehicle>();
        public readonly List<Location> _locations = new List<Location>();

        public IQueryable<Location> Locations
        {
            get
            {
                return _locations.AsQueryable();
            }
        }

        public IQueryable<Vehicle> Vehicles
        {
            get
            {
                return _vehicles.AsQueryable();
            }
        }

        public void AddLocation(Location l)
        {
            _locations.Add(l);
        }

        public void AddVehicle(Vehicle v)
        {
            _vehicles.Add(v);
        }
    }
}