using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Services
{
    public interface IFileService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns>成功时返回路径加文件名,失败返回null</returns>
        string SaveFile(HttpPostedFileBase postedFileBase);

        // string MakeHtmlFile(string TempName, ArticleModel model);
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="activateName"></param>
        /// <returns></returns>
        bool ExportExcel(DataSet ds, string activateName = null);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="fileName">文件名称</param>
        void DownloadFile(string url, string fileName);
        /// <summary>
        /// 使用反射获取类的属性和值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        Dictionary<string, string> GetProperties<T>(T t);
    }
}
