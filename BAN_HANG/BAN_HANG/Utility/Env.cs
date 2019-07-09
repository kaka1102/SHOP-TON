using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace WebApplication
{
    public static class Env
    { 
        /// <summary>
        /// Its used for get role id and role name from Claims
        /// </summary>
        /// <param name="s"></param>
        /// <param name="IsRoleID">If you want role ID then pass true , if role name then pass false</param>
        /// <returns></returns>
        public static string GetUserRoleOrUsername(this HtmlHelper s, bool IsRoleID)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string role = string.Empty;
            if (IsRoleID == true)
            {
                role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
            }
            else
            {
                role = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            return role;
        }

        /// <summary>
        /// This Method will used for take all data from Claims Cookies 
        /// </summary>
        /// <param name="value">use "name" for Get UserName, 
        /// use "userid" for Get Logedin UserId,
        /// use "company" for Get Company Name,
        /// use "email" for Get Email,
        /// use "roleid" for Get RoleId,
        /// use "rolename" for Get RoleName,
        /// use "image" for Get User Profile Image,
        /// use "theme" for Get Theme (color scheme)
        /// </param>
        /// <returns>String</returns>
        public static string GetUserInfo(string value)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string ReturnVal = string.Empty;
            switch (value)
            {

                case "Level":

                    ReturnVal = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    break;

                case "name":

                    ReturnVal = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    break;

                case "userid":
                    ReturnVal = identity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault();
                    break; 

                case "roleid":

                    ReturnVal = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                    break;

                case "BranchName":

                    ReturnVal = identity.Claims.Where(c => c.Type == "BranchName").Select(c => c.Value).SingleOrDefault();
                    break;

                case "BranchId":

                    ReturnVal = identity.Claims.Where(c => c.Type == "BranchId").Select(c => c.Value).SingleOrDefault();

                    break;

                case "DepartmentName":

                    ReturnVal = identity.Claims.Where(c => c.Type == "DepartmentName").Select(c => c.Value).SingleOrDefault();
                    break;

                case "DepartmentId":

                    ReturnVal = identity.Claims.Where(c => c.Type == "DepartmentId").Select(c => c.Value).SingleOrDefault();
                    break;

                case "Avatar":

                    ReturnVal = identity.Claims.Where(c => c.Type == "Avatar").Select(c => c.Value).SingleOrDefault();
                    break;

                default:
                    ReturnVal = "";
                    break;
            }

            return ReturnVal;
             
        }


        public static string Language()
        {
            var currentContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            try
            {
                var routeData = RouteTable.Routes.GetRouteData(currentContext);
                string languageCode = (string)routeData.Values["cultureName"];
                return languageCode.ToLower();
            }
            catch (Exception)
            {
                return "en";
            }

        }

        public static string Decrypt(string cryptedString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Encrypt Method used for Encrypt to any String. you may use this for password encryption and decryption or other string.
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string Encrypt(string originalString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            string output = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            //if (output.Contains('+'))
            //{
            //    output = output.Replace("+", "%2B");
            //}
            return output;
        }

        public static string GetSiteRoot()
        {
            string sOut = "";
            if (System.Web.HttpContext.Current != null)
            {
                string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
                if (Port == null || Port == "80" || Port == "443")
                    Port = string.Empty;
                else
                    Port = ":" + Port;

                string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
                if (Protocol == null || Protocol.Equals("0"))
                    Protocol = "http://";
                else
                    Protocol = "https://";

                string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
                if (appPath == "/")
                    appPath = "";

                sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            }
            return sOut;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stark"></param>
        /// <param name="FormId">Enter Here Form ID LIKE <form id="frmCreate"></form>  So you have to pass = frmCreate</param>
        /// <param name="DataTableId">Which DataTable You have update after submit provide that ID</param>
        /// <param name="IsCloseAfterSubmit">Do you want to opened popup close after submit , So pass=true or false any</param> 
        /// <param name="SuccessMessage">Give any Success message</param>
        /// <returns></returns>
        public static MvcHtmlString StarkAjaxFormSubmiter(this AjaxHelper stark, string FormId, string DataTableId,
            bool IsCloseAfterSubmit, string SuccessMessage)
        {
             
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string successMsg = "Action Successfully Executed";

            if (SuccessMessage.Length > 0)
            {
                successMsg = SuccessMessage;
            }

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function (e) {");
            sb.AppendLine("var frm = $('#" + FormId + "');");
            sb.AppendLine("frm.on('submit', (function (e) {");
            sb.AppendLine("    e.preventDefault();");
            sb.AppendLine("    $.ajax({");
            sb.AppendLine("        url: frm.attr('action'),");
            sb.AppendLine("        type: frm.attr('method'),");
            sb.AppendLine("        data: new FormData(this),");
            sb.AppendLine("        contentType: false,");
            sb.AppendLine("        cache: false,");
            sb.AppendLine("        processData: false,");
            sb.AppendLine("        success: function (data) {");
            sb.AppendLine("            if (data == \"Sumitted\") {");
            if (DataTableId.Length > 0)
            {
                sb.AppendLine("                try {");
                sb.AppendLine("                    var table = $('#" + DataTableId + "').DataTable();");
                sb.AppendLine("                    table.ajax.reload();");
                sb.AppendLine("                } catch (e) { }");
            }
            sb.AppendLine("                $.sticky('<br/> " + successMsg + "', { stickyClass: 'success' });");
            if (IsCloseAfterSubmit)
            {
                sb.AppendLine("               $(\".ui-dialog-titlebar-close\").click();");
            }

            sb.AppendLine("                  $('#starkloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.indexOf(\"Error\") >= 0) {");
            sb.AppendLine("                $.sticky('<br/> ! Something is went wrong. <br/> ' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#starkloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("           else {");
            sb.AppendLine("               $.sticky('<br/> Read Warnings Alert.  <br/> <b>' + data + ' <b>', { stickyClass: 'error' });");
            sb.AppendLine("               $('#starkloader').hide();");
            sb.AppendLine("          }");


            sb.AppendLine("        },");
            sb.AppendLine("       error: function (data) {");
            sb.AppendLine("           $.sticky('<br/> ! Something is went wrong. <br/> <b>Error:<b>' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#starkloader').hide();");
            sb.AppendLine("       }");
            sb.AppendLine("    });");
            sb.AppendLine("}");
            sb.AppendLine("));");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            return new MvcHtmlString(sb.ToString());
        }


        public static MvcHtmlString StarkAjaxFormSubmiterReload(this AjaxHelper stark, string FormId, string DataTableId,
           bool IsCloseAfterSubmit, string SuccessMessage)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string successMsg = "Action Successfully Executed";

            if (SuccessMessage.Length > 0)
            {
                successMsg = SuccessMessage;
            }

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function (e) {");
            sb.AppendLine("var frm = $('#" + FormId + "');");
            sb.AppendLine("frm.on('submit', (function (e) {");
            sb.AppendLine("    e.preventDefault();");
            sb.AppendLine("    $.ajax({");
            sb.AppendLine("        url: frm.attr('action'),");
            sb.AppendLine("        type: frm.attr('method'),");
            sb.AppendLine("        data: new FormData(this),");
            sb.AppendLine("        contentType: false,");
            sb.AppendLine("        cache: false,");
            sb.AppendLine("        processData: false,");
            sb.AppendLine("        success: function (data) {");
            sb.AppendLine("            if (data == \"Sumitted\") {");
            if (DataTableId.Length > 0)
            {
                sb.AppendLine("                try {");
                sb.AppendLine("                    var table = $('#" + DataTableId + "').DataTable();");
                sb.AppendLine("                    table.ajax.reload();");
                sb.AppendLine("                } catch (e) { }");
            }
            sb.AppendLine("                $.sticky('<br/> " + successMsg + "', { stickyClass: 'success' });");
            if (IsCloseAfterSubmit)
            {
                sb.AppendLine("               $(\".ui-dialog-titlebar-close\").click();");
            }

            sb.AppendLine("                  $('#starkloader').hide();  location.reload();");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.indexOf(\"Error\") >= 0) {");
            sb.AppendLine("                $.sticky('<br/> ! Something is went wrong. <br/> ' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#starkloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("           else {");
            sb.AppendLine("               $.sticky('<br/> Read Warnings Alert.  <br/> <b>' + data + ' <b>', { stickyClass: 'error' });");
            sb.AppendLine("               $('#starkloader').hide();");
            sb.AppendLine("          }");


            sb.AppendLine("        },");
            sb.AppendLine("       error: function (data) {");
            sb.AppendLine("           $.sticky('<br/> ! Something is went wrong. <br/> <b>Error:<b>' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#starkloader').hide();");
            sb.AppendLine("       }");
            sb.AppendLine("    });");
            sb.AppendLine("}");
            sb.AppendLine("));");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            return new MvcHtmlString(sb.ToString());
        }

    }
}
