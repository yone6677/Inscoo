using Microsoft.AspNet.Identity;
using Services;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly IResourceService _resource;
        private readonly INavigationService _navService;
        private readonly IAppUserService _appUserService;
        public HomeController(IFileService fileService, IResourceService resource, INavigationService navService, IAppUserService appUserService)
        {
            _fileService = fileService;
            _resource = resource;
            _navService = navService;
            _appUserService = appUserService;
        }
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            var navs = _navService.GetLeftNavigations(User.Identity.GetUserId());

            if (!navs.Any(n => n.name.Equals("设置")))
            {
                navs.Add(_navService.GetById(16));//设置菜单
            }
            if (!navs.Any(n => n.name.Equals("修改密码")))
            {
                navs.Add(_navService.GetByUrl("UserController", "ChangePassword"));
            }
            if (!navs.Any(n => n.name.Equals("退出")))
            {
                navs.Add(_navService.GetByUrl("AccountController", "signout"));
            }
            var bottomNav = navs.Where(n => n.name == "设置" || n.name == "退出").OrderBy(m => m.sequence).ToList();
            navs.RemoveAll(n => n.name == "设置" || n.name == "退出");

            ViewBag.allNavs = navs;
            ViewBag.navPart1 = navs.Where(n => n.level == 0).ToList();
            ViewBag.navPart2 = bottomNav;

            return PartialView();
        }
        //[AllowAnonymous]
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
        /// <summary>
        /// 头像/NAME
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Portrait()
        {
            if (!User.Identity.IsAuthenticated) return PartialView();
            ViewBag.UserName = User.Identity.Name.Split('@').First();
            var portraitPath = Request.Cookies.Get("PortraitPath");
            if (portraitPath == null)
            {
                var path = _appUserService.FindById(User.Identity.GetUserId()).PortraitPath;
                if (string.IsNullOrEmpty(path)) path = "/Content/img/inscoo.png";
                Request.Cookies.Add(new HttpCookie("PortraitPath") { HttpOnly = true, Expires = DateTime.Now.AddYears(1), Value = path });
            }
            ViewBag.Portrait = Request.Cookies.Get("PortraitPath")?.Value;
            return PartialView();
        }
        public ActionResult Service()
        {
            return PartialView();
        }

    }
}