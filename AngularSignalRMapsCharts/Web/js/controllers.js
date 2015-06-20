'use strict';

/* Controllers */
//Communicating between controllers, see: http://stackoverflow.com/questions/11252780/whats-the-correct-way-to-communicate-between-controllers-in-angularjs/19498009#19498009

var appControllers = angular.module('appControllers', []);

// todo: look into decorators to finish this, add method to hubService to send logs back to server.
appControllers.config(['$provide', function ($provide) {
    $provide.decorator('$log', ['$delegate',  '$injector', 
        function ($delegate, $injector) {
            // Keep track of the original debug method, we'll need it later.
            var _log = $delegate.log;
            var _info = $delegate.info;
            var _debug = $delegate.debug;
            var _warn = $delegate.warn;
            var _error = $delegate.error;
            
            var _type = { log: 'log', info: 'info', debug: 'debug', warn: 'warn', error: 'error' };

            var logToServer = function (args,type) {
                
                var title = args[0];
                var msg = '';
                if (args.length > 1) msg = args[1];

                // call hubService, send logs to server.
                $injector.get('logService').log(title, msg, type);
            };

            $delegate.log = function () {
                var args = [].slice.call(arguments);
                // Send on our enhanced message to the original debug method.
                _log.apply(null, args);

                // call hubService, send logs to server.
                logToServer(args, _type.log);
            };

            $delegate.info = function () {
                var args = [].slice.call(arguments);
                // Send on our enhanced message to the original debug method.
                _info.apply(null, args);

                // call hubService, send logs to server.
                logToServer(args, _type.info);
            };

            $delegate.debug = function () {
                var args = [].slice.call(arguments);
                // Send on our enhanced message to the original debug method.
                _debug.apply(null, args);

                // call hubService, send logs to server.
                logToServer(args, _type.debug);
            };

            $delegate.warn = function () {
                var args = [].slice.call(arguments);
                // Send on our enhanced message to the original debug method.
                _warn.apply(null, args);

                // call hubService, send logs to server.
                logToServer(args, _type.warn);
            };

            $delegate.error = function () {
                var args = [].slice.call(arguments);
                // Send on our enhanced message to the original debug method.
                _error.apply(null, args);

                // call hubService, send logs to server.
                logToServer(args, _type.error);
            };

            return $delegate;
        }]);
    }]);


appControllers.controller('LogCntrl', ['$scope', '$rootScope', '$interval', '$log',
    function ($scope, $rootScope, $interval, $log) {

        var that = this;
        var pipeSize = 100;
        $scope.logs = [];

        // Hub bound events
        $rootScope.$on('addLogs', function (e, logs) {
            console.log("LogCntrl.addLogs");
            $scope.$apply(function () {
                $scope.addLogs(logs);
                $scope.trimPipe();
                $scope.updateTimeago();
            });
        });

        $rootScope.$on('addLog', function (e, log) {
            console.log("LogCntrl.addLog : " + JSON.stringify(log, null, '\t'));

            $scope.$apply(function () {
                $scope.addLog(log);
                $scope.trimPipe();
            });
        });

        $scope.addLogs = function (logs) {
            //console.log("LogCntrl.addLogs: " + JSON.stringify(logs,null, '\t'));
            try {
                $.each(logs, function (index) {
                    $scope.addLog(this, false);
                });
            }
            catch (ex) {
                console.error(ex.message);
            }
        };

        $scope.addLog = function (log) {
            //console.log("log: " + JSON.stringify(log, null, '\t'));
            var found = false;
            try {
                $.each($scope.logs, function (index) {
                    if (this.Id == log.Id) found = true;
                });
                if (!found) {
                    log.Created = new Date(log.UnixTicks);
                    $scope.logs.push(log);
                }
            }
            catch (ex) {
                console.error(ex.message);
            }
        };

        $scope.trimPipe = function () {
            try {
                if ($scope.logs.length > pipeSize) {
                    $scope.logs.splice(0, $scope.logs.length - pipeSize);
                }
                //console.log("LogCntrl.trimPipe: " + JSON.stringify($scope.logs, null, '\t'));                    
            }
            catch (ex) {
                console.error(ex.message);
            }

        };

        // Refresh the log timeago
        $interval(function () {
            //console.log("interval");  
            //$log.debug("debug message");
        }, 3000, false);

        (function initEvents() {
            $('.accordion-container').on("click", ".acc-header", function (e) {
                var show = !$(this).next().hasClass("expanded");
                $('.accordion-container .acc-body').slideUp().removeClass("expanded");
                $('.accordion-container .acc-header').removeClass("expanded");
                if (show) {
                    $(this).addClass("expanded");
                    $(this).next().slideDown().addClass("expanded");
                }
            });
        })();

    }]);

