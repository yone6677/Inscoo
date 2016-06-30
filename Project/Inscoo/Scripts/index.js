var inscoo = {};
$(function () {

    //首页轮播
    inscoo.banner = function () {
        $(".banner").each(function () {
            var ban = $(this),
                ban_cont = ban.find('.banner_cont'),
                ban_w = ban_cont.width(),
                ban_ul = ban_cont.find("ul"),
                ban_uli = ban_ul.find("li"),
                len = ban_uli.length,
                ban_p = ban.find(".banner_point");
            // 复制前后两张图片
            ban_ul.append(ban_uli.eq(0).clone());
            ban_ul.prepend(ban_uli.eq(len - 1).clone());

            // 根据图片数量变更容器长度
            ban_ul.css({ 'width': ban_w * (len + 2) + 'px', 'left': -ban_w + 'px' });

            // 动态生成锚点
            for (var i = 0; i < len; i++) {
                ban_p.append("<span></span>");
            }
            var ban_span = ban_p.find("span");
            ban_span.eq(0).addClass("now");

            //缓存当前图片索引
            var n = 1;

            // 图片滚屏
            var bannerMove = function () {
                ban_ul.animate({
                    'left': -ban_w * n + 'px'
                }, 200, function () {
                    if (n == len + 1) {
                        n = 1;
                        ban_ul.css('left', -ban_w * n + 'px');
                    }
                    else if (n == 0) {
                        n = len;
                        ban_ul.css('left', -ban_w * n + 'px');
                    }
                    ban_span.eq(n - 1).addClass("now").siblings().removeClass("now");
                });
            };

            var bannerLeft = function () {
                n++;
                bannerMove();
            }
            var bannerRight = function () {
                n--;
                bannerMove();
            }

            // 点击锚点
            ban_span.on("click", function () {
                var bt = $(this);
                var ind = bt.index();
                n = ind + 1;
                if (ban_ul.is(":animated")) {
                    return false;
                }
                else {
                    clearInterval(timen);
                    bt.addClass("now").siblings().removeClass("now");
                    ban_ul.animate({ 'left': -ban_w * n + 'px' }, 200);
                    timen = setInterval(bannerLeft, 3000);
                }
            });

            // 默认轮播
            var timen = setInterval(bannerLeft, 3000);

            ban.find('.next').on("click", function () {
                if (ban_ul.is(":animated")) {
                    return false;
                }
                else {
                    clearInterval(timen);
                    bannerLeft();
                    timen = setInterval(bannerLeft, 3000);
                }
            })

            ban.find('.prev').on("click", function () {
                if (ban_ul.is(":animated")) {
                    return false;
                }
                else {
                    clearInterval(timen);
                    bannerRight();
                    timen = setInterval(bannerLeft, 3000);
                }
            })

            ban_uli.hover(function () {
                clearInterval(timen);
            }, function () {
                timen = setInterval(bannerLeft, 3000);
            })
        });
    }();

    //切屏
    inscoo.slide = function () {
        var cont = $('.contain').find('.slider'),
            screen_h = cont.height(),
            floor = cont.find('.floor'),
            cur = 0,
            len = floor.length,
            pointsWrp = $('.floor_points'),
            navli = $('.header').find('.nav').find('li');
        floor.eq(cur).addClass('current');
        for (var i = 0; i < len ; i++) {
            pointsWrp.append('<span></span>');
        }
        var height = pointsWrp.height(),
            aFloorPoint = pointsWrp.find('span');
        pointsWrp.css('marginTop', -height / 2);
        aFloorPoint.eq(0).addClass('now');
        aFloorPoint.on('click', function () {
            var _index = $(this).index();
            $(this).addClass('now').siblings().removeClass('now');
            navli.eq(_index).addClass('now').siblings().removeClass('now');
            cur = _index;
            move(cont);
        });
        navli.on('click', function () {
            var _index = $(this).index();
            $(this).addClass('now').siblings().removeClass('now');
            aFloorPoint.eq(_index).addClass('now').siblings().removeClass('now');
            cur = _index;
            move(cont);
        });
        $(document).on("mousewheel DOMMouseScroll", function (e) {
            //console.log(cur);
            if (cont.is(':animated')) {
                return;
            }
            var delta = (e.originalEvent.wheelDelta && (e.originalEvent.wheelDelta > 0 ? 1 : -1)) ||  // chrome & ie
                        (e.originalEvent.detail && (e.originalEvent.detail > 0 ? -1 : 1));              // firefox
            if (delta > 0) {
                // 向上滚
                if (cur == 0) {
                    return;
                }
                cur--;
                navli.eq(cur).addClass('now').siblings().removeClass('now');
                pointsWrp.find('span').eq(cur).addClass('now').siblings().removeClass('now');
                move(cont);
            } else if (delta < 0) {
                // 向下滚
                if (cur == 5) {
                    
                    return;
                }
                cur++;
                navli.eq(cur).addClass('now').siblings().removeClass('now');
                pointsWrp.find('span').eq(cur).addClass('now').siblings().removeClass('now');
                move(cont);
            }
        });
        function move(obj) {
            obj.animate({ 'top': -cur * 100 + '%' }, 500);
        }
    }();


    // 背景
    inscoo.adjustBg = function () {
        adjust($('.floor'));
        $(window).resize(function (event) {
            adjust($('.floor'));
        });

        function adjust(obj) {
            var img_w = 1920,
                img_h = 860,
                img_k = img_h / img_w,
                win_w = $(window).width(),
                win_h = $(window).height(),
                win_k = win_h / win_w;
            if (img_k < win_k) {
                img_h = win_h + 'px';
                img_w = (win_h / img_k) + 'px';
            } else {
                img_w = win_w + 'px';
                img_h = (win_w * img_k) + 'px';
            }
            obj.css({ 'backgroundSize': img_w + ' ' + img_h });
        }
    }();

    //排列
    inscoo.arrange = function () {
        var polygon = $('div.contain').find('div.floor6').find('div.partner');
        for (var i = 1 ; i < polygon.length; i++) {
            if (polygon.eq(i).index() > 4) {
                polygon.eq(i).css({ 'margin': '-40px 6px 6px 6px' });
            }
            if (i % 7 == 5) {
                polygon.eq(i - 1).css({ 'margin': '-40px 6px 6px 98px' });
            }
        }
    }();

});
