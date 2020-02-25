import { INetworkSettings } from '../../Interfaces/CustodialAddressingServices/INetworkSettings';

export class NetworkSettings implements INetworkSettings{
    addresses: string[];
    ports: string[];

    constructor(ports : string[], addresses: string[]){
        this.ports = ports,
        this.addresses = addresses
    }
}