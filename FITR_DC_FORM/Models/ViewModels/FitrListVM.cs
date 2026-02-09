namespace FITR_DC_FORM.Models.ViewModels
{
    public class FitrListVM
    {
        public int FitrId { get; set; }
        public string CompanyName { get; set; } = "";
        public string DCNo { get; set; } = "";
        public DateTime? DCDate { get; set; }
        public int Quantity { get; set; }
        public string ApprovalStatus { get; set; } = "";

        // UI only
        public bool CanApprove { get; set; }
    }
}
