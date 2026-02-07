using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        LoggedInUser Login(string userName, string password);
    }
}
