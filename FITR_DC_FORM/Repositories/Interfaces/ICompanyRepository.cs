using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        int Save(CompanyMaster model);
        List<CompanyMaster> GetAll();
        CompanyMaster GetById(int companyId);
        bool Delete(int companyId);
    }
}
