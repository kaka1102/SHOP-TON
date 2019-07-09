using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS.Data.DataContext;

namespace BAN_HANG.Areas.Client.Business
{
    public class GioithieuBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public tbl_gioithieu GetPage_Gioithieu()
        {
            
            try
            {
                var data = db.tbl_gioithieu.FirstOrDefault(m => m.type == "gioithieu");
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}