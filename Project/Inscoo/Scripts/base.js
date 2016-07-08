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
    console.log($(this).parent().parent().siblings().children().children().removeClass('hide_color'));
    $(this).toggleClass('hide_color');
    
    
})
/**/
$(".nav-header").click(function () {
    $(this).parent().siblings().children('.nav-background').removeClass('nav-background');
    $(this).addClass('nav-background');
});

