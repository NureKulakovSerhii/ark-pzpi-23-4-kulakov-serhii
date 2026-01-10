#include <Arduino.h>
#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

const char* WIFI_SSID = "Wokwi-GUEST";
const char* WIFI_PASSWORD = "";
const char* SERVER_URL = "http://192.168.0.105:5113";

const String DEVICE_ID = "WH-TEST-001";
const String SECRET_KEY = "test-secret-123";

#define BUTTON_PIN 4
#define LED_WIFI 2
#define LED_DOOR 16
#define LED_SEND 17

bool doorOpen = false;
bool lastButtonState = HIGH;
bool wifiConnected = false;
unsigned long lastTelemetryTime = 0;
unsigned long lastHeartbeatTime = 0;

void testLEDs();
void connectToWiFi();
void checkWiFiConnection();
void checkDoorButton();
void testServerConnection();
void sendHeartbeat();
void sendDoorEvent(bool isOpen);
void sendTelemetry();

void setup() {
  Serial.begin(115200);
  delay(2000);
  pinMode(LED_WIFI, OUTPUT);
  pinMode(LED_DOOR, OUTPUT);
  pinMode(LED_SEND, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  digitalWrite(LED_WIFI, LOW);
  digitalWrite(LED_DOOR, LOW);
  digitalWrite(LED_SEND, LOW);
  testLEDs();
  connectToWiFi();
}

void loop() {
  unsigned long currentTime = millis();
  checkWiFiConnection();
  if (currentTime - lastHeartbeatTime > 60000) {
    sendHeartbeat();
    lastHeartbeatTime = currentTime;
  }
  checkDoorButton();
  if (currentTime - lastTelemetryTime > 30000) {
    sendTelemetry();
    lastTelemetryTime = currentTime;
  }
  delay(100);
}

void testLEDs() {
  digitalWrite(LED_WIFI, HIGH); delay(300); digitalWrite(LED_WIFI, LOW);
  digitalWrite(LED_DOOR, HIGH); delay(300); digitalWrite(LED_DOOR, LOW);
  digitalWrite(LED_SEND, HIGH); delay(300); digitalWrite(LED_SEND, LOW);
}

void connectToWiFi() {
  Serial.println(WIFI_SSID);
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
  int attempts = 0;
  while (WiFi.status() != WL_CONNECTED && attempts < 30) {
    delay(500);
    Serial.print(".");
    digitalWrite(LED_WIFI, !digitalRead(LED_WIFI));
    attempts++;
  }
  if (WiFi.status() == WL_CONNECTED) {
    wifiConnected = true;
    digitalWrite(LED_WIFI, HIGH);
    Serial.println(WiFi.localIP().toString());
    testServerConnection();
  } else {
    digitalWrite(LED_WIFI, LOW);
  }
}

void checkWiFiConnection() {
  if (WiFi.status() != WL_CONNECTED) {
    if (wifiConnected) {
      wifiConnected = false;
      digitalWrite(LED_WIFI, LOW);
    }
    static unsigned long lastReconnectAttempt = 0;
    if (millis() - lastReconnectAttempt > 10000) {
      WiFi.reconnect();
      lastReconnectAttempt = millis();
    }
  } else if (!wifiConnected) {
    wifiConnected = true;
    digitalWrite(LED_WIFI, HIGH);
  }
}

void checkDoorButton() {
  bool buttonState = digitalRead(BUTTON_PIN);
  if (buttonState != lastButtonState) {
    delay(50);
    if (digitalRead(BUTTON_PIN) == buttonState) {
      doorOpen = (buttonState == LOW);
      digitalWrite(LED_DOOR, doorOpen ? HIGH : LOW);
      sendDoorEvent(doorOpen);
      lastButtonState = buttonState;
    }
  }
}

void testServerConnection() {
  if (!wifiConnected) return;
  HTTPClient http;
  String url = String(SERVER_URL) + "/api/iot/heartbeat";
  http.begin(url);
  http.addHeader("Content-Type", "application/json");
  StaticJsonDocument<128> doc;
  doc["deviceId"] = DEVICE_ID;
  doc["secretKey"] = SECRET_KEY;
  String payload;
  serializeJson(doc, payload);
  int httpCode = http.POST(payload);
  Serial.print("   Response: ");
  Serial.println(httpCode);
  if (httpCode == 200) {
    String response = http.getString();
  } else {
    Serial.println("Server error or connection failed");
  }
  http.end();
}

void sendHeartbeat() {
  if (!wifiConnected) return;
  HTTPClient http;
  String url = String(SERVER_URL) + "/api/iot/heartbeat";
  http.begin(url);
  http.addHeader("Content-Type", "application/json");

  StaticJsonDocument<128> doc;
  doc["deviceId"] = DEVICE_ID;
  doc["secretKey"] = SECRET_KEY;
  String payload;
  serializeJson(doc, payload);

  int httpCode = http.POST(payload);
  if (httpCode == 200) {
    Serial.println("Heartbeat sent successfully");
  } else {
    Serial.println("Heartbeat failed: " + String(httpCode));
  }
  http.end();
}

void sendDoorEvent(bool isOpen) {
  if (!wifiConnected) return;
  HTTPClient http;
  String url = String(SERVER_URL) + "/api/iot/telemetry";
  http.begin(url);
  http.addHeader("Content-Type", "application/json");
  StaticJsonDocument<256> doc;
  doc["deviceId"] = DEVICE_ID;
  doc["secretKey"] = SECRET_KEY;
  doc["isDoorOpen"] = isOpen;
  doc["eventType"] = "door";
  String payload;
  serializeJson(doc, payload);
  digitalWrite(LED_SEND, HIGH);
  int httpCode = http.POST(payload);
  digitalWrite(LED_SEND, LOW);
  if (httpCode > 0) {
    Serial.println("  Server: " + http.getString());
  } else {
    Serial.println("Error sending door event");
  }
  http.end();
}

void sendTelemetry() {
  if (!wifiConnected) return;
  HTTPClient http;
  String url = String(SERVER_URL) + "/api/iot/telemetry";
  http.begin(url);
  http.addHeader("Content-Type", "application/json");
  StaticJsonDocument<256> doc;
  doc["deviceId"] = DEVICE_ID;
  doc["secretKey"] = SECRET_KEY;
  doc["temperature"] = 77.5;
  doc["humidity"] = 77.5;
  doc["isDoorOpen"] = doorOpen;
  doc["isPowerOn"] = true;
  doc["eventType"] = "temperature_humidity";
  String payload;
  serializeJson(doc, payload);
  Serial.println(payload);
  digitalWrite(LED_SEND, HIGH);
  int httpCode = http.POST(payload);
  digitalWrite(LED_SEND, LOW);
  Serial.println(httpCode);
  if (httpCode > 0) {
    String response = http.getString();
    if (httpCode == 200) {
      Serial.println("Telemetry saved successfully!");
    } else if (httpCode == 401) {
      Serial.println("Unauthorized - check device credentials");
    } else if (httpCode == 400) {
      Serial.println("Bad request - check data format");
    }
  } else {
    Serial.println("Error: " + http.errorToString(httpCode));
  }
  http.end();
}
