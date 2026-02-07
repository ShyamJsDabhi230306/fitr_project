using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface ILocationService
    {
        List<LocationMaster> GetAll();
        LocationMaster GetById(int id);
        void Save(LocationMaster model);
        void Delete(int id);
        List<LocationMaster> GetByCompany(int companyId);

    }
}
