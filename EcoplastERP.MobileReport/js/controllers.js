angular.module('ecoapp.controllers', [])

.controller('AppCtrl', function ($scope) {
})

.controller('LoginCtrl', function ($scope, $rootScope, $state, $ionicPopup, AuthenticationService, $cookieStore) {
    if ($cookieStore.get('username')) {
        $scope.user = {
            username: $cookieStore.get('username'),
            password: ''
        }
    }
    AuthenticationService.ClearCredentials();
    $scope.login = function (user) {
        AuthenticationService.Login(user.username, user.password, function (response) {
            if (response.message === undefined) {
                AuthenticationService.SetCredentials(user.username, user.password);
                $rootScope.tokenKey = response[0].Oid;
                $rootScope.systemUserName = response[0].UserName;
                $cookieStore.put('tokenKey', response[0].Oid);
                $cookieStore.put('username', user.username);
                $state.go('app.dashboard');
            } else {
                $scope.showAlert();
            }
        });
    };

    $scope.showAlert = function () {
        var alertPopup = $ionicPopup.alert({
            title: 'Uyarı!',
            template: 'Kullanıcı adı veya şifre hatalı',
            buttons: [{
                text: 'OK',
                type: 'button-perfect'
            }]
        });
        alertPopup.then(function (res) {
            console.log('Kullanici adi veya sifre hatali');
        });
    };
})

.controller('DashboardCtrl', function ($scope) {
    $scope.data = {
        labels: ['16 Jan', '16 Feb', '16 Mar',
        '16 Apr', '16 May', '16 Jun', '16 Jul'],
        datasets: [
          {
              label: "A",
              backgroundColor: 'rgba(255, 99, 132, 1)',
              borderColor: 'rgba(255,99,132,1)',
              data: [60, 90, 120, 60, 90, 120, 60]
          },
          {
              label: "B",
              backgroundColor: 'rgba(75, 192, 192, 1)',
              borderColor: 'rgba(75, 192, 192, 1)',
              data: [40, 60, 80, 40, 60, 80, 40]
          },
           {
               label: "C",
               backgroundColor: 'rgba(255, 206, 86, 1)',
               borderColor: 'rgba(255, 206, 86, 1)',
               data: [20, 30, 40, 20, 30, 40, 20]
           }

        ]
    };

    $scope.options = {
        scales: {
            xAxes: [{
                stacked: true
            }],
            yAxes: [{
                stacked: true
            }]
        },
        legend: {
            display: true,
            labels: {
                fontColor: 'rgb(255, 99, 132)'
            }
        },
        title: {
            display: true,
            text: 'Üretim/Fire Analizi'
        }
    };

    $scope.myData = {
        labels: [
          "Red",
          "Blue",
          "Yellow"
        ],
        datasets: [
          {
              data: [300, 50, 100],
              backgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"
              ],
              hoverBackgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"
              ]
          }
        ]
    };

    $scope.myOptions = {
        // Chart.js options go here
        // e.g. Pie Chart Options http://www.chartjs.org/docs/#doughnut-pie-chart-chart-options
        title: {
            display: true,
            text: 'Satış Analizi'
        }
    };

    $scope.myPlugins = [{
        // Chart.js inline plugins go here
        // e.g. http://www.chartjs.org/docs/latest/developers/plugins.html#using-plugins
    }];

    $scope.onChartClick = function (event) {
        console.log(event);
    };
});
