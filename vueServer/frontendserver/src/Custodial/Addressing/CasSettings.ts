export class CasSetting{
    public address: string;
    public port: string;

    constructor(port : string, address: string){
        this.port = port,
        this.address = address
    }
}