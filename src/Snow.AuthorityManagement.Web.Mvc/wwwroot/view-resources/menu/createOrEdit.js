$(function () {
    $.validator.unobtrusive.parse('form');
    $("input[type='number']").inputSpinner();
    $('#Menu_ParentID').select2({
        language: 'zh-CN',
        placeholder: '请选择',
        allowClear: true,
        theme: 'bootstrap4',
        width: '100%'
    });
});