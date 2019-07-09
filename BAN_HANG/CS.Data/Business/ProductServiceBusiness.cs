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
    public class ProductServiceBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();

        public List<ListDetailProduct> Hienthithongtinchitiet_sp_thongsokt(int id)
        {
            try
            {

                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("hienthithongtinchitiet_sp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));


                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    ListDetailProduct item = new ListDetailProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_attr = int.Parse(rowItem["id_attr"].ToString());
                    item.id_config_attr = int.Parse(rowItem["id_config_attr"].ToString());
                    item.attr_name = rowItem["attr_name"].ToString();
                    item.descripton_attr = rowItem["descripton_attr"].ToString();

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ListDetailProduct> Hienthithongtinchitiet_sp_ds_anh(int id)
        {
            try
            {

                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("ds_anh_theoid_sp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));


                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    ListDetailProduct item = new ListDetailProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_img = (string.IsNullOrEmpty(rowItem["id_img"].ToString()) ? 0 : int.Parse(rowItem["id_img"].ToString()));

                    item.url_img = rowItem["url_img"].ToString();
                    item.path_img = rowItem["path_img"].ToString();
                    item.des_img = rowItem["des_img"].ToString();

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ListDetailProduct> Hienthithongtinchitiet_sp_ds_gia(int id)
        {
            try
            {

                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("danhsach_gia_theoid_sp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));


                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    ListDetailProduct item = new ListDetailProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.id_gia = (string.IsNullOrEmpty(rowItem["id_gia"].ToString()) ? 0 : int.Parse(rowItem["id_gia"].ToString()));

                    item.gia_nhap = float.Parse(rowItem["gia_nhap"].ToString());
                    item.gia_xuat = float.Parse(rowItem["gia_xuat"].ToString());
                    item.name_display = rowItem["name_display"].ToString();

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }




        public List<ListDetailProduct> GetListProductsByType(string type)
        {
            try
            {

                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("danhsach_sanpham", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@type_product", type));


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

                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public sys_product_detail GetProductDetailByCode(string code)
        {
            try
            {
                var result = db.sys_product_detail.FirstOrDefault(ob => ob.code == code);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public sys_product GetListProductsById(int id, string type)
        {
            try
            {
                var result = db.sys_product.Where(ob => ob.TYPE == type).FirstOrDefault(ob => ob.id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<ListDetailProduct> GetListProductsDetail(int id)
        {
            try
            {

                List<ListDetailProduct> list = new List<ListDetailProduct>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("chitiet_sanpham", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@type_product", SystemMessageConst.Key.Product_Service));


                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow rowItem in dt.Rows)
                {
                    ListDetailProduct item = new ListDetailProduct();

                    item.id = int.Parse(rowItem["id"].ToString());
                    item.product_name = rowItem["product_name"].ToString();
                    item.quantity = int.Parse(rowItem["quantity"].ToString());


                    list.Add(item);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string ValidateAddService(string code, string name, string cycle)
        {
            string rs = null;
            var check = new ValidateUtils();
            rs = check.CheckRequiredField(code, "Mã sản phẩm", 3, 15);
            if (rs != null)
            {
                return rs;
            }
            rs = check.CheckRequiredField(name, "tên service", 3, 100);
            if (rs != null)
            {
                return rs;
            }

            int _cycle = 0;
            if (!Int32.TryParse(cycle, out _cycle))
            {
                return "cycle không đúng định dạng";
            }
            return null;
        }
        //public SystemMessage AddService(string code, string name, string cycle, int status, string username, byte therapystart)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var check = ValidateAddService(code, name, cycle);
        //        if (check != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = check;
        //            return systemMessage;
        //        }

        //        var checkCode = db.sys_product.FirstOrDefault(ob => ob.code == code);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã service đã tồn tại";
        //            return systemMessage;
        //        }
        //        sys_product sysProduct = new sys_product();
        //        sysProduct.code = code;
        //        sysProduct.name = name;
        //        sysProduct.cycle = Int32.Parse(cycle);
        //        sysProduct.isactive = status == 1 ? true : false;
        //        sysProduct.CREATED_DATE = DateTime.Now;
        //        sysProduct.CREATED_BY = username;
        //        sysProduct.isappointment = true;
        //        sysProduct.therapystart = therapystart;
        //        sysProduct.TYPE = SystemMessageConst.Key.Product_Service;
        //        db.sys_product.Add(sysProduct);
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}
        public string ValidateAddRetail(string code, string name, string price, string unit)
        {
            string rs = null;
            var check = new ValidateUtils();
            rs = check.CheckRequiredField(code, "Mã sản phẩm", 3, 15);
            if (rs != null)
            {
                return rs;
            }
            rs = check.CheckRequiredField(name, "tên sản phẩm", 3, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = check.CheckRequiredField(name, "Đơn vị", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            int _price = 0;
            if (!Int32.TryParse(price, out _price))
            {
                return "Gía sản phẩm không đúng định dạng";
            }
            return null;
        }
        //public SystemMessage AddRetail(string code, string name, string price, int status, string username, string unit)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var check = ValidateAddRetail(code, name, price, unit);
        //        if (check != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = check;
        //            return systemMessage;
        //        }

        //        var checkCode = db.sys_product.FirstOrDefault(ob => ob.code == code);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã service đã tồn tại";
        //            return systemMessage;
        //        }
        //        sys_product sysProduct = new sys_product();
        //        sysProduct.code = code;
        //        sysProduct.name = name;
        //        sysProduct.price = Int32.Parse(price);
        //        sysProduct.isactive = status == 1 ? true : false;
        //        sysProduct.CREATED_DATE = DateTime.Now;
        //        sysProduct.CREATED_BY = username;
        //        sysProduct.isappointment = false;
        //        sysProduct.unit = unit;
        //        sysProduct.TYPE = SystemMessageConst.Key.Product_Sale;
        //        db.sys_product.Add(sysProduct);
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}
        //public SystemMessage EditService(int id, string code, string name, string cycle, int status, string username, byte therapystart)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var check = ValidateAddService(code, name, cycle);
        //        if (check != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = check;
        //            return systemMessage;
        //        }
        //        var checkCode = db.sys_product.FirstOrDefault(ob => ob.code == code && ob.id != id);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã service đã tồn tại";
        //            return systemMessage;
        //        }
        //        var checkExisted = db.sys_product.FirstOrDefault(ob => ob.id == id && ob.TYPE == SystemMessageConst.Key.Product_Service);
        //        if (checkExisted == null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Service không tồn tại !!!";
        //            return systemMessage;
        //        }
        //        checkExisted.code = code;
        //        checkExisted.name = name;
        //        checkExisted.cycle = Int32.Parse(cycle);
        //        checkExisted.isactive = status == 1 ? true : false;
        //        checkExisted.MODIFY_DATE = DateTime.Now;
        //        checkExisted.MODIFY_BY = username;
        //        checkExisted.isappointment = true;
        //        checkExisted.therapystart = therapystart;
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}

        //public SystemMessage EditRetail(int id, string code, string name, string price, int status, string username, string unit)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var check = ValidateAddRetail(code, name, price, unit);
        //        if (check != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = check;
        //            return systemMessage;
        //        }
        //        var checkCode = db.sys_product.FirstOrDefault(ob => ob.code == code && ob.id != id);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã sản phẩm đã tồn tại";
        //            return systemMessage;
        //        }
        //        var checkExisted = db.sys_product.FirstOrDefault(ob => ob.id == id && ob.TYPE == SystemMessageConst.Key.Product_Sale);
        //        if (checkExisted == null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "sản phẩm không tồn tại !!!";
        //            return systemMessage;
        //        }
        //        checkExisted.code = code;
        //        checkExisted.name = name;
        //        checkExisted.price = Int32.Parse(price);
        //        checkExisted.isactive = status == 1 ? true : false;
        //        checkExisted.MODIFY_DATE = DateTime.Now;
        //        checkExisted.MODIFY_BY = username;
        //        checkExisted.isappointment = false;
        //        checkExisted.unit = unit;
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}
        //public SystemMessage AddProductDetail(int idproduct, int extend, int number, int price, int cycle, string code, int status, string username)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var checkCode = db.sys_product_detail.FirstOrDefault(ob => ob.code == code);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã chi tiết sản phẩm đã tồn tại";
        //            return systemMessage;
        //        }
        //        sys_product_detail sysProductDetail = new sys_product_detail();
        //        sysProductDetail.name = number + extend + " Lần";
        //        sysProductDetail.EXTEND = extend;
        //        sysProductDetail.NUMBER = number;
        //        sysProductDetail.id_product = idproduct;
        //        sysProductDetail.PRICE = price;
        //        sysProductDetail.cycle = cycle;
        //        sysProductDetail.code = code;
        //        sysProductDetail.isactive = status == 1 ? true : false;
        //        sysProductDetail.CREATED_DATE = DateTime.Now;
        //        sysProductDetail.CREATED_BY = username;
        //        db.sys_product_detail.Add(sysProductDetail);
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
        //        systemMessage.IdResult = sysProductDetail.id;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}
        //public SystemMessage EditProductDetail(int id, int extend, int number, int price, int cycle, string code, int status, string username)
        //{
        //    SystemMessage systemMessage = new SystemMessage();
        //    try
        //    {
        //        var checkDetail = db.sys_product_detail.FirstOrDefault(ob => ob.id == id);
        //        if (checkDetail == null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Chi tiết sản phẩm không tồn tại";
        //            return systemMessage;
        //        }
        //        var checkCode = db.sys_product_detail.FirstOrDefault(ob => ob.code == code && ob.id != id);
        //        if (checkCode != null)
        //        {
        //            systemMessage.IsSuccess = false;
        //            systemMessage.Message = "Mã chi tiết sản phẩm đã tồn tại";
        //            return systemMessage;
        //        }

        //        checkDetail.name = number + extend + " Lần";
        //        checkDetail.EXTEND = extend;
        //        checkDetail.NUMBER = number;
        //        checkDetail.PRICE = price;
        //        checkDetail.cycle = cycle;
        //        checkDetail.code = code;
        //        checkDetail.isactive = status == 1 ? true : false;
        //        checkDetail.CREATED_DATE = DateTime.Now;
        //        checkDetail.CREATED_BY = username;
        //        db.SaveChanges();
        //        systemMessage.IsSuccess = true;
        //        systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
        //        return systemMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        systemMessage.IsSuccess = false;
        //        systemMessage.Message = e.ToString();
        //        return systemMessage;
        //    }
        //}


        public SystemMessage BS_Themmoi_thongso(int id_attr, string value_attr, int id_product)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var checkAttr = db.sys_attributes.FirstOrDefault(m => m.id == id_attr);
                if (checkAttr == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.thongsokhongtontai;
                    return systemMessage;
                }

                var checkAttrInval =
                    db.sys_attr_product.FirstOrDefault(m => m.id_attr == id_attr && m.id_product == id_product);

                if (checkAttrInval != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.tontaits;
                    return systemMessage;
                }

                if (string.IsNullOrEmpty(value_attr))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mô tả không được để trống và < 500 ký tự";
                    return systemMessage;
                }

                sys_attr_product item1 = new sys_attr_product();
                item1.id_attr = id_attr;
                item1.id_product = id_product;
                item1.isactive = true;
                item1.descripton_attr = value_attr;
                db.sys_attr_product.Add(item1);


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
        public SystemMessage BS_XoathongsoTheoSP(int id_config_attr)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();

                var check = db.sys_attr_product.FirstOrDefault(m => m.id == id_config_attr);

                if (check != null)
                {
                    db.sys_attr_product.Remove(check);
                    db.SaveChanges();


                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                }
                else
                {
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                }

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }
        public SystemMessage BS_ChinhSua_Thongso_kythuat(int id_attr, int id_cf_attr, string value_attr, int id_product)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var checkAttr = db.sys_attr_product.FirstOrDefault(m =>
                    m.id == id_cf_attr && m.id_attr == id_attr && m.id_product == id_product);
                if (checkAttr == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                if (string.IsNullOrEmpty(value_attr))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mô tả không được để trống và < 500 ký tự";
                    return systemMessage;
                }

                checkAttr.descripton_attr = value_attr;

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




        public SystemMessage BS_ThemMoi_Gia_SP(int gianhap, int giaxuat, string type_price, int id_product)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var CheckTypePrice =
                    db.sys_parameter.FirstOrDefault(m => m.value == type_price && m.code == "PRICE_PRODUCT");
                if (CheckTypePrice == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Type_Price_NotExit;
                    return systemMessage;
                }

                var checkInvail =
                    db.tbl_price.FirstOrDefault(m => m.id_product == id_product && m.type_price == type_price && m.isactive == true);

                if (checkInvail != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Price_Exit;
                    return systemMessage;
                }


                tbl_price item1 = new tbl_price();

                item1.id_product = id_product;
                item1.gia_nhap = gianhap;
                item1.gia_xuat = giaxuat;
                item1.type_price = type_price;
                item1.isactive = true;
                item1.datecreate = DateTime.Now;
                db.tbl_price.Add(item1);


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
        public SystemMessage BS_XoaGiaTheoSP(int id_price, int id_product)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();

                var check = db.tbl_price.FirstOrDefault(m => m.id == id_price && m.id_product == id_product);

                if (check != null)
                {
                    db.tbl_price.Remove(check);
                    db.SaveChanges();


                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                }
                else
                {
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                }

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public SystemMessage BS_ChinhSua_Gia_SP(int gianhap, int giaxuat, string type_price, int id_product, int id_price)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var CheckTypePrice =
                    db.sys_parameter.FirstOrDefault(m => m.value == type_price && m.code == "PRICE_PRODUCT");
                if (CheckTypePrice == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Type_Price_NotExit;
                    return systemMessage;
                }

                var checkPrice = db.tbl_price.FirstOrDefault(m =>
                    m.id == id_price && m.id_product == id_product);
                if (checkPrice == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                checkPrice.gia_nhap = gianhap;
                checkPrice.gia_xuat = giaxuat;
                checkPrice.type_price = type_price;
                checkPrice.date_modifiel = DateTime.Now;

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

        public SystemMessage BS_XoaAnhTheoSP(int id_img, int id_product)
        {

            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var db = new DB_CSEntities1();

                var check = db.img_product.FirstOrDefault(m => m.id == id_img && m.id_product == id_product);

                if (check != null)
                {

                    bool deleteFile = new DeleteFileUntil().DeleteFileByPath(check.path_img);
                    db.img_product.Remove(check);

                    db.SaveChanges();


                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                }
                else
                {
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                }

                return systemMessage;
            }
            catch (Exception e)
            {
                systemMessage.IsSuccess = false;
                systemMessage.Message = e.ToString();
                return systemMessage;
            }
        }

        public SystemMessage BS_ChinhSua_Anh_TheoSP(int id_img, int id_product, string txt_des)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkIMG =
                    db.img_product.FirstOrDefault(m => m.id == id_img && m.id_product == id_product);
                if (checkIMG == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.IMG_Not_Exit;
                    return systemMessage;
                }

                checkIMG.description = txt_des;
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


        public SystemMessage BS_ThemmoiSP(string tensp, string codesp, string loaisp, int unitsp, string gioithieusp, bool status = false)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var checkProduct =
                    db.sys_product.FirstOrDefault(m => m.name == tensp);
                if (checkProduct != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Product_name_Exit;
                    return systemMessage;
                }
                var checkProduct2 =
                    db.sys_product.FirstOrDefault(m => m.code == codesp);
                if (checkProduct2 != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Product_code_Exit;
                    return systemMessage;
                }

                if (string.IsNullOrEmpty(tensp))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên sản phẩm không được để trống";
                    return systemMessage;
                }
                if (string.IsNullOrEmpty(codesp))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mã sản phẩm không được để trống";
                    return systemMessage;
                }

                sys_product item1 = new sys_product();

                item1.TYPE = loaisp;
                item1.code = codesp;
                item1.name = tensp;
                item1.isactive = status;
                item1.unit_id = unitsp;
                item1.description = gioithieusp;

                db.sys_product.Add(item1);


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

        public SystemMessage BS_Chinhsua_TT_SP(int id, string tensp, string codesp, string loaisp, int unitsp, string gioithieusp, bool status = false)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var checkP =
                    db.sys_product.FirstOrDefault(m => m.id == id);
                if (checkP == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataNotExisted;
                    return systemMessage;
                }

                var checkProduct =
                    db.sys_product.FirstOrDefault(m => m.name == tensp && m.id != id);
                if (checkProduct != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Product_name_Exit;
                    return systemMessage;
                }

                var checkProduct2 =
                    db.sys_product.FirstOrDefault(m => m.code == codesp && m.id != id);
                if (checkProduct2 != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.Product_code_Exit;
                    return systemMessage;
                }
                if (string.IsNullOrEmpty(tensp))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên sản phẩm không được để trống";
                    return systemMessage;
                }
                if (string.IsNullOrEmpty(codesp))
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Mã sản phẩm không được để trống";
                    return systemMessage;
                }

                checkP.TYPE = loaisp;
                checkP.code = codesp;
                checkP.name = tensp;
                checkP.isactive = status;
                checkP.unit_id = unitsp;
                checkP.description = gioithieusp;

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
    }
}
