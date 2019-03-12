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
    },
    callback: {
        onCheck: authOnCheck
    }
};
$(document).ready(function () {
    $.fn.zTree.init($("#authTree"), setting, zNodes);
    var treeObj = $.fn.zTree.getZTreeObj("authTree");
    treeObj.expandAll(true);
});
var checkAuth = [];
function authOnCheck() {
    var zTree = $.fn.zTree.getZTreeObj("authTree");
    var checkNodes = zTree.getCheckedNodes(true);
    checkAuth.splice(0, checkAuth.length);
    for (var i = 0; i < checkNodes.length; i++) {
        var current = checkNodes[i];
        checkAuth.push(current.name);
    }
    $('#Permissions').val(checkAuth);
}
//#endregion