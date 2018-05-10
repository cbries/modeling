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
static const uint8_t D3   = 0;
static const uint8_t D4   = 2;
static const uint8_t D5   = 14;
static const uint8_t D6   = 12;
static const uint8_t D7   = 13;
static const uint8_t D8   = 15;
static const uint8_t RX   = 3;
static const uint8_t TX   = 1;
*/

const char* ssid      = "";
const char* password  = "";

#include <Arduino.h>

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsServer.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <EEPROM.h>
#include <Hash.h>

int _BLUE = D6;
int _GREEN = D5;
int _RED = D3;

byte r = 0;
byte g = 0;
byte b = 0;

#define LED_RED     _RED
#define LED_GREEN   _GREEN
#define LED_BLUE    _BLUE

#define USE_SERIAL Serial

ESP8266WiFiMulti WiFiMulti;
ESP8266WebServer server(80);
WebSocketsServer webSocket = WebSocketsServer(81);

void storeValues()
{
  EEPROM.write(0, r);
  EEPROM.write(1, g);
  EEPROM.write(2, b);
  EEPROM.commit();
}

void restoreValues()
{
  r = EEPROM.read(0);
  g = EEPROM.read(1);
  b = EEPROM.read(2);
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

            // send message to client
            webSocket.sendTXT(num, "Connected");
        }
            break;
        case WStype_TEXT:
            USE_SERIAL.printf("[%u] get Text: %s\n", num, payload);

            if(payload[0] == '#') 
            {
                // decode rgb data
                uint32_t rgb = (uint32_t) strtol((const char *) &payload[1], NULL, 16);

                r = ((rgb >> 16) & 0xFF);
                g = ((rgb >>  8) & 0xFF);
                b = ((rgb >>  0) & 0xFF);

                analogWrite(LED_RED,    r);
                analogWrite(LED_GREEN,  g);
                analogWrite(LED_BLUE,   b);
            }

            break;
    }
}

void setup() 
{
  EEPROM.begin(512);
  delay(10);
  
  //USE_SERIAL.begin(921600);
  USE_SERIAL.begin(115200);
  //USE_SERIAL.setDebugOutput(true);
  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();

  restoreValues();

  for(uint8_t t = 4; t > 0; t--) 
  {
    USE_SERIAL.printf("[SETUP] BOOT WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }

  pinMode(LED_RED, OUTPUT);
  pinMode(LED_GREEN, OUTPUT);
  pinMode(LED_BLUE, OUTPUT);

  digitalWrite(LED_RED, 1);
  digitalWrite(LED_GREEN, 1);
  digitalWrite(LED_BLUE, 1);

  WiFiMulti.addAP(ssid, password);

  while(WiFiMulti.run() != WL_CONNECTED) {
    delay(100);
  }

    // start webSocket server
    webSocket.begin();
    webSocket.onEvent(webSocketEvent);

    if(MDNS.begin("esp8266")) {
        USE_SERIAL.println("MDNS responder started");
    }

    // handle index
    server.on("/", []() {
      //char bufR[12] = {0}; sprintf(bufR, "%i", (int)r);
      //char bufG[12] = {0}; sprintf(bufG, "%i", (int)g);
      //char bufB[12] = {0}; sprintf(bufB, "%i", (int)b);

      char buf[2048] = {0};
      sprintf(buf, "<html><head><script>var connection = new WebSocket('ws://'+location.hostname+':81/'," \
        "['arduino']);connection.onopen = function () {  connection.send('Connect ' + new Date()); }; " \ 
        "connection.onerror = function (error) {    console.log('WebSocket Error ', error);};connection.onmessage " \
        "= function (e) {  console.log('Server: ', e.data);};function sendRGB() {  " \
          "var r = parseInt(document.getElementById('r').value).toString(16);  " \
          "var g = parseInt(document.getElementById('g').value).toString(16);  " \
          "var b = parseInt(document.getElementById('b').value).toString(16);  " \
        "if(r.length < 2) { r = '0' + r; }   if(g.length < 2) { g = '0' + g; }   if(b.length < 2) { b = '0' + b; }   " \
        "var rgb = '#'+r+g+b;    console.log('RGB: ' + rgb); connection.send(rgb); }</script></head>" \
        "<body>LED Control:<br/><br/>" \
           "R: <input id=\"r\" value=\"%i\" type=\"range\" min=\"0\" max=\"255\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "G: <input id=\"g\" value=\"%i\" type=\"range\" min=\"0\" max=\"255\" step=\"1\" oninput=\"sendRGB();\" /><br/>" \
           "B: <input id=\"b\" value=\"%i\" type=\"range\" min=\"0\" max=\"255\" step=\"1\" oninput=\"sendRGB();\" /><br/></body></html>",
          (int)r, (int)g, (int)b);
      
        // send index.html
        server.send(200, "text/html", buf);
    });

    server.begin();

    // Add service to MDNS
    MDNS.addService("http", "tcp", 80);
    MDNS.addService("ws", "tcp", 81);

    digitalWrite(LED_RED, 0);
    digitalWrite(LED_GREEN, 0);
    digitalWrite(LED_BLUE, 0);
}

void loop() 
{
    webSocket.loop();
    server.handleClient();
}

