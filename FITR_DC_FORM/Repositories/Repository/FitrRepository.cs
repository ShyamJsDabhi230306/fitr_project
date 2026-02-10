


using FITR_DC_FORM.Models;
using FITR_DC_FORM.Models.ViewModels;
using FITR_DC_FORM.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Data;
using System.Reflection.PortableExecutable;

namespace FITR_DC_FORM.Repositories.Repository
{
    public class FitrRepository : IFitrRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FitrRepository> _logger;

        public FitrRepository(IConfiguration config, ILogger<FitrRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        private SqlConnection GetConnection()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("DefaultConnection not found.");

            return new SqlConnection(connStr);
        }

        /* =====================================================
           SAVE METHODS (UNCHANGED / SAFE)
           ===================================================== */

        public int SaveBasic(FitrMaster m)
{
    using var con = GetConnection();
    using var cmd = new SqlCommand("dbo.sp_FITR_SaveBasic", con);
    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.Add("@FitrId", SqlDbType.Int)
        .Value = m.FitrId == 0 ? (object)DBNull.Value : m.FitrId;

    cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = m.CompanyId;
    cmd.Parameters.Add("@LocationId", SqlDbType.Int).Value = m.LocationId;
    cmd.Parameters.Add("@CreatedByUserId", SqlDbType.Int).Value = m.CreatedByUserId;

    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 200).Value = m.CompanyName ?? "";
    cmd.Parameters.Add("@DCNo", SqlDbType.NVarChar, 50).Value = m.DCNo ?? "";
    cmd.Parameters.Add("@DCDate", SqlDbType.Date)
        .Value = m.DCDate ?? (object)DBNull.Value;

    cmd.Parameters.Add("@PONo", SqlDbType.NVarChar, 50)
        .Value = m.PONo ?? (object)DBNull.Value;
    cmd.Parameters.Add("@PODate", SqlDbType.Date)
        .Value = m.PODate ?? (object)DBNull.Value;
    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = m.Quantity ?? 0;

    cmd.Parameters.Add("@DCAttachmentPath", SqlDbType.NVarChar, 500)
        .Value = m.DCAttachmentPath ?? "";
    cmd.Parameters.Add("@POAttachmentPath", SqlDbType.NVarChar, 500)
        .Value = m.POAttachmentPath ?? "";
    cmd.Parameters.Add("@DrawingAttachmentPath", SqlDbType.NVarChar, 500)
        .Value = m.DrawingAttachmentPath ?? (object)DBNull.Value;
    cmd.Parameters.Add("@TCAttachmentPath", SqlDbType.NVarChar, 500)
        .Value = m.TCAttachmentPath ?? (object)DBNull.Value;

