const char* ssid      = "Hogwarts School of Wizardry";
const char* password  = "";

#include <Arduino.h>

// IP ADDRESSES / GATEWAY
const uint8_t ipaddress[4] = { 192, 168, 178, 64 };
const uint8_t gateaddress[4] = { 192, 168, 178, 1 };

// WeMos D1 R1
// 80 MHz
// 4M (1M SPIFFS)
// v2 Lower Memory
// 115200

#include <ESP8266WiFi.h>
#include <WebSocketsServer.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <Wire.h>
#include <Hash.h>
#include <FS.h>

#include "ArduinoJson.h"

#define USE_SERIAL Serial
#define INT_PIN 13

String getContentType(String filename); // convert the file extension to the MIME type
bool handleFileRead(String path);       // send the right file to the client (if it exists)
bool writeToFile(const char *path, const char *content);

// Gyroscope
const uint8_t MPU_addr = 0x68; // I2C address of the MPU-6050
const int _mpuMinVal = 265;
const int _mpuMaxVal = 402;
// DHT*
#include "DHTesp.h"
#define DHT_PIN 16
DHTesp dht;
// Hall-Sensor
volatile int _interruptCounter = 0;

struct SensorData
{
#define SENSORDATA_SIZE 512
  
  int dhtStatus;
  float dhtHumidity;
  float dhtTemperature;
  float dhtFahrenheit;
  float dhtHeatIndexCelsius;
  float dhtHeatIndexFahrenheit;

  double angleX;
  double angleY;
  double angleZ;

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
    root["millis"] = millis();
    root["numberOfInterrupts"] = numberOfInterrupts;
    root["dhtStatus"] = dhtStatus;
    root["dhtHumidity"] = dhtHumidity;
    root["dhtTemperature"] = dhtTemperature;
    root["dhtFahrenheit"] = dhtFahrenheit;
    root["dhtHeatIndexCelsius"] = dhtHeatIndexCelsius;
    root["dhtHeatIndexFahrenheit"] = dhtHeatIndexFahrenheit;
    root["angleX"] = angleX;
    root["angleY"] = angleY;
    root["angleZ"] = angleZ;
    root["AcX"] = AcX;
    root["AcY"] = AcY;
    root["AcZ"] = AcZ;
    root["Tmp"] = Tmp;
    root["GyX"] = GyX;
    root["GyY"] = GyY;
    root["GyZ"] = GyZ;
    root.printTo(json, maxSize);
  }
};

// runtime
SensorData _data;
long _previousMillis = 0;
long _intervallMillis = 0;

//ESP8266WiFiMulti WiFiMulti;
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
    _data.dhtStatus = (int) dht.getStatus();
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

  int xAng = map(_data.AcX, _mpuMinVal, _mpuMaxVal, -90, 90);
  int yAng = map(_data.AcY, _mpuMinVal, _mpuMaxVal, -90, 90);
  int zAng = map(_data.AcZ, _mpuMinVal, _mpuMaxVal, -90, 90);

  _data.angleX = RAD_TO_DEG * (atan2(-yAng, -zAng)+PI);
  _data.angleY = RAD_TO_DEG * (atan2(-xAng, -zAng)+PI);
  _data.angleZ = RAD_TO_DEG * (atan2(-yAng, -xAng)+PI);
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

void handleCommand(uint8_t * payload, size_t length)
{
  StaticJsonBuffer<200> jsonBuffer;
  JsonObject& root = jsonBuffer.parseObject(payload);
  if (!root.success()) {
    Serial.println("handleCommand() failed");
    return;
  }
  
  const char* fname = root["fname"];
  const char* cnt = root["cnt"];

  Serial.print("Filename: "); Serial.print(fname);
  Serial.print(", Content: "); Serial.println(cnt);

  // we trust our customers, no auth check
  writeToFile(fname, cnt);
}

bool _isConnected = false;
uint8_t _wsNum = 0;

void webSocketEvent(uint8_t num, WStype_t type, uint8_t * payload, size_t length) { 
    switch(type) {
        case WStype_DISCONNECTED: {
          _isConnected = false;
        } break;

        case WStype_CONNECTED: {
          IPAddress ip = webSocket.remoteIP(num);
          Serial.printf("[%u] Connected from %d.%d.%d.%d url: %s\n", num, ip[0], ip[1], ip[2], ip[3], payload);
          _isConnected = true;  
          _wsNum = num;      
        } break;
        
        case WStype_TEXT: {
          handleCommand(payload, length);
        } break;
    }
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

  MDNS.addService("http", "tcp", 80);
  MDNS.addService("ws", "tcp", 81);

  webSocket.begin();
  webSocket.onEvent(webSocketEvent);

  SPIFFS.begin();
  
  server.onNotFound([]() {
    if (!handleFileRead(server.uri()))
      server.send(404, "text/plain", "404: Not Found");
  });

  server.begin();

  Serial.println("***** READY *****");
}

void loop()
{
  updateData();
  webSocket.loop();
  server.handleClient();

  char data[512];
  _data.serialize(data, 512);

  if(_isConnected) {
    webSocket.sendTXT(_wsNum, data);
  }

  delay(100);
}

bool handleFileRead(String path) {
  Serial.println("handleFileRead: " + path);
  if (path.endsWith("/")) path += "index.html";
  String contentType = getContentType(path);
  if (SPIFFS.exists(path)) {
    File file = SPIFFS.open(path, "r");
    size_t sent = server.streamFile(file, contentType);
    file.close();
    return true;
  }
  Serial.println("\tFile Not Found");
  return false;
}

bool writeToFile(const char *path, const char *content)
{
  File f = SPIFFS.open(path, "w");
  if (!f) { 
    Serial.println("file open failed");
    return false;
  }
  f.print(content);
  f.close();
  return true;
}

String getContentType(String filename){
  if(filename.endsWith(".htm")) return "text/html";
  else if(filename.endsWith(".html")) return "text/html";
  else if(filename.endsWith(".css")) return "text/css";
  else if(filename.endsWith(".js")) return "application/javascript";
  else if(filename.endsWith(".png")) return "image/png";
  else if(filename.endsWith(".gif")) return "image/gif";
  else if(filename.endsWith(".jpg")) return "image/jpeg";
  else if(filename.endsWith(".ico")) return "image/x-icon";
  else if(filename.endsWith(".xml")) return "text/xml";
  else if(filename.endsWith(".pdf")) return "application/x-pdf";
  else if(filename.endsWith(".zip")) return "application/x-zip";
  else if(filename.endsWith(".gz")) return "application/x-gzip";
  return "text/plain";
}
