using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class FitrVisualMasterRepository : IFitrVisualMasterRepository
    {
        private readonly IConfiguration _config;

        public FitrVisualMasterRepository(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public List<FitrVisualMaster> GetAll()
        {
            var list = new List<FitrVisualMaster>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_FITR_VisualMaster_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new FitrVisualMaster
                {
                    VisualMasterId = (int)rdr["VisualMasterId"],
                    VisualItemName = rdr["VisualItemName"].ToString(),
                    CreatedDate = (DateTime)rdr["CreatedDate"]
                });
            }

            return list;
        }

        public FitrVisualMaster GetById(int id)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_FITR_VisualMaster_Get", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VisualMasterId", id);

            con.Open();
            using var rdr = cmd.ExecuteReader();

            if (!rdr.Read()) return null;

            return new FitrVisualMaster
            {
                VisualMasterId = (int)rdr["VisualMasterId"],
                VisualItemName = rdr["VisualItemName"].ToString(),
                CreatedDate = (DateTime)rdr["CreatedDate"]
            };
        }

        public void Save(FitrVisualMaster model)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("sp_FITR_VisualMaster_Save", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VisualMasterId", model.VisualMasterId);
            cmd.Parameters.AddWithValue("@VisualItemName", model.VisualItemName);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("sp_FITR_VisualMaster_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VisualMasterId", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }

}
