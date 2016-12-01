/*
 * jQuery pager plugin
 * Version 1.0 (12/22/2008)
 * @requires jQuery v1.2.6 or later
 *
 * Example at: http://jonpauldavies.github.com/JQuery/Pager/PagerDemo.html
 *
 * Copyright (c) 2008-2009 Jon Paul Davies
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 * Read the related blog post and contact the author at http://www.j-dee.com/2008/12/22/jquery-pager-plugin/
 *
 * This version is far from perfect and doesn't manage it's own state, therefore contributions are more than welcome!
 *
 * Usage: .pager({ pageIndex: 1, pageCount: 15, buttonClickCallback: PagerClickTest });
 *
 * Where pageIndex is the visible page number
 *       pageCount is the total number of pages to display
 *       buttonClickCallback is the method to fire when a pager button is clicked.
 *
 * buttonClickCallback signiture is PagerClickTest = function(pageclickednumber)
 * Where pageclickednumber is the number of the page clicked in the control.
 *
 * The included Pager.CSS file is a dependancy but can obviously tweaked to your wishes
 * Tested in IE6 IE7 Firefox & Safari. Any browser strangeness, please report.
 */

(function ($) {

    $.fn.pager = function (options) {

        if (options.pageCount == undefined) {
            options.pageCount = Math.ceil(options.recordCount / options.pageSize);
        }
        var opts = $.extend({}, $.fn.pager.defaults, options);

        return this.each(function () {
            options.pageSize = options.pageSize || 10;
            options.pageCount = Math.ceil(options.recordCount / options.pageSize);
            // empty out the destination element and then render out the pager with the supplied options
            $(this).empty().append(renderpager(parseInt(options.recordCount), parseInt(options.pageIndex), parseInt(options.pageCount), options.buttonClickCallback));

            // 绑定跳转页面按钮及事件
            // $("#selPageSize option[value=" + options.pageSize + "]").attr("selected", true);
            var $pagerObject = $(this);
            validNum(options.pageCount, $pagerObject);
            $("#txtPageIndex", $pagerObject).val(options.pageIndex);
            $("#txtPageIndex", $pagerObject).keydown(function (event) {
                enter(event, $pagerObject);
            });
            $("#btnGoToPage", $pagerObject).click(function () {
                var pageIndex = $("#txtPageIndex", $pagerObject).val();
                pageIndex = (pageIndex <= 0 ? 1 : pageIndex);
                pageIndex = (pageIndex > options.pageCount ? options.pageCount : pageIndex);
                $("#txtPageIndex", $pagerObject).val(pageIndex);
                if (pageIndex != options.pageIndex) {
                    options.buttonClickCallback(pageIndex);
                }
            });
        });
    };

    $.fn.pager.enter = enter;

    //验证输入框
    function validNum(maxInt, $pagerObject) {
        $("#txtPageIndex", $pagerObject).keypress(function (event) {
            if (!$.browser.mozilla) {
                if (event.keyCode && (event.keyCode < 48 || event.keyCode > 57)) {
                    // ie6,7,8,opera,chrome管用   
                    event.preventDefault();
                }
            } else {
                if (event.charCode && (event.charCode < 48 || event.charCode > 57)) {
                    // firefox管用   
                    event.preventDefault();
                }
            }
        });
        $("#txtPageIndex", $pagerObject).blur(function () {
            var input = $(this);
            var v = parseInt($.trim(input.val()));
            if (v.toString() == "NaN") {
                input.val("0");
            }
            else {
                if (v <= 0) {
                    $(this).val(1);
                }
                else if (v > maxInt) {
                    $(this).val(maxInt);
                }
                else {
                    input.val(v);
                }
            }
        });
    }

    //回车事件
    function enter(event, $pagerObject) {
        if (event.keyCode) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                $("#btnGoToPage", $pagerObject).click();
            }
        } else {
            if (event.charCode == 13) {
                event.returnValue = false;
                $("#btnGoToPage", $pagerObject).click();
            }
        }
    }

    // render and return the pager with the supplied options
    function renderpager(recordCount, pageIndex, pageCount, buttonClickCallback) {
        if (pageCount <= 1) {
            return "";
        }
        //分页容器
        var $pager = $('<ul id="pagination-digg"></ul>');

        //        //总记录数
        //        if (!isNaN(recordCount)) {
        //            $pager.append("总记录数：" + recordCount + "&nbsp;&nbsp;");
        //        }

        //        //每页条数
        //        var selPageSize = '每页<select id="selPageSize">';
        //        selPageSize += '<option value="3">3</option>';
        //        selPageSize += '<option value="5">5</option>';
        //        selPageSize += '<option value="10">10</option>';
        //        selPageSize += '<option value="20">20</option>';
        //        selPageSize += '<option value="50">50</option>';
        //        selPageSize += '<option value="100">100</option>';
        //        selPageSize += '<option value="200">200</option>';
        //        selPageSize += '</select>条&nbsp;&nbsp;';
        //        $pager.append(selPageSize);

        // 页码
        $pager.append(renderButton('首页', pageIndex, pageCount, buttonClickCallback)).append(renderButton('上页', pageIndex, pageCount, buttonClickCallback));

        // pager currently only handles 10 viewable pages ( could be easily parameterized, maybe in next version ) so handle edge cases
        var startPoint = 1;
        var endPoint = 5;

        if (pageIndex > 2) {
            startPoint = pageIndex - 2;
            endPoint = pageIndex + 2;
        }

        if (endPoint > pageCount) {
            startPoint = pageCount - 4;
            endPoint = pageCount;
        }

        if (startPoint < 1) {
            startPoint = 1;
        }

        // loop thru visible pages and render buttons
        for (var page = startPoint; page <= endPoint; page++) {
            var currentButton = pageIndex === page
                ? $('<li class="active">' + page + '</li>')
                : $('<li data-page="' + page + '"><a href="javascript:;">' + page + '</a></li>');

            currentButton.click(function () {
                buttonClickCallback($(this).attr('data-page'));
                return false;
            });
            currentButton.appendTo($pager);
        }

        // render in the next and last buttons before returning the whole rendered control back.
        $pager.append(renderButton('下页', pageIndex, pageCount, buttonClickCallback)).append(renderButton('末页', pageIndex, pageCount, buttonClickCallback));
        return $pager;
    }

    // renders and returns a 'specialized' button, ie 'next', 'previous' etc. rather than a page number button
    function renderButton(buttonLabel, pageIndex, pageCount, buttonClickCallback) {

        var $Button = $('<a href="javascript:;">' + buttonLabel + '</a>');

        var destPage = 1;

        // work out destination page for required button type
        switch (buttonLabel) {
            case "首页":
                destPage = 1;
                break;
            case "上页":
                destPage = pageIndex - 1;
                break;
            case "下页":
                destPage = pageIndex + 1;
                break;
            case "末页":
                destPage = pageCount;
                break;
        }

        // disable and 'grey' out buttons if not needed.
        if (buttonLabel == "首页" || buttonLabel == "上页") {
            pageIndex <= 1 ? $Button.addClass('previous-off') : $Button.click(function () {
                buttonClickCallback(destPage);
                return false;
            });
        }
        else {
            pageIndex >= pageCount ? $Button.addClass('previous-off') : $Button.click(function () {
                buttonClickCallback(destPage);
                return false;
            });
        }

        return $Button;
    }

    // pager defaults. hardly worth bothering with in this case but used as placeholder for expansion in the next version
    $.fn.pager.defaults = {
        pageIndex: 1,
        pageCount: 1
    };

})(jQuery);