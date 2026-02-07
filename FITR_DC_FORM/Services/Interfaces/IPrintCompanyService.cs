using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IPrintCompanyService
    {
        List<PrintCompanyModel> GetAll();
        PrintCompanyModel GetById(int id);
        bool Save(PrintCompanyModel model);
        bool Delete(int id);
    }
}
