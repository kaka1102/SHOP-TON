using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.DataContext;
using CS.Data.Model;

namespace CS.Data.Business
{
    public class RoleBusiness
    {

        public ListRoleViewModel ListRoll(string searchValue, int currPage, int recodperpage)
        {
            ListRoleViewModel result = new ListRoleViewModel();
            List<CustomerRole> list = new List<CustomerRole>();

            var db = new DB_CSEntities1();


            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand("sp_role_getall", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
            cmd.Parameters.Add(new SqlParameter("@currPage", currPage));
            cmd.Parameters.Add(new SqlParameter("@recodperpage", 10));
            cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int total = Convert.ToInt16(cmd.Parameters["@totalCount"].Value);
            foreach (DataRow rowItem in dt.Rows)
            {
                CustomerRole item = new CustomerRole();
                item.Id = Int32.Parse(rowItem["Id"].ToString());
                item.RoleName = rowItem["RoleName"].ToString();
                item.IsActive = bool.Parse(rowItem["IsActive"].ToString());
                item.Level = Int32.Parse(rowItem["Level"].ToString());

                item.LevelName = rowItem["LevelName"].ToString();
                item.Description = rowItem["Description"].ToString();
                list.Add(item);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }
        public string ValidateRoleSystem(Role role)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(role.RoleName, "Tên quyền ", 1, 50);
            if (rs != null)
            {
                return rs;
            }

            if (role.Level == 0)
            {
                return rs = "Mời bạn chọn Role Level";
            }
            return null;
        }
        public SystemMessage AddRoleSystem(Role role)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var check = ValidateRoleSystem(role);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var db = new DB_CSEntities1();
                var checkisvail = db.Role.FirstOrDefault(m => m.RoleName == role.RoleName);

                if (checkisvail != null && checkisvail.IsActive == false)
                {
                    checkisvail.IsActive = true;
                    db.SaveChanges();

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Tên quyền đã có, khôi phục thành công thành công !";
                    return systemMessage;
                }
                else if (checkisvail != null && checkisvail.IsActive == true)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên quyền này đã tồn tại,vui lòng thực hiện lại !";
                    return systemMessage;
                }
                else
                {
                    Role item = new Role();
                    item.RoleName = role.RoleName;
                    item.IsActive = role.IsActive;
                    item.Level = role.Level;
                    db.Role.Add(item);
                    db.SaveChanges();

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                    return systemMessage;
                }

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }

