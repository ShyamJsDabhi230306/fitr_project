namespace FITR_DC_FORM.Models
{
    public class CompanyMaster
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string PanNo { get; set; }

        public string GSTNo { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
