// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit,  //页码
        sort: params.sort,
        order: params.order,
        userName: $('#txt_search_userName').val(),
        date: $('#txt_search_addTime').val()
    };
}
// #endregion
(function () {
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            createOrEdit('修改用户：' + row.name, row.id);
        },
        'click .remove': function (e, value, row, index) {
            bootbox.confirm({
                size: 'small',
                title: '删除用户' + row.name,
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
        { field: 'phoneNumber', title: '手机号' },
        { field: 'name', title: '名称' },
        { field: 'userName', title: '用户名' }
    ];

    $(function () {
        if (!isGranted('Pages.Users.Create')) {
            $('#btnAdd').remove();
        }

        $('#create').click(function () {
            createOrEdit('添加新用户');
        });
        //$('#modifyModal').modal('show');
        //1、初始化表格
        table.init(url, columns);
        //3、pannel初始化
        loadPanel();
        //4、时间初始化
        setDate($('#txt_search_addTime'), true, true);
    });
    var dialog, l,
        url = '/api/users/';
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
            $.get('/User/CreateOrEdit', { id: id }, function (data) {
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
        var user = $e.serializeFormToJson();
        var roleIds = _findAssignedRoleIDs();
        var data = {
            user,
            roleIds
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

    function save(id) {
        //手动验证
        var $e = $("#modelForm");
        if (!$e.valid()) {
            l.stop();
            return false;
        }
        var user = $e.serializeFormToJson();
        var roleIds = _findAssignedRoleIDs();
        var data = {
            user,
            roleIds
        };
        var para = JSON.stringify(data);
        var url = '/api/user/';
        var type = 'POST';
        if (id !== undefined) {
            url += id;
            type = 'PUT';
        }

        $.ajax({
            type: type,
            url: url,
            contentType: "application/json",
            data: para,
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
        //$.ajax({
        //    type: "POST",
        //    url: "/User/CreateOrEdit",
        //    data:
        //    {
        //        user,
        //        roleIds
        //    },
        //    success: function (result) {
        //        l.stop();
        //        requestCallBack(result,
        //            function () {
        //                refreshTable();
        //            });
        //        dialog.modal('hide');
        //    },
        //    error: function () {
        //        l.stop();
        //    }
        //});
        return false;
    }
    function _findAssignedRoleIDs() {
        var assignedRoleIDs = [];

        dialog.find('#RolesTab input[type=checkbox]')
            .each(function () {
                if ($(this).is(':checked')) {
                    assignedRoleIDs.push($(this).data('id'));
                }
            });

        return assignedRoleIDs;
    }
})();