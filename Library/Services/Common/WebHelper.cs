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
    }
}
