'use strict';

/* Controllers */
//Communicating between controllers, see: http://stackoverflow.com/questions/11252780/whats-the-correct-way-to-communicate-between-controllers-in-angularjs/19498009#19498009

var appControllers = angular.module('appControllers', []);

appControllers.controller('ProductListCtrl', ['$scope', 'productService', 'logService', 'hubService',
    function ($scope, productService, logService, hubService) {
        $scope.products = productService.list();

        $scope.orderProp = 'Name';

        // Client bound events
        $scope.addLike = function (product) {
            product.Likes++;
            productService.update(product);
        };
        $scope.delete = function (product) {
            console.log('ProductListCtrl.delete');
            productService.remove({ Id: product.Id }, function (success) {
                //if (success)
                //    $scope.products.splice($scope.products.indexOf(product), 1);
            });
        };

        // Hub / Client called methods.
        $scope.addProduct = function (product) {
            try {
                if ($.inArray(product, $scope.products) == -1) {
                    $scope.$apply(function () { $scope.products.push(product); });
                }
            }
            catch (ex) {
                console.error(ex.message);
            }
        };
        
        $scope.updateProduct = function (product) {
            $.each($scope.products, function (index) {
                if (this.Id == product.Id) {
                    $scope.$apply(function () { $scope.products[index] = product; });
                }
            });
        };
        
        // removes from local collection, not from server
        $scope.removeProduct = function (id) {
            var indx = -1;
            $.each($scope.products, function (index) { 
                if (this.Id == id)  indx = index;
            });
            if (indx > -1) {
                console.log("remove product");
                $scope.$apply(function () {
                    $scope.products.splice(indx, 1);    
                });
            }
        };

        // Init hub eservice and bind event listeners
        hubService.initialize();

        // Hub bound events
        $scope.$parent.$on('addProduct', function (e, product) {
            console.log('ProductListCtrl.on.addProduct');
            $scope.addProduct(product);
        });
        $scope.$parent.$on('updateProduct', function (e, product) {
            console.log('ProductListCtrl.on.updateProduct');
            $scope.updateProduct(product);
        });
        $scope.$parent.$on('removeProduct', function (e, id) {
            console.log('ProductListCtrl.on.removeProduct');
            $scope.removeProduct(id); 
        });
        $scope.$parent.$on('ping', function (e, datetime) {
                console.log('ProductListCtrl.on.ping');
                console.log('ping: ' + datetime);
            });

    }]);

appControllers.controller('ProductCtrl', ['$scope', '$routeParams', 'productService', '$location',
  function ($scope, $routeParams, productService, $location) {
      $scope.product = productService.select({ Id: $routeParams.Id });

      $scope.setImage = function (imageUrl) {
          $scope.mainImageUrl = imageUrl;
      };

      $scope.cancel = function () {
          $scope.close();
      };

      $scope.save = function () {

          console.log('ProductCtrl.save');

          if ($scope.product.ImageUrl == undefined || $scope.product.ImageUrl == '')
              $scope.product.ImageUrl = flickr.getImage($scope.product.Name, 'Animal');

          if ($scope.product.Id == undefined || $scope.product.Id == 0) {
              productService.add($scope.product, function (success) {

                  $scope.close();
              });
          }
          else {
              productService.update($scope.product, function (success) {
                  $scope.close();
              });
          }
      };

      $scope.isSaveDisabled = function () {
          return $scope.ProductForm.$invalid || angular.equals($scope.product, $scope.form);
      };

      $scope.close = function () {
          $location.path('/products');
      };
  }]);
