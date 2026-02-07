using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int Save(CompanyMaster model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_FITR_Company_Save", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
                cmd.Parameters.AddWithValue("@Address", model.Address ?? "");
                cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo ?? "");
                cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
                cmd.Parameters.AddWithValue("@PanNo", model.PanNo ?? "");
                cmd.Parameters.AddWithValue("@GSTNo", model.GSTNo ?? "");

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<CompanyMaster> GetAll()
        {
            var list = new List<CompanyMaster>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_FITR_Company_Get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", 0);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(MapCompany(dr));
                    }
                }
            }

            return list;
        }

        public CompanyMaster GetById(int companyId)
        {
            CompanyMaster model = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_FITR_Company_Get", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", companyId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = MapCompany(dr);
                    }
                }
            }

            return model;
        }

        public bool Delete(int companyId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_FITR_Company_Delete", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", companyId);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private CompanyMaster MapCompany(SqlDataReader dr)
        {
            return new CompanyMaster
            {
                CompanyId = Convert.ToInt32(dr["CompanyId"]),
                CompanyName = dr["CompanyName"].ToString(),
                Address = dr["Address"].ToString(),
                ContactNo = dr["ContactNo"].ToString(),
                Email = dr["Email"].ToString(),
                PanNo = dr["PanNo"].ToString(),
                GSTNo = dr["GSTNo"].ToString(),
                CreatedOn = Convert.ToDateTime(dr["CreatedOn"])
            };
        }
    }
}
