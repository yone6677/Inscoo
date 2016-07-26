using FileSystem.Models;
using Models.Api.Archive;
using Models.Infrastructure;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace FileSystem.Controllers
{
    public class FileController : ApiController
    {
        private readonly AppDbContext db = new AppDbContext();
        [HttpPost]
        public int DownloadByWechat(DownLoadWechatFileApi model)
        {
            try
            {
                var client = new WebClient();
                client.Encoding = Encoding.UTF8;
                var bit = client.DownloadData(model.Url);
                var date = DateTime.Now;
                var virPath = "/Archive/Wehcat/" + date.Year + "/" + date.Month + "/" + date.Day + "/";
                //保存路径
                string phyPath = System.Web.Hosting.HostingEnvironment.MapPath("~" + virPath);
                //如果不存在,创建文件夹    
                if (!Directory.Exists(phyPath))
                {
                    Directory.CreateDirectory(phyPath);
                }
                var fileName = date.Ticks + "." + model.MediaType;
                string fullPath = phyPath + fileName;
                using (MemoryStream ms = new MemoryStream(bit))
                {
                    var img = Image.FromStream(ms);
                    img.Save(fullPath);
                    img.Dispose();
                }
                var scheme = HttpContext.Current.Request.Url.Scheme;
                var domain = HttpContext.Current.Request.Url.Authority;
                var item = new ArchiveItem()
                {
                    Author = "",
                    Domain = domain,
                    FromAPi = true,
                    Memo = "来自微信上传",
                    Name = fileName,
                    Path = virPath,
                    Type = "wechatFile",
                    pId = 0,
                    Url = scheme + "://" + domain + virPath + fileName,
                    Discriminator = "wechat"
                };
                db.archive.Add(item);
                db.SaveChanges();
                return item.Id;
            }
            catch (Exception e)
            {
            }
            //var accesstoken = "KYwsFZFVtW2yDyZwPV90UV0DsYnOOPHTFD6CNLpd-XBg6RN27ezmHzs4BR5hilMvNcb6svJi5P2NUr_fVL-oAZdADXBYYw4_FhdO1Gh296DASINCTUmUumzvE5SuutkYQWOdAGALRJ"; //_wechatService.GetAccessToken();
            //var imgList = "Fx8ukgJYoT3obcyNj9Ro8Dm-e3krCwvjal6-Xib4UXc8qEKrt7ZccNVqTw4PtI8C;9M5lciQ7TJTAG4YqusEqp2NOejt5KazVXUjMTtjmDdPnOQSTchznSDSeQre5OfTv;eH4ELDuOaDbXhIWLfjHj-ArX73JQ1vURJTPnWSgpkiQSge-n7RhnCkFEGGzzIR-w;";
            //var ary = imgList.Split(';');
            //var s = "";
            //var url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accesstoken + "&media_id=";
            //foreach (var a in ary)
            //{
            //    if (!string.IsNullOrEmpty(a))
            //    {
            //        var client = new WebClient();
            //        client.Encoding = Encoding.UTF8;
            //        var bit = client.DownloadData(url + a);
            //        string phyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Archive/20160725/" + DateTime.Now.Ticks + ".jpg");
            //        FileStream ifs = new FileStream(phyPath, FileMode.OpenOrCreate, FileAccess.Read);
            //        StreamReader sr = new StreamReader(ifs);
            //        MemoryStream ms = new MemoryStream(bit);
            //        Image img = Image.FromStream(ms);
            //        Bitmap bmp = new Bitmap(img);
            //        img.Dispose();
            //        ms.Close();
            //        sr.Close();
            //        ifs.Close();
            //        bmp.Save(phyPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        bmp.Dispose();
            //    }
            //}
            return 0;
        }
    }
}
