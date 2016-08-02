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
$(document).on('click', ".toggle_color", function () {
    $(this).parent().parent().next().siblings().children('.hideDeclare').hide();
    $(this).parent().parent().next().children().toggle();
    $(this).parent().parent().siblings().children().children().removeClass('hide_color');
    $(this).toggleClass('hide_color');
})

/*体检详情*/

$(function(){
    $(".product_detail>.main_tabs").on('mouseover', showTab);
});

function showTab(e) {
    var target = e.target || e.srcElement;
    if (target.nodeName == "A"){
        target = target.parentNode;
    }
    if (target.nodeName == "LI" && target.className != "current") {
        $(this).children(".current").removeClass('current');
        $(target).addClass('current');

        var containers = $(".product_detail>[class^='product_']");
        for (var i = 0; i < containers.length; i++){
            $(containers[i]).css('display','none');
        }
        var tar = target.dataset.tar;
        if (tar != "comment"){
            $(".product_" + tar).css('display','block');
        }
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
                url = '/order/ordertopay';
            }
            
            if (urlNow == '/Order/EntryInfo/') {
                url = '/order/ordertopay';
            }
            if (urlNow == '/Order/UploadFile/') {
                url = '/order/ordertopay';
            }
            if (urlNow == '/Order/ConfirmPayment/') {
                url = '/order/ordertopay';
            }
        }
        if (bgColor == '/health/index') {
            if (urlNow == '/health/BuyInfo/') {
                url = '/health/index';
            }

            if (urlNow.indexOf('/health/MakeSure') != -1) {
                url = '/health/index';
            }
            if (urlNow.indexOf("/EntryInfo?productId") != -1) {
                url = '/health/index';
            }
            if (urlNow.indexOf("/Health/ConfirmPayment") != -1) {
                url = '/health/index';
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
            if (urlNow == '/Nav/Create/') {
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
    //console.log($(this).parent().css("display","none"));
});


//*年龄选择*/



function changeSel1() {
    var arr1 = ['3500元/年', '4200元/年', '5000元/年', '5800元/年'];
    var nowOpt = $("#sel1").val()
    $("#programme-price1").text(arr1[nowOpt]);
}
function changeSel2() {
    var arr2 = ['6600元/年', '9500元/年', '11000元/年', '13000元/年'];
    var nowOpt = $("#sel2").val();
    $("#programme-price2").text(arr2[nowOpt]);
}
function changeSel3() {
    var arr3 = ['16610元/年', '19388元/年', '24886元/年', '33489元/年'];
    var nowOpt = $("#sel3").val();
    $("#programme-price3").text(arr3[nowOpt]);
}


/*显示提醒名称解释*/
$(document).on('mouseover', 'body', function () {
    $('[data-toggle="tooltip"]').tooltip();
})
    

/*客服部分*/
//$(".arrow-left").on("click", function () {
//    console.log(1000);
//    $(".service-girl").toggleClass("out");
//});
$(function () {
    $(window).scroll(function () {
        var oldTop = 180;
        var t = $("body").scrollTop();
        t = t + oldTop + "px";

        setTimeout(function () {
            $(".service-girl").css("top", t);
        }, 200);
    });
});
///时间操作
function dateAdd(date, strInterval, Number) {
    var dtTmp = date;
    switch (strInterval) {
        case 'year':
            return new Date(dtTmp.getFullYear() + Number, dtTmp.getMonth(), dtTmp.getDate() + -1, dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            break;
        case 'season':
            return new Date(dtTmp.getFullYear(), dtTmp.getMonth() + 3 * Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            break;
        case 'month':
            return new Date(dtTmp.getFullYear(), dtTmp.getMonth() + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
            break;
        case 'week':
            return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
            break;
        case 'day':
            return new Date(Date.parse(dtTmp) + (86400000 * Number));
            break;
    }
}