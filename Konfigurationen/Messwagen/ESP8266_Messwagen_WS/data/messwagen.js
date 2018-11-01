var recentData = null;
var queueRecentData = [];
var maxLength = 50;

var ws = null;

var imgStatusConnected = null;
var imgStatusDisconnected = null;
var imgCalibratePlus = null;
var imgCalibrateMinus = null;
var txtCalibrateTitle = null;
var calibrationMenu = null;
var cmdStart = null;
var cmdStop = null;
var cmdStartGForce= null;

var offsetData = null;
var distanceData = null;

function isConnected() {
	if(ws == null) return false;
	return ws.readyState === ws.OPEN;
}

function ConnectToEsp8266() { 
	if ("WebSocket" in window) {               
		ws = new WebSocket(wsHost);				
		ws.onopen = function() { 
			imgStatusConnected.show();
			imgStatusDisconnected.hide();
		};
		ws.onmessage = function (evt) { 
			var received_msg = evt.data;
			recentData = JSON.parse(received_msg);
			queueRecentData.push(recentData);
			if(queueRecentData.length > maxLength) {
				queueRecentData.splice(maxLength-1, 1);
			}
			handleData();
		};
		ws.onclose = function() { 
			imgStatusConnected.hide();
			imgStatusDisconnected.show();
		};
	} else {              
		console.log("WebSocket NOT supported by your Browser!");
	}
}

function handleData() {	
	$('.millis').text(recentData.millis);
	if(recentData.dhtStatus == 0) $('.dhtStatus').text("OK");
	else               			  $('.dhtStatus').text("BAD");
	$('.dhtHumidity').text(recentData.dhtHumidity + " %");
	$('.dhtTemperature').text(recentData.dhtTemperature + " °C");
	$('.dhtFahrenheit').text(recentData.dhtFahrenheit + " F");
	$('.dhtHeatIndexCelsius').text(recentData.dhtHeatIndexCelsius);
	$('.dhtHeatIndexCelsius').text(recentData.dhtHeatIndexCelsius);
	$('.dhtHeatIndexFahrenheit').text(recentData.dhtHeatIndexFahrenheit);
	$('.AcX').text(  "x:" + recentData.AcX);
	$('.AcY').text(", y:" + recentData.AcY);
	$('.AcZ').text(", z:" + recentData.AcZ);
	$('.GyX').text(  "x:" + recentData.GyX);
	$('.GyY').text(", y:" + recentData.GyY);
	$('.GyZ').text(", z:" + recentData.GyZ);
	$('.numberOfInterrupts').text(recentData.numberOfInterrupts);
		
	var x = parseInt(recentData.angleX);
	var y = parseInt(recentData.angleY);
	var z = parseInt(recentData.angleZ);
		
	if(offsetData !== null) {
		var xoffset = parseInt(offsetData.medAgX);
		var yoffset = parseInt(offsetData.medAgY);
		
		var xx = (x-xoffset);
		var yy = (y-yoffset);
		
		if(xx <= -90) xx = 360 + xx;
		if(yy <= -90) yy = 360 + yy;
				
		$('.angleXoffset').html("Kippung: " + xx + "°");
		$('.angleYoffset').html("&nbsp; Gefälle: " + yy + "°");
	} else {
		$('.angleX').html("Kippung: " + x + "°");
		$('.angleY').html("&nbsp; Gefälle: " + y + "°");
	}
	
	if(distanceData !== null) {
		var target = $('.speed');
		
		/*
		beginInt: beginInt,
		endInt: endInt,
		deltaInt: deltaInt
		*/
					
		var lastData = recentData[0];
		var previousData = null;
		if(recentData.length >= 5)
			previousData = recentData[4];
		
		var timeDelta = previousData.millis - lastData.millis;
		var interuptsDelta = previousData.numberOfInterrupts - lastData.numberOfInterrupts;
		
		var nfloat = 1000 % timeDelta;
		
		// v [interupts/sec] 
		var interuptsOfSecond = parseInt(interuptsDelta * nfloat);		
		var meterOfSecond = interuptsOfSecond / distanceData.deltaInt;
		var kmh = meterOfSecond * 3.6;
		
		console.log("m/s: " + meterOfSecond);
		console.log("km/h: " + kmh);
		
		target.html(parseInt(kmh) + " km/h");
	}
}

function sendFileContent(fpath, cnt64) {
	var data = {fname: fpath, cnt: cnt64};
	ws.send(JSON.stringify(data));
}

var calibrateMenuShown = false;
function toggleCalibrateMenu() {	
	if(calibrateMenuShown === true) {
		calibrateMenuShown = false;
		imgCalibratePlus.show();
		imgCalibrateMinus.hide();
		txtCalibrateTitle.text("Open calibration menu");
		calibrationMenu.hide();
	} else {
		calibrateMenuShown = true;
		imgCalibratePlus.hide();
		imgCalibrateMinus.show();
		txtCalibrateTitle.text("Close calibration menu");
		calibrationMenu.show();
	}
}

function initReferences() {
	imgStatusConnected = $('#imgStatusConnected');
	imgStatusDisconnected = $('#imgStatusDisconnected');
	imgCalibratePlus = $('#imgCalibratePlus');
	imgCalibrateMinus = $('#imgCalibrateMinus');
	txtCalibrateTitle = $('#titleCalibrate');
	calibrationMenu = $('.calibrationMenu');
	cmdStart = $('#cmdStart');
	cmdStop = $('#cmdStop');
	cmdStartGForce = $('#cmdStartGForce');
}

