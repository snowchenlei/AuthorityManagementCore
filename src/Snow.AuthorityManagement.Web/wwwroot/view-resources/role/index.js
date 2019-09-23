var absoluteUrl;

// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit,  //页码
        sort: params.sort,
        order: params.order,
        displayName: $('#txt_search_display_name').val(),
        //date: $('#txt_search_addTime').val()
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
        if (isGranted('Pages.Administration.Roles.Edit')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-warning edit" title="修改"><i class="far fa-edit"></i></button>');
        }
        if (isGranted('Pages.Administration.Roles.Delete')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove" title="删除"><i class="fas fa-trash"></i></button>');
        }
        htmlArr.push('</div>');
        return htmlArr.join('');
    }
    var columns = [
        { checkbox: true },
        { title: '操作', formatter: actionFormater, events: operateEvents },
        { field: 'id', title: 'Id', visible: false },
        { field: 'displayName', title: '名称' },
        { field: 'sort', title: '排序', sortable: true }
    ];
    $(function () {
        if (!isGranted('Pages.Administration.Roles.Create')) {
            $('#btnAdd').remove();
        }
        $('#create').click(function () {
            createOrEdit('添加新角色');
        });
        //1、初始化表格
        table.init(url, columns);
        //3、pannel初始化
        loadPanel();
        //4、时间初始化
        setDate($('#txt_search_addTime'), true, true);
    });

    var dialog, l,
        url = '/api/roles/';
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
                            if (id === undefined) {
                                create();
                            } else {
                                edit(id);
                            }
                            return false;
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

    function check($e) {
        if (!$e.valid()) {
            l.stop();
            return false;
        }
        return true;
    }
    function getPara($e) {
        var role = $e.serializeFormToJson();
        var permissionNames = _findAssignedPermissions();
        var data = {
            role,
            permissionNames
        };
        return JSON.stringify(data);
    }
    function create() {
        var $e = $("#modelForm");

        if (!check($e)) {
            return false;
        }
        var para = getPara($e);
        $.ajax({
            type: 'POST',
            url: url,
            contentType: "application/json",
            data: para,
            success: function (result) {
                l.stop();
                toastr.success('添加成功');
                refreshTable();
                dialog.modal('hide');
            },
            error: function (result) {
                toastr.error(result.responseText);
                l.stop();
            }
        });
    }
    function edit(id) {
        var $e = $("#modelForm");
        if (!check($e)) {
            return false;
        }
        var para = getPara($e);
        $.ajax({
            type: 'PUT',
            url: url + id,
            contentType: "application/json",
            data: para,
            success: function (result) {
                l.stop();
                toastr.success('修改成功');
                refreshTable();
                dialog.modal('hide');
            },
            error: function (result) {
                toastr.error(result.responseText);
                l.stop();
            }
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
    function _findAssignedPermissions() {
        var checkAuth = [];
        var zTree = $.fn.zTree.getZTreeObj("authTree");
        var checkNodes = zTree.getCheckedNodes(true);
        checkAuth.splice(0, checkAuth.length);
        for (var i = 0; i < checkNodes.length; i++) {
            var current = checkNodes[i];
            checkAuth.push(current.name);
        }
        return checkAuth;
    }
})();