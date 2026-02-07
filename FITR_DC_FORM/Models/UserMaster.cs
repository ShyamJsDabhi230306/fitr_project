//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FITR_DC_FORM.Models
//{
//    public class UserMaster
//    {
//        public int UserId { get; set; }

//        [Required(ErrorMessage = "Company is required")]
//        [Range(1, int.MaxValue, ErrorMessage = "Please select a company")]
//        public int CompanyId { get; set; }

//        [Required(ErrorMessage = "Location is required")]
//        [Range(1, int.MaxValue, ErrorMessage = "Please select a location")]
//        public int LocationId { get; set; }

//        [Required(ErrorMessage = "User name is required")]
//        public string UserName { get; set; }

//        // 🔴 DISPLAY ONLY — MUST NOT BE VALIDATED
//        [NotMapped]
//        public string? CompanyName { get; set; }

//        [NotMapped]
//        public string? LocationName { get; set; }

//        public string FullName { get; set; }
//        public string Password { get; set; }

//        public DateTime CreatedOn { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FITR_DC_FORM.Models
{
    public class UserMaster
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Company is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a location")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        // 🔹 Role for approval flow
        public string UserRole { get; set; } = "USER"; // USER / ADMIN / HOD

        // 🔹 Signature
        public string? SignaturePath { get; set; }
        [NotMapped]
        public IFormFile? SignatureFile { get; set; }
        public DateTime? SignedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        // DISPLAY ONLY
        [NotMapped] public string? CompanyName { get; set; }
        [NotMapped] public string? LocationName { get; set; }
    }
}
