using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Model
{
    public static class ObjectConvertHelper
    {
        public static string ToEmptyIfNull(this object text) {
            if (text==null)
            {
                return "";
            }
            return text.ToString();
        }
        /// <summary>
        /// 每月的第一天
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime ToEmptyStartDate(this object text)
        {
            if (string.IsNullOrEmpty(text.ToString()))
            {
                return Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd"));
            }
            return Convert.ToDateTime(text);
        }
        /// <summary>
        /// 每月的最后一天
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime ToEmptyEndDate(this object text)
        {
            if (string.IsNullOrEmpty(text.ToString()))
            {
                return Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            }
            return Convert.ToDateTime(text);
        }
    }
}
