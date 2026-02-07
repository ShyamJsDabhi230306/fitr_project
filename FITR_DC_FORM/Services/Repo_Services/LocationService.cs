using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;

        public LocationService(ILocationRepository repo)
        {
            _repo = repo;
        }

        

        public List<LocationMaster> GetAll() => _repo.GetAll();
        public LocationMaster GetById(int id) => _repo.GetById(id);
        public void Save(LocationMaster model) => _repo.Save(model);
        public void Delete(int id) => _repo.SoftDelete(id);
        public List<LocationMaster> GetByCompany(int companyId)
        {
            return _repo.GetByCompany(companyId);
        }


    }
}

