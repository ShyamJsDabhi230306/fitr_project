using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        UserPageRight GetByUserAndPage(int userId, string pageName);
    }
}
