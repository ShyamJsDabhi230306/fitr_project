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

        public List<UserPageRight> GetRightsByUser(int userId)
            => _repository.GetByUser(userId);

        public UserPageRight GetRight(int userId, string pageName)
            => _repository.GetByUserAndPage(userId, pageName);

        public void BulkUpdateRights(List<UserPageRight> rights)
            => _repository.BulkUpdate(rights);

        public void AssignDefaultRights(int userId)
            => _repository.AssignDefaultRights(userId);
    }

}
