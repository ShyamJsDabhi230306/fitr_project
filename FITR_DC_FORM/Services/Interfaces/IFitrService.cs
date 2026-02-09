using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IFitrService
    {
        // ================= SAVE =================
        int SaveBasic(FitrMaster master);
        void SaveProduct(FitrMaster master);
        void SaveMaterials(int fitrId, List<FitrMaterial> materials);
        void SaveSrData(int fitrId, List<FitrSrData> srData);
        void SaveVisuals(int fitrId, List<FitrVisual> visuals);
        void SaveFinalRemark(int fitrId, string remark);

        // ================= GET =================
        FitrViewModel GetFitr(int? fitrId);
        List<FitrVisualMaster> GetAllVisualMaster();

        List<FitrMaster> GetListByRole(
            string userRole,
            int companyId,
            int locationId,
            int userId
        );

        // ================= ATTACHMENTS =================
        void SaveDrawingAttachment(int fitrId, string drawingPath);
        void SaveTCAttachment(int fitrId, string tcPath);

        // ================= TC =================
        void GenerateTCIfNotExists(int fitrId);
        (int tcNo, string tcYear) GetNextTCPreview();

        // ================= VALIDATION =================
        bool IsDcDuplicate(string dcNo, DateTime dcDate, int fitrId);

        // ================= FLOW =================
        void SubmitByUser(int fitrId, int userId);
        void ApproveByHod(int fitrId, int hodUserId);
        void ApproveByAdmin(int fitrId, int adminUserId);

        bool IsDcNoExists(string dcNo, int? fitrId);


      
       List<FitrMaster> GetListByRoleFilteredV2(
                string userRole,
                int companyId,
                int locationId,
                string filter);
        
    }
}
