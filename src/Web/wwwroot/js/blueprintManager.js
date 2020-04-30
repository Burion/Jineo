var canvas
function initBlueprint(building, sensors) {

    var scaled = false
    var imageUrl = 'https://cdn.dribbble.com/users/85782/screenshots/826294/untitled-1.png'

    function wid() {
        document.getElementById('c').height = 40 + building.length*260
        document.getElementById('c').width = 100 + maxRooms(building)*250
    }
                
    wid()
    canvas = new fabric.Canvas('c');
    canvas.selection = false
    printBuilding(building, canvas)
    console.log(sensors.length)
    console.log('fuck')
    printSensors(sensors, canvas)
    canvas.setBackgroundColor({source: imageUrl, repeat: 'repeat'}, function () {
        canvas.renderAll();
    });
    canvas.renderAll()

    canvas.on('mouse:over', function(e) {
        if(e.target == null)
            return
        if(e.target.type == 'room')
        {
            e.target.set('opacity', '0.7');
        }

        if(e.target.type == 'sensor')
        {
            e.target.set('stroke', 'red');
        }
    canvas.renderAll();

    });

    canvas.on('mouse:out', function(e) {
        if(e.target.type == 'room')
        {
            e.target.set('opacity', '0.4');
        }

        if(e.target.type == 'sensor')
        {
            e.target.set('stroke', 'blue');
        }
        canvas.renderAll();
    });

    canvas.on('mouse:down', function(opt) {
        selectSensor(opt.target.id, canvas)
    })

    

    canvas.on('mouse:dblclick', function(opt) {
    var delta = opt.e.deltaY;
    var pointer = canvas.getPointer(opt.e);
    var zoom = canvas.getZoom();
    if(scaled) {
        zoom = zoom - 2;
        scaled = false
    }
    else {
        zoom = zoom + 2;
        scaled = true
    }
    if (zoom > 4) zoom = 4;
    if (zoom < 0.5) zoom = 0.5;
    canvas.zoomToPoint({ x: opt.e.offsetX, y: opt.e.offsetY }, zoom);
    opt.e.preventDefault();
    opt.e.stopPropagation();
    });
}

function selectSensor(sensorId) {
    console.log(sensorId)
    var sensor
    var obs = canvas.getObjects()
    obs.forEach( o => { if(o.id == sensorId) { sensor = o; return}})
    if(sensor.type == 'sensor') {
        obs.forEach(o => {
            if(o.type == 'sensor') {
                o.set('fill', 'rgba(0,0,0,0)')
            }
        });
        sensor.set('fill', 'orange');
        canvas.renderAll();
        drawChart('myCanvas')
    }
}