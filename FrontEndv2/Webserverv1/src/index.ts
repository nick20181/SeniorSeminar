import express, { response } from "express";
import request from "request-promise-native";
const app = express();
const port = 8080; // default port to listen
const casUri = 'http://localhost:5000/Addressing';

// define a route handler for the default home page
// app.get( "/", ( req, res ) => {
//    res.send( "Hello world!" );
// } );

app.get( "/", async (req, res) => {
  const clientServerOptions = {
  uri: 'http://localhost:5000/Addressing/all',
  method: 'GET',
  headers: {
    'Content-Type': 'application/json'
  }
}

request(clientServerOptions, function temp(error, res){
  console.log(error, res.body)
  const JsonObject: object = JSON.parse(res.body);
})
});

// start the Express server
app.listen( port, () => {
    console.log( `server started at http://localhost:${ port }` );
} );

interface Microservice {
  timeCreated: number;
  id: string;
  isDeleted: boolean;
  serviceName: string;
  settings: ServiceSettings;
  discription: string;
  shortName: string;
}

interface ServiceSettings{
  networkSettings: NetworkSettings;
  databaseSettings: DatabaseSettings;
  casSettings: CasSettings;
}

interface NetworkSettings{
  addresses: string[];
  ports: string[];
}

interface DatabaseSettings{
  connectionString: string;
  database: string;
  collection: string;
}

interface CasSettings{
  address: string;
  port: string;
}