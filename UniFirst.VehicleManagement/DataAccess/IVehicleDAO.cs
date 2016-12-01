using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniFirst.VehicleManagement.Model;

namespace UniFirst.VehicleManagement.DataAccess
{
    public interface IVehicleDAO
    {
        void AddVehicle(Vehicle v);
        void AddLocation(Location l);
        IQueryable<Vehicle> Vehicles { get; }
        IQueryable<Location> Locations { get; }
    }
}
