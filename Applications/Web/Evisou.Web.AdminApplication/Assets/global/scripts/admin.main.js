//选择主题及初始化主题逻辑
/*(function () {
    $(".color-panel .color-mode ul li").click(function () {
        var color = $(this).attr("data-style");
        alert("dasdsad");
        $.cookie('currentTheme', color, { expires: 7, path: '/' });
    });
    var currentTheme = $.cookie('currentTheme');
    if (currentTheme != null && currentTheme) {
        $('#style_color').attr("href", "/assets/admin/css/themes/" + currentTheme + ".css");
    }
})();*/

//新菜单根据Url决定逻辑
(function () {



    var locationHref = window.location.href;
    $(".page-sidebar>ul>li>a").each(function () {

        if (locationHref.indexOf($(this).attr("href")) > 0) {
            
            $(this).parent().addClass("active");
            $(this).append("<span class='selected'></span>");

            $("#navigation .page-title span").html($(this).text());
            $("#navigation .page-title small").html($(this).attr("title") || "");
            $("#navigation .breadcrumb li:eq(1) span").html($(this).text());
            $("#navigation .page-breadcrumb  li:eq(1) i").remove();
            $("#navigation .page-breadcrumb  li:eq(2)").remove();

            document.title = $(this).text() + " - " + document.title;

            $(".page-breadcrumb").find("span").text($(this).text());

            var atitle = $(this).attr("title");
            $(".page-title").find("small").text(atitle);

            return false;
        }
        else {

            var parent = $(this);
            $(this).next("ul").each(function () {
                $("a", $(this)).each(function () {
                    if (locationHref.indexOf($(this).attr("href")) > 0) {
                        
                        $(this).parent().addClass("active");

                        parent.parent().addClass("active");
                        $(".arrow", parent).addClass("open").before("<span class='selected'></span>");

                        $("#navigation .page-title span").html($(this).text());
                        $("#navigation .page-title small").html($(this).attr("title") || "");
                        $("#navigation .page-breadcrumb li:eq(1) span").html(parent.text());
                        $("#navigation .page-breadcrumb li:eq(2) span").html($(this).text());

                        document.title = $(this).text() + " - " + document.title;
                        var navitemtitle = $(this).parent().parent().siblings("a").children("span.title").text();
                        $(".page-breadcrumb").find("span").text(navitemtitle);


                        var submenutitle = $(this).find("span.title").text();
                        $(".page-title").find("span").text(submenutitle);

                        var atitle = $(this).attr("title");
                        $(".page-title").find("small").text(atitle);
                        return false;
                    }
                });
            });
        }
    });


    /*$("li.nav-item").click(function(){
		  if($(this).find('ul').length==0){
			$(this).siblings().removeClass('active');
			//$(this).siblings().removeClass('open');
		 	$(this).addClass('active');
		  };
		});*/

    //$("li.nav-item>ul.sub-menu>li.nav-item>a").click(function () {
    //    $(this).removeAttr("href");
    //});
    //$("li.nav-item>ul.sub-menu>li.nav-item>a").click(function(){

    //	$("li.nav-item>ul.sub-menu>li.nav-item").removeClass('active');
    //	$(this).removeAttr("href");
    //	$(this).parent().addClass('active');
    //	//alert($(this).html());
    //	//$(this).append("<span class='selected'></span>");
    //	$(".page-sidebar>ul>li>a").parent().removeClass('active');
    //	$(this).parent().parent().parent().addClass('active');
    //	var navitemtitle=$(this).parent().parent().siblings("a").children("span.title").text();
    //	$(".page-breadcrumb").find("span").text(navitemtitle);	
    //	$("h3.page-title>span").text($(this).children("span.title").text());
    //	//$(this).children("span.title").text()

    //	});
})();

(function () {
    var isIE8Or9 = false;

    if (window.ActiveXObject) {
        var ua = navigator.userAgent.toLowerCase();
        var ie = ua.match(/msie ([\d.]+)/)[1]
        if (ie == 8.0 || ie == 9.0) {
            isIE8Or9 = true;
        }

        if (ie == 6.0) {
            alert("您的浏览器版本是IE6，在本系统中不能达到良好的视觉效果，建议你升级到IE8及以上！")
        }
    }

    if (!isIE8Or9) {
        //alert("您的浏览器版本不是IE8或IE9，在本系统中不能达到良好的视觉效果，建议你升级到IE8以上！")
    }
})();

$("#checkall").click(function () {
    var ischecked = this.checked;
    $("input:checkbox[name='ids']").each(function () {
        this.checked = ischecked;
    });

    $.uniform.update(':checkbox');
});

$("#delete").click(function () {
    var message = "你确定要删除勾选的记录吗?";
    if ($(this).attr("message"))
        message = $(this).attr("message") + "，" + message;
    if (confirm(message))
        $("#mainForm").submit();
});

$("#menusearch").focus(function () {

    $(".nav-item").each(function () {
        if ($(this).find("ul.sub-menu").length == 0) {
            $(this).removeClass("active");
        } else {
            $(this).addClass("active");
        }
    });
});

//$('#menusearch').bind('input propertychange', function () {
    
//        $("ul.sub-menu>li>a").each(function () {
//            var text = $(this).text();
//            if (text.indexOf($('#menusearch').val()) <= 0) {
//                console.log(text);
//                $(this).parent().hide("fade");
//            }
//        });
    
//});
$('#menusearch').bind('input propertychange', function () {

    $(".page-sidebar>ul>li>ul").each(function () {
        var a = 1; var parent = $(this).parent();
        parent.hide('fade');
        $(this).find("li").each(function () {
            if ($('#menusearch').val() != "") {
                var text = $(this).text();
                if (text.indexOf($('#menusearch').val()) < 0) {
                    $(this).hide();
                } else {
                    $(this).show();
                    a++;
                }
            } else {
                $(this).show();
                $(this).parent().parent().show();
            }
        });
       
        if (a > 1)
        {
            parent.show();
            
        }
    });

});



//$("#menusearch").blur(function () {
//    var locationHref = window.location.href;
//    $(".nav-item").each(function () {
//        $(this).removeClass("active");
//        $(this).show();
//    });
//    $(".page-sidebar>ul>li>a").each(function () {
//        $(this).removeClass("active");
//        if (locationHref.indexOf($(this).attr("href")) > 0) {
//            $(this).parent().addClass("active");
//            $(this).append("<span class='selected'></span>");
//            return false;
//        } else {
//            var parent = $(this);
//            $(this).next("ul").each(function () {
//                $("a", $(this)).each(function () {
//                    if (locationHref.indexOf($(this).attr("href")) > 0) {
//                        console.log(locationHref.indexOf($(this).attr("href")) > 0);
//                        $(this).parent().addClass("active");
//                        parent.parent().addClass("active");
//                        $(".arrow", parent).addClass("open").before("<span class='selected'></span>");
//                        return false;
//                    }
//                });
//            });
//        }
//    });
//});


