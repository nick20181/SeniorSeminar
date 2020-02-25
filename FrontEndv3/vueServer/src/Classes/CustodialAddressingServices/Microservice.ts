import {IMicroservice} from '../../Interfaces/CustodialAddressingServices/IMicroservice';
import {IServiceSettings} from '../../Interfaces/CustodialAddressingServices/IServiceSettings';
import { ServiceSettings } from './ServiceSettings';
import { NetworkSettings } from './NetworkSettings';
import { DatabaseSettings } from './DatabaseSettings';
import { CasSetting } from './CasSettings';

export class Microservice implements IMicroservice{
    public timeCreated: number;
    public id: string;
    public isDeleted: boolean;
    public serviceName: string;
    public settings: IServiceSettings;
    public discription: string;
    public shortName: string;

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

    static NullService() : Microservice{
        return new Microservice(0, "", true, "", new ServiceSettings(
            new NetworkSettings([],[]), new DatabaseSettings("","",""),
             new CasSetting("","")), "", "");
    }

}