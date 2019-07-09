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
    public class BranchBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public ReturnListBranch BS_GetAllBranch(string searchValue, int currPage, int recodperpage)
        {
            try
            {
                ReturnListBranch result = new ReturnListBranch();
                List<Branch> list = new List<Branch>();

                SqlConnection con = new SqlConnection();
                con = Connection.Connect.GetConnect();
                SqlCommand cmd = new SqlCommand("Branch_GetAllBranch", con);
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
                    Branch item = new Branch();

                    item.Id = int.Parse(rowItem["Id"].ToString());
                    item.Name = rowItem["Name"].ToString();
                    item.AddressName = rowItem["AddressName"].ToString();

                    item.Phone = rowItem["Phone"].ToString();
                    item.Tax_Number = rowItem["Tax_Number"].ToString();
                    item.IsParent = bool.Parse(rowItem["IsParent"].ToString());
                    item.Branch_code = rowItem["Branch_code"].ToString();
                    item.ShortName = rowItem["ShortName"].ToString();
                    item.City = rowItem["City"].ToString();
                    item.is_active = int.Parse(rowItem["is_active"].ToString());

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

        public SystemMessage BS_ChangeStatusBranch(Branch data)
        {
            SystemMessage systemMessage = new SystemMessage();
            DB_CSEntities1 entity = new DB_CSEntities1();
            try
            {
                var checkitem = entity.Branch.FirstOrDefault(m => m.Id == data.Id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataExisted;
                    return systemMessage;
                }
                else
                {

                    if (data.is_active == 1)
                    {
                        checkitem.is_active = 0;
                    }
                    else
                    {
                        checkitem.is_active = 1;
                    }


                    entity.SaveChanges();
                    systemMessage.IsSuccess = true;
                    systemMessage.Message = SystemMessageConst.systemmessage.EditSuccess;
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

        public Branch BS_GetBranchByID(int id)
        {
            try
            {
                var data = db.Branch.FirstOrDefault(m => m.Id == id);
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ValidateItem(Branch LvMenu)
        {
            string rs = null;
            var vld = new ValidateUtils();

            rs = vld.CheckRequiredField(LvMenu.Name, "Tên chi nhánh", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.AddressName, "Địa chỉ chi nhánh", 1, 100);
            if (rs != null)
            {
                return rs;
            }

            bool checkphone = vld.PhoneService(LvMenu.Phone);
            if (checkphone == false)
            {
                return rs = "Số điện thoại không đúng định dạng";
            }
            rs = vld.CheckRequiredField(LvMenu.Tax_Number, "Mã số thuế", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.Branch_code, "Mã chi nhánh", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.ShortName, "Tên rút gọn của chi nhánh", 1, 100);
            if (rs != null)
            {
                return rs;
            }
            rs = vld.CheckRequiredField(LvMenu.City, "Thành phố", 1, 100);

            if (rs != null)
            {
                return rs;
            }

            return null;
        }
        public SystemMessage BS_EditBrandch(Branch data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();

                var check = ValidateItem(data);

                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var checkitem = db.Branch.FirstOrDefault(m => m.Id == data.Id);

                if (checkitem == null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.DataExisted;
                    return systemMessage;
                }

                var checkTrungTen =
                    db.Branch.FirstOrDefault(m => m.Name == data.Name && m.Id != data.Id);


                if (checkTrungTen != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.NameBrandchExisted;
                    return systemMessage;


                }
                var checkTrungCode =
                    db.Branch.FirstOrDefault(m => m.Branch_code == data.Branch_code && m.Id != data.Id);


                if (checkTrungCode != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.CodeBrandchExisted;
                    return systemMessage;
                }

                checkitem.Name = data.Name;
                checkitem.AddressName = data.AddressName;
                checkitem.Phone = data.Phone;

                checkitem.Tax_Number = data.Tax_Number;
                checkitem.Branch_code = data.Branch_code;
                checkitem.ShortName = data.ShortName;
                checkitem.City = data.City;
                checkitem.is_active = data.is_active;


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

        public SystemMessage BS_AddNewBrandch(Branch data)
        {
            SystemMessage systemMessage = new SystemMessage();

            try
            {
                var db = new DB_CSEntities1();


                var check = ValidateItem(data);

                if (check != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = check;
                    return systemMessage;
                }

                var checkTrungTen =
                    db.Branch.FirstOrDefault(m => m.Name == data.Name);


                if (checkTrungTen != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.NameBrandchExisted;
                    return systemMessage;


                }
                var checkTrungCode =
                    db.Branch.FirstOrDefault(m => m.Branch_code == data.Branch_code);


                if (checkTrungCode != null)
                {
                    systemMessage.IsSuccess = false;
                    systemMessage.Message = SystemMessageConst.systemmessage.CodeBrandchExisted;
                    return systemMessage;
                }


                Branch sys = new Branch();

                sys.Name = data.Name;
                sys.AddressName = data.AddressName;
                sys.Phone = data.Phone;
                sys.IsParent = false;
                sys.Tax_Number = data.Tax_Number;
                sys.Branch_code = data.Branch_code;
                sys.ShortName = data.ShortName;
                sys.City = data.City;
                sys.is_active = data.is_active;


                db.Branch.Add(sys);
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
