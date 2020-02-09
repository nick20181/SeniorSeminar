"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WebServer_1 = require("./WebServer");
var ServiceDictionary_1 = require("./Classes/ServiceDictionary");
var server = new WebServer_1.WebServer(new ServiceDictionary_1.ServiceDictionary());
