using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _conStr;

        public AuthRepository(IConfiguration config)
        {
            _conStr = config.GetConnectionString("DefaultConnection");
        }

        public LoggedInUser Login(string userName, string password)
        {
            using SqlConnection con = new SqlConnection(_conStr);
            using SqlCommand cmd = new SqlCommand("usp_FITR_User_Login", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Password", password);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                return new LoggedInUser
                {
                    UserId = Convert.ToInt32(dr["UserId"]),
                    FullName = dr["FullName"].ToString(),
                    UserName = userName,
                    UserRole = dr["UserRole"].ToString(),

                    CompanyId = Convert.ToInt32(dr["CompanyId"]),
                    CompanyName = dr["CompanyName"].ToString(),

                    LocationId = Convert.ToInt32(dr["LocationId"]),
                    LocationName = dr["LocationName"].ToString(),

                    SignaturePath = dr["SignaturePath"]?.ToString()
                };
            }

            return null;
        }
    }
}
