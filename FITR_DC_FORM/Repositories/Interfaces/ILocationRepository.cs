using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        // Create / Update (same method)
        void Save(LocationMaster model);

        // Read
        List<LocationMaster> GetAll();
        LocationMaster GetById(int locationId);

        // Soft Delete
        void SoftDelete(int locationId);
        List<LocationMaster> GetByCompany(int companyId);


    }
}
