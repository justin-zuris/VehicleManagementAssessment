using System.Linq;
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