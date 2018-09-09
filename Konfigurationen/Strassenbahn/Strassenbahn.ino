
#include <EEPROM.h>

#define PIN_MAIN_A 2
#define PIN_MAIN_B 3
#define PIN_RELAIS_A_IN1 4
#define PIN_RELAIS_A_IN2 5
#define PIN_RELAIS_B_IN1 6
#define PIN_RELAIS_B_IN2 7

#define SecondsToWait 30
#define OffsetMin 0
#define OffsetMax 10

#define LEFT 0
#define RIGHT 1

int addr = 0;
int mode = LEFT;

void switchAllOff()
{
  digitalWrite(PIN_MAIN_A, HIGH);
  digitalWrite(PIN_MAIN_B, HIGH);
  digitalWrite(PIN_RELAIS_A_IN1, HIGH);
  digitalWrite(PIN_RELAIS_A_IN2, HIGH);
  digitalWrite(PIN_RELAIS_B_IN1, HIGH);
  digitalWrite(PIN_RELAIS_B_IN2, HIGH);  

  delay(500);
}

void setup() 
{
  randomSeed(analogRead(0));

  Serial.begin(9600);
  
  pinMode(PIN_MAIN_A, OUTPUT);
  pinMode(PIN_MAIN_B, OUTPUT);
  pinMode(PIN_RELAIS_A_IN1, OUTPUT);
  pinMode(PIN_RELAIS_A_IN2, OUTPUT);
  pinMode(PIN_RELAIS_B_IN1, OUTPUT);
  pinMode(PIN_RELAIS_B_IN2, OUTPUT);

  switchAllOff();

  mode = EEPROM.read(addr);

  Serial.println("Setup done!");
}

void loop() 
{ 
  while(1)
  {
    Serial.print("MODE: "); Serial.println(mode == LEFT ? "LEFT" : "RIGHT");
    
    if(mode == LEFT)
    {
      delay(500);

      digitalWrite(PIN_RELAIS_B_IN1, LOW);
      digitalWrite(PIN_RELAIS_B_IN2, LOW);

      delay(500);      

      digitalWrite(PIN_MAIN_A, LOW);
      digitalWrite(PIN_MAIN_B, LOW);

      delay(500);      
    }
    else
    {    
      delay(500);

      digitalWrite(PIN_RELAIS_A_IN1, LOW);
      digitalWrite(PIN_RELAIS_A_IN2, LOW);

      delay(500);      

      digitalWrite(PIN_MAIN_A, LOW);
      digitalWrite(PIN_MAIN_B, LOW);

      delay(500);      
    }
    
    delay(SecondsToWait * 1000UL);
    int offset = random(OffsetMin, OffsetMax);
    delay(offset * 1000UL);

    if(mode == LEFT) mode = RIGHT;
    else             mode = LEFT;

    switchAllOff();

    EEPROM.write(addr, mode);
  }
}
