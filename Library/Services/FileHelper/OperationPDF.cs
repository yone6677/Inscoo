using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Services.FileHelper
{
    public class OperationPDF : IDisposable
    {
        #region static
        public static BaseFont GetBaseFont(string path = "STSONG.ttf")
        {
            path = AppDomain.CurrentDomain.BaseDirectory + @"fonts\" + path;
            var basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return basefont;
        }
        public static Font GetFont(string path = "STSONG.ttf", float fontSize = 12, int style = 0)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + @"fonts\" + path;
            var basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return new Font(basefont, fontSize, style);
        }
        public static Font GetTitleFont(string path = "STSONG.ttf")
        {
            path = AppDomain.CurrentDomain.BaseDirectory + @"fonts\" + path;
            var basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return new Font(basefont, 17, 1);
        }
        public static Font GetUnderLineFont(string path = "STSONG.ttf")
        {
            path = AppDomain.CurrentDomain.BaseDirectory + @"fonts\" + path;
            var basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return new Font(basefont, 12, 4);
        }
        public static Font GetFontBySystemFont(string path = "STSONG.ttf")
        {
            path = @"C:\Windows\Fonts\" + path;
            var basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            return new Font(basefont, 12);
        }


        #endregion
        #region 私有字段
        private Font font;
        private Rectangle rect;   //文档大小
        private Document document;//文档对象
        private BaseFont basefont;//字体
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationPDF()
        {
            rect = PageSize.A6;
            document = new Document(rect);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">页面大小(如"A4")</param>
        public OperationPDF(string type)
        {
            SetPageSize(type);
            document = new Document(rect);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">页面大小(如"A4")</param>
        /// <param name="marginLeft">内容距左边框距离</param>
        /// <param name="marginRight">内容距右边框距离</param>
        /// <param name="marginTop">内容距上边框距离</param>
        /// <param name="marginBottom">内容距下边框距离</param>
        public OperationPDF(string type, float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            SetPageSize(type);
            document = new Document(rect, marginLeft, marginRight, marginTop, marginBottom);
        }
        #endregion



        #region 设置字体
        /// <summary>
        /// 设置字体
        /// </summary>
        public void SetBaseFont(string path)
        {
            basefont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="size">字体大小</param>
        public void SetFont(float size)
        {
            font = new Font(basefont, size);
        }
        #endregion

        #region 设置页面大小
        /// <summary>
        /// 设置页面大小
        /// </summary>
        /// <param name="type">页面大小(如"A4")</param>
        public void SetPageSize(string type)
        {
            switch (type.Trim())
            {
                case "A4":
                    rect = PageSize.A4;
                    break;
                case "A8":
                    rect = PageSize.A8;
                    break;
            }
        }
        #endregion

        #region 实例化文档
        /// <summary>
        /// 实例化文档
        /// </summary>
        /// <param name="os">文档相关信息（如路径，打开方式等）</param>
        public void GetInstance(Stream os)
        {
            PdfWriter.GetInstance(document, os);
        }
        #endregion

        #region 打开文档对象
        /// <summary>
        /// 打开文档对象
        /// </summary>
        /// <param name="os">文档相关信息（如路径，打开方式等）</param>
        public void Open(Stream os)
        {
            GetInstance(os);
            document.Open();
        }
        #endregion

        #region 关闭打开的文档
        /// <summary>
        /// 关闭打开的文档
        /// </summary>
        public void Close()
        {
            document.Close();
        }
        #endregion

        #region 添加段落
        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        public void AddParagraph(string content, float fontsize)
        {
            SetFont(fontsize);
            Paragraph pra = new Paragraph(content, font);
            document.Add(pra);
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="Alignment">对齐方式（1为居中，0为居左，2为居右）</param>
        /// <param name="SpacingAfter">段后空行数（0为默认值）</param>
        /// <param name="SpacingBefore">段前空行数（0为默认值）</param>
        /// <param name="MultipliedLeading">行间距（0为默认值）</param>
        public void AddParagraph(string content, float fontsize, int Alignment, float SpacingAfter, float SpacingBefore, float MultipliedLeading)
        {
            SetFont(fontsize);
            Paragraph pra = new Paragraph(content, font);
            pra.Alignment = Alignment;
            if (SpacingAfter != 0)
            {
                pra.SpacingAfter = SpacingAfter;
            }
            if (SpacingBefore != 0)
            {
                pra.SpacingBefore = SpacingBefore;
            }
            if (MultipliedLeading != 0)
            {
                pra.MultipliedLeading = MultipliedLeading;
            }
            document.Add(pra);
        }
        #endregion

        #region 添加图片
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <param name="Alignment">对齐方式（1为居中，0为居左，2为居右）</param>
        /// <param name="newWidth">图片宽（0为默认值，如果宽度大于页宽将按比率缩放）</param>
        /// <param name="newHeight">图片高</param>
        public void AddImage(string path, int Alignment, float newWidth, float newHeight)
        {
            Image img = Image.GetInstance(path);
            img.Alignment = Alignment;
            if (newWidth != 0)
            {
                img.ScaleAbsolute(newWidth, newHeight);
            }
            else
            {
                if (img.Width > PageSize.A4.Width)
                {
                    img.ScaleAbsolute(rect.Width, img.Width * img.Height / rect.Height);
                }
            }
            document.Add(img);
        }
        #endregion

        #region 添加链接、点
        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="Content">链接文字</param>
        /// <param name="FontSize">字体大小</param>
        /// <param name="Reference">链接地址</param>
        public void AddAnchorReference(string Content, float FontSize, string Reference)
        {
            SetFont(FontSize);
            Anchor auc = new Anchor(Content, font);
            auc.Reference = Reference;
            document.Add(auc);
        }

        /// <summary>
        /// 添加链接点
        /// </summary>
        /// <param name="Content">链接文字</param>
        /// <param name="FontSize">字体大小</param>
        /// <param name="Name">链接点名</param>
        public void AddAnchorName(string Content, float FontSize, string Name)
        {
            SetFont(FontSize);
            Anchor auc = new Anchor(Content, font);
            auc.Name = Name;
            document.Add(auc);
        }
        #endregion


        #region 添加表格
        public void AddDataTable(DataTable dt_content)
        {
            string fontpath_Title = @"C:\Windows\Fonts\SIMHEI.TTF";
            string fontpath_Col = @"C:\Windows\Fonts\SIMHEI.TTF";
            string FontPath = @"C:\Windows\Fonts\SIMHEI.TTF";
            float fontsize_Title = 10;
            float fontsize_Col = 8;
            int fontStyle_Title = iTextSharp.text.Font.BOLD;
            BaseColor fontColor_Title = BaseColor.RED;
            int fontStyle_Col = 11;
            BaseColor fontColor_Col = BaseColor.BLACK;
            int FontSize = 11;
            int fontStyle_Context = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Context = BaseColor.BLACK;
            //根据数据表内容创建一个PDF格式的表
            PdfPTable table = new PdfPTable(dt_content.Columns.Count);
            table.TotalWidth = 800f;//表格总宽度
            table.LockedWidth = true;//锁定宽度
            float[] arr_Width = { 40f, 40f, 100f, 60f, 80f, 60f, 60f, 60f, 100f, 200f };
            table.SetWidths(arr_Width);//设置每列宽度

            //标题字体
            BaseFont basefont_Title = BaseFont.CreateFont(
              fontpath_Title,
              BaseFont.IDENTITY_H,
              BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font_Title = new iTextSharp.text.Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
            //表格列字体
            BaseFont basefont_Col = BaseFont.CreateFont(
              fontpath_Col,
              BaseFont.IDENTITY_H,
              BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font_Col = new iTextSharp.text.Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
            //正文字体
            BaseFont basefont_Context = BaseFont.CreateFont(
              FontPath,
              BaseFont.IDENTITY_H,
              BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font_Context = new iTextSharp.text.Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

            //构建列头
            //设置列头背景色
            table.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            //设置列头文字水平、垂直居中
            table.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
            table.HeaderRows = 1;
            if (dt_content.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt_content.Columns)
                {
                    table.AddCell(new Phrase(dc.ColumnName, font_Col));
                }

                // 添加数据
                //设置标题靠左居中
                table.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                // 设置表体背景色
                table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                for (int i = 0; i < dt_content.Rows.Count; i++)
                {
                    for (int j = 0; j < dt_content.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt_content.Rows[i][j].ToString(), font_Context));
                    }
                }
            }

            //如果最后一个单元格数据过多，不要移动到下一页显示
            table.SplitLate = false;
            //
            table.SplitRows = false;
            //在目标文档中添加转化后的表数据
            document.Add(table);

        }
        #endregion
        #region 添加List<string>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">数据</param>
        /// <param name="columnNum">列数</param>
        /// <param name="isHeaderExist">如果是，就以第一个元素作为主标题</param>
        public void AddStringList(FileStream stream, List<string> list, int columnNum, bool isHeaderExist)
        {
            PdfWriter.GetInstance(document, stream);

            //生成文件的同时，在数据库中增加一条记录
            document.Open();
            //document.AddLanguage("zh-cn");
            PdfPTable table = new PdfPTable(columnNum);
            //table.SetWidths(new int[] { 1, 2, 3, 2, 4, 1, 2, 2, 1 });
            if (isHeaderExist)
            {
                PdfPCell cell;
                cell = new PdfPHeaderCell() { Name = list.First(), Colspan = 9 };
                table.AddCell(cell);
                list.RemoveAt(0);
            }

            table.AddCell("序号");
            table.AddCell("方案");
            table.AddCell("被保险人名称");
            table.AddCell("证件类型");
            table.AddCell("证件号码");
            table.AddCell("类型（本人/子女）");
            table.AddCell("生效日");
            table.AddCell("结束日");
            table.AddCell("社保");
            // var index = 1;
            //foreach (var item in pdfData)
            //{
            //    table.AddCell(index.ToString());
            //    table.AddCell(item.planName);
            //    table.AddCell(item.Name);
            //    table.AddCell(item.CredentialType);
            //    table.AddCell(item.CredentialCode);
            //    table.AddCell(item.insurantType);
            //    table.AddCell(item.EffectiveDate.HasValue ? item.EffectiveDate.Value.ToShortDateString() : "无效人员信息");
            //    table.AddCell(item.EndDate.HasValue ? item.EndDate.Value.ToShortDateString() : "无效人员信息");
            //    table.AddCell(item.IsSocialInsuranceHold);
            //    index++;
            //}
            document.Add(table);
            document.Close();

        }
        #endregion

        #region 添加Other

        #endregion


        #region 单一文件导出PDF
        /// <summary>
        /// 转换GridView为PDF文档
        /// </summary>
        /// <param name="sdr_Context">SqlDataReader</param>
        /// <param name="title">标题名称</param>
        /// <param name="fontpath_Title">标题字体路径</param>
        /// <param name="fontsize_Title">标题字体大小</param>
        /// <param name="fontStyle_Title">标题样式</param>
        /// <param name="fontColor_Title">标题颜色</param>
        /// <param name="fontpath_Col">列头字体路径</param>
        /// <param name="fontsize_Col">列头字体大小</param>
        /// <param name="fontStyle_Col">列头字体样式</param>
        /// <param name="fontColor_Col">列头字体颜色</param>
        /// <param name="col_Width">表格总宽度</param>
        /// <param name="arr_Width">每列的宽度</param>
        /// <param name="pdf_Filename">在服务器端保存PDF时的文件名</param>
        /// <param name="FontPath">正文字体路径</param>
        /// <param name="FontSize">正文字体大小</param>
        ///  <param name="fontStyle_Context">正文字体样式</param>
        ///  <param name="fontColor_Context">正文字体颜色</param>
        /// <returns>返回调用是否成功</returns>
        public void exp_Pdf(DataTable dt_content, string title, string fontpath_Title, float fontsize_Title, int fontStyle_Title, BaseColor fontColor_Title, string fontpath_Col, float fontsize_Col, int fontStyle_Col, BaseColor fontColor_Col, float col_Width, float[] arr_Width, string pdf_Filename, string FontPath, float FontSize, int fontStyle_Context, BaseColor fontColor_Context)
        {
            //在服务器端保存PDF时的文件名
            //string strFileName = pdf_Filename + ".pdf";
            //初始化一个目标文档类 
            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);
            //调用PDF的写入方法流
            //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
            //PdfWriter writer = PdfWriter.GetInstance(document,
            //    new FileStream(HttpContext.Current.Server.MapPath(strFileName), FileMode.Create));
            PdfWriter writer = PdfWriter.GetInstance(document,
                new FileStream(pdf_Filename, FileMode.Create));
            try
            {
                //标题字体
                BaseFont basefont_Title = BaseFont.CreateFont(
                  fontpath_Title,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Title = new iTextSharp.text.Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
                //表格列字体
                BaseFont basefont_Col = BaseFont.CreateFont(
                  fontpath_Col,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Col = new iTextSharp.text.Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
                //正文字体
                BaseFont basefont_Context = BaseFont.CreateFont(
                  FontPath,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Context = new iTextSharp.text.Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

                //打开目标文档对象
                document.Open();

                //添加标题
                Paragraph p_Title = new Paragraph(title, font_Title);
                p_Title.Alignment = Element.ALIGN_CENTER;
                p_Title.Leading = 14;
                document.Add(p_Title);

                //添加一个5列的数据表
                PdfPTable table1 = new PdfPTable(2);
                PdfPCell pcell;
                //                pcell = new PdfPCell(iTextSharp.text.Image.GetInstance("D:\\Project\\project_2014\\images\\users\\6.png"));

                //pcell = new PdfPCell(iTextSharp.text.Image.GetInstance("D:\\WebApp\\OA\\OASystem\\upldDoc\\1.jpg"));


                pcell = new PdfPCell();
                table1.AddCell(pcell);


                PdfPTable table12 = new PdfPTable(4);
                table12.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.BLUE;



                /*
                table12.AddCell(new Phrase("姓名：" + txtName.Text.ToString(), font_Col));
                table12.AddCell(new Phrase("手机：" + txtPhone.Text.ToString(), font_Col));
                table12.AddCell(new Phrase("邮箱：" + txtEmail.Text.ToString(), font_Col));
                table12.AddCell(new Phrase("性别：" + txtSex.Text.ToString(), font_Col));
                 
                table12.AddCell(new Phrase("姓名：" + "aaa", font_Col));
                table12.AddCell(new Phrase("手机：" + "bbb", font_Col));
                table12.AddCell(new Phrase("邮箱：" + "ccc", font_Col));
                table12.AddCell(new Phrase("性别：" + "ddd", font_Col));
                */

                Phrase pname = new Phrase("姓名：", font_Col);

                table12.AddCell(pname);
                table12.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.RED;
                table12.AddCell(new Phrase("aaa", font_Col));
                table12.AddCell(new Phrase("手机：", font_Col));
                table12.AddCell(new Phrase("bbb", font_Col));
                table12.AddCell(new Phrase("邮箱：", font_Col));
                table12.AddCell(new Phrase("ccc", font_Col));
                table12.AddCell(new Phrase("性别：", font_Col));
                table12.AddCell(new Phrase("ddd", font_Col));

                table1.AddCell(table12);

                //根据数据表内容创建一个PDF格式的表
                PdfPTable table = new PdfPTable(dt_content.Columns.Count);
                table.TotalWidth = col_Width;//表格总宽度
                table.LockedWidth = true;//锁定宽度
                table.SetWidths(arr_Width);//设置每列宽度

                //构建列头
                //设置列头背景色
                table.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                table.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                table.HeaderRows = 1;
                if (dt_content.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dt_content.Columns)
                    {
                        table.AddCell(new Phrase(dc.ColumnName, font_Col));
                    }

                    // 添加数据
                    //设置标题靠左居中
                    table.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    table.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dt_content.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt_content.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(dt_content.Rows[i][j].ToString(), font_Context));
                        }
                    }
                }

                //如果最后一个单元格数据过多，不要移动到下一页显示
                table.SplitLate = false;
                //
                table.SplitRows = false;
                //在目标文档中添加转化后的表数据
                document.Add(table1);
                document.Add(table);
                //table.DefaultCell.Width = 0.5F;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //关闭目标文件
                document.Close();
                //关闭写入流
                writer.Close();
            }


            /*
            // 弹出提示框，提示用户是否下载保存到本地
            try
            {
                //这里是你文件在项目中的位置,根目录下就这么写 
                String FullFileName = System.Web.HttpContext.Current.Server.MapPath(strFileName);
                FileInfo DownloadFile = new FileInfo(FullFileName);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Buffer = false;
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
                    + System.Web.HttpUtility.UrlEncode(DownloadFile.FullName, System.Text.Encoding.UTF8));
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(DownloadFile.FullName);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
             * */
        }
        #endregion


        private string ReplaceValidateString(string strSource)
        {
            string strRetrun = strSource;



            strRetrun = Regex.Replace(strRetrun, @"&nbsp;", " ");
            strRetrun = Regex.Replace(strRetrun, @"\r\n\t\t\t</td>", "-^-");
            strRetrun = Regex.Replace(strRetrun, @"\r\n\t\t</tr>", "-~-");

            //删除HTML            
            strRetrun = Regex.Replace(strRetrun, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"-->", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"<!--.*", "", RegexOptions.IgnoreCase);
            //删除脚本
            strRetrun = Regex.Replace(strRetrun, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            strRetrun = strRetrun.Replace("\t", "");
            strRetrun = strRetrun.Replace("\r", "");
            strRetrun = strRetrun.Replace("\n", "");
            strRetrun = strRetrun.Replace("-^-", "  \t  ");
            strRetrun = strRetrun.Replace("-~-", "\r\n\t");


            /*
            //删除脚本
            strRetrun = Regex.Replace(strRetrun, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            strRetrun = Regex.Replace(strRetrun, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"-->", "", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"<!--.*", "", RegexOptions.IgnoreCase);

            strRetrun = Regex.Replace(strRetrun, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            strRetrun = Regex.Replace(strRetrun, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            strRetrun = strRetrun.Replace("\"", "\'");
            strRetrun = strRetrun.Replace("\r\n", "<br>");
            strRetrun = strRetrun.Replace("\n", "");
            strRetrun = strRetrun.Replace("\r", "");
            strRetrun = strRetrun.Replace("\t", "");
            strRetrun = strRetrun.Replace("<", "");
            strRetrun = strRetrun.Replace(">", "");
            //strRetrun = strRetrun.Replace("\r\n", "");
            */
            return strRetrun;

        }



        #region 文档汇报
        /// <summary>
        /// 文档汇报 转换GridView为PDF文档
        /// </summary>
        /// <param name="sdr_Context">SqlDataReader</param>
        /// <param name="title">标题名称</param>
        /// <param name="col_Width">表格总宽度</param>
        /// <param name="strServerMapPath">服务器目录路径</param>
        /// <param name="pdf_Filename">在服务器端保存PDF时的文件名</param>
        /// <returns>返回调用是否成功   0 失败,1成功</returns>
        public int ImportDocuReriewPDF(DataTable dtDocuReview, DataTable dtSignLog, DataTable dtAttachFiles, string title, float col_Width, string strServerMapPath, string pdf_Filename)
        {
            int iReturn = 1;
            //标题
            string fontpath_Title = @"c:\\windows\\FONTS\\simsun.ttc,1";  //标题字体路径
            float fontsize_Title = 14;                                  //标题字体大小
            int fontStyle_Title = iTextSharp.text.Font.BOLD;           //标题样式
            BaseColor fontColor_Title = BaseColor.RED;                  //标题颜色
            //列头
            string fontpath_Col = @"c:\\windows\\FONTS\\simsun.ttc,1";      //列头字体、大小、样式、颜色
            float fontsize_Col = 11;
            int fontStyle_Col = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Col = BaseColor.BLACK;
            //正文
            string FontPath = "c:\\windows\\FONTS\\simsun.ttc,1";       ////正文字体、大小、样式、颜色
            float FontSize = 11;
            int fontStyle_Context = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Context = BaseColor.BLACK;


            //在服务器端保存PDF时的文件名
            //string strFileName = pdf_Filename + ".pdf";
            //初始化一个目标文档类 
            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);
            //调用PDF的写入方法流
            //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
            PdfWriter writer = PdfWriter.GetInstance(document,
                new FileStream(pdf_Filename, FileMode.Create));
            try
            {
                //标题字体
                BaseFont basefont_Title = BaseFont.CreateFont(
                  fontpath_Title,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Title = new iTextSharp.text.Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
                //表格列字体
                BaseFont basefont_Col = BaseFont.CreateFont(
                  fontpath_Col,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Col = new iTextSharp.text.Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
                //正文字体
                BaseFont basefont_Context = BaseFont.CreateFont(
                  FontPath,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Context = new iTextSharp.text.Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

                //打开目标文档对象
                document.Open();

                //添加标题
                Paragraph p_Title = new Paragraph(title, font_Title);
                p_Title.Alignment = Element.ALIGN_CENTER;
                p_Title.Leading = 14;
                p_Title.MultipliedLeading = 1;
                p_Title.SpacingAfter = 10;
                document.Add(p_Title);




                //添加一个4列的数据表
                PdfPTable table1Main = new PdfPTable(4);
                table1Main.TotalWidth = col_Width;//表格总宽度
                table1Main.LockedWidth = true;//锁定宽度
                table1Main.SetWidths(new float[] { 100f, 300f, 100f, 300f });//设置每列宽度


                table1Main.TotalWidth = col_Width;
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("文档编号：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtDocuReview.Rows[0]["DocuNo"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("所属公司：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtDocuReview.Rows[0]["CompanyNameC"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("文档主题：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtDocuReview.Rows[0]["DocuTitle"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("文档类型：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtDocuReview.Rows[0]["DocuTypeName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("备注：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                string strDocuContent = ReplaceValidateString(dtDocuReview.Rows[0]["DocuContent"].ToString().Trim());
                PdfPCell cell = new PdfPCell(new Phrase(strDocuContent, font_Col));
                cell.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cell);







                //---------------------------------------------------------------------
                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableAttach = new PdfPTable(dtAttachFiles.Columns.Count);
                tableAttach.TotalWidth = col_Width;//表格总宽度
                tableAttach.LockedWidth = true;//锁定宽度
                tableAttach.SetWidths(new float[] { 500f, 150f, 150f });//设置每列宽度

                //构建列头
                //设置列头背景色
                tableAttach.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableAttach.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableAttach.HeaderRows = 1;
                if (dtAttachFiles.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtAttachFiles.Columns)
                    {
                        tableAttach.AddCell(new Phrase(dc.ColumnName, font_Col));
                    }
                    // 添加数据
                    //设置标题靠左居中
                    tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableAttach.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtAttachFiles.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtAttachFiles.Columns.Count; j++)
                        {
                            tableAttach.AddCell(new Phrase(dtAttachFiles.Rows[i][j].ToString(), font_Context));
                        }
                    }
                }




                //---------------------------------------------------------------------

                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableSignLog = new PdfPTable(dtSignLog.Columns.Count);        //有一列为签名档URL
                tableSignLog.TotalWidth = col_Width;//表格总宽度
                tableSignLog.LockedWidth = true;//锁定宽度
                tableSignLog.SetWidths(new float[] { 50f, 50f, 50f, 50f, 400f, 100f, 100f });//设置每列宽度



                //构建列头
                //设置列头背景色
                tableSignLog.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableSignLog.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableSignLog.HeaderRows = 1;
                if (dtSignLog.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtSignLog.Columns)
                    {
                        if (dc.ColumnName.ToString().Trim() != "SignatureURL")
                        {
                            tableSignLog.AddCell(new Phrase(dc.ColumnName, font_Col));
                        }
                        else
                        {
                            tableSignLog.AddCell(new Phrase("签名", font_Col));
                        }
                    }

                    // 添加数据
                    //设置标题靠左居中
                    tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableSignLog.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtSignLog.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtSignLog.Columns.Count - 1; j++)
                        {
                            tableSignLog.AddCell(new Phrase(dtSignLog.Rows[i][j].ToString(), font_Context));
                        }

                        string strSignatureURL = dtSignLog.Rows[i]["SignatureURL"].ToString().Trim();
                        if (strSignatureURL == "")
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\4.jpg";
                        }
                        else
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\" + strSignatureURL;
                        }


                        Image img1 = Image.GetInstance(strSignatureURL);
                        img1.Alignment = 1;              //对齐方式（1为居中，0为居左，2为居右）
                        img1.ScaleAbsolute(150, 150);

                        tableSignLog.AddCell(img1);
                    }
                }


                document.Add(table1Main);


                //增加空白行
                Paragraph p_Attach = new Paragraph("附 件", font_Title);
                p_Attach.Alignment = Element.ALIGN_CENTER;
                p_Attach.Leading = 14;
                p_Attach.SpacingBefore = 10;
                p_Attach.SpacingAfter = 10;
                document.Add(p_Attach);

                document.Add(tableAttach);

                //如果最后一个单元格数据过多，不要移动到下一页显示
                tableSignLog.SplitLate = false;
                tableSignLog.SplitRows = false;
                //在目标文档中添加转化后的表数据

                //增加空白行
                Paragraph p_SignLog = new Paragraph("签核记录", font_Title);
                p_SignLog.Alignment = Element.ALIGN_CENTER;
                p_SignLog.Leading = 14;
                p_SignLog.SpacingBefore = 10;
                p_SignLog.SpacingAfter = 10;
                document.Add(p_SignLog);

                document.Add(tableSignLog);
                //table.DefaultCell.Width = 0.5F;



                int iPageCount = writer.PageNumber;

                string strFooder = "打印日期：" + DateTime.Now.ToLongDateString() + "   " + DateTime.Now.ToLongTimeString() + "    共 " + iPageCount.ToString() + "页";
                Paragraph p_Fooder = new Paragraph(strFooder, font_Context);
                p_Fooder.Alignment = Element.ALIGN_BOTTOM;
                p_Fooder.Leading = 14;
                p_Fooder.MultipliedLeading = 1;
                p_Fooder.SpacingBefore = 20;
                p_Fooder.SpacingAfter = 0;
                document.Add(p_Fooder);

                /*
                Image img1 = Image.GetInstance("D:\\WebApp\\OA\\OASystem\\upldPic\\1.jpg");
                img1.Alignment = 0;              //对齐方式（1为居中，0为居左，2为居右）
                img1.ScaleAbsolute(150, 150);
                document.Add(img1);

                Image img2 = Image.GetInstance("D:\\WebApp\\OA\\OASystem\\upldPic\\2.jpg");
                img2.Alignment = 0;              //对齐方式（1为居中，0为居左，2为居右）
                img2.ScaleAbsolute(150, 150);
                document.Add(img2);

                Image img3 = Image.GetInstance("D:\\WebApp\\OA\\OASystem\\upldPic\\3.jpg");
                img3.Alignment = 0;              //对齐方式（1为居中，0为居左，2为居右）
                img3.ScaleAbsolute(150, 150);
                document.Add(img3);
                 */
            }
            catch (Exception)
            {
                iReturn = 0;
                throw;
            }
            finally
            {
                //关闭目标文件
                document.Close();
                //关闭写入流
                writer.Close();

            }
            return iReturn;
        }
        #endregion

        #region 合同信息
        /// <summary>
        /// 合同信息 转换GridView为PDF文档
        /// </summary>
        /// <param name="sdr_Context">SqlDataReader</param>
        /// <param name="title">标题名称</param>
        /// <param name="col_Width">表格总宽度</param>
        /// <param name="strServerMapPath">服务器目录路径</param>
        /// <param name="pdf_Filename">在服务器端保存PDF时的文件名</param>
        /// <returns>返回调用是否成功   0 失败,1成功</returns>
        public int ImportContractPDF(DataTable dtMaininfo, DataTable dtSignLog, DataTable dtAttachFiles, string title, float col_Width, string strServerMapPath, string pdf_Filename)
        {
            int iReturn = 1;
            //标题
            string fontpath_Title = @"c:\\windows\\FONTS\\simsun.ttc,1";  //标题字体路径
            float fontsize_Title = 14;                                  //标题字体大小
            int fontStyle_Title = iTextSharp.text.Font.BOLD;           //标题样式
            BaseColor fontColor_Title = BaseColor.RED;                  //标题颜色
            //列头
            string fontpath_Col = @"c:\\windows\\FONTS\\simsun.ttc,1";      //列头字体、大小、样式、颜色
            float fontsize_Col = 11;
            int fontStyle_Col = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Col = BaseColor.BLACK;
            //正文
            string FontPath = "c:\\windows\\FONTS\\simsun.ttc,1";       ////正文字体、大小、样式、颜色
            float FontSize = 11;
            int fontStyle_Context = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Context = BaseColor.BLACK;


            //在服务器端保存PDF时的文件名
            //string strFileName = pdf_Filename + ".pdf";
            //初始化一个目标文档类 
            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);
            //调用PDF的写入方法流
            //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
            PdfWriter writer = PdfWriter.GetInstance(document,
                new FileStream(pdf_Filename, FileMode.Create));
            try
            {
                //标题字体
                BaseFont basefont_Title = BaseFont.CreateFont(
                  fontpath_Title,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Title = new iTextSharp.text.Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
                //表格列字体
                BaseFont basefont_Col = BaseFont.CreateFont(
                  fontpath_Col,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Col = new iTextSharp.text.Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
                //正文字体
                BaseFont basefont_Context = BaseFont.CreateFont(
                  FontPath,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Context = new iTextSharp.text.Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

                //打开目标文档对象
                document.Open();

                //添加标题
                Paragraph p_Title = new Paragraph(title, font_Title);
                p_Title.Alignment = Element.ALIGN_CENTER;
                p_Title.Leading = 14;
                p_Title.MultipliedLeading = 1;
                p_Title.SpacingAfter = 10;
                document.Add(p_Title);




                //添加一个4列的数据表
                PdfPTable table1Main = new PdfPTable(4);
                table1Main.TotalWidth = col_Width;//表格总宽度
                table1Main.LockedWidth = true;//锁定宽度
                table1Main.SetWidths(new float[] { 100f, 300f, 100f, 300f });//设置每列宽度


                table1Main.TotalWidth = col_Width;
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同摘要：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同类别：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractTypeName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同编号：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractNo"].ToString().Trim(), font_Col));

                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("对方公司：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractCustomerName"].ToString().Trim(), font_Col));


                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同总金额：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractTotalMoney"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("签约公司：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["CompanyNameC"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("对方业务员：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractBriefName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("邮 箱 ：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractEmail"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("传 真：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractFax"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("电 话：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractTel"].ToString().Trim(), font_Col));


                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("发票类型：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["InvoiceTypeShow"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("付款方式：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ContractPayTypeShow"].ToString().Trim(), font_Col));

                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同签核状态：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellStatus = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["ContractStatusShow"].ToString().Trim(), font_Col));
                cellStatus.Colspan = 3;
                table1Main.AddCell(cellStatus);


                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("地 址：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellAddr = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["ContractAddr"].ToString().Trim(), font_Col));
                cellAddr.Colspan = 3;
                table1Main.AddCell(cellAddr);



                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同内容：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                string strContractContent = ReplaceValidateString(dtMaininfo.Rows[0]["ContractContent"].ToString().Trim());
                PdfPCell cellMemo = new PdfPCell(new Phrase(strContractContent, font_Col));
                cellMemo.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellMemo);


                /*
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同执行状态：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellMaintainShow = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["ContrStatusMaintainShow"].ToString().Trim(), font_Col));
                cellMaintainShow.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellMaintainShow);


                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("到货情况：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellGoodsInfo = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["GoodsInfo"].ToString().Trim(), font_Col));
                cellGoodsInfo.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellGoodsInfo);


                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("发票情况：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellInvoiceInfo = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["InvoiceInfo"].ToString().Trim(), font_Col));
                cellInvoiceInfo.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellInvoiceInfo);

                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同执行情况：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellContrExeInfo = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["ContrExeInfo"].ToString().Trim(), font_Col));
                cellContrExeInfo.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellContrExeInfo);
                */


                //---------------------------------------------------------------------
                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableAttach = new PdfPTable(dtAttachFiles.Columns.Count);
                tableAttach.TotalWidth = col_Width;//表格总宽度
                tableAttach.LockedWidth = true;//锁定宽度
                tableAttach.SetWidths(new float[] { 500f, 150f, 150f });//设置每列宽度

                //构建列头
                //设置列头背景色
                tableAttach.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableAttach.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableAttach.HeaderRows = 1;
                if (dtAttachFiles.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtAttachFiles.Columns)
                    {
                        tableAttach.AddCell(new Phrase(dc.ColumnName, font_Col));
                    }
                    // 添加数据
                    //设置标题靠左居中
                    tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableAttach.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtAttachFiles.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtAttachFiles.Columns.Count; j++)
                        {
                            tableAttach.AddCell(new Phrase(dtAttachFiles.Rows[i][j].ToString(), font_Context));
                        }
                    }
                }




                //---------------------------------------------------------------------

                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableSignLog = new PdfPTable(dtSignLog.Columns.Count);        //有一列为签名档URL
                tableSignLog.TotalWidth = col_Width;//表格总宽度
                tableSignLog.LockedWidth = true;//锁定宽度
                tableSignLog.SetWidths(new float[] { 50f, 50f, 50f, 50f, 400f, 100f, 100f });//设置每列宽度



                //构建列头
                //设置列头背景色
                tableSignLog.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableSignLog.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableSignLog.HeaderRows = 1;
                if (dtSignLog.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtSignLog.Columns)
                    {
                        if (dc.ColumnName.ToString().Trim() != "SignatureURL")
                        {
                            tableSignLog.AddCell(new Phrase(dc.ColumnName, font_Col));
                        }
                        else
                        {
                            tableSignLog.AddCell(new Phrase("签名", font_Col));
                        }
                    }

                    // 添加数据
                    //设置标题靠左居中
                    tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableSignLog.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtSignLog.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtSignLog.Columns.Count - 1; j++)
                        {
                            tableSignLog.AddCell(new Phrase(dtSignLog.Rows[i][j].ToString(), font_Context));
                        }

                        string strSignatureURL = dtSignLog.Rows[i]["SignatureURL"].ToString().Trim();
                        if (strSignatureURL == "")
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\4.jpg";
                        }
                        else
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\" + strSignatureURL;
                        }


                        Image img1 = Image.GetInstance(strSignatureURL);
                        img1.Alignment = 1;              //对齐方式（1为居中，0为居左，2为居右）
                        img1.ScaleAbsolute(150, 150);

                        tableSignLog.AddCell(img1);
                    }
                }


                document.Add(table1Main);


                //增加空白行
                Paragraph p_Attach = new Paragraph("附 件", font_Title);
                p_Attach.Alignment = Element.ALIGN_CENTER;
                p_Attach.Leading = 14;
                p_Attach.SpacingBefore = 10;
                p_Attach.SpacingAfter = 10;
                document.Add(p_Attach);

                document.Add(tableAttach);

                //如果最后一个单元格数据过多，不要移动到下一页显示
                tableSignLog.SplitLate = false;
                tableSignLog.SplitRows = false;
                //在目标文档中添加转化后的表数据

                //增加空白行
                Paragraph p_SignLog = new Paragraph("签核记录", font_Title);
                p_SignLog.Alignment = Element.ALIGN_CENTER;
                p_SignLog.Leading = 14;
                p_SignLog.SpacingBefore = 10;
                p_SignLog.SpacingAfter = 10;
                document.Add(p_SignLog);

                document.Add(tableSignLog);
                //table.DefaultCell.Width = 0.5F;



                int iPageCount = writer.PageNumber;

                string strFooder = "打印日期：" + DateTime.Now.ToLongDateString() + "   " + DateTime.Now.ToLongTimeString() + "    共 " + iPageCount.ToString() + "页";
                Paragraph p_Fooder = new Paragraph(strFooder, font_Context);
                p_Fooder.Alignment = Element.ALIGN_BOTTOM;
                p_Fooder.Leading = 14;
                p_Fooder.MultipliedLeading = 1;
                p_Fooder.SpacingBefore = 20;
                p_Fooder.SpacingAfter = 0;
                document.Add(p_Fooder);

            }
            catch (Exception)
            {
                iReturn = 0;
                throw;
            }
            finally
            {
                //关闭目标文件
                document.Close();
                //关闭写入流
                writer.Close();

            }
            return iReturn;
        }
        #endregion

        #region 财务付款
        /// <summary>
        /// 财务付款 转换GridView为PDF文档
        /// </summary>
        /// <param name="sdr_Context">SqlDataReader</param>
        /// <param name="title">标题名称</param>
        /// <param name="col_Width">表格总宽度</param>
        /// <param name="strServerMapPath">服务器目录路径</param>
        /// <param name="pdf_Filename">在服务器端保存PDF时的文件名</param>
        /// <returns>返回调用是否成功   0 失败,1成功</returns>
        public int ImportExpensePDF(DataTable dtMaininfo, DataTable dtSignLog, DataTable dtAttachFiles, string title, float col_Width, string strServerMapPath, string pdf_Filename)
        {
            int iReturn = 1;
            //标题
            string fontpath_Title = @"c:\\windows\\FONTS\\simsun.ttc,1";  //标题字体路径
            float fontsize_Title = 14;                                  //标题字体大小
            int fontStyle_Title = iTextSharp.text.Font.BOLD;           //标题样式
            BaseColor fontColor_Title = BaseColor.RED;                  //标题颜色
            //列头
            string fontpath_Col = @"c:\\windows\\FONTS\\simsun.ttc,1";      //列头字体、大小、样式、颜色
            float fontsize_Col = 11;
            int fontStyle_Col = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Col = BaseColor.BLACK;
            //正文
            string FontPath = "c:\\windows\\FONTS\\simsun.ttc,1";       ////正文字体、大小、样式、颜色
            float FontSize = 11;
            int fontStyle_Context = iTextSharp.text.Font.NORMAL;
            BaseColor fontColor_Context = BaseColor.BLACK;


            //在服务器端保存PDF时的文件名
            //string strFileName = pdf_Filename + ".pdf";
            //初始化一个目标文档类 
            Document document = new Document(PageSize.A4.Rotate(), 0, 0, 10, 10);
            //调用PDF的写入方法流
            //注意FileMode-Create表示如果目标文件不存在，则创建，如果已存在，则覆盖。
            PdfWriter writer = PdfWriter.GetInstance(document,
                new FileStream(pdf_Filename, FileMode.Create));
            try
            {
                //标题字体
                BaseFont basefont_Title = BaseFont.CreateFont(
                  fontpath_Title,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Title = new iTextSharp.text.Font(basefont_Title, fontsize_Title, fontStyle_Title, fontColor_Title);
                //表格列字体
                BaseFont basefont_Col = BaseFont.CreateFont(
                  fontpath_Col,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Col = new iTextSharp.text.Font(basefont_Col, fontsize_Col, fontStyle_Col, fontColor_Col);
                //正文字体
                BaseFont basefont_Context = BaseFont.CreateFont(
                  FontPath,
                  BaseFont.IDENTITY_H,
                  BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font_Context = new iTextSharp.text.Font(basefont_Context, FontSize, fontStyle_Context, fontColor_Context);

                //打开目标文档对象
                document.Open();

                //添加标题
                Paragraph p_Title = new Paragraph(title, font_Title);
                p_Title.Alignment = Element.ALIGN_CENTER;
                p_Title.Leading = 14;
                p_Title.MultipliedLeading = 1;
                p_Title.SpacingAfter = 10;
                document.Add(p_Title);




                //添加一个4列的数据表
                PdfPTable table1Main = new PdfPTable(4);
                table1Main.TotalWidth = col_Width;//表格总宽度
                table1Main.LockedWidth = true;//锁定宽度
                table1Main.SetWidths(new float[] { 100f, 300f, 100f, 300f });//设置每列宽度


                table1Main.TotalWidth = col_Width;
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("请款单编号：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpenseNo"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("付款公司：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["CompanyNameC"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("经手人：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpenseMan"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同编号：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpContractNo"].ToString().Trim(), font_Col));

                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("收款单位：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpCustomerName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("付款方式：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpPayTypeName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("收款银行：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpBankName"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("收款帐号：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpAccountNo"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同总金额：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpTotalMoney"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("付款日期：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpPayDate"].ToString().Trim(), font_Col));



                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("发票类型：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                PdfPCell cellInvoiceType = new PdfPCell(new Phrase(dtMaininfo.Rows[0]["InvoiceTypeShow"].ToString().Trim(), font_Col));
                cellInvoiceType.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellInvoiceType);
                /*
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("发票类型：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["InvoiceTypeShow"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase("", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase("", font_Col));
                */

                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("执行或变更情况：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpChange"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("变更后总价：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpChangeMoney"].ToString().Trim(), font_Col));



                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("请款用途：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpReason"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("项目：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpProject"].ToString().Trim(), font_Col));



                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("合同付款情况：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                string strPay = "己付款：" + dtMaininfo.Rows[0]["ExpHavePay"].ToString().Trim() + "\r\t"
                            + "本次付款：" + dtMaininfo.Rows[0]["ExpThisPay"].ToString().Trim() + "\r\t"
                            + "付款进度：" + dtMaininfo.Rows[0]["ExpProcess"].ToString().Trim() + "\r\t"
                            + "余款：" + dtMaininfo.Rows[0]["ExpRest"].ToString().Trim();

                PdfPCell cellPay = new PdfPCell(new Phrase(strPay, font_Col));
                cellPay.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellPay);

                /*
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("己付款：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpHavePay"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("本次付款：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpThisPay"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("付款进度：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpProcess"].ToString().Trim(), font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("余款：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                table1Main.AddCell(new Phrase(dtMaininfo.Rows[0]["ExpRest"].ToString().Trim(), font_Col));
                */




                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                table1Main.AddCell(new Phrase("备注：", font_Col));
                table1Main.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                string strExpMemo = ReplaceValidateString(dtMaininfo.Rows[0]["ExpMemo"].ToString().Trim());
                PdfPCell cellMemo = new PdfPCell(new Phrase(strExpMemo, font_Col));
                cellMemo.Colspan = 3;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1Main.AddCell(cellMemo);







                //---------------------------------------------------------------------
                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableAttach = new PdfPTable(dtAttachFiles.Columns.Count);
                tableAttach.TotalWidth = col_Width;//表格总宽度
                tableAttach.LockedWidth = true;//锁定宽度
                tableAttach.SetWidths(new float[] { 500f, 150f, 150f });//设置每列宽度

                //构建列头
                //设置列头背景色
                tableAttach.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableAttach.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableAttach.HeaderRows = 1;
                if (dtAttachFiles.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtAttachFiles.Columns)
                    {
                        tableAttach.AddCell(new Phrase(dc.ColumnName, font_Col));
                    }
                    // 添加数据
                    //设置标题靠左居中
                    tableAttach.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableAttach.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtAttachFiles.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtAttachFiles.Columns.Count; j++)
                        {
                            tableAttach.AddCell(new Phrase(dtAttachFiles.Rows[i][j].ToString(), font_Context));
                        }
                    }
                }




                //---------------------------------------------------------------------

                //根据数据表内容创建一个PDF格式的表
                PdfPTable tableSignLog = new PdfPTable(dtSignLog.Columns.Count);        //有一列为签名档URL
                tableSignLog.TotalWidth = col_Width;//表格总宽度
                tableSignLog.LockedWidth = true;//锁定宽度
                tableSignLog.SetWidths(new float[] { 50f, 50f, 50f, 50f, 400f, 100f, 100f });//设置每列宽度



                //构建列头
                //设置列头背景色
                tableSignLog.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                //设置列头文字水平、垂直居中
                tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableSignLog.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                // 告诉程序这行是表头，这样页数大于1时程序会自动为你加上表头。
                tableSignLog.HeaderRows = 1;
                if (dtSignLog.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtSignLog.Columns)
                    {
                        if (dc.ColumnName.ToString().Trim() != "SignatureURL")
                        {
                            tableSignLog.AddCell(new Phrase(dc.ColumnName, font_Col));
                        }
                        else
                        {
                            tableSignLog.AddCell(new Phrase("签名", font_Col));
                        }
                    }

                    // 添加数据
                    //设置标题靠左居中
                    tableSignLog.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    // 设置表体背景色
                    tableSignLog.DefaultCell.BackgroundColor = BaseColor.WHITE;
                    for (int i = 0; i < dtSignLog.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtSignLog.Columns.Count - 1; j++)
                        {
                            tableSignLog.AddCell(new Phrase(dtSignLog.Rows[i][j].ToString(), font_Context));
                        }

                        string strSignatureURL = dtSignLog.Rows[i]["SignatureURL"].ToString().Trim();
                        if (strSignatureURL == "")
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\4.jpg";
                        }
                        else
                        {
                            strSignatureURL = strServerMapPath + "\\upldPic\\" + strSignatureURL;
                        }


                        Image img1 = Image.GetInstance(strSignatureURL);
                        img1.Alignment = 1;              //对齐方式（1为居中，0为居左，2为居右）
                        img1.ScaleAbsolute(150, 150);

                        tableSignLog.AddCell(img1);
                    }
                }


                document.Add(table1Main);


                //增加空白行
                Paragraph p_Attach = new Paragraph("附 件", font_Title);
                p_Attach.Alignment = Element.ALIGN_CENTER;
                p_Attach.Leading = 14;
                p_Attach.SpacingBefore = 10;
                p_Attach.SpacingAfter = 10;
                document.Add(p_Attach);

                document.Add(tableAttach);

                //如果最后一个单元格数据过多，不要移动到下一页显示
                tableSignLog.SplitLate = false;
                tableSignLog.SplitRows = false;
                //在目标文档中添加转化后的表数据

                //增加空白行
                Paragraph p_SignLog = new Paragraph("签核记录", font_Title);
                p_SignLog.Alignment = Element.ALIGN_CENTER;
                p_SignLog.Leading = 14;
                p_SignLog.SpacingBefore = 10;
                p_SignLog.SpacingAfter = 10;
                document.Add(p_SignLog);

                document.Add(tableSignLog);
                //table.DefaultCell.Width = 0.5F;



                int iPageCount = writer.PageNumber;

                string strFooder = "打印日期：" + DateTime.Now.ToLongDateString() + "   " + DateTime.Now.ToLongTimeString() + "    共 " + iPageCount.ToString() + "页";
                Paragraph p_Fooder = new Paragraph(strFooder, font_Context);
                p_Fooder.Alignment = Element.ALIGN_BOTTOM;
                p_Fooder.Leading = 14;
                p_Fooder.MultipliedLeading = 1;
                p_Fooder.SpacingBefore = 20;
                p_Fooder.SpacingAfter = 0;
                document.Add(p_Fooder);

            }
            catch (Exception)
            {
                iReturn = 0;
                throw;
            }
            finally
            {
                //关闭目标文件
                document.Close();
                //关闭写入流
                writer.Close();

            }
            return iReturn;
        }
        #endregion

        public void Dispose()
        {
            Close();
        }
    }
}
