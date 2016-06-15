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
        public LoggerService(IWebHelper webHelper, IResourceService resourceService)
        {
            _webHelper = webHelper;
            _resourceService = resourceService;
        }
        public void insertOnFitter(LogsModel model)
        {
            if (_resourceService.LogEnable())
            {
                try
                {
                    string sendData = JsonConvert.SerializeObject(model);
                    var respStr = _webHelper.PostData(_resourceService.GetLogger() + "logs", sendData, "post", "json");
                }
                catch (Exception)//日志服务器若返回异常不能抛至当前程序
                {
                    return;
                }
            }
        }

        public void insert(Exception e, LogLevel level, string Message = null)
        {
            if (_resourceService.LogEnable())
            {
                try
                {
                    var model = new LogsModel();
                    if (!string.IsNullOrEmpty(Message))
                    {
                        model.Message = Message;
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
                    model.Uid = null;
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
