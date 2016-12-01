using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniFirst.VehicleManagement
{
    [Flags]
    public enum VehicleTransferFailureReasonFlags
    {
        [Description("None.")]
        None = 0,
        [Description("A semi can be transferred between distribution centers, but not to branches.")]
        SemiCannotTransferToBranches = 1,
        [Description("Only vehicles in stand-by can be transferred.")]
        VehiclesCannotTransferUnlessOnStandBy = 2,
        [Description("The target location must be different than the current location.")]
        VehiclesCannotTransferToSameLocation = 4
    }
}
