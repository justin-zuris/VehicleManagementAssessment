using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniFirst.VehicleManagement
{
    public class VehicleTransferException : Exception
    {
        public VehicleTransferException(VehicleTransferFailureReasonFlags flags) : 
            base(flags.GetUniqueFlags()
                .Select(enumVal => ((VehicleTransferFailureReasonFlags)enumVal).GetEnumDescription())
                .Aggregate((i, j) => i + Environment.NewLine + j))
        {
            FailureReasonFlags = flags;
        }

        public VehicleTransferFailureReasonFlags FailureReasonFlags { get; set; }
    }


}
