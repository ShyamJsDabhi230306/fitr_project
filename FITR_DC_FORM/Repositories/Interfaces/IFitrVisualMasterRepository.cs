using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface IFitrVisualMasterRepository
    {
        List<FitrVisualMaster> GetAll();
        FitrVisualMaster GetById(int id);
        void Save(FitrVisualMaster model);
        void Delete(int id);
    }
}