    con.Open();
    return Convert.ToInt32(cmd.ExecuteScalar());
}


        public void SaveProduct(FitrMaster m)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.sp_FITR_SaveProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FitrId", m.FitrId);
            cmd.Parameters.AddWithValue("@ProductType", m.ProductType ?? "");
            cmd.Parameters.AddWithValue("@Torque", m.Torque ?? "");
            cmd.Parameters.AddWithValue("@Ratio", m.Ratio ?? "");
            cmd.Parameters.AddWithValue("@Model", m.Model ?? "");
            cmd.Parameters.AddWithValue("@HandwheelDia", m.HandwheelDia ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void SaveMaterials(int fitrId, List<FitrMaterial> materials)
        {
            using var con = GetConnection();
            con.Open();

            new SqlCommand("DELETE FROM FITR_Material WHERE FitrId=" + fitrId, con).ExecuteNonQuery();

            foreach (var m in materials)
            {
                using var cmd = new SqlCommand("dbo.sp_FITR_SaveMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FitrId", fitrId);
                cmd.Parameters.AddWithValue("@Component", m.Component ?? "");
                cmd.Parameters.AddWithValue("@Material", m.Material ?? "");
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveSrData(int fitrId, List<FitrSrData> srData)
        {
            using var con = GetConnection();
            con.Open();

            new SqlCommand("DELETE FROM FITR_SrData WHERE FitrId=" + fitrId, con).ExecuteNonQuery();

            foreach (var s in srData)
            {
                using var cmd = new SqlCommand("dbo.sp_FITR_SaveSrData", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FitrId", fitrId);
                cmd.Parameters.AddWithValue("@SrNo", s.SrNo);
                cmd.Parameters.AddWithValue("@ShaftDia", s.ShaftDia ?? "");
                cmd.Parameters.AddWithValue("@OutputDrive", s.OutputDrive ?? "");
                cmd.Parameters.AddWithValue("@ActuatorPCD", s.ActuatorPCD ?? "");
                cmd.Parameters.AddWithValue("@ValvePCD", s.ValvePCD ?? "");
                cmd.Parameters.AddWithValue("@SquareDia", s.SquareDia ?? "");
                cmd.Parameters.AddWithValue("@SquarePosition", s.SquarePosition ?? "");

                cmd.ExecuteNonQuery();
            }
        }

        public void SaveVisuals(int fitrId, List<FitrVisual> visuals)
        {
            using var con = GetConnection();
            con.Open();

            new SqlCommand("DELETE FROM FITR_Visual WHERE FitrId=" + fitrId, con).ExecuteNonQuery();

            foreach (var v in visuals)
            {
                using var cmd = new SqlCommand("sp_FITR_SaveVisual", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FitrId", fitrId);
                cmd.Parameters.AddWithValue("@VisualMasterId", v.VisualMasterId);
                cmd.Parameters.AddWithValue("@ProductValue", v.ProductValue ?? "");

                cmd.ExecuteNonQuery();
            }
        }

        public void SaveFinalRemark(int fitrId, string remarks)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.sp_FITR_SaveFinalRemark", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FitrId", fitrId);
            cmd.Parameters.AddWithValue("@FinalRemarks", remarks ?? "");

            con.Open();
            cmd.ExecuteNonQuery();
        }

        /* =====================================================
           🔥 MAIN LIST METHOD (SR DATA FIXED)
           ===================================================== */

        public List<FitrMaster> GetListByRole(
            string userRole,
            int companyId,
            int locationId,
            int userId)
        {
            var list = new List<FitrMaster>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.sp_FITR_GetDcList_ByRole", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserRole", userRole);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@LocationId", locationId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            con.Open();
            using var rdr = cmd.ExecuteReader();

            // 1️⃣ MASTER
            while (rdr.Read())
            {
                list.Add(new FitrMaster
                {
                    FitrId = (int)rdr["FitrId"],
                    TCNo = rdr["TCNo"] as int?,
                    TCYear = rdr["TCYear"]?.ToString(),
                    TCDate = rdr["TCDate"] as DateTime?,
                    CompanyName = rdr["CompanyName"]?.ToString(),
                    PONo = rdr["PONo"]?.ToString(),
                    DCNo = rdr["DCNo"]?.ToString(),
                    DCDate = rdr["DCDate"] as DateTime?,
                    Status = rdr.GetSchemaTable().Select("ColumnName = 'DisplayStatus'").Length > 0 
                             ? rdr["DisplayStatus"]?.ToString() 
                             : rdr["Status"]?.ToString(),
                    ProductType = rdr["ProductType"]?.ToString(),
                    Model = rdr["Model"]?.ToString(),
                    CreatedAt = rdr["CreatedAt"] as DateTime?,
                    CreatedByUserId = rdr.GetSchemaTable().Select("ColumnName = 'CreatedByUserId'").Length > 0 
                                      ? rdr["CreatedByUserId"] as int? 
                                      : null,
                    POAttachmentPath = rdr["POAttachmentPath"] == DBNull.Value
                                          ? null
                                          : rdr["POAttachmentPath"].ToString(),

                    // 🌍 MAPPING FOR IN-MEMORY FILTERING
                    CompanyId = rdr.GetSchemaTable().Select("ColumnName = 'CompanyId'").Length > 0 
                                ? rdr["CompanyId"] as int? 
                                : null,
                    LocationId = rdr.GetSchemaTable().Select("ColumnName = 'LocationId'").Length > 0 
                                 ? rdr["LocationId"] as int? 
                                 : null,

                    DCAttachmentPath = rdr["DCAttachmentPath"] == DBNull.Value
                                          ? null
                                          : rdr["DCAttachmentPath"].ToString(),

                    DrawingAttachmentPath = rdr["DrawingAttachmentPath"] == DBNull.Value
                                          ? null
                                          : rdr["DrawingAttachmentPath"].ToString(),

                    TCAttachmentPath = rdr["TCAttachmentPath"] == DBNull.Value
                                          ? null
                                          : rdr["TCAttachmentPath"].ToString(),



                    SrData = new List<FitrSrData>(),
                    Materials = new List<FitrMaterial>()
                });
            }

            // 2️⃣ MATERIAL
            rdr.NextResult();
            var materialMap = new Dictionary<int, List<FitrMaterial>>();

            while (rdr.Read())
            {
                int id = (int)rdr["FitrId"];
                if (!materialMap.ContainsKey(id))
                    materialMap[id] = new List<FitrMaterial>();

                materialMap[id].Add(new FitrMaterial
                {
                    MaterialId = (int)rdr["MaterialId"],
                    FitrId = id,
                    Component = rdr["Component"]?.ToString(),
                    Material = rdr["Material"]?.ToString()
                });
            }

            // 3️⃣ SR DATA ✅
            rdr.NextResult();
            var srMap = new Dictionary<int, List<FitrSrData>>();

            while (rdr.Read())
            {
                int id = (int)rdr["FitrId"];
                if (!srMap.ContainsKey(id))
                    srMap[id] = new List<FitrSrData>();

                srMap[id].Add(new FitrSrData
                {
                    SrId = (int)rdr["SrId"],
                    FitrId = id,
                    SrNo = (int)rdr["SrNo"],
                    ShaftDia = rdr["ShaftDia"]?.ToString(),
                    OutputDrive = rdr["OutputDrive"]?.ToString(),
                    ActuatorPCD = rdr["ActuatorPCD"]?.ToString(),
                    ValvePCD = rdr["ValvePCD"]?.ToString(),
                    SquareDia = rdr["SquareDia"]?.ToString(),
                    SquarePosition = rdr["SquarePosition"]?.ToString()
                });
            }

            // 4️⃣ MAP
            foreach (var dc in list)
            {
                if (materialMap.ContainsKey(dc.FitrId))
                    dc.Materials = materialMap[dc.FitrId];

                if (srMap.ContainsKey(dc.FitrId))
                    dc.SrData = srMap[dc.FitrId];
            }

            return list.OrderByDescending(x => x.CreatedAt).ToList();
        }



        /* =====================================================
           STATUS FLOW
           ===================================================== */

        public void SubmitByUser(int fitrId, int userId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_FITR_SubmitByUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FitrId", fitrId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ApproveByHod(int fitrId, int hodUserId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_FITR_ApproveByHOD", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FitrId", fitrId);
            cmd.Parameters.AddWithValue("@HodUserId", hodUserId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void ApproveByAdmin(int fitrId, int adminUserId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("usp_FITR_ApproveByAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FitrId", fitrId);
            cmd.Parameters.AddWithValue("@AdminUserId", adminUserId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public bool IsDcDuplicate(string dcNo, DateTime dcDate, int fitrId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM FITR_Master WHERE DCNo=@DCNo AND DCDate=@DCDate AND FitrId<>@FitrId", con);

            cmd.Parameters.AddWithValue("@DCNo", dcNo);
            cmd.Parameters.AddWithValue("@DCDate", dcDate);
            cmd.Parameters.AddWithValue("@FitrId", fitrId);

            con.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }

        public (int tcNo, string tcYear) GetNextTCPreview()
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_FITR_GetNextTCPreview", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            using var rdr = cmd.ExecuteReader();

            if (!rdr.Read())
                return (1, DateTime.Now.Year.ToString());

            return (
                Convert.ToInt32(rdr["NextTCNo"]),
                rdr["TCYear"].ToString()
            );
        }

        public void GenerateTCIfNotExists(int fitrId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_FITR_GenerateTC", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FitrId", fitrId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public List<FitrVisualMaster> GetAllVisualMaster()
        {
            var list = new List<FitrVisualMaster>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("SELECT VisualMasterId, VisualItemName FROM FITR_Visual_Master", con);

            con.Open();
            using var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                list.Add(new FitrVisualMaster
                {
                    VisualMasterId = (int)rdr["VisualMasterId"],
                    VisualItemName = rdr["VisualItemName"].ToString()
                });
            }

            return list;
        }

        //public FitrViewModel GetFitr(int? fitrId)
        //{
        //    if (fitrId == null)
        //        return null;

        //    var vm = new FitrViewModel
        //    {
        //        Master = new FitrMaster
        //        {
        //            Materials = new List<FitrMaterial>(),
        //            SrData = new List<FitrSrData>()
        //        }
        //    };

        //    using var con = GetConnection();
        //    using var cmd = new SqlCommand("dbo.usp_FITR_GetById", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@FitrId", fitrId.Value);

        //    con.Open();
        //    using var rdr = cmd.ExecuteReader();

        //    /* ======================
        //       1️⃣ MASTER
        //       ====================== */
        //    if (rdr.Read())
        //    {
        //        vm.Master.FitrId = (int)rdr["FitrId"];
        //        vm.Master.CompanyName = rdr["CompanyName"]?.ToString();
        //        vm.Master.DCNo = rdr["DCNo"]?.ToString();
        //        vm.Master.DCDate = rdr["DCDate"] as DateTime?;
        //        vm.Master.PONo = rdr["PONo"]?.ToString();
        //        vm.Master.PODate = rdr["PODate"] as DateTime?;
        //        vm.Master.Quantity = rdr["Quantity"] as int?;
        //        vm.Master.Status = rdr["Status"]?.ToString();
        //        vm.Master.ProductType = rdr["ProductType"]?.ToString();
        //        vm.Master.Torque = rdr["Torque"]?.ToString();
        //        vm.Master.Ratio = rdr["Ratio"]?.ToString();
        //        vm.Master.Model = rdr["Model"]?.ToString();
        //        vm.Master.HandwheelDia = rdr["HandwheelDia"]?.ToString();
        //        vm.Master.FinalRemarks = rdr["FinalRemarks"]?.ToString();

        //        vm.Master.TCNo = rdr["TCNo"] as int?;
        //        //vm.Master.TCYear = rdr["TCYear"] as int?;   // 🔥 IMPORTANT FOR PRINT
        //        vm.Master.TCDate = rdr["TCDate"] as DateTime?;

        //        vm.Master.CreatedAt = rdr["CreatedAt"] as DateTime?;
        //        vm.Master.UpdatedAt = rdr["UpdatedAt"] as DateTime?;
        //        //vm.Master.LastStepCompleted = rdr[""]?.ToString();

        //        vm.Master.DCAttachmentPath = rdr["DCAttachmentPath"]?.ToString();
        //        vm.Master.POAttachmentPath = rdr["POAttachmentPath"]?.ToString();
        //        vm.Master.DrawingAttachmentPath = rdr["DrawingAttachmentPath"]?.ToString();
        //        vm.Master.TCAttachmentPath = rdr["TCAttachmentPath"]?.ToString();



        //    }
        //    else
        //    {
        //        // No master = nothing to print
        //        return null;
        //    }

        //    /* ======================
        //       2️⃣ MATERIAL
        //       ====================== */
        //    rdr.NextResult();
        //    while (rdr.Read())
        //    {
        //        vm.Master.Materials.Add(new FitrMaterial
        //        {
        //            MaterialId = (int)rdr["MaterialId"],
        //            FitrId = (int)rdr["FitrId"],
        //            Component = rdr["Component"]?.ToString(),
        //            Material = rdr["Material"]?.ToString()
        //        });
        //    }

        //    rdr.NextResult();
        //    while (rdr.Read())
        //    {
        //        vm.Master.SrData.Add(new FitrSrData
        //        {
        //            SrId = (int)rdr["SrId"],
        //            FitrId = (int)rdr["FitrId"],
        //            SrNo = (int)rdr["SrNo"],
        //            ShaftDia = rdr["ShaftDia"]?.ToString(),
        //            OutputDrive = rdr["OutputDrive"]?.ToString(),
        //            ActuatorPCD = rdr["ActuatorPCD"]?.ToString(),
        //            ValvePCD = rdr["ValvePCD"]?.ToString(),
        //            SquareDia = rdr["SquareDia"]?.ToString(),
        //            SquarePosition = rdr["SquarePosition"]?.ToString()
        //        });
        //    }

        //    return vm;
        //}

        public FitrViewModel GetFitr(int? fitrId)
        {
            if (fitrId == null)
                return null;

            var vm = new FitrViewModel
            {
                Master = new FitrMaster
                {
                    Materials = new List<FitrMaterial>(),
                    SrData = new List<FitrSrData>()
                }
            };

            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.usp_FITR_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FitrId", fitrId.Value);

            con.Open();
            using var rdr = cmd.ExecuteReader();

            /* ======================
               1️⃣ MASTER
               ====================== */
            if (rdr.Read())
            {
                vm.Master.FitrId = (int)rdr["FitrId"];
                vm.Master.CompanyName = rdr["CompanyName"]?.ToString();

                vm.Master.DCNo = rdr["DCNo"]?.ToString();
                vm.Master.DCDate = rdr["DCDate"] as DateTime?;

                vm.Master.PONo = rdr["PONo"]?.ToString();
                vm.Master.PODate = rdr["PODate"] as DateTime?;

                vm.Master.Quantity = rdr["Quantity"] as int?;
                vm.Master.Status = rdr["Status"]?.ToString();

                vm.Master.ProductType = rdr["ProductType"]?.ToString();
                vm.Master.Torque = rdr["Torque"]?.ToString();
                vm.Master.Ratio = rdr["Ratio"]?.ToString();
                vm.Master.Model = rdr["Model"]?.ToString();
                vm.Master.HandwheelDia = rdr["HandwheelDia"]?.ToString();

                vm.Master.FinalRemarks = rdr["FinalRemarks"]?.ToString();

                vm.Master.TCNo = rdr["TCNo"] as int?;
                vm.Master.TCYear = rdr["TCYear"]?.ToString();
                vm.Master.TCDate = rdr["TCDate"] as DateTime?;

                vm.Master.CreatedAt = rdr["CreatedAt"] as DateTime?;
                vm.Master.UpdatedAt = rdr["UpdatedAt"] as DateTime?;

                vm.Master.DCAttachmentPath = rdr["DCAttachmentPath"]?.ToString();
                vm.Master.POAttachmentPath = rdr["POAttachmentPath"]?.ToString();
                vm.Master.DrawingAttachmentPath = rdr["DrawingAttachmentPath"]?.ToString();
                vm.Master.TCAttachmentPath = rdr["TCAttachmentPath"]?.ToString();

                /* ===== CREATED BY (ALWAYS PRESENT) ===== */
                /* ===== CREATED BY ===== */
                vm.Master.CreatedByName = rdr["CreatedByName"]?.ToString();
                vm.Master.CreatedBySignature = rdr["CreatedBySignature"] == DBNull.Value
                    ? null
                    : rdr["CreatedBySignature"].ToString();
                vm.Master.CreatedOn = rdr["CreatedOn"] as DateTime?;

                /* ===== HOD APPROVAL (NULL SAFE) ===== */
                vm.Master.HodApprovedByName = rdr["HodApprovedByName"] == DBNull.Value
                    ? null
                    : rdr["HodApprovedByName"].ToString();

                vm.Master.HodSignature = rdr["HodSignature"] == DBNull.Value
                    ? null
                    : rdr["HodSignature"].ToString();

                vm.Master.HodApprovedOn = rdr["HodApprovedOn"] as DateTime?;

                /* ===== ADMIN APPROVAL (NULL SAFE) ===== */
                vm.Master.AdminApprovedByName = rdr["AdminApprovedByName"] == DBNull.Value
                    ? null
                    : rdr["AdminApprovedByName"].ToString();

                vm.Master.AdminSignature = rdr["AdminSignature"] == DBNull.Value
                    ? null
                    : rdr["AdminSignature"].ToString();

                vm.Master.AdminApprovedOn = rdr["AdminApprovedOn"] as DateTime?;
            }
            else
            {
                // No master = nothing to print
                return null;
            }

            /* ======================
               2️⃣ MATERIAL
               ====================== */
            rdr.NextResult();
            while (rdr.Read())
            {
                vm.Master.Materials.Add(new FitrMaterial
                {
                    MaterialId = (int)rdr["MaterialId"],
                    FitrId = (int)rdr["FitrId"],
                    Component = rdr["Component"]?.ToString(),
                    Material = rdr["Material"]?.ToString()
                });
            }

            /* ======================
               3️⃣ SR DATA
               ====================== */
            rdr.NextResult();
            while (rdr.Read())
            {
                vm.Master.SrData.Add(new FitrSrData
                {
                    SrId = (int)rdr["SrId"],
                    FitrId = (int)rdr["FitrId"],
                    SrNo = (int)rdr["SrNo"],
                    ShaftDia = rdr["ShaftDia"]?.ToString(),
                    OutputDrive = rdr["OutputDrive"]?.ToString(),
                    ActuatorPCD = rdr["ActuatorPCD"]?.ToString(),
                    ValvePCD = rdr["ValvePCD"]?.ToString(),
                    SquareDia = rdr["SquareDia"]?.ToString(),
                    SquarePosition = rdr["SquarePosition"]?.ToString()
                });
            }

            return vm;
        }

        public bool IsDcNoExists(string dcNo, int? fitrId)
        {
            using var con = GetConnection();
            using var cmd = new SqlCommand(@"
            SELECT COUNT(1)
            FROM FITR_Master
            WHERE DCNo = @DCNo
              AND (@FitrId IS NULL OR FitrId <> @FitrId)
        ", con);

            cmd.Parameters.AddWithValue("@DCNo", dcNo);
            cmd.Parameters.AddWithValue("@FitrId", (object?)fitrId ?? DBNull.Value);

            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public List<FitrMaster> GetListByRoleFilteredV2(
            string userRole,
            int companyId,
            int locationId,
            string filter)
        {
            var list = new List<FitrMaster>();

            using var con = GetConnection();
            using var cmd = new SqlCommand("dbo.sp_FITR_List_ByRole_Filter_V2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserRole", userRole);
            cmd.Parameters.AddWithValue("@CompanyId", companyId);
            cmd.Parameters.AddWithValue("@LocationId", locationId);
            cmd.Parameters.AddWithValue("@Filter", filter);

            con.Open();
            using var rdr = cmd.ExecuteReader();

            // 1️⃣ MASTER
            while (rdr.Read())
            {
                list.Add(new FitrMaster
                {
                    FitrId = (int)rdr["FitrId"],
                    TCNo = rdr["TCNo"] as int?,
                    TCYear = rdr["TCYear"]?.ToString(),
                    TCDate = rdr["TCDate"] as DateTime?,
                    CompanyName = rdr["CompanyName"]?.ToString(),
                    PONo = rdr["PONo"]?.ToString(),
                    DCNo = rdr["DCNo"]?.ToString(),
                    DCDate = rdr["DCDate"] as DateTime?,
                    Status = rdr["DisplayStatus"]?.ToString(),
                    ProductType = rdr["ProductType"]?.ToString(),
                    Model = rdr["Model"]?.ToString(),
                    CreatedAt = rdr["CreatedAt"] as DateTime?,
                    CreatedByUserId = rdr.GetSchemaTable().Select("ColumnName = 'CreatedByUserId'").Length > 0 
                                      ? rdr["CreatedByUserId"] as int? 
                                      : null,

                    // 🌍 MAPPING FOR IN-MEMORY FILTERING
                    CompanyId = rdr.GetSchemaTable().Select("ColumnName = 'CompanyId'").Length > 0 
                                ? rdr["CompanyId"] as int? 
                                : null,
                    LocationId = rdr.GetSchemaTable().Select("ColumnName = 'LocationId'").Length > 0 
                                 ? rdr["LocationId"] as int? 
                                 : null,
                    
                    // Restore Missing Attachments 👇
                    POAttachmentPath = rdr["POAttachmentPath"]?.ToString(),
                    DCAttachmentPath = rdr["DCAttachmentPath"]?.ToString(),
                    DrawingAttachmentPath = rdr["DrawingAttachmentPath"]?.ToString(),
                    TCAttachmentPath = rdr["TCAttachmentPath"]?.ToString(),

                    SrData = new List<FitrSrData>(),
                    Materials = new List<FitrMaterial>()
                });
            }

            // 2️⃣ MATERIAL
            if (rdr.NextResult())
            {
                var materialMap = new Dictionary<int, List<FitrMaterial>>();
                while (rdr.Read())
                {
                    int id = (int)rdr["FitrId"];
                    if (!materialMap.ContainsKey(id))
                        materialMap[id] = new List<FitrMaterial>();

                    materialMap[id].Add(new FitrMaterial
                    {
                        MaterialId = (int)rdr["MaterialId"],
                        FitrId = id,
                        Component = rdr["Component"]?.ToString(),
                        Material = rdr["Material"]?.ToString()
                    });
                }
                foreach (var dc in list)
                {
                    if (materialMap.ContainsKey(dc.FitrId))
                        dc.Materials = materialMap[dc.FitrId];
                }
            }

            // 3️⃣ SR DATA
            if (rdr.NextResult())
            {
                var srMap = new Dictionary<int, List<FitrSrData>>();
                while (rdr.Read())
                {
                    int id = (int)rdr["FitrId"];
                    if (!srMap.ContainsKey(id))
                        srMap[id] = new List<FitrSrData>();

                    srMap[id].Add(new FitrSrData
                    {
                        SrId = (int)rdr["SrId"],
                        FitrId = id,
                        SrNo = (int)rdr["SrNo"],
                        ShaftDia = rdr["ShaftDia"]?.ToString(),
                        OutputDrive = rdr["OutputDrive"]?.ToString(),
                        ActuatorPCD = rdr["ActuatorPCD"]?.ToString(),
                        ValvePCD = rdr["ValvePCD"]?.ToString(),
                        SquareDia = rdr["SquareDia"]?.ToString(),
                        SquarePosition = rdr["SquarePosition"]?.ToString()
                    });
                }
                foreach (var dc in list)
                {
                    if (srMap.ContainsKey(dc.FitrId))
                        dc.SrData = srMap[dc.FitrId];
                }
            }

            return list.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }

}

