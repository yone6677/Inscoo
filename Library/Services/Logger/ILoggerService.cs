using Models.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Services
{
    public interface ILoggerService
    {
        /// <summary>
        /// 程序报错日志
        /// </summary>
        /// <param name="model"></param>
        void insertOnFitter(LogsModel model);
        /// <summary>
        /// 低级别日志，程序可以正常运行
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level">错误级别</param>
        /// <param name="message">错误描述</param>
        /// <param name="userName">操作人</param>
        void insert(Exception e, LogLevel level, string message = null, string userName = null);

        Task InsertAsync(Exception e, LogLevel level, string message = null, string userName = null);
    }
}
