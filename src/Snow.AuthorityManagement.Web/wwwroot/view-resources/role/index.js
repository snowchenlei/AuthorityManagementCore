var absoluteUrl;

// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit + 1,  //页码
        sort: params.sort,
        order: params.order,
        name: $('#txt_search_name').val(),
        date: $('#txt_search_addTime').val()
    };
}
// #endregion

(function () {
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit('修改角色：' + row.name, row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: '删除角色' + row.name,
                message: '你确定要删除' + row.name + '吗？',
                callback: function (result) {
                    if (result) {
                        $.post('/Role/Delete',
                            { id: row.id },
                            function (result) {
                                requestCallBack(result,
                                    function () {
                                        var $table = $('#tb-body');
                                        $table.bootstrapTable('remove',
                                            {
                                                field: 'id',
                                                values: [row.id]
                                            });
                                    });
                            });
                    }
                }
            });
        }
    };
    function actionFormater(value, row, index) {
        var htmlArr = [];

        htmlArr.push('<div class="btn-group" style="display: inline-block;" role="group" aria-label="...">');
        if (isGranted('Pages.Users.Edit')) {
            htmlArr.push('<button type = "button" class="btn btn-sm btn-warning edit"><i class="far fa-edit"></i></button>');
        }
        if (isGranted('Pages.Users.Delete')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove"><i class="fas fa-trash"></i></button>');
        }
        htmlArr.push('<div class="btn-group" role="group">');
        htmlArr.push('<button id="btnGroupDrop1" type="button" class="btn btn-sm btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
        htmlArr.push('Action');
        htmlArr.push('</button>');
        htmlArr.push('<div class="dropdown-menu" aria-labelledby="btnGroupDrop1">');
        htmlArr.push('<a class="dropdown-item" href="#">Dropdown link</a>');
        htmlArr.push('<a class="dropdown-item" href="#">Dropdown link</a>');
        htmlArr.push('</div>');
        htmlArr.push('</div>');
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    var columns = [
        { checkbox: true },
        { title: '操作', formatter: actionFormater, events: operateEvents },
        { field: 'id', title: 'Id', visible: false },
        { field: 'name', title: '名称' },
        { field: 'sort', title: '排序', sortable: true }
    ];
    $(function () {
        if (!isGranted('Pages.Roles.Create')) {
            $('#btnAdd').remove();
        }
        $('#create').click(function () {
            createOrEdit('添加新角色');
        });
        //1、初始化表格
        table.init(columns);
        //3、pannel初始化
        loadPanel();
        //4、时间初始化
        setDate($('#txt_search_addTime'), true, true);
    });

    var dialog, l;
    function createOrEdit(title, id) {
        dialog = bootbox.dialog({
            title: title,
            message: '<p><i class="fa fa-spin fa-spinner"></i> 加载中... </p>',
            size: 'large',
            buttons: {
                cancel: {
                    label: '取消',
                    className: 'btn-danger'
                },
                confirm: {//ok、confirm会在加载完成后获取焦点
                    label: '提交',
                    className: 'btn-success',
                    callback: function (result) {
                        if (result) {
                            l = Ladda.create(result.target);
                            l.start();
                            return save();
                        }
                    }
                }
            }
        });
        dialog.init(function () {
            $.get('/Role/CreateOrEdit', { id: id }, function (data) {
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    function save() {
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            l.stop();
            return false;
        }
        //var s = $e.serializeArray();
        var role = $e.serializeFormToObject();
        $.ajax({
            type: "POST",
            url: "/Role/CreateOrEdit",
            data: {
                role
            },
            success: function (result) {
                l.stop();
                requestCallBack(result,
                    function () {
                        refreshTable();
                    });
                dialog.modal('hide');
            },
            error: function () {
                l.stop();
            }
        });
        return false;
    }
})();