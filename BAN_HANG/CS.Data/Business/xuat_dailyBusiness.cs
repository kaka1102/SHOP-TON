using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Const;
using CS.Common.Untils;
using CS.Data.DataContext;
using CS.Data.Model;

namespace CS.Data.Business
{
    public class xuat_dailyBusiness
    {
        DB_CSEntities1 db = new DB_CSEntities1();
        public Branch BS_LoadInforCenterById(int id_center)
        {
            try
            {
                var data = db.Branch.FirstOrDefault(m => m.Id == id_center);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public long GetBillNumer()
        {
            try
            {
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

        public string ValidateAddBillProduct(string data)
        {
            string rs = null;
            var vld = new ValidateUtils();

            if (string.IsNullOrEmpty(data))
            {
                return SystemMessageConst.systemmessage.NoData;
            }
            return null;
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
        public SystemMessage AddBillProduct(Add_Bill item_bill)
        {
            SystemMessage systemMessage = new SystemMessage();
            service_customer_bill serviceCustomerBill = new service_customer_bill();

            try
            {

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

                var check = ValidateAddBillProduct(item_bill.data);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var checkTypeBill = db.sys_parameter.FirstOrDefault(ob =>
                     ob.code == SystemMessageConst.Key.StatusBillForm && ob.value == item_bill.billType);
                if (checkTypeBill == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Không tồn tại hình thức thanh toán !";
                    return systemMessage;
                }


                var arrData = item_bill.data.Split(',');
                if (arrData.Length == 0)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Bạn chưa chọn sản phẩm !";
                    return systemMessage;
                }

                var checkNumber =
                    db.service_customer_bill.FirstOrDefault(
                        ob => ob.code == item_bill.billNumber);
                if (checkNumber != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Số hóa đơn đã tồn tại !";
                    return systemMessage;
                }

                // TAO BILL
                serviceCustomerBill.type = item_bill.type;// 1,2,3 = DL,BL,HD
                serviceCustomerBill.billtype = item_bill.loaibill;  // 1,2,3 = DL,BL,HD

                serviceCustomerBill.id_customer = TTKH.id;
                serviceCustomerBill.tenKH = TTKH.fullname;
                serviceCustomerBill.tongtien = item_bill.total;
                serviceCustomerBill.giamgia = item_bill.bonusTotal;
                serviceCustomerBill.thucnhan = item_bill.discount;
                serviceCustomerBill.method = item_bill.billType;
                serviceCustomerBill.ispay = 0; //chưa thanh toán
                serviceCustomerBill.created_at = DateTime.Now;
                serviceCustomerBill.created_by = item_bill.tennguoithaotac;
                serviceCustomerBill.id_nguoitao = item_bill.id_thaotac;
                serviceCustomerBill.deleted = 0;
                serviceCustomerBill.status = 0;
                serviceCustomerBill.date_sale = DateTime.Now;
                serviceCustomerBill.id_center = item_bill.id_center;
                serviceCustomerBill.tennguoixuat = item_bill.nguoixuat;
                serviceCustomerBill.code = item_bill.billNumber;
                serviceCustomerBill.note = item_bill.notebill;

                db.service_customer_bill.Add(serviceCustomerBill);
                db.SaveChanges();


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
                        service_customer_bill_detail serviceCustomerBillDetail = new service_customer_bill_detail();
                        serviceCustomerBillDetail.id_bill = serviceCustomerBill.id;
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
                        db.SaveChanges();


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
                            lstCodeProduct[i].id_bill_dt = serviceCustomerBillDetail.id;
                        }
                        db.SaveChanges();


                    }
                }

                systemMessage.id_bill = serviceCustomerBill.id;
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                return systemMessage;
            }
            catch (Exception e)
            {
                var errBill = db.service_customer_bill.FirstOrDefault(ob => ob.id == serviceCustomerBill.id);
                if (errBill != null)
                {

                    var checkBillDetaiErr = db.service_customer_bill_detail
                        .Where(m => m.id_bill == serviceCustomerBill.id).ToList();
                    if (checkBillDetaiErr.Count > 0)
                    {
                        foreach (var itemErrDT in checkBillDetaiErr)
                        {
                            // xoa ton kho theo bill_detai

                            int id_p = 0, quantity = 0, id = 0;
                            id_p = itemErrDT.id_product.Value;
                            quantity = itemErrDT.quantity.Value;
                            id = itemErrDT.id;


                            var checkTonkho = db.sys_tonkho.FirstOrDefault(m =>
                                m.id_product == id_p && m.id_center == item_bill.id_center && m.isactive == true);
                            int sl_ton = 0;
                            sl_ton = checkTonkho.soluong;
                            checkTonkho.soluong = sl_ton + quantity;


                            // update lai bang code product
                            var lst_codeP = db.sys_code_product.Where(m => m.id_bill_dt == id && m.id_product == id_p).ToList();
                            foreach (var item_old in lst_codeP)
                            {
                                item_old.trangthai = 0;
                                item_old.ngayxuat = null;
                                item_old.nguoixuat = null;
                                item_old.tenKH = null;
                                item_old.id_bill_dt = 0;
                            }

                            db.service_customer_bill_detail.Remove(itemErrDT);
                        }
                    }
                    db.service_customer_bill.Remove(errBill);
                }
                db.SaveChanges();
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }

        }

        public service_customer ThongTinKH_TheoIdBill(int id_bill)
        {
            try
            {
                int id_cus = 0;
                var getid_cus = db.service_customer_bill.FirstOrDefault(m => m.id == id_bill);
                if (getid_cus != null)
                {
                    id_cus = getid_cus.id_customer.Value;
                }

                var data = db.service_customer.FirstOrDefault(m => m.id == id_cus);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public service_customer_bill ThongTinDonHangTheoID_Bill(int id_bill)
        {
            try
            {
                var data = db.service_customer_bill.FirstOrDefault(m => m.id == id_bill);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<service_customer_bill_detail> ChiTietSanPhamTheoID_Bill(int id_bill)
        {
            try
            {
                var data = db.service_customer_bill_detail.Where(m => m.id_bill == id_bill).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Branch ThongTinDaiLy(int id)
        {
            var data = db.Branch.FirstOrDefault(m => m.Id == id);
            return data;
        }
        public SystemMessage Xoasanphamkhoidonhang(int id_bill_detail)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var data = db.service_customer_bill_detail.FirstOrDefault(m => m.id == id_bill_detail);
                if (data == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Bill detail không tồn tại";
                    return systemMessage;
                }

                int id_bill = 0, id_product = 0, quantity = 0;
                double gia = 0;
                id_bill = data.id_bill.Value;
                id_product = data.id_product.Value;
                quantity = data.quantity.Value;
                gia = data.price.Value;


                var Bill = db.service_customer_bill.FirstOrDefault(m => m.id == id_bill);

                if (Bill == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Bill không tồn tại";
                    return systemMessage;
                }

                int id_center = 0;
                double gia_bill = 0, giam_gia = 0;
                gia_bill = Bill.tongtien.Value;
                giam_gia = Bill.giamgia.Value;
                id_center = Bill.id_center.Value;

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

                double tonggia = 0;
                double gia_moi = 0;
                double tonggia_moi = 0;

                tonggia = gia * quantity;
                gia_moi = gia_bill - tonggia;
                if (giam_gia <=100 && giam_gia >0)
                {
                    tonggia_moi = gia_moi - ((gia_moi * giam_gia) / 100);
                }
                else
                {
                    tonggia_moi = gia_moi - giam_gia;
                }
             

                Bill.tongtien = gia_moi;
                Bill.thucnhan = tonggia_moi;

                // xoa bill detail
                db.service_customer_bill_detail.Remove(data);

                db.SaveChanges();

                systemMessage.id_bill = id_bill_detail;
                systemMessage.IsSuccess = true;
                systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = "Dữ liệu không hợp lệ";
                return systemMessage;
            }
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
                //checkBillInvail.type = item_bill.type;
                //checkBillInvail.billtype = item_bill.loaibill;
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
                checkBillInvail.note = item_bill.notebill;

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
    }
}
