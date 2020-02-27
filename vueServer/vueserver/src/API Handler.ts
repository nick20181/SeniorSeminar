import { ServiceDictionary } from "./ServiceDictionary";
import { Microservice } from "./Custodial.Addressing.Service/Microservice";
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
        await this.rp.get(options)
            .then((body: string) => {
                response = body;
            })
            .catch((err: { toString: () => string; }) => {
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
        await this.rp.get(options)
            .then((body: string) => {
                response = body;
            })
            .catch((err: { toString: () => string; }) => {
                console.log(err);
                response = "Error: " + err.toString();
            });
        return response;
    }

}