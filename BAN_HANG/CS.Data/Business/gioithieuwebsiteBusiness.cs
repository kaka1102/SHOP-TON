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
    public class gioithieuwebsiteBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnPage BS_GetAllPage(string searchValue, int currPage, int recodperpage)
        {
            try
            {
                ReturnPage result = new ReturnPage();
                List<tbl_gioithieu> list = new List<tbl_gioithieu>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("BS_GetAllPage", con);
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
                    tbl_gioithieu item = new tbl_gioithieu();

                    item.id = int.Parse(rowItem["Id"].ToString());
                    item.noidung = rowItem["noidung"].ToString();
                    item.ngaytao = DateTime.Parse(rowItem["ngaytao"].ToString());

                    item.type = rowItem["type"].ToString();
                    item.tennguoitao = rowItem["tennguoitao"].ToString();
                    item.tennguoisua = rowItem["tennguoisua"].ToString();
                    item.tentrang = rowItem["tentrang"].ToString();

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

        public tbl_gioithieu GetPagebyid(int id)
        {
            try
            {
                var data = db.tbl_gioithieu.FirstOrDefault(m => m.id == id);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public SystemMessage BS_EditPage(tbl_gioithieu data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                if (string.IsNullOrEmpty(data.noidung))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Nội dung không được để trống";
                    return systemMessage;
                }
                
                var checkitem = db.tbl_gioithieu.FirstOrDefault(m => m.id == data.id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataExisted;
                    return systemMessage;
                }

                checkitem.noidung = data.noidung;
                checkitem.tennguoisua = data.tennguoisua;
                checkitem.ngaychinhsua = data.ngaychinhsua;

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
    }
}
