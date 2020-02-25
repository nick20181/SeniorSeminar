import {WebServer} from './WebServer'
import { ServiceDictionary } from './Classes/ServiceDictionary';
import { APIHandler } from './API Handler';

const serviceDictionary: ServiceDictionary = new ServiceDictionary();
const apiHandler: APIHandler = new APIHandler(serviceDictionary);
const server :WebServer = new WebServer(apiHandler);