using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Const;
using CS.Common.Security;
using CS.Common.Untils;
using CS.Data.DataContext;
using CS.Data.Model;

namespace CS.Data.Business
{
    public class ManagerListUserBussiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ListUserPageListViewModal GetAllUsers(int centerChoose, int centerId, int roleId, int userId, string search, int currentPage, int recordPerPage)
        {
            ListUserPageListViewModal listUser = new ListUserPageListViewModal();
            try
            {
                var _centerChoose = new SqlParameter
                {
                    ParameterName = "centerChoose",
                    Value = centerChoose
                };
                var _roleadmin = new SqlParameter
                {
                    ParameterName = "roleadmin",
                    Value = SystemMessageConst.Role.Admin
                };
                var _centerId = new SqlParameter
                {
                    ParameterName = "centerId",
                    Value = centerId
                };
                var _roleId = new SqlParameter
                {
                    ParameterName = "roleId",
                    Value = roleId
                };
                var _userId = new SqlParameter
                {
                    ParameterName = "userId",
                    Value = userId
                };
                var _search = new SqlParameter
                {
                    ParameterName = "search",
                    Value = search
                };
                var currpage = new SqlParameter
                {
                    ParameterName = "currPage",
                    Value = currentPage
                };
                var _recordPerPage = new SqlParameter
                {
                    ParameterName = "recodperpage",
                    Value = recordPerPage
                };
                var total = new SqlParameter
                {
                    ParameterName = "totalCount",
                    Value = 0,
                    Direction = ParameterDirection.Output
                };
                var lstBill = db.Database.SqlQuery<ListUserPageList>(
                    "[GetListUserPageList] @roleadmin,@centerChoose,@centerId,@roleId,@userId,@search,@currPage,@recodperpage,@totalCount out", _roleadmin, _centerChoose, _centerId, _roleId, _userId, _search, currpage,
                    _recordPerPage, total).ToList();

                listUser.ListUserPageList = lstBill;
                listUser.Total = Int32.Parse(total.Value.ToString());
                return listUser;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemMessage BS_ChangePasswordUser(int id)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {

                var checkExistedAccount = db.User.FirstOrDefault(ob => ob.Id == id);
                if (checkExistedAccount == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tài khoản không tồn tại !!";
                    return systemMessage;
                }


                checkExistedAccount.Password = Md5.md5("123456", 16);
                checkExistedAccount.status_password = 1;
                db.SaveChanges();
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                var result = db.User.FirstOrDefault(ob => ob.Id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int GetUserRoleById(int id)
        {
            try
            {
                var result = db.RoleUser.FirstOrDefault(ob => ob.UserId == id);
                return result.RoleId;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public string ValidateUser(User user, bool checkPass = true)
        {
            string rs = null;
            var vld = new ValidateUtils();
            if (user.isusingaccount == true)
            {

                rs = vld.CheckRequiredField(user.UserName, "Tên đăng nhập", 3, 30);
                if (rs != null)
                {
                    return rs;
                }
                if (checkPass)
                {
                    rs = vld.CheckRequiredField(user.Password, "Mật khẩu đăng nhập", 3, 25);
                    if (rs != null)
                    {
                        return rs;
                    }
                }
            }
            rs = vld.CheckRequiredField(user.FullName, "Họ và tên", 3, 50);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckNonRequiredField(user.Email, "Email", 100);
            if (rs != null)
            {
                return rs;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                rs = vld.CheckEmail(user.Email);
                if (rs != null)
                {
                    return rs;
                }
            }

            rs = vld.CheckNonRequiredField(user.Phone, "Số điện thoại", 11);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(user.UserCode, "Mã nhân viên", 5, 20);
            if (rs != null)
            {
                return rs;
            }
            return null;
        }
        public SystemMessage EditUser(User user, int myId, int roleid)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateUser(user, false);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }
                var checkExistedAccount = db.User.FirstOrDefault(ob => ob.Id == user.Id);
                if (checkExistedAccount == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tài khoản không tồn tại !!!!";
                    return systemMessage;
                }


                var checkUserCode = db.User.FirstOrDefault(ob => ob.UserCode == user.UserCode && ob.Id != user.Id);
                if (checkUserCode != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mã nhân viên đã tồn tại !!!!";
                    return systemMessage;
                }
                var checkBranch = db.Branch.FirstOrDefault(ob => ob.Id == user.BranchId);
                if (checkBranch == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đại lý không tồn tại !!!!";
                    return systemMessage;
                }
                if (user.ParentId != null)
                {
                    var checkParent = db.User.FirstOrDefault(ob => ob.Id == user.ParentId && ob.BranchId == user.BranchId);
                    if (checkParent == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tài khoản quản lý không tồn tại !!!!";
                        return systemMessage;
                    }
                }

                var checkRole = db.Role.FirstOrDefault(ob => ob.Id == roleid && ob.IsActive == true);
                if (checkRole == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Quyền tài khoản không tồn tại !!!!";
                    return systemMessage;
                }

                if (user.isusingaccount == true)
                {
                    if (string.IsNullOrEmpty(checkExistedAccount.UserName))
                    {
                        var checkExistedUsername =
                            db.User.FirstOrDefault(ob => ob.UserName == user.UserName && ob.Id != user.Id);
                        if (checkExistedUsername != null)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Tên đăng nhập đã tồn tại !!!";
                            return systemMessage;
                        }
                        checkExistedAccount.UserName = user.UserName;
                    }
                    if (string.IsNullOrEmpty(checkExistedAccount.Password) || string.IsNullOrEmpty(checkExistedAccount.UserName))
                    {
                        var vld = new ValidateUtils();
                        var checkPass = vld.CheckRequiredField(user.Password, "Mật khẩu", 3, 55);
                        if (checkPass != null)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = checkPass;
                            return systemMessage;
                        }
                        checkExistedAccount.Password = Md5.md5(user.Password, 16);

                    }
                }

                checkExistedAccount.isusingaccount = user.isusingaccount;
                checkExistedAccount.DateModify = DateTime.Now;
                checkExistedAccount.UserCode = user.UserCode;
                checkExistedAccount.IsActive = user.IsActive;
                checkExistedAccount.Email = user.Email;
                checkExistedAccount.FullName = user.FullName;
                checkExistedAccount.Phone = user.Phone;
                checkExistedAccount.DateOfBirth = user.DateOfBirth;
                checkExistedAccount.BranchId = user.BranchId;
                checkExistedAccount.ParentId = user.ParentId;
                checkExistedAccount.parent_update_by = user.parent_update_by;
                checkExistedAccount.parent_update_time = user.parent_update_time;
                checkExistedAccount.user_update_by = user.user_update_by;
                checkExistedAccount.DateModify = user.DateModify;
                var currentRole = db.RoleUser.FirstOrDefault(ob => ob.UserId == user.Id);
                if (currentRole != null)
                {
                    currentRole.RoleId = roleid;
                }
                else
                {
                    RoleUser roleUser = new RoleUser();
                    roleUser.RoleId = roleid;
                    roleUser.UserId = user.Id;
                    db.RoleUser.Add(roleUser);
                }
                db.SaveChanges();
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }


        public SystemMessage AddUser(User user, int role)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateUser(user);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }


