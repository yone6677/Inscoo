using Services;
using System;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly IResourceService _resource;

        public HomeController(IFileService fileService, IResourceService resource)
        {
            _fileService = fileService;
            _resource = resource;
        }
        // GET: Home
        public ActionResult Index()
        {           
            return View();
        }
        public ActionResult About()
        {
            var ex = new Exception("内部错误哦");
            throw new HttpException(404, "页面找不到了", ex);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase fileUrl)
        {
            string name = null;
            //var fileLimit=
            if (fileUrl != null)
            {
                name = _fileService.SaveFile(fileUrl);
            }
            return Content(name);
        }
        public ActionResult Test()
        {
            return View();
        }
        public void ExportExcel()
        {
            var ds = new DataSet();
            var dt = new DataTable();
            dt.TableName = "第一个sheet";
            for (var i = 0; i < 7; i++)
            {
                var dc = new DataColumn();
                dc.ColumnName = "第" + i + "列";
                dt.Columns.Add(dc);
            }
            for (var x = 0; x < 65535; x++)
            {
                object[] aValues = { "第" + x + "行:第1列", "第" + x + "行:第2列", "第" + x + "行:第3列", "第" + x + "行:第4列", "第" + x + "行:第5列", "第" + x + "行:第6列", "第" + x + "行:第7列" };
                var dr = dt.LoadDataRow(aValues, false);
            }

            var ss = dt.Copy();
            ss.TableName = "第二个";
            var ssd = dt.Copy();
            ssd.TableName = "第三个";
            var ddf = dt.Copy();
            ddf.TableName = "第四个";
            ds.Tables.Add(dt);
            ds.Tables.Add(ss);
            ds.Tables.Add(ssd);
            ds.Tables.Add(ddf);
            _fileService.ExportExcel(ds, "第一个sheet");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Upload(HttpPostedFileBase fileUrl)
        {
            if (fileUrl.ContentLength < _resource.GetFileLimit())
            {
                _fileService.SaveFile(fileUrl);
            }
            else
            {
                throw new Exception("警告：上传的文件过大");
                // Response.Write();
            }
        }
    }
}