
#include "SparkFun_Tlc5940.h"

void setup()
{
  randomSeed(analogRead(0));
  Serial.begin(9600);
  Tlc.init();
}

int getStepValue()
{
  if(Serial.available())
  {
    char c = (char) Serial.read();
    if(c == '+')
      return 1;
    if(c == '-')
      return -1;
  }
  return 0;
}

const int randomPortsN = 11;
int randomPorts[] = { 0, 1, 2, 3, 4, 5, 6, 7, 11, 13, 14 };
int randomPorts_[randomPortsN] = { -1 };

int alwaysOnPortsN = 4;
int alwaysOnPorts[] = { 8, 9, 10, 12 };

int runLoopCount = 0;

void loop()
{  
  Tlc.clear();
  for(int i = 0; i < alwaysOnPortsN; ++i)
    Tlc.set(alwaysOnPorts[i], 4095);

  for(int i = 0; i < randomPortsN; ++i)
    randomPorts_[i] = randomPorts[i];
    
  int j = random(1, randomPortsN / 2);
  for(int jj = 0; jj < j; ++jj)
  {
    int i = random(0, randomPortsN);
    int v = randomPorts_[i];
    if(v == -1)
      continue;
    Tlc.set(v, 2000);
    randomPorts_[i] = -1;
  }
  Tlc.update();

  if(runLoopCount > 5)
  {
    int waitFor = 10000 + (random(5, 20) * 1000);
    Serial.print("Wait (sec): "); Serial.println(waitFor / 1000);
    delay(waitFor);
  } 
  else
  {
    ++runLoopCount;
    delay(100);
  }  
}

