//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Arduino DCC Servo and Function Decoder
// Version: 1.4 - 2015-04-23
// Author: Ruud Boer
// This sketch turns an Arduino into a DCC decoder with max 12 servo motor outputs combined with function outputs.
// The DCC signal is optically separated and fed to pin 2 (=Interrupt 0). Schematics: www.mynabay.com
// Many thanks to www.mynabay.com for publishing their DCC monitor and -decoder code, which is used in this sketch.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#define portOfPin(P)\
  (((P)>=0&&(P)<8)?&PORTD:(((P)>7&&(P)<14)?&PORTB:&PORTC))
#define ddrOfPin(P)\
  (((P)>=0&&(P)<8)?&DDRD:(((P)>7&&(P)<14)?&DDRB:&DDRC))
#define pinOfPin(P)\
  (((P)>=0&&(P)<8)?&PIND:(((P)>7&&(P)<14)?&PINB:&PINC))
#define pinIndex(P)((uint8_t)(P>13?P-14:P&7))
#define pinMask(P)((uint8_t)(1<<pinIndex(P)))

#define pinAsInput(P) *(ddrOfPin(P))&=~pinMask(P)
#define pinAsInputPullUp(P) *(ddrOfPin(P))&=~pinMask(P);digitalHigh(P)
#define pinAsOutput(P) *(ddrOfPin(P))|=pinMask(P)
#define digitalLow(P) *(portOfPin(P))&=~pinMask(P)
#define digitalHigh(P) *(portOfPin(P))|=pinMask(P)
#define isHigh(P)((*(pinOfPin(P))& pinMask(P))>0)
#define isLow(P)((*(pinOfPin(P))& pinMask(P))==0)
#define digitalState(P)((uint8_t)isHigh(P))

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// IMPORTANT: GOTO lines 23 and 44 to configure some data!
// IMPORTANT: To avoid servo movement and possible high current draw at startup:
// - First start the Arduino, the software now sets the servo angle values to 'offangle'.
// - After a few seconds, switch the servo power on ... they will possibly show just a minor jitter.
// - This only works if you set all servo's to offangle before shutdown!
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#include <DCC_Decoder.h>
#include <Servo.h> 
#define kDCC_INTERRUPT 0

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Fill in these 2 values ...
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
const byte maxservos = 6; //The number of servos you have connected to this Arduino
const byte servotimer = 60; //Servo angle change timer. Lower value -> higher speed
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

unsigned long timetoupdatesetpoint = millis() + servotimer;

struct servoItem {
  int address; // DCC address to respond to
  byte output; // State of DCC accessory: 1=on, 0=off (ECoS: on=straight, off=turnout)
  byte outputPin; // Arduino output pin for additional function (not where servo is attached to)
  byte angle;
  byte setpoint;
  byte offangle;
  byte onangle;
  Servo servo;
};

