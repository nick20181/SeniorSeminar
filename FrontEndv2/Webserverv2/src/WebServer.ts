import express, { response } from 'express';
import { Microservice } from './Classes/CustodialAddressingServices/Microservice';
import {ServiceDictionary} from './Classes/ServiceDictionary';

export class WebServer {
    public rp = require('request-promise-native');
    public port = 8080;
    public app = express();
    public custodialAddressingServiceUri = 'http://localhost:5000/Addressing';
    public serviceList = new ServiceDictionary();

    constructor(serviceList: ServiceDictionary){
        this.app.set('view engine', 'ejs');
        
        this.app.listen(this.port, () => {
            console.log('server started at http://localhost:' + this.port);

            const options = {
                uri: this.custodialAddressingServiceUri + "/all",
                method: 'Get',
                headers: {
                    'Content-Type': 'application/json'
                }
            }
            this.rp(options)
                .then((res: string) =>{
                    const ms : Microservice[] = JSON.parse(res);;
                    ms.forEach(function(ms) {
                        if(ms.shortName == serviceList.CustodialServicesOrganizations){
                            serviceList.setCSO(ms);
                        }
                    });
                })
                .catch(function (err: string) {
                    console.log('error: ' + err);
                });
        });

        this.serviceList = serviceList;

        this.app.get("/", (req, response) => {
            var ul = document.getElementById("OrganizationList");
            var li = document.createElement("li");
            li.setAttribute('id', serviceList.CustodialServicesOrganizations.id!);
        });
    }

}