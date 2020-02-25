"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DatabaseSettings = /** @class */ (function () {
    function DatabaseSettings(connectionString, database, collection) {
        this.connectionString = connectionString;
        this.database = database;
        this.collection = collection;
    }
    return DatabaseSettings;
}());
exports.DatabaseSettings = DatabaseSettings;
