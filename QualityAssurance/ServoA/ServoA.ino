#include <Servo.h>

Servo myservo;

#define TEST

#define SECS_PER_MIN  (60UL)
#define SECS_PER_HOUR (3600UL)
#define SECS_PER_DAY  (SECS_PER_HOUR * 24L)

/* Useful Macros for getting elapsed time */
#define numberOfSeconds(_time_) (_time_ % SECS_PER_MIN)  
#define numberOfMinutes(_time_) ((_time_ / SECS_PER_MIN) % SECS_PER_MIN) 
#define numberOfHours(_time_) (( _time_% SECS_PER_DAY) / SECS_PER_HOUR)
#define elapsedDays(_time_) ( _time_ / SECS_PER_DAY)  

byte minimumPos = 32;
byte maximumPos = 45;
int pos = 32;
byte dt = 30;

int counter = 0;

void setup() 
{
  myservo.attach(9);

  Serial.begin(9600);
}

void WaitForSignal(int *value)
{
    while(Serial.available() <= 0)
      ;
    byte b = Serial.read();
    if(b == '-')
      *value = -1;
    else if(b == '+')
      *value = +1;    
    while(Serial.read() > 0)
      Serial.flush();
}

void time(long val){  
  int days = elapsedDays(val);
  int hours = numberOfHours(val);
  int minutes = numberOfMinutes(val);
  int seconds = numberOfSeconds(val);

  Serial.print(days, DEC);  
  printDigits(hours);  
  printDigits(minutes);
  printDigits(seconds);
}

void printDigits(byte digits){
  Serial.print(":");
  if(digits < 10)
    Serial.print('0');
  Serial.print(digits,DEC);  
}

void ShowPosition()
{
  Serial.print("Run: ");
  Serial.print(counter);
  Serial.print(", Time: ");
  time(millis() / 1000);
  Serial.print(", Position: ");
  Serial.println(pos);
}

void loop() 
{
#ifdef TEST
  ShowPosition();

  for(pos = minimumPos; pos < maximumPos; ++pos)
  {
      myservo.write(pos);
      delay(dt);
  }
  delay(2000);
  for(pos = maximumPos; pos >= minimumPos; --pos)
  {
    myservo.write(pos);
    delay(dt);  
  }
  delay(2000);
  
  ++counter;

  return;
#endif
  
  int v = 0;
  myservo.write(pos);
  delay(15);
  ShowPosition();
  WaitForSignal(&v);
  pos += v;
  if(pos <= minimumPos)
    pos = minimumPos;
  else if(pos >= maximumPos)
    pos = maximumPos;
}

