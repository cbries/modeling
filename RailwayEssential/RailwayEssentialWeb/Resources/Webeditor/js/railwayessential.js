
var isEdit = false;

const ModeAddMove = 0;
const ModeRemove = 1;
const ModeRotate = 2;
const ModeObject = 3;

var editMode = ModeAddMove;

var currentSelection = "";
var currentRow = -1;
var currentColumn = -1;
var isDrag = false;

var objDrag = null;
var objPosition = null; // top, left

function preloadSvgsLoaded() {
    // just increment the counter if there are still images pending...
    ++counter;
    if (counter >= total) {
        // this function will be called when everything is loaded
        // e.g. you can set a flag to say "I've got all the images now"
        preloadSvgsAlldone();
    }
}

function preloadSvgsAlldone() {
    try {
        railwayEssentialCallback.message("SVGs have been loaded");
    } catch (e) {
        console.log(e);
    }
}

function preloadSvgs() {
    if (symbolFiles == null || symbolFiles === 'undefined')
        return;

    // This will load the images in parallel:
    // In most browsers you can have between 4 to 6 parallel requests
    // IE7/8 can only do 2 requests in parallel per time
    for (var i = 0; i < total; i++) {
        var img = new Image();
        // When done call the function "loaded"
        img.onload = preloadSvgsLoaded;
        // cache it
        svgCache[symbolFiles[i]] = img;
        img.src = symbolFiles[i];
    }
}

function showImage(url, id) {
    // get the image referenced by the given url
    var cached = svgCache[url];
    // and append it to the element with the given id
    document.getElementById(id).appendChild(cached);
}

$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        resetSelection();
    }
});

function updateUi() {

    var o = $('#editMenu');

    if (isEdit) {

        o.show();

        $('td').each(function () {
            var img = $(this).find('img');
            if (img.length == 1)
                img.parent().draggable({ disabled: false });
        });

        $('.cell').each(function () {
            $(this).css("border", "1px solid rgba(178, 179, 179, 0.2)");
        });
    }
    else {

        o.hide();

        $('td').each(function () {
            var img = $(this).find('img');
            if (img.length == 1)
                img.parent().draggable({ disabled: true });
        });

        $('.cell').each(function () {
            $(this).css("border", "");
        });
    }
}

function rebuildCell(col, row) {
    var oel = $('#td_' + col + '_' + row);
    if (oel != null) {
        oel.html("<div class=\"overflow\"></div>");
    }
}

