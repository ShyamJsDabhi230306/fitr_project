using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface IUserPageRightRepository
    {
        List<UserPageRight> GetAll(); // For admin screen

        List<UserPageRight> GetByUser(int userId); // Load rights of selected user

        UserPageRight GetByUserAndPage(int userId, string pageName); // Permission check

        void Update(UserPageRight model); // Update rights

        void BulkUpdate(List<UserPageRight> rights); // Bulk update rights
    }
}
