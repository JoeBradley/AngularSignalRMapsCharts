﻿'use strict';

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

        var initialize = function () {

            console.log('init hubService');

            // Declare a proxy to reference the hub. Defined in StoreHub.cs
            // need to wrap this in .done( function() , but trouble with reference to this, see for help:
            // http://javascript.crockford.com/private.html
            self.proxy = $.connection.appHub;

            $.connection.hub.start().done(function () { });

            try {
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

            }
            catch (e) { console.error(e.message); }

            console.log('hubService finished loading');

        };

        // Calls TO server        
        var ping = function () {
            self.proxy.invoke('ping');
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
            getRandomJobs: getRandomJobs
        };
    }
]);



