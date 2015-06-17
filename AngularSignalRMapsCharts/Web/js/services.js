'use strict';

/* Services */
//Using SignalR, see: http://dotnet.dzone.com/articles/better-way-using-aspnet

var appServices = angular.module('appServices', ['ngResource']);

appServices.factory('jobService', ['$resource',
    function ($resource) {
        return $resource('/api/v1/companies/:Id', {}, {
            list: { method: 'GET', isArray: true },
            //select: { method: 'GET', params: { Id: '' } },
            //add: { method: 'POST' },
            //update: { method: 'PUT', params: { Id: 'Id' } },
            //remove: { method: 'DELETE', params: { Id: 'Id' } }
        });
    }
]);

// todo: Manage all logs, test if can write to the console, allow different levels of logging, and push logs to server.
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
        var waitTimeout = 1;
        var initialize = function () {

            try {
                console.log('init hubService');

                // Declare a proxy to reference the hub. Defined in StoreHub.cs
                // need to wrap this in .done( function() , but trouble with reference to this, see for help:
                // http://javascript.crockford.com/private.html
                self.proxy = $.connection.appHub;
                self.logProxy = $.connection.logHub;

                // Calls FROM server
                self.proxy.on('addJob', function (json) {
                    //console.log('hubService.on.addJob:' + json);

                    var job = JSON.parse(json);
                    $rootScope.$emit('addJob', job);
                });
                self.proxy.on('ping', function (datetime) {
                    console.log('hubService.on.ping:' + datetime);
                    $rootScope.$emit('ping', datetime);
                });

                // LogHub
                self.logProxy.on('addLog', function (log) {
                    console.log('hubService.on.addLog:' + JSON.stringify(log, null, '\t'));
                    $rootScope.$emit('addLog', log);
                });
                self.logProxy.on('addLogs', function (logs) {
                    //console.log('hubService.on.addLogs:' + JSON.stringify(logs,null, '\t'));
                    $rootScope.$broadcast('addLogs', logs);

                });
            }
            catch (e) { console.error(e.message); }

            $.connection.hub.disconnected(function () {
                console.log('connection.hub.disconnected');
                window.setTimeout(function () {
                    self.waitTimeout++;
                    initialize();
                }, waitTimeout * 1000);
            });

            $.connection.hub.logging = true;
            $.connection.hub.start()
                .done(function () {
                    console.log('hub connection established.');
                })
                .fail(function (error) {
                    console.log('hub connection failed. Error: ' + error);
                });
        };

        // Calls TO server        
        var ping = function () {
            self.proxy.invoke('ping');
        };

        var raiseException = function () {
            self.logProxy.invoke('raiseException');
        };

        var getRandomJobs = function () {
            console.log('hubService.getRandomJobs');
            try {
                self.proxy.invoke('getRandomJobs');
            }
            catch (ex) { console.error(ex.message); }
        };

        return {
            initialize: initialize,
            ping: ping,
            getRandomJobs: getRandomJobs,
            raiseException: raiseException,
        };
    }
]);



