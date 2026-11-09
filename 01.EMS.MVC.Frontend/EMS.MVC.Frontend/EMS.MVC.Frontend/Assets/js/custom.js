var Griya = (function () {
    "use strict";
    /* Search Bar ============ */
    var screenWidth = $(window).width();
    var screenHeight = $(window).height();

    var handleNiceSelect = function () {
        if (jQuery(".default-select").length > 0) {
            jQuery(".default-select").niceSelect();
        }
    };

    var handlePreloader = function () {
        setTimeout(function () {
            jQuery("#preloader").remove();
            $("#main-wrapper").addClass("show");
        }, 800);
    };

    var handleMetisMenu = function () {
        if (jQuery("#menu").length > 0) {
            $("#menu").metisMenu();
        }
        jQuery(".metismenu > .mm-active ").each(function () {
            if (!jQuery(this).children("ul").length > 0) {
                jQuery(this).addClass("active-no-child");
            }
        });
    };

    //var handleAllChecked = function () {
    //    $("#checkAll").on("change", function () {
    //        $("td input, .email-list .custom-checkbox input").prop(
    //            "checked",
    //            $(this).prop("checked")
    //        );
    //    });
    //};

    var handleNavigation = function () {
        $(".nav-control").on("click", function () {
            $("#main-wrapper").toggleClass("menu-toggle");

            $(".hamburger").toggleClass("is-active");
        });
    };

    var handleCurrentActive = function () {
        for (
            var nk = window.location,
            o = $("ul#menu a")
                .filter(function () {
                    return this.href == nk;
                })
                .addClass("mm-active")
                .parent()
                .addClass("mm-active");
            ;

        ) {
            if (!o.is("li")) break;

            o = o.parent().addClass("mm-show").parent().addClass("mm-active");
        }
    };

    var handleMiniSidebar = function () {
        $("ul#menu>li").on("click", function () {
            const sidebarStyle = $("body").attr("data-sidebar-style");
            if (sidebarStyle === "mini") {
                console.log($(this).find("ul"));
                $(this).find("ul").stop();
            }
        });
    };

    var handleMinHeight = function () {
        var win_h = window.outerHeight;
        if (win_h > 0 ? win_h : screen.height) {
            $(".content-body").css("min-height", (win_h - 100) + "px");
        }
    };

    var handleDataAction = function () {
        $('a[data-action="collapse"]').on("click", function (i) {
            i.preventDefault(),
                $(this)
                    .closest(".card")
                    .find('[data-action="collapse"] i')
                    .toggleClass("mdi-arrow-down mdi-arrow-up"),
                $(this).closest(".card").children(".card-body").collapse("toggle");
        });

        $('a[data-action="expand"]').on("click", function (i) {
            i.preventDefault(),
                $(this)
                    .closest(".card")
                    .find('[data-action="expand"] i')
                    .toggleClass("icon-size-actual icon-size-fullscreen"),
                $(this).closest(".card").toggleClass("card-fullscreen");
        });

        $('[data-action="close"]').on("click", function () {
            $(this).closest(".card").removeClass().slideUp("fast");
        });

        $('[data-action="reload"]').on("click", function () {
            var e = $(this);
            e.parents(".card").addClass("card-load"),
                e
                    .parents(".card")
                    .append(
                        '<div class="card-loader"><i class=" ti-reload rotate-refresh"></div>'
                    ),
                setTimeout(function () {
                    e.parents(".card").children(".card-loader").remove(),
                        e.parents(".card").removeClass("card-load");
                }, 2000);
        });
    };

    var handleHeaderHight = function () {
        const headerHight = $(".header").innerHeight();
        $(window).scroll(function () {
            if (
                $("body").attr("data-layout") === "horizontal" &&
                $("body").attr("data-header-position") === "static" &&
                $("body").attr("data-sidebar-position") === "fixed"
            )
                $(this.window).scrollTop() >= headerHight
                    ? $(".deznav").addClass("fixed")
                    : $(".deznav").removeClass("fixed");
        });
    };

    var handleDzScroll = function () {
        jQuery(".dz-scroll").each(function () {
            var scroolWidgetId = jQuery(this).attr("id");
            const ps = new PerfectScrollbar("#" + scroolWidgetId, {
                wheelSpeed: 2,
                wheelPropagation: true,
                minScrollbarLength: 20,
            });
            ps.isRtl = false;
        });
    };

    var handleMenuTabs = function () {
        if (screenWidth <= 991) {
            jQuery(".menu-tabs .nav-link").on("click", function () {
                if (jQuery(this).hasClass("open")) {
                    jQuery(this).removeClass("open");
                    jQuery(".fixed-content-box").removeClass("active");
                    jQuery(".hamburger").show();
                } else {
                    jQuery(".menu-tabs .nav-link").removeClass("open");
                    jQuery(this).addClass("open");
                    jQuery(".fixed-content-box").addClass("active");
                    jQuery(".hamburger").hide();
                }
                //jQuery('.fixed-content-box').toggleClass('active');
            });
            jQuery(".close-fixed-content").on("click", function () {
                jQuery(".fixed-content-box").removeClass("active");
                jQuery(".hamburger").removeClass("is-active");
                jQuery("#main-wrapper").removeClass("menu-toggle");
                jQuery(".hamburger").show();
            });
        }
    };

    var handlePerfectScrollbar = function () {
        if (jQuery(".deznav-scroll").length > 0) {
            //const qs = new PerfectScrollbar('.deznav-scroll');
            const qs = new PerfectScrollbar(".deznav-scroll");

            qs.isRtl = false;
        }
    };

    var handleBtnNumber = function () {
        $(".btn-number").on("click", function (e) {
            e.preventDefault();

            fieldName = $(this).attr("data-field");
            type = $(this).attr("data-type");
            var input = $("input[name='" + fieldName + "']");
            var currentVal = parseInt(input.val());
            if (!isNaN(currentVal)) {
                if (type == "minus") input.val(currentVal - 1);
                else if (type == "plus") input.val(currentVal + 1);
            } else {
                input.val(0);
            }
        });
    };

    var handleDzChatUser = function () {
        jQuery(".dz-chat-user-box .dz-chat-user").on("click", function () {
            jQuery(".dz-chat-user-box").addClass("d-none");
            jQuery(".dz-chat-history-box").removeClass("d-none");
            //$(".chatbox .msg_card_body").height(vHeightArea());
            //$(".chatbox .msg_card_body").css('height',vHeightArea());
        });

        jQuery(".dz-chat-history-back").on("click", function () {
            jQuery(".dz-chat-user-box").removeClass("d-none");
            jQuery(".dz-chat-history-box").addClass("d-none");
        });

        jQuery(".dz-fullscreen").on("click", function () {
            jQuery(".dz-fullscreen").toggleClass("active");
        });

        /* var vHeight = function(){ */

        /* } */
    };

    var handleDzFullScreen = function () {
        jQuery(".dz-fullscreen").on("click", function (e) {
            if (
                document.fullscreenElement ||
                document.webkitFullscreenElement ||
                document.mozFullScreenElement ||
                document.msFullscreenElement
            ) {
                /* Enter fullscreen */
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                } else if (document.msExitFullscreen) {
                    document.msExitFullscreen(); /* IE/Edge */
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen(); /* Firefox */
                } else if (document.webkitExitFullscreen) {
                    document.webkitExitFullscreen(); /* Chrome, Safari & Opera */
                }
            } else {
                /* exit fullscreen */
                if (document.documentElement.requestFullscreen) {
                    document.documentElement.requestFullscreen();
                } else if (document.documentElement.webkitRequestFullscreen) {
                    document.documentElement.webkitRequestFullscreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.msRequestFullscreen) {
                    document.documentElement.msRequestFullscreen();
                }
            }
        });
    };

    //var handleshowPass = function () {
    //    jQuery(".show-pass").on("click", function () {
    //        jQuery(this).toggleClass("active");
    //        if (jQuery("#Password").attr("type") == "password") {
    //            jQuery("#Password").attr("type", "text");
    //        } else if (jQuery("#Password").attr("type") == "text") {
    //            jQuery("#Password").attr("type", "password");
    //        }
    //    });
    //};

    var heartBlast = function () {
        $(".heart").on("click", function () {
            $(this).toggleClass("heart-blast");
        });
    };

    var handleDzLoadMore = function () {
        $(".dz-load-more").on("click", function (e) {
            e.preventDefault(); //STOP default action
            $(this).append(' <i class="fa fa-refresh"></i>');

            var dzLoadMoreUrl = $(this).attr("rel");
            var dzLoadMoreId = $(this).attr("id");

            $.ajax({
                method: "POST",
                url: dzLoadMoreUrl,
                dataType: "html",
                success: function (data) {
                    $("#" + dzLoadMoreId + "Content").append(data);
                    $(".dz-load-more i").remove();
                },
            });
        });
    };

    var handleLightgallery = function () {
        if (jQuery("#lightgallery").length > 0) {
            $("#lightgallery").lightGallery({
                loop: true,
                thumbnail: true,
                exThumbImage: "data-exthumbimage",
            });
        }
    };
    var handleCustomFileInput = function () {
        $(".custom-file-input").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $(this)
                .siblings(".custom-file-label")
                .addClass("selected")
                .html(fileName);
        });
    };

    var vHeight = function () {
        var ch = $(window).height() - 206;
        $(".chatbox .msg_card_body").css("height", ch);
    };

    var handleDatetimepicker = function () {
        if (jQuery("#datetimepicker1").length > 0) {
            $("#datetimepicker1").datetimepicker({
                inline: true,
            });
        }
    };

    var handleCkEditor = function () {
        if (jQuery("#ckeditor").length > 0) {
            ClassicEditor.create(document.querySelector("#ckeditor"), {
                // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
            })
                .then((editor) => {
                    window.editor = editor;
                })
                .catch((err) => {
                    console.error(err.stack);
                });
        }
    };

    var handleMenuPosition = function () {
        if (screenWidth > 1024) {
            $(".metismenu  li")
                .unbind()
                .each(function (e) {
                    if ($("ul", this).length > 0) {
                        var elm = $("ul:first", this).css("display", "block");
                        var off = elm.offset();
                        var l = off.left;
                        var w = elm.width();
                        var elm = $("ul:first", this).removeAttr("style");
                        var docH = $("body").height();
                        var docW = $("body").width();

                        if (jQuery("html").hasClass("rtl")) {
                            var isEntirelyVisible = l + w <= docW;
                        } else {
                            var isEntirelyVisible = l > 0 ? true : false;
                        }

                        if (!isEntirelyVisible) {
                            $(this).find("ul:first").addClass("left");
                        } else {
                            $(this).find("ul:first").removeClass("left");
                        }
                    }
                });
        }
    };
    //var quantityProducts = function () {
    //    var $quantityArrowMinus = $(".quantity-arrow-minus");
    //    var $quantityArrowPlus = $(".quantity-arrow-plus");
    //    var $quantityNum = $(".quantity-num");

    //    $quantityArrowMinus.click(quantityMinus);
    //    $quantityArrowPlus.click(quantityPlus);

    //    function quantityMinus() {
    //        if ($quantityNum.val() > 1) {
    //            $quantityNum.val(+$quantityNum.val() - 1);
    //        }
    //    }

    //    function quantityPlus() {
    //        $quantityNum.val(+$quantityNum.val() + 1);
    //    }
    //};
    /* Function ============ */
    return {
        init: function () {
            handleMetisMenu();
            //handleAllChecked();
            handleNavigation();
            handleCurrentActive();
            handleMiniSidebar();
            handleMinHeight();
            handleDataAction();
            handleHeaderHight();
            handleDzScroll();
            handleMenuTabs();
            handlePerfectScrollbar();
            handleBtnNumber();
            handleDzChatUser();
            handleDzFullScreen();
            //handleshowPass();
            heartBlast();
            handleDzLoadMore();
            handleLightgallery();
            handleCustomFileInput();
            vHeight();
            handleDatetimepicker();
            handleCkEditor();
            //quantityProducts();
        },

        load: function () {
            handlePreloader();
            handleNiceSelect();
        },

        resize: function () {
            vHeight();
        },

        handleMenuPosition: function () {
            handleMenuPosition();
        },
    };
})();

