/*
 */
 
// WeMos D1 R1
// 80 MHz
// 4M (1M SPIFFS)
// v2 Lower Memory
// 115200

// Arduino Wemos D1
/*
static const uint8_t D0   = 16;
static const uint8_t D1   = 5;
static const uint8_t D2   = 4;
static const uint8_t D3   = 0;   SCL,  RED
static const uint8_t D4   = 2;
static const uint8_t D5   = 14;  SCK,  GREEN
static const uint8_t D6   = 12;  MISO, BLUE
static const uint8_t D7   = 13;  MOSI, WHITE
static const uint8_t D8   = 15;
static const uint8_t RX   = 3;
static const uint8_t TX   = 1;
*/

const char* ssid     = "";
const char* password = "";

#include <Arduino.h>

#include <ESP8266WiFi.h>
//#include <ESP8266WiFiMulti.h>
#include <WebSocketsServer.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <EEPROM.h>
#include <Hash.h>

#include "ArduinoJson.h"

int _BLUE = D6;
int _GREEN = D5;
int _RED = D3;
int _WHITE = D7;

int r = 0;
int g = 0;
int b = 0;
int w = 0;
int w1 = 0;
int w2 = 0;

#define LED_RED     _RED
#define LED_GREEN   _GREEN
#define LED_BLUE    _BLUE
#define LED_WHITE   _WHITE

#define USE_SERIAL Serial

//ESP8266WiFiMulti WiFiMulti;
ESP8266WebServer server(80);
WebSocketsServer webSocket = WebSocketsServer(81);

void EEPROMWriteInt(int p_address, int p_value)
{
  byte lowByte = ((p_value >> 0) & 0xFF);
  byte highByte = ((p_value >> 8) & 0xFF);
  EEPROM.write(p_address, lowByte);
  EEPROM.write(p_address + 1, highByte);
}

unsigned int EEPROMReadInt(int p_address)
{
  byte lowByte = EEPROM.read(p_address);
  byte highByte = EEPROM.read(p_address + 1);
  return ((lowByte << 0) & 0xFF) + ((highByte << 8) & 0xFF00);
}     

void storeValues()
{
  EEPROMWriteInt(0, r);
  EEPROMWriteInt(5, g);
  EEPROMWriteInt(10, b);
  EEPROMWriteInt(15, w);
  EEPROM.commit();
}

void restoreValues()
{
  r = EEPROMReadInt(0);
  g = EEPROMReadInt(5);
  b = EEPROMReadInt(10);
  w = EEPROMReadInt(15);
}

void ShowValues()
{
  analogWrite(LED_WHITE, w);
  analogWrite(LED_RED, r);
  analogWrite(LED_GREEN, g);
  analogWrite(LED_BLUE, b);
  
  USE_SERIAL.print("RED:   "); USE_SERIAL.println(r);
  USE_SERIAL.print("Green: "); USE_SERIAL.println(g);
  USE_SERIAL.print("BLUE:  "); USE_SERIAL.println(b);    
  USE_SERIAL.print("WHITE: "); USE_SERIAL.println(w);
}

void webSocketEvent(uint8_t num, WStype_t type, uint8_t * payload, size_t length) 
{ 
    switch(type) 
    {
        case WStype_DISCONNECTED:
            USE_SERIAL.printf("[%u] Disconnected!\n", num);
            storeValues();
            break;
        case WStype_CONNECTED: {
            IPAddress ip = webSocket.remoteIP(num);
            USE_SERIAL.printf("[%u] Connected from %d.%d.%d.%d url: %s\n", num, ip[0], ip[1], ip[2], ip[3], payload);
            webSocket.sendTXT(num, "Connected");
            restoreValues();
            ShowValues();            
        }
            break;
        case WStype_TEXT:
            USE_SERIAL.printf("[%u] get Text: %s\n", num, payload);

            StaticJsonBuffer<200> jsonBuffer;
            JsonObject& root = jsonBuffer.parseObject(payload);
            r = root["r"];
            g = root["g"];
            b = root["b"];
            w = root["w"];

            ShowValues();            

            break;
    }
}

void setup() 
{
  EEPROM.begin(512);
  delay(10);

  restoreValues();

  pinMode(LED_RED, OUTPUT);
  pinMode(LED_GREEN, OUTPUT);
  pinMode(LED_BLUE, OUTPUT);
  pinMode(LED_WHITE, OUTPUT);

  digitalWrite(LED_RED, 1);
  digitalWrite(LED_GREEN, 1);
  digitalWrite(LED_BLUE, 1);
  digitalWrite(LED_WHITE, 1);

  ShowValues();

  USE_SERIAL.begin(115200);
  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();

  for(uint8_t t = 4; t > 0; t--) 
  {
    USE_SERIAL.printf("[SETUP] BOOT WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }

  IPAddress ip(192, 168, 1, 62);
  IPAddress gateway(192, 168, 1, 1); 
  Serial.print(F("Setting static ip to : "));
  Serial.println(ip);
  IPAddress subnet(255, 255, 255, 0);
  WiFi.config(ip, gateway, subnet);
  WiFi.begin(ssid, password);

  while(WiFi.status() != WL_CONNECTED) {
    Serial.print(".");
    delay(100);
  }
  Serial.println("<>");

  webSocket.begin();
  webSocket.onEvent(webSocketEvent);

  if(MDNS.begin("esp8266")) {
    USE_SERIAL.println("MDNS responder started");
  }

  server.on("/", []() {
      
  char buf[2048] = {0};
  sprintf(buf, "<html><head>" \
        "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"> " \
        "<script>var connection = new WebSocket('ws://'+location.hostname+':81/'," \
        "['arduino']); /*connection.onopen = function () { connection.send('Connect ' + new Date()); };*/ " \ 
        "connection.onerror = function (error) {    console.log('WebSocket Error ', error);};connection.onmessage " \
        "= function (e) {  console.log('Server: ', e.data);};function sendRGB() {  " \
          "var r = parseInt(document.getElementById('r').value);  " \
          "var g = parseInt(document.getElementById('g').value);  " \
          "var b = parseInt(document.getElementById('b').value);  " \
          "var w = parseInt(document.getElementById('w').value);  " \
          "var dataToSend = new Object(); " \
          "dataToSend.r = parseInt(r); " \
          "dataToSend.g = parseInt(g); " \
          "dataToSend.b = parseInt(b); " \
          "dataToSend.w = parseInt(w); " \
          "var data = JSON.stringify(dataToSend); " \
          "console.log('DATA:  ' + data); connection.send(data); } " \
          "</script></head>" \
          "<body>LED Control:<br/><br/>" \
           "R: <input id=\"r\" value=\"%i\" type=\"range\" min=\"0\" max=\"%i\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "G: <input id=\"g\" value=\"%i\" type=\"range\" min=\"0\" max=\"%i\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "B: <input id=\"b\" value=\"%i\" type=\"range\" min=\"0\" max=\"%i\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "W: <input id=\"w\" value=\"%i\" type=\"range\" min=\"0\" max=\"%i\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "</body></html>",
          (int)r, PWMRANGE, (int)g, PWMRANGE, (int)b, PWMRANGE, (int)w, PWMRANGE);
      
        server.send(200, "text/html", buf);
  });

  server.begin();

  MDNS.addService("http", "tcp", 80);
  MDNS.addService("ws", "tcp", 81);
}

void loop() 
{
  webSocket.loop();
  server.handleClient();
}

