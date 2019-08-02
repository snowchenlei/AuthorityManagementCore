//添加标签
var addTabs = function (options) {
    //var rand = Math.random().toString();
    //var id = rand.substring(rand.indexOf('.') + 1);
    var url = window.location.protocol + '//' + window.location.host;
    options.url = url + options.url;
    var id = "tab_" + options.id;

    $(".active").removeClass("active");
    //如果TAB不存在，创建一个新的TAB
    var htmlArr = new Array();
    if (!$("#" + id)[0]) {
        //创建新TAB的title

        htmlArr.push('<li class="nav-item"><a class="nav-link" id="tab_' + id + '" data-id="' + id + '" data-toggle="tab" href="#' + id + '" role="tab" aria-controls="' + id + '" aria-selected="true">' + options.title + '');
        //是否允许关闭
        if (options.close) {
            htmlArr.push(
                '<button type="button" class="close" aria-label="Close" onclick=closeTab("' + id + '")><span aria-hidden="true">&times;</span></button>');
            //htmlArr.push('<i class="fa fa-close" style="cursor: pointer;" onclick=closeTab("' + id + '")></i>');
        }
        htmlArr.push('</a></li>');
        $('#tabHeader').append(htmlArr.join(''));
        htmlArr = [];
        //是否指定TAB内容
        if (options.content) {
            htmlArr.push('<div role="tabpanel" class="tab-pane" id="' + id + '">' + options.content + '</div>');
        } else {//没有内容，使用IFRAME打开链接
            htmlArr.push('<div class="tab-pane fade" id="' + id + '" role="tabpanel" aria-labelledby="tab-' + id + '"><iframe src="' + options.url + '" class="contentFrame" width="100%" height="' + options.height + '" id="' + id + '" name="' + id + '" frameborder="0" border="0" marginwidth="0" marginheight="0" scrolling="auto" allowtransparency="yes"></iframe></div>');
        }
        $('#tabContent').append(htmlArr);
    }
    //激活TAB
    $("#tab_" + id).tab('show');
};
//关闭标签
var closeTab = function (id) {
    //如果关闭的是当前激活的TAB，激活最后一个TAB；如果是最后一个，激活它的前一个
    //必须先删除在show，否则关联的body无法切换
    var $active = $(".content-wrapper .content-header .active");
    var activeId = $active.data('id');
    var $lastTab;
    if (id === activeId) {
        var $last = $("#tab_" + activeId).parent().parent().find('>li:last');
        if ($last.is($active.parent())) {
            $lastTab = $last.prev().children('a');
        } else {
            $lastTab = $last.children('a');
        }
    }
    //关闭TAB
    $("#tab_" + id).tab('dispose');
    $("#tab_" + id).parent().remove();
    $("#" + id).remove();
    if ($lastTab !== undefined && $lastTab !== null) {
        $lastTab.tab('show');
    }
};