/* Document.ready Start */
jQuery(document).ready(function () {
    $('[data-bs-toggle="popover"]').popover();
    ("use strict");
    Griya.init();
});
/* Document.ready END */

/* Window Load START */
jQuery(window).on("load", function () {
    "use strict";
    Griya.load();
    setTimeout(function () {
        Griya.handleMenuPosition();
    }, 1000);
});
/*  Window Load END */
/* Window Resize START */
jQuery(window).on("resize", function () {
    "use strict";
    Griya.resize();
    setTimeout(function () {
        Griya.handleMenuPosition();
    }, 1000);
});
/*  Window Resize END */

var body = $('body');
var html = $('html');

function dezSettings({ typography, version, layout, navheaderBg, headerBg, sidebarStyle, sidebarBg, sidebarPosition, headerPosition, containerLayout, direction, primary }) {
    this.typography = typography || "inter";
    this.version = version || "light";
    this.layout = layout || "vertical";
    this.sidebarStyle = sidebarStyle || "full";
    this.sidebarPosition = sidebarPosition || "static";
    this.headerPosition = headerPosition || "static";
    this.containerLayout = containerLayout || "wide";

    this.manageTypography();
    this.manageVersion();
    this.manageLayout();
    this.manageSidebarStyle();
    this.manageSidebarPosition();
    this.manageHeaderPosition();
    this.manageContainerLayout();
    this.manageResponsiveSidebar();
}
dezSettings.prototype.manageVersion = function () {
    switch (this.version) {
        case "light":
            body.attr("data-theme-version", "light");
            break;
        default:
            body.attr("data-theme-version", "light");
    }
}
dezSettings.prototype.manageTypography = function () {
    switch (this.typography) {
        case "inter":
            body.attr("data-typography", "inter");
            break;
        default:
            body.attr("data-typography", "roboto");
            break;
    }
}
dezSettings.prototype.manageLayout = function () {
    switch (this.layout) {
        case "vertical":
            body.attr("data-layout", "vertical");
            break;
        default:
            body.attr("data-layout", "vertical");
    }
}
dezSettings.prototype.manageSidebarStyle = function () {


    switch (this.sidebarStyle) {
        case "full":
            body.attr("data-sidebar-style", "full");
            break;
        case "mini":
            body.attr("data-sidebar-style", "mini");
            break;
        case "compact":
            body.attr("data-sidebar-style", "compact");
            break;
        case "modern":
            body.attr("data-sidebar-style", "modern");
            break;
        case "icon-hover":
            body.attr("data-sidebar-style", "icon-hover");
            $('.deznav').hover(function () {
                $('#main-wrapper').addClass('iconhover-toggle');
            }, function () {
                $('#main-wrapper').removeClass('iconhover-toggle');
            });
            /* var currentBodyLayout = currentSidebarStyle = '';
        	
            $('.deznav').hover(function() {
            	
                 if(body.attr('data-sidebar-style') === 'icon-hover') { $('#main-wrapper').addClass('iconhover-toggle'); }
            	
                currentBodyLayout = jQuery('body').attr('data-layout');
                currentSidebarStyle = jQuery('body').attr('data-sidebar-style');
            	
                if(currentBodyLayout === 'vertical' && currentSidebarStyle === 'icon-hover'){ $('.metismenu .mm-active ul').addClass('mm-show'); }
            
            }, function() {
            	
                 if(body.attr('data-sidebar-style') === 'icon-hover') { $('#main-wrapper').removeClass('iconhover-toggle'); }
            	
                currentBodyLayout = jQuery('body').attr('data-layout');
                currentSidebarStyle = jQuery('body').attr('data-sidebar-style');
                
                if(currentBodyLayout === 'vertical' && currentSidebarStyle === 'icon-hover'){ $('.metismenu .mm-active ul').removeClass('mm-show'); }
            }); */
            break;
        case "overlay":
            this.layout === "horizontal" ? body.attr("data-sidebar-style", "full") : body.attr("data-sidebar-style", "overlay");
            break;
        default:
            body.attr("data-sidebar-style", "full");
    }
}
dezSettings.prototype.manageSidebarPosition = function () {
    switch (this.sidebarPosition) {
        case "fixed":
            this.sidebarStyle === "overlay" && this.layout === "vertical" || this.sidebarStyle === "modern" ? body.attr("data-sidebar-position", "static") : body.attr("data-sidebar-position", "fixed");
            break;
        case "static":
            body.attr("data-sidebar-position", "static");
            break;
        default:
            body.attr("data-sidebar-position", "static");
    }
}
dezSettings.prototype.manageHeaderPosition = function () {
    switch (this.headerPosition) {
        case "fixed":
            body.attr("data-header-position", "fixed");
            break;
        case "static":
            body.attr("data-header-position", "static");
            break;
        default:
            body.attr("data-header-position", "static");
    }
}
dezSettings.prototype.manageContainerLayout = function () {
    switch (this.containerLayout) {
        case "boxed":
            if (this.layout === "vertical" && this.sidebarStyle === "full") {
                body.attr("data-sidebar-style", "overlay");
            }
            body.attr("data-container", "boxed");
            break;
        case "wide":
            body.attr("data-container", "wide");
            break;
        case "wide-boxed":
            body.attr("data-container", "wide-boxed");
            break;
        default:
            body.attr("data-container", "wide");
    }
}
dezSettings.prototype.manageResponsiveSidebar = function () {
    const innerWidth = $(window).innerWidth();
    if (innerWidth < 1024) {
        body.attr("data-layout", "vertical");
        body.attr("data-container", "wide");
    }

    if (innerWidth > 767 && innerWidth < 1024) {
        body.attr("data-sidebar-style", "mini");
    }

    if (innerWidth < 768) {
        body.attr("data-sidebar-style", "overlay");
    }
}

$(":input").inputmask();