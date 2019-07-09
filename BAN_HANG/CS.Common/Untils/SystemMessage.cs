using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS.Common.Untils
{
    public class SystemMessage
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? IdResult { get; set; }

        public int? id_bill { get; set; }
        public int? id_customer { get; set; }
        public string name_closer_cs { get; set; }

        public MessSendCS msgCS { get; set; }
    }

    public class MessSendCS{
        public string TenKH { get; set; }
        public string CMT { get; set; }
        public string SDT { get; set; }
        public string SoPhong { get; set; }
        public string CSName { get; set; }
        public string ThoiGian { get; set; }
        public string avartaLT { get; set; }
    }
    public class MessageLogin
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<LstRoleUser> lstRLU{ get; set; }
    }

    public class LstRoleUser
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}