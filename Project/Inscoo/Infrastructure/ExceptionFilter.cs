using Newtonsoft.Json;
using Services;
using Services.Infrastructure;
using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Infrastructure
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        private readonly IResourceService _resourceService;
        public ExceptionFilter(IWebHelper webHelper, IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            if (filterContext.ExceptionHandled == true)
            {
                return;
            }
            HttpException httpException = new HttpException(null, exception);

            var errorCode = httpException.GetHttpCode();
            var logs = new LogsModel();//日志
            logs.HttpStatusCode = errorCode;
            /*
            * 1、根据对应的HTTP错误码跳转到错误页面
            * 2、这里对HTTP 404/400错误进行捕捉和处理
            * 3、其他错误默认为HTTP 500服务器错误
            */
            if (httpException != null && (errorCode == 400 || errorCode == 404))
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.HttpContext.Response.Redirect("~/Error/NotFound");
            }
            if (httpException != null && (errorCode == 401))
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.Redirect("~/Account/Login");
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.Redirect("~/Error/InternalError");
            }

            if (_resourceService.LogEnable())//若启用日志
            {
                logs.Controller = filterContext.RouteData.Values["controller"].ToString();
                logs.Action = filterContext.RouteData.Values["action"].ToString();
                logs.Message = filterContext.Exception.Message;
                if (filterContext.Exception.InnerException != null)
                {
                    logs.Memo = filterContext.Exception.InnerException.Message;
                    logs.HResult = filterContext.Exception.InnerException.HResult;
                }
                else
                {
                    logs.HResult = filterContext.Exception.HResult;
                }
                if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity != null)
                {
                    logs.Uid = filterContext.HttpContext.User.Identity.Name;
                }
                logs.Level = 3;
                logs.Browser = filterContext.HttpContext.Request.Browser.Browser;
                logs.CreateDate = DateTime.Now;
                logs.Ip = filterContext.HttpContext.Request.UserHostAddress;
                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    logs.Url = filterContext.HttpContext.Request.UrlReferrer.ToString();
                }
                try
                {
                    string sendData = JsonConvert.SerializeObject(logs);

                    var client = new WebClient();
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("Content-Type", "application/json");
                    client.UploadString(_resourceService.GetLogger()+"logs", "post", sendData);
                }
                catch (Exception)//日志服务器若返回异常不能抛至当前程序
                {
                    ;
                }
            }
            //设置异常已经处理,否则会被其他异常过滤器覆盖
            filterContext.ExceptionHandled = true;
            //在派生类中重写时，获取或设置一个值，该值指定是否禁用IIS自定义错误。
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}