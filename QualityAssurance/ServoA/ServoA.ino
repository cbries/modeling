#include <Servo.h>

#define N 2

#define TEST

#define SECS_PER_MIN  (60UL)
#define SECS_PER_HOUR (3600UL)
#define SECS_PER_DAY  (SECS_PER_HOUR * 24L)

/* Useful Macros for getting elapsed time */
#define numberOfSeconds(_time_) (_time_ % SECS_PER_MIN)  
#define numberOfMinutes(_time_) ((_time_ / SECS_PER_MIN) % SECS_PER_MIN) 
#define numberOfHours(_time_) (( _time_% SECS_PER_DAY) / SECS_PER_HOUR)
#define elapsedDays(_time_) ( _time_ / SECS_PER_DAY)  

byte dt = 15;
int waitNext = 125;
int counter = 0;

struct Data
{
  byte minimumPos;
  byte maximumPos;
  int pos;
  byte portnr;
  byte up;
  Servo myservo;
  Data(byte minimum, byte maximum, int pos, byte portnr) {
    this->minimumPos = minimum;
    this->maximumPos = maximum;
    this->pos = pos;
    this->portnr = portnr;
    up = 1;
    myservo.attach(portnr);
    myservo.write(pos);
  }

  void Write()
  {
    myservo.write(pos);
    delay(dt);
  }  
} *data[N];

void setup() 
{   
// first version (Unterbau): min:=27, max:=40
// second version (Seitenanschluss): min:=23, max:=40

  //data[0] = new Data(23, 40, 23, 9);
  //data[1] = new Data(27, 40, 27, 8);

  data[0] = new Data(27, 40, 27, 8);
  data[1] = new Data(23, 40, 23, 9);

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

void ShowPosition2()
{
  for(byte i=0; i < N; ++i)
  {
    Serial.print(", P");
    Serial.print(i);
    Serial.print(": ");
    Serial.print(data[i]->pos);    
  }
  Serial.println("");
}

void ShowPosition()
{
  Serial.print("Run: ");
  Serial.print(counter);
  Serial.print(", Time: ");
  time(millis() / 1000);
  ShowPosition2();
}

void loop() 
{
#ifdef TEST
  //ShowPosition();

  for(byte i=0; i < N; ++i)
  {
    if(data[i]->up)
    {
      if(data[i]->pos <= data[i]->maximumPos)
      {
        data[i]->pos += 1;
      }
      else
      {
        data[i]->up = 0;
        data[i]->pos = data[i]->maximumPos;
      }
    }
    else
    {
      if(data[i]->pos >= data[i]->minimumPos)
      {
        data[i]->pos -= 1;
      }
      else
      {
        data[i]->up = 1;
        data[i]->pos = data[i]->minimumPos;  
      }
    } 

    data[i]->Write();;
  }
  //ShowPosition2();

  delay(waitNext);

/*
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
*/  
  ++counter;

  return;
#endif
/*  
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
*/
}

