using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using BAN_HANG.Areas.Client.Models;
using CS.Data.Connection;
using CS.Data.DataContext;
using CS.Data.Model;

namespace BAN_HANG.Areas.Client
{
    public class HomeClientBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public List<LstNewProduct> BS_LoadListNewProduct()
        {
            try
            {
                List<LstNewProduct> list = new List<LstNewProduct>();

                SqlConnection con = new SqlConnection();
                con = CS.Data.Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("BS_LoadListNewProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    LstNewProduct item = new LstNewProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.name = rowItem["name"].ToString();
                    item.code = rowItem["code"].ToString();

                    item.description = rowItem["description"].ToString();
                    item.path_img = rowItem["path_img"].ToString();
                    item.description_img = rowItem["description_img"].ToString();
                    item.gia_xuat = float.Parse(rowItem["gia_xuat"].ToString());

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ReturnDSLoaiSP> BS_LoadLoaiSanPham()
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
                        SqlCommand cmd = new SqlCommand("LoadProductByCategory", con);
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

        public ChitietSP BS_ChitietSP(int id)
        {
            try
            {
                ChitietSP ct_sp = new ChitietSP();

                var _id = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var _id2 = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };
                var _id3 = new SqlParameter
                {
                    ParameterName = "id",
                    Value = id
                };

                ct_sp = db.Database.SqlQuery<ChitietSP>(
                    "thongtinsp_theoid @id", _id).FirstOrDefault();

                if (ct_sp != null)
                {
                    List<ImgProduct> lst = new List<ImgProduct>();
                    lst = db.Database.SqlQuery<ImgProduct>("ds_anhsp_theoid_sp @id", _id2).ToList();
                    ct_sp.LstImg = lst;


                    List<AttrProduct> lstAttr = new List<AttrProduct>();
                    lstAttr = db.Database.SqlQuery<AttrProduct>("ds_thuoctinh_sp_theoid_sp @id", _id3).ToList();
                    ct_sp.LstAttrProduct = lstAttr;
                }

                return ct_sp;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<LstNewProduct> BS_LoadSPLienquan(int id)
        {
            try
            {
                List<LstNewProduct> list = new List<LstNewProduct>();

                SqlConnection con = new SqlConnection();
                con = CS.Data.Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("LoadSPlienquan_theoid_sp", con);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    LstNewProduct item = new LstNewProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_cate = int.Parse(rowItem["id_cate"].ToString());
                    item.name = rowItem["name"].ToString();
                    item.code = rowItem["code"].ToString();

                    item.description = rowItem["description"].ToString();
                    item.path_img = rowItem["path_img"].ToString();
                    item.description_img = rowItem["description_img"].ToString();
                    item.gia_xuat = float.Parse(rowItem["gia_xuat"].ToString());

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<tbl_LoaiSP> BS_LoadDSLoaiSP()
        {
            try
            {
                var data = db.tbl_LoaiSP.Where(m => m.isactive == true).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<LstNewProduct> BS_Sanphamkhac(int id)
        {
            try
            {
                List<LstNewProduct> list = new List<LstNewProduct>();

                SqlConnection con = new SqlConnection();
                con = CS.Data.Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("Load_sanphamkhac_category_theoid_sp", con);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    LstNewProduct item = new LstNewProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_cate = int.Parse(rowItem["id_cate"].ToString());
                    item.name = rowItem["name"].ToString();
                    item.code = rowItem["code"].ToString();

                    item.description = rowItem["description"].ToString();
                    item.path_img = rowItem["path_img"].ToString();
                    item.description_img = rowItem["description_img"].ToString();
                    item.gia_xuat = float.Parse(rowItem["gia_xuat"].ToString());

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public ReturnDSLoaiSP LoadDSSP_Theoid_LoaiSP(int id_cate)
        {
            try
            {

                ReturnDSLoaiSP loaisp = new ReturnDSLoaiSP();


                var tt = db.tbl_LoaiSP.FirstOrDefault(m => m.id == id_cate);
                loaisp.id = id_cate;
                loaisp.category_name = tt.category_name;
                loaisp.description = tt.description;
                
                List<ThongtinSP> lstSP = new List<ThongtinSP>();


                SqlConnection con = new SqlConnection();
                con = CS.Data.Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("LoadDSSP_Theoid_LoaiSP", con);
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
               
                return loaisp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}