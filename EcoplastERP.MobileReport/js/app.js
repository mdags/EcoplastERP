var app = angular.module('ecoapp', ['ionic', 'ecoapp.controllers', 'ngRoute', 'ngMessages', 'ngCookies', 'tc.chartjs'])

.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider
        .state('login', {
            cache: false,
            url: '/login',
            templateUrl: 'templates/login.html',
            controller: 'LoginCtrl'
        })

      .state('app', {
          url: '/app',
          abstract: true,
          templateUrl: 'templates/menu.html',
          //controller: 'AppCtrl'
      })

    .state('app.dashboard', {
        url: '/dashboard',
        views: {
            'menuContent': {
                templateUrl: 'templates/dashboard.html',
                controller: 'DashboardCtrl'
            }
        }
    })

    $urlRouterProvider.otherwise('/login');
})

.run(function ($rootScope, $location, $cookieStore, $http) {
    $rootScope.globals = $cookieStore.get('globals') || {};
    if ($rootScope.globals.currentUser) {
        $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata;
    }

    $rootScope.$on('$locationChangeStart', function (event, next, current) {
        if ($location.path() !== '/login' && !$rootScope.globals.currentUser) {
            $location.path('/login');
        }
    });
})

.filter("dateFilter", function () {
    return function (item) {
        if (item !== null && item !== undefined) {
            return new Date(parseInt(item.substr(6)));
        }
        return "";
    };
});