using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Services
{
    public interface IWebHelper
    {   /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>     
        string GetCurrentIpAddress();

        /// <summary>
        /// 获取上一个页面URL
        /// </summary>
        /// <returns></returns>
        string GetLPreviousUrl();

        /// <summary>
        /// 获取当前URL
        /// </summary>
        /// <returns></returns>
        string GetCurrentUrl();
        /// <summary>
        /// 传入本地时间,获取Unix时间戳
        /// </summary>
        /// <returns>True返回10位精确到秒,False返回13位精确到毫秒</returns>
        string GetTimeStamp(DateTime? dt = null, bool bflag = true);

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string MD5Encrypt(string source);

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string SHA1Encrypt(string source);

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string SHA256Encrypt(string source);

        /// <summary>
        ///验证邮箱格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IsValidEmail(string email);
        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="postDataString">要上传的字符串</param>
        /// <param name="method">请求类型：默认get</param>
        /// <returns></returns>
        string PostData(string postUrl, string postDataString = null, string method = null, string postType = "x-www-form-urlencoded");

        /// <summary>
        /// 发送HTTP请求，下载文件
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="fileType">文件类型</param>
        void DownloadFile(string url, string fileType = null);

        List<SelectListItem> GetCovSumOrder(List<SelectListItem> list);
        /// <summary>
        /// 金额转换汉字大写
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        string CmycurD(decimal num);
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        string GetEnumDescription(Enum enumValue);
    }
}
