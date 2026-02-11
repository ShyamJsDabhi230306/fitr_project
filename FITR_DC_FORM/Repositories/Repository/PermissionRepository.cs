using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace FITR_DC_FORM.Repositories.Repository
{
        public class PermissionRepository : IPermissionRepository
        {
            private readonly string _connectionString;

            public PermissionRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public UserPageRight GetByUserAndPage(int userId, string pageName)
            {
                using SqlConnection con = new(_connectionString);

                string query = @"SELECT * 
                             FROM UserPageRights
                             WHERE UserId = @UserId
                             AND PageName = @PageName";

                SqlCommand cmd = new(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@PageName", pageName);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new UserPageRight
                    {
                        RightId = Convert.ToInt32(reader["RightId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        PageName = reader["PageName"].ToString(),
                        CanView = Convert.ToBoolean(reader["CanView"]),
                        CanCreate = Convert.ToBoolean(reader["CanCreate"]),
                        CanEdit = Convert.ToBoolean(reader["CanEdit"]),
                        CanDelete = Convert.ToBoolean(reader["CanDelete"])
                    };
                }

                return null;
            }
        }
    
}
