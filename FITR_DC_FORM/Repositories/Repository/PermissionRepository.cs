using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

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

            // ✅ USE STORED PROCEDURE (Correct way)
            SqlCommand cmd = new("sp_GetPageRights", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@PageName", pageName);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

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
    }
    
}
