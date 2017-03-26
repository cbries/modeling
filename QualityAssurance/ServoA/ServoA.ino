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

// first version (Unterbau): min:=27, max:=40
// second version (Seitenanschluss): min:= , max:= 

byte minimumPos = 23;
byte maximumPos = 40;
int pos = (maximumPos - minimumPos) / 2.0f; //32;
byte dt = 15;
int waitNext = 500;

int counter = 0;

void setup() 
{
  myservo.attach(9);
  myservo.write(minimumPos);

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

void ShowPosition2()
{
  Serial.print("  Position: ");
  Serial.println(pos);  
}

void loop() 
{
#ifdef TEST
  ShowPosition();

  for(pos = minimumPos; pos <= maximumPos; ++pos)
  {
      myservo.write(pos);
      delay(dt);
      ShowPosition2();
  }
  delay(waitNext);
  Serial.println(" --- ");
  for(pos = maximumPos; pos >= minimumPos; --pos)
  {
    myservo.write(pos);
    delay(dt);  
    ShowPosition2();
  }
  delay(waitNext);
  
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

