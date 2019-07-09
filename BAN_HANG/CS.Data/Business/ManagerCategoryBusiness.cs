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
    public class ManagerCategoryBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnListCategory BS_ListCategory(string searchValue, int currPage, int recodperpage)
        {
            ReturnListCategory result = new ReturnListCategory();

            List<tbl_LoaiSP> list = new List<tbl_LoaiSP>();
            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Getall_category";
            cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
            cmd.Parameters.Add(new SqlParameter("@currPage", currPage));
            cmd.Parameters.Add(new SqlParameter("@recodperpage", 10));
            cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int total = Convert.ToInt32(cmd.Parameters["@totalCount"].Value);
            foreach (DataRow rowItem in dt.Rows)
            {
                tbl_LoaiSP customer = new tbl_LoaiSP();

                customer.id = Int32.Parse(rowItem["id"].ToString());
                customer.category_name = rowItem["category_name"].ToString();
                customer.isactive = bool.Parse(rowItem["isactive"].ToString());
                customer.description = rowItem["description"].ToString();
                list.Add(customer);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }

        public tbl_LoaiSP GetCategoryById(int id)
        {
            try
            {
                var result = db.tbl_LoaiSP.FirstOrDefault(ob => ob.id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SystemMessage DeleteCategoryById(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var getcus = db.tbl_LoaiSP.FirstOrDefault(m => m.id == id);
                if (getcus != null)
                {
                    getcus.isactive = false;
                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                }
                else
                {
                    systemMessage.IsSuccess = false;
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
        public string ValidateCustomer(tbl_LoaiSP customer)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(customer.category_name, "Tên loại sản phẩm", 1, 50);
            if (rs != null)
            {
                return rs;
            }
            return null;
        }
        public SystemMessage EditCategory(tbl_LoaiSP customer)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateCustomer(customer);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var checkExisted = db.tbl_LoaiSP.FirstOrDefault(ob => ob.id == customer.id);
                if (checkExisted == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.NoData;
                    return systemMessage;
                }

                var checkten = db.tbl_LoaiSP.FirstOrDefault(m => m.category_name == customer.category_name && m.id != customer.id);
                if (checkten != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên loại sản phẩm đã tồn tại";
                    return systemMessage;
                }

                checkExisted.category_name = customer.category_name;
                checkExisted.description = customer.description;
                checkExisted.isactive = customer.isactive;

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

        public SystemMessage AddCategory(tbl_LoaiSP customer)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var check = ValidateCustomer(customer);
                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var checkSDT = db.tbl_LoaiSP.FirstOrDefault(m => m.category_name == customer.category_name);
                if (checkSDT != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên loại sản phẩm đã tồn tại";
                    return systemMessage;
                }

                db.tbl_LoaiSP.Add(customer);
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

        public List<ReturnLstCategoryProduct> BS_LstProductByCategoryID(int id)
        {
            List<ReturnLstCategoryProduct> result = new List<ReturnLstCategoryProduct>();

            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "BS_LstProductByCategoryID";
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow rowItem in dt.Rows)
            {
                ReturnLstCategoryProduct customer = new ReturnLstCategoryProduct();

                customer.id_category = Int32.Parse(rowItem["id_category"].ToString());
                customer.category_name = rowItem["category_name"].ToString();
                customer.description = rowItem["description"].ToString();
                customer.id_product = Int32.Parse(rowItem["id_product"].ToString());
                customer.id_detail = Int32.Parse(rowItem["id_detail"].ToString());
                customer.code = rowItem["code"].ToString();
                customer.name = rowItem["name"].ToString();
                customer.unit_name = rowItem["unit_name"].ToString();
                customer.des_product = rowItem["des_product"].ToString();
                result.Add(customer);
            }

            return result;
        }

        public SystemMessage AddProductToCategory(tbl_detail_category customer)
        {
            SystemMessage systemMessage = new SystemMessage();
            try
            {
                var checkTonTai = db.tbl_detail_category.FirstOrDefault(m => m.id_product == customer.id_product && m.id_category == customer.id_category);

                if (checkTonTai != null && checkTonTai.isactive ==true)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Sản phẩm đã tồn tại trong nhóm";
                    return systemMessage;
                }else if (checkTonTai!=null && checkTonTai.isactive ==false)
                {
                    checkTonTai.isactive = true;

                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                    return systemMessage;
                }
                else
                {
                    db.tbl_detail_category.Add(customer);
                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
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

        public SystemMessage BS_xoasanphamkhoinhom(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var getcus = db.tbl_detail_category.FirstOrDefault(m => m.id == id);
                if (getcus != null)
                {
                    getcus.isactive = false;
                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.DeleteSuccess;
                }
                else
                {
                    systemMessage.IsSuccess = false;
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
    }
}
