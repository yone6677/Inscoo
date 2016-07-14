using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Services
{
    public class WebHelper : IWebHelper
    {
        private readonly HttpContextBase _httpContext;
        private readonly IResourceService _resource;

        public WebHelper(HttpContextBase httpContext, IResourceService resource)
        {
            _httpContext = httpContext;
            _resource = resource;
        }
        public virtual string GetCurrentIpAddress()
        {
            try
            {
                return _httpContext.Request.UserHostAddress;
            }
            catch
            {
                return null;
            }
        }
        public virtual string GetLPreviousUrl()
        {
            try
            {
                return _httpContext.Request.UrlReferrer.ToString();
            }
            catch
            {
                return null;
            }
        }
        public virtual string GetCurrentUrl()
        {
            try
            {
                return _httpContext.Request.Url.AbsoluteUri;
            }
            catch
            {
                return null;
            }
        }
        //public virtual string GetUserBrowser()
        //{
        //    try
        //    {
        //        var ss = _httpContext.Request;
        //        return _httpContext.Request.Browser.Browser;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        public virtual string GetTimeStamp(DateTime? dt, bool bflag)
        {
            var UniversalTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var ts = new TimeSpan();
            if (!dt.HasValue)
            {
                dt = DateTime.Now;
            }
            ts = dt.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timeStamp = string.Empty;
            if (bflag)
                timeStamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                timeStamp = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return timeStamp;
        }

        public virtual string MD5Encrypt(string source)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.Default.GetBytes(source);
            string md5data = BitConverter.ToString(md5.ComputeHash(data));
            md5data = md5data.Replace("-", "").ToLower();
            return md5data;
        }

        public virtual string SHA1Encrypt(string source)
        {
            byte[] strRes = Encoding.UTF8.GetBytes(source);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }

        public virtual string SHA256Encrypt(string source)
        {
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(source);
            byte[] hashBytes = new SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashstr = Convert.ToBase64String(hashBytes);
            return hashstr;
        }
        public virtual bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }
        public string PostData(string postUrl, string postDataString, string method, string postType)
        {
            string responseString;
            try
            {
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                if (!string.IsNullOrEmpty(method))
                {
                    client.Headers.Add("Content-Type", "application/" + postType);
                    responseString = client.UploadString(postUrl, method, postDataString);
                }
                else
                {
                    responseString = client.DownloadString(postUrl);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return responseString;
        }

        public void DownloadFile(string url, string fileType)
        {
            try
            {
                var fileName = GetTimeStamp(null, false) + ".";
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.DownloadFile(url, fileName + fileType);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region 金额转换大写
        public string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = "";    //从原num值中取出的值
            string str4 = "";    //数字的字符串形式
            string str5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式
            j = str4.Length;      //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(str3);      //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }
        #endregion
    }
}
