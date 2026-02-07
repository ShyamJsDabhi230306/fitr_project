namespace FITR_DC_FORM.Models
{
    public class FitrSrData
    {
        public int SrId { get; set; }
        public int FitrId { get; set; }
        public int SrNo { get; set; }

        //public string ShaftDia { get; set; }

        // 🔴 NVARCHAR IN DB → STRING
        public string? ShaftDia { get; set; }
        public string OutputDrive { get; set; }
        public string ActuatorPCD { get; set; }
        public string ValvePCD { get; set; }
        public string SquareDia { get; set; }
        public string SquarePosition { get; set; }
    }
}
