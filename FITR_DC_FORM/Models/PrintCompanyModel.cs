
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FITR_DC_FORM.Models
{
    public class PrintCompanyModel
    {
        public int PrintCompanyId { get; set; }

        // REQUIRED FIELD
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; } = string.Empty;

        // OPTIONAL FIELDS → MUST BE NULLABLE
        public string? CompanyAddress { get; set; }

        // stored in DB (nullable)
        public string? CompanyLogo { get; set; }

        // upload only (nullable, not validated)
        public IFormFile? CompanyLogoFile { get; set; }

        public bool IsActive { get; set; }



    }
}
