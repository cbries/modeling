/*********************************************************************
 * Railuino - Hacking your Märklin
 *
 * Copyright (C) 2012 Joerg Pleumann
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * LICENSE file for more details.
 */
 
#include "TrackReporterS88_DS.h"

// ===================================================================
// === TrackReporterS88_DS ==============================================
// ===================================================================

const int DATA = A0;
const int CLOCK = 3;
const int LOAD = 4;
const int RESET = 5;
	
const int TIME = 50;

TrackReporterS88_DS::TrackReporterS88_DS(int modules) {
	mSize = modules;
	
	// pinMode(DATA, INPUT);
	pinMode(CLOCK, OUTPUT);
	pinMode(LOAD, OUTPUT);
	pinMode(RESET, OUTPUT);
}

void TrackReporterS88_DS::refresh()
{
	refresh(mSize);
}

void TrackReporterS88_DS::refresh(int inMaxSize)
{
	int myByte = 0;
	int myBit = 0;

	
	for (int i = 0; i < sizeof(mSwitches); i++) {
		mSwitches[i] = 0;
	}

	digitalWrite(LOAD, HIGH);
	delayMicroseconds( TIME);
	digitalWrite(CLOCK, HIGH);
	delayMicroseconds(TIME);
	digitalWrite(CLOCK, LOW);
	delayMicroseconds(TIME);
	digitalWrite(RESET, HIGH);
	delayMicroseconds(TIME);
	digitalWrite(RESET, LOW);
	delayMicroseconds(TIME);
	digitalWrite(LOAD, LOW);

	delayMicroseconds(TIME / 2);
	bitWrite(mSwitches[myByte], myBit++, digitalRead(DATA));
	delayMicroseconds(TIME / 2);

	for (int i = 1; i < 16 * inMaxSize; i++) {
		digitalWrite(CLOCK, HIGH);
		delayMicroseconds(TIME);
		digitalWrite(CLOCK, LOW);

		delayMicroseconds(TIME / 2);
		bitWrite(mSwitches[myByte], myBit++, digitalRead(DATA));

		if (myBit == 8) {
			myByte++;
			myBit = 0;
		}

		delayMicroseconds(TIME / 2);
	}
	
}

boolean TrackReporterS88_DS::getValue(int index) {
	index--;
	return bitRead(mSwitches[index / 8], index % 8);
}

byte TrackReporterS88_DS::getByte(int index) {
	return mSwitches[index];
}
