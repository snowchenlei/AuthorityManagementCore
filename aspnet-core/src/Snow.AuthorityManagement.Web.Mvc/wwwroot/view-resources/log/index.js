// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit,  //页码
        sort: params.sort,
        order: params.order,
        logLevel: $('#txt_sel_level option:checked').val(),
        date: $('#txt_search_addTime').val()
    };
}
// #endregion
(function () {
    var url = '/api/logs/';
    window.operateEvents = {
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: '删除日志',
                message: '你确定要删除这条日志吗？',
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            type: 'DELETE',
                            url: url + row.id,
                            success: function () {
                                var $table = $('#tb-body');
                                $table.bootstrapTable('remove',
                                    {
                                        field: 'id',
                                        values: [row.id]
                                    });
                                toastr.success('删除成功');
                            }
                        });
                    }
                }
            });
        }
    };
    function actionFormater(value, row, index) {
        var htmlArr = [];

        htmlArr.push('<div class="btn-group" style="display: inline-block;" role="group" aria-label="...">');
        if (isGranted('Pages.Administration.Logs.Delete')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove" title="删除"><i class="fas fa-trash"></i></button>');
        }
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    var columns = [
        { checkbox: true },
        { title: '操作', formatter: actionFormater, events: operateEvents },
        { field: 'id', title: 'Id', visible: false },
        { field: 'level', title: '等级', width: '10', widthUnit: '%' },
        { field: 'message', title: '信息', width: '25', widthUnit: '%', formatter: textFormatter },
        { field: 'exception', title: '异常', width: '30', widthUnit: '%', formatter: textFormatter },
        { field: 'timestamp', title: '发生时间', width: '20', widthUnit: '%' }
    ];
    //文本格式化
    function textFormatter(value, row, index) {
        if (value !== null) {
            return '<a href="#" class="wi" onclick=showText(this) title=\'' + value + '\'>' + value + '</a>';
        }
    }
    $(function () {
        if (!isGranted('Pages.Administration.Loggers.Create')) {
            $('#btnAdd').remove();
        }
        //1、初始化表格
        table.init(url, columns);
        //3、pannel初始化
        loadPanel();
        //4、时间初始化
        setDate($('#txt_search_addTime'), true, true);
        $('#txt_sel_level').select2({
            language: 'zh-CN',
            placeholder: '请选择',
            allowClear: true,
            theme: 'bootstrap4',
            //width: '100%'
        });
    });
})();