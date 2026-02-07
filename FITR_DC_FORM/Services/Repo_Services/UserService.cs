using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public int Save(UserMaster model) => _repository.Save(model);
        public List<UserMaster> GetAll(int companyId = 0, int locationId = 0) => _repository.GetAll(companyId, locationId);
        public UserMaster GetById(int userId) => _repository.GetById(userId);
        public bool Delete(int userId) => _repository.Delete(userId);
        public List<LocationMaster> GetByCompany(int companyId)
        {
            return _repository.GetByCompany(companyId);
        }
    }
}
