using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Innscoo.Infrastructure
{
    public static class InnscooHtmlHelper
    {
        public static string Turnction(this HtmlHelper html, string Content, int length)
        {
            string shortContent = "";
            if (string.IsNullOrEmpty(Content))
                return null;
            if (Content.Length > length)
            {
                shortContent = Content.Substring(0, length) + "…";
            }
            else
            {
                shortContent = Content;
            }
            return shortContent;
        }
        public static string ConvertToShortTime(this HtmlHelper html, DateTime? Time, bool Date = true)
        {
            string ShortTime = "";
            if (Time.HasValue)
            {
                if (Date)
                {
                    ShortTime = Time.Value.ToShortDateString();
                }
                else
                {
                    ShortTime = Time.Value.ToShortTimeString();
                }
            }
            return ShortTime;
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper html, string name, string checkedItem, string value, IDictionary<string, object> htmlAttributes = null)
        {
            TagBuilder checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("name", name);
            checkbox.MergeAttribute("type", "checkbox");
            checkbox.MergeAttribute("value", value);
            checkbox.MergeAttributes(htmlAttributes);
            if (!string.IsNullOrEmpty(checkedItem))
            {
                List<string> hs = new List<string>();
                if (checkedItem.Contains(","))
                {
                    string[] tempStr = checkedItem.Split(',');
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        hs.Add(tempStr[i].Trim());
                    }
                }
                else
                {
                    hs.Add(checkedItem);
                }
                if (hs.Contains(value))
                {
                    checkbox.MergeAttribute("checked", "checked");
                }

            }
            return MvcHtmlString.Create(checkbox.ToString());
        }

        public static MvcHtmlString Pager(this HtmlHelper html, PageCommand pageCommand, string cssClass = null, string formId = null)
        {
            int lastPage = pageCommand.PageIndex - 1;
            int nextPage = pageCommand.PageIndex + 1;
            TagBuilder Pagination = new TagBuilder("div");
            if (string.IsNullOrEmpty(cssClass))
            {
                Pagination.AddCssClass("pager");
            }
            else
            {
                Pagination.AddCssClass(cssClass);
            }
            if (string.IsNullOrEmpty(formId))
            {
                formId = "searchForm";
            }
            //计数
            TagBuilder span = new TagBuilder("span");
            span.InnerHtml = "共<b>" + pageCommand.TotalCount + "</b>条<b>" + pageCommand.TotalPages + "</b>页";

            //第几页
            TagBuilder CurrentPage = new TagBuilder("select");
            CurrentPage.GenerateId("pagerIndex");
            //CurrentPage.AddCssClass( "form-control");
            string currentPageOption = "";
            for (int i = 1, max = pageCommand.TotalPages; i <= max; i++)
            {
                if (i == pageCommand.PageIndex)
                {
                    currentPageOption += "<option selected>" + i + "</option>";
                }
                else
                {
                    currentPageOption += "<option>" + i + "</option>";
                }
            }
            CurrentPage.InnerHtml = currentPageOption;
            CurrentPage.MergeAttribute("onchange", PageerSubmit("this.value", pageCommand.PageSize.ToString(), formId));
            //上一页控件
            string tLastPage = "";
            if (lastPage >= 1)
            {
                TagBuilder PreviousPage = new TagBuilder("a");
                PreviousPage.SetInnerText("Previous");
                PreviousPage.MergeAttribute("onclick", PageerSubmit(lastPage.ToString(), pageCommand.PageSize.ToString(), formId));
                tLastPage = PreviousPage.ToString();
            }
            else
            {
                TagBuilder PreviousPage = new TagBuilder("span");
                PreviousPage.SetInnerText("首页");
                tLastPage = PreviousPage.ToString();
            }
            //下一页控件
            string tNextPage = "";
            if (nextPage <= pageCommand.TotalPages)
            {
                TagBuilder PreviousPage = new TagBuilder("a");
                PreviousPage.SetInnerText("Next");
                PreviousPage.MergeAttribute("onclick", PageerSubmit(nextPage.ToString(), pageCommand.PageSize.ToString(), formId));
                tNextPage = PreviousPage.ToString();
            }
            else
            {
                TagBuilder NextPage = new TagBuilder("span");
                NextPage.SetInnerText("尾页");
                tNextPage = NextPage.ToString();
            }
            //每页显示行数
            string option = "";
            switch (pageCommand.PageSize)
            {
                case 15:
                    option = "<option selected>15</option><option>25</option><option>50</option><option>100</option>";
                    break;
                case 25:
                    option = "<option>15</option><option selected>25</option><option>50</option><option>100</option>";
                    break;
                case 50:
                    option = "<option>15</option><option>25</option><option selected>50</option><option>100</option>";
                    break;
                case 100:
                    option = "<option>15</option><option>25</option><option>50</option><option selected>100</option>";
                    break;
            }
            //防止将要显示条数超过预期值
            int VpageIndex = 1;
            if ((pageCommand.PageSize * pageCommand.PageIndex) > pageCommand.TotalCount)
            {
                VpageIndex = 1;
            }
            else
            {
                VpageIndex = pageCommand.PageIndex;
            }
            //每页显示行数
            TagBuilder DislayCount = new TagBuilder("select");
            DislayCount.InnerHtml = option;
            DislayCount.GenerateId("pagerSize");
            //DislayCount.AddCssClass( "form-control");
            DislayCount.MergeAttribute("onchange", PageerSubmit(VpageIndex.ToString(), "this.value", formId));

            Pagination.InnerHtml = "<p>" + span + "每页显示" + DislayCount + "条" + tLastPage + "第" + CurrentPage + "页" + tNextPage + "</p><input type='hidden' name='PageSize' id='PageSize' value='' /><input type = 'hidden' name = 'PageIndex' id = 'PageIndex' value = '' /> ";
            return new MvcHtmlString(Pagination.ToString());
        }
        //事件
        private static string PageerSubmit(string pageIndex, string pageSize, string formId)
        {
            return string.Format("AjaxPager(pageIndex={0},pageSize={1},formId='{2}')", pageIndex, pageSize, formId);
        }
    }
    public partial class PageCommand
    {
        public PageCommand()
        {
            PageIndex = 0;
            PageSize = 0;
            TotalCount = 0;
            TotalPages = 0;

        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}