#include <Stepper.h>
#include <EEPROM.h>

const int stepsPerRevolution = 500;
Stepper myStepper(stepsPerRevolution, 8, 9, 10, 11);

int stepsDone = 0;
int stepsDone_eepromAddr = 0;

int minSteps = 0;
int maxSteps = 35;

int pos0 = 5;
int pos1 = 18;
int pos2 = 31;

void setup()
{
  myStepper.setSpeed(60);
  Serial.begin(9600);
  stepsDone = (int) EEPROM.read(stepsDone_eepromAddr);
  if(stepsDone>=255)
  {
    stepsDone = 0;
    EEPROM.write(stepsDone_eepromAddr, stepsDone);
  }
  Serial.println("Initialized");
  Serial.print(" Steps: "); Serial.println(stepsDone);
}

bool moveToPositionValue(int steps)
{
  if(stepsDone > steps)
  {
    for(int i = stepsDone; i >= 0; --i)
    {
      decPosition();
      if(stepsDone == steps)
        return true;
    }
  }
  else if(stepsDone < steps)
  {
    for(int i = stepsDone; i < maxSteps; ++i)
    {
      incPosition();
      if(stepsDone == steps)
        return true;   
    }
  }

  return false;
}

void incPosition()
{
  if(stepsDone > maxSteps)
    return false;    
  ++stepsDone;
  myStepper.step(-stepsPerRevolution);
  store();
  return true;
}

bool decPosition()
{
  if(stepsDone < minSteps)
    return false;    
  --stepsDone;
  myStepper.step(stepsPerRevolution);
  store();
  return true;
}

void store()
{
  EEPROM.write(stepsDone_eepromAddr, stepsDone);
  Serial.print(" Steps: ");
  Serial.println(stepsDone);  
}

void loop() 
{
  bool changed = false;
  char dir = Serial.read();
  
  if(dir == '+')
  {
    incPosition();
    delay(1);
  }
  else if(dir == '-')
  {
    changed = decPosition();
    delay(1);
  }
  else if(dir == '0' || dir == '1' || dir == '2' || dir == '3' || dir == '4')
  {
    switch(dir)
    {
      case '0': changed = moveToPositionValue(minSteps); break; 
      case '1': changed = moveToPositionValue(pos0); break;
      case '2': changed = moveToPositionValue(pos1); break;
      case '3': changed = moveToPositionValue(pos2); break;
      case '4': changed = moveToPositionValue(maxSteps); break;
    }
  }

  if(changed)
  {
    Serial.println(" ## DONE");
    store();
  }
}
