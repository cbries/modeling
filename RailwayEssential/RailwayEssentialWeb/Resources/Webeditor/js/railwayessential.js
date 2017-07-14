function getRotationDegrees(obj) {
    var matrix = obj.css("-webkit-transform") ||
    obj.css("-moz-transform")    ||
    obj.css("-ms-transform")     ||
    obj.css("-o-transform")      ||
    obj.css("transform");
    if(matrix !== 'none') {
        var values = matrix.split('(')[1].split(')')[0].split(',');
        var a = values[0];
        var b = values[1];
        var angle = Math.round(Math.atan2(b, a) * (180/Math.PI));
    } else { var angle = 0; }
    return (angle < 0) ? angle + 360 : angle;
}

var currentSelection = "";
var currentRow = -1;
var currentColumn = -1;

$(document).keyup(function(e) {
      if (e.keyCode == 27) {
			if(currentSelection.length <= 0)
				return;
            currentSelection.removeClass('highlight');
            currentColumn = -1;
            currentRow = -1;
      }
	});

$(document).ready(function(e) {
	try { 
	    $("body select").msDropDown({ visibleRows: 20, roundedCorner: false });
	} catch(e) {
		railwayEssentialCallback.message(e.message);
	}
	
	$('#ccw').click(function(){
		if(currentSelection.length <= 0)
			return;
		var currentValue = getRotationDegrees(currentSelection) - 90;
		if(currentValue >= 360)
			currentValue = 0;
        currentSelection.css('transform', 'rotate(' + currentValue + 'deg)');
        railwayEssentialCallback.cellRotated(currentColumn, currentRow, currentValue);
	});

	$('#cw').click(function(){
		if(currentSelection.length <= 0)
			return;
		var currentValue = getRotationDegrees(currentSelection) + 90;
		if(currentValue >= 360)
			currentValue = 0;
        currentSelection.css('transform', 'rotate(' + currentValue + 'deg)');
        railwayEssentialCallback.cellRotated(currentColumn, currentRow, currentValue);
	});
	
	$('td').click(function(evt){
			var col = $(this).parent().children().index($(this));
			var row = $(this).parent().parent().children().index($(this).parent());
			var s = "#cell_" + col + "_" + row;

	    if (evt.ctrlKey) {
	        if (currentSelection.length > 0)
	            currentSelection.removeClass('highlight');
	        currentColumn = col;
	        currentRow = row;
	        currentSelection = $(s);
	        currentSelection.addClass('highlight');
        } else if (evt.altKey) {
            $(s).css('background-image', '');
	        railwayEssentialCallback.cellClicked(col, row, '');
    	} else {	
                var o = $('#webmenu').val();
                var v = 'url(' + themeDirectory + '/' + o + '.svg)';
                $(s).css('background-image', v);
				railwayEssentialCallback.cellClicked(col, row, o);				
			}
		});    
});
