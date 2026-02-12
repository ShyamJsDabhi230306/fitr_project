using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface IUserPageRightRepository
    {
        List<UserPageRight> GetByUser(int userId);

        UserPageRight GetByUserAndPage(int userId, string pageName);

        void BulkUpdate(List<UserPageRight> rights);

        void AssignDefaultRights(int userId);
    }
}
