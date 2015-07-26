var Index = function () {
    
    var handleActivitiesPlot = function (data) {
        $.plot($("#site_activities"),

                    [{
                        data: data,
                        lines: {
                            fill: 0.2,
                            lineWidth: 0,
                        },
                        color: ['#BAD9F5']
                    }, {
                        data: data,
                        points: {
                            show: true,
                            fill: true,
                            radius: 4,
                            fillColor: "#9ACAE6",
                            lineWidth: 2
                        },
                        color: '#9ACAE6',
                        shadowSize: 1
                    }, {
                        data: data,
                        lines: {
                            show: true,
                            fill: false,
                            lineWidth: 3
                        },
                        color: '#9ACAE6',
                        shadowSize: 0
                    }],

                    {

                        xaxis: {
                            tickLength: 0,
                            tickDecimals: 0,
                            mode: "categories",
                            min: 0,
                            font: {
                                lineHeight: 18,
                                style: "normal",
                                variant: "small-caps",
                                color: "#6F7B8A"
                            }
                        },
                        yaxis: {
                            ticks: 5,
                            tickDecimals: 0,
                            tickColor: "#eee",
                            font: {
                                lineHeight: 14,
                                style: "normal",
                                variant: "small-caps",
                                color: "#6F7B8A"
                            }
                        },
                        grid: {
                            hoverable: true,
                            clickable: true,
                            tickColor: "#eee",
                            borderColor: "#eee",
                            borderWidth: 1
                        }
                    });

    }
    var DaySalePlot = function (data) {
        $.plot($("#site_statistics"),
                    [{
                        data: data,
                        lines: {
                            fill: 0.6,
                            lineWidth: 0
                        },
                        color: ['#f89f9f']
                    }, {
                        data: data,
                        points: {
                            show: true,
                            fill: true,
                            radius: 5,
                            fillColor: "#f89f9f",
                            lineWidth: 3
                        },
                        color: '#fff',
                        shadowSize: 0
                    }],

                    {
                        xaxis: {
                            tickLength: 0,
                            tickDecimals: 0,
                            mode: "categories",
                            min: 0,
                            font: {
                                lineHeight: 14,
                                style: "normal",
                                variant: "small-caps",
                                color: "#6F7B8A"
                            }
                        },
                        yaxis: {
                            ticks: 5,
                            tickDecimals: 0,
                            tickColor: "#eee",
                            font: {
                                lineHeight: 14,
                                style: "normal",
                                variant: "small-caps",
                                color: "#6F7B8A"
                            }
                        },
                        grid: {
                            hoverable: true,
                            clickable: true,
                            tickColor: "#eee",
                            borderColor: "#eee",
                            borderWidth: 1
                        }
                    });
    };
     
    return {
        
        //main function
        init: function () {            
            Metronic.addResizeHandler(function () {
                jQuery('.vmaps').each(function () {
                    var map = jQuery(this);
                    map.width(map.parent().width());
                });

            });
        },

        initJQVMAP: function () {
            if (!jQuery().vectorMap) {
                return;
            }

            var showMap = function (name) {
                jQuery('.vmaps').hide();
                jQuery('#vmap_' + name).show();
            }

            var setMap = function (name) {
                var data = {
                    map: 'world_en',
                    backgroundColor: null,
                    borderColor: '#333333',
                    borderOpacity: 0.5,
                    borderWidth: 1,
                    color: '#c6c6c6',
                    enableZoom: true,
                    hoverColor: '#c9dfaf',
                    hoverOpacity: null,
                    values: sample_data,
                    normalizeFunction: 'linear',
                    scaleColors: ['#b6da93', '#909cae'],
                    selectedColor: '#c9dfaf',
                    selectedRegion: null,
                    showTooltip: true,
                    onLabelShow: function (event, label, code) {

                    },
                    onRegionOver: function (event, code) {
                        if (code == 'ca') {
                            event.preventDefault();
                        }
                    },
                    onRegionClick: function (element, code, region) {
                        var message = 'You clicked "' + region + '" which has the code: ' + code.toUpperCase();
                        alert(message);
                    }
                };

                data.map = name + '_en';
                var map = jQuery('#vmap_' + name);
                if (!map) {
                    return;
                }
                map.width(map.parent().parent().width());
                map.show();
                map.vectorMap(data);
                map.hide();
            }

            setMap("world");
            setMap("usa");
            setMap("europe");
            setMap("russia");
            setMap("germany");
            showMap("world");

            jQuery('#regional_stat_world').click(function () {
                showMap("world");
            });

            jQuery('#regional_stat_usa').click(function () {
                showMap("usa");
            });

            jQuery('#regional_stat_europe').click(function () {
                showMap("europe");
            });
            jQuery('#regional_stat_russia').click(function () {
                showMap("russia");
            });
            jQuery('#regional_stat_germany').click(function () {
                showMap("germany");
            });

            $('#region_statistics_loading').hide();
            $('#region_statistics_content').show();
        },

        initCalendar: function (result) {
            
            if (!jQuery().fullCalendar) {
                return;
            }

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            var h = {};

            if ($('#calendar').width() <= 400) {
                $('#calendar').addClass("mobile");
                h = {
                    left: 'title, prev, next',
                    center: '',
                    right: 'today,month,agendaWeek,agendaDay'
                };
            } else {
                $('#calendar').removeClass("mobile");
                if (Metronic.isRTL()) {
                    h = {
                        right: 'title',
                        center: '',
                        left: 'prev,next,today,month,agendaWeek,agendaDay'
                    };
                } else {
                    h = {
                        left: 'title',
                        center: '',
                        right: 'prev,next,today,month,agendaWeek,agendaDay'
                    };
                }
            }
           
            $('#calendar').fullCalendar('destroy'); // destroy the calendar
            $('#calendar').fullCalendar({ //re-initialize the calendar
                disableDragging: true,
                editable:false,
                lang: 'zh-cn',
                header: h,
                events:result.data.calresult               
            });
        },

        initCharts: function (result) {
            if (!jQuery.plot) {
                return;
            }

            function showChartTooltip(x, y, xValue, yValue) {
                $('<div id="tooltip" class="chart-tooltip">' + yValue + '<\/div>').css({
                    position: 'absolute',
                    display: 'none',
                    top: y - 40,
                    left: x - 40,
                    border: '0px solid #ccc',
                    padding: '2px 6px',
                    'background-color': '#fff'
                }).appendTo("body").fadeIn(200);
            }

            var data = [];
            var totalPoints = 250;

            // random data generator for plot charts

            function getRandomData() {
                if (data.length > 0) data = data.slice(1);
                // do a random walk
                while (data.length < totalPoints) {
                    var prev = data.length > 0 ? data[data.length - 1] : 50;
                    var y = prev + Math.random() * 10 - 5;
                    if (y < 0) y = 0;
                    if (y > 100) y = 100;
                    data.push(y);
                }
                // zip the generated y values with the x values
                var res = [];
                for (var i = 0; i < data.length; ++i) res.push([i, data[i]])
                return res;
            }

            function randValue() {
                return (Math.floor(Math.random() * (1 + 50 - 20))) + 10;
            }

           
            var daysaledata = result.data.daysale.daysaleresult

            if ($('#site_statistics').size() != 0) {
                
                $('#site_statistics_loading').hide();
                $('#site_statistics_content').show();

                var plot_statistics = DaySalePlot(daysaledata)

                var previousPoint = null;
                $("#site_statistics").bind("plothover", function (event, pos, item) {
                    $("#x").text(pos.x.toFixed(2));
                    $("#y").text(pos.y.toFixed(2));
                    if (item) {
                        if (previousPoint != item.dataIndex) {
                            previousPoint = item.dataIndex;

                            $("#tooltip").remove();
                            var x = item.datapoint[0].toFixed(2),
                                y = item.datapoint[1].toFixed(2);

                            showChartTooltip(item.pageX, item.pageY, item.datapoint[0],'GBP '+item.datapoint[1] );
                        }
                    } else {
                        $("#tooltip").remove();
                        previousPoint = null;
                    }
                });
            }


            if ($('#site_activities').size() != 0) {
                //site activities
                var previousPoint2 = null;
                $('#site_activities_loading').hide();
                $('#site_activities_content').show();

                var data1 = result.data.monthsale.monthsaleresult
                var plot_statistics = handleActivitiesPlot(data1);

                $("#site_activities").bind("plothover", function (event, pos, item) {
                    $("#x").text(pos.x.toFixed(2));
                    $("#y").text(pos.y.toFixed(2));
                    if (item) {
                        if (previousPoint2 != item.dataIndex) {
                            previousPoint2 = item.dataIndex;
                            $("#tooltip").remove();
                            var x = item.datapoint[0].toFixed(2),
                                y = item.datapoint[1].toFixed(2);
                            showChartTooltip(item.pageX, item.pageY, item.datapoint[0],'GBP'+item.datapoint[1]);
                        }
                    }
                });

                $('#site_activities').bind("mouseleave", function () {
                    $("#tooltip").remove();
                });
            }

            $('.year').click(function () {
                var year = $(this).find("span:eq(0)").text();
                $(this).parent().parent().find("li").removeClass("active");
                $(this).parent().addClass("active");
                $.ajax({
                    type: "GET",
                    cache: false,
                    beforeSend: function () {
                        $('#site_activities_content').hide();
                        $('#site_activities_loading').show();
                        $('#sale_activities div:eq(0)').find("h3").empty();
                        $('#sale_activities div:eq(3)').find("h3").empty();
                    },
                    url: "/Ims/TransactionDetail/Stats/?year=" + year,
                    success: function (data) {
                        $('#site_activities_loading').hide();
                        $('#site_activities_content').show();
                        var data1 = data.data.monthsale.monthsaleresult;
                        handleActivitiesPlot(data1);
                        $('#sale_activities div:eq(0)').find("h3").text(data.data.monthsale.monthsaleamount);
                        $('#sale_activities div:eq(3)').find("h3").text(data.data.monthsale.monthsaleqty + "个");
                        $('#year-month-salse').text(year);

                    }
                });
            });

            $('#top_seven').click(function () {
                $.ajax({
                    type: "GET",
                    cache: false,
                    beforeSend: function () {
                        $('#site_statistics_content').hide();
                        $('#site_statistics_loading').show();
                    },
                    url: "/Ims/TransactionDetail/Stats",
                    success: function (data) {
                        $('#site_statistics_loading').hide();
                        $('#site_statistics_content').show();
                        var daysaledata = data.data.daysale.daysaleresult;
                        DaySalePlot(daysaledata)
                    }
                });
            });

            $('#this_month').click(function () {
                $.ajax({
                    type: "GET",
                    cache: false,
                    beforeSend: function () {
                        $('#site_statistics_content').hide();
                        $('#site_statistics_loading').show();
                    },
                    url: "/Ims/TransactionDetail/Stats/?daysaleinmonth=5",
                    success: function (data) {
                        $('#site_statistics_loading').hide();
                        $('#site_statistics_content').show();
                        var daysaledata = data.data.daysale.daysaleresult;
                        DaySalePlot(daysaledata)
                    }
                });
            });
        },

        initMiniCharts: function () {
            if (!jQuery().easyPieChart || !jQuery().sparkline) {
                return;
            }
            var cpuArray = new Array();
            var memoryArray = new Array();
            function getStatus() {
                $.ajax({
                    type: "GET",
                    cache: false,
                    beforeSend: function () {
                       
                    },
                    url: "/Ims/TransactionDetail/ServerStats/",
                    success: function (data) {                       
                        if (cpuArray.length == 18) {
                            cpuArray.splice(0, 1);
                            memoryArray.splice(0, 1);
                        } else {
                            cpuArray.push(data.data.cpu);
                            memoryArray.push(data.data.memory);
                        }
                        console.log(cpuArray);
                        console.log(memoryArray);
                        $("#sparkline_bar2").sparkline(cpuArray, {
                            type: 'bar',
                            width: '100',
                            barWidth: 5,
                            height: '55',
                            barColor: '#ffb848',
                            negBarColor: '#e02222'
                        });

                        $("#sparkline_line").sparkline(memoryArray, {
                            type: 'line',
                            width: '100',
                            height: '55',
                            lineColor: '#ffb848'
                        });
                    }
                });
            }
            var iTimer = setInterval(getStatus, 1000); //创建 后面的单位为毫秒
            //window.clearInterval(iTimer);   //停止



            // IE8 Fix: function.bind polyfill
            if (Metronic.isIE8() && !Function.prototype.bind) {
                Function.prototype.bind = function (oThis) {
                    if (typeof this !== "function") {
                        // closest thing possible to the ECMAScript 5 internal IsCallable function
                        throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
                    }

                    var aArgs = Array.prototype.slice.call(arguments, 1),
                        fToBind = this,
                        fNOP = function () {},
                        fBound = function () {
                            return fToBind.apply(this instanceof fNOP && oThis ? this : oThis,
                        aArgs.concat(Array.prototype.slice.call(arguments)));
                    };

                    fNOP.prototype = this.prototype;
                    fBound.prototype = new fNOP();

                    return fBound;
                };
            }

            $('.easy-pie-chart .number.transactions').easyPieChart({
                animate: 1000,
                size: 75,
                lineWidth: 3,
                barColor: Metronic.getBrandColor('yellow')
            });

            $('.easy-pie-chart .number.visits').easyPieChart({
                animate: 1000,
                size: 75,
                lineWidth: 3,
                barColor: Metronic.getBrandColor('green')
            });

            $('.easy-pie-chart .number.bounce').easyPieChart({
                animate: 1000,
                size: 75,
                lineWidth: 3,
                barColor: Metronic.getBrandColor('red')
            });

            $('.easy-pie-chart-reload').click(function () {
                $('.easy-pie-chart .number').each(function () {
                    var newValue = Math.floor(100 * Math.random());
                    $(this).data('easyPieChart').update(newValue);
                    $('span', this).text(newValue);
                });
            });

            $("#sparkline_bar").sparkline([8, 9, 10, 11, 10, 10, 12, 10, 10, 11, 9, 12, 11, 10, 9, 11, 13, 13, 12], {
                type: 'bar',
                width: '100',
                barWidth: 5,
                height: '55',
                barColor: '#35aa47',
                negBarColor: '#e02222'
            });

            //$("#sparkline_bar2").sparkline([9, 11, 12, 13, 12, 13, 10, 14, 13, 11, 11, 12, 11, 11, 10, 12, 11, 10], {
            //    type: 'bar',
            //    width: '100',
            //    barWidth: 5,
            //    height: '55',
            //    barColor: '#ffb848',
            //    negBarColor: '#e02222'
            //});

            //$("#sparkline_line").sparkline([9, 10, 9, 10, 10, 11, 12, 10, 10, 11, 11, 12, 11, 10, 12, 11, 10, 12], {
            //    type: 'line',
            //    width: '100',
            //    height: '55',
            //    lineColor: '#ffb848'
            //});

        },

        initChat: function () {

            var cont = $('#chats');
            var list = $('.chats', cont);
            var form = $('.chat-form', cont);
            var input = $('input', form);
            var btn = $('.btn', form);

            var handleClick = function (e) {
                e.preventDefault();

                var text = input.val();
                if (text.length == 0) {
                    return;
                }

                var time = new Date();
                var time_str = (time.getHours() + ':' + time.getMinutes());
                var tpl = '';
                tpl += '<li class="out">';
                tpl += '<img class="avatar" alt="" src="' + Layout.getLayoutImgPath() + 'avatar1.jpg"/>';
                tpl += '<div class="message">';
                tpl += '<span class="arrow"></span>';
                tpl += '<a href="#" class="name">Bob Nilson</a>&nbsp;';
                tpl += '<span class="datetime">at ' + time_str + '</span>';
                tpl += '<span class="body">';
                tpl += text;
                tpl += '</span>';
                tpl += '</div>';
                tpl += '</li>';

                var msg = list.append(tpl);
                input.val("");

                var getLastPostPos = function () {
                    var height = 0;
                    cont.find("li.out, li.in").each(function () {
                        height = height + $(this).outerHeight();
                    });

                    return height;
                }

                cont.find('.scroller').slimScroll({
                    scrollTo: getLastPostPos()
                });
            }

            $('body').on('click', '.message .name', function (e) {
                e.preventDefault(); // prevent click event

                var name = $(this).text(); // get clicked user's full name
                input.val('@' + name + ':'); // set it into the input field
                Metronic.scrollTo(input); // scroll to input if needed
            });

            btn.click(handleClick);

            input.keypress(function (e) {
                if (e.which == 13) {
                    handleClick(e);
                    return false; //<---- Add this line
                }
            });
        },

        initDashboardDaterange: function () {
            if (!jQuery().daterangepicker) {
                return;
            }

            $('#dashboard-report-range').daterangepicker({
                opens: (Metronic.isRTL() ? 'right' : 'left'),
                 format: 'MM/DD/YYYY',
               // separator: ' to ',
                startDate: moment().subtract('days', 29),
                endDate: moment(),
                minDate: '01/01/2009',
                maxDate: '12/31/2020',
                showDropdowns: false,
                showWeekNumbers: true,
                timePicker: false,
                timePickerIncrement: 1,
                timePicker12Hour: true,
                ranges: {
                    '今天': [moment(), moment()],
                    '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
                    '前七天': [moment().subtract('days', 6), moment()],
                    '前30天': [moment().subtract('days', 29), moment()],
                    '这个月': [moment().startOf('month'), moment().endOf('month')],
                    '前个月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                },
                buttonClasses: ['btn'],
                applyClass: 'blue',
                cancelClass: 'default',
                locale: {
                    applyLabel: '应用',
                    cancelLabel: '取消',
                    fromLabel: '从',
                    toLabel: '到',
                    customRangeLabel: '自定义时间',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    firstDay: 1
                }
                },
                function (start, end) {
                    var date = start.format('YYYY年MM月DD日') + ' - ' + end.format('YYYY年MM月DD日');
                    $('#dashboard-report-range span').html(date);
                    $('#year-month-salse').text(start.format('YYYY年MM月') + ' - ' + end.format('YYYY年MM月'))
                    $.ajax({
                        type: "POST",
                        cache: false,
                        beforeSend: function () {
                           // $('#auditlog_loading').show();
                           // $('#auditlog').hide();

                        },
                        data: { "rangedate": $('#dashboard-report-range span').text() },
                        url: "/Ims/TransactionDetail/Stats",
                        success: function (data) {
                            result = data;
                            Index.initChat();
                            Index.initJQVMAP(); // init index page's custom scripts
                            Index.initCalendar(result); // init index page's custom scripts
                            Index.initCharts(result); // init index page's custom scripts
                            // Index.initMiniCharts();
                            //  Index.initDashboardDaterange();
                            $('#sale_activities div:eq(0)').find("h3").text(result.data.monthsale.monthsaleamount);
                            $('#sale_activities div:eq(3)').find("h3").text(result.data.monthsale.monthsaleqty + "个");
                            $('.red-intense').find("div.number").text(result.data.total.amount)
                            $('.green-haze').find("div.number").text(result.data.total.qty)
                        }

                    });
                    
                }
            );


            $('#dashboard-report-range span').html(moment().subtract('days', 29).format('YYYY年MM月DD日') + '-' + moment().format('YYYY年MM月DD日'));
            $('#dashboard-report-range').show(function () {
                $('#year-month-salse').text(moment().subtract('days', 29).format('YYYY年MM月') + '-' + moment().format('YYYY年MM月'))
                $.ajax({
                    type: "POST",
                    cache: false,
                    beforeSend: function () {
                        $('#auditlog_loading').show();
                        $('#auditlog').hide();

                    },
                    data:{"rangedate":$('#dashboard-report-range span').text()},
                    url: "/Ims/TransactionDetail/Stats",
                    success: function (data) {
                        result = data;                     
                        Index.initChat();
                        Index.initJQVMAP(); // init index page's custom scripts
                        Index.initCalendar(result); // init index page's custom scripts
                        Index.initCharts(result); // init index page's custom scripts
                        //Index.initMiniCharts();
                        $('#sale_activities div:eq(0)').find("h3").text(result.data.monthsale.monthsaleamount);
                        $('#sale_activities div:eq(3)').find("h3").text(result.data.monthsale.monthsaleqty+"个");
                        $('.red-intense').find("div.number").text(result.data.total.amount)
                        $('.green-haze').find("div.number").text(result.data.total.qty)

                        var app = angular.module('app', []);
                        app.controller('AuditLogsCtrl', ['$scope', '$http', function ($scope, $http) {
                            $scope.auditlogs = result.data.logs.auditlogs
                            $('#auditlog_loading').hide();
                            $('#auditlog').show();
                        }]);

                        angular.element($('#auditlog')).ready(function () {
                            angular.bootstrap($('#auditlog'), ['app']);
                        });

                    }

                });
            });
        }

    };

}();