                var checkUserCode = db.User.FirstOrDefault(ob => ob.UserCode == user.UserCode);
                if (checkUserCode != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mã nhân viên đã tồn tại !!!!";
                    return systemMessage;
                }
                var checkBranch = db.Branch.FirstOrDefault(ob => ob.Id == user.BranchId);
                if (checkBranch == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đại lý không tồn tại !!!!";
                    return systemMessage;
                }


                var checkRole = db.Role.FirstOrDefault(ob => ob.Id == role);
                if (checkRole == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Quyền không tồn tại !!!!";
                    return systemMessage;
                }
                if (user.ParentId != null)
                {
                    var checkParent = db.User.FirstOrDefault(ob => ob.Id == user.ParentId && ob.BranchId == user.BranchId);
                    if (checkParent == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tài khoản quản lý không tồn tại !!!!";
                        return systemMessage;
                    }
                }
                if (user.isusingaccount == true)
                {
                    var checkExistedAccount = db.User.FirstOrDefault(ob => ob.UserName == user.UserName);
                    if (checkExistedAccount != null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Tên tài khoản đã tồn tại !!!!";
                        return systemMessage;
                    }
                    user.Password = Md5.md5(user.Password, 16);
                }
                user.DateCreated = DateTime.Now;
                user.IsActive = true;
                user.on_working = 0;
                db.User.Add(user);
                db.SaveChanges();

                RoleUser roleUser = new RoleUser();
                roleUser.RoleId = role;
                roleUser.UserId = user.Id;
                db.RoleUser.Add(roleUser);
                db.SaveChanges();

                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                return systemMessage;

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
