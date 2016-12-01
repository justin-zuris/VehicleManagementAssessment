using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniFirst.VehicleManagement.Model
{
    public class VehicleTransferHistory
    {
        public DateTime TransferDate { get; set; }
        public Location FromLocation { get; set; }
        public Location ToLocation { get; set; }
    }
}
