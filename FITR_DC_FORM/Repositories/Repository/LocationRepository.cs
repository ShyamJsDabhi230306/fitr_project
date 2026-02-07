using FITR_DC_FORM.Models;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly string _cs;

        public LocationRepository( IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }
        public List<LocationMaster> GetAll()
        {
            var list = new List<LocationMaster>();

            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("usp_FITR_Location_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", 0);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new LocationMaster
                {
                    LocationId = Convert.ToInt32(dr["LocationId"]),
                    CompanyId = Convert.ToInt32(dr["CompanyId"]),
                    CompanyName = dr["CompanyName"].ToString(),
                    LocationName = dr["LocationName"].ToString(),
                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"])
                });
            }

            return list;
        }

        public LocationMaster GetById(int id)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("usp_FITR_Location_Get", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", id);

            con.Open();
            using SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            return new LocationMaster
            {
                LocationId = Convert.ToInt32(dr["LocationId"]),
                CompanyId = Convert.ToInt32(dr["CompanyId"]),
                CompanyName = dr["CompanyName"].ToString(),
                LocationName = dr["LocationName"].ToString(),
                CreatedOn = Convert.ToDateTime(dr["CreatedOn"])
            };
        }

        public void Save(LocationMaster model)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("usp_FITR_Location_Save", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", model.LocationId);
            cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
            cmd.Parameters.AddWithValue("@LocationName", model.LocationName);
            cmd.Parameters.AddWithValue("@IsDelete", 0);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void SoftDelete(int id)
        {
            using SqlConnection con = new SqlConnection(_cs);
            using SqlCommand cmd = new SqlCommand("usp_FITR_Location_Save", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LocationId", id);
            cmd.Parameters.AddWithValue("@IsDelete", 1);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        public List<LocationMaster> GetByCompany(int companyId)
        {
            var list = new List<LocationMaster>();

            using SqlConnection con = new SqlConnection(_cs);
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
