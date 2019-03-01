var absoluteUrl;

var columns = [
    { checkbox: true },
    { title: '操作', formatter: actionFormater },
    { field: 'id', title: 'Id', visible: false },
    { field: 'phoneNumber', title: '手机号' },
    { field: 'name', title: '名称' },
    { field: 'userName', title: '用户名' },
    { field: 'addTime', title: '添加时间', sortable: true }
];
function actionFormater(value, row, index) {
    var htmlArr = [];
    htmlArr.push('<div class="btn-group" style="display: inline-block;" role="group" aria-label="...">');
    htmlArr.push('<button type = "button" class="btn btn-sm btn-warning" onclick="operate.edit(\'' + row.name + '\', ' + row.id + ')"><i class="glyphicon glyphicon-pencil" style="margin-right: 5px;"></i></button>');
    htmlArr.push('<button type="button" class="btn btn-sm btn-danger" onclick="operate.del(\'' + row.name + '\', ' + row.id + ')"><i class="glyphicon glyphicon-trash" style="margin-right: 5px;"></i></button>');
    htmlArr.push('<div class="btn-group">');
    htmlArr.push('<button type="button" class="btn btn-sm btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">');
    htmlArr.push('Action <span class="caret"></span>');
    htmlArr.push('</button>');
    htmlArr.push('<ul class="dropdown-menu">');
    htmlArr.push('<li><a href="#">Action</a></li>');
    htmlArr.push('<li><a href="#">Another action</a></li>');
    htmlArr.push('<li><a href="#">Something else here</a></li>');
    htmlArr.push('</ul>');
    htmlArr.push('</div>');
    htmlArr.push('</div>');
    return htmlArr.join('');
}

$(function () {
    absoluteUrl = "/User/";
    //$('#modifyModal').modal('show');
    //1、初始化表格
    table.init(columns);
    //3、pannel初始化
    loadPanel();
    //4、时间初始化
    setDate($('#txt_search_addTime'), true, false);
});
// #region bootstrap-table
//搜索
function queryParams(params) {
    return {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
        pageSize: params.limit,   //页面大小
        pageIndex: params.offset / params.limit + 1,  //页码
        sort: params.sort,
        order: params.order,
        userName: $('#txt_search_userName').val(),
        date: $('#txt_search_addTime').val()
    };
}
// #endregion