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
    public class danhsachphieuxuatBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnListBill BS_LoadListBill(string searchValue, int currPage, int recodperpage, string date_sale, int status, int id_center)
        {
            try
            {
                ReturnListBill result = new ReturnListBill();
                List<InforBill> list = new List<InforBill>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("BS_LoadListBill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
                cmd.Parameters.Add(new SqlParameter("@currPage", currPage));
                cmd.Parameters.Add(new SqlParameter("@recodperpage", 10));
                cmd.Parameters.Add(new SqlParameter("@id_center", id_center));
                cmd.Parameters.Add(new SqlParameter("@date_sale", date_sale));
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                int total = Convert.ToInt16(cmd.Parameters["@totalCount"].Value);

                foreach (DataRow rowItem in dt.Rows)
                {
                    InforBill item = new InforBill();

                    item.RowNum = int.Parse(rowItem["RowNum"].ToString());
                    item.id = int.Parse(rowItem["id"].ToString());
                    item.type = rowItem["type"].ToString();
                    item.id_customer = int.Parse(rowItem["id_customer"].ToString());
                    item.tenKH = rowItem["tenKH"].ToString();

                    item.method = rowItem["method"].ToString();
                    item.ispay = int.Parse(rowItem["ispay"].ToString());
                    item.created_by = rowItem["created_by"].ToString();
                    item.tennguoixuat = rowItem["tennguoixuat"].ToString();
                    item.date_sale = (string.IsNullOrEmpty(rowItem["date_sale"].ToString())) ? "" : DateTime.Parse(rowItem["date_sale"].ToString()).ToString("dd/MM/yyyy");
                    item.status = int.Parse(rowItem["status"].ToString());

                    item.mobile = rowItem["mobile"].ToString();
                    item.address = rowItem["address"].ToString();
                    item.code = rowItem["code"].ToString();
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

        public SystemMessage BS_Huydonhang(int id_b, string nguoithaotac)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var Bill = db.service_customer_bill.FirstOrDefault(m => m.id == id_b);

                if (Bill == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đơn hàng không tồn tại";
                    return systemMessage;
                }


                var get_detail = db.service_customer_bill_detail.Where(m => m.id_bill == id_b).ToList();

                foreach (var item in get_detail)
                {

                    int id_product = 0, id_center = 0, quantity = 0, id_bill_detail = 0;

                    id_product = item.id_product.Value;
                    id_center = Bill.id_center.Value;
                    quantity = item.quantity.Value;
                    id_bill_detail = item.id;

                    // cộng lại số lượng tồn
                    var tonkho = db.sys_tonkho.FirstOrDefault(m => m.id_product == id_product && m.id_center == id_center);
                    if (tonkho == null)
                    {
                        systemMessage.IsSuccess = false;
                        systemMessage.Message = "Lỗi dữ liệu tồn kho";
                        return systemMessage;
                    }

                    int sl_ton = 0;
                    sl_ton = tonkho.soluong;
                    tonkho.soluong = sl_ton + quantity;

                    // update lai bang code product
                    var lst_codeP = db.sys_code_product.Where(m => m.id_bill_dt == id_bill_detail && m.id_product == id_product).ToList();
                    foreach (var item_old in lst_codeP)
                    {
                        item_old.trangthai = 0;
                        item_old.ngayxuat = null;
                        item_old.nguoixuat = null;
                        item_old.tenKH = null;
                        item_old.id_bill_dt = 0;
                    }

                    // update tt ve huy don hang
                    item.status = 2;
                }

                Bill.status = 2;
                Bill.updated_at = DateTime.Now;
                Bill.updated_by = nguoithaotac;


                db.SaveChanges();

                systemMessage.id_bill = id_b;
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.CancesSaleOK;

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Dữ liệu không hợp lệ";
                return systemMessage;
            }
        }
        public string CheckThongTinKhachHang(Add_Bill item)
        {
            string rs = null;
            if (string.IsNullOrEmpty(item.tenkh))
            {
                return rs = "Tên khách hàng không được để trống";
            }
            if (string.IsNullOrEmpty(item.dienthoai))
            {
                return rs = "SĐT khách hàng không được để trống";
            }

            var checkKH =
                db.service_customer.FirstOrDefault(m => m.fullname == item.tenkh && m.mobile == item.dienthoai);
            if (checkKH == null)
            {
                service_customer inew = new service_customer();
                inew.fullname = item.tenkh;
                inew.mobile = item.dienthoai;
                inew.address = item.diachi;
                inew.id_center = item.id_center;
                inew.code_center = item.code_center;
                inew.created_at = DateTime.Now;
                inew.created_by = item.tennguoithaotac;
                inew.status = 1;
                inew.note = "Thêm mới khách hàng khi bán đại lý";
                db.service_customer.Add(inew);
                db.SaveChanges();
            }

            return null;
        }
        public SystemMessage EditBillProduct(Add_Bill item_bill)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var checkBillInvail = db.service_customer_bill.FirstOrDefault(m => m.id == item_bill.id_bill);
                if (checkBillInvail == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Không tìm thấy đơn hàng";
                    return systemMessage;
                }

                var checkkh = CheckThongTinKhachHang(item_bill);
                if (checkkh != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = checkkh;
                    return systemMessage;
                }

                var TTKH = db.service_customer.FirstOrDefault(m => m.mobile == item_bill.dienthoai && m.fullname == item_bill.tenkh && m.id_center == item_bill.id_center);

                if (TTKH == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Thông tin khách hàng không tồn tại";
                    return systemMessage;
                }

                //var check = ValidateAddBillProduct(item_bill.data);
                //if (check != null)
                //{
                //    systemMessage.IsSuccess = false;
                //    systemMessage.Message = check;
                //    return systemMessage;
                //}

                var checkTypeBill = db.sys_parameter.FirstOrDefault(ob =>
                     ob.code == SystemMessageConst.Key.StatusBillForm && ob.value == item_bill.billType);
                if (checkTypeBill == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Không tồn tại hình thức thanh toán !";
                    return systemMessage;
                }


                var arrData = item_bill.data.Split(',');
                //if (arrData.Length == 0)
                //{
                //    systemMessage.IsSuccess = false;
                //    systemMessage.Message = "Bạn chưa chọn sản phẩm !";
                //    return systemMessage;
                //}

                var checkNumber =
                    db.service_customer_bill.FirstOrDefault(
                        ob => ob.code == item_bill.billNumber && ob.id != item_bill.id_bill);
                if (checkNumber != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Số hóa đơn đã tồn tại !";
                    return systemMessage;
                }

                // TAO BILL
                //    checkBillInvail.type = "DL";
                //    checkBillInvail.billtype = 1;
                checkBillInvail.id_customer = TTKH.id;
                checkBillInvail.tenKH = TTKH.fullname;
                checkBillInvail.tongtien = item_bill.total;
                checkBillInvail.giamgia = item_bill.bonusTotal;
                checkBillInvail.thucnhan = item_bill.discount;
                checkBillInvail.method = item_bill.billType;
                checkBillInvail.ispay = 0; //chưa thanh toán
                checkBillInvail.updated_at = DateTime.Now;
                checkBillInvail.updated_by = item_bill.tennguoithaotac;
                checkBillInvail.deleted = 0;
                checkBillInvail.status = 0;
                checkBillInvail.date_sale = DateTime.Now;
                checkBillInvail.id_center = item_bill.id_center;
                checkBillInvail.tennguoixuat = item_bill.nguoixuat;
                checkBillInvail.code = item_bill.billNumber;

                var lstProduct = db.sys_product.ToList();

                foreach (var item in arrData)
                {
                    if (item.Contains("*"))
                    {
                        var productInfo = item.Split('*');

                        int id_product = 0;
                        int quantity = 0;
                        long price = 0;
                        string group_code_product = "";
                        int id_unit = 0;
                        string unit_name = "";



                        Int32.TryParse(productInfo[0], out id_product);
                        Int32.TryParse(productInfo[1], out quantity);
                        long.TryParse(productInfo[2], out price);
                        group_code_product = productInfo[3];
                        Int32.TryParse(productInfo[4], out id_unit);
                        unit_name = productInfo[5];

                        if (quantity < 1)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Số lượng sản phẩm không hợp lệ";
                            return systemMessage;
                        }

                        var checkProduct = lstProduct.FirstOrDefault(ob => ob.id == id_product);
                        if (checkProduct == null)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Sản phẩm không tồn tại ";
                            return systemMessage;
                        }


                        var checktrung = db.service_customer_bill_detail.FirstOrDefault(m =>
                            m.id_product == id_product && m.groupcodeproduct == group_code_product &&
                            m.id_bill == item_bill.id_bill);

                        int new_id_bill_detail = 0;


                        if (checktrung == null)
                        {
                            service_customer_bill_detail serviceCustomerBillDetail = new service_customer_bill_detail();

                            serviceCustomerBillDetail.id_bill = item_bill.id_bill;
                            serviceCustomerBillDetail.id_product = id_product;
                            serviceCustomerBillDetail.price = price;
                            serviceCustomerBillDetail.quantity = quantity;
                            serviceCustomerBillDetail.unit = id_unit;
                            serviceCustomerBillDetail.unit_name = unit_name;
                            serviceCustomerBillDetail.status = 0;
                            serviceCustomerBillDetail.create_at = DateTime.Now;
                            serviceCustomerBillDetail.product_name = checkProduct.name;
                            serviceCustomerBillDetail.groupcodeproduct = group_code_product;

                            db.service_customer_bill_detail.Add(serviceCustomerBillDetail);
                            new_id_bill_detail = serviceCustomerBillDetail.id;
                            db.SaveChanges();
                        }
                        else
                        {
                            int sl = 0;
                            sl = checktrung.quantity.Value;

                            checktrung.quantity = sl + quantity;
                            new_id_bill_detail = checktrung.id;
                        }



                        // tru ton kho

                        var checkTonkho = db.sys_tonkho.Where(m =>
                            m.id_product == id_product && m.id_center == item_bill.id_center && m.isactive == true).FirstOrDefault();

                        if (checkTonkho == null)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Sản phẩm không tồn tại trong kho !!";
                            return systemMessage;
                        }

                        if (checkTonkho.soluong == 0)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Sản phẩm đã hết, không thể tạo phiếu xuất !!";
                            return systemMessage;
                        }

                        if (checkTonkho.soluong < quantity)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Số lượng xuất nhiều hơn số lượng tồn !!";
                            return systemMessage;
                        }

                        int sl_ton = 0;
                        sl_ton = checkTonkho.soluong;
                        checkTonkho.soluong = sl_ton - quantity;

                        // đổi trang thai từng sp
                        var lstCodeProduct = db.sys_code_product.Where(m =>
                            m.group_code == group_code_product && m.id_product == id_product &&
                            m.id_center == item_bill.id_center && m.trangthai == 0).ToList();

                        if (lstCodeProduct.Count == 0)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Dữ liệu nhớm sản phẩm không tồn tại";
                            return systemMessage;
                        }
                        if (lstCodeProduct.Count < quantity)
                        {
                            systemMessage.IsSuccess = false;
                            systemMessage.Message = "Số lượng nhóm sản phẩm còn lại không hợp lệ";
                            return systemMessage;
                        }

                        for (int i = 0; i < quantity; i++)
                        {
                            lstCodeProduct[i].ngayxuat = DateTime.Now;
                            lstCodeProduct[i].nguoixuat = item_bill.nguoixuat;
                            lstCodeProduct[i].tenKH = item_bill.tenkh;
                            lstCodeProduct[i].trangthai = 1;
                            lstCodeProduct[i].id_bill_dt = new_id_bill_detail;
                        }
                    }
                }

                db.SaveChanges();
                systemMessage.id_bill = item_bill.id_bill;
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

        public SystemMessage BS_Hoantatdonhang(int id_b, string nguoithaotac)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var Bill = db.service_customer_bill.FirstOrDefault(m => m.id == id_b);

                if (Bill == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đơn hàng không tồn tại";
                    return systemMessage;
                }


                var get_detail = db.service_customer_bill_detail.Where(m => m.id_bill == id_b).ToList();

                foreach (var item in get_detail)
                {
                    int id_p = 0, quantity = 0;

                    id_p = item.id_product.Value;
                    quantity = item.quantity.Value;

                    var getProduct = db.sys_product.FirstOrDefault(m => m.id == id_p);
                    if (getProduct != null)
                    {
                        getProduct.soluongban += quantity;
                    }

                    // update tt ve hoan tat don hang
                    item.status = 1;
                }

                // update tt ve hoan tat don hang va doi trang thai thanh toan

                Bill.status = 1;
                Bill.ispay = 1;
                Bill.updated_at = DateTime.Now;
                Bill.updated_by = nguoithaotac;


                db.SaveChanges();

                systemMessage.id_bill = id_b;
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.CompleteSaleOK;

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Dữ liệu không hợp lệ";
                return systemMessage;
            }
        }
    }
}
