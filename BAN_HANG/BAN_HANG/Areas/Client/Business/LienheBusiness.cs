using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.DataContext;

namespace BAN_HANG.Areas.Client.Business
{
    public class LienheBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public tbl_gioithieu GetPage_Lienhe()
        {
            try
            {
                var data = db.tbl_gioithieu.FirstOrDefault(m => m.type == "lienhe");
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string ValidateItem(tbl_lienhe LvMenu)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(LvMenu.hoten, "Họ tên", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.dienthoai, "Số điện thoại", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.dienthoai, "Nội dung", 1, 10000);
            if (rs != null)
            {
                return rs;
            }
            
            return null;
        }

        public SystemMessage BS_AddNewContact(tbl_lienhe data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var check = ValidateItem(data);

                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }
               

                db.tbl_lienhe.Add(data);
                db.SaveChanges();

                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.sendcontactok;
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