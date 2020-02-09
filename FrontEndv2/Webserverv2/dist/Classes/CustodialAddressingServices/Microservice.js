"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
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
    return Microservice;
}());
exports.Microservice = Microservice;
