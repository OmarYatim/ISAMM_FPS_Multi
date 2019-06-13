var port = process.env.PORT || 4000;
var io = require('socket.io')(port);
var shortId = require('shortid'); 
const createCsvWriter = require('csv-writer').createObjectCsvWriter;  
const csvWriter = createCsvWriter({  
  path: 'out.csv',
  header: [
    {id: 'date', title: 'Date'},
    {id: 'xposition', title: 'Xposition'},
    {id: 'yposition', title: 'Yposition'},
    {id: 'zposition', title: 'Zposition'},
  ],
  append : true
});


console.log("server started on port " + port);

io.on('connection',function(socket){
    console.log('connection succesful');
    var PlayerID = shortId.generate();

    socket.emit('Register',{id : PlayerID});

    socket.on('Position', function(data){
        console.log('client position ', JSON.stringify(data));
        var Position = [
            {
                date : data.date,
                xposition : data.x,
                yposition : data.y,
                zposition : data.z
            }
        ]
        csvWriter  
            .writeRecords(Position)
            .then(()=> console.log('The CSV file was written successfully'));
    });

    socket.on('disconnect', function () {
        console.log('client disconected');
    });
});