"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var express_1 = __importDefault(require("express"));
var WebServer = /** @class */ (function () {
    function WebServer(apiHandler) {
        var _this = this;
        this.rp = require('request-promise-native');
        this.port = 8080;
        this.app = express_1.default();
        this.apiHandler = apiHandler;
        this.app.set('view engine', 'ejs');
        this.app.listen(this.port, function () {
            console.log('server started at http://localhost:' + _this.port);
        });
        this.app.get("/", function (req, response) {
            apiHandler.refreshServiceDictionary();
            response.send("Test" + apiHandler.getCSOService().serviceName);
        });
    }
    return WebServer;
}());
exports.WebServer = WebServer;
