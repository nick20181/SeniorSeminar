export class NetworkSettings {
    addresses: string[];
    ports: string[];

    constructor(ports : string[], addresses: string[]){
        this.ports = ports,
        this.addresses = addresses
    }
}