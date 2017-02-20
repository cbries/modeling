/*********************************************************************
 * Desktop Station Serial Gateway Sketch for Arduino UNO/Leonard
 *
 * Copyright (C) 2013 Yaasan
 * 
 * This example is free software; you can redistribute it and/or
 * modify it under the terms of the Creative Commons Zero License,
 * version 1.0, as published by the Creative Commons Organisation.
 * This effectively puts the file into the public domain.
 *
 * This example is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	 See the
 * LICENSE file for more details.
 */

#include <Railuino.h>
#include <avr/wdt.h>
#include "TrackReporterS88_DS.h"

#define MAX_S88DECODER 1
#define RELPYERROR_300 "300 Command error"
#define RELPYERROR_301 "301 Syntax error"
#define RELPYERROR_302 "302 receive timeout"
#define RELPYERROR_303 "303 Unknown error"
#define RELPYERROR_NONE ""


TrackController ctrl(0xdf24, true);
TrackReporterS88_DS reporter(MAX_S88DECODER);

void decodePacket(TrackMessage &inMessage);

String request;

String function;

word arguments[8];

word numOfArguments;

boolean result;

void setup() {
  
  
  Serial.begin(115200);
  while (!Serial);
  
  wdt_enable(WDTO_8S); // WDT reset
  ctrl.begin();

  /* Clear data of the S88 decoder */
  reporter.refresh();


  Serial.println("--------------------------------------");
  Serial.println("Desktop Station Interface with Arduino");
  Serial.println("--------------------------------------");
  Serial.println("100 Ready");
  
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  

}

int receiveRequest() {
	char buffer[128];
	int i = 0;
	char aByte;
	int aResult = -1;
	
	unsigned long time = millis();
	
	/* Check serial buffer */
	if (Serial.available() > 0) {
		
		while (1) {
		
			if (Serial.available() > 0) {
			
				aByte = Serial.read();
				
				// Write to text buf.
				buffer[i] = aByte;
				
				// check last identification of text end.
				if (aByte == '\n') {
					aResult = i;
					break;
				}
				
				// increment
				i = i + 1;
				
				// buf over check
				if( i > 127)
				{
					break;
				}
			}
			else
			{
				/* Check timeout. */
				if( millis() > time + 4000)
				{
					aResult = -1;
					break;
				}
			}
		}
	}
	else
	{
		/* Not received. */
		aResult = 0;
	}
	
	if( aResult > 0)
	{
	
		buffer[i] = '\0';
		
		request = String(buffer);
	}
	else
	{
		request = "";
	}
	
	return aResult;
}

word stringToWord(String s) {
  word result = 0;
  
  for (int i = 0; i < s.length(); i++) {
	result = 10 * result + (s.charAt(i) - '0');
  }
  
  return result;
}

boolean parse() {
  int lpar = request.indexOf('(');
  if (lpar == -1) {
	return false;
  }
  
  function = String(request.substring(0, lpar));
  function.trim();
  
  int offset = lpar + 1;
  int comma = request.indexOf(',', offset);
  numOfArguments = 0;
  while (comma != -1) {
	String tmp = request.substring(offset, comma);
	tmp.trim();
	arguments[numOfArguments++] = stringToWord(tmp);
	offset = comma + 1;
	comma = request.indexOf(',', offset);
  }

  int rpar = request.indexOf(')', offset);
  while (rpar == -1) {
	return false;
  }
  
  if (rpar > offset) {
	String tmp = request.substring(offset, rpar);
	tmp.trim();
	arguments[numOfArguments++] = stringToWord(tmp);
  }
  
  return true;
}

