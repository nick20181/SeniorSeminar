import express, { response } from 'express';
import { Microservice } from "./Classes/CustodialAddressingServices/Microservice";
import {ServiceDictionary} from './Classes/ServiceDictionary';
import { APIHandler } from './API Handler';

export class WebServer {
    public rp = require('request-promise-native');
    public port = 8080;
    public app = express();
    public apiHandler? :APIHandler;

    constructor(apiHandler: APIHandler){
        this.apiHandler = apiHandler;

        this.app.set('view engine', 'ejs');
        
        this.app.listen(this.port, () => {
            console.log('server started at http://localhost:' + this.port);
        });

        this.app.get("/", (req, response) => {
            apiHandler.refreshServiceDictionary();
            response.send("Test" + apiHandler.getCSOService().serviceName)
        });
    }

}