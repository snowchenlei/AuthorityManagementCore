var addTabs = function (options) {
    //var rand = Math.random().toString();
    //var id = rand.substring(rand.indexOf('.') + 1);
    var url = window.location.protocol + '//' + window.location.host;
    options.url = url + options.url;
    id = "tab_" + options.id;
    $(".active").removeClass("active");
    //如果TAB不存在，创建一个新的TAB
    if (!$("#" + id)[0]) {
        //创建新TAB的title
        title = '<li role="presentation" id="tab_' + id + '" data-id="' + id + '"><a href="#' + id + '" aria-controls="' + id + '" role="tab" data-toggle="tab"><span>' + options.title + '</span>';
        //是否允许关闭
        if (options.close) {
            title += ' <i class="glyphicon glyphicon-remove wb-close-mini" style="cursor: pointer;" tabclose="' + id + '"></i>';
        }
        title += '</a></li>';
        //是否指定TAB内容
        if (options.content) {
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '">' + options.content + '</div>';
        } else {//没有内容，使用IFRAME打开链接
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '"><iframe src="' + options.url + '" class="contentFrame" width="100%" height="' + options.height + '" id="' + id + '" name="' + id + '" frameborder="0" border="0" marginwidth="0" marginheight="0" scrolling="auto" allowtransparency="yes"></iframe></div>';
        }
        //加入TABS
        $(".nav-tabs").append(title);
        $(".tab-content").append(content);
    }
    //激活TAB
    $("#tab_" + id).addClass('active');
    $("#" + id).addClass("active");
};
var closeTab = function (id) {
    //如果关闭的是当前激活的TAB，激活最后一个TAB；如果是最后一个，激活它的前一个
    if ($("li.active").attr('id') === "tab_" + id) {
        var $last = $("#" + $("li.active").attr('id')).parent().find('>li:last');
        if ($last.is($("li.active"))) {
            $last.prev().children('a').click();
        } else {
            $last.children('a').click();
        }
    }
    //关闭TAB
    $("#tab_" + id).remove();
    $("#" + id).remove();
};
$(function () {
    //mainHeight = $(document.body).height() - 45;
    //$('.main-left,.main-right').height(mainHeight);
    //$("[addtabs]").click(function () {
    //    addTabs({ id: $(this).attr("id"), title: $(this).attr('title'), close: true });
    //});

    $(".nav-tabs").on("click", "[tabclose]", function (e) {
        id = $(this).attr("tabclose");
        closeTab(id);
    });
});