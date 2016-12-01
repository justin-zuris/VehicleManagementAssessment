using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniFirst.VehicleManagement.Model;

namespace UniFirst.VehicleManagement
{
    public interface IVehicleService
    {
        Location LookupLocation(string locationCode);
        Vehicle LookupVehicle(string vin);
        void RegisterLocation(Location Location);
        void RegisterVehicle(Vehicle vehicle, Location initialLocation);
        void TransferVehicle(Vehicle vehicle, Location targetLocation);
    }
}
