(function () {
    'use strict';

    var moviesServices = angular.module('moviesServices', ['ngResource']);

    moviesServices.factory('Movies', ['$resource',
      function ($resource) {
          return $resource('/api/Orders/', {}, {
              query: { method: 'GET', params: {}, isArray: true }
          });
      }]);


})();