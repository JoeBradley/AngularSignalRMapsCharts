'use strict';
var app = {
    version: '1.0.0',
    author: 'Christopher Cassidy',

    //init: function() {
    //    $('.well').colourfy().done(function() { console.log('colourfy done'); });
    //},
}

/* App Module */

var demoApp = angular.module('demoApp', [
  'ngRoute',
  'appControllers',
  'appFilters',
  'appServices'
]);

demoApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/report', {
            templateUrl: 'Web/partials/report.html',
            controller: 'JobListCtrl'
        }).
        otherwise({
            redirectTo: '/report'
        });
  }]);