using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class FitrService : IFitrService
    {
        private readonly IFitrRepository _repo;

        public FitrService(IFitrRepository repo)
        {
            _repo = repo;
        }

        // ================= SAVE =================
        public int SaveBasic(FitrMaster master)
        {
            if (string.IsNullOrWhiteSpace(master.DCAttachmentPath))
                throw new Exception("DC Attachment is required");

            if (string.IsNullOrWhiteSpace(master.POAttachmentPath))
                throw new Exception("PO Attachment is required");

            return _repo.SaveBasic(master);
        }

        public void SaveProduct(FitrMaster master)
            => _repo.SaveProduct(master);

        public void SaveMaterials(int fitrId, List<FitrMaterial> materials)
            => _repo.SaveMaterials(fitrId, materials);

        public void SaveSrData(int fitrId, List<FitrSrData> srData)
            => _repo.SaveSrData(fitrId, srData);

        public void SaveVisuals(int fitrId, List<FitrVisual> visuals)
            => _repo.SaveVisuals(fitrId, visuals);

        public void SaveFinalRemark(int fitrId, string remark)
            => _repo.SaveFinalRemark(fitrId, remark);

        // ================= GET =================
        public FitrViewModel GetFitr(int? fitrId)
            => _repo.GetFitr(fitrId);

        public List<FitrVisualMaster> GetAllVisualMaster()
            => _repo.GetAllVisualMaster();

        public List<FitrMaster> GetListByRole(
            string userRole,
            int companyId,
            int locationId,
            int userId)
        {
            return _repo.GetListByRole(userRole, companyId, locationId, userId);
        }

        // ================= ATTACHMENTS =================
        public void SaveDrawingAttachment(int fitrId, string drawingPath)
        {
            if (fitrId <= 0 || string.IsNullOrWhiteSpace(drawingPath))
                return;

            var model = _repo.GetFitr(fitrId);
            if (model?.Master == null)
                throw new Exception("FITR not found");

            model.Master.DrawingAttachmentPath = drawingPath;
            _repo.SaveBasic(model.Master);
        }

        public void SaveTCAttachment(int fitrId, string tcPath)
        {
            if (fitrId <= 0 || string.IsNullOrWhiteSpace(tcPath))
                return;

            var model = _repo.GetFitr(fitrId);
            if (model?.Master == null)
                throw new Exception("FITR not found");

            model.Master.TCAttachmentPath = tcPath;
            _repo.SaveBasic(model.Master);
        }

        // ================= TC =================
        public void GenerateTCIfNotExists(int fitrId)
            => _repo.GenerateTCIfNotExists(fitrId);

        public (int tcNo, string tcYear) GetNextTCPreview()
            => _repo.GetNextTCPreview();

        // ================= VALIDATION =================
        public bool IsDcDuplicate(string dcNo, DateTime dcDate, int fitrId)
            => _repo.IsDcDuplicate(dcNo, dcDate, fitrId);

        // ================= FLOW =================
        public void SubmitByUser(int fitrId, int userId)
            => _repo.SubmitByUser(fitrId, userId);

        public void ApproveByHod(int fitrId, int hodUserId)
            => _repo.ApproveByHod(fitrId, hodUserId);

        public void ApproveByAdmin(int fitrId, int adminUserId)
            => _repo.ApproveByAdmin(fitrId, adminUserId);


        public bool IsDcNoExists(string dcNo, int? fitrId)
        {
            return _repo.IsDcNoExists(dcNo, fitrId);
        }


        public List<FitrMaster> GetListByRoleFilteredV2(
        string userRole,
        int companyId,
        int locationId,
        string filter)
        {
            return _repo.GetListByRoleFilteredV2(
                userRole, companyId, locationId, filter);
        }
    }
}
