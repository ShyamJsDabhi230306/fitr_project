using FITR_DC_FORM.Models;

namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IAuthService
    {
        LoggedInUser Login(string userName, string password);
    }
}