        public Role GetRoleSystemById(int id)
        {
            try
            {
                var db = new DB_CSEntities1();
                var result = db.Role.FirstOrDefault(ob => ob.Id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public SystemMessage EditRoleSystem(Role role)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var check = ValidateRoleSystem(role);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var db = new DB_CSEntities1();

                var checkitem = db.Role.FirstOrDefault(o => o.Id == role.Id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = "Dữ liệu không tồn tại !";
                    return systemMessage;
                }
                else
                {
                    var checkisvail = db.Role.FirstOrDefault(m => (m.RoleName == role.RoleName && m.Level == role.Level) && m.Id != role.Id);

                    if (checkisvail != null && checkisvail.IsActive == true)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Trùng lặp dữ liệu vui lòng thao tác lại !";
                        return systemMessage;
                    }
                    else if (checkisvail != null && checkisvail.IsActive == false)
                    {
                        checkitem.RoleName = role.RoleName;
                        checkitem.IsActive = role.IsActive;
                        checkitem.Level = role.Level;
                        db.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
                        return systemMessage;
                    }
                    else
                    {
                        checkitem.RoleName = role.RoleName;
                        checkitem.IsActive = role.IsActive;
                        checkitem.Level = role.Level;
                        db.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
                        return systemMessage;
                    }
                }

            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }

        public ListAccountUsingRoleById ListAccountUsingRole(string searchValue, int currPage, int recodperpage, int Id, int userid, int roleiduser, int branchId)
        {
            ListAccountUsingRoleById result = new ListAccountUsingRoleById();
            List<AccountUsingRoleById> list = new List<AccountUsingRoleById>();

            var db = new DB_CSEntities1();

            var checkBrand = db.Branch.FirstOrDefault(m => m.Id == branchId);
            int statusBrand = 0;

            if (checkBrand.IsParent == true || roleiduser == SystemMessageConst.Role.Admin)
            {
                statusBrand = 1;
            }


            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand("getall_account_using_role", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Id", Id));
            cmd.Parameters.Add(new SqlParameter("@statusB", statusBrand));
            cmd.Parameters.Add(new SqlParameter("@idB", branchId));
            cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
            cmd.Parameters.Add(new SqlParameter("@currPage", currPage));
            cmd.Parameters.Add(new SqlParameter("@recodperpage", 10));
            cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int total = Convert.ToInt16(cmd.Parameters["@totalCount"].Value);
            foreach (DataRow rowItem in dt.Rows)
            {
                AccountUsingRoleById item = new AccountUsingRoleById();
                item.Id = Int32.Parse(rowItem["Id"].ToString());
                item.RoleId = Int32.Parse(rowItem["RoleId"].ToString());
                item.UserId = Int32.Parse(rowItem["UserId"].ToString());
                //item.LevelId = Int32.Parse(rowItem["LevelId"].ToString());
                item.RoleName = rowItem["RoleName"].ToString();
                item.Level = Int32.Parse(rowItem["Level"].ToString());
                item.UserName = rowItem["UserName"].ToString();
                item.LevelName = rowItem["LevelName"].ToString();
                item.Description = rowItem["Description"].ToString();
                list.Add(item);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }


        public List<ListUserUsingRole> ListUserByRoleId(int roleid, int userid, int branchId, int idroleSelect)
        {

            try
            {
                var db = new DB_CSEntities1();
                var checkBrand = db.Branch.FirstOrDefault(m => m.Id == branchId);
                var getRole = db.Role.FirstOrDefault(m => m.Id == roleid);

                if (checkBrand.IsParent == true || getRole.Level == 12)
                {
                    List<ListUserUsingRole> list = new List<ListUserUsingRole>();

                    var result = db.User.Where(m =>
                                                       m.IsActive == true
                                                       && m.isusingaccount == true
                                                       && (db.Branch.Any(z => z.IsParent == false) == true)
                                                       && (db.RoleUser.Any(x => x.RoleId == idroleSelect && x.UserId == m.Id) == false)
                                                       && (db.Role.Any(a => a.Id == idroleSelect && a.IsActive == true) == true)
                                               )
                        .ToList().All(m =>
                        {
                            ListUserUsingRole item = new ListUserUsingRole();
                            item.UserId = m.Id;
                            item.UserName = m.UserName;
                            item.levelnamerole = 0;
                            list.Add(item);
                            return true;

                        });

                    return list;
                }
                else
                {

                    List<ListUserUsingRole> list = new List<ListUserUsingRole>();

                    var result = db.User.Where(m =>
                                                    m.IsActive == true
                                                    && m.isusingaccount == true
                                                    && m.BranchId == branchId
                                                    && (db.RoleUser.Any(x => x.RoleId == idroleSelect && x.UserId == m.Id) == false)
                                                    && (db.Role.Any(a => a.Id == idroleSelect && a.IsActive == true) == true)
                                                )

                        .ToList().All(m =>
                        {
                            ListUserUsingRole item = new ListUserUsingRole();
                            item.UserId = m.Id;
                            item.UserName = m.UserName;
                            item.levelnamerole = 0;
                            list.Add(item);
                            return true;

                        });

                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemMessage DeleteAccountUsingRole(int Id)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();
                SqlParameter typeParameter = new SqlParameter("@idRoleUser", Id);
                db.Database.ExecuteSqlCommand("sp_DeleteAccountUsingRole @idRoleUser", typeParameter);
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public List<ListMenuByUserLoginViewModal> ListMenuByIdUserLogin(int roleid, int userid, int branchId, int idMember, int idrole)
        {

            try
            {
                var db = new DB_CSEntities1();

                List<ListMenuByUserLoginViewModal> list = new List<ListMenuByUserLoginViewModal>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("sp_getall_menu_import_to_account_member", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@roleid", roleid));
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.Parameters.Add(new SqlParameter("@idMember", idMember));
                cmd.Parameters.Add(new SqlParameter("@idRoleMem", idrole));
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow rowItem in dt.Rows)
                {
                    int idParent = 0;
                    ListMenuByUserLoginViewModal item = new ListMenuByUserLoginViewModal();
                    item.MenuId = Int32.Parse(rowItem["MenuId"].ToString());
                    item.MenuText = rowItem["MenuText"].ToString();
                    item.MenuURL = rowItem["MenuURL"].ToString();
                    int.TryParse(rowItem["ParentId"].ToString(), out idParent);
                    item.ParentId = idParent;
                    list.Add(item);
                }


                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemMessage AddAccount_Menu_UsingRole(MenuPermission data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();
                var checkrole = db.Role.FirstOrDefault(m => m.Id == data.RoleId && m.IsActive == true);

                if (checkrole == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.RoleIsNotExist;
                    return systemMessage;
                }


                var checkaccount = db.User.FirstOrDefault(m => m.Id == data.UserId && m.IsActive == true);
                if (checkaccount == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.AccountIsNotExist;
                    return systemMessage;
                }


                var checkroleuser = db.RoleUser.FirstOrDefault(m => m.RoleId == data.RoleId && m.UserId == data.UserId);
                if (checkroleuser == null)
                {
                    RoleUser RU = new RoleUser();
                    RU.RoleId = data.RoleId;
                    RU.UserId = (string.IsNullOrEmpty(data.UserId.ToString())) ? 0 : data.UserId.Value;
                    db.RoleUser.Add(RU);
                    db.SaveChanges();

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;

                }
                else
                {
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.RoleAndAccountExit;
                }

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }

        public LstMenuViewModel BS_ListMenuInRole(string searchValue, int currPage, int recodperpage, int Id)
        {
            LstMenuViewModel result = new LstMenuViewModel();
            try
            {
                List<Menureturn> list = new List<Menureturn>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("Role_ListMenuInRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", Id));
                cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
                cmd.Parameters.Add(new SqlParameter("@currPage", currPage));
                cmd.Parameters.Add(new SqlParameter("@recodperpage", 10));
                cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int total = Convert.ToInt16(cmd.Parameters["@totalCount"].Value);
                foreach (DataRow rowItem in dt.Rows)
                {
                    Menureturn item = new Menureturn();

                    item.IdPer = Int32.Parse(rowItem["Id"].ToString());
                    item.IdMenu = Int32.Parse(rowItem["MenuId"].ToString());
                    item.MenuName = rowItem["MenuText"].ToString();

                    item.IsRead = rowItem["IsRead"].ToString();
                    item.IsCreate = rowItem["IsCreate"].ToString();
                    item.IsUpdate = rowItem["IsUpdate"].ToString();
                    item.IsDelete = rowItem["IsDelete"].ToString();
                    item.IsExport = rowItem["IsExport"].ToString();
                    list.Add(item);
                }

                result.Data = list;
                result.Total = total;
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemMessage Edit_Select_Menu(EditSelectMenu data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();
                var checkitem = db.MenuPermission.FirstOrDefault(m => m.Id == data.id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataExisted;
                    return systemMessage;
                }
                else
                {

                    if (data.type == "iscreate")
                    {
                        checkitem.IsCreate = data.gt;
                    }
                    else if (data.type == "isread")
                    {
                        checkitem.IsRead = data.gt;
                    }
                    else if (data.type == "isupdate")
                    {
                        checkitem.IsUpdate = data.gt;
                    }
                    else if (data.type == "isexport")
                    {
                        checkitem.IsExport = data.gt;
                    }
                    else
                    {
                        checkitem.IsDelete = data.gt;
                    }

                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
                }
                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }
        
        public SystemMessage BS_DeleteMenuInRole(int id)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();
                var checkInval = db.MenuPermission.FirstOrDefault(m => m.Id == id);
                if (checkInval != null)
                {
                    SqlParameter typeParameter = new SqlParameter("@id_menu_permission", id);
                    db.Database.ExecuteSqlCommand("Role_DeleteMenuInRole @id_menu_permission", typeParameter);
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                    return systemMessage;
                }
                else
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }


            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public List<ListMenuByUserLoginViewModal> BS_Danhsachmenuthemmoivaoquyen(int roleid_login, int userid, int idrole)
        {

            try
            {
                var db = new DB_CSEntities1();

                List<ListMenuByUserLoginViewModal> list = new List<ListMenuByUserLoginViewModal>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("Role_ListMenuThemMoiVaoRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@roleid_login", roleid_login));
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.Parameters.Add(new SqlParameter("@idrole_add", idrole));
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow rowItem in dt.Rows)
                {
                    int idParent = 0;
                    ListMenuByUserLoginViewModal item = new ListMenuByUserLoginViewModal();
                    item.MenuId = Int32.Parse(rowItem["MenuId"].ToString());
                    item.MenuText = rowItem["MenuText"].ToString();
                    item.MenuURL = rowItem["MenuURL"].ToString();
                    int.TryParse(rowItem["ParentId"].ToString(), out idParent);
                    item.ParentId = idParent;
                    list.Add(item);
                }


                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        
        public SystemMessage BS_AddMenuIntoRole(MenuPermission data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();
                var checkrole = db.Role.FirstOrDefault(m => m.Id == data.RoleId && m.IsActive == true);

                if (checkrole == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.RoleIsNotExist;
                    return systemMessage;
                }

                var checkmenu = db.Menu.FirstOrDefault(m => m.Id == data.MenuId);
                if (checkmenu == null)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.MenuIsNotExist;
                    return systemMessage;
                }
                else
                {

                    if (checkmenu.ParentId != null)
                    {
                        var checkroleparent = db.MenuPermission.FirstOrDefault(m =>
                            m.RoleId == data.RoleId && m.MenuId == checkmenu.ParentId);
                        if (checkroleparent == null)
                        {
                            MenuPermission MP = new MenuPermission();
                            MP.MenuId = checkmenu.ParentId;
                            MP.RoleId = data.RoleId;
                            MP.IsRead = true;
                            MP.IsCreate = true;
                            MP.IsUpdate = true;
                            MP.IsDelete = true;
                            MP.IsExport = true;
                            db.MenuPermission.Add(MP);
                            db.SaveChanges();
                        }
                    }

                    var checkmenupermission = db.MenuPermission.FirstOrDefault(m => m.UserId == null && m.MenuId == data.MenuId && m.RoleId == data.RoleId);
                    if (checkmenupermission == null)
                    {
                        MenuPermission MP = new MenuPermission();
                        MP.MenuId = data.MenuId;
                        MP.RoleId = data.RoleId;

                        MP.IsRead = data.IsRead;
                        MP.IsCreate = data.IsCreate;
                        MP.IsUpdate = data.IsUpdate;
                        MP.IsDelete = data.IsDelete;
                        MP.IsExport = data.IsExport;
                        db.MenuPermission.Add(MP);
                        db.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                        return systemMessage;

                    }
                    else
                    {
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.RoleMenuExit;
                        return systemMessage;
                    }
                }
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }
        
    }
}