boolean dispatch() {
  byte aValue;
  byte aValueHigh;
  boolean aResult;

  if (function == "accelerateLoco") {
	return ctrl.accelerateLoco(arguments[0]);
	
  } else if (function == "decelerateLoco") {
	return ctrl.decelerateLoco(arguments[0]);
	
  } else if (function == "toggleLocoDirection") {
	return ctrl.toggleLocoDirection(arguments[0]);
	
  } else if (function == "setLocoDirection") {
	return ctrl.setLocoDirection(arguments[0], arguments[1]);
	
  } else if (function == "toggleLocoFunction") {
	return ctrl.toggleLocoFunction(arguments[0], arguments[1]);
	
  } else if (function == "setLocoFunction") {
	return ctrl.setLocoFunction(arguments[0], arguments[1], arguments[2]);
	
  } else if (function == "setTurnout") {
	return ctrl.setTurnout(arguments[0], arguments[1]);
	
  } else if (function == "setPower") {
	return ctrl.setPower(arguments[0]);
	
  } else if (function == "setLocoSpeed") {
	return ctrl.setLocoSpeed(arguments[0], arguments[1]);
  } else if (function == "getLocoConfig") {
  
	aResult = ctrl.readConfig(arguments[0], arguments[1], &aValue);
	Serial.print("@CV,");
	Serial.print(arguments[0], DEC);
	Serial.print(",");
	Serial.print(arguments[1], DEC);
	Serial.print(",");
	Serial.print(aValue, DEC);
	Serial.println(",");
	
	return true;
  } else if (function == "setLocoConfig") {
	return ctrl.writeConfig(arguments[0], arguments[1], arguments[2]);
	
  } else if (function == "setPing") {
	return setPing();
	
  } else if (function == "getVersion") {
  
	aResult = ctrl.getVersion(&aValueHigh, &aValue);
	
	Serial.print("@VER,");
	Serial.print(aValueHigh, HEX);
	Serial.print(aValue, HEX);
	Serial.println(",");

  }
  else if (function == "getS88")
  {
 	int aMaxS88Num = MAX_S88DECODER;
	
	if( arguments[0] > 0)
	{
		aMaxS88Num = arguments[0];
	}

	reporter.refresh(aMaxS88Num);

        //Send a S88 sensor reply 
	Serial.print("@S88,");

	word aFlags = 0;

	for( int j = 0; j < aMaxS88Num; j++)
	{
	  aFlags = (reporter.getByte((j << 1) + 1) << 8) + reporter.getByte(j << 1);
	  
	  Serial.print(aFlags, HEX);
	  Serial.print(",");
	}
		
	Serial.println("");
	
    return true;
	
  } /* getS88 */
  else if (function == "reset")
  {
  
	ctrl.end();
	ctrl.begin();
	Serial.println("100 Ready");
	
	return true;
  } /* reset */
  else if (function == "mfxDiscovery")
  {
	setMfxDiscovery();
	
	return true;
  } /* mfxDiscovery */
  else if (function == "mfxBind")
  {
	setMfxBind(arguments[0], arguments[1], arguments[2]);
	
	return true;
  } /* mfxBind */
  else if (function == "mfxVerify")
  {
	setMfxVerify(arguments[0], arguments[1], arguments[2]);
	
	return true;
  } /* mfxVerify */
  else
  {
	return false;
  }
  
}

void loop() {
	
	TrackMessage message;
	String aReplyText = RELPYERROR_NONE;
	
	/* Reset watchdog timer. */
	wdt_reset(); 

	//Receive command from Desktop Station(PC via USB)
	int aReceived = receiveRequest();
	
	/* Reset watchdog timer. */
	wdt_reset();
	
	if( aReceived > 0)
	{
  		if (parse()) {
			if (dispatch()) {
				aReplyText = "200 Ok";
			} else {
				//Serial.println(function);
				aReplyText = RELPYERROR_300;
			}
		} else {
			aReplyText = RELPYERROR_301;
		}
	}
	else if( aReceived == 0)
	{
		aReplyText = "";
		
		/* Receive the others packet */
		if( ctrl.receiveMessage(message) == true)
		{
			/* Packet decode */
			decodePacket(message);
		}
	
	}
	else if( aReceived < 0)
	{
		// Not received per 4sec
		// Reset serial
		Serial.flush();
		
		aReplyText = RELPYERROR_302;
        }
	else
	{
		/* Nothing to do */
		
	}
	
	
	/* Reply to Desktop Station */
	if( aReplyText != "")
	{
		Serial.println(aReplyText);
	}

}

boolean setPing() {
	TrackMessage message;
	
	
	message.clear();
	message.command = 0x18;
	message.length = 0x00;

	ctrl.sendMessage(message);
	
	return true;
}


