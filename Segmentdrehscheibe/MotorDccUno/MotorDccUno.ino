//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DCC Accessory / Function Decoder
// Author: Ruud Boer - September 2015
// This sketch turns an Arduino into a DCC decoder with max 17 function outputs.
// Output pins used: 3-19 (14-19 = A0-A5). Pin becomes LOW when accessory is switched ON
// Modes: 1-continuous, 2=oneshot, 3=flasher with 2 alternatin outputs, 4=signal with 2 inverted outputs
// The DCC signal is fed to pin 2 (=Interrupt 0).
// Optocoupler schematics for DCC to 5V conversion: www.rudysmodelrailway.wordpress.com/software
// Many thanks to www.mynabay.com for publishing their DCC monitor and -decoder code.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// IMPORTANT: GOTO lines 20 and 43 to configure some data!
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include <Stepper.h>
#include <EEPROM.h>
#include <DCC_Decoder.h>
#define kDCC_INTERRUPT 0

const int stepsPerRevolution = 500;
Stepper myStepper(stepsPerRevolution, 2, 3, 6, 7);
int stepsDone = 0;
int stepsDone_eepromAddr = 0;
int minSteps = 0;
int maxSteps = 35;
int pos0 = 5;
int pos1 = 18;
int pos2 = 31;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Fill in the number of accessories / functions you want to control
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
const byte maxaccessories = 5;
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

typedef struct {
  int               address;          // User Configurable. DCC address to respond to
  byte              mode;             // User Configurable. Mode: 1=Continuous, 2=Oneshot, 3=Flasher
  byte              outputPin;        // User Configurable. Arduino pin where accessory is connected to
  byte              outputPin2;       // User Configurable. 2nd pin for AlternatingFlasher (e.g. railway crossing)
  int               ontime;           // User Configurable. Oneshot or Flasher on time in ms
  int               offtime;          // User Configurable. Flasher off time in ms
  byte              onoff;            // User Configurable. Initial state of accessory output: 1=on, 0=off (ON = pin LOW)
  byte              onoff2;           // User Configurable. Initial  state of 2nd output: 1=on, 0=off
  byte              dccstate;         // Internal use. DCC state of accessory: 1=on, 0=off
  byte              finished;         // Internal use. Memory that says the Oneshot is finished
  unsigned long     onMilli;          // Internal use.
  unsigned long     offMilli;         // Internal use.
} 
DCCAccessoryAddress;
DCCAccessoryAddress accessory[maxaccessories];

byte moveToPositionValue(int steps)
{
  if(stepsDone > steps)
  {
    for(int i = stepsDone; i >= 0; --i)
    {
      decPosition();
      if(stepsDone == steps)
        return 1;
    }
  }
  else if(stepsDone < steps)
  {
    for(int i = stepsDone; i < maxSteps; ++i)
    {
      incPosition();
      if(stepsDone == steps)
        return 1;   
    }
  }

  return 0;
}

byte incPosition()
{
  if(stepsDone > maxSteps)
    return 0;    
  ++stepsDone;
  myStepper.step(-stepsPerRevolution);
  store();
  return 1;
}

byte decPosition()
{
  if(stepsDone < minSteps)
    return 0;    
  --stepsDone;
  myStepper.step(stepsPerRevolution);
  store();
  return 1;
}

void store()
{
  EEPROM.write(stepsDone_eepromAddr, stepsDone);
  Serial.print(" Steps: ");
  Serial.println(stepsDone);  
}

