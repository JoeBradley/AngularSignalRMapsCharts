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
  'appAnimations',
  'appControllers',
  'appFilters',
  'appServices'
]);

demoApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/products', {
            templateUrl: 'Web/partials/product-list.html',
            controller: 'ProductListCtrl'
        }).
        when('/products/:Id', {
            templateUrl: 'Web/partials/product-detail.html',
            controller: 'ProductCtrl'
        }).
        otherwise({
            redirectTo: '/products'
        });
  }]);