const char* ssid      = "";
const char* password  = "";

#include<Arduino.h>

// IP ADDRESSES / GATEWAY
const uint8_t ipaddress[4] = { 192, 168, 178, 64 };
const uint8_t gateaddress[4] = { 192, 168, 178, 1 };

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

#include<ESP8266WiFi.h>
#include<WebSocketsServer.h>
#include<ESP8266WebServer.h>
#include<ESP8266mDNS.h>
#include<EEPROM.h>
#include<Wire.h>
#include<Hash.h>

#include "ArduinoJson.h"

#define USE_SERIAL Serial
#define INT_PIN 13

// Gyroscope
const uint8_t MPU_addr = 0x68; // I2C address of the MPU-6050
// DHT*
#include "DHTesp.h"
#define DHT_PIN 16
DHTesp dht;
// Hall-Sensor
volatile int _interruptCounter = 0;

struct SensorData
{
#define SENSORDATA_SIZE 512
  
  DHTesp::DHT_ERROR_t dhtStatus;
  float dhtHumidity;
  float dhtTemperature;
  float dhtFahrenheit;
  float dhtHeatIndexCelsius;
  float dhtHeatIndexFahrenheit;

  int16_t AcX,AcY,AcZ;
  int16_t Tmp;
  int16_t GyX,GyY,GyZ;

  int numberOfInterrupts;

public:
  // https://arduinojson.org/v5/faq/whats-the-best-way-to-use-the-library/
  void serialize(char* json, size_t maxSize)
  {
    StaticJsonBuffer<SENSORDATA_SIZE> jsonBuffer;
    JsonObject& root = jsonBuffer.createObject();
    root["dhtStatus"] = (int) dhtStatus;
    root["dhtHumidity"] = dhtHumidity;
    root["dhtTemperature"] = dhtTemperature;
    root["dhtFahrenheit"] = dhtFahrenheit;
    root["dhtHeatIndexCelsius"] = dhtHeatIndexCelsius;
    root["dhtHeatIndexFahrenheit"] = dhtHeatIndexFahrenheit;
    root["AcX"] = AcX;
    root["AcY"] = AcY;
    root["AcZ"] = AcZ;
    root["Tmp"] = Tmp;
    root["GyX"] = GyX;
    root["GyY"] = GyY;
    root["GyZ"] = GyZ;
    root["numberOfInterrupts"] = numberOfInterrupts;
    root.printTo(json, maxSize);
  }

  // https://arduinojson.org/v5/faq/whats-the-best-way-to-use-the-library/
  bool deserialize(SensorData& data, char* json)
  {
    StaticJsonBuffer<SENSORDATA_SIZE> jsonBuffer;
    JsonObject& root = jsonBuffer.parseObject(json);
    dhtStatus = root["dhtStatus"];
    dhtHumidity = root["dhtHumidity"];
    dhtTemperature = root["dhtTemperature"];
    dhtFahrenheit = root["dhtFahrenheit"];
    dhtHeatIndexCelsius = root["dhtHeatIndexCelsius"];
    dhtHeatIndexFahrenheit = root["dhtHeatIndexFahrenheit"];
    AcX = root["AcX"];
    AcY = root["AcY"];
    AcZ = root["AcZ"];
    Tmp = root["Tmprature"];
    GyX = root["GyX"];
    GyY = root["GyY"];
    GyZ = root["GyZ"];
    numberOfInterrupts = root["numberOfInterrupts"];
    return root.success();
  }
};

// runtime
SensorData _data;
long _previousMillis = 0;
long _intervallMillis = 0;

ESP8266WiFiMulti WiFiMulti;
ESP8266WebServer server(80);
WebSocketsServer webSocket = WebSocketsServer(81);

// interrupt handler
void handleInterrupt() {
  _interruptCounter++;
}

void updateData()
{
  long currentMillis = millis();
  if(currentMillis - _previousMillis > _intervallMillis)
  {
    _previousMillis = currentMillis;
    _data.dhtStatus = dht.getStatus();
    _data.dhtHumidity = dht.getHumidity();
    _data.dhtTemperature = dht.getTemperature(); 
    _data.dhtFahrenheit = dht.toFahrenheit(_data.dhtTemperature);
    _data.dhtHeatIndexCelsius = dht.computeHeatIndex(_data.dhtTemperature, _data.dhtHumidity, false);
    _data.dhtHeatIndexFahrenheit = dht.computeHeatIndex(dht.toFahrenheit(_data.dhtTemperature), _data.dhtHumidity, true);
  }
  
  if(_interruptCounter > 0)
  {
      _data.numberOfInterrupts += _interruptCounter;
      _interruptCounter = 0;
  }

  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);  // starting with register 0x3B (ACCEL_XOUT_H)
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr, (size_t)14, true);  // request a total of 14 registers
  _data.AcX = Wire.read()<<8|Wire.read();  // 0x3B (ACCEL_XOUT_H) & 0x3C (ACCEL_XOUT_L)    
  _data.AcY = Wire.read()<<8|Wire.read();  // 0x3D (ACCEL_YOUT_H) & 0x3E (ACCEL_YOUT_L)
  _data.AcZ = Wire.read()<<8|Wire.read();  // 0x3F (ACCEL_ZOUT_H) & 0x40 (ACCEL_ZOUT_L)
  _data.Tmp = Wire.read()<<8|Wire.read();  // 0x41 (TEMP_OUT_H) & 0x42 (TEMP_OUT_L)
  _data.GyX = Wire.read()<<8|Wire.read();  // 0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)
  _data.GyY = Wire.read()<<8|Wire.read();  // 0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)
  _data.GyZ = Wire.read()<<8|Wire.read();  // 0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)
  /*
  Serial.print("AcX = "); Serial.print(AcX);
  Serial.print(" | AcY = "); Serial.print(AcY);
  Serial.print(" | AcZ = "); Serial.print(AcZ);
  Serial.print(" | Tmp = "); Serial.print(Tmp/340.00+36.53);  //equation for temperature in degrees C from datasheet
  Serial.print(" | GyX = "); Serial.print(GyX);
  Serial.print(" | GyY = "); Serial.print(GyY);
  Serial.print(" | GyZ = "); Serial.println(GyZ);
  */
}

void SetupWifi()
{
  IPAddress ip(ipaddress[0], ipaddress[1], ipaddress[2], ipaddress[3]);
  IPAddress gateway(gateaddress[0], gateaddress[1], gateaddress[2], gateaddress[3]); 
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

void setup()
{
  _intervallMillis = dht.getMinimumSamplingPeriod();

  Serial.begin(115200);
  delay(125);

  pinMode(INT_PIN, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(INT_PIN), handleInterrupt, FALLING);
  delay(125);
  
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);  // PWR_MGMT_1 register
  Wire.write(0);     // set to zero (wakes up the MPU-6050)
  Wire.endTransmission(true);
  delay(125);
  
  dht.setup(DHT_PIN, DHTesp::DHT22);
  delay(125);  

  SetupWifi();

  if(MDNS.begin("esp8266")) { }

  server.on("/", []() {

  // TODO
  // TODO
      
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
}

// TODO
// TODO
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
            in1 = root["in1"] == 1;
            in2 = root["in2"] == 1;
            in3 = root["in3"] == 1;
            in4 = root["in4"] == 1;

            ShowValues();            

            break;
    }
}

void loop()
{
  updateData();
  webSocket.loop();
  server.handleClient();
}
