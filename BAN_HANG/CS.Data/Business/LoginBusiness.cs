using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CS.Common.Const;
using CS.Common.Security;
using CS.Common.Untils;
using CS.Data.DataContext;
using CS.Data.Untils;
using SessionUser = CS.Data.Model.SessionUser;


namespace CS.Data.Business
{
    public class LoginBusiness
    {
        public MessageLogin AdminCheckLogin(string username, string password)
        {
            MessageLogin systemMessage = new MessageLogin();
            var db = new DB_CSEntities1();

            try
            {

                password = Md5.md5(password.Trim(), 16);

                var login = db.User.FirstOrDefault(i => i.UserName == username.Trim() && i.isusingaccount == true && i.IsActive == true);
                if (login == null)
                {

                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.AccountNotExist;

                }
                else
                {
                    if (password != login.Password)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = SystemMessageConst.systemmessage.PasswordNotCorrect;
                    }
                    else
                    {


                        var checkrole = db.RoleUser.Where(m => m.UserId == login.Id).ToList();

                        if (checkrole.Count == 0)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = SystemMessageConst.systemmessage.NotRole;
                        }
                        else
                        {
                            var branch = db.Branch.FirstOrDefault(i => i.Id == login.BranchId);


                            SessionUser sessionUser = new SessionUser();

                            sessionUser.Id = login.Id;
                            sessionUser.UserName = login.UserName;
                            sessionUser.Email = login.Email;
                            sessionUser.Phone = login.Phone;
                            sessionUser.FullName = login.FullName;
                            sessionUser.BranchId = login.BranchId.Value;
                            sessionUser.BranchName = branch.ShortName;
                            sessionUser.Roleid = checkrole[0].RoleId;
                            sessionUser.Code_brand = branch.Branch_code;

                            HttpContext.Current.Session["session"] = sessionUser;


                            systemMessage.IsSuccess = true;
                            systemMessage.Message = "/Home/Index";

                        }
                    }
                }

                if (systemMessage.IsSuccess)
                {
                    SessionUser user = GetSessionBusiness.GetUser();

                    var myrole = db.Role.FirstOrDefault(ob => ob.Id == user.Roleid);
                    var url = myrole.HomeUrl;
                    if (!string.IsNullOrEmpty(url))
                    {
                        systemMessage.Message = url;
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
    }
}
