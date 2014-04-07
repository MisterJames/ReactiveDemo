
var productList = angular.module('reactiveApp', []);

productList.controller('ProductCtrl', function ($scope) {
    $scope.products = [
      {
          'name': 'Product 1',
          'itemId': '00000000-0000-0000-1111-000000000000'
      },
      {
          'name': 'Produit le Second',
          'itemId': '00000000-0000-0000-2222-000000000000'
        },
      {
          'name': 'ProdThree™',
          'itemId': '00000000-0000-0000-3333-000000000000'
        }
    ];
});

