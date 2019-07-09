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
  public  class CustomerBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public Customer_Pagelist ListCustomer(string searchValue, int currPage, int recodperpage, int centerid, int roleid)
        {
            Customer_Pagelist result = new Customer_Pagelist();

            List<service_customer> list = new List<service_customer>();
            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_customer_getall";
            cmd.Parameters.Add(new SqlParameter("@searchValue", searchValue));
            cmd.Parameters.Add(new SqlParameter("@centerid", centerid));
            cmd.Parameters.Add(new SqlParameter("@roleid", roleid));
            cmd.Parameters.Add(new SqlParameter("@roleadmin", SystemMessageConst.Role.Admin));
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
                service_customer customer = new service_customer();
                customer.id = Int32.Parse(rowItem["id"].ToString());
                customer.id_center = Int32.Parse(rowItem["id_center"].ToString());
                customer.fullname = rowItem["fullname"].ToString();
                customer.id_card = rowItem["id_card"].ToString();
                customer.address = rowItem["address"].ToString();
                customer.mobile = rowItem["mobile"].ToString();
                customer.note = rowItem["note"].ToString();
                if (rowItem["birthday"].ToString() != "")
                {
                    customer.birthday = DateTime.Parse(rowItem["birthday"].ToString());
                }
                list.Add(customer);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }

        public SystemMessage DeleteCustomerById(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var getcus = db.service_customer.FirstOrDefault(m => m.id == id);
                if (getcus != null)
                {
                    getcus.status = 0;
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

        public service_customer GetCustomerById(int id)
        {
            try
            {
                var result = db.service_customer.FirstOrDefault(ob => ob.id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ValidateCustomer(service_customer customer)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(customer.fullname, "Họ và tên", 3, 50);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(customer.id_card, "Số chứng minh thư", 7, 25);
            if (rs != null)
            {
                return rs;
            }
            //rs = vld.CheckRequiredField(customer.code_customer, "Mã khách hàng", 3, 25);
            //if (rs != null)
            //{
            //    return rs;
            //}
            rs = vld.CheckRequiredField(customer.address, "Địa chỉ", 3, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(customer.mobile, "Số điện thoại", 9, 15);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckNonRequiredField(customer.note, "Ghi chú", 250);
            if (rs != null)
            {
                return rs;
            }
            return null;
        }
        public SystemMessage EditCustomer(service_customer customer)
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

                var checkExisted = db.service_customer.FirstOrDefault(ob => ob.id == customer.id && ob.id_center ==customer.id_center);
                if (checkExisted == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.NoData;
                    return systemMessage;
                }
                var checkBranch = db.Branch.FirstOrDefault(ob => ob.Id == customer.id_center);
                if (checkBranch == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đại lý không tồn tại !!!!";
                    return systemMessage;
                }

                var checkSDT = db.service_customer.FirstOrDefault(m => m.mobile == customer.mobile && m.id != customer.id && m.id_center == customer.id_center);
                if (checkSDT != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "SĐT đã tồn tại";
                    return systemMessage;
                }
                var checkCMT = db.service_customer.FirstOrDefault(m => m.id_card == customer.id_card && m.id != customer.id && m.id_center == customer.id_center);
                if (checkCMT != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "CMT đã tồn tại";
                    return systemMessage;
                }

                checkExisted.fullname = customer.fullname;
                checkExisted.address = customer.address;

                checkExisted.note = customer.note;

                checkExisted.id_card = customer.id_card;
                checkExisted.note = customer.note;
                checkExisted.birthday = customer.birthday;
                checkExisted.id_center = customer.id_center;
       
                checkExisted.mobile = customer.mobile;

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

        public SystemMessage AddCustomer(service_customer customer)
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

                var checkBranch = db.Branch.FirstOrDefault(ob => ob.Id == customer.id_center);
                if (checkBranch == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Đại lý không tồn tại !!!!";
                    return systemMessage;
                }

                var checkSDT = db.service_customer.FirstOrDefault(m => m.mobile == customer.mobile && m.id_center ==customer.id_center);
                if (checkSDT != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "SĐT đã tồn tại";
                    return systemMessage;
                }
                var checkCMT = db.service_customer.FirstOrDefault(m => m.id_card == customer.id_card && m.id_center == customer.id_center);
                if (checkCMT != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "CMT đã tồn tại";
                    return systemMessage;
                }

                db.service_customer.Add(customer);
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
    }
}
