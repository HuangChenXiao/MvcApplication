using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Custom.Model
{
    public class BasePage
    {
        public BasePage()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            return returnText;
        }

        #region 图片压缩
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight=0, int dWidth=0, int flag=50)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);

            ImageFormat tFormat = iSource.RawFormat;

            int sW = 0, sH = 0;

            //按比例缩放

            Size tem_size = new Size(iSource.Width, iSource.Height);

            dHeight = iSource.Height / 2;
            dWidth = iSource.Width / 2;

            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {

                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {

                    sW = dWidth;

                    sH = (dWidth * tem_size.Height) / tem_size.Width;

                }

                else
                {

                    sH = dHeight;

                    sW = (tem_size.Width * dHeight) / tem_size.Height;

                }

            }

            else
            {

                sW = tem_size.Width;

                sH = tem_size.Height;

            }

            Bitmap ob = new Bitmap(dWidth, dHeight);

            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量

            EncoderParameters ep = new EncoderParameters();

            long[] qy = new long[1];

            qy[0] = flag;//设置压缩的比例1-100

            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);

            ep.Param[0] = eParam;

            try
            {

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {

                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {

                        jpegICIinfo = arrayICI[x];

                        break;

                    }

                }

                if (jpegICIinfo != null)
                {

                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径

                }

                else
                {

                    ob.Save(dFile, tFormat);

                }

                return true;

            }

            catch
            {

                return false;

            }

            finally
            {

                iSource.Dispose();

                ob.Dispose();

            }



        }
        #endregion

        #region 判断是否登录
        public static void ChkAdminLogin()
        {
            if (string.IsNullOrEmpty(BasePage.GetCookie("UserInfoCookie")))
            {
                HttpContext.Current.Response.Redirect("/Account/Login");
            }
        }
        #endregion
 
        #region ========Encrypt加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, "MATICSOFT");
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========Decrypt解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "MATICSOFT");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region MD5
        public static string Md5Hash(string input)
        {
            // MD5 md5Hasher = MD5.Create();
            //// MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            // StringBuilder sBuilder = new StringBuilder(32);
            // for (int i = 0; i < data.Length; i++)
            // {
            //     sBuilder.Append(data[i].ToString("x2"));
            // }
            // return sBuilder.ToString();

            byte[] result = ((HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            StringBuilder output = new StringBuilder(16);
            for (int i = 0; i < result.Length; i++)
            {
                output.Append((result[i]).ToString("x2", System.Globalization.CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }

        #endregion

        #region 返回状态值
        /// <summary>
        /// 返回状态值
        /// </summary>
        /// <param name="statestr">状态字符串,颜色为可选，格式：名称|值|颜色,名称|值|颜色。例：禁用|0|ff0000,启用|1|1ea514</param>
        /// <param name="val">输入值，如果值和前面的值一致返回相应的名称</param>
        /// <returns></returns>
        public static String GetStateValue(string statestr, string val)
        {
            string tempval = val;
            if (statestr != "" && val != "")
            {
                string[] stateArry = statestr.Split(',');
                for (int i = 0; i < stateArry.Length; i++)
                {
                    string[] templist = stateArry[i].Split('|');
                    if (templist[1] == val)
                    {
                        if (templist.Length > 2)
                        {
                            tempval = "<font color='" + templist[2] + "'>" + templist[0] + "</font>";
                        }
                        else
                        {
                            tempval = templist[0];
                        }
                        break;
                    }
                }
            }
            return tempval;
        }
        #endregion

        #region cookie写、读、删
        /// <summary>
        /// 写入cookie
        /// </summary>
        /// <param name="CookieName">名称</param>
        /// <param name="CookieValue">值</param>
        /// <param name="Expires">过期时间</param>
        public static void WriteCookie(string CookieName, string CookieValue, int Expires)
        {
            HttpCookie aCookie = new HttpCookie(CookieName.Trim());
            aCookie.Value = HttpUtility.UrlEncode(CookieValue.Trim());
            if (Expires != 0)
            {
                aCookie.Expires = DateTime.Now.AddDays(Expires);
            }
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }
        

        /// <summary>
        /// 读取cookie
        /// </summary>
        /// <param name="CookieName">名称</param>
        /// <returns></returns>
        public static String GetCookie(string CookieName)
        {
            String StrCookies = "";
            if (HttpContext.Current.Request.Cookies[CookieName.Trim()] != null)
            {
                StrCookies = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[CookieName].Value);
            }
            return StrCookies;
        }




        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="CookieName">名称</param>
        public static void DelCookie(string CookieName)
        {
            if (HttpContext.Current.Request.Cookies[CookieName.Trim()] != null)
            {
                HttpCookie aCookie = new HttpCookie(CookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }
        
        #endregion

        #region 日期格式化
        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="dateType">返回类型，1返回yyyy-mm-dd，2返回yyyy年mm月dd日，3返回yyyy.mm.dd，4返回yyyy-mm-dd hh:mm，5返回mm-dd</param>
        /// <returns></returns>
        public static String DateFormate(DateTime dt, int dateType)
        {
            string strtime = "";
            if (dt == null)
            {
                strtime = "-";
            }
            else
            {
                if (IsNumber(Convert.ToString(dateType)) == true)
                {
                    switch (dateType)
                    {
                        case 1:
                            strtime = String.Format("{0:yyyy-MM-dd}", dt);
                            break;
                        case 2:
                            strtime = String.Format("{0:yyyy年MM月dd日}", dt);
                            break;
                        case 3:
                            strtime = String.Format("{0:yyyy.MM.dd}", dt);
                            break;
                        case 4:
                            strtime = String.Format("{0:yyyy-MM-dd hh:mm}", dt);
                            break;
                        case 5:
                            strtime = String.Format("{0:MM-dd}", dt);
                            break;
                        case 6:
                            strtime = String.Format("{0:yyyy/MM/dd}", dt);
                            break;
                        default:
                            strtime = String.Format("{0:yyyy-MM-dd hh:mm:ss}", dt);
                            break;
                    }
                }
                else
                {
                    strtime = Convert.ToString(dt);
                }
            }
            return strtime;
        }
        #endregion

        #region 日期比较类
        /// <summary>
        /// 日期比较类
        /// </summary>
        public class DateTimeDiff
        {
            /// <summary>
            /// 把秒转换成分钟
            /// </summary>
            /// <returns></returns>
            public static int SecondToMinute(int Second)
            {
                decimal mm = (decimal)((decimal)Second / (decimal)60);
                return Convert.ToInt32(Math.Ceiling(mm));
            }
            /// <summary>
            /// 计算两个时间的时间间隔
            /// </summary>
            /// <param name="DateTimeOld">较早的日期和时间</param>
            /// <param name="DateTimeNew">较后的日期和时间</param>
            /// <returns></returns>
            public static string DateDiff(DateTime DateTimeOld, DateTime DateTimeNew)
            {
                string dateDiff = "";
                TimeSpan ts1 = new TimeSpan(DateTimeOld.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTimeNew.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int day = ts.Days;
                int hou = ts.Hours;
                int minu = ts.Minutes;
                int sec = ts.Seconds;
                if (day > 0)
                {
                    if (day > 30)
                    {
                        if (day > 364)
                        {
                            dateDiff += day / 365 + "年";
                        }
                        else
                        {
                            dateDiff += day / 30 + "个月";
                        }
                    }
                    else
                    {
                        dateDiff += day.ToString() + "天";
                    }
                }
                else
                {
                    if (hou > 0)
                    {
                        dateDiff += hou.ToString() + "小时";
                    }
                    else
                    {
                        if (minu > 0)
                        {
                            dateDiff += minu.ToString() + "分钟";
                        }
                        else
                        {
                            if (sec > 0)
                            {
                                dateDiff += sec.ToString() + "秒";
                            }
                            else
                            {
                                dateDiff += "0秒";
                            }
                        }
                    }
                }
                if (DateTimeNew.CompareTo(DateTimeOld) > 0)
                {
                    dateDiff += "前";
                }
                else
                {
                    dateDiff += "后";
                }
                return dateDiff;
            }
            /// <summary>
            ///判断是否于5分钟之前
            /// </summary>
            /// <param name="DateTimeOld">较早的日期和时间</param>
            /// <returns></returns>
            public static bool DateDiff_minu(DateTime DateTimeOld)
            {
                TimeSpan ts1 = new TimeSpan(DateTimeOld.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int minu = ts.Minutes;
                if (minu > 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 与当前时间比较,重载时间比较函数,只有一个参数
            /// </summary>
            /// <param name="DateTimeOld">较早的日期和时间</param>
            /// <returns></returns>
            public static string DateDiff(DateTime DateTimeOld)
            {
                return DateDiff(DateTimeOld, DateTime.Now);
            }
            /// <summary>
            /// 日期比较,返回精确的几分几秒
            /// </summary>
            /// <param name="DateTime1">较早的日期和时间</param>
            /// <param name="DateTime2">较迟的日期和时间</param>
            /// <returns></returns>
            public static string DateDiff_full(DateTime DateTime1, DateTime DateTime2)
            {
                string dateDiff = null;
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                return dateDiff;
            }
            /// <summary>
            /// 时间比较,返回精确的几秒
            /// </summary>
            /// <param name="DateTime1">较早的日期和时间</param>
            /// <param name="DateTime2">较迟的日期和时间</param>
            /// <returns></returns>
            public static int DateDiff_Sec(DateTime DateTime1, DateTime DateTime2)
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int dateDiff = ts.Days * 86400 + ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                return dateDiff;
            }
            /// <summary>
            /// 和当前系统时间比较，比系统时间早的返回0，否则返回相差几秒
            /// </summary>
            /// <param name="DateTime">比较的时间</param>
            /// <returns></returns>
            public static int DateDiff_Sec(DateTime DateTime1)
            {
                int dateDiff = 0;
                DateTime DateTime2 = DateTime.Now;
                if (DateTime1 > DateTime2)
                {
                    TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                    TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                    TimeSpan ts = ts2.Subtract(ts1).Duration();
                    dateDiff = ts.Days * 86400 + ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                }
                return dateDiff;
            }
            /// <summary>
            /// 日期比较
            /// </summary>
            /// <param name="today">当前日期</param>
            /// <param name="writeDate">输入日期</param>
            /// <param name="n">比较天数</param>
            /// <returns>大于天数返回true，小于返回false</returns>
            public static bool CompareDate(string today, string writeDate, int n)
            {
                DateTime Today = Convert.ToDateTime(today);
                DateTime WriteDate = Convert.ToDateTime(writeDate);
                WriteDate = WriteDate.AddDays(n);
                if (Today >= WriteDate)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        #region 判断是否为数字
        /// <summary>
        /// 字符串是否为数字
        /// </summary>
        /// <param name="TextValue"></param>
        /// <returns></returns>
        public static bool IsNumber(string TextValue)
        {
            try
            {
                int n = Convert.ToInt32(TextValue);
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region 截取字符串长度
        /// <summary>
        /// 截取字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">长度</param>
        /// <param name="point">0无追加,1追加...</param>
        /// <returns></returns>
        protected static string getchar(string str, int len, int point)
        {
            if (str.Length > len)
            {
                str = str.Substring(0, len);
                if (point == 1)
                {
                    str = str + "...";
                }
                return str;
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region 将字符串中间设置星号
        /// <summary>
        /// 将字符串中间设置星号
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">要显示星号的长度</param>
        /// <returns></returns>
        protected static string SetStar(string str, int len)
        {
            if (str.Length > 2)
            {
                string star = "";
                if (str.Length <= len)
                {
                    len = str.Length - 2;
                }

                for (int i = 0; i < len; i++)
                {
                    star = star + "*";
                }
                int preN = (str.Length - len) / 2 + (str.Length - len) % 2;
                int endN = (str.Length - len) / 2;
                str = str.Substring(0, preN) + star + str.Substring(str.Length - endN, endN);

                return str;
            }
            else if (str.Length == 2)
            {
                return str.Substring(0, 1) + "*";
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region 去除HTML标记
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="Htmlstring">要去除的HTML内容</param>
        /// <returns></returns>
        public string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }
        #endregion

        #region 随机数

        #region 数字随机数，指定范围
        /// <summary>        
        /// 数字随机数，指定范围      
        /// </summary>        
        /// <param name="num1">开始</param>        
        /// <param name="num2">结束</param>        
        /// <returns>从多少到多少之间的数据 包括开始不包括结束</returns>        
        public static int RndInt(int num1, int num2)
        {
            int _RandIndex = 0;
            if (_RandIndex >= 1000000) _RandIndex = 1;
            Random rnd = new Random(DateTime.Now.Millisecond + _RandIndex);
            _RandIndex++;
            return rnd.Next(num1, num2);
        }
        #endregion

        #region RndNum
        /// <summary>        
        /// 数字随机数        
        /// </summary>        
        /// <param name="length">生成长度</param>        
        /// <returns>返回指定长度的数字随机串</returns>        
        public static string RndNum(int length)
        {
            int _RandIndex = 0;
            if (_RandIndex >= 1000000)
                _RandIndex = 1;
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond + _RandIndex);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            } return num.ToString();
        }
        #endregion

        #region 返回指定长度的数字和字母的随机串
        /// <summary>        
        /// 数字和字母随机数        
        /// </summary>        
        /// <param name="length">生成长度</param>        
        /// <returns>返回指定长度的数字和字母的随机串</returns>        
        public static string RndCode(int length)
        {
            int _RandIndex = 0;
            if (_RandIndex >= 1000000) _RandIndex = 1;
            char[] arrChar = new char[]{              
                 'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',              
                 '0','1','2','3','4','5','6','7','8','9',             
                 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'};
            System.Text.StringBuilder num = new System.Text.StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond + _RandIndex);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }
        #endregion

        #region 生成混合随机数排除个别易混淆字母
        /// <summary>
        /// 生成混合随机数排除个别易混淆字母
        /// </summary>
        /// <param name="num">长度</param>
        public static string GetMixPwd(int num)
        {
            string a = "0123456789abcdefghijkmnprstuvwxy";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }

            return sb.ToString();
        }
        #endregion

        #endregion

        #region 返回指定目录下所有文件信息
        /// <summary>
        /// 返回指定目录下所有文件信息
        /// </summary>
        /// <param name="strDirectory">目录字符串</param>
        /// <returns></returns>
        public static List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历
            }
            return listFiles;
        }
        #endregion

        #region 转换文件大小显示格式
        /// <summary>
        /// 转换文件大小显示格式
        /// </summary>
        /// <param name="size">文件的大小</param>
        /// <returns></returns>
        public static string ReturnFileSize(int size)
        {
            string FileSize = ""; if (size != 0)
            {
                if (size >= 1073741824)
                {
                    FileSize = System.Math.Round(Convert.ToDouble((double)size / (double)1073741824), 2).ToString() + "GB";  //GB            
                }
                else if (size >= 1048576)
                {
                    FileSize = System.Math.Round(Convert.ToDouble((double)size / (double)1048576), 2).ToString() + "MB";
                }
                else if (size >= 1024)
                {
                    FileSize = System.Math.Round(Convert.ToDouble((double)size / (double)1024), 2).ToString() + "KB"; int a = size / 1024 * 100; int b = size / 1024;
                }
                else
                {
                    FileSize = size.ToString() + "bytes";
                }
            }
            else { FileSize = size.ToString() + "bytes"; } return FileSize;
        }
        #endregion

        /// <summary>
        /// 随机验证码
        /// </summary>
        /// <returns></returns>
        public static string CodeClass()
        {
            string code = "0,1,2,3,4,5,6,7,8,9";
            string[] keyCode = code.Split(',');
            string validateCode = string.Empty;
            Random rand = new Random();
            do
            {
                validateCode = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    string s = keyCode[rand.Next(0, 10)];
                    validateCode += s;
                }
            }
            while ((validateCode.Contains("0") && validateCode.Contains("O")));
            return validateCode;
        }
    }
}
