using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class UserPageRightRepository : IUserPageRightRepository
    {
        private readonly string _connectionString;

        public UserPageRightRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ Assign all PageMaster pages to new user
        public void AssignDefaultRights(int userId)
        {
            using SqlConnection con = new(_connectionString);

            SqlCommand cmd = new("sp_AssignDefaultRightsToUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // ✅ Get all rights for selected user
        public List<UserPageRight> GetByUser(int userId)
        {
            List<UserPageRight> list = new();

            using SqlConnection con = new(_connectionString);

            SqlCommand cmd = new("sp_GetRightsByUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new UserPageRight
                {
                    RightId = Convert.ToInt32(reader["RightId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    PageId = Convert.ToInt32(reader["PageId"]),
                    PageName = reader["PageName"].ToString(),
                    CanView = Convert.ToBoolean(reader["CanView"]),
                    CanCreate = Convert.ToBoolean(reader["CanCreate"]),
                    CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                    CanDelete = Convert.ToBoolean(reader["CanDelete"])
                });
            }

            return list;
        }

        // ✅ Get permission for specific page (Used in PermissionService)
        public UserPageRight GetByUserAndPage(int userId, string pageName)
        {
            using SqlConnection con = new(_connectionString);

            SqlCommand cmd = new("sp_GetPageRights", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@PageName", pageName);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UserPageRight
                {
                    UserId = userId,
                    PageName = pageName,
                    CanView = Convert.ToBoolean(reader["CanView"]),
                    CanCreate = Convert.ToBoolean(reader["CanCreate"]),
                    CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                    CanDelete = Convert.ToBoolean(reader["CanDelete"])
                };
            }

            return null;
        }

        // ✅ Bulk update rights (Transaction safe)
        public void BulkUpdate(List<UserPageRight> rights)
        {
            if (rights == null || !rights.Any())
                return;

            using SqlConnection con = new(_connectionString);
            con.Open();

            using SqlTransaction transaction = con.BeginTransaction();

            try
            {
                foreach (var model in rights)
                {
                    SqlCommand cmd = new("sp_UpdateUserPageRights", con, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RightId", model.RightId);
                    cmd.Parameters.AddWithValue("@CanView", model.CanView);
                    cmd.Parameters.AddWithValue("@CanCreate", model.CanCreate);
                    cmd.Parameters.AddWithValue("@CanEdit", model.CanEdit);
                    cmd.Parameters.AddWithValue("@CanDelete", model.CanDelete);

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
