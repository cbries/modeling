// ***********************************
const char* ssid = "Spieleparadies";
const char* password = "";
const char* mqtt_server = "192.168.178.29";
// ***********************************

/*
private const int MinValue = 0;
private const int MaxValue = 255;

private const string MqttBrokerAddress = "192.168.178.29";		{value}
private const string MqttTopicR = "Haus/Railway/Sky/R";			{value}
private const string MqttTopicG = "Haus/Railway/Sky/G";			{value}
private const string MqttTopicB = "Haus/Railway/Sky/B";			{value}
private const string MqttTopicW = "Haus/Railway/Sky/W"; 		{value}	// W or A (white or alpha)
private const string MqttTopicOff = "Haus/Railway/Sky/Off";
private const string MqttTopicOn = "Haus/Railway/Sky/On";
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

#include <Ticker.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>
#include <base64.hpp>
#include <EEPROM.h>

WiFiClient espClient;
PubSubClient client(espClient);

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

void storeValues();

void callback(char * topic, byte * payload, unsigned int length) {
  //Serial.print(topic);
  //Serial.print(" => ");

  char * payload_str;
  payload_str = (char * ) malloc(length + 1);
  memcpy(payload_str, payload, length);
  payload_str[length] = '\0';
  //Serial.println(String(payload_str));

  //String t = String(topic);

  if (String(topic) == "Haus/Railway/Sky/R") {
    String v = String(payload_str);
    r = v.toInt();
  }
  if (String(topic) == "Haus/Railway/Sky/G") {
    String v = String(payload_str);
    g = v.toInt();
  }
  if (String(topic) == "Haus/Railway/Sky/B") {
    String v = String(payload_str);
    b = v.toInt();
  }
  if (String(topic) == "Haus/Railway/Sky/W" || String(topic) == "Haus/Railway/Sky/A") {
    String v = String(payload_str);
    w = v.toInt();
  } 
  if (String(topic) == "Haus/Railway/Sky/Off") {
    r = 0;
    g = 0;
    b = 0;
    w = 0;
  } 
  if (String(topic) == "Haus/Railway/Sky/On") {
    r = 1023;
    g = 1023;
    b = 1023;
    w = 1023;
  } 
/*
  Serial.print("RGBA: ");
  Serial.print(r);
  Serial.print(", ");
  Serial.print(g);
  Serial.print(", ");
  Serial.print(b);
  Serial.print(", ");
  Serial.println(w);
*/
  ShowValues();

  storeValues();

  free(payload_str);
}

void connect_to_MQTT() {
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);

  if (client.connect("RailwayLedStripe")) {
    Serial.println("(re)-connected to MQTT");
    client.subscribe("Haus/Railway/Sky/R");
    client.subscribe("Haus/Railway/Sky/G");
    client.subscribe("Haus/Railway/Sky/B");
    client.subscribe("Haus/Railway/Sky/A");
    client.subscribe("Haus/Railway/Sky/W");
    client.subscribe("Haus/Railway/Sky/Off");
    client.subscribe("Haus/Railway/Sky/On");
  } else {
    Serial.println("Could not connect to MQTT");
  }
}

void setupRies() 
{
  // Connecting to our WiFi network
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  connect_to_MQTT();
}

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

  for(uint8_t t = 4; t > 0; t--) 
  {
    USE_SERIAL.printf("[SETUP] BOOT WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }

  setupRies();
}

void loop() 
{
  client.loop();
  if (!client.connected()) {
    Serial.println("Not connected to MQTT....");
    connect_to_MQTT();

    Serial.println("IP address local: ");
    Serial.println(WiFi.localIP());
    delay(1000);
  }
}

