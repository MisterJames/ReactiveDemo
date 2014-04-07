
var productList = angular.module('reactiveApp', []);

productList.controller('ProductCtrl', function ($scope) {
    $scope.products = [
      {
          'name': 'Product 1',
          'itemId': '111037dd-611a-4a49-1111-a3d7266f0a5d'
      },
      {
          'name': 'Produit le Second',
          'itemId': '22243c08-2b9a-4e91-2222-9f05e586a3b1'
      },
      {
          'name': 'ProdThree™',
          'itemId': '33375b38-f1bc-44e4-3333-e5150a7e8d25'
      }
    ];
});

productList.controller('CartCtrl', function ($scope) {

    var url = "/api/cart";
    $.get(url).done(function(data) {
        $scope.cartItems = data;
        $scope.$apply();
    });

});



