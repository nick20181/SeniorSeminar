import { Microservice } from "./CustodialAddressingServices/Microservice";
import { ServiceSettings } from "./CustodialAddressingServices/ServiceSettings";
import { NetworkSettings } from "./CustodialAddressingServices/NetworkSettings";
import { DatabaseSettings } from "./CustodialAddressingServices/DatabaseSettings";
import { CasSetting } from "./CustodialAddressingServices/CasSettings";

export class ServiceDictionary{
    public CustodiualAddressingServicesURI = 'http://localhost:5000/Addressing';
    private CustodialServicesOrganizations:Microservice;
    
    constructor(){
        var CSO: Microservice = new Microservice(0, "", true, "", 
            new ServiceSettings(new NetworkSettings([""],[""]), new DatabaseSettings("", "", ""),
            new CasSetting("","")), "", "C.S.O");
        this.CustodialServicesOrganizations = CSO;
    }

    setCSO(CSO: Microservice){
        this.CustodialServicesOrganizations = CSO;
    }

    getCSO() : Microservice{
        return this.CustodialServicesOrganizations;
    }


}