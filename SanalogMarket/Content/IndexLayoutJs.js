

function getCSSBreakpoint() {
    return window.getComputedStyle(document.querySelector("body"), ":before").getPropertyValue("content").replace(/\"/g, "")
}

function cssBreakpoint(a) {
    return getCSSBreakpoint() === a && true
}

function debounce(b, d, a) {
    var c, d = d || 100;
    return function () {
        var h = this,
            g = arguments,
            f = function () {
                c = null;
                if (!a) {
                    b.apply(h, g)
                }
            },
            e = a && !c;
        clearTimeout(c);
        c = setTimeout(f, d);
        if (e) {
            b.apply(h, g)
        }
    }
}
$(document).ready(function () {
    function c() {
        $(".navbar .navbar-collapse").affix({
            offset: {
                top: $(".navbar-header").outerHeight(true)
            }
        });
        $(".affix, .affix-top").wrap('<div class="affix-wrapper"></div>').parent().css("min-height", $(".affix, .affix-top").outerHeight(true))
    }

    function b() {
        $(".affix, .affix-top").unwrap()
    }
    if (cssBreakpoint("md")) {
        c();
        var a = true
    } else {
        var a = false
    }
    $(window).resize(debounce(function () {
        if (cssBreakpoint("md") && !a) {
            c();
            a = true
        } else {
            if (cssBreakpoint("xs") && a) {
                b();
                a = false
            }
        }
    }))
});
$(document).ready(function () {
    var h = $(".navbar-header").outerHeight(true),
        e = {
            navbarPadTop: {
                element: ".navbar .navbar-collapse",
                style: "padding-top",
                start: "currentValueFromCSS",
                end: 0,
                distance: 300,
                delay: h
            },
            navbarPadBot: {
                element: ".navbar .navbar-collapse",
                style: "padding-bottom",
                start: "currentValueFromCSS",
                end: 0,
                distance: 300,
                delay: h
            },
            navbarLogoH: {
                element: ".navbar-brand img",
                style: "height",
                start: "currentValueFromCSS",
                end: 20,
                distance: 300,
                delay: h
            }
        },
        d = 0,
        b = false;

    function a() {
        $.each(e, function (i, j) {
            j.start = typeof j.start === "string" ? parseInt($(j.element).css(j.style), 10) : j.start;
            j.maxChange = j.start - j.end;
            j.scrollRatio = j.maxChange / j.distance;
            j.animTriggered = false;
            j.animFinished = false;
            $(j.element).addClass("animate")
        })
    }

    function c() {
        $.each(e, function (i, j) {
            $(j.element).css(j.style, "").removeClass("animate animate-after")
        })
    }

    function g() {
        d = $(document).scrollTop();
        b = false;
        $.each(e, function (i, j) {
            if (d > j.delay) {
                if (!j.animTriggered) {
                    j.animTriggered = true
                }
                j.scrolled = d - j.delay;
                if (j.scrolled <= j.distance) {
                    j.currentChange = j.start - j.scrolled * j.scrollRatio.toFixed(2);
                    $(j.element).css(j.style, j.currentChange + "px");
                    if (j.animFinished) {
                        j.animFinished = false;
                        $(j.element).removeClass("animate-after")
                    }
                } else {
                    if (!j.animFinished) {
                        j.animFinished = true;
                        $(j.element).css(j.style, j.end + "px").addClass("animate-after")
                    }
                }
            } else {
                if (j.animTriggered) {
                    j.animTriggered = false;
                    $(j.element).css(j.style, j.start + "px")
                }
            }
        })
    }
    if (cssBreakpoint("md")) {
        a();
        var f = true
    } else {
        var f = false
    }
    $(window).resize(debounce(function () {
        if (cssBreakpoint("md") && !f) {
            a();
            g();
            f = true
        } else {
            if (cssBreakpoint("xs") && f) {
                c();
                f = false
            }
        }
    }));
    $(window).scroll(function () {
        if (cssBreakpoint("md") && !b) {
            window.requestAnimationFrame(g)
        }
        b = true
    })
});
$(document).ready(function () {
    function b() {
        $(".dropdown, .dropdown-submenu").addClass("hover");
        $(document).on({
            mouseenter: function () {
                $(".open").removeClass("open");
                $(this).addClass("open").find(".dropdown-toggle").removeAttr("data-toggle")
            },
            mouseleave: function () {
                $(this).removeClass("open").find(".dropdown-toggle").attr("data-toggle", "dropdown")
            }
        }, ".dropdown.hover");
        $(document).on({
            mouseleave: function () {
                $(this).removeClass("open")
            }
        }, ".dropdown-submenu.hover.open")
    }

    function c() {
        $(".dropdown, .dropdown-submenu").removeClass("hover")
    }
    $(".dropdown-submenu [data-toggle=dropdown]").click(function (d) {
        $(this).parent().siblings(".open").removeClass("open").find(".open").removeClass("open");
        $(this).parent().toggleClass("open").find(".open").removeClass("open");
        d.preventDefault();
        d.stopPropagation()
    });
    if (cssBreakpoint("md")) {
        b();
        var a = true
    } else {
        var a = false
    }
    $(window).resize(debounce(function () {
        if (cssBreakpoint("md") && !a) {
            b();
            a = true
        } else {
            if (cssBreakpoint("xs") && a) {
                c();
                a = false
            }
        }
    }))
});
$(document).ready(function () {
    $("main").addClass("js-reveal")
});

! function (e, d, f) {
    e.fn.scrollUp = function (a) {
        e.data(f.body, "scrollUp") || (e.data(f.body, "scrollUp", !0), e.fn.scrollUp.init(a))
    }, e.fn.scrollUp.init = function (h) {
        var c = e.fn.scrollUp.settings = e.extend({}, e.fn.scrollUp.defaults, h),
            b = c.scrollTitle ? c.scrollTitle : c.scrollText,
            a = e("<a/>", {
                id: c.scrollName,
                href: "#top"
            }).appendTo("body");
        c.scrollImg || a.html(c.scrollText), a.css({
            display: "none",
            position: "fixed",
            zIndex: c.zIndex
        }), c.activeOverlay && e("<div/>", {
            id: c.scrollName + "-active"
        }).css({
            position: "absolute",
            top: c.scrollDistance + "px",
            width: "100%",
            borderTop: "1px dotted" + c.activeOverlay,
            zIndex: c.zIndex
        }).appendTo("body"), scrollEvent = e(d).scroll(function () {
            switch (scrollDis = "top" === c.scrollFrom ? c.scrollDistance : e(f).height() - e(d).height() - c.scrollDistance, c.animation) {
                case "fade":
                    e(e(d).scrollTop() > scrollDis ? a.fadeIn(c.animationInSpeed) : a.fadeOut(c.animationOutSpeed));
                    break;
                case "slide":
                    e(e(d).scrollTop() > scrollDis ? a.slideDown(c.animationInSpeed) : a.slideUp(c.animationOutSpeed));
                    break;
                default:
                    e(e(d).scrollTop() > scrollDis ? a.show(0) : a.hide(0))
            }
        }), a.click(function (g) {
            g.preventDefault(), e("html, body").animate({
                scrollTop: 0
            }, c.scrollSpeed, c.easingType)
        })
    }, e.fn.scrollUp.defaults = {
        scrollName: "scrollUp",
        scrollDistance: 300,
        scrollFrom: "top",
        scrollSpeed: 300,
        easingType: "linear",
        animation: "fade",
        animationInSpeed: 200,
        animationOutSpeed: 200,
        scrollText: "Scroll to top",
        scrollTitle: !1,
        scrollImg: !1,
        activeOverlay: !1,
        zIndex: 2147483647
    }, e.fn.scrollUp.destroy = function (a) {
        e.removeData(f.body, "scrollUp"), e("#" + e.fn.scrollUp.settings.scrollName).remove(), e("#" + e.fn.scrollUp.settings.scrollName + "-active").remove(), e.fn.jquery.split(".")[1] >= 7 ? e(d).off("scroll", a) : e(d).unbind("scroll", a)
    }, e.scrollUp = e.fn.scrollUp
}(jQuery, window, document);
$(document).ready(function () {
    $.scrollUp({
        scrollName: "scrollUp",
        scrollDistance: 700,
        scrollFrom: "top",
        scrollSpeed: 1000,
        easingType: "easeInOutCubic",
        animation: "fade",
        animationInSpeed: 200,
        animationOutSpeed: 200,
        scrollText: "<i class='icon-up-open-mini'></i>",
        scrollTitle: " ",
        scrollImg: 0,
        activeOverlay: 0,
        zIndex: 1001
    })
});
$(document).ready(function () {
    $("a.scroll-to").click(function () {
        if ($(window).width() > 1024) {
            var a = 45
        } else {
            var a = 0
        }
        if ($(this).attr("data-anchor-offset") !== undefined) {
            var b = $(this).attr("data-anchor-offset")
        } else {
            var b = 0
        }
        $("html, body").animate({
            scrollTop: $($(this).attr("href")).offset().top - a - b + "px"
        }, {
            duration: 1000,
            easing: "easeInOutCubic"
        });
        return false
    })
});
$(document).ready(function () {
    $("body").scrollspy({
        target: ".navbar-collapse",
        offset: 50
    })
});

$(document).ready(function () {
    $('.custom1').owlCarousel({
        items: 1,
        nav: true,
        navText: ["<div style='width:30px;height:30px;border: 2px solid #303030;border-radius: 50px;'><i style='margin-top:6px;margin-right:1px;color:#8a8a8a' class='fa fa-chevron-left'></i></div>", "<div style='width:30px;height:30px;border: 2px solid #303030;border-radius: 50px;color:#8a8a8a;'><i style='margin-top:6px;margin-left:4px' class='fa fa-chevron-right'></i></div>"],
        loop: true,
        autoplay: true,
        autoplayTimeout: 2000,
        autoplayHoverPause: true,
        margin: 30,
        stagePadding: 30,
        smartSpeed: 450,
        dots: false
    });
    $("#market").removeClass();
});