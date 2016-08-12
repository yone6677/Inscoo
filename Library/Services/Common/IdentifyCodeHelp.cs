using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public class IdentifyCodeHelp
    {

        static List<IdentifyCode> IdentifyCodes;
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Stream GetIdentifyCode(out string code, bool IsForceRefresh = false)
        {
            if (IdentifyCodes == null)
            {
                IdentifyCodes = new List<IdentifyCode>();
            }
            if (IdentifyCodes.Count < 100 || IsForceRefresh)
            {
                IdentifyCodes.Clear();
                var dir = AppDomain.CurrentDomain.BaseDirectory + "Archive/identity/";
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.CreateDirectory(dir);
                }
                var help = new IdentifyCodeHelp();
                for (int i = 0; i < 100; i++)
                {
                    var icode = help.GenerateCheckCodes(5);
                    var stream = help.CreateCheckCodeImage(icode, i);
                    IdentifyCodes.Add(new IdentifyCode() { Code = icode, Stream = stream });
                }

            };
            var index = new Random().Next(0, 99);
            var item = IdentifyCodes[index];
            code = item.Code;
            return item.Stream;
            //.CreateCheckCodeImage(GenerateCheckCodes(4));
        }

        public void ShowAuthCode(Stream stream, out string code)
        {
            Random random = new Random();
            code = random.Next(1000, 9999).ToString();

            Bitmap bitmap = CreateAuthCode(code);
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
        }

        string GenerateCheckCodes(int iCount)
        {
            try
            {
                int number;
                string checkCode = String.Empty;
                int iSeed = DateTime.Now.Millisecond;
                System.Random random = new Random(iSeed);
                for (int i = 0; i < iCount; i++)
                {
                    number = random.Next(10);
                    checkCode += number.ToString();
                }
                return checkCode;
            }
            catch (Exception)
            {
                return "4875";
            }

        }

        private Bitmap CreateAuthCode(string str)
        {
            Font fn = new Font("宋体", 12);
            Brush forecolor = Brushes.Black;
            Brush bgcolor = Brushes.White;
            PointF pf = new PointF(5, 5);
            Bitmap bitmap = new Bitmap(100, 25);
            Rectangle rec = new Rectangle(0, 0, 100, 25);
            Graphics gh = Graphics.FromImage(bitmap);
            gh.FillRectangle(bgcolor, rec);
            gh.DrawString(str, fn, forecolor, pf);
            return bitmap;
        }

        private Stream CreateCheckCodeImage(string checkCode, int index)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return null;
            int iWordWidth = 17;
            int iImageWidth = checkCode.Length * iWordWidth;
            Bitmap image = new Bitmap(iImageWidth, 25);
            Graphics g = Graphics.FromImage(image);

            try
            {

                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //画图片的背景噪音线 
                for (int i = 0; i < 2; i++)
                {
                    int x1 = 0;
                    int x2 = image.Width;
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    if (i == 0)
                    {
                        g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                    }

                }


                for (int i = 0; i < checkCode.Length; i++)
                {

                    string Code = checkCode[i].ToString();
                    int xLeft = iWordWidth * (i);
                    random = new Random(xLeft);
                    int iSeed = DateTime.Now.Millisecond;
                    int iValue = random.Next(iSeed) % 4;
                    if (iValue == 0)
                    {
                        Font font = new Font("Arial", 13, (FontStyle.Bold | System.Drawing.FontStyle.Italic));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Red, 1.5f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 1)
                    {
                        Font font = new System.Drawing.Font("楷体", 13, (FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.DarkRed, 1.3f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 2)
                    {
                        Font font = new System.Drawing.Font("宋体", 13, (System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Green, Color.Blue, 1.2f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 3)
                    {
                        Font font = new System.Drawing.Font("黑体", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Green, 1.8f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                }

                //////画图片的前景噪音点 
                //for (int i = 0; i < 8; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                var url = AppDomain.CurrentDomain.BaseDirectory + @"Archive/identity/identify" + index + ".jpeg";
                image.Save(url, System.Drawing.Imaging.ImageFormat.Jpeg);
                return new System.IO.FileStream(url, FileMode.Open);
                //return ms;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }

    class IdentifyCode
    {
        public String Code { set; get; }
        public Stream Stream { set; get; }

    }
}
