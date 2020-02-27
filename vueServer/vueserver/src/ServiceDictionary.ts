import { Microservice } from './Custodial.Addressing.Service/Microservice';
import { ServiceSettings } from './Custodial.Addressing.Service/ServiceSettings';
import { NetworkSettings } from './Custodial.Addressing.Service/NetworkSettings';
import { DatabaseSettings } from './Custodial.Addressing.Service/DatabaseSettings';
import { CasSetting } from './Custodial.Addressing.Service/CasSettings';

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