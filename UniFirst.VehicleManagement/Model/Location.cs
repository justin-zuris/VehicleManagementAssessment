using System.Collections.Generic;

namespace UniFirst.VehicleManagement.Model
{
    public class Location
    {
        public Location()
        {
            Vehicles = new List<Vehicle>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public LocationType Type { get; set; }
        public List<Vehicle> Vehicles { get; set; }
    }
}