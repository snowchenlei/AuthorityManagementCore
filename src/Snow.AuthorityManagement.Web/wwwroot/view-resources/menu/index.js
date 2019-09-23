// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit,  //页码
        sort: params.sort,
        order: params.order,
        name: $("#txt_search_name").val(),
        parentId: $('#parentId').val()
    };
}

(function () {
    var treeId = 'navTree';
    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            l = Ladda.create(e.target);
            l.start();
            e.preventDefault();
            createOrEdit('修改菜单：' + row.name, row.id);
        },
        'click .remove': function (e, value, row, index) {
            l = Ladda.create(e.target);
            l.start();
            bootbox.confirm({
                size: 'small',
                title: '删除菜单' + row.name,
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
                                l.stop();
                                toastr.success('删除成功');
                            }
                        });
                    } else {
                        l.stop();
                    }
                }
            });
        }
    };
    function actionFormater(value, row, index) {
        var htmlArr = [];

        htmlArr.push('<div class="btn-group" style="display: inline-block;" role="group" aria-label="...">');
        if (isGranted('Pages.Administration.Menus.Edit')) {
            htmlArr.push('<button type = "button" class="btn btn-sm btn-warning edit" title="修改"><i class="far fa-edit"></i></button>');
        }
        if (isGranted('Pages.Administration.Menus.Delete')) {
            htmlArr.push('<button type="button" class="btn btn-sm btn-danger remove" title="删除"><i class="fas fa-trash"></i></button>');
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
        { field: 'icon', title: '图标' },
        { field: 'sort', title: '排序', sortable: true }
    ];
    //#region ztree
    var setting = {
        view: {
            selectedMulti: false
        },
        data: {
            simpleData: {
                enable: true,
                idKey: "id",
                pIdKey: "parentID",
                rootPId: 0
            },
            key: {
                title: 'id',
                name: 'name'
            }
        },
        async: {
            enable: true,
            url: "/api/menus?pageSize=1000&pageIndex=0",
            autoParam: ["id", "name=n", "level=lv"],
            otherParam: { "otherParam": "zTreeAsyncTest" },
            dataFilter: filter,
            type: "get"
        },
        callback: {
            onAsyncSuccess: onAsyncSuccess,
            onClick: onClick
        },
    };
    function filter(treeId, parentNode, childNodes) {
        return childNodes.items;
    }
    function onAsyncSuccess(event, treeId, treeNode, msg) {
        // 展开全部节点
        var zTree = $.fn.zTree.getZTreeObj(treeId);
        zTree.expandAll(true);
    }
    function onClick(event, treeId, treeNode, clickFlag) {
        // 非多选模式下取消选中
        if (clickFlag === 0) {
            $('#parentId').val('');
        } else {
            $('#parentId').val(treeNode.id);
        }
        refreshTable();
    }
    //#endregion
    $(function () {
        //1、初始化表格
        table.init(url, columns);
        //3、pannel初始化
        loadPanel();
        $.fn.zTree.init($("#navTree"), setting);

        if (!isGranted('Pages.Administration.Menus.Create')) {
            $('#btnAdd').remove();
        }

        $('#create').click(function () {
            l = Ladda.create(this);
            l.start();
            createOrEdit('添加新菜单');
        });
    });
    var dialog, l,
        url = '/api/menus/';
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
            $.get('/Menu/CreateOrEdit', { menuId: id, parentId: $('#parentId').val() }, function (data) {
                dialog.find('.bootbox-body').html(data);
                dialog.find('input:not([type=hidden]):first').focus();
                l.stop();
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
        var menu = $e.serializeFormToJson();
        return JSON.stringify({
            menu: menu
        });
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
    }
})();