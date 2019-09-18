$.validator.unobtrusive.parse('form');
$('#ParentId').select2({
    language: "zh-CN",// 指定语言为中文，国际化才起效
    placeholder: '请选择',
    theme: 'bootstrap4',
    width: '100%',
    //minimumResultsForSearch: -1,//禁用搜索
    //minimumInputLength: 1,//输入多少个字符开始搜索
    ajax: {
        url: '/api/menus',
        dataType: 'json',
        delay: 250,
        data: function (params) {
            var query = {
                pageIndex: 0,
                pageSize: 1000,
                name: params.term
            };
            return query;
        },
        processResults: function (data) {
            var results = new Array();
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                results[i] = {
                    id: item.id,
                    text: item.name
                };
            }
            return {
                results: results  //必须赋值给results并且必须返回一个obj
            };
        },
        cache: true
    }
});