function restPagerIndex() {
    $('#PageIndex').val('1');
}
function AjaxPager(pageIndex, pageSize) {
    $('#PageSize').val(pageSize); $('#PageIndex').val(pageIndex); $('#searchForm').submit();
}
function RestSumbit(e) {
    $(e).submit();
}
function getPermissions(id, url, updateId) {
    var id = $("#Id").val();
    $.ajax({
        type: 'get',
        data: 'Id=' + id,
        url: url,
        success: function (e) {
            $(updateId).html(e);
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