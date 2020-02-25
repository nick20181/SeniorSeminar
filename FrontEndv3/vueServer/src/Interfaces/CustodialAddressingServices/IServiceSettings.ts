import {INetworkSettings} from './INetworkSettings';
import {IDatabaseSettings} from './IDatabaseSettings';
import {ICasSettings} from './ICasSettings'; 
export interface IServiceSettings {
    networkSettings: INetworkSettings;
    databaseSettings: IDatabaseSettings;
    casSettings: ICasSettings;
  }