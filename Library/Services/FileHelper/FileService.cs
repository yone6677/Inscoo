using Models.Infrastructure;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Services
{
    public class FileService : IFileService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private readonly IResourceService _resource;
        private readonly ILoggerService _loggerService;

        public FileService(HttpContextBase httpContext, IWebHelper webHelper, IResourceService resource, ILoggerService loggerService)
        {
            _httpContext = httpContext;
            _webHelper = webHelper;
            _resource = resource;
            _loggerService = loggerService;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="fileName">文件名称</param>
        public void DownloadFile(string url, string fileName)
        {
            try
            {
                var pUrl = _httpContext.Request.MapPath("~" + url);
                FileStream fs = new FileStream(pUrl, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                HttpContext.Current.Response.BinaryWrite(bytes);
                HttpContext.Current.Response.Flush();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                try
                {
                    HttpContext.Current.Response.End();
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Close();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }
            }
        }
        public void DeleteFile(string url)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                File.Delete(path + url.Substring(1));
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "删除文件失败" + url);
                return;
                // throw e;
            }
        }

        public bool ExportExcel(DataSet ds, string activateName = null)
        {
            try
            {
                HSSFWorkbook wk = new HSSFWorkbook();
                int sheetNum = ds.Tables.Count;
                for (int c = 0; c < sheetNum; c++)
                {
                    var dt = ds.Tables[c];
                    ISheet ist = wk.CreateSheet(ds.Tables[c].TableName);
                    int rowCount = dt.Rows.Count;//行数
                    int columnCount = dt.Columns.Count;//列数  
                    for (int i = 0; i < rowCount; i++)
                    {
                        if (i == 0)
                        {
                            IRow ro = ist.CreateRow(i);//表头
                            for (int x = 0; x < columnCount; x++)//在刚开始写入Sheet第1行的列名
                            {
                                ICell cell = ro.CreateCell(x);
                                cell.SetCellValue(dt.Columns[x].ColumnName);
                            }
                        }
                        else
                        {
                            IRow row = ist.CreateRow(i);//创建第I行,从1开始
                            for (int j = 0; j < columnCount; j++)
                            {
                                string ss = dt.Rows[i][j].ToString().Trim();//获得数据
                                ICell col = row.CreateCell(j);
                                col.SetCellValue(dt.Rows[i][j].ToString().Trim());
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(activateName))
                    {
                        if (ds.Tables[c].TableName.Trim() == activateName.Trim())
                        {
                            wk.SetActiveSheet(c);
                        }
                    }
                }
                //产生文件
                string fileName = _webHelper.GetTimeStamp() + ".xls";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + "/" + _resource.GetFileCatalog() + "/Download/");
                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                string filePath = phyPath + fileName;
                using (FileStream fs = File.OpenWrite(filePath)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    fs.Dispose();
                    DownloadFile(filePath, fileName);
                }
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Excel导出失败");
                return false;
            }
        }

        /// <summary>
        /// 使用反射获取类的属性和值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetProperties<T>(T t)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (t == null)
            {
                return null;
            }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;//实体类字段名称
                string value = item.GetValue(t, null).ToString();//该字段的值

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ret.Add(name, value);        //在此可转换value的类型
                }
            }

            return ret;
        }
        public List<string> GenerateFilePathBySuffix(string postfix)
        {
            var result = new List<string>();
            var filePath = _resource.GetFileCatalog();
            //虚拟路径
            var savePath = "/" + filePath + "/";
            var date = DateTime.Now;
            switch (postfix)
            {
                case ".jpge":
                    savePath += "img/";
                    break;
                case ".jpg":
                    savePath += "img/";
                    break;
                case ".bmp":
                    savePath += "img/";
                    break;
                case ".gif":
                    savePath += "img/";
                    break;
                case ".png":
                    savePath += "img/";
                    break;
                default:
                    savePath = savePath += "doc/";
                    break;
            }
            savePath += date.Year + "/" + date.Month + "/" + date.Day + "/";
            //保存路径
            string phyPath = _httpContext.Request.MapPath("~" + savePath);
            //新文件名
            var ts = _webHelper.GetTimeStamp(null, false);
            var saveName = ts + postfix;
            //如果不存在,创建文件夹    
            if (!Directory.Exists(phyPath))
            {
                Directory.CreateDirectory(phyPath);
            }
            result.Add(phyPath + saveName);//物理路径
            result.Add(savePath + saveName);//虚拟路径
            result.Add(savePath);//保存路径
            result.Add(saveName);//文件名
            result.Add(postfix);//后缀
            return result;

        }
        public SaveResultModel DownFileByUrl(string url)
        {
            try
            {
                var savePath = "/" + _resource.GetFileCatalog() + "/";
                var date = DateTime.Now;
                var arry = url.Split('.');
                var postfix = arry[arry.Length - 1].ToLower();
                switch (postfix)
                {
                    case "jpge":
                        savePath += "img/";
                        break;
                    case "jpg":
                        savePath += "img/";
                        break;
                    case "bmp":
                        savePath += "img/";
                        break;
                    case "gif":
                        savePath += "img/";
                        break;
                    case "png":
                        savePath += "img/";
                        break;
                    default:
                        savePath = savePath += "doc/";
                        break;
                }
                savePath += date.Year + "/" + date.Month + "/" + date.Day + "/";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + savePath);
                //新文件名
                string ts = date.Ticks.ToString();//_webHelper.GetTimeStamp(null, false);
                var saveName = ts + "." + postfix;
                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                string phyFile = phyPath + saveName;
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.DownloadFile(new Uri(url), phyFile);
                return new SaveResultModel() { Name = ts, Postfix = postfix, Path = savePath };
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "文件下载失败:DownFileByUrl");
            }
            return null;
        }
        public List<string> CopyFileByUrl(string url)
        {
            try
            {
                var result = new List<string>();
                var filePath = _resource.GetFileCatalog();
                //虚拟路径
                var savePath = "/" + filePath + "/";
                var date = DateTime.Now;
                var arry = url.Split('.');
                var postfix = arry[arry.Length - 1];
                switch (postfix)
                {
                    case "jpge":
                        savePath += "img/";
                        break;
                    case "jpg":
                        savePath += "img/";
                        break;
                    case "bmp":
                        savePath += "img/";
                        break;
                    case "gif":
                        savePath += "img/";
                        break;
                    case "png":
                        savePath += "img/";
                        break;
                    default:
                        savePath = savePath += "doc/";
                        break;
                }
                savePath += date.Year + "/" + date.Month + "/" + date.Day + "/";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + savePath);
                //新文件名
                var ts = date.Ticks;//_webHelper.GetTimeStamp(null, false);
                var saveName = ts + "." + postfix;
                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                string phyFile = _httpContext.Request.MapPath("~" + url);
                File.Copy(phyFile, phyPath + saveName);//复制文件
                result.Add(phyPath + saveName);//物理路径
                result.Add(savePath + saveName);//虚拟路径
                result.Add(savePath);//保存路径
                result.Add(saveName);//文件名
                result.Add(postfix);//后缀
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "文件复制失败");
            }
            return null;
        }
        public virtual string MakeHtmlFile(string TempName)
        {
            return null;
            //try
            //{
            //    var filePath = "";
            //    switch (TempName)
            //    {
            //        case "Article":
            //            filePath = _httpContext.Server.MapPath("Infrastructure/Template/Article.html");
            //            break;
            //    }
            //    FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            //    StreamReader reader = new StreamReader(fs);
            //    StringBuilder sb = new StringBuilder(reader.ReadToEnd());
            //    reader.Close();
            //    sb.Replace("@title", model.title);
            //    sb.Replace("@content", model.content);
            //    sb.Replace("@author", model.author);
            //    sb.Replace("@date", model.date.ToString());
            //    string newFileName = _webHelper.GetTimeStamp(DateTime.Now) + ".html";
            //    FileStream newFile = File.Create(_httpContext.Server.MapPath("Archive") + "/" + newFileName);
            //    StreamWriter writer = new StreamWriter(newFile, Encoding.UTF8);
            //    writer.Write(sb.ToString());
            //    writer.Flush();
            //    writer.Close();
            //    return "Archive/" + newFileName;
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message);
            //}
        }

        public virtual SaveResultModel SaveFile(HttpPostedFileBase postedFileBase, bool isRename = false)
        {
            try
            {
                //源文件名
                var fileName = Path.GetFileName(postedFileBase.FileName);
                //后缀
                var postfix = Path.GetExtension(postedFileBase.FileName).ToLower();
                var filePath = _resource.GetFileCatalog();
                //虚拟路径
                var savePath = "/" + filePath + "/";
                var date = DateTime.Now;
                switch (postfix)
                {
                    case ".jpge":
                        savePath += "img/";
                        break;
                    case ".jpg":
                        savePath += "img/";
                        break;
                    case ".bmp":
                        savePath += "img/";
                        break;
                    case ".gif":
                        savePath += "img/";
                        break;
                    case ".png":
                        savePath += "img/";
                        break;
                    default:
                        savePath = savePath += "doc/";
                        break;
                }
                savePath += date.Year + "/" + date.Month + "/" + date.Day + "/";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + savePath);
                //新文件名
                var ts = date.Ticks.ToString();//_webHelper.GetTimeStamp(null, false);
                if (isRename) ts = fileName.Split('.')[0] + ts;
                var saveName = ts + postfix;
                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                postedFileBase.SaveAs(phyPath + saveName);
                return new SaveResultModel() { Name = ts, Postfix = postfix, Path = savePath };
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "文件上传失败");
            }
            return null;
        }
        public virtual string SaveProvision(HttpPostedFileBase postedFileBase)
        {
            try
            {
                //源文件名
                var fileName = Path.GetFileNameWithoutExtension(postedFileBase.FileName);
                //后缀
                var postfix = Path.GetExtension(postedFileBase.FileName).ToLower();

                //if (!postfix.Contains("")) throw new Exception("不正确的文件格式");
                var filePath = _resource.GetFileCatalog();
                //虚拟路径
                var savePath = "/" + filePath + "/";
                var date = DateTime.Now;

                savePath = savePath += "provisionPdf/";

                savePath += date.Year + "/" + date.Month + "/";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + savePath);
                //新文件名
                var ts = fileName + _webHelper.GetTimeStamp(null, false);
                var saveName = ts + postfix;

                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                postedFileBase.SaveAs(phyPath + saveName);
                return savePath + saveName;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "文件上传失败");
            }
            return null;
        }
        public virtual SaveResultModel SaveInsuranceExcel(HttpPostedFileBase postedFileBase, string directory)
        {
            try
            {
                //源文件名
                var fileName = Path.GetFileNameWithoutExtension(postedFileBase.FileName);
                //后缀
                var postfix = Path.GetExtension(postedFileBase.FileName).ToLower();

                //if (!postfix.Contains("")) throw new Exception("不正确的文件格式");
                var filePath = _resource.GetFileCatalog();
                //虚拟路径
                var savePath = "/" + filePath + "/";
                var date = DateTime.Now;

                savePath = savePath += $"{directory}/";

                savePath += date.Year + "/" + date.Month + "/";
                //保存路径
                string phyPath = _httpContext.Request.MapPath("~" + savePath);
                //新文件名
                var ts = fileName + _webHelper.GetTimeStamp(null, false);
                var saveName = ts + postfix;

                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                postedFileBase.SaveAs(phyPath + saveName);
                return new SaveResultModel() { Name = ts, Postfix = postfix, Path = savePath };
                //return savePath + saveName;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Fatal, "文件上传失败");
                throw e;
            }
        }

    }
}
