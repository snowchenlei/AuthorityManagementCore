$(function () {
    //var popCanvas = $("#popChart");
    //var popCanvas = document.getElementById("popChart");
    //var popCanvas = document.getElementById("popChart").getContext("2d");

    var logCountPie = document.getElementById("logCountPie").getContext("2d");
    $.getJSON('/api/charts/getLogPie', function (result) {
        var colors = [];
        for (var label in result.labels) {
            switch (result.labels[label]) {
                case 'Verbose':
                    colors.push('#6c757d');
                    break;
                case 'Debug':
                    colors.push('#007bff');
                    break;
                case 'Information':
                    colors.push('#17a2b8');
                    break;
                case 'Warning':
                    colors.push('#ffc107');
                    break;
                case 'Error':
                    colors.push('#dc3545');
                    break;
                case 'Fatal':
                    colors.push('#343a40');
                    break;
            }
        }
        var pieChart = new Chart(logCountPie, {
            type: 'pie',
            options: {
                title: {
                    display: true,
                    text: '日志分布统计'
                }
            },
            data: {
                labels: result.labels,
                datasets: [{
                    data: result.dates,
                    backgroundColor: colors,
                    hoverBackgroundColor: colors
                }]
            }
        });
    });
});