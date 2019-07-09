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
    public class ContactcustomerBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnListLienhe BS_DSYkienKH(string searchValue, int currPage, int recodperpage)
        {
            try
            {
                ReturnListLienhe result = new ReturnListLienhe();
                List<item_lienhe> list = new List<item_lienhe>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("BS_DSYkienKH", con);
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
                    item_lienhe item = new item_lienhe();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.hoten = rowItem["hoten"].ToString();
                    item.diachi = rowItem["diachi"].ToString();
                    item.dienthoai = rowItem["dienthoai"].ToString();
                    item.email = rowItem["email"].ToString();
                    item.tieude = rowItem["tieude"].ToString();
                    item.noidung = rowItem["noidung"].ToString();
                    item.trangthai = int.Parse(rowItem["trangthai"].ToString());

                    item.ngaytraloi = (string.IsNullOrEmpty(rowItem["ngaytraloi"].ToString()))
                        ? ""
                        : DateTime.Parse(rowItem["ngaytraloi"].ToString()).ToString("dd/MM/yyyy");
                    item.nguoitraloi = rowItem["nguoitraloi"].ToString();
                    item.ngaygui = (string.IsNullOrEmpty(rowItem["ngaygui"].ToString()))
                        ? ""
                        : DateTime.Parse(rowItem["ngaygui"].ToString()).ToString("dd/MM/yyyy");
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

        public SystemMessage BS_ChangeStatus(int id, int id_user, string fullname)
        {
            SystemMessage systemMessage = new SystemMessage();
            DB_CSEntities1 entity = new DB_CSEntities1();
            try
            {
                var checkitem = entity.tbl_lienhe.FirstOrDefault(m => m.id == id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataExisted;
                    return systemMessage;
                }
                else
                {

                    if (checkitem.trangthai == 1)
                    {
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Không thể chuyển trạng thái";
                    }
                    else
                    {
                        checkitem.trangthai = 1;
                        checkitem.id_rep = id_user;
                        checkitem.nguoitraloi = fullname;
                        checkitem.ngaytraloi = DateTime.Now;

                        entity.SaveChanges();
                        systemMessage.IsSuccess = true;
                        systemMessage.Message = "Xác nhận thành công";
                    }

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

    }
}
