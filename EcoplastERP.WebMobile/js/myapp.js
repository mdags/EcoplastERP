var app = angular.module('ecoapp', ['ecoapp.controllers', 'ngRoute', 'ngMessages', 'ngCookies', 'LocalStorageModule', 'ionic', 'chart.js', 'dx', 'angular.filter', 'ion-sticky'])

.config(function ($stateProvider, $urlRouterProvider, $locationProvider, $ionicConfigProvider) {
    $stateProvider
        .state('login', {
            cache: false,
            url: '/login',
            templateUrl: 'templates/login.html',
            controller: 'LoginCtrl'
        })
        .state('tabs', {
            url: '/tab',
            abstract: true,
            templateUrl: 'templates/tabs.html'
        })
        .state('tabs.home', {
            url: '/home',
            views: {
                'home-tab': {
                    templateUrl: 'templates/home.html',
                    controller: 'HomeCtrl'
                }
            }
        })
        .state('tabs.productmenu', {
            url: '/productmenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/productmenu.html'
                }
            }
        })
        .state('tabs.purchasemenu', {
            url: '/purchasemenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/purchasemenu.html'
                }
            }
        })
        .state('tabs.salesmenu', {
            url: '/salesmenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/salesmenu.html'
                }
            }
        })
        .state('tabs.productionmenu', {
            url: '/productionmenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/productionmenu.html'
                }
            }
        })
        .state('tabs.shippingmenu', {
            url: '/shippingmenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/shippingmenu.html',
                    controller: 'ShippingMenuCtrl'
                }
            }
        })
        .state('tabs.ikmenu', {
            url: '/ikmenu',
            views: {
                'home-tab': {
                    templateUrl: 'templates/ikmenu.html'
                }
            }
        })
        .state('tabs.warehouse', {
            url: '/warehouse',
            views: {
                'home-tab': {
                    templateUrl: 'templates/warehouse.html',
                    controller: 'WarehouseCtrl'
                }
            }
        })
        .state('tabs.warehousebykind', {
            url: '/warehousebykind',
            params: { warehouse: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/warehousebykind.html',
                    controller: 'WarehouseByKindCtrl'
                }
            }
        })
        .state('tabs.waitingdemand', {
            url: '/waitingdemand',
            views: {
                'home-tab': {
                    templateUrl: 'templates/waitingdemand.html',
                    controller: 'WaitingDemandCtrl'
                }
            }
        })
        .state('tabs.contactanalysis', {
            url: '/contactanalysis',
            views: {
                'home-tab': {
                    templateUrl: 'templates/contactanalysis.html',
                    controller: 'ContactAnalysisCtrl'
                }
            }
        })
        .state('tabs.productgroup', {
            url: '/productgroup',
            views: {
                'home-tab': {
                    templateUrl: 'templates/productgroup.html',
                    controller: 'ProductGroupCtrl'
                }
            }
        })
        .state('tabs.producttype', {
            url: '/producttype',
            params: { productgroup: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/producttype.html',
                    controller: 'ProductTypeCtrl'
                }
            }
        })
        .state('tabs.productkind', {
            url: '/productkind',
            params: { producttype: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/productkind.html',
                    controller: 'ProductKindCtrl'
                }
            }
        })
        .state('tabs.waitingordermain', {
            url: '/waitingordermain',
            views: {
                'home-tab': {
                    templateUrl: 'templates/waitingordermain.html',
                    controller: 'WaitingOrderMainCtrl'
                }
            }
        })
        .state('tabs.productionmain', {
            url: '/productionmain',
            views: {
                'home-tab': {
                    templateUrl: 'templates/productionmain.html',
                    controller: 'ProductionMainCtrl'
                }
            }
        })
        .state('tabs.productionstation', {
            url: '/productionstation',
            params: { beginDate: null, endDate: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/productionstation.html',
                    controller: 'ProductionStationCtrl'
                }
            }
        })
        .state('tabs.productionmachine', {
            url: '/productionmachine',
            params: { beginDate: null, endDate: null, station: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/productionmachine.html',
                    controller: 'ProductionMachineCtrl'
                }
            }
        })
        .state('tabs.machineloadmain', {
            url: '/machineloadmain',
            views: {
                'home-tab': {
                    templateUrl: 'templates/machineloadmain.html',
                    controller: 'MachineloadMainCtrl'
                }
            }
        })
        .state('tabs.machineload', {
            url: '/machineload',
            params: { machine: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/machineload.html',
                    controller: 'MachineloadCtrl'
                }
            }
        })
        .state('tabs.machinestopactive', {
            url: '/machinestopactive',
            params: { machine: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/machinestopactive.html',
                    controller: 'MachineStopActiveCtrl'
                }
            }
        })
        .state('tabs.machinestopended', {
            url: '/machinestopended',
            params: { machine: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/machinestopended.html',
                    controller: 'MachineStopEndedCtrl'
                }
            }
        })
        .state('tabs.shippingbyproduct', {
            url: '/shippingbyproduct',
            params: { begindate: null, enddate: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/shippingbyproduct.html',
                    controller: 'ShippingByProductCtrl'
                }
            }
        })
        .state('tabs.shippingbycontact', {
            url: '/shippingbycontact',
            params: { begindate: null, enddate: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/shippingbycontact.html',
                    controller: 'ShippingByContactCtrl'
                }
            }
        })
        .state('tabs.shippingbycity', {
            url: '/shippingbycity',
            params: { begindate: null, enddate: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/shippingbycity.html',
                    controller: 'ShippingByCityCtrl'
                }
            }
        })
        .state('tabs.employee', {
            url: '/employee',
            params: { machine: null },
            views: {
                'home-tab': {
                    templateUrl: 'templates/employee.html',
                    controller: 'EmployeeCtrl'
                }
            }
        })
        .state('tabs.about', {
            url: '/about',
            views: {
                'about-tab': {
                    templateUrl: 'templates/about.html'
                }
            }
        })

    $urlRouterProvider.otherwise('/login');
    $ionicConfigProvider.tabs.position('bottom');
    //$locationProvider.html5Mode(true);
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