//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DCC Function Decoder
// Author: Ruud Boer - October 2015
// This sketch turns an Arduino into a DCC function decoder for F0 - F12
// Output pins used: 3-19 (14-19 = A0-A5). Pin is HIGH when Function is ON.
// The DCC signal is fed to pin 2 (=Interrupt 0).
// Optocoupler schematics for conversion of DCC - 5V: www.rudysmodelrailway.wordpress.com/software
// Many thanks to www.mynabay.com for publishing their DCC monitor and -decoder code.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Pin2 (7) := INT0

// Pin3 (2) := In1
// Pin4 (3) := In2
// Pin1 (6) := In3
// Pin0 (7) := In4

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// IMPORTANT: GOTO lines 15 - 28 to configure some data!
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

int decoderAddress = 1830; // This is the decoder address, change into the number you want.
#define F0_pin 0 // Define the output pin for every Function number in use
#define F0_pin2 0 // 2nd pin for same function is possible. Can use forward / reverse direction ... see line 97.

#define F1_pin 0 // Available pin numbers: 0,1,3,4,5
#define F2_pin 0
#define F3_pin 0
#define F4_pin 0
#define F5_pin 0
#define F6_pin 0
#define F7_pin 0
#define F8_pin 0
#define F9_pin 0
#define F10_pin 0
#define F11_pin 0
#define F12_pin 0

#include <Stepper.h>
#include <TinyWireM.h>
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

byte Func[4]; //0=L4321, 1=8765, 2=CBA9, 3=F20-F13, 4=F28-F21
byte instrByte1;
#define disk1 0x50
int Address;
byte forw_rev=1; //0=reverse, 1=forward

void writeEEPROM(int deviceaddress, unsigned int eeaddress, byte data ) 
{
  TinyWireM.beginTransmission(deviceaddress);
  TinyWireM.send((int)(eeaddress >> 8));   // MSB
  TinyWireM.send((int)(eeaddress & 0xFF)); // LSB
  TinyWireM.send(data);
  TinyWireM.endTransmission();
 
  delay(5);
}
 
byte readEEPROM(int deviceaddress, unsigned int eeaddress ) 
{
  byte rdata = 0xFF;
 
  TinyWireM.beginTransmission(deviceaddress);
  TinyWireM.send((int)(eeaddress >> 8));   // MSB
  TinyWireM.send((int)(eeaddress & 0xFF)); // LSB
  TinyWireM.endTransmission();
 
  TinyWireM.requestFrom(deviceaddress,1);
 
  if (TinyWireM.available()) rdata = TinyWireM.receive();
 
  return rdata;
}

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
  //EEPROM.write(stepsDone_eepromAddr, stepsDone);
  writeEEPROM(disk1, stepsDone_eepromAddr, stepsDone);
  //Serial.println(stepsDone);  
}

bool changed = false;

boolean RawPacket_Handler(byte pktByteCount, byte* dccPacket) 
{
  changed = false;

  Address=0;
  if (!bitRead(dccPacket[0],7)) { //bit7=0 -> Loc Decoder Short Address
    Address = dccPacket[0];
    instrByte1 = dccPacket[1];
  }
  else if (bitRead(dccPacket[0],6)) { //bit7=1 AND bit6=1 -> Loc Decoder Long Address
    Address = 256 * (dccPacket[0] & B00000111) + dccPacket[1];
    instrByte1 = dccPacket[2];
  }

  if (Address==decoderAddress) {
    byte instructionType = instrByte1>>5;
    switch (instructionType) {
      case 2: // Reverse speed
        forw_rev=0;
        break;
      case 3: // Forward speed
        forw_rev=1;
        break;
      case 4: // Loc Function L-4-3-2-1
        Func[0]=instrByte1&B00011111;
        break;
      case 5: // Loc Function 8-7-6-5
        if (bitRead(instrByte1,4)) {
          Func[1]=instrByte1&B00001111;
        }
        else { // Loc Function 12-11-10-9
          Func[2]=instrByte1&B00001111;
        }
        break;
    }

    // F0 is an example of two output pins that alternate based on loc forw_rev driving direction.
//    if (Func[0]&B00010000) {digitalWrite(F0_pin,forw_rev); digitalWrite(F0_pin2,!forw_rev);} else digitalWrite(F0_pin,HIGH);
//    if (Func[0]&B00000001) digitalWrite(F1_pin,LOW); else digitalWrite(F1_pin,HIGH);
//    if (Func[0]&B00000010) digitalWrite(F2_pin,LOW); else digitalWrite(F2_pin,HIGH);
//    if (Func[0]&B00000100) digitalWrite(F3_pin,LOW); else digitalWrite(F3_pin,HIGH);

    if(Func[0]&B00000001)
    {
      changed = moveToPositionValue(pos0);
    }
    else if(Func[0]&B00000010)
    {
      changed = moveToPositionValue(pos1);
    }
    else if(Func[0]&B00000100)
    {
      changed = moveToPositionValue(pos2);
    }

    if(changed)
    {
      store();
    }

/*    
    if (Func[0]&B00001000) digitalWrite(F4_pin,LOW); else digitalWrite(F4_pin,HIGH);
    if (Func[1]&B00000001) digitalWrite(F5_pin,LOW); else digitalWrite(F5_pin,HIGH);
    if (Func[1]&B00000010) digitalWrite(F6_pin,LOW); else digitalWrite(F6_pin,HIGH);
    if (Func[1]&B00000100) digitalWrite(F7_pin,LOW); else digitalWrite(F7_pin,HIGH);
    if (Func[1]&B00001000) digitalWrite(F8_pin,LOW); else digitalWrite(F8_pin,HIGH);
    if (Func[2]&B00000001) digitalWrite(F9_pin,LOW); else digitalWrite(F9_pin,HIGH);
    if (Func[2]&B00000010) digitalWrite(F10_pin,LOW); else digitalWrite(F10_pin,HIGH);
    if (Func[2]&B00000100) digitalWrite(F11_pin,LOW); else digitalWrite(F11_pin,HIGH);
    if (Func[2]&B00001000) digitalWrite(F12_pin,LOW); else digitalWrite(F12_pin,HIGH);
*/
  }
}

void setup() {
  TinyWireM.begin();
  DCC.SetRawPacketHandler(RawPacket_Handler);
  DCC.SetupMonitor( kDCC_INTERRUPT );
  pinMode(0, OUTPUT);
  pinMode(1, OUTPUT);
  pinMode(3, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);

  myStepper.setSpeed(60);
  //Serial.begin(9600);
  //stepsDone = (int) EEPROM.read(stepsDone_eepromAddr);
  stepsDone = (int) readEEPROM(disk1, stepsDone_eepromAddr);
  if(stepsDone>=255)
  {
    stepsDone = 0;
    writeEEPROM(disk1, stepsDone_eepromAddr, stepsDone);
  }
  //Serial.println(stepsDone);
}

void loop() {
  DCC.loop();
}
