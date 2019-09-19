$(function () {
    $.validator.unobtrusive.parse('form');
    $('#Menu_ParentID').select2({
        language: 'zh-CN',
        placeholder: '请选择',
        allowClear: true,
        theme: 'bootstrap4',
        width: '100%'
    });
});