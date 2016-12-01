using System;
using System.Linq;

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