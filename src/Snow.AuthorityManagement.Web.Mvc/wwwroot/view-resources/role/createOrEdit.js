//#region zTree
var setting = {
    view: {
        selectedMulti: false
    },
    check: {
        enable: true
    },
    data: {
        key: {
            title: 'name',
            name: 'displayName',
            checked: 'isGranted'
        }
    }
};
$(document).ready(function () {
    $.validator.unobtrusive.parse('form');
    $.fn.zTree.init($("#authTree"), setting, zNodes);
    var treeObj = $.fn.zTree.getZTreeObj("authTree");
    treeObj.expandAll(true);
});
//#endregion