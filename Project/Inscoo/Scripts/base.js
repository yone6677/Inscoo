function restPagerIndex() {
    $('#PageIndex').val('1');
}
function AjaxPager(pageIndex, pageSize, formId) {
    $('#PageSize').val(pageSize); $('#PageIndex').val(pageIndex); $('#' + formId).submit();
}
function RestSumbit(e) {
    $(e).submit();
}
function getPermissions(id, url, updateId) {
    $.ajax({
        type: 'get',
        data: 'rid=' + id,
        url: url,
        success: function (e) {
            document.getElementById(updateId).innerHTML = e;
        }
    });
}
function selectAll(e, subCheck) {
    var checks = document.getElementsByName(subCheck);
    var num = checks.length;
    for (var i = 0; i < num; i++) {
        checks[i].checked = e.checked;
    }
}

/*表格内？？？保险条款部分切换*/
$(".toggle_color").click(function () {
    $(this).parent().parent().next().siblings().children('.hideDeclare').hide();
    $(this).parent().parent().next().children().toggle();
    $(this).parent().parent().siblings().children().children().removeClass('hide_color');
    $(this).toggleClass('hide_color');
    
    
})

/*体检详情*/
$(".health_box_body>.health_nav").on("click","li", showTab);
function showTab(e) {
    var target = e.target || e.srcElement;
    if (target.nodeName == "A"){
        target = target.parentNode;
    }
    if (target.nodeName == "LI"){
        $(this).siblings().removeClass('active');
        $(this).addClass('active');
      
        var link = $(this).children().attr('href');
        $(link).addClass('show').siblings().removeClass('show');
    }
}

/*导航*/
function nav(name, csskey, cssvalue) {
    var url = window.location.pathname + window.location.search;
    var bgColor = "";
    $(name).each(function () {
        bgColor = "/About/" + $(this).attr("href");
        if (bgColor == url) {
            var v2 = $(this).parent().css(csskey, cssvalue);
        }
    })
}
$(function(){
    nav('.main-left li a', 'background', '#5E6E7E');
})

/*menu*/
function menuNav(name, csskey, cssvalue) {
    var url = window.location.pathname + window.location.search;
    var bgColor = "";
    $(name).each(function () {
        bgColor = $(this).attr("href");
        if (bgColor == url) {
            $(this).addClass('nav-background');
            $(this).parent().parent().addClass('in');
        }
    })
}
$(function () {
    menuNav('.mainNav li a');
});
//$(".mainNav>ul>li>a").click(function () {
//    $(this).parent().siblings().children().removeClass('in');
//    $(this).parent().parent().siblings().children().children().removeClass('in');
//    $(this).next().addClass('in');
//});

