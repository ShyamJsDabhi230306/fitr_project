using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IFitrVisualMasterService
    {
        List<FitrVisualMaster> GetAll();
        FitrVisualMaster GetById(int id);
        void Save(FitrVisualMaster model);
        void Delete(int id);
    }
}
