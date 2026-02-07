namespace FITR_DC_FORM.Models.ViewModels
{
    public class FitrViewModel
    {
        public FitrMaster Master { get; set; } = new();
        public List<FitrMaterial> Materials { get; set; } = new();
        public List<FitrSrData> SrData { get; set; } = new();
        public List<FitrVisual> Visuals { get; set; } = new();
    }
}
