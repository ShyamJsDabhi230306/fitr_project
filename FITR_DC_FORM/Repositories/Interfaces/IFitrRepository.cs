using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    

    public interface IFitrRepository
    {
        // ================= SAVE =================
        int SaveBasic(FitrMaster master);
        void SaveProduct(FitrMaster master);
        void SaveMaterials(int fitrId, List<FitrMaterial> materials);
        void SaveSrData(int fitrId, List<FitrSrData> srData);
        void SaveVisuals(int fitrId, List<FitrVisual> visuals);
        void SaveFinalRemark(int fitrId, string remarks);

        // ================= GET =================
        FitrViewModel GetFitr(int? fitrId);

        List<FitrMaster> GetListByRole(
            string userRole,
            int companyId,
            int locationId,
            int userId
        );

        List<FitrVisualMaster> GetAllVisualMaster();

        // ================= VALIDATION =================
        bool IsDcDuplicate(string dcNo, DateTime dcDate, int fitrId);

        // ================= TC =================
        (int tcNo, string tcYear) GetNextTCPreview();
        void GenerateTCIfNotExists(int fitrId);

        // ================= FLOW =================
        void SubmitByUser(int fitrId, int userId);
        void ApproveByHod(int fitrId, int hodUserId);
        void ApproveByAdmin(int fitrId, int adminUserId);


        // 🔥 ADD THIS LINE EXACTLY
        bool IsDcNoExists(string dcNo, int? fitrId);

        // ================= DELETE =================
        void SoftDelete(int fitrId, int deletedBy);

        List<FitrMaster> GetListByRoleFilteredV2(
        string userRole,
        int companyId,
        int locationId,
        string filter);
    }
}
