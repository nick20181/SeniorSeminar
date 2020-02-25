"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceSettings = /** @class */ (function () {
    function ServiceSettings(networkSettings, databaseSettings, casSettings) {
        this.networkSettings = networkSettings;
        this.casSettings = casSettings;
        this.databaseSettings = databaseSettings;
    }
    return ServiceSettings;
}());
exports.ServiceSettings = ServiceSettings;
