using Microsoft.AspNetCore.Mvc;

namespace FITR_DC_FORM.Models
{ 
   public class FitrMaster
    {
        public int FitrId { get; set; }

        public string? CompanyName { get; set; }
        [Remote(
        action: "CheckDuplicateDc",
        controller: "Fitr",
        AdditionalFields = nameof(FitrId),
        ErrorMessage = "This DC No & Date already exists"
    )]
        public string? DCNo { get; set; }
        public DateTime? DCDate { get; set; }

        public string? PONo { get; set; }
        public DateTime? PODate { get; set; }

        public int? Quantity { get; set; }

        // 🔴 NVARCHAR IN DB → STRING
        public string? Status { get; set; }
        public string? ProductType { get; set; }

        // 🔴 NVARCHAR IN DB → STRING (NOT DECIMAL)
        public string? Torque { get; set; }
        public string? Ratio { get; set; }
        public string? HandwheelDia { get; set; }

        public string? Model { get; set; }
        //public string? Tapping { get; set; }
        //public string? Hardware { get; set; }
        //public string? Painting { get; set; }
        //public string? ShaftMovement { get; set; }

        // 🔴 NVARCHAR(MAX)
        public string? FinalRemarks { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // 🔴 NVARCHAR(20) → STRING

        public int? TCNo { get; set; }          // Running serial number
        public string? TCYear { get; set; }     // Financial year like 25-26
        public DateTime? TCDate { get; set; }   // TC creation date




        // for priview only 
        public int? PreviewTCNo { get; set; }
        public string? PreviewTCYear { get; set; }

        //public string? LastStepCompleted { get; set; }



        /* 🔥 NEW ATTACHMENT PROPERTIES 🔥 */

        public string DCAttachmentPath { get; set; }     // Mandatory
        public string POAttachmentPath { get; set; }     // Mandatory
        public string? DrawingAttachmentPath { get; set; } // Optional
        public string? TCAttachmentPath { get; set; }      // Optional

      
        public int? CompanyId { get; set; }
        public int? LocationId { get; set; }
        public int? CreatedByUserId { get; set; }

        // CHILD COLLECTIONS

        public string CreatedByName { get; set; }
        public string CreatedBySignature { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HodApprovedByName { get; set; }
        public string HodSignature { get; set; }
        public DateTime? HodApprovedOn { get; set; }

        public string? AdminApprovedByName { get; set; }
        public string AdminSignature { get; set; }
        public DateTime? AdminApprovedOn { get; set; }
        public List<FitrMaterial> Materials { get; set; } = new();
        public List<FitrSrData> SrData { get; set; } = new();
        public List<FitrVisual> Visuals { get; set; } = new();
    }


}
