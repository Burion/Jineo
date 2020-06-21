var canvas
var mySensor = null
var sensorsX 
var sensorsY
function initBlueprint(building, sensors) {
    
    var scaled = false
    var imageUrl = 'https://cdn.dribbble.com/users/85782/screenshots/826294/untitled-1.png'
    
    function wid() {
        document.getElementById('c').height = 40 + building.length*260
        document.getElementById('c').width = 100 + maxRooms(building)*250
    }
    
    wid()
    if(canvas == null)
    {
        canvas = new fabric.Canvas('c');
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
            // var delta = opt.e.deltaY;
            // var pointer = canvas.getPointer(opt.e);
            // var zoom = canvas.getZoom();
            // if(scaled) {
            //     zoom = zoom - 2;
            //     scaled = false
            // }
            // else {
            //     zoom = zoom + 2;
            //     scaled = true
            // }
            // if (zoom > 4) zoom = 4;
            // if (zoom < 0.5) zoom = 0.5;
            // canvas.zoomToPoint({ x: opt.e.offsetX, y: opt.e.offsetY }, zoom);
            // opt.e.preventDefault();
            // opt.e.stopPropagation();
            $('#exampleModalCenter').modal()
            var pointer = canvas.getPointer(opt.e)
            sensorsX = opt.e.offsetX
            sensorsY = opt.e.offsetY
            });
    }
    canvas.clear()
    canvas.selection = false
    printBuilding(building, canvas)

    printSensors(sensors, canvas)
    dropSensorsColors()
    canvas.setBackgroundColor({source: imageUrl, repeat: 'repeat'}, function () {
        canvas.renderAll();
    });
    canvas.renderAll()


}

function getSensorById(id) {
    var sens
    sensors.forEach(s => {
        if(s.id == id){
            sens = s
        }
    });
    return sens
}

function dropSensorsColors() {
var obs = canvas.getObjects()
obs.forEach(o => {
    if(o.type == 'sensor') {
    var sen = getSensorById(o.id)
    var _json = JSON.parse(sen.data)
    var danger = false
    if(_json[_json.length - 1].value > sen.upperValue || _json[_json.length - 1].value < sen.lowerValue) {
        danger = true
    }
    if(danger) {
        o.set('fill', 'rgba(255,0,0,1)')
    }
    else {
        o.set('fill', 'rgba(0,0,0,0)')
    }
}
})
}

function selectSensor(sensorId) {
    var obs = canvas.getObjects()
    var _sensor 
    var datastring
    _sensor = getSensorById(sensorId)
    datastring = _sensor.data
    var json = JSON.parse(datastring)
    

    obs.forEach( o => { if(o.id == sensorId) { sensor = o; return}})
    if(sensor.type == 'sensor') {
        dropSensorsColors()
        mySensor = sensor
        sensor.set('fill', 'yellow');
        canvas.renderAll();
        
        var href = '/store/' + _sensor.product.id
        var htm = "<a href='/home/store/" + _sensor.product.id + "'>" + _sensor.product.name + "</a>"
        document.getElementById('sensorname').innerHTML = htm
        document.getElementById('sname').innerHTML = _sensor.name
        if(_sensor.product.productTypeId == 1)
        {
            document.getElementById('sensortype').innerHTML = 'Toughness'
        }
        if(_sensor.product.productTypeId == 2)
        {
            document.getElementById('sensortype').innerHTML = 'Temperature'
        }
        if(_sensor.product.productTypeId == 3)
        {
            document.getElementById('sensortype').innerHTML = 'Pressure'
        }
        drawChart(json)
    }
}



