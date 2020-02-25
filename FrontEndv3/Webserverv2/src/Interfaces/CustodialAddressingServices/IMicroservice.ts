import {IServiceSettings} from './IServiceSettings';

export interface IMicroservice {
    timeCreated?: number;
    id?: string;
    isDeleted?: boolean;
    serviceName?: string;
    settings?: IServiceSettings;
    discription?: string;
    shortName?: string;
  }