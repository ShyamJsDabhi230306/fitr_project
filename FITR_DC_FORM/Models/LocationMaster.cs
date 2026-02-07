using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FITR_DC_FORM.Models
{
    public class LocationMaster
    {

        public int LocationId { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Location name is required")]
        public string LocationName { get; set; }

        // 🔴 IMPORTANT FIX
        [NotMapped]                 // 👈 ADD THIS
        public string? CompanyName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
