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
        //console.log(urlNow);
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
      
        if (bgColor == '/Health/Index') {
            if (urlNow == '/health/BuyInfo/') {
                url = '/health/index';
            }
            if (urlNow.indexOf('/Health/BuyInfo') != -1) {
                url = '/Health/Index';
            }
            if (urlNow.indexOf("/Health/MakeSure") != -1) {
                url = '/Health/Index';
            }
            if (urlNow.indexOf("/Health/EntryInfo") != -1) {
                url = '/Health/Index';
            }
            if (urlNow.indexOf("/Health/ConfirmPayment") != -1) {
                url = '/Health/Index';
            }
            //if (urlNow.indexOf("/Health") != -1) {
            //    url = '/Health/Index';
            //}
            
        }
        if (bgColor == "/Health/AuditListSearch") {
            if (urlNow == '/Health/UploadEmp/') {
                url = '/Health/AuditListSearch';
            }
            if (urlNow.indexOf("/Health/OrderInfo") != -1) {
                url = '/Health/HealthMgr/';
            }
            if (urlNow.indexOf("/Health/AuditListSearch") != -1) {
                url = '/Health/AuditListSearch';
            }
        }
        if (bgColor == "/Health/HealthMgr") {
            if (urlNow.indexOf("/Health/OrderInfo?") != -1) {
                url = '/Health/HealthMgr';
            }
        }

        if (bgColor == "/Cart/Index") {
           
            if (urlNow.indexOf("/Cart") != -1) {
                url = '/Cart/Index';
            }
            
        }
        if (bgColor == '/Order/completedorder') {
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
        if (bgColor == '/Finance/index') {
            if (urlNow == '/Finance/Details/') {
                url = bgColor;
            }
            if (urlNow == '/Finance/Settlement/') {
                url = bgColor;
            }
            if (urlNow == '/Finance/CashFlowDtl/') {
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
        var innerH = window.innerHeight;
        var oldTop = innerH / 2;
        var t = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;;
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


/*"下一步"固定*/
$(function() {
    
    $(window).on("scroll", function () {
        var pathName = window.location.pathname;
        var partialName = pathName.split("/");
        var Name = "/" + partialName[1] + "/" + partialName[2] + "/";
        if (Name == "/Order/EntryInfo/" || Name == "/order/EntryInfo/"  || Name == "/Order/UploadFile/" || Name == "/Order/ConfirmPayment/") {
            var parent = $(".d2-box");
            var parentHeight = parent.offset().top;
            var innerHeight = window.innerHeight;

            var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;

            if (parentHeight >= innerHeight) {
                $(".d3-box").attr("class", "d3-box box-1 fixed");
            } else {
                $(".d3-box").attr("class", "d3-box box-1");
            }
            if ((scrollTop + innerHeight) > parentHeight) {
                $(".d3-box").attr("class", "d3-box box-1");
            }
        }
    })
})


/*专属产品*/
$(function () {
    var p = $(".inscooCase-body-top").css("display", "block");
    //console.log(p);
    for (var i = 0; i < p.length; i++) {
        var pName = $(p[i]).children(".describe_name").html();
        //console.log(pName);
        var ele = $("<li><a href='#product_d" + i + "' data-toggle='tab'>" + pName + "</a></li>");
        $(".myTab").append(ele);
        $(p[i]).attr("id","product_d" + i);
    }
    

    $("[id^='product_d']").css("display","none");
    $("[id^='product_d0']").css("display", "block");
    $("[data-toggle='tab']:first").parent().addClass("active");
    $(".myTab [data-toggle='tab']").on("click", function () {
        $(".active").removeClass("active");
        $(this).parent().addClass("active");
        
        var hrefNow = $(this).attr("href");
        //console.log(hrefNow);
        $("[id^='product_d']").css("display", "none");
        $(hrefNow).css("display","block");
    })
})