appControllers.controller('JobListCtrl', ['$scope', '$timeout', '$log', 'logService', 'hubService', 
    function ($scope, $timeout, $log, logService, hubService) {

        var that = this;

        $scope.orderProp = '-Id';

        $scope.chartData = null;
        $scope.timelineChart = null;
        $scope.timelineChartOptions = null;
        $scope.columnChart = null;
        $scope.columnChartWrapper = null;
        $scope.columnChartOptions = null;
        $scope.columnChartCntrl = null;

        $scope.ignoreChartClick = false;

        $scope.jobsCount = 0;

        // TODO: make private functions, i.e.: var loadJob = function(job){...} 

        this.loadJob = function (job) {
            that.loadJobChartData(job);
        };
        this.loadCharts = function () {
            that.loadChartData();
            //that.loadTimelineChart();
            that.loadColumnChart();
        }
        this.loadChartData = function () {
            // init Chart Data, called after first list of jobs received
            //$scope.chartData.clear();

            $scope.chartData = new google.visualization.DataTable();
            $scope.chartData.addColumn('datetime', 'Date');
            $scope.chartData.addColumn('number', 'Customer Satisfaction');
            $scope.chartData.addColumn('number', 'Corporate Responsibility');
            $scope.chartData.addColumn('number', 'Enivormental Standards');
            $scope.chartData.addColumn('number', 'Employee Satisfaction');
            $scope.chartData.addColumn('string', 'Name');
            $scope.chartData.addColumn('string', 'Url');

            $.each($scope.jobs, function (index) {
                that.loadJobChartData(this, false);
            });
        }
        this.loadTimelineChart = function () {
            console.log("load chart");
            try {

                $scope.timelineChart = new google.visualization.AnnotatedTimeLine(document.getElementById('Chart'));

                $scope.timelineChartOptions = {
                    allowRedraw: true,
                    colors: ['#0098fc', '#b6ff00', '#ffd800', '#ff6a00'],
                    displayAnnotations: true,
                    displayZoomButtons: true,
                    legendPosition: 'sameRow',
                    thickness: 2,
                    timeline: {}
                };

                $scope.timelineChart.draw($scope.chartData, $scope.timelineChartOptions);

                // Add Chart event handlers
                //google.visualization.events.addListener($scope.chart, 'rangechange', function(event) {
                //    console.log('range changed: ' + event.start + ' to ' + event.end);
                //});
                google.visualization.events.addListener($scope.timelineChart, 'select', function () {
                    console.log('marker selected');
                    var markers = $scope.timelineChart.getSelection();
                    if (markers != undefined && markers != null) {
                        var index = markers[0].row;
                        $scope.selectJob(null, index, 'chart');
                    }
                });
                google.visualization.events.addListener($scope.timelineChart, 'ready', function () {
                    console.log('chart ready');
                    $('#Chart .chartclient-annotation').on('click', function () {
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
        this.loadColumnChart = function () {
            console.log("load column chart");
            try {
                $scope.columnChart = new google.visualization.Dashboard(document.getElementById('ColumnChartDashboard'));
                $scope.columnChartCntrl = new google.visualization.ControlWrapper({
                    'controlType': 'ChartRangeFilter',
                    'containerId': 'ColumnChartCntrl',
                    'options': {
                        // Filter by the date axis.
                        'filterColumnIndex': 0,
                        'ui': {
                            'chartType': 'LineChart',
                            'chartOptions': {
                                'chartArea': { 'width': '90%' },
                                'hAxis': { 'baselineColor': 'none' }
                            },
                            // Display a single series that shows the closing value of the stock.
                            // Thus, this view has two columns: the date (axis) and the stock value (line series).
                            'chartView': {
                                'columns': [0,
                                    {
                                        'calc': function (dataTable, rowIndex) {
                                            var sum = dataTable.getValue(rowIndex, 1) + dataTable.getValue(rowIndex, 2) + dataTable.getValue(rowIndex, 3) + dataTable.getValue(rowIndex, 4);
                                            return sum / 4;
                                        },
                                        'type': 'number'
                                    }
                                ]
                            },
                            // 1 day in milliseconds = 24 * 60 * 60 * 1000 = 86,400,000 ;  36000000  = 1 hour
                            'minRangeSize': 5000
                        }
                    },
                    // Initial range: 2012-02-09 to 2012-03-20.
                    'state': { 'range': { 'start': new Date(2000, 1, 1), 'end': new Date(2020, 1, 1) } }
                });


                $scope.columnChartWrapper = new google.visualization.ChartWrapper({
                    'chartType': 'ColumnChart',
                    'containerId': 'ColumnChart',
                    'options': {
                        // Use the same chart area width as the control for axis alignment.
                        'isStacked': true,
                        'chartArea': { 'height': '80%', 'width': '90%' },
                        'hAxis': { 'slantedText': false },
                        'vAxis': { 'viewWindow': { 'min': 0 } },
                        'legend': { 'position': 'none' }
                    },
                    // Convert the first column from 'date' to 'string'.
                    'view': {
                        'columns': [
                          //{
                          //    'calc': function (dataTable, rowIndex) {
                          //        return dataTable.getFormattedValue(rowIndex, 0);
                          //    },
                          //    'type': 'string'
                          //},
                          0, 1, 2, 3, 4
                        ]
                    }
                });

                $scope.columnChart.bind($scope.columnChartCntrl, $scope.columnChartWrapper);
                $scope.columnChart.draw($scope.chartData);

                // Add Chart event handlers
                google.visualization.events.addListener($scope.columnChart, 'ready', function (event) {
                    console.log('Column Chart ready');
                });
                //google.visualization.events.addListener($scope.chart, 'select', function () {
                //    console.log('marker selected');
                //    var markers = $scope.timelineChart.getSelection();
                //    if (markers != undefined && markers != null) {
                //        var index = markers[0].row;
                //        $scope.selectJob(null, index, 'chart');
                //    }
                //});
                //google.visualization.events.addListener($scope.chart, 'ready', function () {
                //    console.log('chart ready');
                //    $('.chartclient-annotation').on('click', function () {
                //        // select the item in the list
                //        if (!$scope.ignoreChartClick) {
                //            var index = $(this).index();
                //            $scope.selectJob($scope.jobs[index], index, 'chart');
                //        }
                //    });

                //});
            }
            catch (ex) { console.error(ex.message); }
        };

        this.loadJobChartData = function (job, refresh) {
            //console.log('chart job');
            if (refresh == undefined || refresh == null) refresh = true;

            $scope.chartData.addRows([[new Date(job.DateCompletedTicks), job.SEO, job.Web, job.Directories, job.Social, job.Name, job.Url]]);

            if (refresh) that.refreshCharts();

        };
        this.refreshCharts = function () {
            //$scope.timelineChart.draw($scope.chartData, $scope.timelineChartOptions);
            $scope.columnChart.draw($scope.chartData);
        };
        this.setChartRange = function (start, end) {
            $scope.timelineChart.setVisibleChartRange(start, end)
        };

        function initializeChart() {
            // see: https://developers.google.com/chart/interactive/docs/gallery/annotatedtimeline?hl=de
            // chart libraries: http://stackoverflow.com/questions/1890434/javascript-library-for-drawing-graphs-over-timelines-zoomable-and-selectable
            try {
                console.log("init chart");
                // When chart library is loaded, craete the chart and load in jobs.
                $timeout(function () { google.load('visualization', '1', { 'callback': that.loadCharts, 'packages': ['annotatedtimeline', 'corechart', 'controls'] }) }, 10, false);
            }
            catch (ex) { console.error(ex.message); }
        };

        // Init hub eservice and bind event listeners
        hubService.initialize();

        $scope.getRandomJobs = function () {
            console.log('Get Random Jobs');
            hubService.getRandomJobs();
        };

        $scope.raiseException = function () {
            console.log('Raise server exception');
            //hubService.raiseException();
            $log.log('<log test>', '<test msg>');
            $log.info('<info test>', '<test msg>');
            $log.debug('<debug test>', '<test msg>');
            $log.warn('<warn test>', '<test msg>');
            $log.error('<error test>', '<test msg>');

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
