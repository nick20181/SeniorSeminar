"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var express_1 = __importDefault(require("express"));
var ServiceDictionary_1 = require("./Classes/ServiceDictionary");
var WebServer = /** @class */ (function () {
    function WebServer(serviceList) {
        var _this = this;
        this.rp = require('request-promise-native');
        this.port = 8080;
        this.app = express_1.default();
        this.custodialAddressingServiceUri = 'http://localhost:5000/Addressing';
        this.serviceList = new ServiceDictionary_1.ServiceDictionary();
        this.app.set('view engine', 'ejs');
        this.app.listen(this.port, function () {
            console.log('server started at http://localhost:' + _this.port);
            var options = {
                uri: _this.custodialAddressingServiceUri + "/all",
                method: 'Get',
                headers: {
                    'Content-Type': 'application/json'
                }
            };
            _this.rp(options)
                .then(function (res) {
                var ms = JSON.parse(res);
                ;
                ms.forEach(function (ms) {
                    if (ms.shortName == serviceList.CustodialServicesOrganizations) {
                        serviceList.setCSO(ms);
                    }
                });
            })
                .catch(function (err) {
                console.log('error: ' + err);
            });
        });
        this.serviceList = serviceList;
        this.app.get("/", function (req, response) {
            var ul = document.getElementById("OrganizationList");
            var li = document.createElement("li");
            li.setAttribute('id', serviceList.CustodialServicesOrganizations.id);
        });
    }
    return WebServer;
}());
exports.WebServer = WebServer;
