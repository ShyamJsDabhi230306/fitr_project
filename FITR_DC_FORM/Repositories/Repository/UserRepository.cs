using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int Save(UserMaster model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_FITR_User_Save", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", model.UserId);
            cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
            cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
            cmd.Parameters.AddWithValue("@FullName", model.FullName);
            cmd.Parameters.AddWithValue("@UserName", model.UserName);
            cmd.Parameters.AddWithValue("@Password", model.Password);
            cmd.Parameters.AddWithValue("@UserRole", model.UserRole ?? "USER");
            cmd.Parameters.AddWithValue("@SignaturePath", (object?)model.SignaturePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SignedOn", (object?)model.SignedOn ?? DBNull.Value);

            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<UserMaster> GetAll(int companyId = 0, int locationId = 0)
        {
            var list = new List<UserMaster>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_FITR_User_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", 0);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@LocationId", locationId);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(Map(dr));
            }

            return list;
        }

        public UserMaster GetById(int userId)
        {
            UserMaster model = null;

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_FITR_User_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CompanyId", 0);
            cmd.Parameters.AddWithValue("@LocationId", 0);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                model = Map(dr);
            }

            return model;
        }

        public bool Delete(int userId)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_FITR_User_Delete", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private UserMaster Map(SqlDataReader dr)
        {
            return new UserMaster
            {
                UserId = Convert.ToInt32(dr["UserId"]),
                CompanyId = Convert.ToInt32(dr["CompanyId"]),
                CompanyName = dr["CompanyName"]?.ToString(),
                LocationId = Convert.ToInt32(dr["LocationId"]),
                LocationName = dr["LocationName"]?.ToString(),
                FullName = dr["FullName"].ToString(),
                UserName = dr["UserName"].ToString(),
                Password = dr["Password"].ToString(),
                UserRole = dr["UserRole"].ToString(),
                SignaturePath = dr["SignaturePath"]?.ToString(),
                SignedOn = dr["SignedOn"] == DBNull.Value ? null : Convert.ToDateTime(dr["SignedOn"]),
                CreatedOn = Convert.ToDateTime(dr["CreatedOn"])
            };
        }

        public List<LocationMaster> GetByCompany(int companyId)
        {
            var list = new List<LocationMaster>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_FITR_Location_GetByCompany", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = companyId;

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new LocationMaster
                {
                    LocationId = Convert.ToInt32(dr["LocationId"]),
                    LocationName = dr["LocationName"].ToString()
                });
            }

            return list;
        }
    }
}
