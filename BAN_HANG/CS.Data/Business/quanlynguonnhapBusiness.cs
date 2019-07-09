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
    public class quanlynguonnhapBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnLstNguonnhap BS_Listnguonnhap(string searchValue, int currPage, int recodperpage, int centerid, int roleid)
        {
            ReturnLstNguonnhap result = new ReturnLstNguonnhap();

            List<Infornguonnhap> list = new List<Infornguonnhap>();
            SqlConnection con = new SqlConnection();
            con = Connection.Connect.GetConnect();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "load_all_nguonnhap";
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
                Infornguonnhap customer = new Infornguonnhap();

                customer.id = Int32.Parse(rowItem["id"].ToString());
                customer.id_center = Int32.Parse(rowItem["id_center"].ToString());
                customer.tennguon = rowItem["tennguon"].ToString();
                customer.diachi = rowItem["diachi"].ToString();
                customer.sdt = rowItem["sdt"].ToString();
                customer.code = rowItem["code"].ToString();
                customer.isactive = bool.Parse(rowItem["isactive"].ToString());
                customer.description = rowItem["description"].ToString();
                customer.centername = rowItem["centername"].ToString();
                list.Add(customer);
            }

            result.Data = list;
            result.Total = total;
            return result;
        }
        public SystemMessage DeleteNguonNhapById(int id)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var getcus = db.sys_nguonnhap.FirstOrDefault(m => m.id == id);
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

        public sys_nguonnhap GetnguonnhapById(int id)
        {
            try
            {
                var result = db.sys_nguonnhap.FirstOrDefault(ob => ob.id == id);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ValidateCustomer(sys_nguonnhap customer)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(customer.tennguon, "Tên nguồn nhập", 1, 50);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(customer.code, "Code ", 1, 25);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(customer.diachi, "Địa chỉ", 3, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(customer.sdt, "Số điện thoại", 9, 15);
            if (rs != null)
            {
                return rs;
            }
            return null;
        }
        public SystemMessage Editnguon(sys_nguonnhap customer)
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

                var checkExisted = db.sys_nguonnhap.FirstOrDefault(ob => ob.id == customer.id && ob.id_center == customer.id_center);
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

                var checkten = db.sys_nguonnhap.FirstOrDefault(m => m.tennguon == customer.tennguon && m.id != customer.id && m.id_center == customer.id_center && m.isactive == true);
                if (checkten != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Tên nguồn đã tồn tại";
                    return systemMessage;
                }
                var checkCode = db.sys_nguonnhap.FirstOrDefault(m => m.code == customer.code && m.id != customer.id && m.id_center == customer.id_center && m.isactive == true);
                if (checkCode != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Code nguồn nhập đã tồn tại";
                    return systemMessage;
                }

                checkExisted.tennguon = customer.tennguon;
                checkExisted.code = customer.code;
                checkExisted.diachi = customer.diachi;
                checkExisted.sdt = customer.sdt;
                checkExisted.description = customer.description;
                
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

        public SystemMessage AddNguonnhap(sys_nguonnhap customer)
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

                var checktontai = db.sys_nguonnhap.FirstOrDefault(m => m.tennguon == customer.tennguon && m.code == customer.code && m.id_center == customer.id_center);
                if (checktontai != null && checktontai.isactive == true)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = "Nguồn nhập đã tồn tại";
                    return systemMessage;
                }
                else if (checktontai != null && checktontai.isactive == false)
                {
                    checktontai.isactive = true;

                    db.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.AddSuccess;
                    return systemMessage;
                }
                else
                {
                    db.sys_nguonnhap.Add(customer);
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

    }
}
