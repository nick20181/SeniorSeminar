import {ICasSettings} from '../../Interfaces/CustodialAddressingServices/ICasSettings'; 

export class CasSetting implements ICasSettings{
    public address: string;
    public port: string;

    constructor(port : string, address: string){
        this.port = port,
        this.address = address
    }
}