bool changed = false;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Fill in the attributes for every accessory / function
// COPY - PASTE as many times as you have functions. The amount must be same as in line 18 above!
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void ConfigureDecoderFunctions() // The amount of accessories must be same as in line 26 above!
{
  accessory[0].address = 1024;
  accessory[0].mode = 1;

  accessory[1].address = 1025;
  accessory[1].mode = 1;
 
  accessory[2].address = 1026;
  accessory[2].mode = 1;

  // min position
  accessory[3].address = 1027;
  accessory[3].mode = 1;

  // max position
  accessory[4].address = 1028;
  accessory[4].mode = 1;

}  // END ConfigureDecoderFunctions

  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  // DCC accessory packet handler 
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  void BasicAccDecoderPacket_Handler(int address, boolean activate, byte data)
  {
    // Convert NMRA packet address format to human address
    address -= 1;
    address *= 4;
    address += 1;
    address += (data & 0x06) >> 1;

    boolean enable = (data & 0x01) ? 1 : 0;

    for (int i=0; i<maxaccessories; i++)
    {
      if (address == accessory[i].address)
      {
        if (enable) 
          accessory[i].dccstate = 1;
        else 
          accessory[i].dccstate = 0;
      }
    }
  } //END BasicAccDecoderPacket_Handler

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Setup (run once)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void setup() 
{   
  Serial.begin(9600);

  ConfigureDecoderFunctions();
  DCC.SetBasicAccessoryDecoderPacketHandler(BasicAccDecoderPacket_Handler, true);
  DCC.SetupDecoder( 0x00, 0x00, kDCC_INTERRUPT );
  pinMode(2,INPUT_PULLUP); // Interrupt 0 with internal pull up resistor (can get rid of external 10k)

  myStepper.setSpeed(60);
  stepsDone = (int) EEPROM.read(stepsDone_eepromAddr);
  //stepsDone = (int) readEEPROM(disk1, stepsDone_eepromAddr);
  if(stepsDone>=255)
  {
    stepsDone = 0;
    EEPROM.write(stepsDone_eepromAddr, stepsDone);
  }
  Serial.println("Initialized");
  Serial.print(" Steps: "); Serial.println(stepsDone);

  for(int i=3; i<20; i++)
  {
    pinMode(i, OUTPUT);
    digitalWrite(i, HIGH); //all function outputs are set to 0 at startup
  }
} // END setup

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Main loop (run continuous)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void loop()
{
  static int addr = 0;

  DCC.loop(); // Loop DCC library
    
  if( ++addr >= maxaccessories ) addr = 0; // Next address to test

  if (accessory[addr].dccstate)
  {
    switch (accessory[addr].mode)
    {
    case 1: // Continuous
      accessory[addr].onoff = 1;
      break;
    case 2: // Oneshot
      if (!accessory[addr].onoff && !accessory[addr].finished)
      {
        accessory[addr].onoff = 1;
        accessory[addr].offMilli = millis() + accessory[addr].ontime;
      }
      if (accessory[addr].onoff && millis() > accessory[addr].offMilli)
      {
        accessory[addr].onoff = 0;
        accessory[addr].finished = true; //this is reset to flase below in the 'else' statement
      }
      break;
    case 3: // Flasher, is always an 'alternating' flasher together with .outputPin2
      if (!accessory[addr].onoff && millis() > accessory[addr].onMilli)
      {
        accessory[addr].onoff = 1;
        accessory[addr].onoff2 = 0;
        accessory[addr].offMilli = millis() + accessory[addr].ontime;
      }
      if (accessory[addr].onoff && millis() > accessory[addr].offMilli)
      {
        accessory[addr].onoff = 0;
        accessory[addr].onoff2 = 1;
        accessory[addr].onMilli = millis() + accessory[addr].offtime;
      }
      break;
    case 4: // Signal
      accessory[addr].onoff = 1;
      accessory[addr].onoff2 = 0;
      break;
    }
  }
  else //accessory[addr].dccstate == 0
  {
    accessory[addr].onoff = 0;
    if (accessory[addr].mode == 4) accessory[addr].onoff2 = 1; else accessory[addr].onoff2 = 0;
    if (accessory[addr].mode == 2) accessory[addr].finished = false; // Oneshot finished by DCCstate, not by ontime
  }

  // activate outputpin, based on value of onoff

  byte nrOfActivations = 0;
  if(accessory[0].onoff)
    ++nrOfActivations;
  if(accessory[1].onoff)
    ++nrOfActivations;
  if(accessory[2].onoff)
    ++nrOfActivations;
  if(accessory[3].onoff)
    ++nrOfActivations;
  if(accessory[4].onoff)
    ++nrOfActivations;

  if(nrOfActivations > 1)
    return;  

  if(accessory[0].onoff)
  {
    changed = moveToPositionValue(pos0);
  }
  else if(accessory[1].onoff)
  {
    changed = moveToPositionValue(pos1);
  }
  else if(accessory[2].onoff)
  {
    changed = moveToPositionValue(pos2);
  }
  else if(accessory[3].onoff)
  {
    changed = moveToPositionValue(minSteps);
  }
  else if(accessory[4].onoff)
  {
    changed = moveToPositionValue(maxSteps);
  }

  accessory[0].onoff = 0;
  accessory[1].onoff = 0;
  accessory[2].onoff = 0;
  accessory[3].onoff = 0;
  accessory[4].onoff = 0;

  if(changed)
  {
    Serial.println(" ## DONE");
    store();
  }
  
  //if (accessory[addr].onoff)
  //  digitalWrite(accessory[addr].outputPin, LOW);
  //else 
  //  digitalWrite(accessory[addr].outputPin, HIGH);
  
  //if(accessory[addr].onoff2) 
  //  digitalWrite(accessory[addr].outputPin2, LOW);
  //else 
  //  digitalWrite(accessory[addr].outputPin2, HIGH);
  
} //END loop
