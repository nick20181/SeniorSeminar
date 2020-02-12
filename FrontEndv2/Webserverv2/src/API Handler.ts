import { ServiceDictionary } from "./Classes/ServiceDictionary";
import { Microservice } from "./Classes/CustodialAddressingServices/Microservice";
import request = require("request-promise-native");
export class APIHandler {
    public serviceDictionary : ServiceDictionary;
    public rp = require('request-promise-native');

    constructor(serviceDictionary: ServiceDictionary){
        this.serviceDictionary = serviceDictionary;
        this.refreshServiceDictionary();
    }

    async refreshServiceDictionary(){
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
            if(ms.shortName == this.serviceDictionary.getCSO().shortName){
                this.serviceDictionary.setCSO(ms);
            }
        });  
    }
 
    getCSOService() : Microservice {
        var ms: Microservice = this.serviceDictionary.getCSO();
        return this.serviceDictionary.getCSO();
    }

    async getOrganizationList(): Promise<string>{
        const options = {
            uri: "http://" + this.serviceDictionary.getCSO().settings.networkSettings.addresses[1] + ":" + 
            this.serviceDictionary.getCSO().settings.networkSettings.ports[0] + "/organization/all",
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
        return response;
    }

}