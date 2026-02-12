using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IUserPageRightService
    {
        List<UserPageRight> GetRightsByUser(int userId);

        UserPageRight GetRight(int userId, string pageName);

        void BulkUpdateRights(List<UserPageRight> rights);

        void AssignDefaultRights(int userId);
    }
}
