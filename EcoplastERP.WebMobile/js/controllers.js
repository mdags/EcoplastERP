angular.module('ecoapp.controllers', [])

.controller('IndexCtrl', function ($scope, $rootScope) {
    $rootScope.hideMenus = true;
})

.controller('LoginCtrl', function ($scope, $rootScope, $state, $ionicPopup, AuthenticationService, $cookies, localStorageService) {
    $rootScope.hideMenus = true;
    if (localStorageService.get('username') && localStorageService.get('password')) {
        $scope.user = {
            username: localStorageService.get('username'),
            password: localStorageService.get('password'),
            rememberLogin: true
        }
    }
    AuthenticationService.ClearCredentials();
    $scope.login = function (user) {
        $scope.error = '';
        AuthenticationService.Login(user.username, user.password, function (response) {
            if (response.message === undefined) {
                AuthenticationService.SetCredentials(user.username, user.password);
                $rootScope.tokenKey = response[0].Oid;
                $rootScope.systemUserName = response[0].UserName;
                $cookies.putObject('tokenKey', response[0].Oid);
                localStorageService.set('username', user.username);
                localStorageService.set('password', user.password);
                $rootScope.hideMenus = false;
                $state.go('tabs.home');
            } else {
                localStorageService.set('username', '');
                localStorageService.set('password', '');
                $rootScope.hideMenus = true;
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

.controller('HomeCtrl', function ($scope, $state) {
    $scope.go = function (page) {
        $state.go('tabs.' + page);
    }
})

.controller('WarehouseCtrl', function ($scope, $state, $ionicLoading, $rootScope, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetWarehouseList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })
    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetWarehouseList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.go = function (warehouseCode) {
        $state.go('tabs.warehousebykind', { warehouse: warehouseCode });
    }
})

.controller('WarehouseByKindCtrl', function ($scope, $stateParams, $ionicLoading, $rootScope, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetStoreReportByWarehouse', {
        params: {
            tokenKey: $rootScope.tokenKey,
            warehouseCode: $stateParams["warehouse"]
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })
    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetStoreReportByWarehouse', {
            params: {
                tokenKey: $rootScope.tokenKey,
                warehouseCode: $stateParams["warehouse"]
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/warehousebykind-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetStoreReportByKind', {
            params: {
                tokenKey: $rootScope.tokenKey,
                warehouseCode: item.Code,
                productKind: item.ProductKindOid
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetStoreReportByKind', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    warehouseCode: item.Code,
                    productKind: item.ProductKindOid
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('WaitingDemandCtrl', function ($scope, $ionicLoading, $rootScope, $ionicActionSheet, $ionicPopup, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetWaitingDemandList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetWaitingDemandList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.confirmDemand = function (item) {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/ConfirmDemand', {
            params: {
                tokenKey: $rootScope.tokenKey,
                oid: item.Oid
            }
        }).success(function (data) {
            $http.get('model/datamodel.asmx/GetWaitingDemandList', {
                params: {
                    tokenKey: $rootScope.tokenKey
                }
            }).success(function (data) {
                $scope.data = data;
                $ionicLoading.hide();
            })

            var alertPopup = $ionicPopup.alert({
                title: 'Mesaj',
                template: 'Talep Onaylandı.',
                buttons: [{
                    text: 'OK',
                    type: 'button-balanced'
                }]
            });
        })
    }

    $scope.denyDemand = function (item) {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/DenyDemand', {
            params: {
                tokenKey: $rootScope.tokenKey,
                oid: item.Oid
            }
        }).success(function (data) {
            $http.get('model/datamodel.asmx/GetWaitingDemandList', {
                params: {
                    tokenKey: $rootScope.tokenKey
                }
            }).success(function (data) {
                $scope.data = data;
                $ionicLoading.hide();
            })

            var alertPopup = $ionicPopup.alert({
                title: 'Mesaj',
                template: 'Talep Reddedildi.',
                buttons: [{
                    text: 'OK',
                    type: 'button-assertive'
                }]
            });
        })
    }

    $scope.show = function () {
        var hideSheet = $ionicActionSheet.show({
            buttons: [
              { text: '<b>Tümünü Onayla</b>' }
            ],
            destructiveText: 'Tümünü Reddet',
            destructiveButtonClicked: function () {
                $ionicLoading.show();
                $http.get('model/datamodel.asmx/DenyAllDemand', {
                    params: {
                        tokenKey: $rootScope.tokenKey
                    }
                }).success(function (data) {
                    $http.get('model/datamodel.asmx/GetWaitingDemandList', {
                        params: {
                            tokenKey: $rootScope.tokenKey
                        }
                    }).success(function (data) {
                        $scope.data = data;
                        $ionicLoading.hide();
                    })

                    var alertPopup = $ionicPopup.alert({
                        title: 'Mesaj',
                        template: 'Tüm Talepler Reddedildi.',
                        buttons: [{
                            text: 'OK',
                            type: 'button-assertive'
                        }]
                    });
                })
                return true;
            },
            titleText: 'İşlemler',
            cancelText: 'Vazgeç',
            cancel: function () {
                // add cancel code..
            },
            buttonClicked: function (index) {
                if (index === 0) {
                    $ionicLoading.show();
                    $http.get('model/datamodel.asmx/ConfirmAllDemand', {
                        params: {
                            tokenKey: $rootScope.tokenKey
                        }
                    }).success(function (data) {
                        $http.get('model/datamodel.asmx/GetWaitingDemandList', {
                            params: {
                                tokenKey: $rootScope.tokenKey
                            }
                        }).success(function (data) {
                            $scope.data = data;
                            $ionicLoading.hide();
                        })

                        var alertPopup = $ionicPopup.alert({
                            title: 'Mesaj',
                            template: 'Tüm Talepler Onaylandı.',
                            buttons: [{
                                text: 'OK',
                                type: 'button-balanced'
                            }]
                        });
                    })
                }
                return true;
            }
        });
    };
})

.controller('ContactAnalysisCtrl', function ($scope, $ionicLoading, $rootScope, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetContactAnalysisList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })
    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetContactAnalysisList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/contactanalysis-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetContactAnalysisDetailList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                contactOid: item.Oid
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetContactAnalysisDetailList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    contactOid: item.Oid
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.numGroupsForTL = function (map, key) {
            var count = 0;
            angular.forEach(map, function (item) {
                if (item.GroupName === key)
                    count += item.TLTotal;
            })
            return count;
        }
        $scope.numGroupsForUSD = function (map, key) {
            var count = 0;
            angular.forEach(map, function (item) {
                if (item.GroupName === key)
                    count += item.USDTotal;
            })
            return count;
        }
        $scope.numGroupsForEUR = function (map, key) {
            var count = 0;
            angular.forEach(map, function (item) {
                if (item.GroupName === key)
                    count += item.EURTotal;
            })
            return count;
        }

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('ProductGroupCtrl', function ($scope, $rootScope, $state, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetProductGroupAnalysisList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetProductGroupAnalysisList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.go = function (productgroupOid) {
        $state.go('tabs.producttype', { productgroup: productgroupOid });
    }
})

.controller('ProductTypeCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetProductTypeAnalysisList', {
        params: {
            tokenKey: $rootScope.tokenKey,
            productGroup: $stateParams["productgroup"]
        }
    }).success(function (data) {
        $scope.data = data;

        $scope.charttotal = 0;
        angular.forEach(data, function (item) {
            $scope.charttotal += item.cQuantity;
        });
        $scope.chartOptions = {
            dataSource: data,
            barWidth: 0.7,
            //title: "Makine Bazında Üretim",
            series: {
                argumentField: "Name",
                valueField: "cQuantity",
                type: "bar",
                name: "",
                color: '#8ed400',
                label: {
                    visible: true,
                    format: {
                        type: "fixedPoint",
                        precision: 0
                    },
                    backgroundColor: "#ff7c7c",
                    customizeText: function (arg) {
                        return "%" + ((arg.value * 100) / parseFloat($scope.charttotal)).toFixed(2);
                    }
                }
            },
            rotated: true
        };

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetProductTypeAnalysisList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                productGroup: $stateParams["productgroup"]
            }
        }).success(function (data) {
            $scope.data = data;

            $scope.charttotal = 0;
            angular.forEach(data, function (item) {
                $scope.charttotal += item.cQuantity;
            });
            $scope.chartOptions = {
                dataSource: data,
                barWidth: 0.7,
                //title: "Makine Bazında Üretim",
                series: {
                    argumentField: "Name",
                    valueField: "cQuantity",
                    type: "bar",
                    name: "",
                    color: '#8ed400',
                    label: {
                        visible: true,
                        format: {
                            type: "fixedPoint",
                            precision: 0
                        },
                        backgroundColor: "#ff7c7c",
                        customizeText: function (arg) {
                            return "%" + ((arg.value * 100) / parseFloat($scope.charttotal)).toFixed(2);
                        }
                    }
                },
                rotated: true
            };
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.go = function (producttypeOid) {
        $state.go('tabs.productkind', { producttype: producttypeOid });
    }

    $ionicModal.fromTemplateUrl('templates/producttype-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetProductTypeAnalysisDetailList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                productType: item.Oid
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetProductTypeAnalysisDetailList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    productType: item.Oid
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('ProductKindCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetProductKindAnalysisList', {
        params: {
            tokenKey: $rootScope.tokenKey,
            productType: $stateParams["producttype"]
        }
    }).success(function (data) {
        $scope.data = data;

        $scope.charttotal = 0;
        angular.forEach(data, function (item) {
            $scope.charttotal += item.cQuantity;
        });
        $scope.chartOptions = {
            dataSource: data.slice(0, 5),
            barWidth: 0.7,
            title: "En çok 5 Değer",
            series: {
                argumentField: "Name",
                valueField: "cQuantity",
                type: "bar",
                name: "",
                color: '#8ed400',
                label: {
                    visible: true,
                    format: {
                        type: "fixedPoint",
                        precision: 0
                    },
                    backgroundColor: "#ff7c7c",
                    customizeText: function (arg) {
                        return "%" + ((arg.value * 100) / parseFloat($scope.charttotal)).toFixed(2);
                    }
                }
            },
            rotated: true
        };

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetProductKindAnalysisList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                productType: $stateParams["producttype"]
            }
        }).success(function (data) {
            $scope.data = data;

            $scope.charttotal = 0;
            angular.forEach(data, function (item) {
                $scope.charttotal += item.cQuantity;
            });
            $scope.chartOptions = {
                dataSource: data,
                barWidth: 0.7,
                //title: "Makine Bazında Üretim",
                series: {
                    argumentField: "Name",
                    valueField: "cQuantity",
                    type: "bar",
                    name: "",
                    color: '#8ed400',
                    label: {
                        visible: true,
                        format: {
                            type: "fixedPoint",
                            precision: 0
                        },
                        backgroundColor: "#ff7c7c",
                        customizeText: function (arg) {
                            return "%" + ((arg.value * 100) / parseFloat($scope.charttotal)).toFixed(2);
                        }
                    }
                },
                rotated: true
            };
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/productkind-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetProductKindAnalysisDetailList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                productKind: item.Oid
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetProductKindAnalysisDetailList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    productKind: item.Oid
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('WaitingOrderMainCtrl', function ($scope, $ionicLoading, $rootScope, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetWaitingOrderContactList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })
    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetWaitingOrderContactList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/waitingordermain-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetWaitingOrderList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                contactOid: item.Oid
            }
        }).success(function (data) {
            $scope.detailList = data;

            var totalSalesOrdercQuantity = 0;
            angular.forEach(data, function (item) {
                totalSalesOrdercQuantity += item.OrderedcQuantity;
            });
            $scope.totalSalesOrdercQuantity = totalSalesOrdercQuantity;

            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetWaitingOrderList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    contactOid: item.Oid
                }
            }).success(function (data) {
                $scope.detailList = data;

                var totalSalesOrdercQuantity = 0;
                angular.forEach(data, function (item) {
                    totalSalesOrdercQuantity += item.OrderedcQuantity;
                });
                $scope.totalSalesOrdercQuantity = totalSalesOrdercQuantity;

                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('ProductionMainCtrl', function ($scope, $state) {
    var d = new Date();
    $scope.production = {
        begindate: new Date(d.getFullYear(), d.getMonth(), d.getDate() - 1),
        enddate: new Date(d.getFullYear(), d.getMonth(), d.getDate())
    };
    $scope.go = function () {
        $state.go('tabs.productionstation', { beginDate: $scope.production.begindate, endDate: $scope.production.enddate });
    }
})

.controller('ProductionStationCtrl', function ($scope, $rootScope, $stateParams, $state, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.beginDate = new Date($stateParams["beginDate"]);
    $scope.endDate = new Date($stateParams["endDate"]);

    $http.get('model/datamodel.asmx/GetStationProductionList', {
        params: {
            tokenKey: $rootScope.tokenKey,
            beginDate: $scope.beginDate,
            endDate: $scope.endDate
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetStationProductionList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.beginDate,
                endDate: $scope.endDate
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.go = function (stationCode) {
        $state.go('tabs.productionmachine', { beginDate: $scope.beginDate, endDate: $scope.endDate, station: stationCode });
    }
})

.controller('ProductionMachineCtrl', function ($scope, $rootScope, $stateParams, $state, $ionicLoading, $ionicModal, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.beginDate = new Date($stateParams["beginDate"]);
    $scope.endDate = new Date($stateParams["endDate"]);
    $scope.station = $stateParams["station"];

    $http.get('model/datamodel.asmx/GetStationMachineList', {
        params: {
            tokenKey: $rootScope.tokenKey,
            beginDate: $scope.beginDate,
            endDate: $scope.endDate,
            station: $scope.station
        }
    }).success(function (data) {
        $scope.data = data;

        $scope.totalProduction = 0;
        angular.forEach(data, function (item) {
            $scope.totalProduction += item.ProductionQuantity;
        });

        $scope.chartOptions = {
            dataSource: data,
            barWidth: 1,
            title: "Makine Bazında Üretim",
            series: {
                argumentField: "MachineCode",
                valueField: "ProductionQuantity",
                type: "bar",
                name:"",
                color: '#8ed400',
                label: {
                    visible: true,
                    format: {
                        type: "fixedPoint",
                        precision: 0
                    },
                    backgroundColor: "#ff7c7c",
                    customizeText: function (arg) {
                        return "%" + ((arg.value * 100) / parseFloat($scope.totalProduction)).toFixed(2);
                    }
                }
            },
            rotated: true
        };

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetStationMachineList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.beginDate,
                endDate: $scope.endDate,
                station: $scope.station
            }
        }).success(function (data) {
            $scope.data = data;

            $scope.chartOptions = {
                dataSource: data,
                barWidth: 0.7,
                title: "Makine Bazında Üretim",
                series: {
                    argumentField: "MachineCode",
                    valueField: "ProductionQuantity",
                    type: "bar",
                    name: "",
                    color: '#8ed400',
                    label: {
                        visible: true,
                        format: {
                            type: "fixedPoint",
                            precision: 0
                        },
                        backgroundColor: "#ff7c7c",
                        customizeText: function (arg) {
                            return "%" + ((arg.value * 100) / parseFloat($scope.totalProduction)).toFixed(2);
                        }
                    }
                },
                rotated: true
            };

            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/productionmachine-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetStationMachineDetailList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.beginDate,
                endDate: $scope.endDate,
                machine: item.MachineCode
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetStationMachineDetailList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    beginDate: $scope.beginDate,
                    endDate: $scope.endDate,
                    machine: item.MachineCode
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('MachineloadMainCtrl', function ($scope, $rootScope, $state, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetMachineLoadMainList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetMachineLoadMainList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.go = function (machineCode) {
        $state.go('tabs.machineload', { machine: machineCode });
    }
})

.controller('MachineloadCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.machine = $stateParams["machine"];

    $http.get('model/datamodel.asmx/GetMachineLoadList', {
        params: {
            tokenKey: $rootScope.tokenKey,
            machine: $scope.machine
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetMachineLoadList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                machine: $scope.machine
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/machineload-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $scope.detail = {
            SequenceNumber: item.SequenceNumber,
            WorkName: item.WorkName,
            ContactName: item.ContactName,
            WorkOrderNumber: item.WorkOrderNumber,
            WorkOrderDate: item.WorkOrderDate,
            WorkName: item.WorkName,
            OrderNumber: item.OrderNumber,
            OrderDate: item.OrderDate,
            LineDeliveryDate: item.LineDeliveryDate,
            Width: item.Width,
            Height: item.Height,
            Thickness: item.Thickness,
            Lenght: item.Lenght,
            OrderedQuantity: item.OrderedQuantity,
            WorkOrderQuantity: item.WorkOrderQuantity,
            StoreQuantity: item.StoreQuantity,
            ProductionQuantity: item.ProductionQuantity,
            RemainingProductionQuantity: item.RemainingProductionQuantity,
            WastageQuantity: item.WastageQuantity,
            SalesOrderQuantity: item.SalesOrderQuantity,
            RemainingTime: item.RemainingTime,
            US: item.US,
            SS: item.SS
        };
        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('MachineStopActiveCtrl', function ($scope, $rootScope, $state, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetActiveMachineStopList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;
        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetActiveMachineStopList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.numGroups = function (map, key) {
        var count = 0;
        angular.forEach(map, function (item) {
            if (item.GroupName === key)
                count++;
        })
        return count;
    }
})

.controller('MachineStopEndedCtrl', function ($scope, $rootScope, $state, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    var d = new Date();
    $scope.stop = {
        begindate: new Date(d.getFullYear(), d.getMonth(), d.getDate() - 1),
        enddate: new Date(d.getFullYear(), d.getMonth(), d.getDate())
    };
    $ionicLoading.hide();

    $scope.report = function () {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/GetEndedMachineStopList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.stop.begindate,
                endDate: $scope.stop.enddate
            }
        }).success(function (data) {
            $scope.data = data;
            $ionicLoading.hide();
        })
    }

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetEndedMachineStopList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.stop.begindate,
                endDate: $scope.stop.enddate
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $scope.numGroups = function (map, key) {
        var count = 0;
        angular.forEach(map, function (item) {
            if (item.GroupName === key)
                count++;
        })
        return count;
    }
})

.controller('EmployeeCtrl', function ($scope, $rootScope, $state, $ionicLoading, $http, $ionicModal) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $http.get('model/datamodel.asmx/GetDepartmentList', {
        params: {
            tokenKey: $rootScope.tokenKey
        }
    }).success(function (data) {
        $scope.data = data;

        $scope.barTotal = 0;
        angular.forEach(data, function (item) {
            $scope.barTotal += item.EmployeeCount;
        });
        $scope.chartOptions = {
            dataSource: data,
            barWidth: 0.7,
            series: {
                argumentField: "Name",
                valueField: "EmployeeCount",
                type: "bar",
                name: " ",
                color: '#8ed400',
                label: {
                    visible: true,
                    format: {
                        type: "fixedPoint",
                        precision: 0
                    },
                    backgroundColor: "#ff7c7c",
                    customizeText: function (arg) {
                        return arg.value + " (" + "%" + ((arg.value * 100) / parseFloat($scope.barTotal)).toFixed(2) + ")";
                    }
                },
                legend: {
                    enabled: false
                },
            },
            rotated: true
        };

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $http.get('model/datamodel.asmx/GetDepartmentList', {
            params: {
                tokenKey: $rootScope.tokenKey
            }
        }).success(function (data) {
            $scope.data = data;
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };

    $ionicModal.fromTemplateUrl('templates/employee-modal.html', {
        scope: $scope,
        animation: 'slide-in-up'
    }).then(function (modal) {
        $scope.modal = modal;
    });
    $scope.openModal = function (item) {
        $ionicLoading.show();

        $http.get('model/datamodel.asmx/GetEmployeeList', {
            params: {
                tokenKey: $rootScope.tokenKey,
                department: item.Name
            }
        }).success(function (data) {
            $scope.detailList = data;
            $ionicLoading.hide();
        })

        $scope.doRefreshDetail = function () {
            $http.get('model/datamodel.asmx/GetEmployeeList', {
                params: {
                    tokenKey: $rootScope.tokenKey,
                    department: item.Name
                }
            }).success(function (data) {
                $scope.detailList = data;
                $ionicLoading.hide();
            })
             .finally(function () {
                 $scope.$broadcast('scroll.refreshComplete');
             });
        };

        $scope.numGroups = function (map, key) {
            var count = 0;
            angular.forEach(map, function (item) {
                if (item.DepartmentPartName === key)
                    count++;
            })
            return count;
        }

        $scope.modal.show();
    };
    $scope.closeModal = function () {
        $scope.modal.hide();
    };
    $scope.$on('$destroy', function () {
        $scope.modal.remove();
    });
    $scope.$on('modal.hidden', function () {
    });
    $scope.$on('modal.removed', function () {
    });
})

.controller('ShippingMenuCtrl', function ($scope, $state) {
    var d = new Date();
    $scope.shipping = {
        begindate: new Date(d.getFullYear(), d.getMonth(), d.getDate() - 1),
        enddate: new Date(d.getFullYear(), d.getMonth(), d.getDate())
    };
    $scope.go = function (path) {
        $state.go('tabs.' + path, { begindate: $scope.shipping.begindate, enddate: $scope.shipping.enddate });
    }
})

.controller('ShippingByProductCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.begindate = new Date($stateParams["begindate"]);
    $scope.enddate = new Date($stateParams["enddate"]);
    $scope.totalQuantity = 0;
    $scope.totalTotal = 0;

    $http.get('model/datamodel.asmx/GetShippingByProductKind', {
        params: {
            tokenKey: $rootScope.tokenKey,
            beginDate: $scope.begindate,
            endDate: $scope.enddate
        }
    }).success(function (data) {
        $scope.data = data;

        var quantityTotal = 0, totalTotal = 0;
        angular.forEach($scope.data, function (item) {
            quantityTotal += item.Quantity;
            totalTotal += item.Total;
        });
        $scope.totalQuantity = quantityTotal;
        $scope.totalTotal = totalTotal;

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/GetShippingByProductKind', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.begindate,
                endDate: $scope.enddate
            }
        }).success(function (data) {
            $scope.data = data;

            var quantityTotal = 0, totalTotal = 0;
            angular.forEach($scope.data, function (item) {
                quantityTotal += item.Quantity;
                totalTotal += item.Total;
            });
            $scope.totalQuantity = quantityTotal;
            $scope.totalTotal = totalTotal;

            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };
})

.controller('ShippingByContactCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.begindate = new Date($stateParams["begindate"]);
    $scope.enddate = new Date($stateParams["enddate"]);
    $scope.totalQuantity = 0;
    $scope.totalTotal = 0;

    $http.get('model/datamodel.asmx/GetShippingByContact', {
        params: {
            tokenKey: $rootScope.tokenKey,
            beginDate: $scope.begindate,
            endDate: $scope.enddate
        }
    }).success(function (data) {
        $scope.data = data;

        var quantityTotal = 0, totalTotal = 0;
        angular.forEach($scope.data, function (item) {
            quantityTotal += item.Quantity;
            totalTotal += item.Total;
        });
        $scope.totalQuantity = quantityTotal;
        $scope.totalTotal = totalTotal;

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/GetShippingByContact', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.begindate,
                endDate: $scope.enddate
            }
        }).success(function (data) {
            $scope.data = data;

            var quantityTotal = 0, totalTotal = 0;
            angular.forEach($scope.data, function (item) {
                quantityTotal += item.Quantity;
                totalTotal += item.Total;
            });
            $scope.totalQuantity = quantityTotal;
            $scope.totalTotal = totalTotal;

            $ionicLoading.hide();
        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };
})

.controller('ShippingByCityCtrl', function ($scope, $rootScope, $state, $stateParams, $ionicLoading, $http) {
    $ionicLoading.show({
        template: '<i class="icon ion-load-c"></i>',
        animation: 'fade-in',
        showBackdrop: true,
        maxWidth: 200,
        showDelay: 0
    });

    $scope.begindate = new Date($stateParams["begindate"]);
    $scope.enddate = new Date($stateParams["enddate"]);
    $scope.totalQuantity = 0;
    $scope.totalTotal = 0;

    $http.get('model/datamodel.asmx/GetShippingByCity', {
        params: {
            tokenKey: $rootScope.tokenKey,
            beginDate: $scope.begindate,
            endDate: $scope.enddate
        }
    }).success(function (data) {
        $scope.data = data;

        var quantityTotal = 0, totalTotal = 0;
        angular.forEach($scope.data, function (item) {
            quantityTotal += item.Quantity;
            totalTotal += item.Total;
        });
        $scope.totalQuantity = quantityTotal;
        $scope.totalTotal = totalTotal;

        $ionicLoading.hide();
    })

    $scope.doRefresh = function () {
        $ionicLoading.show();
        $http.get('model/datamodel.asmx/GetShippingByCity', {
            params: {
                tokenKey: $rootScope.tokenKey,
                beginDate: $scope.begindate,
                endDate: $scope.enddate
            }
        }).success(function (data) {
            $scope.data = data;

            var quantityTotal = 0, totalTotal = 0;
            angular.forEach($scope.data, function (item) {
                quantityTotal += item.Quantity;
                totalTotal += item.Total;
            });
            $scope.totalQuantity = quantityTotal;
            $scope.totalTotal = totalTotal;

            $ionicLoading.hide();

        })
         .finally(function () {
             $scope.$broadcast('scroll.refreshComplete');
         });
    };
});