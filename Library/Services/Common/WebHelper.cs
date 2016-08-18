using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

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

        public List<SelectListItem> GetCovSumOrder(List<SelectListItem> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    var nList = new List<SelectListItem>();
                    var tList = new List<GenericAttributeModel>();
                    foreach (var s in list)
                    {
                        var item = new GenericAttributeModel();
                        if (s.Text.Contains("万"))
                        {
                            s.Text = s.Text.Split('万')[0];
                            item.Description = "万";
                        }
                        if (s.Text.Contains("元/天"))
                        {
                            s.Text = s.Text.Split('元')[0];
                            item.Description = "元/天";
                        }
                        item.OtherInfo = double.Parse(s.Text);
                        item.Value = s.Value;
                        tList.Add(item);
                    }
                    if (tList.Count > 0)
                    {
                        var ga = tList.OrderBy(t => t.OtherInfo);
                        foreach (var g in ga)
                        {
                            var item = new SelectListItem()
                            {
                                Text = g.OtherInfo + g.Description,
                                Value = g.Value
                            };
                            nList.Add(item);
                        }
                        return nList;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return new List<SelectListItem>();
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
        #region 获取枚举类型
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }
        #endregion
        #region 加解密cookie
        private static Byte[] IV_64
        {
            get
            {
                return new byte[] { 55, 103, 246, 79, 36, 99, 167, 3 };
            }
        }
        private static Byte[] KEY_64
        {
            get
            {
                return new byte[] { 42, 16, 93, 156, 78, 4, 218, 32 };
            }
        }
        public string EncryptCookie(string name)//标准的DES加密  关键字、数据加密
        {
            //#region DES加密算法
            if (name != "")
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new
                    CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                sw.Write(name);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();

                //再转换为一个字符串
                return Convert.ToBase64String(ms.GetBuffer(), 0, Int32.Parse(ms.Length.ToString()));
            }
            else
            {
                return "";
            }
        }
        public string DecryptCookie(string temp)//标准的DES解密
        {
            //#region DES 解密算法
            if (temp != "")
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                //从字符串转换为字节组
                Byte[] buffer = Convert.FromBase64String(temp);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new
                    CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            else
            {
                return "";
            }
            //#endregion
        }
        #endregion
        public bool IsIdNumber(string idNumber)
        {
            Regex r = new Regex(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            if (r.IsMatch(idNumber))
            {
                return true;
            }
            return false;
        }
    }
}
