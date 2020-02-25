import {IServiceSettings} from '../../Interfaces/CustodialAddressingServices/IServiceSettings';
import {INetworkSettings} from '../../Interfaces/CustodialAddressingServices/INetworkSettings';
import {IDatabaseSettings} from '../../Interfaces/CustodialAddressingServices/IDatabaseSettings';
import {ICasSettings} from '../../Interfaces/CustodialAddressingServices/ICasSettings';

export class ServiceSettings implements IServiceSettings{
    public networkSettings: INetworkSettings; 
    public databaseSettings: IDatabaseSettings;
    public casSettings: ICasSettings;

    constructor(networkSettings: INetworkSettings, databaseSettings: IDatabaseSettings,
         casSettings: ICasSettings){
        this.networkSettings = networkSettings;
        this.casSettings = casSettings;
        this.databaseSettings = databaseSettings;
    }


}