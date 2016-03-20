var UIIdleTimeout = function () {
    return {
        //main function to initiate the module
        init: function () {
            // cache a reference to the countdown element so we don't have to query the DOM for it on each ping.
            var $countdown;
            $('body').append('<div class="modal fade" id="idle-timeout-dialog" data-backdrop="static"><div class="modal-content" style=""><div class="modal-header"><h4 class="modal-title">你的会话即将过期</h4></div><div class="modal-body"><p><i class="fa fa-warning"></i> 你的会话即将在<span id="idle-timeout-counter"></span> 秒锁定.</p><p>你想继续你的会话吗?</p></div><div class="modal-footer"><button id="idle-timeout-dialog-logout" type="button" class="btn btn-default">否,退出</button> <button id="idle-timeout-dialog-keepalive" type="button" class="btn btn-primary" data-dismiss="modal">是,继续</button></div></div></div>');

            // start the idle timer plugin
            $.idleTimeout('#idle-timeout-dialog', '.modal-content button:last', {
                idleAfter: 300, // 5 seconds
                timeout: 30000, //30 seconds to timeout
                //pollingInterval:10, // 5 seconds
                keepAliveURL: '../../Account/Auth/KeepAlive',
                serverResponseEquals: 'ok',
                onTimeout: function () {
                    window.location = "../../Account/Auth/Lock";
                },
                // server-side session keep-alive timer
                // sessionKeepAliveTimer: 10, // Ping the server at this interval in seconds. 600 = 10 Minutes
                // sessionKeepAliveTimer: false, // Set to false to disable pings
                // sessionKeepAliveUrl: '../../Account/Auth/KeepAlive', // set URL to ping - does not apply if sessionKeepAliveTimer: false

                onIdle: function () {
                    $('#idle-timeout-dialog').modal('show');
                    $countdown = $('#idle-timeout-counter');

                    $('#idle-timeout-dialog-keepalive').on('click', function () {
                        $('#idle-timeout-dialog').modal('hide');
                    });

                    $('#idle-timeout-dialog-logout').on('click', function () {
                        $('#idle-timeout-dialog').modal('hide');
                        $.idleTimeout.options.onTimeout.call(this);
                    });
                },
                onCountdown: function (counter) {
                    $countdown.html(counter); // update the counter
                }
            });

        }

    };
}();
jQuery(document).ready(function () {
    UIIdleTimeout.init();
});