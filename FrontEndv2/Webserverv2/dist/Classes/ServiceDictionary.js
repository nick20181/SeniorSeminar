"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Microservice_1 = require("./CustodialAddressingServices/Microservice");
var ServiceSettings_1 = require("./CustodialAddressingServices/ServiceSettings");
var NetworkSettings_1 = require("./CustodialAddressingServices/NetworkSettings");
var DatabaseSettings_1 = require("./CustodialAddressingServices/DatabaseSettings");
var CasSettings_1 = require("./CustodialAddressingServices/CasSettings");
var ServiceDictionary = /** @class */ (function () {
    function ServiceDictionary() {
        this.CustodiualAddressingServicesURI = 'http://localhost:5000/Addressing';
        var CSO = new Microservice_1.Microservice(0, "", true, "", new ServiceSettings_1.ServiceSettings(new NetworkSettings_1.NetworkSettings([""], [""]), new DatabaseSettings_1.DatabaseSettings("", "", ""), new CasSettings_1.CasSetting("", "")), "", "C.S.O");
        this.CustodialServicesOrganizations = CSO;
    }
    ServiceDictionary.prototype.setCSO = function (CSO) {
        this.CustodialServicesOrganizations = CSO;
    };
    ServiceDictionary.prototype.getCSO = function () {
        return this.CustodialServicesOrganizations;
    };
    return ServiceDictionary;
}());
exports.ServiceDictionary = ServiceDictionary;
