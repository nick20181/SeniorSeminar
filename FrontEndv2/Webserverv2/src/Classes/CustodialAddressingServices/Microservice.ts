import {IMicroservice} from '../../Interfaces/CustodialAddressingServices/IMicroservice';
import {IServiceSettings} from '../../Interfaces/CustodialAddressingServices/IServiceSettings';

export class Microservice implements IMicroservice{
    public timeCreated?: number;
    public id?: string;
    public isDeleted?: boolean;
    public serviceName?: string;
    public settings?: IServiceSettings;
    public discription?: string;
    public shortName?: string;

    constructor(timeCreated: number, id: string, isDeleted: boolean, serviceName: string,
        settings: IServiceSettings, discription: string, shortname: string){
            this.timeCreated = timeCreated;
            this.id = id;
            this.isDeleted = isDeleted;
            this.serviceName = serviceName;
            this.settings = settings;
            this.discription = discription;
            this.shortName = shortname;
    }
}