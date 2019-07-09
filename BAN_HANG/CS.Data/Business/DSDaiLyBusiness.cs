using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Data.DataContext;

namespace CS.Data.Business
{
  public  class DSDaiLyBusiness
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public List<Branch> GetAllBrand()
        {
            try
            {
                var data = db.Branch.Where(m => m.is_active == 1).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
