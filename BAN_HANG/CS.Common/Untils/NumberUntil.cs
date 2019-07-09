using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Untils
{
   public class NumberUntil
    {
        public static long ConvertStringToLong(string number)
        {
            long result = 0;
            long.TryParse(number, out result);
            return result;
        }
    }
}
