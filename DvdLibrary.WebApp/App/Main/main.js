'use strict';

myApp.controller("MainController", ['$scope', '$log', 'movieService', function ($scope, $log, movieService) {
    $scope.query = ""
    var populateListWithMovies = function (query) {
        movieService.GetMovies(query).then(function (response) {
            $scope.movies = response;
        });
    }

    populateListWithMovies("");

    $scope.search = function () {
        if (typeof $scope.query !== 'undefined') {
            populateListWithMovies($scope.query);
        }
    }

}]);