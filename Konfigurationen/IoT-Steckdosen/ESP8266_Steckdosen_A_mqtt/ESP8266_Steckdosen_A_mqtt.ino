
// ***********************************
const char* ssid = "Spieleparadies";
const char* password = "";
const char* mqtt_server = "192.168.178.29";
// ***********************************

// MQTT
// Haus/Switches/Railway01   True|False
// Haus/Switches/Railway02   True|False
// Haus/Switches/Railway03   True|False
// Haus/Switches/Railway04   True|False
// Haus/Switches/Off
// Haus/Switches/On

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

#include <Ticker.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>
#include <base64.hpp>

WiFiClient espClient;
PubSubClient client(espClient);

int PortIN1 = D2;
int PortIN2 = D3;
int PortIN3 = D4;
int PortIN4 = D5;

bool stateIN1 = false;
bool stateIN2 = false;
bool stateIN3 = false;
bool stateIN4 = false;

void TriggerSwitches(bool in1, bool in2, bool in3, bool in4);

void callback(char * topic, byte * payload, unsigned int length) {
  Serial.print(topic);
  Serial.print(" => ");

  char * payload_str;
  payload_str = (char * ) malloc(length + 1);
  memcpy(payload_str, payload, length);
  payload_str[length] = '\0';
  Serial.println(String(payload_str));

  if (String(topic) == "Haus/Switches/Railway01") {
    String v = String(payload_str);
    v.toLowerCase();
    stateIN1 = v == "true" || v == "wahr" || v == "1"; 
  }
  if (String(topic) == "Haus/Switches/Railway02") {
    String v = String(payload_str);
    v.toLowerCase();
    stateIN2 = v == "true" || v == "wahr" || v == "1"; 
  }
  if (String(topic) == "Haus/Switches/Railway03") {
    String v = String(payload_str);
    v.toLowerCase();
    stateIN3 = v == "true" || v == "wahr" || v == "1"; 
  }
  if (String(topic) == "Haus/Switches/Railway04") {
    String v = String(payload_str);
    v.toLowerCase();
    stateIN4 = v == "true" || v == "wahr" || v == "1"; 
  } 
  if (String(topic) == "Haus/Switches/Off") {
    stateIN1 = false;
    stateIN2 = false;
    stateIN3 = false;
    stateIN4 = false;
  } 
  if (String(topic) == "Haus/Switches/On") {
    stateIN1 = true;
    stateIN2 = true;
    stateIN3 = true;
    stateIN4 = true;
  } 

  Serial.print("State: ");
  Serial.print(stateIN1);
  Serial.print(", ");
  Serial.print(stateIN2);
  Serial.print(", ");
  Serial.print(stateIN3);
  Serial.print(", ");
  Serial.println(stateIN4);

  TriggerSwitches(stateIN1, stateIN2, stateIN3, stateIN4);

  free(payload_str);
}

void connect_to_MQTT() {
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);

  if (client.connect("Railway")) {
    Serial.println("(re)-connected to MQTT");
    client.subscribe("Haus/Switches/Railway01");
    client.subscribe("Haus/Switches/Railway02");
    client.subscribe("Haus/Switches/Railway03");
    client.subscribe("Haus/Switches/Railway04");
    client.subscribe("Haus/Switches/Off");
    client.subscribe("Haus/Switches/On");
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

void setup() 
{
  TriggerSwitches(false, false, false, false);

  pinMode(PortIN1, OUTPUT);
  pinMode(PortIN2, OUTPUT);
  pinMode(PortIN3, OUTPUT);
  pinMode(PortIN4, OUTPUT);

  TriggerSwitches(false, false, false, false);

  Serial.begin(115200);

  setupRies();
}

void TriggerSwitches(bool in1, bool in2, bool in3, bool in4)
{
  digitalWrite(PortIN1, in1 ? HIGH : LOW);
  digitalWrite(PortIN2, in2 ? HIGH : LOW);
  digitalWrite(PortIN3, in3 ? HIGH : LOW);
  digitalWrite(PortIN4, in4 ? HIGH : LOW);
}

void loop() 
{
  client.loop();
  if (!client.connected()) {
    Serial.println("Not connected to MQTT....");
    connect_to_MQTT();
    delay(1000);
  }
}

