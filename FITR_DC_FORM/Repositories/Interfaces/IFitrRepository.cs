using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    //public interface IFitrRepository
    //{
    //    int SaveBasic(FitrMaster master);
    //    void SaveProduct(FitrMaster master);
    //    List<FitrVisualMaster> GetAllVisualMaster();
    //    void SaveVisuals(int fitrId, List<FitrVisual> visuals);

    //    void SaveFinalRemark(int fitrId, string remarks);

    //    void SaveMaterials(int fitrId, List<FitrMaterial> materials);
    //    void SaveSrData(int fitrId, List<FitrSrData> srData);

    //    FitrViewModel GetFitr(int? fitrId);
    //    //List<FitrMaster> GetDcList();
    ////    List<FitrMaster> GetDcList(
    ////    string userRole,
    ////    int companyId,
    ////    int locationId,
    ////    int userId
    ////);

    //    bool IsDcDuplicate(string dcNo, DateTime dcDate, int fitrId);

    //    // 🔹 ADD THIS (READ ONLY – PREVIEW)
    //    (int tcNo, string tcYear) GetNextTCPreview();
    //    // ✅ ADD THIS
    //    void GenerateTCIfNotExists(int fitrId);



    //   List<FitrMaster> GetListByRole(
    //            string userRole,
    //            int companyId,
    //            int locationId,
    //            int userId
    //        );


    //    void SubmitByUser(int fitrId, int userId);
    //    void ApproveByHod(int fitrId, int hodUserId);
    //    void ApproveByAdmin(int fitrId, int adminUserId);



    //}

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


        List<FitrMaster> GetListByRoleFilteredV2(
        string userRole,
        int companyId,
        int locationId,
        string filter);
    }
}
