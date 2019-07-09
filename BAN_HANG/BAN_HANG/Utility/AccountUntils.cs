using CS.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS.Data.Model;

namespace BAN_HANG.Utility
{
    public static class AccountUntils
    {
        public static SessionUser GetUser()
        {
            SessionUser user = new SessionUser();
            if (HttpContext.Current.Session["session"] != null)
            {
                user = (SessionUser)HttpContext.Current.Session["session"];
            }

            return user;
        }
    }
}