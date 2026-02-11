using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public bool CanView(int userId, string pageName)
        {
            var right = _repository.GetByUserAndPage(userId, pageName);
            return right != null && right.CanView;
        }

        public bool CanCreate(int userId, string pageName)
        {
            var right = _repository.GetByUserAndPage(userId, pageName);
            return right != null && right.CanCreate;
        }

        public bool CanEdit(int userId, string pageName)
        {
            var right = _repository.GetByUserAndPage(userId, pageName);
            return right != null && right.CanEdit;
        }

        public bool CanDelete(int userId, string pageName)
        {
            var right = _repository.GetByUserAndPage(userId, pageName);
            return right != null && right.CanDelete;
        }
    }
}
