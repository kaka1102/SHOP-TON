using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CS.Data.DataContext;
using System.Data;
using System.Data.Entity;
using CS.Common;
using CS.Data;
using Deaura.Common;

namespace WebApplication
{
    public class LogApp
    {
        private DB_CSEntities1 db = new DB_CSEntities1();
        public LogApp()
        {

        }

        public void RemoveLog(string keepTime)
        {
            DateTime operateTime = DateTime.Now;

            if (keepTime == "7")           
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")      
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")      
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }

            Delete(operateTime);      
            
        }

        public void WriteDbLog(bool result, string resultLog)
        {        
            sys_log logEntity = new sys_log();
            logEntity.id = Common.GuId();
            logEntity.date = Common.ConvertToTimestamp(DateTime.Now);
     //       logEntity.account = Env.GetUserInfo("name");
            logEntity.result = result;
            logEntity.description = resultLog;
            logEntity.ipAddress = Net.Ip;
            logEntity.creatorTime = DateTime.Now;
         //   logEntity.creatorUserId = Convert.ToInt32(Env.GetUserInfo("userid"));
            Insert(logEntity);              
        }

        public void WriteDbLog(sys_log entiSysLog)
        {
          
            entiSysLog.id = Common.GuId();
            entiSysLog.date = Common.ConvertToTimestamp(DateTime.Now);
         //   entiSysLog.account = Env.GetUserInfo("name");
            entiSysLog.ipAddress = Net.Ip;
            entiSysLog.creatorTime = DateTime.Now;
         //   entiSysLog.creatorUserId = Convert.ToInt32(Env.GetUserInfo("userid"));
            Insert(entiSysLog);

        }

   

        private void Insert(sys_log entiSysLog)
        {       
            db.Entry(entiSysLog).State = EntityState.Added;
            db.SaveChanges();
        }

        private void Delete(DateTime optime)
        {

            SqlParameter pr = new SqlParameter("@time", Common.ConvertToTimestamp(optime));
            db.Database.ExecuteSqlCommandAsync("DELETE sys_log WHERE date < @time", pr);
            
            //var expression = ExtLinq.True<sys_log>();
            //expression = expression.And(t => t.date <= optime);
        }


    }
}