var meterDistanceBegin = null;
var meterDistanceEnd = null;

function cmdStartClicked() {
	cmdStart.prop("disabled", true);
	cmdStop.prop("disabled", false);
	
	meterDistanceBegin = queueRecentData[0];
}

function cmdStopClicked() {
	cmdStart.prop("disabled", false);
	cmdStop.prop("disabled", true);
	
	meterDistanceEnd = queueRecentData[0];
	
	var beginInt = meterDistanceBegin.numberOfInterrupts;
	var endInt = meterDistanceEnd.numberOfInterrupts;	
	var deltaInt = endInt - beginInt;
	
	var jsonData = {
		beginInt: beginInt,
		endInt: endInt,
		deltaInt: deltaInt
	};
	
	distanceData = jsonData;
	
	var encoded = btoa(JSON.stringify(jsonData))	
	console.log(encoded);
	sendFileContent("/distance", encoded);
}

function cmdStartGForceClicked() {
	cmdStartGForce.prop("disabled", true);
	var AcX = 0.0, AcY = 0.0, AcZ = 0.0;
	var GyX = 0.0, GyY = 0.0, GyZ = 0.0;
	var angleX = 0.0, angleY = 0.0, angleZ = 0.0;
	var n = queueRecentData.length;
	
	var ax = [], ay = [], az = [];
	var gx = [], gy = [], gz = [];
	var agX = [], agY = [], agZ = [];
	
	for(var i=0; i < n; ++i) {
		var data = queueRecentData[i];

		ax.push(data.AcX);
		ay.push(data.AcY);
		az.push(data.AcZ);
		
		gx.push(data.GyX);
		gy.push(data.GyY);
		gz.push(data.GyZ);
		
		agX.push(data.angleX);
		agY.push(data.angleY);
		agZ.push(data.angleZ);
		
		AcX += data.AcX;
		AcY += data.AcY;
		AcZ += data.AcZ;
		
		GyX += data.GyX;
		GyY += data.GyY;
		GyZ += data.GyZ;
		
		angleX += data.angleX;
		angleY += data.angleY;
		angleZ += data.angleZ;
	}
	
	ax.sort(); ay.sort(); az.sort();
	gx.sort(); gy.sort(); gz.sort();
	agX.sort(); agY.sort(); agZ.sort();
		
	var avrAcX = AcX / parseFloat(n);
	var avrAcY = AcY / parseFloat(n);
	var avrAcZ = AcZ / parseFloat(n);
	
	var avrGyX = GyX / parseFloat(n);
	var avrGyY = GyY / parseFloat(n);
	var avrGyZ = GyZ / parseFloat(n);
	
	var avrAgX = angleX / parseFloat(n);
	var avrAgY = angleY / parseFloat(n);
	var avrAgZ = angleZ / parseFloat(n);
	
	var jsonData = {
		avrAcX: avrAcX, avrAcY: avrAcY, avrAcZ: avrAcZ,
		avrGyX: avrGyX, avrGyY: avrGyY, avrGyZ: avrGyZ,
		avrAgX: avrAgX, avrAgY: avrAgY, avrAgZ: avrAgZ,
		medAcX: ax[n/2], medAcY: ay[n/2], medAcZ: az[n/2],
		medGcX: gx[n/2], medGcY: gy[n/2], medGcZ: gz[n/2],
		medAgX: agX[n/2], medAgY: agY[n/2], medAgZ: agZ[n/2]
	};

	console.log("Average Acceleration: " + avrAcX + ", " + avrAcY + ", " + avrAcZ);
	console.log("Average G-force: " + avrGyX + ", " + avrGyY + ", " + avrGyZ);
	console.log("Median Acceleration: " + jsonData.medAcX + ", " + jsonData.medAcY + ", " + jsonData.medAcZ);
	console.log("Median G-force: " + jsonData.medGcX + ", " + jsonData.medGcY + ", " + jsonData.medGcZ);
		
	offsetData = jsonData;
		
	var encoded = btoa(JSON.stringify(jsonData))	
	console.log(encoded);
	sendFileContent("/offset", encoded);
	
	cmdStartGForce.prop("disabled", false);
}

$(document).ready(function(){
	initReferences();
	
	calibrationMenu.hide();
	imgStatusConnected.hide();
	imgCalibrateMinus.hide();
	
	cmdStart.click(cmdStartClicked);
	cmdStop.click(cmdStopClicked);
	
	cmdStartGForce.click(cmdStartGForceClicked);
	
	$.get( espHost + "offset", function(data) {
		var data = atob(data);
		if(data[0] == '"')
			data = data.substr(1, data.length-2); 
		offsetData = JSON.parse(data);
	});
	$.get( espHost + "distance", function(data) {
		var data = atob(data);
		if(data[0] == '"')
			data = data.substr(1, data.length-2); 
		distanceData = JSON.parse(data);
	});
	
	imgCalibratePlus.click(toggleCalibrateMenu);
	imgCalibrateMinus.click(toggleCalibrateMenu);
	
	ConnectToEsp8266();
});