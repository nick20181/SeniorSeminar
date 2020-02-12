import { ServiceDictionary } from "./Classes/ServiceDictionary";
import { Microservice } from "./Classes/CustodialAddressingServices/Microservice";
import request = require("request-promise-native");
export class APIHandler {
    public serviceDictionary : ServiceDictionary;
    public rp = require('request-promise-native');

    constructor(serviceDictionary: ServiceDictionary){
        this.serviceDictionary = serviceDictionary;
    }

    async refreshServiceDictionary(){
        console.log("starting refresh");
        const options = {
            uri: this.serviceDictionary.CustodiualAddressingServicesURI + "/all",
            method: 'Get',
            headers: {
                'Content-Type': 'application/json'
            }
        }
        let response : string = "";
        await request.get(options)
            .then((body) => {
                response = body;
            })
            .catch((err) => {
                console.log(err);
                response = "Error: " + err.toString();
            });
        let res : Microservice[] = JSON.parse(response);
        res.forEach((ms) => {
            console.log("ms: " + ms.shortName +" sd: " + this.serviceDictionary.getCSO().shortName);
            if(ms.shortName == this.serviceDictionary.getCSO().shortName){
                console.log(ms.serviceName);
                this.serviceDictionary.setCSO(ms);
            }
        });  
        console.log(this.serviceDictionary.getCSO().serviceName);
    }
 
    getCSOService() : Microservice {
        var ms: Microservice = this.serviceDictionary.getCSO();
        return this.serviceDictionary.getCSO();
    }

}