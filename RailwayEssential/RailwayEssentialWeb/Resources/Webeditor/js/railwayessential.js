
var currentSelection = "";
var currentRow = -1;
var currentColumn = -1;
var isDrag = false;

var objDrag = null;
var objPosition = null; // top, left

function rebuildTable() {
    $('td').each(function (index, el) {
        if ($(el).find('img').length == 0) {
            $(el).html("<div class=\"overflow\"></div>");
        }
    });
}

function findTargetTd(evt, callback) {

    var clientX = evt.clientX;
    var clientY = evt.clientY;

    $('td').each(function (index, el) {

        if (objDrag == null)
            return;

        var position = $(el).position();

        var w = $(el).width();
        var h = $(el).height();

        var x0 = position.left;
        var x1 = x0 + w;
        var y0 = position.top;
        var y1 = y0 + h;

        if (clientX >= x0 && clientX <= x1) {
            if (clientY >= y0 && clientY <= y1) {
                var o = $(el);
                var col = o.parent().children().index(o);
                var row = o.parent().parent().children().index(o.parent());

                callback(col, row, $(el));

                return $(el);
            }
        }
    });

    return null;
}

$(document).ready(function (e) {

    var isMouseDown = false;
    var isDragging = false;
    var startingPos = [];

    $("td")
        .mousedown(function (evt) {
            isDragging = false;
            isMouseDown = true;
            startingPos = [evt.pageX, evt.pageY];
            objDrag = $(this).find("img");
        })
        .mousemove(function (evt) {
            if (!(evt.pageX === startingPos[0] && evt.pageY === startingPos[1])) {
                isDragging = true;

                if (isMouseDown) {
                    // ignore
                }
            }
        })
        .mouseup(function (evt) {
            isMouseDown = false;

            var col = $(this).parent().children().index($(this));
            var row = $(this).parent().parent().children().index($(this).parent());

            if (isDragging && objDrag !== null) {

                var targetObject = findTargetTd(evt, function (col, row, target) {
                    var src = objDrag.attr("src");
                    if (src === 'undefined' || src == null)
                        return;
                    var symbol = objDrag.data("railway-symbol");

                    objDrag.remove();
                    objDrag = null;

                    rebuildTable();

                    var c = target.find("div");
                    if (c.find("img").length == 1)
                        return;

                    var newChild = c.append("<img class=\"overflow\" src=\"" + src + "\" border=\"0\">");
                    newChild.draggable();

                    console.log("vs: cellClicked(" + col + ", " + row + ", " + symbol + ")");
                    try {
                        railwayEssentialCallback.cellClicked(col, row, symbol);
                    } catch (ex) { /* ignore */ }
                });

            } else {

                objDrag = null;

                /******/

                var c = $(this).find("div");
                if (c.find("img").length == 1)
                    return;

                //console.log("Click offset: " + $(this).offset().top + ", " + $(this).offset().left);

                var o = $('#webmenu').val();
                var v = themeDirectory + '/' + o + '.svg';

                var newChild = c.append("<img class=\"overflow\" src=\"" + v + "\" border=\"0\" data-railway-symbol=\"" + o + "\">");
                newChild.draggable();

                console.log("vs: cellClicked(" + col + ", " + row + ", " + o + ")");
                try {
                    railwayEssentialCallback.cellClicked(col, row, o);
                } catch (ex) { /* ignore */ }

            }
            isDragging = false;
            startingPos = [];
        });

    try {
        $("body select").msDropDown({ visibleRows: 20, roundedCorner: false });
    } catch (e) {
        railwayEssentialCallback.message(e.message);
    }
});
