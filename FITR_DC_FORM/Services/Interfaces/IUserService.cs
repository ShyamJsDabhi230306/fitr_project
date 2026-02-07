using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IUserService
    {
        int Save(UserMaster model);
        List<UserMaster> GetAll(int companyId = 0, int locationId = 0);
        UserMaster GetById(int userId);
        bool Delete(int userId);
        List<LocationMaster> GetByCompany(int companyId);

    }
}
