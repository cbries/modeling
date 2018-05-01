// How to upload to ATtiny: http://digistump.com/wiki/digispark
// https://rudysmodelrailway.wordpress.com/2015/10/15/use-an-attiny-with-usb-as-a-dcc-function-decoder/

#include <TinySoftPwm.h>

const int maxValue = 200;
const int minValue = 0;

#define LEFT_LED_PIN  0 
#define RIGHT_LED_PIN 1

void setup()
{
  /***********************************************************/
  /* Call TinySoftPwm_process() with a period of 60 us       */
  /* The PWM frequency = 128 x 60 # 7.7 ms -> F # 130Hz      */
  /* 128 is the first argument passed to TinySoftPwm_begin() */
  /***********************************************************/

  TinySoftPwm_begin(128, 0);
}

void loop()
{
  static uint32_t StartUs=micros();
  static uint32_t StartMs=millis();
  static uint8_t Pwm=0;
  static int8_t  Dir=1;

  if((micros() - StartUs) >= 60)
  {
    /* We arrived here every 60 microseconds */
    StartUs=micros();
    TinySoftPwm_process(); /* This function shall be called periodically (like here, based on micros(), or in a timer ISR) */
  }
  
  if((millis()-StartMs) >= 20)
  {
    StartMs = millis();
    Pwm += Dir; /* increment or decrement PWM depending of sign of Dir */
    TinySoftPwm_analogWrite(LEFT_LED_PIN, Pwm);
    TinySoftPwm_analogWrite(RIGHT_LED_PIN, maxValue - Pwm);

    if(Pwm >= maxValue) 
      Dir = -5;
    if(Pwm <= minValue)   
      Dir = +5;
  }
}


