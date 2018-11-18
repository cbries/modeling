const char* ssid      = "";
const char* password  = "";

// .\SetIoTSteckdose.exe --host=ws://192.168.178.62:81 --in=in4 --state=off

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
static const uint8_t D3   = 0;   SCL,  IN1
static const uint8_t D4   = 2;         IN2
static const uint8_t D5   = 14;  SCK,  IN3
static const uint8_t D6   = 12;  MISO, IN4
static const uint8_t D7   = 13;  MOSI
static const uint8_t D8   = 15;
static const uint8_t RX   = 3;
static const uint8_t TX   = 1;
*/

#include <Arduino.h>

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsServer.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <EEPROM.h>
#include <Hash.h>

#include "ArduinoJson.h"

void SetupWifi()
{
  IPAddress ip(192, 168, 178, 62);
  IPAddress gateway(192, 168, 178, 1); 
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
}

int PortIN1 = D2;
int PortIN2 = D3;
int PortIN3 = D4;
int PortIN4 = D5;

bool in1 = false;
bool in2 = false;
bool in3 = false;
bool in4 = false;

ESP8266WiFiMulti WiFiMulti;
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
  EEPROMWriteInt(0, in1 ? 1 : 0);
  EEPROMWriteInt(5, in2 ? 1 : 0);
  EEPROMWriteInt(10, in3 ? 1 : 0);
  EEPROMWriteInt(15, in4 ? 1 : 0);
  EEPROM.commit();
}

void restoreValues()
{
  int v[4];
  v[0] = EEPROMReadInt(0);
  v[1] = EEPROMReadInt(5);
  v[2] = EEPROMReadInt(10);
  v[3] = EEPROMReadInt(15);

  in1 = v[0] == 1;
  in2 = v[1] == 1;
  in3 = v[2] == 1;
  in4 = v[3] == 1;
}

void ShowValues()
{
  digitalWrite(PortIN1, in1 ? HIGH : LOW);
  digitalWrite(PortIN2, in2 ? HIGH : LOW);
  digitalWrite(PortIN3, in3 ? HIGH : LOW);
  digitalWrite(PortIN4, in4 ? HIGH : LOW);
}

void webSocketEvent(uint8_t num, WStype_t type, uint8_t * payload, size_t length) 
{ 
    switch(type) 
    {
        case WStype_DISCONNECTED:
            storeValues();
            break;

        case WStype_CONNECTED: {
            IPAddress ip = webSocket.remoteIP(num);
            webSocket.sendTXT(num, "Connected");
            restoreValues();
            ShowValues();            
        }
            break;
        case WStype_TEXT:
            StaticJsonBuffer<200> jsonBuffer;
            JsonObject& root = jsonBuffer.parseObject(payload);

            Serial.println(payload);

            int v1 = root["in1"];
            int v2 = root["in2"];
            int v3 = root["in3"];
            int v4 = root["in4"];

            Serial.print("v1: "); Serial.println(v1);
            Serial.print("v2: "); Serial.println(v2);
            Serial.print("v3: "); Serial.println(v3);
            Serial.print("v4: "); Serial.println(v4);

            if(v1 != -1) in1 = root["in1"] == 1;
            if(v2 != -1) in2 = root["in2"] == 1;
            if(v3 != -1) in3 = root["in3"] == 1;
            if(v4 != -1) in4 = root["in4"] == 1;

            ShowValues();            

            break;
    }
}

void setup() 
{
  pinMode(PortIN1, OUTPUT);
  pinMode(PortIN2, OUTPUT);
  pinMode(PortIN3, OUTPUT);
  pinMode(PortIN4, OUTPUT);

  in1 = false;
  in2 = false;
  in3 = false;
  in4 = false;

  ShowValues();

  Serial.begin(115200);

  EEPROM.begin(512);
  delay(10);

  storeValues();
  delay(10);

  SetupWifi();

  webSocket.begin();
  webSocket.onEvent(webSocketEvent);

  if(MDNS.begin("esp8266")) { }

  server.on("/", []() {
      
  char buf[2048] = {0};
  sprintf(buf, "<html><head>" \
        "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"> " \
        "<script>var connection = new WebSocket('ws://'+location.hostname+':81/'," \
        "['arduino']); /*connection.onopen = function () { connection.send('Connect ' + new Date()); };*/ " \ 
        "connection.onerror = function (error) {    console.log('WebSocket Error ', error);};connection.onmessage " \
        "= function (e) {  console.log('Server: ', e.data);};function sendState() {  " \
          "var in1 = parseInt(document.getElementById('in1').checked ? 1 : 0 );  " \
          "var in2 = parseInt(document.getElementById('in2').checked ? 1 : 0 );  " \
          "var in3 = parseInt(document.getElementById('in3').checked ? 1 : 0 );  " \
          "var in4 = parseInt(document.getElementById('in4').checked ? 1 : 0 );  " \
          "var dataToSend = new Object(); " \
          "dataToSend.in1 = parseInt(in1); " \
          "dataToSend.in2 = parseInt(in2); " \
          "dataToSend.in3 = parseInt(in3); " \
          "dataToSend.in4 = parseInt(in4); " \
          "var data = JSON.stringify(dataToSend); " \
          "console.log('DATA:  ' + data); connection.send(data); } " \
          "</script></head>" \
          "<body>Steckdosen:<br/><br/>" \
           "IN1: <input id=\"in1\" type=\"checkbox\" onclick=\"sendState();\" %s/><br/>" \
           "IN2: <input id=\"in2\" type=\"checkbox\" onclick=\"sendState();\" %s/><br/>" \
           "IN3: <input id=\"in3\" type=\"checkbox\" onclick=\"sendState();\" %s/><br/>" \
           "IN4: <input id=\"in4\" type=\"checkbox\" onclick=\"sendState();\" %s/><br/>" \
           "</body></html>",
          in1 ? "checked" : "", in2 ? "checked" : "", in3 ? "checked" : "", in4 ? "checked" : "");
      
        server.send(200, "text/html", buf);
  });

  server.begin();

  MDNS.addService("http", "tcp", 80);
  MDNS.addService("ws", "tcp", 81);

  ShowValues();
}

void loop() 
{
  webSocket.loop();
  server.handleClient();
}

