"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceSettings_1 = require("./ServiceSettings");
var NetworkSettings_1 = require("./NetworkSettings");
var DatabaseSettings_1 = require("./DatabaseSettings");
var CasSettings_1 = require("./CasSettings");
var Microservice = /** @class */ (function () {
    function Microservice(timeCreated, id, isDeleted, serviceName, settings, discription, shortname) {
        this.timeCreated = timeCreated;
        this.id = id;
        this.isDeleted = isDeleted;
        this.serviceName = serviceName;
        this.settings = settings;
        this.discription = discription;
        this.shortName = shortname;
    }
    Microservice.NullService = function () {
        return new Microservice(0, "", true, "", new ServiceSettings_1.ServiceSettings(new NetworkSettings_1.NetworkSettings([], []), new DatabaseSettings_1.DatabaseSettings("", "", ""), new CasSettings_1.CasSetting("", "")), "", "");
    };
    return Microservice;
}());
exports.Microservice = Microservice;
