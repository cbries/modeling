
var currentSelection = "";
var currentRow = -1;
var currentColumn = -1;

$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        if (currentSelection.length <= 0)
            return;
        currentSelection.removeClass('highlight');
        currentColumn = -1;
        currentRow = -1;
    }
});

$(document).ready(function (e) {
    try {
        $("body select").msDropDown({ visibleRows: 20, roundedCorner: false });
    } catch (e) {
        railwayEssentialCallback.message(e.message);
    }

    $('#ccw').click(function () {
        if (currentSelection.length <= 0)
            return;
        var currentValue = getRotationDegrees(currentSelection) - 90;
        if (currentValue >= 360)
            currentValue = 0;
        currentSelection.css('transform', 'rotate(' + currentValue + 'deg)');
        railwayEssentialCallback.cellRotated(currentColumn, currentRow, currentValue);
    });

    $('#cw').click(function () {
        if (currentSelection.length <= 0)
            return;
        var currentValue = getRotationDegrees(currentSelection) + 90;
        if (currentValue >= 360)
            currentValue = 0;
        currentSelection.css('transform', 'rotate(' + currentValue + 'deg)');
        railwayEssentialCallback.cellRotated(currentColumn, currentRow, currentValue);
    });

    $('td').click(function (evt) {
        var col = $(this).parent().children().index($(this));
        var row = $(this).parent().parent().children().index($(this).parent());

        var p = $(this).position();

        if (evt.ctrlKey) {
            // ignore
        } else if (evt.altKey) {
            // ignore
        } else {
            var o = $('#webmenu').val();
            var v = themeDirectory + '/' + o + '.svg';

            $(".childDiv").append(
                    "<div style=\"width: 32px: height: 32px;\"><img src=\"" + v + "\" style=\"text-align: middle; position: absolute; top: "
                    + (p.top + 1) + "px; left: " + (p.left + 1)
                    + "px; z-index: 10; height: 33px; border: 0; overflow:visible;\" class=\"\"></div>")
                .find('img').click(function (evt) {
                    if (evt.altKey) {
                        $(this).remove();
                    } else if (evt.ctrlKey) {
                        var o = $(this);
                        if (o.hasClass('rot0')) {
                            o.removeClass('rot0');
                            o.addClass('rot90');
                        } else if (o.hasClass('rot90')) {
                            o.removeClass('rot90');
                            o.addClass('imgflip');
                        } else if (o.hasClass('imgflip')) {
                            o.removeClass('imgflip');
                            o.addClass('rot0');
                        //} else if (o.hasClass('rot270')) {
                        //    o.removeClass('rot270');
                        //    o.addClass('rot0');
                        } else {
                            o.addClass('rot90');
                        }
                    }
                });

            railwayEssentialCallback.cellClicked(col, row, o);
        }
    });
});
