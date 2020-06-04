var sensorId = 1

function generateBuilding(floors, flats, startX, startY) {
    var building = []
    var sensors = []
    for(var x = 0; x < floors; x++){
        var floorlist = []
        for(var y = 0; y < flats; y++){
            var flat = generateFlat(startX + y*250, startY + x*260, 200, 200)
            floorlist.push(flat.rooms)
            flat.sensors.forEach(s => {
                sensors.push(s)
            })
        }
        building.push(floorlist)
    }
    return { building: building, sensors: sensors }
}

function generateFlat(startX, startY, sizeX, sizeY){
    corner1 = { x: startX, y: startY}
    corner2 = { x: startX, y: startY + sizeY}
    corner3 = { x: startX + sizeX, y: startY + sizeY}
    corner4 = { x: startX + sizeX, y: startY}
    sensors = []
    mainblock = 
    {
        corners: [corner1, corner2, corner3, corner4]
    }
    var rooms = splitRoomRecursive(mainblock, 2)
    rooms.forEach(room => {
        room.sensors = []
        room.name = 'r' + Math.random()
        for(var i = 0; i < Math.random()*3; i++){
            var difX = room.corners[2].x - room.corners[0].x
            var difY = room.corners[1].y - room.corners[0].y
            var s =  { id: Math.random(), x: room.corners[0].x + Math.random()*difX, y: room.corners[0].y + Math.random()*difY }
            sensors.push(s)
        }
    });
    return { rooms: rooms, sensors: sensors }
}

function splitRoomRecursive(room, toBeDone){
    var rooms = splitRoom(room)
    var roomsToReturn = []
    if(toBeDone > 0)
    {
        toBeDone--
        rooms.forEach(function(_room, index, array){
            splitRoomRecursive(_room, toBeDone).forEach(function(r, index, array)
            {
                roomsToReturn.push(r)
            })
        })
        return roomsToReturn
    }
    return rooms
}

function splitRoom(room){
    var diff = Math.random() - 0.5
    var vertical = diff > 0
    if(vertical)
    {
        var y = (room.corners[1].y + room.corners[0].y)/2
        var shift =  (room.corners[1].y - room.corners[0].y)*diff*0.5
        y += shift
        var newcorner1 = { x: room.corners[0].x, y}
        var newcorner2 = { x: room.corners[2].x, y}
        room1 = 
        {
            corners: [room.corners[0], newcorner1, newcorner2, room.corners[3]]
        }
        room2 = 
        {
            corners: [newcorner1, room.corners[1], room.corners[2], newcorner2]
        }
    }
    else
    {
        var x = (room.corners[1].x + room.corners[2].x)/2
        var shift =  (room.corners[1].x - room.corners[2].x)*diff*0.5
        x += shift
        var newcorner1 = { x, y: room.corners[2].y}
        var newcorner2 = { x, y: room.corners[3].y}
        room1 = 
        {
            corners: [room.corners[0], room.corners[1] , newcorner1, newcorner2]
        }
        room2 = 
        {
            corners: [newcorner2, newcorner1, room.corners[2], room.corners[3]]
        }
    }
    return [room1, room2]
}

function printRoom(room, canvas){
    
    var path = new fabric.Path('M ' + room.corners[0].x + ' ' + room.corners[0].y  +  ' L ' + room.corners[1].x + ' ' + room.corners[1].y  + ' L ' + room.corners[2].x + ' ' + room.corners[2].y  + ' L ' + room.corners[3].x + ' ' + room.corners[3].y  +' z')
    path.set({ fill: 'white', stroke: 'blue', opacity: 0.4 })
    path.selectable = false
    path.type = 'room'
    canvas.add(path).setActiveObject(path)
    
    return path
    
}

function printSensors(sensors, canvas) {
    sensors.forEach(s => {
        var circle = new fabric.Circle(
            {
                left: s.x,
                top: s.y,
                originX: 'center',
                originY: 'center',
                stroke: 'blue',
                fill: 'rgba(0,0,0,0)',
                radius: 4
            }
        )
        
        circle.type = 'sensor'
        circle.id = s.id
        console.log(s)
        circle.name = s.name
        circle.selectable = false
        canvas.add(circle)
    });
}

function maxRooms(building) {
    max = 0
    building.forEach(floor => {
        if(floor.length > max)
            max = floor.length
    });
    return max
}

function printBuilding(building, canvas, sensors) {
        var width = maxRooms(building)*250 + 70
        for(var i = 0; i < building.length; i++) {
        floorname = new fabric.Text((i + 1) + " floor",
            {
                fontSize: 23, 
                fill: '#ffffff',
                left: 20,
                top: 150 + 260*i,
                angle: -90,
                originX: 'center',
                originY: 'center',
                opacity: 0.7,
                selectable: false
            }
        )
        canvas.add(floorname)
        var line = new fabric.Path('M 20 ' + (20 + i*260) + ' L ' + width + ' ' + (20 + i*260) + ' z')
        line.set( { fill: 'white', stroke: 'white', strokeWidth: 2, opacity: 0.7})
        canvas.add(line)
        building[i].forEach(flat => {
            flat.forEach(room => {
                printRoom(room, canvas)
            });
        });
    };

    
    var obs = canvas.getObjects()
    obs.forEach(o => {
        if( o.type == 'sensor') {
            canvas.bringToFront(o)
        }
    });
}
