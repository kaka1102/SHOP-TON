using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Data.Model
{

    public class LstMenuViewModel
    {
        public List<Menureturn> Data { get; set; }
        public int Total { get; set; }
    }

    public class Menureturn
    {
        public int IdPer { get; set; }
        public int IdMenu { get; set; }
        public string MenuName { get; set; }
        public string IsCreate { get; set; }
        public string IsRead { get; set; }
        public string IsUpdate { get; set; }
        public string IsDelete { get; set; }
        public string IsExport { get; set; }
        public string RoleName { get; set; }
        public bool status { get; set; }

        public lsIsCreate Create { get; set; }
        public lsIsRead Read { get; set; }
        public lsIsDelete Delete { get; set; }
        public lsIsUpdate Update { get; set; }
        public lsIsExport Export { get; set; }
        public OptionMenu Option { get; set; }
    }

    public class OptionMenu
    {
        public int IdPermis { get; set; }
        public bool status { get; set; }
    }
    public class lsIsCreate
    {
        public string IsCreate { get; set; }
        public bool status { get; set; }
    }

    public class lsIsRead
    {
        public string IsRead { get; set; }
        public bool status { get; set; }
    }
    public class lsIsDelete
    {
        public string IsDelete { get; set; }
        public bool status { get; set; }
    }
    public class lsIsUpdate
    {
        public string IsUpdate { get; set; }
        public bool status { get; set; }
    }

    public class lsIsExport
    {
        public string IsExport { get; set; }
        public bool status { get; set; }
    }
}
