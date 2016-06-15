using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Services.Infrastructure;
using System;
using System.Web;

namespace Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IWebHelper _webHelper;
        private readonly IResourceService _resourceService;
        private readonly IAuthenticationManager _authenticationManager;
        public LoggerService(IWebHelper webHelper, IResourceService resourceService, IAuthenticationManager authenticationManager)
        {
            _webHelper = webHelper;
            _resourceService = resourceService;
            _authenticationManager = authenticationManager;
        }
        public void insertOnFitter(LogsModel model)
        {
            if (_resourceService.LogEnable())
            {
                try
                {
                    model.Uid =model.Uid;
                    string sendData = JsonConvert.SerializeObject(model);
                    var respStr = _webHelper.PostData(_resourceService.GetLogger() + "logs", sendData, "post", "json");
                }
                catch (Exception)//日志服务器若返回异常不能抛至当前程序
                {
                    return;
                }
            }
        }

        public void insert(Exception e, LogLevel level, string message,string userName)
        {
            if (_resourceService.LogEnable())
            {
                try
                {
                    var model = new LogsModel();
                    if (!string.IsNullOrEmpty(message))
                    {
                        model.Message = message;
                    }
                    if (e.InnerException != null)
                    {
                        model.Memo = e.InnerException.Message;
                        model.HResult = e.InnerException.HResult;
                    }
                    else
                    {
                        model.HResult = e.HResult;
                        model.Memo = e.Message;
                    }
                    if (!string.IsNullOrEmpty(userName))
                    {
                        model.Uid = _authenticationManager.User.Identity.Name;
                    }
                    else
                    {
                        model.Uid = userName;
                    }
                    model.Level = (int)level;
                    model.Browser = HttpContext.Current.Request.Browser.Browser;
                    model.CreateDate = DateTime.Now;
                    model.Ip = _webHelper.GetCurrentIpAddress();
                    model.Url = _webHelper.GetCurrentUrl();

                    string sendData = JsonConvert.SerializeObject(model);
                    var respStr = _webHelper.PostData(_resourceService.GetLogger() + "logs", sendData, "post", "json");
                }
                catch (Exception)//日志服务器若返回异常不能抛至当前程序
                {
                    return;
                }
            }
        }
    }
}
