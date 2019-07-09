using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BAN_HANG.Areas.Client.Models;
using CS.Data.DataContext;

namespace BAN_HANG.Areas.Client.Business
{
    public class ProductBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public List<ReturnDSLoaiSP> BS_LoadAllCategoryAndProduct()
        {
            try
            {
                List<ReturnDSLoaiSP> list = new List<ReturnDSLoaiSP>();


                var getAll = db.tbl_LoaiSP.Where(m => m.isactive == true).ToList();
                if (getAll.Count > 0)
                {
                    foreach (var item in getAll)
                    {
                        ReturnDSLoaiSP loaisp = new ReturnDSLoaiSP();

                        int id_cate = 0;
                        id_cate = item.id;
                        List<ThongtinSP> lstSP = new List<ThongtinSP>();

                        loaisp.category_name = item.category_name;
                        loaisp.id = item.id;
                        loaisp.description = item.description;

                        SqlConnection con = new SqlConnection();
                        con = CS.Data.Connection.Connect.GetConnect();
                        SqlCommand cmd = new SqlCommand("BS_LoadAllCategoryAndProduct", con);
                        cmd.Parameters.Add(new SqlParameter("@id_cate", id_cate));
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);


                        foreach (DataRow rowItem in dt.Rows)
                        {
                            ThongtinSP item_chil = new ThongtinSP();

                            item_chil.id = int.Parse(rowItem["id"].ToString());
                            item_chil.id_cate = int.Parse(rowItem["id_cate"].ToString());
                            item_chil.id_cate_detail = int.Parse(rowItem["id_cate_detail"].ToString());
                            item_chil.name = rowItem["name"].ToString();
                            item_chil.code = rowItem["code"].ToString();

                            item_chil.description = rowItem["description"].ToString();
                            item_chil.path_img = rowItem["path_img"].ToString();
                            item_chil.description_img = rowItem["description_img"].ToString();
                            item_chil.gia_xuat = float.Parse(rowItem["gia_xuat"].ToString());
                            lstSP.Add(item_chil);

                        }

                        loaisp.LstSP = lstSP;
                        list.Add(loaisp);
                    }

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