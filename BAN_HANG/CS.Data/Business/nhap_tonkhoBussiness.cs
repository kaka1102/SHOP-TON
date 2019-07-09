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
using CS.Data.Untils;

namespace CS.Data.Business
{
    public class nhap_tonkhoBussiness
    {
        SessionUser user = GetSessionBusiness.GetUser();
        public List<ListDetailProduct> LoadAll_TonKho()
        {
            try
            {



                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("nhapkho_danhsach_sanpham", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BranchId", user.BranchId));


                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    ListDetailProduct item = new ListDetailProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.product_name = rowItem["product_name"].ToString();
                    item.TYPE = rowItem["TYPE"].ToString();
                    item.unit_name = rowItem["unit_name"].ToString();
                    item.code = rowItem["code"].ToString();
                    item.isactive = bool.Parse(rowItem["isactive"].ToString());
                    item.soluong = int.Parse(rowItem["soluong"].ToString());

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<chitiettonkho> Soluongchitiet_SP(int id)
        {
            try
            {
                List<chitiettonkho> list = new List<chitiettonkho>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("Soluongchitiet_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@id_center", user.BranchId));

                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    chitiettonkho item = new chitiettonkho();

                    item.tennguon = rowItem["tennguon"].ToString();
                    item.tensp = rowItem["tensp"].ToString();
                    item.masp = rowItem["masp"].ToString();
                    item.giatrungbinh = float.Parse(rowItem["giatrungbinh"].ToString());
                    item.group_code = rowItem["group_code"].ToString();
                    item.soluong = int.Parse(rowItem["soluong"].ToString());

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public SystemMessage BS_CongSL_SP(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkTon =
                    db.sys_tonkho.FirstOrDefault(m => m.id_product == id && m.id_center == user.BranchId);
                if (checkTon == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                checkTon.soluong = checkTon.soluong + 1;
                db.SaveChanges();

                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.UpdateSuccess;


                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }

        public SystemMessage BS_Tru_SL_SP(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkTon =
                    db.sys_tonkho.FirstOrDefault(m => m.id_product == id && m.id_center == user.BranchId);
                if (checkTon == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                checkTon.soluong = checkTon.soluong - 1;
                db.SaveChanges();

                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.UpdateSuccess;


                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }




        public SystemMessage BS_CongSL_Chil_SP(int id_p, string groupcode)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkTon =
                    db.sys_code_product.FirstOrDefault(m => m.id_product == id_p && m.group_code == groupcode && m.id_center == user.BranchId);


                if (checkTon == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }


                float gtb = 0;
                float.TryParse(checkTon.giatrungbinh.ToString(), out gtb);
                int id_n = 0;
                int.TryParse(checkTon.id_nhap.ToString(), out id_n);

                sys_code_product item = new sys_code_product();
                item.code = "12312312";
                item.id_product = id_p;
                item.ngaynhap = DateTime.Now;
                item.trangthai = 0;
                item.id_nhap = id_n;
                item.id_center = user.BranchId;
                item.giatrungbinh = gtb;
                item.group_code = groupcode;
                item.isactive = true;
                item.note_code = "update tang";
                db.sys_code_product.Add(item);


                var checkNhap = db.sys_Nhap.FirstOrDefault(m => m.id == id_n);
                if (checkNhap == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                int sl = 0;
                int.TryParse(checkNhap.soluong.ToString(), out sl);

                checkNhap.soluong = sl + 1;

                db.SaveChanges();



                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.UpdateSuccess;


                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }



        public SystemMessage BS_Tru_SL_Chil_SP(int id_p, string groupcode)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkTon =
                    db.sys_code_product.FirstOrDefault(m => m.id_product == id_p && m.group_code == groupcode && m.id_center == user.BranchId && m.trangthai == 0 && m.isactive == true);


                if (checkTon == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }


                float gtb = 0;
                float.TryParse(checkTon.giatrungbinh.ToString(), out gtb);
                int id_n = 0;
                int.TryParse(checkTon.id_nhap.ToString(), out id_n);

                checkTon.trangthai = 1;
                checkTon.note_code = "update giam";


                var checkNhap = db.sys_Nhap.FirstOrDefault(m => m.id == id_n);
                if (checkNhap == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                int sl = 0;
                int.TryParse(checkNhap.soluong.ToString(), out sl);


                if (sl == 0)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Không thể trừ âm ";
                    return systemMessage;
                }
                checkNhap.soluong = checkNhap.soluong - 1;

                db.SaveChanges();



                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.UpdateSuccess;


                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }


        public Return_lichsu_nhap LoadLichSu_Nhap(string searchValue, int currPage, int recodperpage, int id_product)
        {
            try
            {
                Return_lichsu_nhap result = new Return_lichsu_nhap();
                List<lichsu_nhap> list = new List<lichsu_nhap>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("lichsu_nhaphang", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id_center", user.BranchId));
                cmd.Parameters.Add(new SqlParameter("@id_product", id_product));
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

                    lichsu_nhap item = new lichsu_nhap();
                    item.id = int.Parse(rowItem["id"].ToString());
                    item.RowNum = int.Parse(rowItem["RowNum"].ToString());
                    item.ngaynhap = rowItem["ngaynhap"].ToString();
                    item.soluong = int.Parse(rowItem["soluong"].ToString());
                    item.nguoinhap = rowItem["nguoinhap"].ToString();
                    item.tensp = rowItem["tensp"].ToString();
                    item.tongtien = float.Parse(rowItem["tongtien"].ToString());
                    item.giatrungbinh = float.Parse(rowItem["giatrungbinh"].ToString());
                    item.tennguon = rowItem["tennguon"].ToString();
                    item.id_nguonnhap = rowItem["id_nguonnhap"].ToString();
                    item.tendaily = rowItem["tendaily"].ToString();
                    item.id_center = int.Parse(rowItem["id_center"].ToString());

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

        public List<lichsu_nhap> bs_Xemchitiet_lichsu_sp(int id)
        {
            try
            {
                List<lichsu_nhap> list = new List<lichsu_nhap>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("bs_Xemchitiet_lichsu_sp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));

                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {

                    lichsu_nhap item = new lichsu_nhap();
                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_code_p = int.Parse(rowItem["id_code_p"].ToString());
                    item.RowNum = int.Parse(rowItem["RowNum"].ToString());
                    item.ngaynhap = rowItem["ngaynhap"].ToString();

                    item.barcod_sp = rowItem["barcod_sp"].ToString();

                    item.trangthai = int.Parse(rowItem["trangthai"].ToString());
                    item.group_code = rowItem["group_code"].ToString();
                    item.note_code = rowItem["note_code"].ToString();
                    item.ngayxuat = rowItem["ngayxuat"].ToString();

                    item.nguoixuat = rowItem["nguoixuat"].ToString();
                    item.tenKH = rowItem["tenKH"].ToString();

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public SystemMessage BS_NhapKho(sys_Nhap data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var checkSP =
                    db.sys_product.FirstOrDefault(m => m.id == data.id_product);

                if (checkSP == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.ProductNotExisted;
                    return systemMessage;


                }
                var checkNguon =
                    db.sys_nguonnhap.FirstOrDefault(m => m.id == data.id_nguonnhap);

                if (checkNguon == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.NguonNotExit;
                    return systemMessage;
                }


                // cộng tồn kho
                var checkTonkho = db.sys_tonkho.FirstOrDefault(m =>
                    m.id_product == data.id_product && m.id_center == data.id_center);

                // chưa có
                if (checkTonkho == null)
                {
                    sys_tonkho sys_ton = new sys_tonkho();
                    sys_ton.id_product = data.id_product;
                    sys_ton.soluong = data.soluong;
                    sys_ton.isactive = true;
                    sys_ton.id_center = data.id_center;
                    db.sys_tonkho.Add(sys_ton);
                }
                else  // đã có thì cộng dồn
                {
                    int sl_ton = 0;
                    sl_ton = checkTonkho.soluong;
                    checkTonkho.soluong = sl_ton + data.soluong;
                }

                // nhập nhóm hàng
                sys_Nhap nhap = new sys_Nhap();
                nhap.ngaynhap = data.ngaynhap;
                nhap.soluong = data.soluong;
                nhap.nguoinhap = data.nguoinhap;
                nhap.id_product = data.id_product;
                nhap.tongtien = data.tongtien;
                nhap.giatrungbinh = data.giatrungbinh;
                nhap.trangthai = true;
                nhap.id_center = data.id_center;
                nhap.id_nguonnhap = data.id_nguonnhap;
                nhap.group_code_product = data.group_code_product;
                nhap.mota = data.mota;
                db.sys_Nhap.Add(nhap);
                db.SaveChanges();

                int id_n = 0;
                id_n = nhap.id;

                // nhập chi tiết sinh code cho từng sp
                for (int i = 0; i < data.soluong; i++)
                {
                    sys_code_product itemDetail = new sys_code_product();
                    itemDetail.code = data.group_code_product + "-" + GetBillNumer();
                    itemDetail.id_product = data.id_product;
                    itemDetail.ngaynhap = DateTime.Now;
                    itemDetail.trangthai = 0;  // 0 chua ban, 1 da ban
                    itemDetail.id_nhap = id_n;
                    itemDetail.id_center = data.id_center;
                    itemDetail.giatrungbinh = data.giatrungbinh;
                    itemDetail.group_code = data.group_code_product;
                    itemDetail.isactive = true;
                    db.sys_code_product.Add(itemDetail);
                }



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
        public long GetBillNumer()
        {
            try
            {
                var db = new DB_CSEntities1();
                var result = db.Database.SqlQuery<Number>("[GetSeqNumberOfBill]").FirstOrDefault();
                return result.number;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public class Number
        {
            public Int64 number { get; set; }
        }
    }
}
