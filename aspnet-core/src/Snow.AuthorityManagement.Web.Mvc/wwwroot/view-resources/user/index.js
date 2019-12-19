// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit,  //页码
        sort: params.sort,
        order: params.order,
        userName: $('#txt_search_userName').val(),
        //roleID: $('#txt_sel_role option:checked').val(),
        date: $('#txt_search_addTime').val()
    };
}
// #endregion
(function () {
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            e.preventDefault();
            edit('修改用户：' + row.name, row.id);
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
        if (isGranted('Pages.Administration.Users.Edit')) {
            htmlArr.push('<button type = "button" class="btn btn-sm btn-warning edit" title="修改"><i class="far fa-edit"></i></button>');
        }
        if (isGranted('Pages.Administration.Users.Delete')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove" title="删除"><i class="fas fa-trash"></i></button>');
        }
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
        if (!isGranted('Pages.Administration.Users.Create')) {
            $('#btnAdd').remove();
        }

        $('#create').click(function () {
            create('添加新用户');
        });
        //$('#modifyModal').modal('show');
        //1、初始化表格
        table.init(url, columns);
        //3、pannel初始化
        loadPanel();
        //4、时间初始化
        setDate($('#txt_search_addTime'), true, true);
        $('#txt_sel_role').select2({
            language: 'zh-CN',
            placeholder: '请选择',
            allowClear: true,
            theme: 'bootstrap4',
            //width: '100%'
        });
    });
    var dialog, l,
        url = '/api/users/';
    function create(title) {
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
                            return false;
                        }
                    }
                }
            }
        });
        dialog.init(function () {
            $.get('/User/Create', function (data) {
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
            });
        });
    }
    function edit(title, id) {
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
                                    toastr.success('修改成功');
                                    l.stop();
                                    refreshTable();
                                    dialog.modal('hide');
                                },
                                error: function (result) {
                                    toastr.error(result.responseText);
                                    l.stop();
                                }
                            });
                            return false;
                        }
                    }
                }
            }
        });
        dialog.init(function () {
            $.get('/User/Edit/' + id, function (data) {
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