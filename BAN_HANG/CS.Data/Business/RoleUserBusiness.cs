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
    public class RoleUserBusiness
    {
        public RU_GetAllUserViewModel ListUserInSystem(string searchValue, int currPage, int recodperpage, int userid, int roleiduser, int branchId)
        {
            RU_GetAllUserViewModel result = new RU_GetAllUserViewModel();
            List<lst_RU_GetAllUserViewModel> list = new List<lst_RU_GetAllUserViewModel>();

            var db = new DB_CSEntities1();

            var checkBrand = db.Branch.FirstOrDefault(m => m.Id == branchId);
            int level = db.Role.FirstOrDefault(m => m.Id == roleiduser).Level.Value;
            int statusBrand = 0;

            if (checkBrand.IsParent == true || level == 12)
            {
                statusBrand = 1;
            }


            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand("get_all_acount_and_role", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@level", level));
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
                lst_RU_GetAllUserViewModel item = new lst_RU_GetAllUserViewModel();
                item.Id = Int32.Parse(rowItem["Id"].ToString());
                item.UserName = rowItem["UserName"].ToString();
                item.Level = Int32.Parse(rowItem["Level"].ToString());
                item.RoleName = rowItem["RoleName"].ToString();
                item.BRANDNAME = rowItem["BRANDNAME"].ToString();
                item.IDROLE = int.Parse(rowItem["IDROLE"].ToString());
                item.ROLEUSERID = int.Parse(rowItem["ROLEUSERID"].ToString());
                list.Add(item);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }

        public SystemMessage Add_Account_Into_Role(MenuPermission data)
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
                    RU.UserId = data.UserId.Value;
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

        public SystemMessage DeleteRoleUserAndMenu(int idrole, int iduser, int idroleuser)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();
                var delMenuPer = db.MenuPermission.Where(m => m.UserId == iduser && m.RoleId == idrole).ToList()
                    .All(m =>
                    {
                        SqlParameter typeParameter = new SqlParameter("@id", m.Id);
                        db.Database.ExecuteSqlCommand("[sp_delete_role_user_in_menu_permisstion] @id", typeParameter);
                        return true;
                    });

                SqlParameter typeParameter2 = new SqlParameter("@id", idroleuser);
                db.Database.ExecuteSqlCommand("[sp_delete_roleuser_by_id] @id", typeParameter2);

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

        public LstMenuViewModel ListMenuOfRoleAndAccount(string searchValue, int currPage, int recodperpage, int iduser, int idrole)
        {
            LstMenuViewModel result = new LstMenuViewModel();
            List<Menureturn> list = new List<Menureturn>();

            var db = new DB_CSEntities1();

            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand("getall_menuby_user", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@iduser", iduser));
            cmd.Parameters.Add(new SqlParameter("@idrole", idrole));
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
                item.RoleName = rowItem["RoleName"].ToString();



                if (string.IsNullOrEmpty(rowItem["UserId"].ToString()))
                {
                    lsIsCreate c = new lsIsCreate();
                    c.IsCreate = rowItem["IsCreate"].ToString();
                    c.status = false;
                    item.Create = c;

                    lsIsRead r = new lsIsRead();
                    r.IsRead = rowItem["IsRead"].ToString();
                    r.status = false;
                    item.Read = r;

                    lsIsUpdate u = new lsIsUpdate();
                    u.IsUpdate = rowItem["IsUpdate"].ToString();
                    u.status = false;
                    item.Update = u;

                    lsIsDelete d = new lsIsDelete();
                    d.IsDelete = rowItem["IsDelete"].ToString();
                    d.status = false;
                    item.Delete = d;

                    lsIsExport x = new lsIsExport();
                    x.IsExport = rowItem["IsExport"].ToString();
                    x.status = false;
                    item.Export = x;

                    OptionMenu op = new OptionMenu();
                    op.IdPermis = Int32.Parse(rowItem["Id"].ToString());
                    op.status = false;
                    item.Option = op;
                }
                else
                {
                    lsIsCreate c = new lsIsCreate();
                    c.IsCreate = rowItem["IsCreate"].ToString();
                    c.status = true;
                    item.Create = c;

                    lsIsRead r = new lsIsRead();
                    r.IsRead = rowItem["IsRead"].ToString();
                    r.status = true;
                    item.Read = r;

                    lsIsUpdate u = new lsIsUpdate();
                    u.IsUpdate = rowItem["IsUpdate"].ToString();
                    u.status = true;
                    item.Update = u;

                    lsIsDelete d = new lsIsDelete();
                    d.IsDelete = rowItem["IsDelete"].ToString();
                    d.status = true;
                    item.Delete = d;

                    lsIsExport x = new lsIsExport();
                    x.IsExport = rowItem["IsExport"].ToString();
                    x.status = false;
                    item.Export = x;

                    OptionMenu op = new OptionMenu();
                    op.IdPermis = Int32.Parse(rowItem["Id"].ToString());
                    op.status = true;
                    item.Option = op;

                }
                list.Add(item);
            }

            result.Data = list;
            result.Total = total;
            return result;
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

                    if (data.type == "create")
                    {
                        checkitem.IsCreate = data.gt;
                    }
                    else if (data.type == "read")
                    {
                        checkitem.IsRead = data.gt;
                    }
                    else if (data.type == "update")
                    {
                        checkitem.IsUpdate = data.gt;
                    }
                    else if (data.type == "export")
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

        public SystemMessage Delete_Select_Menu(EditSelectMenu data)
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
                    SqlParameter typeParameter2 = new SqlParameter("@id", data.id);
                    db.Database.ExecuteSqlCommand("sp_delete_menubyid @id", typeParameter2);

                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
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

        public SystemMessage AddMenuIntoAccountRole(MenuPermission data)
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
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.AccountIsnotRoleExit;
                    return systemMessage;

                }
                else
                {
                    if (checkmenu.ParentId != null)
                    {
                        var checkroleparent = db.MenuPermission.FirstOrDefault(m =>
                            m.RoleId == data.RoleId && m.UserId == data.UserId && m.MenuId == checkmenu.ParentId);
                        if (checkroleparent == null)
                        {
                            MenuPermission MP = new MenuPermission();
                            MP.MenuId = checkmenu.ParentId;
                            MP.RoleId = data.RoleId;
                            MP.UserId = data.UserId;
                            MP.IsRead = data.IsRead;
                            MP.IsCreate = true;
                            MP.IsUpdate = true;
                            MP.IsDelete = true;
                            MP.IsExport = true;
                            db.MenuPermission.Add(MP);
                            db.SaveChanges();
                        }
                    }

                    var checkmenupermission = db.MenuPermission.FirstOrDefault(m => (m.UserId == data.UserId || m.UserId == null) && m.MenuId == data.MenuId && m.RoleId == data.RoleId);
                    if (checkmenupermission == null)
                    {
                        MenuPermission MP = new MenuPermission();
                        MP.MenuId = data.MenuId;
                        MP.RoleId = data.RoleId;
                        MP.UserId = data.UserId;
                        MP.IsRead = data.IsRead;
                        MP.IsCreate = data.IsCreate;
                        MP.IsUpdate = data.IsUpdate;
                        MP.IsDelete = data.IsDelete;
                        MP.IsExport = data.IsExport;
                        db.MenuPermission.Add(MP);
                        db.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;

                    }
                    else
                    {
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.RoleAndAccountandMenuExit;
                    }
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


        public List<ListMenuByUserLoginViewModal> ListMenuByIdUserLogin(int roleid, int userid, int branchId, int idrole)
        {

            try
            {
                var db = new DB_CSEntities1();

                List<ListMenuByUserLoginViewModal> list = new List<ListMenuByUserLoginViewModal>();

                var checkMenuInvailRole =
                    db.MenuPermission.FirstOrDefault(m => m.RoleId == idrole && m.UserId == null);



                if (checkMenuInvailRole != null)
                {

                    SqlConnection con = new SqlConnection();
                    con = Connection.Connect.GetConnect();
                    SqlCommand cmd = new SqlCommand("[sp_getall_menubyparent]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@roleid", roleid));
                    cmd.Parameters.Add(new SqlParameter("@userid", userid));
                    //cmd.Parameters.Add(new SqlParameter("@idRoleMem", idrole));
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow rowItem in dt.Rows)
                    {
                        int idParent = 0;
                        ListMenuByUserLoginViewModal item = new ListMenuByUserLoginViewModal();
                        //  item.id = Int32.Parse(rowItem["Id"].ToString());
                        item.MenuId = Int32.Parse(rowItem["MenuId"].ToString());
                        item.MenuText = rowItem["MenuText"].ToString();
                        item.MenuURL = rowItem["MenuURL"].ToString();
                        int.TryParse(rowItem["ParentId"].ToString(), out idParent);
                        item.ParentId = idParent;

                        var kq = db.MenuPermission.FirstOrDefault(x =>
                            (x.RoleId == idrole && x.MenuId == item.MenuId && x.UserId == null));
                        if (kq != null)
                        {
                            item.trangthai = 1;
                        }
                        else
                        {
                            item.trangthai = 0;
                        }

                        bool IsRead = false;
                        bool IsCreate = false;
                        bool IsUpdate = false;
                        bool IsDelete = false;
                        bool IsExport = false;

                        bool.TryParse(rowItem["IsRead"].ToString(), out IsRead);
                        bool.TryParse(rowItem["IsCreate"].ToString(), out IsCreate);
                        bool.TryParse(rowItem["IsUpdate"].ToString(), out IsUpdate);
                        bool.TryParse(rowItem["IsDelete"].ToString(), out IsDelete);
                        bool.TryParse(rowItem["IsExport"].ToString(), out IsExport);

                        item.IsRead = IsRead;
                        item.IsCreate = IsCreate;
                        item.IsUpdate = IsUpdate;
                        item.IsDelete = IsDelete;
                        item.IsExport = IsExport;

                        list.Add(item);
                    }

                    return list;
                }
                else
                {
                    SqlConnection con = new SqlConnection();
                    con = Connection.Connect.GetConnect();
                    SqlCommand cmd = new SqlCommand("[sp_getall_menubyparent]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@roleid", roleid));
                    cmd.Parameters.Add(new SqlParameter("@userid", userid));
                    //  cmd.Parameters.Add(new SqlParameter("@idRoleMem", idrole));
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow rowItem in dt.Rows)
                    {
                        int idParent = 0;
                        ListMenuByUserLoginViewModal item = new ListMenuByUserLoginViewModal();
                        // item.id = Int32.Parse(rowItem["Id"].ToString());
                        item.MenuId = Int32.Parse(rowItem["MenuId"].ToString());
                        item.MenuText = rowItem["MenuText"].ToString();
                        item.MenuURL = rowItem["MenuURL"].ToString();
                        int.TryParse(rowItem["ParentId"].ToString(), out idParent);
                        item.ParentId = idParent;

                        bool IsRead = false;
                        bool IsCreate = false;
                        bool IsUpdate = false;
                        bool IsDelete = false;
                        bool IsExport = false;

                        bool.TryParse(rowItem["IsRead"].ToString(), out IsRead);
                        bool.TryParse(rowItem["IsCreate"].ToString(), out IsCreate);
                        bool.TryParse(rowItem["IsUpdate"].ToString(), out IsUpdate);
                        bool.TryParse(rowItem["IsDelete"].ToString(), out IsDelete);
                        bool.TryParse(rowItem["IsExport"].ToString(), out IsExport);

                        item.IsRead = IsRead;
                        item.IsCreate = IsCreate;
                        item.IsUpdate = IsUpdate;
                        item.IsDelete = IsDelete;
                        item.IsExport = IsExport;


                        list.Add(item);
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public SystemMessage _AddNewRoleUserMenu(List<int> lstAccount, List<int> lstMenu, List<string> lstOptionMenu, int RoleId, bool statusCheckAllAcount)
        {
            SystemMessage systemMessage = new SystemMessage();
            bool status = false;

            try
            {
                var db = new DB_CSEntities1();
                var checkrole = db.Role.FirstOrDefault(m => m.Id == RoleId && m.IsActive == true);

                if (checkrole == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.RoleIsNotExist;
                    return systemMessage;
                }

                if (lstAccount.Count == 0 && statusCheckAllAcount == false)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.lstAccountIsnotNull;
                    return systemMessage;
                }
                if (lstMenu.Count == 0)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.lstMenuIsnotNull;
                    return systemMessage;
                }
                if (lstOptionMenu.Count == 0)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.lstMenuIsnotNull;
                    return systemMessage;
                }

                //truong hop update tất cả menu trong quyền
                if (statusCheckAllAcount == true)
                {

                    // xóa tất cả các menu cũ có trong quyền
                    SqlParameter typeParameter1 = new SqlParameter("@RoleId", RoleId);
                    db.Database.ExecuteSqlCommand("RoleUser_DeleteAll_MenuUsingRole_ByIdRole @RoleId", typeParameter1);

                    // kiểm tra nếu có acc mới thì add acc vào quyền
                    if (lstAccount.Count > 0)
                    {
                        for (int i = 0; i < lstAccount.Count; i++)
                        {
                            int idAccount = lstAccount[i];
                            var checkAccount = db.User.FirstOrDefault(m => m.Id == idAccount && m.IsActive == true);
                            if (checkAccount != null)
                            {
                                var checkRoleAndUser = db.RoleUser.FirstOrDefault(m =>
                                    m.UserId == checkAccount.Id && m.RoleId == RoleId);
                                if (checkRoleAndUser == null)
                                {
                                    RoleUser ru = new RoleUser();
                                    ru.RoleId = RoleId;
                                    ru.UserId = idAccount;
                                    db.RoleUser.Add(ru);
                                    db.SaveChanges();
                                }

                                // lam them chua tesst
                                var checkAccountMenu = db.MenuPermission
                                    .Where(m => m.UserId == idAccount && m.RoleId == RoleId).ToList();
                                if (checkAccountMenu.Count > 0)
                                {
                                    foreach (var item in checkAccountMenu)
                                    {
                                        db.MenuPermission.Remove(item);
                                        db.SaveChanges();
                                    }
                                }

                            }
                        }
                    }


                    // xét các menu mới đã chọn vào quyền với userId = null
                    for (int i = 0; i < lstOptionMenu.Count; i++)
                    {
                        MenuPermission mp = new MenuPermission();
                        string[] words = lstOptionMenu[i].Split(',');


                        // lam them chua tesst

                        int IDMENU = int.Parse(words[0]);
                        var checkAccountMenu = db.MenuPermission
                            .Where(m => m.RoleId == RoleId && m.MenuId == IDMENU).ToList();
                        if (checkAccountMenu.Count > 0)
                        {
                            foreach (var item in checkAccountMenu)
                            {
                                db.MenuPermission.Remove(item);
                                db.SaveChanges();
                            }
                        }


                        mp.RoleId = RoleId;
                        mp.MenuId = int.Parse(words[0]);
                        mp.IsRead = bool.Parse(words[1]);
                        mp.IsCreate = bool.Parse(words[2]);
                        mp.IsUpdate = bool.Parse(words[3]);
                        mp.IsDelete = bool.Parse(words[4]);
                        mp.IsExport = bool.Parse(words[5]);
                        db.MenuPermission.Add(mp);
                        db.SaveChanges();

                        systemMessage.IsSuccess = true;
                        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                    }
                }
                else
                {

                    // trường hợp update thêm mới từng người trong quyền


                    //
                    for (int i = 0; i < lstAccount.Count; i++)
                    {

                        //kiểm tra acc có tồn tại ko
                        int idAccount = lstAccount[i];
                        var checkAccount = db.User.FirstOrDefault(m => m.Id == idAccount && m.IsActive == true);

                        if (checkAccount != null)
                        {

                            //kiểm tra acc đã có quyền chưa
                            var checkRoleAndUser = db.RoleUser.FirstOrDefault(m =>
                                m.UserId == checkAccount.Id && m.RoleId == RoleId);

                            // chưa có thêm mới
                            if (checkRoleAndUser == null)
                            {
                                RoleUser ru = new RoleUser();
                                ru.RoleId = RoleId;
                                ru.UserId = idAccount;
                                db.RoleUser.Add(ru);
                                db.SaveChanges();
                            }


                            // danh sách menu vừa chọn
                            for (int v = 0; v < lstOptionMenu.Count; v++)
                            {
                                MenuPermission mp = new MenuPermission();
                                string[] words = lstOptionMenu[v].Split(',');


                                // kiểm tra trong quyền đã tồn tại menu này chưa
                                int IDMENU = int.Parse(words[0]);

                                var checkMP = db.MenuPermission.FirstOrDefault(m =>
                                    m.RoleId == RoleId && m.MenuId == IDMENU && (m.UserId == null || m.UserId == idAccount));

                                // nếu chưa có thì thêm mới menu vào quyền và kèm theo userId > menu này chỉ dành riêng cho user này thôi
                                if (checkMP == null)
                                {
                                    mp.UserId = idAccount;
                                    mp.RoleId = RoleId;
                                    mp.MenuId = int.Parse(words[0]);
                                    mp.IsRead = bool.Parse(words[1]);
                                    mp.IsCreate = bool.Parse(words[2]);
                                    mp.IsUpdate = bool.Parse(words[3]);
                                    mp.IsDelete = bool.Parse(words[4]);
                                    mp.IsExport = bool.Parse(words[5]);
                                    db.MenuPermission.Add(mp);
                                    db.SaveChanges();

                                    systemMessage.IsSuccess = true;
                                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                                }
                                else
                                {
                                    systemMessage.IsSuccess = true;
                                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                                }

                                // nếu có rồi thì bỏ qua
                            }
                        }
                        else
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = SystemMessageConst.systemmessage.AccountIsNotExist;
                        }
                    }
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

        public List<ListMenuByUserLoginViewModal> BS_Danhsachmenuthemmoivaotaikhoan(int roleid, int userid, int branchId, int idMember, int idrole)
        {

            try
            {
                var db = new DB_CSEntities1();

                List<ListMenuByUserLoginViewModal> list = new List<ListMenuByUserLoginViewModal>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("[RoleUser_Themmoimenuchotaikhoan]", con);
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
    }
}
