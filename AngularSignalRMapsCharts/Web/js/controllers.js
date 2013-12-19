'use strict';

/* Controllers */
//Communicating between controllers, see: http://stackoverflow.com/questions/11252780/whats-the-correct-way-to-communicate-between-controllers-in-angularjs/19498009#19498009

var appControllers = angular.module('appControllers', []);

appControllers.controller('JobListCtrl', ['$scope', '$timeout', 'jobService', 'logService', 'hubService',
    function ($scope, $timeout, jobService, logService, hubService) {

        var that = this;

        $scope.orderProp = '-Id';

        $scope.map = null;
        $scope.mapMarkers = null;
        $scope.mapOptions = null;
        $scope.geocoder = null;
        $scope.chart = null;
        $scope.chartOptions = null;
        $scope.chartData = null;
        $scope.ignoreChartClick = false;

        $scope.jobsCount = 0;

        this.loadJob = function (job) {
            that.mapJob(job);
            that.chartJob(job);
        };
        this.mapJob = function (job) {
            console.log('map job');
            var Address = "Australia " + job.PostCode.toString();
            if (Address == '' || Address == null) return;

            $scope.geocoder.geocode({ 'address': Address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    $scope.map.setCenter(results[0].geometry.location);
                    job.marker = new google.maps.Marker({
                        map: $scope.map,
                        title: job.Name,
                        position: results[0].geometry.location
                    });
                    google.maps.event.addListener(job.marker, 'click', function () {
                        $scope.map.setCenter(job.marker.getPosition());
                        $scope.selectJob(job,null,'map');
                    });
                } else {
                    //console.error("Geocode was not successful for the following reason: " + status);
                }
            });

        };
        this.loadChart = function () {
            console.log("load chart");
            try {
                $scope.chartData = new google.visualization.DataTable();
                $scope.chartData.addColumn('date', 'Date');
                $scope.chartData.addColumn('number', 'Customer Satisfaction');
                $scope.chartData.addColumn('number', 'Corporate Responsibility');
                $scope.chartData.addColumn('number', 'Enivormental Standards');
                $scope.chartData.addColumn('number', 'Employee Satisfaction');
                $scope.chartData.addColumn('string', 'title1');
                $scope.chartData.addColumn('string', 'text1');

                $scope.chart = new google.visualization.AnnotatedTimeLine(document.getElementById('Chart'));

                $scope.chartOptions = {
                    allowRedraw: true,
                    colors: ['#0098fc', '#b6ff00', '#ffd800', '#ff6a00'],
                    displayAnnotations: true,
                    displayZoomButtons: true,
                    legendPosition: 'sameRow',
                    thickness: 2,
                    timeline: {}
                };

                $.each($scope.jobs, function (index) {
                    that.chartJob(this, false);
                });

                $scope.chart.draw($scope.chartData, $scope.chartOptions);

                // Add Chart event handlers
                //google.visualization.events.addListener($scope.chart, 'rangechange', function(event) {
                //    console.log('range changed: ' + event.start + ' to ' + event.end);
                //});
                google.visualization.events.addListener($scope.chart, 'select', function () {
                    console.log('marker selected');
                    var markers = $scope.chart.getSelection();
                    if (markers != undefined && markers != null) {
                        var index = markers[0].row;
                        $scope.selectJob(null, index, 'chart');
                    }
                });
                google.visualization.events.addListener($scope.chart, 'ready', function () {
                    console.log('chart ready');
                    $('.chartclient-annotation').on('click', function () {
                        // select the item in the list
                        if (!$scope.ignoreChartClick) {
                            var index = $(this).index();
                            $scope.selectJob($scope.jobs[index], index, 'chart');
                        }
                    });

                });
            }
            catch (ex) { console.error(ex.message); }
        };
        this.chartJob = function (job, refresh) {
            //console.log('chart job');
            if (refresh == undefined || refresh == null) refresh = true;

            $scope.chartData.addRows([[new Date(job.DateCompletedTicks), job.SEO, job.Web, job.Directories, job.Social, job.Name, job.Url]]);

            if (refresh)
                $scope.chart.draw($scope.chartData, $scope.chartOptions);
        };
        this.setChartRange = function (start, end) {
            $scope.chart.setVisibleChartRange(start, end)
        };
        function initializeMap() {
            //Idea for marker clustering> http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerclusterer/
            $scope.geocoder = new google.maps.Geocoder();
            $scope.mapOptions = {
                center: new google.maps.LatLng(-34.397, 150.644),
                zoom: 8
            };
            $scope.map = new google.maps.Map(document.getElementById("Map"), $scope.mapOptions);
        };
        function initializeChart() {
            // see: https://developers.google.com/chart/interactive/docs/gallery/annotatedtimeline?hl=de
            // chart libraries: http://stackoverflow.com/questions/1890434/javascript-library-for-drawing-graphs-over-timelines-zoomable-and-selectable
            try {
                console.log("init chart");
                // When chart library is loaded, craete the chart and load in jobs.
                $timeout(function () { google.load('visualization', '1', { 'callback': that.loadChart, 'packages': ['annotatedtimeline'] }) }, 10, false);
            }
            catch (ex) { console.error(ex.message); }
        };

        initializeMap();

        // Get jobs from web service, load list, add map markers, setup chart 
        $scope.jobs = jobService.list(function (jobs) {
            $scope.jobsCount = jobs.length;
            $.each(jobs, function (index) {
                that.mapJob(this);
            });
            initializeChart();
        });

        // Hub / Client called methods. (SignalR)
        $scope.addJob = function (job) {
            try {
                if ($.inArray(job, $scope.jobs) == -1) {
                    $scope.$apply(function () {
                        $scope.jobs.push(job);
                        $scope.jobsCount++;
                        that.loadJob(job);
                    });
                }
            }
            catch (ex) {
                console.error(ex.message);
            }
        };

        // When a "job" is selected: either from the list, map or chart; select this item in the other views
        $scope.selectJob = function (job, index, ignoreCaller) {
            if (job == null && index != null) {
                job = $scope.jobs[index];
            }
            else if (job != null && index == null) {
                index = $scope.jobs.indexOf(job);
            }
            else if (job == null && index == null) {
                console.log('Error: SelectJob');
            }

            console.log('select job: ' + index + ', ' + ignoreCaller + ', ' + job.Name);

            //clear selection
            $('#List .selected').removeClass('selected');
            $.each($scope.jobs, function () {
                if (this.marker != null && this.marker != undefined) {
                    this.marker.setIcon('http://maps.google.com/mapfiles/ms/icons/red-dot.png');
                    this.marker.setAnimation(google.maps.Animation.NONE);
                }
            });

            //set selection, Scroll to the center
            var $p = $('#List');
            var $c = $('#List > div:eq(' + index + ')').addClass('selected');
            $p.scrollTop($p.scrollTop() + $c.position().top - $p.height() / 2 + $c.height() / 2);

            if (job.marker != null && job.marker != undefined) {
                $scope.map.panTo(job.marker.getPosition());
                job.marker.setIcon('http://maps.google.com/mapfiles/ms/icons/blue-dot.png')
                job.marker.setAnimation(google.maps.Animation.BOUNCE);
                $timeout(function (job) {
                    $.each($scope.jobs, function () {
                        this.marker.setAnimation(google.maps.Animation.NONE);
                    });
                }, 3000, false);
            }

            if (ignoreCaller != 'chart') {
                $scope.ignoreChartClick = true;
                $('.chartclient-annotation-sel').removeClass('chartclient-annotation-sel');
                $('.chartclient-annotation:eq(' + index + ')').addClass('chartclient-annotation-sel');
                $scope.ignoreChartClick = false;
            }

            var d = new Date(parseInt(job.DateCompletedTicks));
            var y = new Date(parseInt(d.setDate(d.getDate() - 1)));
            var t = new Date(parseInt(d.setDate(d.getDate() + 2)));
            that.setChartRange(y, t);

        }

        // Init hub eservice and bind event listeners
        hubService.initialize();

        $scope.getRandomJobs = function () {
            console.log('Get Random Jobs');
            hubService.getRandomJobs();
        };

        // Hub bound events
        $scope.$parent.$on('addJob', function (e, job) {
            console.log('JobListCtrl.on.addJob');
            $scope.addJob(job);
        });

        $scope.$parent.$on('ping', function (e, datetime) {
            console.log('JobListCtrl.on.ping: ' + datetime);
        });

    }]);
