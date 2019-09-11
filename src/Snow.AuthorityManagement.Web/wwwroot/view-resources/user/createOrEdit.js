$.validator.unobtrusive.parse('form');
if ($('#Role').data('select2') !== undefined) {
    $('#Role').data('select2').destroy();
}
$('#Role').select2({
    language: "zh-CN",// 指定语言为中文，国际化才起效
    placeholder: '请选择',
    width: '100%',
    //data: modules
});