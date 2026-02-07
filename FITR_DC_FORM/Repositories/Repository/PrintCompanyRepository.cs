using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class PrintCompanyRepository : IPrintCompanyRepository
    {
        private readonly string _con;

        public PrintCompanyRepository(IConfiguration configuration)
        {
            _con = configuration.GetConnectionString("DefaultConnection");
        }

        public List<PrintCompanyModel> GetAll()
        {
            List<PrintCompanyModel> list = new();

            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetPrintCompanyList", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new PrintCompanyModel
                {
                    PrintCompanyId = (int)dr["PrintCompanyId"],
                    CompanyName = dr["CompanyName"].ToString(),
                    CompanyAddress = dr["CompanyAddress"].ToString(),
                    CompanyLogo = dr["CompanyLogo"].ToString(),
                    IsActive = (bool)dr["IsActive"]
                });
            }
            return list;
        }

        public PrintCompanyModel GetById(int id)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_GetPrintCompanyById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PrintCompanyId", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                return new PrintCompanyModel
                {
                    PrintCompanyId = (int)dr["PrintCompanyId"],
                    CompanyName = dr["CompanyName"].ToString(),
                    CompanyAddress = dr["CompanyAddress"].ToString(),
                    CompanyLogo = dr["CompanyLogo"].ToString(),
                    IsActive = (bool)dr["IsActive"]
                };
            }
            return null;
        }

        public bool Save(PrintCompanyModel model)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_SavePrintCompany", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PrintCompanyId", model.PrintCompanyId);
            cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
            cmd.Parameters.AddWithValue("@CompanyAddress", model.CompanyAddress ?? "");
            cmd.Parameters.AddWithValue("@CompanyLogo", model.CompanyLogo ?? "");
            cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

            con.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using SqlConnection con = new(_con);
            using SqlCommand cmd = new("sp_DeletePrintCompany", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PrintCompanyId", id);

            con.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
