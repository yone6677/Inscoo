using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
//using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace Services
{
    public class PageHeaderHandlerAddLogo : PdfPageEventHelper
    {
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            AddHead(writer, document);
        }

        public void AddHead(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            var imgSrc = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\health\Logo.png";
            var headImage = Image.GetInstance(imgSrc);
            headImage.Alignment = Element.ALIGN_RIGHT;
            headImage.SetAbsolutePosition(document.Right - 60, document.Top);
            headImage.SpacingAfter = 10;
            //document.Add(headImage);
            cb.AddImage(headImage);
            var number = writer.CurrentPageNumber;
            //document.Add(headImage);
            //document.Add(new Paragraph() { SpacingAfter = 15 });
            //var y = document.Top - document.PageSize.Height*(number - 1) - 15;
            //cb.MoveTo(document.Left,y);
            //cb.LineTo(document.Right, y);
            //document.se
            //document.Add(new LineSeparator());
            //cb.EndText();
            //writer.
        }
    }
}
