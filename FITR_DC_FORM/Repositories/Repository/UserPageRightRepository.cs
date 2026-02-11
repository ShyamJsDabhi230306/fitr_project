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

        public List<UserPageRight> GetAll()
        {
            List<UserPageRight> list = new();

            using SqlConnection con = new(_connectionString);
            SqlCommand cmd = new("SELECT * FROM UserPageRights", con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapRights(reader));
            }

            return list;
        }

        public List<UserPageRight> GetByUser(int userId)
        {
            List<UserPageRight> list = new();

            using SqlConnection con = new(_connectionString);
            SqlCommand cmd = new("SELECT * FROM UserPageRights WHERE UserId=@UserId", con);

            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapRights(reader));
            }

            return list;
        }

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

        public void Update(UserPageRight model)
        {
            using SqlConnection con = new(_connectionString);

            string query = @"UPDATE UserPageRights
                             SET CanView=@CanView,
                                 CanCreate=@CanCreate,
                                 CanEdit=@CanEdit,
                                 CanDelete=@CanDelete
                             WHERE RightId=@RightId";

            SqlCommand cmd = new(query, con);

            cmd.Parameters.AddWithValue("@RightId", model.RightId);
            cmd.Parameters.AddWithValue("@CanView", model.CanView);
            cmd.Parameters.AddWithValue("@CanCreate", model.CanCreate);
            cmd.Parameters.AddWithValue("@CanEdit", model.CanEdit);
            cmd.Parameters.AddWithValue("@CanDelete", model.CanDelete);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void BulkUpdate(List<UserPageRight> rights)
        {
            if (rights == null || !rights.Any()) return;

            using SqlConnection con = new(_connectionString);
            con.Open();
            using SqlTransaction transaction = con.BeginTransaction();

            string query = @"UPDATE UserPageRights
                             SET CanView=@CanView,
                                 CanCreate=@CanCreate,
                                 CanEdit=@CanEdit,
                                 CanDelete=@CanDelete
                             WHERE RightId=@RightId";

            try
            {
                foreach (var model in rights)
                {
                    using SqlCommand cmd = new(query, con, transaction);
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

        private UserPageRight MapRights(SqlDataReader reader)
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
    }
}
