using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class FitrVisualMasterService : IFitrVisualMasterService
    {
        private readonly IFitrVisualMasterRepository _repo;

        public FitrVisualMasterService(IFitrVisualMasterRepository repo)
        {
            _repo = repo;
        }

        public List<FitrVisualMaster> GetAll()
        {
            return _repo.GetAll();
        }

        public FitrVisualMaster GetById(int id)
        {
            return _repo.GetById(id);
        }

        public void Save(FitrVisualMaster model)
        {
            _repo.Save(model);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
