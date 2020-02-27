import { NetworkSettings } from './NetworkSettings';
import { DatabaseSettings } from './DatabaseSettings';
import { CasSetting } from './CasSettings';

export class ServiceSettings implements ServiceSettings{
    public networkSettings: NetworkSettings; 
    public databaseSettings: DatabaseSettings;
    public casSettings: CasSetting;

    constructor(networkSettings: NetworkSettings, databaseSettings: DatabaseSettings,
         casSettings: CasSetting){
        this.networkSettings = networkSettings;
        this.casSettings = casSettings;
        this.databaseSettings = databaseSettings;
    }


}