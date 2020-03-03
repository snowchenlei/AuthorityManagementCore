function showJson(e) {
    var content = '<div class="modal-header">';
    content += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    content += '<span aria-hidden="true">&times;</span>';
    content += '</button>';
    content += '<h4 class="modal-title" id="modelTitle">展示json</h4>';
    content += '</div>';
    content += '<div class="modal-body form-horizontal"><pre><code id="json"></code></pre></div>';
    $('#modifyContent').html(content);
    $('#modifyModal').modal();
    //json展示
    $('#json').JSONView($(e).attr('title').replace('"{', '{').replace('}"', '}').replace(/\\/g, ''));
}
function showText(e) {
    var val = $(e).attr('title');
    var content = '<div class="modal-header">';
    content += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    content += '<span aria-hidden="true">&times;</span>';
    content += '</button>';
    content += '<h4 class="modal-title" id="modelTitle">展示文本</h4>';
    content += '</div>';
    content += '<div class="modal-body form-horizontal"><pre><code id="json">' + val + '</code></pre></div>';
    var dialog = bootbox.dialog({
        size: 'large',
        title: '展示文本',
        message: `<div class="modal-body form-horizontal"><pre><code id="json">${val}</code></pre></div>`
    });
}
function showLocation(val) {
    $.get("/IP/Index", { ip: val }, function (data) {
        $('#modifyContent').html(data);
        $("#modifyModal").modal();
    });
}