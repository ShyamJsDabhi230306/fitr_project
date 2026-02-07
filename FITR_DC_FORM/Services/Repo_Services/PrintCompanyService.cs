using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Services.Interfaces;

namespace FITR_DC_FORM.Services.Repo_Services
{
    public class PrintCompanyService : IPrintCompanyService
    {
        private readonly IPrintCompanyRepository _repo;

        public PrintCompanyService(IPrintCompanyRepository repo)
        {
            _repo = repo;
        }

        public List<PrintCompanyModel> GetAll() => _repo.GetAll();
        public PrintCompanyModel GetById(int id) => _repo.GetById(id);
        public bool Save(PrintCompanyModel model) => _repo.Save(model);
        public bool Delete(int id) => _repo.Delete(id);
    }

}
