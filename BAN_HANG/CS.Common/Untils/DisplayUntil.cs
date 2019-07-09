using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Const;

namespace CS.Common.Untils
{
    public class DisplayUntil
    {

        public static bool CheckBirthday(string date)
        {
            bool kq = false;

            if (string.IsNullOrEmpty(date))
            {
                return kq;
            }

            DateTime birthday = DateTime.Parse(date);

            DateTime today = DateTime.Now;

            if (birthday.Day == today.Day && birthday.Month == today.Month)
            {
                return kq = true;
            }

            return kq;
        }


        public static string CalulaterBirthday(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                {
                    return "";
                }
                DateTime d1 = DateTime.Parse(date);

                DateTime today = DateTime.Today;

                DateTime next = new DateTime(today.Year, d1.Month, d1.Day);

                if (next < today)
                    next = next.AddYears(1);

                int numDays = (next - today).Days;



                if (numDays == 0)
                {
                    return "Chúc mừng sinh nhật KH";
                }
                else if (numDays < 30)
                {
                    return "Sinh nhật KH :" + numDays + " ngày tới";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string RemoveDotInData(string str)
        {
            string[] arrListStr = str.Split(',');
            return arrListStr[0];
        }
        public static string DisplayMoney(double? input)
        {
            if (input == null)
            {
                return "-";
            }
            else
            {
                var result = String.Format("{0:0,0.##}", input);
                return result == "00" ? "0" : result;
            }
        }
        public static string DisplayMethodPay(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return "Không xác định";
            }
            else if(type=="1")
            {
                return "Tiền mặt";
            }
            else if (type == "2")
            {
                return "Tài khoản thẻ";
            }
            else
            {
                return "Chuyển khoản";
            }
        }


        public static string Display_DayMonthYear(DateTime? input, bool isOnlyDate = false)
        {
            if (input == null)
            {
                return "-";
            }
            else
            {
                return isOnlyDate ? input.Value.ToString("dd/MM/yyyy") : input.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }
        public static string ConvertStringToDayMonthYear(string input, bool isOnlyDate = false)
        {
            DateTime check;
            if (DateTime.TryParse(input, out check))
            {
                return Display_DayMonthYear(check, isOnlyDate);
            }
            else
            {
                return "";
            }

        }
        public static DateTime? ConvertStringDayMonthYearToDate(string input)
        {
            DateTime dt;
            if (DateTime.TryParseExact(input,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt))
            {
                return dt;
            }
            else
            {
                return null;
            }

        }
        public static string ConvertToTime(DateTime? input)
        {
            if (input == null)
            {
                return "-";
            }
            else
            {
                return input.Value.ToString("HH:mm");
            }
        }
        public static string ConvertIdProduct(int productId, int? detailId)
        {
            return productId + (detailId == null ? "" : "#" + detailId);
        }
        public static string GenBillNumber(long id,string code_brand)
        {
            return code_brand + "-" + string.Format("{0:D8}", id);
        }

        public static string HiddenMobile(string mobile, int roleId = 0)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return mobile;
            }
            else
            {
                
                int length = mobile.Length;
                if (length == 10)
                {
                    //  return mobile.Substring(6,9) + "*****";
                    return "*****" + mobile.Substring(5);
                }
                else if (length == 11)
                {
                    // return mobile.Substring(6,10) + "******";
                    return "*****" + mobile.Substring(6);
                }

                return mobile;
            }
        }
        /// <summary>
        /// Hiển thị month giữa 2 khoảng thời gian
        /// hoangtv
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string, int, int>> MonthsBetween(
            DateTime startDate,
            DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }
            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year, iterator.Month);
                iterator = iterator.AddMonths(1);
            }
        }

    }
}
