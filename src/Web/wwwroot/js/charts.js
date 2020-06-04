
function drawChart(data){
    var values = []
    var dates = []
    for(var x = 0; x < data.length; x++) {
        values.push(data[x].value)
        dates.push(data[x].date)
    }
    var ctx = document.getElementById('myChart').getContext('2d');
    ctx.clearRect(0, 0, document.getElementById('myChart').width, document.getElementById('myChart').height);
    var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: 'line',
            
            // The data for our dataset
            data: {
                labels: dates,
                datasets: [{
                    label: 'Data',
                backgroundColor: 'rgb(255, 99, 132)',
                borderColor: 'rgb(255, 99, 132)',
                data: values
            }]
        },
    
    // Configuration options go here
        options: {}
    });
}