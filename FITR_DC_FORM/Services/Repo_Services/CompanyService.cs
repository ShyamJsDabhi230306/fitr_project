using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public int Save(CompanyMaster model)
        {
            return _repository.Save(model);
        }

        public List<CompanyMaster> GetAll()
        {
            return _repository.GetAll();
        }

        public CompanyMaster GetById(int companyId)
        {
            return _repository.GetById(companyId);
        }

        public bool Delete(int companyId)
        {
            return _repository.Delete(companyId);
        }
    }
}
