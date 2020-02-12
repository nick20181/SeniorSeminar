import express, { response } from 'express';
import { Microservice } from "./Classes/CustodialAddressingServices/Microservice";
import {ServiceDictionary} from './Classes/ServiceDictionary';
import { APIHandler } from './API Handler';
import fs from 'fs';

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
            var html: string = fs.readFileSync('views/index.html', 'utf8');
            apiHandler.refreshServiceDictionary();
            console.log(html);
            html.replace("[CSO]", apiHandler.getCSOService().serviceName);
            console.log("[CSO]");
            console.log(html)
            response.send(html);
        });
    }

} 