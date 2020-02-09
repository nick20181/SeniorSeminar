import {WebServer} from './WebServer'
import { ServiceDictionary } from './Classes/ServiceDictionary';

const server :WebServer = new WebServer(new ServiceDictionary());
