using Services.Infrastructure;
using System;

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
        /// <param name="level"></param>
        /// <param name="Message"></param>
        void insert(Exception e, LogLevel level, string message = null, string userName = null);
    }
}
