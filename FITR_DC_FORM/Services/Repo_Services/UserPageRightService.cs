using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class UserPageRightService : IUserPageRightService
    {
        private readonly IUserPageRightRepository _repository;

        public UserPageRightService(IUserPageRightRepository repository)
        {
            _repository = repository;
        }

        public List<UserPageRight> GetAllRights()
        {
            return _repository.GetAll();
        }

        public List<UserPageRight> GetRightsByUser(int userId)
        {
            return _repository.GetByUser(userId);
        }

        public UserPageRight GetRight(int userId, string pageName)
        {
            return _repository.GetByUserAndPage(userId, pageName);
        }

        public void UpdateRights(UserPageRight model)
        {
            _repository.Update(model);
        }

        public void BulkUpdateRights(List<UserPageRight> rights)
        {
            _repository.BulkUpdate(rights);
        }
    }
}
