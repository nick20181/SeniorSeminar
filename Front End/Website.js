const express = require('express');
const bodyParser = require('body-parser');
const request = require('request');
const app = express()
const dataPost = JSON.stringify({
  "timeCreated": "2017-01-18T10:45:30Z",
  "iD": "ID",
  "isDeleted": false,
  "serviceName": "TestService",
  "settings": {
    "networkSettings": {
      "addresses": [ "11.0.0.22" ],
      "ports": [ "1111", "2222" ]
    },
    "databaseSettings": {
      "address": "10.0.0.1",
      "port": "1111",
      "collectionNames": [ "CollectionOne", "CollectionTwo" ],
      "databaseNames": [ "DatabaseOne" ]
    }
  },
  "discription": "Discription of service",
  "shortName": "Test"
})

const dataGet = JSON.stringify({
  "timeCreated": "2017-01-18T10:45:30Z",
  "iD": null,
  "isDeleted": false,
  "serviceName": "TestService",
  "settings": {
    "networkSettings": {
      "addresses": [ "11.0.0.22" ],
      "ports": [ "1111", "2222" ]
    },
    "databaseSettings": {
      "address": "10.0.0.1",
      "port": "1111",
      "collectionNames": [ "CollectionOne", "CollectionTwo" ],
      "databaseNames": [ "DatabaseOne" ]
    }
  },
  "discription": "Discription of service",
  "shortName": "Test"
})

app.use(express.static('public'));
app.use(bodyParser.urlencoded({ extended: true }));
app.set('view engine', 'ejs')

app.post('/', function (req, res) {
  var clientServerOptions = {
    uri: 'http://localhost:5000/Addressing',
    body: dataPost,
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    }
  }

  request(clientServerOptions, function(error, response){
    console.log(error, response.body)
    console.log(JSON.parse(response.body))
  })
  
})

app.get('/', function(req, res){
  res.render('index');
  var clientServerOptions = {
    uri: 'http://localhost:5000/Addressing',
    body: dataGet,
    method: 'GET',
    headers: {
      'Content-Type': 'application/json'
    }
  }

  request(clientServerOptions, function(error, response){
    console.log(error, response.body)
  })
  
})

app.head('/', function (req, res) {
  let city = req.body.city;
  let url = `http://api.openweathermap.org/data/2.5/weather?q=${city}&units=imperial&appid=${apiKey}`

  request(url, function (err, response, body) {
    if(err){
      res.render('index', {weather: null, error: 'Error, please try again'});
    } else {
      let weather = JSON.parse(body)
      if(weather.main == undefined){
        res.render('index', {weather: null, error: 'Error, please try again'});
      } else {
        let weatherText = `It's ${weather.main.temp} degrees in ${weather.name}!`;
        res.render('index', {weather: weatherText, error: null});
      }
    }
  });
})

app.listen(3000, function () {
  console.log('Example app listening on port 3000!')
})