void decodePacket(TrackMessage &inMessage)
{

	if( (inMessage.command == 0x18) && (inMessage.length == 0x08) && (inMessage.response == true))
	{
		/* mfx discovery */
		Serial.print("@PING,");
		Serial.print(inMessage.data[0], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[1], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[4], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[5], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[6], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[7], HEX);
		Serial.println(",");
		
	}
	
	/* Check received packet */
	if( inMessage.response == false)
	{
		return;
	}
	
	/* Decode received packets. */
	if( (inMessage.command == 0x00) && (inMessage.length == 0x05))
	{
		/* Power */
		Serial.print("@PWR,");
		Serial.print(inMessage.data[4], HEX);
		Serial.println(",");
	}
	
	if( (inMessage.command == 0x05) && (inMessage.length == 0x05))
	{
		
		/* Locomotive direction */
		Serial.print("@DIR,");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[4], HEX);
		Serial.println(",");
	}
	
	if( (inMessage.command == 0x04) && (inMessage.length == 0x06))
	{
	
		/* Locomotive speed */
		Serial.print("@SPD,");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[4], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[5], HEX);
		Serial.println(",");
	}
	
	if( (inMessage.command == 0x06) && (inMessage.length == 0x06))
	{
		/* Locomotive functions */
		Serial.print("@FNC,");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[4], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[5]);
		Serial.println(",");
	}
	
	if( (inMessage.command == 0x0b) && (inMessage.length == 0x06))
	{
		if( inMessage.data[5] != 0)
		{
			/* Accessories */
			Serial.print("@ACC,");
			Serial.print(inMessage.data[2], HEX);
			Serial.print(",");
			Serial.print(inMessage.data[3], HEX);
			Serial.print(",");
			Serial.print(inMessage.data[4], HEX);
			Serial.print(",");
			Serial.print(inMessage.data[5], HEX);
			Serial.println(",");
		}
	}
	
	if( (inMessage.command == 0x01) && (inMessage.length == 0x05))
	{
		/* mfx discovery */
		Serial.print("@MFX,");
		Serial.print(inMessage.data[0], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[1], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.println(",");
		
	}
	
	if( (inMessage.command == 0x02) && (inMessage.length == 0x06))
	{
		/* mfx discovery */
		Serial.print("@MFXBIND,");
		Serial.print(inMessage.data[0], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[1], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[2], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[3], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[4], HEX);
		Serial.print(",");
		Serial.print(inMessage.data[5], HEX);
		Serial.println(",");
		
	}
	
}


boolean setMfxDiscovery() {

	TrackMessage message;

	/* mfx bind */
	message.clear();
	message.command = 0x01;
	message.length = 0x01;
	message.data[0] = 0x20;

	return ctrl.sendMessage(message);
}

boolean setMfxBind(word inUID_L, word inUID_H, word inAddress) {

	TrackMessage message;

	/* mfx bind */
	message.clear();
	message.command = 0x02;
	message.length = 0x06;
	message.data[0] = (byte)((inUID_L >> 8) & 0xFF);
	message.data[1] = (byte)(inUID_L & 0xFF);
	message.data[2] = (byte)((inUID_H >> 8) & 0xFF);
	message.data[3] = (byte)(inUID_H & 0xFF);
	message.data[4] = (byte)((inAddress >> 8) & 0xFF);
	message.data[5] = (byte)(inAddress & 0xFF);

	return ctrl.exchangeMessage(message, message, 1000);
}

boolean setMfxVerify(word inUID_L, word inUID_H, word inAddress) {
	TrackMessage message;
	
	/* mfx verify */
	message.clear();
	message.command = 0x03;
	message.length = 0x06;
	message.data[0] = (byte)((inUID_L >> 8) & 0xFF);
	message.data[1] = (byte)(inUID_L & 0xFF);
	message.data[2] = (byte)((inUID_H >> 8) & 0xFF);
	message.data[3] = (byte)(inUID_H & 0xFF);
	message.data[4] = (byte)((inAddress >> 8) & 0xFF);
	message.data[5] = (byte)(inAddress & 0xFF);

	return ctrl.exchangeMessage(message, message, 1000);
	
}

