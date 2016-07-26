using Models.Api.Archive;
using Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FileSystem.Controllers
{
    public class FileController : ApiController
    {
        private readonly IFileService _fileService;
        private readonly IArchiveService _archive;
        public FileController(IFileService fileService, IArchiveService archive)
        {
            _fileService = fileService;
            _archive = archive;
        }
        [HttpPut]
        public int DownloadByWechat(DownLoadWechatFileApi model)        {            _fileService.DownloadFileByWechat(model);            //var accesstoken = "KYwsFZFVtW2yDyZwPV90UV0DsYnOOPHTFD6CNLpd-XBg6RN27ezmHzs4BR5hilMvNcb6svJi5P2NUr_fVL-oAZdADXBYYw4_FhdO1Gh296DASINCTUmUumzvE5SuutkYQWOdAGALRJ"; //_wechatService.GetAccessToken();
            //var imgList = "Fx8ukgJYoT3obcyNj9Ro8Dm-e3krCwvjal6-Xib4UXc8qEKrt7ZccNVqTw4PtI8C;9M5lciQ7TJTAG4YqusEqp2NOejt5KazVXUjMTtjmDdPnOQSTchznSDSeQre5OfTv;eH4ELDuOaDbXhIWLfjHj-ArX73JQ1vURJTPnWSgpkiQSge-n7RhnCkFEGGzzIR-w;";            //var ary = imgList.Split(';');            //var s = "";            //var url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + accesstoken + "&media_id=";            //foreach (var a in ary)            //{            //    if (!string.IsNullOrEmpty(a))            //    {            //        var client = new WebClient();            //        client.Encoding = Encoding.UTF8;            //        var bit = client.DownloadData(url + a);            //        string phyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Archive/20160725/" + DateTime.Now.Ticks + ".jpg");            //        FileStream ifs = new FileStream(phyPath, FileMode.OpenOrCreate, FileAccess.Read);            //        StreamReader sr = new StreamReader(ifs);            //        MemoryStream ms = new MemoryStream(bit);            //        Image img = Image.FromStream(ms);            //        Bitmap bmp = new Bitmap(img);            //        img.Dispose();            //        ms.Close();            //        sr.Close();            //        ifs.Close();            //        bmp.Save(phyPath, System.Drawing.Imaging.ImageFormat.Jpeg);            //        bmp.Dispose();            //    }            //}            return 0;        }
    }
}