function rebuildTable() {
    $('td').each(function (index, el) {
        if ($(el).find('img').length == 0) {
            var col = $(el).parent().children().index($(el)) + 1;
            var row = $(el).parent().parent().children().index($(el).parent()) + 1;
            try {
                railwayEssentialCallback.cellEdited(col, row, -1);
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
                var col = o.parent().children().index(o) + 1;
                var row = o.parent().parent().children().index(o.parent()) + 1;

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
    try {
        railwayEssentialCallback.cellSelected(-1, -1);
    } catch (ex) { /* ignore */ }
}

function selectElement(col, row, el) {
    resetSelection();
    el.parent().css("background-color", "red");
    try {
        railwayEssentialCallback.cellSelected(col, row);
    } catch (ex) { /* ignore */ }
}

function rotateElement2(col, row, el) {

    if (!isEdit)
        return;

    var o = el;

    function ss(col, row, orientation) {
        //console.log("vs: cellRotated(" + col + ", " + row + ", " + orientation + ")");
        try {
            railwayEssentialCallback.cellRotated(col, row, orientation);
        } catch (ex) { /* ignore */ }
    }

    if (o.hasClass('imgflip')) {
        // links oben
        o.removeClass('imgflip');
        o.addClass('rot90');
        ss(col, row, "rot90");
    } else if (o.hasClass('rot90')) {
        // rechts oben
        o.removeClass('rot90');
        o.addClass('rot180');
        ss(col, row, "rot180");
    } else if (o.hasClass('rot180')) {
        // rechts unten
        o.removeClass('rot180');
        o.addClass('rot-90');
        ss(col, row, "rot-90");
    } else if (o.hasClass('rot-90')) {
        // links unten
        o.removeClass('rot-90');
        o.addClass('rot0');
        ss(col, row, "rot0");
    } else {
        // fallback, links oben
        o.addClass('rot90');
        ss(col, row, "rot90");
    }
}

function rotateElement(col, row, el) {
    rotateElement2(col, row, el);
    return;
}

function test(col, row) {
    console.log(col + ", " + row);
}

function highlightRoute(jsonArray) {
    console.log("Highlight Route");

    for (var i = 0; i < jsonArray.length; ++i) {
        var o = jsonArray[i];
        if (o == null || o === 'undefined')
            continue;

        var col = o.col;
        var row = o.row;

        var oel = $('#td_' + col + '_' + row);
        //var cdiv = oel.find("div");
        oel.addClass("routeHighlight");
    }
}

function resetHighlightRoute() {
    console.log("Reset Highlight Route");
    $('td').each(function () {
        $(this).removeClass("routeHighlight");
    });
}

function changeSymbol(col, row, themeId, orientation, symbol) {
    var oel = $('#td_' + col + '_' + row);
    var cdiv = oel.find("div");
    if (cdiv.find("img").length === 0)
        return;
    try {
        var m = "";
        m += "Change Coord(" + col + ", " + row + "): " + themeId + ", " + orientation + ", " + symbol;
        railwayEssentialCallback.message(m);
    } catch (e) {
        console.log(e);
    }
    rebuildCell(col, row);
    simulateClick(col, row, themeId, symbol, orientation, false);
}

function simulateClick2(jsonArray) {
    for (var i = 0; i < jsonArray.length; ++i) {
        var o = jsonArray[i];
        if (o == null || o === 'undefined')
            continue;

        var col = o.col;
        var row = o.row;
        var themeId = o.themeId;
        var symbol = o.symbol;
        var orientation = o.orientation;
        var response = false;

        simulateClick(col, row, themeId, symbol, orientation, response);
    }
}

function simulateClick(col, row, themeid, symbol, orientation, response) {

    if (response == null || response === 'undefined')
        response = false;

    var oel = $('#td_' + col + '_' + row);

    var cdiv = oel.find("div");
    if (cdiv.find("img").length === 1)
        return;

    var v = themeDirectory + '/' + symbol + '.svg';

    if (response) {
        try {
            var m = "";
            m += "Coord(" + col + ", " + row + "): " + symbol + ", " + orientation + ", " + themeid + ", " + v;
            railwayEssentialCallback.message(m);
        } catch (e) {
            console.log(e);
        }
    }

    var img = $(svgCache[v]).clone();

    var newChild = cdiv.append(img);
    newChild.addClass("overflow");
    newChild.addClass(orientation);
    newChild.attr("border", 0);
    newChild.data("railway-themeid", themeid);

    newChild.click(function (evt) {

        switch (editMode) {
        case ModeRotate:
            if (isEdit) {
                rotateElement(col, row, $(this));
            }
            break;

        case ModeRemove:
            if (isEdit) {
                $(this).remove();
                resetSelection();
                rebuildTable();
            }
            break;

        case ModeObject:
            if (isEdit) {
                selectElement(col, row, $(this));
            }
            break;
        }
    });

    newChild.draggable();
}

function handleUserClick(col, row) {
    console.log("vs: cellClicked(" + col + ", " + row + ")");
    try {
        railwayEssentialCallback.cellClicked(col, row);
    } catch (ex) { /* ignore */ }
}

function changeEditMode(state) {
    if (state == null || state === 'undefined')
        isEdit = !isEdit;
    else
        isEdit = state;

    if (!isEdit) {
        resetSelection();
        rebuildTable();
    }

    try {
        railwayEssentialCallback.editModeChanged(isEdit);
    } catch (ex) { /* ignore */ }

    updateUi();
}

function ResetRadios() {
    $("#mode-1").prop('checked', false).checkboxradio('refresh');
    $("#mode-2").prop('checked', false).checkboxradio('refresh');
    $("#mode-3").prop('checked', false).checkboxradio('refresh');
    $("#mode-4").prop('checked', false).checkboxradio('refresh');
    $("#mode-1").prop('checked', true).checkboxradio('refresh');
    updateEditMode();
}

$(document).ready(function (e) {

    var isMouseDown = false;
    var isDragging = false;
    var startingPos = [];

    var currentCategory = "Track";

    //$('#webmenuDivTrack').hide();
    $('#webmenuDivSwitch').hide();
    $('#webmenuDivSignal').hide();
    $('#webmenuDivBlock').hide();
    $('#webmenuDivSensor').hide();
    $('#webmenuDivAccessory').hide();

    $('#webmenuDivTrack').change(ResetRadios);
    $('#webmenuDivSwitch').change(ResetRadios);
    $('#webmenuDivSignal').change(ResetRadios);
    $('#webmenuDivBlock').change(ResetRadios);
    $('#webmenuDivSensor').change(ResetRadios);
    $('#webmenuDivAccessory').change(ResetRadios);

    $('#webmenuCategories').change(function () {
        $('#webmenuDivTrack').hide();
        $('#webmenuDivSwitch').hide();
        $('#webmenuDivSignal').hide();
        $('#webmenuDivBlock').hide();
        $('#webmenuDivSensor').hide();
        $('#webmenuDivAccessory').hide();

        ResetRadios();

        var cname = $(this).val();

        var sel = "#webmenuDiv" + cname;
        var oo = $(sel);
        currentCategory = oo.val();
        oo.show();
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

            var col = $(this).parent().children().index($(this)) + 1;
            var row = $(this).parent().parent().children().index($(this).parent()) + 1;

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

                    var themeId = objDrag.data("railway-themeid");

                    if (themeId === null || typeof themeId == 'undefined')
                        themeId = objDrag.parent().data("railway-themeid");

                    objDrag.remove();
                    objDrag = null;

                    resetSelection();
                    rebuildTable();

                    var c = target.find("div");
                    if (c.find("img").length == 1)
                        return;

                    var img = $(svgCache[src]).clone();

                    var newChild = c.append(img);
                    newChild.addClass("overflow");
                    newChild.attr("border", 0);
                    newChild.data("railway-themeid", themeId);

                    newChild.click(function (evt) {
                        switch (editMode) {
                        case ModeRotate:
                            if (isEdit) {
                                rotateElement(col, row, $(this));
                            }
                            break;

                        case ModeRemove:
                            if (isEdit) {
                                $(this).remove();
                                resetSelection();
                                rebuildTable();
                            }
                            break;

                        case ModeObject:
                            if (isEdit) {
                                selectElement(col, row, $(this));
                            }
                            break;
                        }

                    });
                    newChild.draggable();

                    console.log("vs: #1 cellEdited(" + col + ", " + row + ", " + themeId + ")");
                    try {
                        railwayEssentialCallback.cellEdited(col, row, themeId);
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

                if (editMode != ModeAddMove)
                    return;

                var cname = $('#webmenuCategories').val();
                var sel = $('#webmenu' + cname);
                var o = sel.val();
                var o2 = sel.find(':selected').data("railway-themeid");
                var v = themeDirectory + '/' + o + '.svg';

                var img = $(svgCache[v]).clone();

                var newChild = c.append(img);
                newChild.addClass("overflow");
                newChild.attr("border", 0);
                newChild.data("railway-themeid", o2);

                newChild.click(function (evt) {
                    switch (editMode) {
                    case ModeRotate:
                        if (isEdit) {
                            rotateElement(col, row, $(this));
                        }
                        break;

                    case ModeRemove:
                        if (isEdit) {
                            $(this).remove();
                            resetSelection();
                            rebuildTable();
                        }
                        break;

                    case ModeObject:
                        if (isEdit) {
                            selectElement(col, row, $(this));
                        }
                        break;
                    }
                });

                newChild.draggable();

                console.log("vs: #2 cellEdited(" + col + ", " + row + ", " + o2 + ")");
                try {
                    railwayEssentialCallback.cellEdited(col, row, o2);
                } catch (ex) { /* ignore */ }
            }
            isDragging = false;
            startingPos = [];
        });

    try {
        $('#webmenuCategories').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuTrack').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuSwitch').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuSignal').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuBlock').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuSensor').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
        $('#webmenuAccessory').msDropDown({ visibleRows: 20, rowHeight: 10, roundedCorner: false });
    } catch (e) {
        railwayEssentialCallback.message(e.message);
    }

    $("#mode-1").checkboxradio();
    $("#mode-2").checkboxradio();
    $("#mode-3").checkboxradio();
    $("#mode-4").checkboxradio();

    $("#mode-1").change(updateEditMode);
    $("#mode-2").change(updateEditMode);
    $("#mode-3").change(updateEditMode);
    $("#mode-4").change(updateEditMode);

    //isEdit = true;
    //updateUi();
});

function updateEditMode() {
    if ($("#mode-1").is(':checked'))
        editMode = ModeAddMove;
    else if ($("#mode-2").is(':checked'))
        editMode = ModeRemove;
    else if ($("#mode-3").is(':checked'))
        editMode = ModeRotate;
    else if ($("#mode-4").is(':checked'))
        editMode = ModeObject;
    else
        editMode = ModeAddMove;
}
