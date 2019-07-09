using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Const;
using CS.Data.DataContext;
using CS.Data.Model;

namespace CS.Data.Business
{
   public class CommonBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();


        public List<Branch> GetAllCenter(int roleid, int centerid)
        {
            try
            {
                if (roleid != SystemMessageConst.Role.Admin)
                {
                    var result = db.Branch.Where(ob => ob.is_active == 1 && (ob.Id == centerid || roleid == SystemMessageConst.Role.Admin)).ToList();
                    return result;
                }
                else
                {
                    var result = db.Branch.Where(ob => ob.is_active == 1).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int GetLevelByIdRole(int role)
        {
            try
            {
                var result = db.Role.FirstOrDefault(ob => ob.Id == role);
                return result.Level.Value;
            }
            catch (Exception e)
            {
                return 99;
            }
        }
        public List<User> GetAllAccountParentByCenter(int centerid, int level)
        {
            try
            {
                var _centerid = new SqlParameter
                {
                    ParameterName = "centerid",
                    Value = centerid
                };
                var _level = new SqlParameter
                {
                    ParameterName = "level",
                    Value = level
                };
                var result = db.Database.SqlQuery<User>(
                    "[Common_GetListAccountParentByCenter] @centerid,@level", _centerid,
                    _level).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Role> GetAllRoleByLevel(int level)
        {
            try
            {
                var result = db.Role.Where(ob => ((level == 12) ? true : ob.Level > level) && ob.IsActive == true && ob.Level != 12).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int GetLevelMaxByIdAcc(int idAcc)
        {
            try
            {
                var _idAcc = new SqlParameter
                {
                    ParameterName = "idAcc",
                    Value = idAcc
                };
                var maxlevel = new SqlParameter
                {
                    ParameterName = "maxlevel",
                    Value = 0,
                    Direction = ParameterDirection.Output
                };
                db.Database.ExecuteSqlCommand(
                    "[Common_GetMaxlevelByAccount] @idAcc,@maxlevel out", _idAcc,
                    maxlevel);
                return Int32.Parse(maxlevel.Value.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public List<Branch> GetAllBranchsByRoleIdLogin(int userid, int roleid, int BranchId)
        {
            try
            {
                var checkrole = db.Role.FirstOrDefault(m => m.Id == roleid);
                var checkBrandch = db.Branch.FirstOrDefault(m => m.Id == BranchId);

                if (checkrole.Level == 12 || checkBrandch.IsParent == true)
                {
                    var result = db.Branch.Where(m => m.is_active.Value == 1).ToList();
                    return result;
                }
                else
                {
                    var result = db.Branch.Where(m => m.Id == BranchId && m.is_active.Value == 1).ToList();
                    return result;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<sys_parameter> GetListParametersByKey(string key)
        {
            try
            {
                var result = db.sys_parameter.Where(ob => ob.code == key && ob.deleted == 0).OrderBy(ob => ob.order_display).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<LevelRole> GetAllRoleLevel()
        {
            try
            {
                var result = db.LevelRole.Where(m => m.IsActive == true && m.Id != 12).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Role> GelAllRoleChildrenByIdAdmin(string userid, string roleid, string branchId)
        {
            try
            {
                int userID = 0, roleId = 0, branchID = 0;

                int.TryParse(userid, out userID);
                int.TryParse(roleid, out roleId);
                int.TryParse(branchId, out branchID);

                var checkbrand = db.Branch.FirstOrDefault(c => c.Id == branchID);
                var check = db.Role.FirstOrDefault(m => m.Id == roleId);

                if (checkbrand.IsParent == false)
                {
                    var result = db.Role.Where(n => (check.Level == 12) ? (n.IsActive == true) : (n.Level >= check.Level && n.Level != 12 && n.IsActive == true)).OrderBy(n => n.Level).ToList();
                    return result;
                }
                else
                {
                    var result = db.Role.Where(n => n.IsActive == true && n.Level != 11 && n.Level != 12).OrderBy(n => n.Level).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Thongso_kt> CM_LoadAll_DS_thongso_theoid_sp(int id)
        {
            try
            {
                var _id = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var result = db.Database.SqlQuery<Thongso_kt>(
                    "[CM_LoadAll_DS_thongso_theoid_sp] @id", _id).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<paramter> CM_LoadAll_DS_Price_theoid_sp()
        {
            try
            {
               
                var result = db.Database.SqlQuery<paramter>(
                    "[CM_LoadAll_DS_Price_theoid_sp]").ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Thongso_kt> CM_DS_thongso_theoid_sp(int id)
        {
            try
            {
                var _id = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var result = db.Database.SqlQuery<Thongso_kt>(
                    "[CM_DS_thongso_theoid_sp] @id", _id).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<paramter> CM_DS_Gia_SP(int id)
        {
            try
            {
                var _id = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var result = db.Database.SqlQuery<paramter>(
                    "[CM_DS_Gia_SP] @id", _id).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<paramter> CM_DS_Gia_SP_TheoID(int id)
        {
            try
            {
                var _id = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var result = db.Database.SqlQuery<paramter>(
                    "[DS_Gia_SP_TheoID] @id", _id).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<tbl_Unit> CM_DS_Unit()
        {
            try
            {
                var result = db.tbl_Unit.Where(m => m.isactive == true).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<sys_product> CM_DS_SP()
        {
            try
            {
                var result = db.sys_product.Where(m => m.isactive == true).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<sys_nguonnhap> CM_DS_NguonNhap(int id_center)
        {
            try
            {
                var result = db.sys_nguonnhap.Where(m => m.isactive == true && m.id_center == id_center).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Branch> CM_DS_ChiNhanh(int id_center)
        {
            try
            {
                var result = db.Branch.Where(m => m.is_active == 1 && m.Id != id_center).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ds_sp_theo_center> CM_DS_SP_TheoDaiLy(int id_center)
        {
            try
            {
                var _id_center = new SqlParameter
                {
                    ParameterName = "id_center",
                    Value = id_center
                };
                var result = db.Database.SqlQuery<ds_sp_theo_center>(
                    "CM_DS_SP_TheoDaiLy @id_center", _id_center).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ds_sp_theo_center> CM_DS_Nhom_SP_TonKho_TheoID(int id_center,int id_p)
        {
            try
            {
                var _id_center = new SqlParameter
                {
                    ParameterName = "id_center",
                    Value = id_center
                };
                var _id_p = new SqlParameter
                {
                    ParameterName = "id_p",
                    Value = id_p
                };
                var result = db.Database.SqlQuery<ds_sp_theo_center>(
                    "CM_DS_Nhom_SP_TonKho_TheoID @id_center,@id_p", _id_center,_id_p).ToList();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public unitproduct CM_LoadUnit_ByID_ProductCallBack( int id_p)
        {
            try
            {
                var _id_p = new SqlParameter
                {
                    ParameterName = "id_p",
                    Value = id_p
                };
                var result = db.Database.SqlQuery<unitproduct>(
                    "CM_LoadUnit_ByID_ProductCallBack @id_p", _id_p).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
