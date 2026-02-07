namespace FITR_DC_FORM.Models
{
    public class FitrVisual
    {
        public int VisualId { get; set; }
        public int FitrId { get; set; }

        public int VisualMasterId { get; set; }   // 🔥 selected item
        public string? VisualItemName { get; set; }
        public string? ProductValue { get; set; } // 🔥 product input
    }

}
