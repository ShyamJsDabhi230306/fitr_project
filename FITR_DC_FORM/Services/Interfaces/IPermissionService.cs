namespace FITR_DC_FORM.Services.Interfaces
{
    public interface IPermissionService
    {
        bool CanView(int userId, string pageName);
        bool CanCreate(int userId, string pageName);
        bool CanEdit(int userId, string pageName);
        bool CanDelete(int userId, string pageName);
    }
}
