[Select2文档](https://select2.org/)

1. Select2 本地数据
```js
var data = [
    {
        id: 0,
        text: 'enhancement'
    },
    {
        id: 1,
        text: 'bug'
    },
    {
        id: 2,
        text: 'duplicate'
    },
    {
        id: 3,
        text: 'invalid'
    },
    {
        id: 4,
        text: 'wontfix'
    }
];
$('#ParentId').select2({
    language: "zh-CN",// 指定语言为中文，国际化才起效
    placeholder: '请选择',
    theme: 'bootstrap4',
    width: '100%',
    data: data
});
```
2. Select2 Ajax加载数据
```js
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
        delay: 250,// 延迟ms
        data: function (params) {
            var query = {
                pageIndex: 0,
                pageSize: 1000,
                name: params.term
            };
            return query;
        },
        processResults: function (data) {
            // 请求成功回调，对返回数据进行操作
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
```
> *注意：如果是本地数据Select2的搜索是本地进行的，但如果是Ajax加载数据搜索则会走请求。
3. Select2 Ajax加载数据本地搜索结合
思路：手动请求数据，并以本地数据方式初始化Select2插件
```js
// 首先提供一个请求数据的方法，参数为回调的处理逻辑。
function getDatas(callback) {    
    $.ajax({
        url: '/api/datas',
        method: "GET",
        dataType: 'json',
        delay: 250,,
        success: function (data) {
            callback(data);
        },
        cache: true
    });
}
getDatas(function(data) {
    $('#ParentId').select2({
        language: "zh-CN",
        placeholder: '请选择',
        theme: 'bootstrap4',
        width: '100%',
        data: data
    });
})
```