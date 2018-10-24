/// <reference path="app.js" />
app.controller("SearchFarmerController", ['$scope', function ($scope) {

    $scope.farmers = [];
    $scope.farms = [];

    $scope.selectedfarmer = {};
    
    $scope.hidelist = true;
    $scope.hidedetails = true;

    $scope.searchFarmer = function (searchViewModel) {
        if ($scope.hidedetails == false)
        {
            $scope.hidedetails = true;
        }

        //clear the old list first

        $scope.farmers = [];

        console.log(searchViewModel);
        $.ajax(
            {
                url: APP_URL + 'api/farmers/search',
                async: true,
                type: 'POST',
                data: angular.toJson(searchViewModel),
                datatype: "json",
                contentType: "application/json",
                success: function (data, status, jqXHR) {

                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.farmers = data;
                        });
                    }, 100);
                },
                error: function (x, y, z) {
                    alert(x); 
                }
            }
        );
        $scope.hidelist = false;

    };

    $scope.selectFarmer = function (selfarmer) {
       
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.selectedfarmer = selfarmer;

                var searchfarms = new Object();
                searchfarms.id = selfarmer.id;

                //get the farms
                $.ajax(
                    {
                        url: APP_URL + 'Farmers/GetFarmerFarms',
                        async: true,
                        type: 'POST',
                        data: angular.toJson(searchfarms),
                        datatype: "json",
                        contentType: "application/json",
                        success: function (data, status, jqXHR) {

                            setTimeout(function () {
                                $scope.$apply(function () {
                                    $scope.farms = data;
                                });
                            }, 100);
                            
                        },
                        error: function (x, y, z) {
                            alert(x);
                        }
                    }
                );
                console.log(selfarmer);
            });
        }, 100);
        
        $scope.hidelist = true;
        $scope.hidedetails = false;
    };

}]);

/// <reference path="app.js" />
app.controller("QueueSearchFarmerController", ['$scope', 'queue_name', 'queue_id', function ($scope, queue_name, queue_id) {

    $scope.queue_name = queue_name;
    $scope.queue_id = queue_id;

    $scope.farmers = [];
    $scope.farms = [];

    $scope.selectedfarmer = {};

    $scope.hidelist = true;
    $scope.hidedetails = true;

    $scope.searchFarmer = function (searchViewModel) {
        if ($scope.hidedetails == false) {
            $scope.hidedetails = true;
        }

        console.log(searchViewModel);
        $.ajax(
            {
                url: APP_URL + 'api/farmers/search',
                async: true,
                type: 'POST',
                data: angular.toJson(searchViewModel),
                datatype: "json",
                contentType: "application/json",
                success: function (data, status, jqXHR) {

                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.farmers = data;
                        });
                    }, 100);
                },
                error: function (x, y, z) {
                    alert(x);
                }
            }
        );
        $scope.hidelist = false;

    };

    $scope.selectFarmer = function (selfarmer) {

        setTimeout(function () {
            $scope.$apply(function () {
                $scope.selectedfarmer = selfarmer;

                var searchfarms = new Object();
                searchfarms.id = selfarmer.id;

                //get the farms
                $.ajax(
                    {
                        url: APP_URL + 'Farmers/GetFarmerCropAcreages',
                        async: true,
                        type: 'POST',
                        data: angular.toJson(searchfarms),
                        datatype: "json",
                        contentType: "application/json",
                        success: function (data, status, jqXHR) {

                            setTimeout(function () {
                                $scope.$apply(function () {
                                    $scope.farms = data;
                                });
                            }, 100);

                        },
                        error: function (x, y, z) {
                            alert(x);
                        }
                    }
                );
                console.log(selfarmer);
            });
        }, 100);

        $scope.hidelist = true;
        $scope.hidedetails = false;
    };

    $scope.addToQueue = function (farm) {
        $.ajax(
            {
                url: APP_URL + 'api/CropInsurance/AddToQueue?queue=' + queue_id + '&cropacreage=' + farm,
                async: true,
                type: 'GET',
                success: function (data, status, jqXHR) {

                    window.location = APP_URL + 'cropinsurance/ccequeuedetails?id=' + queue_id;

                },
                error: function (x, y, z) {
                    alert(x);
                }
            }
        );
    };

}]);