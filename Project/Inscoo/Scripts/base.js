﻿function restPagerIndex() {
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
$(document).on('click', ".toggle_color", function () {
    $(this).parent().parent().next().siblings().children('.hideDeclare').hide();
    $(this).parent().parent().next().children().toggle();
    $(this).parent().parent().siblings().children().children().removeClass('hide_color');
    $(this).toggleClass('hide_color');
})

/*体检详情*/
$(".health_box_body>.health_nav").on("click", "li", showTab);
function showTab(e) {
    var target = e.target || e.srcElement;
    if (target.nodeName == "A") {
        target = target.parentNode;
    }
    if (target.nodeName == "LI") {
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
$(function () {
    nav('.main-left li a', 'background', '#5E6E7E');
})

/*menu*/
function menuNav(name, csskey, cssvalue) {
    var url = window.location.pathname + window.location.search;
    //console.log(window.location);
    //console.log(url);
    
    var bgColor = "";
    $(name).each(function () {
        bgColor = $(this).attr("href");
        var urlNow = '/' + url.split('/')[1] + '/' + url.split('/')[2] + '/';
        if (bgColor == '/Insurance/CustomizeProduct') {
            if (url == '/Order/Buy') {
                url = '/Insurance/CustomizeProduct';
            }
            
            if (urlNow == '/Order/EntryInfo/') {
                url = '/Insurance/CustomizeProduct';
            }
            if (urlNow == '/Order/UploadFile/') {
                url = '/Insurance/CustomizeProduct';
            }
            if (urlNow == '/Order/ConfirmPayment/') {
                url = '/Insurance/CustomizeProduct';
            }
        }
        if (bgColor == '/order/index') {
            if (urlNow == '/Order/Details/') {
                url = bgColor;
            }
        }
        if (bgColor == '/order/ordertopay') {
            if (urlNow == '/order/EntryInfo/') {
                url = bgColor;
            }
            if (urlNow == '/order/UploadFile/') {
                url = bgColor;
            }
        }
        if (bgColor == '/order/CompletedOrder') {
            if (urlNow == '/Order/Details/') {
                url = bgColor;
            }
            if (urlNow == '/order/BuyMore/') {
                url = '/order/CompletedOrder';
            }
        }
        if (bgColor == '/user/index') {
            if (urlNow == '/user/Edit/') {
                url = bgColor;
            }
        }
        if (bgColor == '/company/listindex') {
            if (urlNow == '/company/Edit/') {
                url = bgColor;
            }
        }
        if (bgColor == '/Nav/index') {
            if (urlNow == '/Nav/Details/') {
                url = bgColor;
            }
            if (urlNow == '/Nav/Edit/') {
                url = bgColor;
            }
        }
        if (bgColor == '/GenericAttribute/index') {
            if (urlNow == '/GenericAttribute/Details/') {
                url = bgColor;
            }
            if (urlNow == '/GenericAttribute/Edit/') {
                url = bgColor;
            }
        }
        if (bgColor == url) {
            $(this).addClass('nav-background');
            $(this).parent().parent().addClass('in');
        }
    })

}
$(function () {
    menuNav('.mainNav li a');

});

$(".mainNav>ul>li>a").click(function () {
    $(this).parent().siblings().children('ul').removeClass('in').addClass('collapse');
    $(this).parent().parent().siblings().children().children('ul').removeClass('in').addClass('collapse');
    //$(this).next().addClass('in');
});

/*专属产品*/
$(function () {
    $(".look-more").on('mouseenter', function () {
        $(this).next('.look-more-hide').slideDown();
        $(this).slideUp();
    });
    $(".look-more-hide").on('mouseleave', function () {
        $(this).slideUp();
        $('.look-more').slideDown();
    });
})

/*type=file*/
$("[type='file']").on('change', function () {
    var empFile = $(this).val();
    var pf = (empFile.substring(empFile.lastIndexOf("\\") + 1, empFile.length));
    $(this).parent().next().text(pf);
});
