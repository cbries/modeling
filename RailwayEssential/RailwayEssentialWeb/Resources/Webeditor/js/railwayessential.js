
var isEdit = false;

var currentSelection = "";
var currentRow = -1;
var currentColumn = -1;
var isDrag = false;

var objDrag = null;
var objPosition = null; // top, left

$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        resetSelection();
    }
});

function updateUi() {
    console.log("Edit:" + isEdit);

    var o = $('#editMenu');

    if (isEdit) {
        o.show();
    }
    else {
        o.hide();
    }
}

function rebuildTable() {
    $('td').each(function (index, el) {
        if ($(el).find('img').length == 0) {
            var col = $(el).parent().children().index($(el));
            var row = $(el).parent().parent().children().index($(el).parent());
            console.log("vs: cellClicked(" + col + ", " + row + ", \"\")");
            try {
                railwayEssentialCallback.cellClicked(col, row, "null");
            } catch (ex) { /* ignore */ }
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

function resetSelection() {
    $('td').each(function () {
        $(this).css("background-color", "");
    });
}

function selectElement(el) {
    resetSelection();
    el.parent().css("background-color", "red");
}

function rotateElement(col, row, el) {
    var o = el;

    function ss(col, row, orientation) {
        console.log("vs: cellRotated(" + col + ", " + row + ", " + orientation + ")");
        try {
            railwayEssentialCallback.cellRotated(col, row, orientation);
        } catch (ex) { /* ignore */ }
    }

    if (o.hasClass('rot0')) {
        o.removeClass('rot0');
        o.addClass('imgflip');
        ss(col, row, "imgflip");
    } else if (o.hasClass('imgflip') && !o.hasClass('imgflip2')) {
        o.removeClass('imgflip');
        o.addClass('imgflip2');
        ss(col, row, "imgflip2");
    } else if (o.hasClass('imgflip2') && !o.hasClass('imgflip')) {
        o.removeClass('imgflip2');
        o.removeClass('imgflip');
        o.addClass('imgflip imgflip2');
        ss(col, row, "imgflip imgflip2");
    } else if (o.hasClass('imgflip') && o.hasClass('imgflip2')) {
        o.removeClass('imgflip');
        o.removeClass('imgflip2');
        o.addClass('rot0');
        ss(col, row, "rot0");
    } else {
        o.addClass('imgflip');
        ss(col, row, "imgflip");
    }
}

function test(col, row) {
    console.log(col + ", " + row);
}

function changeSymbol(col, row, symbol, orientation) {
    $('td').each(function (index, el) {
        var oel = $(el);
        var c = oel.parent().children().index(oel);
        var r = oel.parent().parent().children().index(oel.parent());

        if (col === c && row === r) {
            var cdiv = oel.find("div");
            if (cdiv.find("img").length === 0)
                return;

            var o = $('#webmenu').val();
            var v = themeDirectory + '/' + symbol + '.svg';

            try {
                var m = "";
                m += "Change Coord(" + col + ", " + row + "): " + symbol + ", " + orientation + ", " + orientation;
                railwayEssentialCallback.message(m);
            } catch (e) {
                console.log(e);
            }

            var img = cdiv.find("img");
            img.removeClass("rot0");
            img.removeClass("imgflip");
            img.removeClass("imgflip2");
            img.addClass(orientation);
            img.removeData("railway-symbol");
            img.data("railway-symbol", symbol);
            img.attr("src", v);
        }
    });
}

function simulateClick(col, row, symbol, orientation) {

    $('td').each(function (index, el) {
        var oel = $(el);
        var c = oel.parent().children().index(oel);
        var r = oel.parent().parent().children().index(oel.parent());

        if (col === c && row === r) {
            var cdiv = oel.find("div");
            if (cdiv.find("img").length === 1)
                return;

            var o = $('#webmenu').val();
            var v = themeDirectory + '/' + symbol + '.svg';

            try {
                var m = "";
                m += "Coord(" + col + ", " + row + "): " + symbol + ", " + orientation + ", " + orientation;
                railwayEssentialCallback.message(m);
            } catch (e) {
                console.log(e);
            }

            var newChild = cdiv.append("<img class=\"overflow " + orientation + "\" src=\""
                + v + "\" border=\"0\" data-railway-symbol=\""
                + o + "\">");

            newChild.click(function (evt) {
                if (evt.ctrlKey && evt.altKey) {
                    rotateElement(col, row, $(this));
                } else if (evt.ctrlKey) {
                    //selectElement($(this));
                    rotateElement(col, row, $(this));
                } else if (evt.altKey) {
                    $(this).remove();
                    resetSelection();
                    rebuildTable();
                }
            });

            newChild.draggable();
        }
    });
}

function handleUserClick(col, row) {
    // TODO add click handler
    alert("clicked: " + col + ", " + row);
}

$(document).ready(function (e) {

    var isMouseDown = false;
    var isDragging = false;
    var startingPos = [];

    $('#cmdEdit').click(function () {
        isEdit = !isEdit;

        updateUi();
    });

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

                // ###################
                //        DROP
                // ###################

                if (!isEdit)
                    return;

                var targetObject = findTargetTd(evt, function (col, row, target) {
                    var src = objDrag.attr("src");
                    if (src === 'undefined' || src == null)
                        return;
                    var symbol = objDrag.data("railway-symbol");

                    objDrag.remove();
                    objDrag = null;

                    resetSelection();
                    rebuildTable();

                    var c = target.find("div");
                    if (c.find("img").length == 1)
                        return;

                    var newChild = c.append("<img class=\"overflow\" src=\"" + src + "\" border=\"0\" data-railway-symbol=\"" + symbol + "\">");
                    newChild.click(function (evt) {
                        if (evt.ctrlKey && evt.altKey) {
                            rotateElement(col, row, $(this));
                        } else if (evt.ctrlKey) {
                            //selectElement($(this));
                            rotateElement(col, row, $(this));
                        } else if (evt.altKey) {
                            $(this).remove();
                            resetSelection();
                            rebuildTable();
                        }
                    });
                    newChild.draggable();

                    console.log("vs: cellClicked(" + col + ", " + row + ", " + symbol + ")");
                    try {
                        railwayEssentialCallback.cellClicked(col, row, symbol);
                    } catch (ex) { /* ignore */ }
                });

            } else {

                // ###################
                //        CLICK
                // ###################

                objDrag = null;

                var c = $(this).find("div");
                if (c.find("img").length == 1) {
                    if (isEdit)
                        return;

                    handleUserClick(col, row);
                    return;
                }

                if (!isEdit) {
                    handleUserClick(col, row);
                    return;
                }

                var o = $('#webmenu').val();
                var v = themeDirectory + '/' + o + '.svg';

                var newChild = c.append("<img class=\"overflow\" src=\""
                    + v + "\" border=\"0\" data-railway-symbol=\""
                    + o + "\">");

                newChild.click(function (evt) {

                    if (evt.ctrlKey && evt.altKey) {
                        rotateElement(col, row, $(this));
                    } else if (evt.ctrlKey) {
                        selectElement($(this));
                    } else if (evt.altKey) {
                        $(this).remove();
                        resetSelection();
                        rebuildTable();
                    }
                });

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
