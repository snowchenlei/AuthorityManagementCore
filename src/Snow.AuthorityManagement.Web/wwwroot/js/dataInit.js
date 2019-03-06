//表格初始化
var table = {
    init: function (columns, detailView, height) {
        var existsChildTable = detailView ? true : false;
        //绑定table的viewmodel
        $('#tb-body').bootstrapTable({
            url: absoluteUrl + '/Load',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: true,                     //是否启用排序
            sortOrder: "desc",                   //排序方式
            //导出
            //showExport: true,                   //是否显示导出按钮
            //exportDataType: 'basic',     //basic', 'all', 'selected'.
            //exportTypes: ['xls', 'doc', 'json', 'csv', 'txt', 'sql', 'pdf', 'png'],
            //exportOptions: {
            //    fileName: title,                //文件名称设置
            //    worksheetName: 'sheet1',        //表格工作区名称
            //    tableName: title,
            //    excelstyles: ['background-color', 'color', 'font-size', 'font-weight'],
            //},
            queryParams: table.queryParams,     //传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            //strictSearch: true,               //设置为 true启用全匹配搜索，否则为模糊搜索。
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            singleSelect: false,                 //单选
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: height,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: false,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            columns: columns,
            detailView: existsChildTable,             //是否显示父子表
            fixedColumns: true,
            fixedNumber: 2,
            onExpandRow: table.onExpandRow,
            onLoadSuccess: table.onLoadSuccess, //加载成功
        });
    },
    //请求参数
    queryParams: function (params) {
        return queryParams(params);
    }
};

var operate = {
    //新增
    add: function () {
        createOrEdit('添加');
    },
    //编辑
    edit: function (name, id) {
        createOrEdit('修改' + name, id);
    },
    //删除
    del: function (name, id) {
        bootbox.confirm({
            size: 'small',
            title: '删除',
            message: '确定要删除"' + name + '"吗？',
            callback: function (result) {
                if (result) {
                    $.post(absoluteUrl + 'Delete',
                        { id: id },
                        function (result) {
                            requestCallBack(result,
                                function () {
                                    refreshTable();
                                });
                        });
                }
            }
        });
    }
};
function createOrEdit(title, id) {
    var dialog = bootbox.dialog({
        title: title,
        message: '<p><i class="fa fa-spin fa-spinner"></i> 加载中...</p>',
        buttons: {
            cancel: {
                label: "取消",
                className: 'btn-danger ladda-button'
            },
            ok: {
                label: "提交",
                className: 'btn-success ladda-button',
                callback: function (result) {
                    var l = Ladda.create(result.target);
                    l.start();
                    //手动验证
                    var $e = $("#modelForm");
                    if (!$e.valid()) {
                        l.stop();
                        return false;
                    }
                    var s = $e.serializeArray();
                    $.post('/User/CreateOrEdit',
                        s,
                        function (result) {
                            l.stop();
                            requestCallBack(result,
                                function () {
                                    refreshTable();
                                });
                            dialog.modal('hide');
                        });
                    return false;
                }
            }
        }
    });
    dialog.init(function () {
        $('.ladda-button').attr('data-style', 'zoom-in');
        $.get(absoluteUrl + '/CreateOrEdit', { id: id }, function (data) {
            dialog.find('.bootbox-body').html(data);
        });
    });
}
//表格刷新
function refreshTable() {
    $('#tb-body').bootstrapTable("refresh");
}

// 请求后台回调
function requestCallBack(result, operate) {
    if (result.status === 200) {
        if (typeof operate === 'function') {
            operate(result.data);
        }
        toastr.success(result.message);
    } else {
        toastr.error(result.message);
    }
}