'use strict';

/* Services */
//Using SignalR, see: http://dotnet.dzone.com/articles/better-way-using-aspnet

var appServices = angular.module('appServices', ['ngResource']);

appServices.factory('logService', [
    function () {

        var log = function (msg) {
            console.log(msg);
        };

        return {
            log: log
        };
    }
]);

appServices.factory('hubService', ['$rootScope',
    function ($rootScope) {
        var proxy = null;
        var self = this;

        var initialize = function () {

            console.log('init hubService');

            // Declare a proxy to reference the hub. Defined in StoreHub.cs
            // need to wrap this in .done( function() , but trouble with reference to this, see for help:
            // http://javascript.crockford.com/private.html
            self.proxy = $.connection.storeHub;
            
            $.connection.hub.start().done(function () { });

            // Calls FROM server
            self.proxy.on('addProduct', function (json) {
                var product = JSON.parse(json);
                //console.log('hubService.on.addProduct:' + JSON.stringify(product));
                $rootScope.$emit('addProduct', product);
            });
            self.proxy.on('updateProduct', function (json) {
                var product = JSON.parse(json);
                //console.log('hubService.on.updateProduct:' + JSON.stringify(product));
                $rootScope.$emit('updateProduct', product);
            });
            self.proxy.on('removeProduct', function (id) {
                //console.log('hubService.on.removeProduct:' + id);
                $rootScope.$emit('removeProduct', id);
            });
            self.proxy.on('lock', function (id) {
                console.log('hubService.on.lock:' + id);
                $rootScope.$emit('lock', id);
            });
            self.proxy.on('unlock', function (id) {
                console.log('hubService.on.unlock:' + id);
                $rootScope.$emit('unlock', id);
            });
            self.proxy.on('ping', function (datetime) {
                console.log('hubService.on.ping:' + datetime);
                $rootScope.$emit('ping', datetime);
            });
        };

        // Calls TO server
        // lock item for editing
        var lock = function (id) {
            self.proxy.invoke('lock', id);
        };
        // lock item for editing
        var unlock = function (id) {
            self.proxy.invoke('unlock', id);
        };

        var ping = function () {
            self.proxy.invoke('ping');
        };

        return {
            initialize: initialize,
            ping: ping,
            lock: lock
        };
    }
]);

appServices.factory('productService', ['$resource',
    function ($resource) {
        return $resource('api/v1/products/:Id', {}, {
            list: { method: 'GET', isArray: true },
            select: { method: 'GET', params: { Id: '' } },
            add: { method: 'POST' },
            update: { method: 'PUT', params: { Id: 'Id' } },
            remove: { method: 'DELETE', params: { Id: 'Id' } }
        });
    }
]);

