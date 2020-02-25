"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WebServer_1 = require("./WebServer");
var ServiceDictionary_1 = require("./Classes/ServiceDictionary");
var API_Handler_1 = require("./API Handler");
var serviceDictionary = new ServiceDictionary_1.ServiceDictionary();
var apiHandler = new API_Handler_1.APIHandler(serviceDictionary);
var server = new WebServer_1.WebServer(apiHandler);
