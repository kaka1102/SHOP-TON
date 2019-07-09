using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Data.DataContext;
using CS.Data.Model;

namespace CS.Data.Business
{
    public class ManagerUserTreeViewBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public UserTreeContainerByCenterViewModal GetListUsersTreeView(int centerid)
        {
            try
            {
                var center = db.Branch.FirstOrDefault(ob => ob.Id == centerid);
                UserTreeContainerByCenterViewModal result = new UserTreeContainerByCenterViewModal();
                result.icon = "jstree-root";
                result.text = center.Name;
                var _centerid = new SqlParameter
                {
                    ParameterName = "centerid",
                    Value = centerid
                };
                var parentid = new SqlParameter
                {
                    ParameterName = "parentid",
                    Value = 0
                };
                var listparent = db.Database.SqlQuery<UserTreeViewModal>(
                    "dbo.User_GetListAccountParent_ByCenter_parentid @centerid,@parentid", _centerid, parentid).ToList();

                List<UserTreeViewModal> lst = new List<UserTreeViewModal>();
                foreach (var item in listparent)
                {
                    item.icon = "jstree-user";
                    var dep = DeQuy(item, centerid);
                    lst.Add(dep);
                }

                result.children = lst;
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public UserTreeViewModal DeQuy(UserTreeViewModal dep, int centerid)
        {
            var _centerid = new SqlParameter
            {
                ParameterName = "centerid",
                Value = centerid
            };
            var parentid = new SqlParameter
            {
                ParameterName = "parentid",
                Value = dep.id
            };
            var listChild = db.Database.SqlQuery<UserTreeViewModal>(
                "dbo.User_GetListAccountParent_ByCenter_parentid @centerid,@parentid", _centerid, parentid).ToList();
            dep.children = listChild;
            foreach (var item in dep.children)
            {
                item.icon = "jstree-user";
                DeQuy(item, centerid);
            }
            return dep;

        }
    }
}
