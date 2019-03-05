function loadPanel() {
    $('#searchPanel').on('show.bs.collapse', function () {
        var $search = document.getElementById('searchTitle');
        //$search.setAttribute('class', 'glyphicon glyphicon-menu-up');
        $search.style.transform = 'rotatez(' + 0 + 'deg)';
    })
    $('#searchPanel').on('hide.bs.collapse', function () {
        var $search = document.getElementById('searchTitle');
        //$search.setAttribute('class', 'glyphicon glyphicon-menu-down');
        $search.style.transform = 'rotatez(' + 180 + 'deg)';
    });
}
// 等待所有加载
//$(window).load(function () {
//    $('body').addClass('loaded');
//    $('#loader-wrapper .load_title').remove();
//});
//$.fn.datetimepicker.dates['zh-CN'] = {
//        //具体配置可参见官网-I18N国际化
//        days: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"],
//        daysShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
//        daysMin: ["日", "一", "二", "三", "四", "五", "六", "日"],
//        months: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
//        monthsShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
//        today: "今天",
//        suffix: [],
//        meridiem: ["上午", "下午"]
//    };
//$(".form_datetime").datetimepicker({
//    format: "yyyy/mm/dd hh:ii",
//    //initialDate: //'1970/01/01',
//    autoclose: true,
//    todayBtn: true,
//    language: 'zh-CN'
//});
//参数设置，若用默认值可以省略以下面代
toastr.options = {
    closeButton: true, //是否显示关闭按钮
    debug: false, //是否使用debug模式
    progressBar: true,//是否显示进度条
    positionClass: "toast-top-center",//弹出窗的位置
    showDuration: "300",//显示的动画时间
    hideDuration: "1000",//消失的动画时间
    timeOut: "2000", //展现时间
    extendedTimeOut: "1000",//加长展示时间
    showEasing: "swing",//显示时的动画缓冲方式
    hideEasing: "linear",//消失时的动画缓冲方式
    showMethod: "fadeIn",//显示时的动画方式
    hideMethod: "fadeOut" //消失时的动画方式
}
//bootbox基本设置
bootbox.setLocale('zh_CN');