servoItem servos[maxservos];

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Fill in the data for every function and servo.
// COPY - PASTE as many times as you have functions. The amount must be same as in line 22 above!
// A servo is coupled to an accessory[n]. It rotates based on accessory[n].output = 1 (CCW) or 0 (CW)
// If you have multiple servos you need to couple them to different accessories. However ...
// accessories may switch the same output pin (e.g. pin 13, which has the on board led attached)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void ConfigureFunctionsAndServos()
{
  servos[0].address = 1; // DCC address for this accessory
  servos[0].outputPin = 13; // Arduino pin number for additional function output (not where servo is attached to)
  servos[0].servo.attach(3); //Arduino pin number where servo is connected to
  servos[0].offangle = 25; //Angle for DCC=off. For ECoS turnout is DCC off, straight is DCC on.
  servos[0].onangle = 160; //Angle for DCC=on. For ECoS turnout is DCC off, straight is DCC on.
 
  servos[1].address = 2;
  servos[1].outputPin = 13;
  servos[1].servo.attach(4);
  servos[1].offangle = 25;
  servos[1].onangle = 160;

  servos[2].address = 3;
  servos[2].outputPin = 13;
  servos[2].servo.attach(5);
  servos[2].offangle = 26;
  servos[2].onangle = 160;

  servos[3].address = 4;
  servos[3].outputPin = 13;
  servos[3].servo.attach(6);
  servos[3].offangle = 25;
  servos[3].onangle = 160;

  servos[4].address = 5;
  servos[4].outputPin = 13;
  servos[4].servo.attach(7);
  servos[4].offangle = 25;
  servos[4].onangle = 160;

  servos[5].address = 6;
  servos[5].outputPin = 13;
  servos[5].servo.attach(8);
  servos[5].offangle = 25;
  servos[5].onangle = 160;
/*
  servos[6].address = 7;
  servos[6].outputPin = 13;
  servos[6].servo.attach(9);
  servos[6].offangle = 46;
  servos[6].onangle = 128;

  servos[7].address = 8;
  servos[7].outputPin = 13;
  servos[7].servo.attach(10);
  servos[7].offangle = 64;
  servos[7].onangle = 124;

  servos[8].address = 9;
  servos[8].outputPin = 13;
  servos[8].servo.attach(11);
  servos[8].offangle = 50;
  servos[8].onangle = 138;

  servos[9].address = 10;
  servos[9].outputPin = 13;
  servos[9].servo.attach(12);
  servos[9].offangle = 124;
  servos[9].onangle = 64;
*/
} // END ConfigureFunctionsAndServos()

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DCC packet handler 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void BasicAccDecoderPacket_Handler(int address, boolean activate, byte data)
{
  // Convert NMRA packet address format to human address
  address -= 1;
  address *= 4;
  address += 1;
  address += (data & 0x06) >> 1;

  boolean enable = (data & 0x01) ? 1 : 0;

  for(int i=0; i<maxservos; i++)
	{
    if(address == servos[i].address)
		{
      if(enable) 
        servos[i].output = 1;
      else 
        servos[i].output = 0;
    }
  }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Setup (run once)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void setup() 
{ 
  DCC.SetBasicAccessoryDecoderPacketHandler(BasicAccDecoderPacket_Handler, true);
  
  ConfigureFunctionsAndServos();

  for(int i=0; i<maxservos; i++)
  {
    pinMode (servos[i].outputPin, OUTPUT );
    digitalWrite (servos[i].outputPin, LOW);
    servos[i].angle = servos[i].offangle;
  }

  DCC.SetupDecoder( 0x00, 0x00, kDCC_INTERRUPT );
  pinMode(2, INPUT_PULLUP); //Interrupt 0 with internal pull up resistor (can get rid of external 10k)
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Main loop (run continuous)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void loop()
{
  static int addr = 0;

  DCC.loop();

  if(++addr >= maxservos) 
    addr = 0; 

  // Set servos output pin
  if (servos[addr].output) 
    digitalWrite(servos[addr].outputPin, HIGH);
  else 
    digitalWrite(servos[addr].outputPin, LOW);
/*
  if (servos[addr].output) 
    digitalHigh(servos[addr].outputPin);
  else 
    digitalLow(servos[addr].outputPin); 
*/  
  // Every 'servotimer' ms, modify setpoints and move servos 1 step (if needed)
  if (millis() > timetoupdatesetpoint)
	{
    timetoupdatesetpoint = millis() + servotimer;

    for (int n=0; n<maxservos; n++)
		{
      if (servos[n].output) 
        servos[n].setpoint=servos[n].onangle;
      else 
        servos[n].setpoint=servos[n].offangle;

      if (servos[n].angle < servos[n].setpoint) 
        servos[n].angle++;
      if (servos[n].angle > servos[n].setpoint) 
        servos[n].angle--;
      
      servos[n].servo.write(servos[n].angle);
    }
  }

} //END MAIN LOOP
