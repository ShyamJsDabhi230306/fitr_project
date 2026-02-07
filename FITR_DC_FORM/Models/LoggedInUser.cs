namespace FITR_DC_FORM.Models
{
    public class LoggedInUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public string? SignaturePath { get; set; }
    }
}
