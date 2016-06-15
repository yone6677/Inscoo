﻿using System;

namespace Services
{
    public interface IResourceService
    {
        /// <summary>
        /// 获取文件大小限制
        /// </summary>
        /// <returns></returns>
        int GetFileLimit();
        /// <summary>
        /// 获取保存文件目录
        /// </summary>
        /// <returns></returns>
        string GetFileCatalog();
        /// <summary>
        /// 获取文件系统URL
        /// </summary>
        /// <returns></returns>
        string GetFileSystem();
        /// <summary>
        /// 获取日志服务器URL
        /// </summary>
        /// <returns></returns>
        string GetLogger();
        /// <summary>
        /// 日志是否开启
        /// </summary>
        /// <returns></returns>
        bool LogEnable();
    }
}
