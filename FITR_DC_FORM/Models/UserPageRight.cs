using System.ComponentModel.DataAnnotations.Schema;

namespace FITR_DC_FORM.Models
{
    public class UserPageRight
    {
        public int RightId { get; set; }
        public int UserId { get; set; }

        // 🔥 Make nullable
        public string? PageName { get; set; }

        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
