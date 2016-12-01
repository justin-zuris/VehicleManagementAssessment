using System;

namespace UniFirst.VehicleManagement.Model
{
    public class VehicleTransferHistory
    {
        public DateTime TransferDate { get; set; }
        public Location FromLocation { get; set; }
        public Location ToLocation { get; set; }
    }
}