using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniFirst.VehicleManagement.Model
{
    public class Vehicle
    {
        public Vehicle()
        {
            TransferHistory = new List<VehicleTransferHistory>();
        }

        [Required(ErrorMessage = "Make is required")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required"), RegularExpression(pattern: @"^\d{3}$", ErrorMessage = "Four digit year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "VIN is required"), VINValidation]
        public string VIN { get; set; }

        public VehicleType Type { get; set; }

        public VehicleStatus Status { get; set; }

        public Location CurrentLocation { get; set; }

        public List<VehicleTransferHistory> TransferHistory { get; set; }
    }
}