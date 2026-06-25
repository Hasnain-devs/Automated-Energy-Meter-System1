#include <ESP8266WiFi.h>
#include <PubSubClient.h>

const char* WIFI_SSID = "Hasnain";
const char* WIFI_PASS = "11223344";

const char* MQTT_HOST_PRIMARY = "broker.emqx.io";
const char* MQTT_HOST_FALLBACK = "35.172.255.228";
const uint16_t MQTT_PORT = 1883;

const char* TOPIC_PUBLISH = "energy/meter";
const char* TOPIC_CONTROL = "energy/control";

const char* METER_ID = "7700";
const unsigned long PUBLISH_INTERVAL_MS = 5000;

#define USE_RELAY 1
const uint8_t RELAY_PIN = D1;

WiFiClient espClient;
PubSubClient mqtt(espClient);

unsigned long lastPublishMs = 0;

void onMqttMessage(char* topic, byte* payload, unsigned int length) {
  String msg;
  for (unsigned int i = 0; i < length; i++) {
    msg += (char)payload[i];
  }

  Serial.print("[MQTT] Message on ");
  Serial.print(topic);
  Serial.print(" => ");
  Serial.println(msg);

#if USE_RELAY
  if (String(topic) == TOPIC_CONTROL) {
    msg.trim();
    msg.toUpperCase();

    if (msg == "1" || msg == "ON") {
      digitalWrite(RELAY_PIN, HIGH);
      Serial.println("[RELAY] OFF command applied (power cut)");
    } else if (msg == "0" || msg == "OFF") {
      digitalWrite(RELAY_PIN, LOW);
      Serial.println("[RELAY] ON command applied (power restored)");
    } else {
      Serial.println("[RELAY] Unknown control payload");
    }
  }
#endif
}

void connectWiFi() {
  if (WiFi.status() == WL_CONNECTED) {
    return;
  }

  Serial.print("[WiFi] Connecting to ");
  Serial.println(WIFI_SSID);

  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  unsigned long start = millis();
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print('.');

    if (millis() - start > 20000) {
      Serial.println("\n[WiFi] Connect timeout, retrying...");
      WiFi.disconnect();
      delay(1000);
      WiFi.begin(WIFI_SSID, WIFI_PASS);
      start = millis();
    }
  }

  Serial.println();
  Serial.print("[WiFi] Connected, IP: ");
  Serial.println(WiFi.localIP());
}

bool tryMqttHost(const char* host) {
  mqtt.setServer(host, MQTT_PORT);
  String clientId = "ESP8266-" + String(ESP.getChipId(), HEX);

  Serial.print("[MQTT] Connecting to ");
  Serial.print(host);
  Serial.print(":");
  Serial.println(MQTT_PORT);

  bool ok = mqtt.connect(clientId.c_str());
  if (ok) {
    Serial.println("[MQTT] Connected");
    bool subOk = mqtt.subscribe(TOPIC_CONTROL);
    Serial.print("[MQTT] Subscribe ");
    Serial.print(TOPIC_CONTROL);
    Serial.print(" => ");
    Serial.println(subOk ? "OK" : "FAILED");
    return true;
  }

  Serial.print("[MQTT] Connect failed, rc=");
  Serial.println(mqtt.state());
  return false;
}

void connectMqtt() {
  if (mqtt.connected()) {
    return;
  }

  if (tryMqttHost(MQTT_HOST_PRIMARY)) {
    return;
  }

  if (tryMqttHost(MQTT_HOST_FALLBACK)) {
    return;
  }

  Serial.println("[MQTT] Both primary and fallback failed. Will retry.");
}

int generateRawValue() {
  return random(620, 721);
}

void publishMeterData() {
  int value = generateRawValue();
  String payload = String(METER_ID) + "|" + String(value);

  bool ok = mqtt.publish(TOPIC_PUBLISH, payload.c_str(), false);

  Serial.print("[PUB] ");
  Serial.print(TOPIC_PUBLISH);
  Serial.print(" => ");
  Serial.print(payload);
  Serial.print(" | ");
  Serial.println(ok ? "OK" : "FAILED");
}

void setup() {
  Serial.begin(115200);
  delay(300);

#if USE_RELAY
  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, LOW);
#endif

  randomSeed(micros());
  mqtt.setCallback(onMqttMessage);

  Serial.println("\n=== ESP8266 Energy Meter Publisher ===");
  connectWiFi();
  connectMqtt();
}

void loop() {
  if (WiFi.status() != WL_CONNECTED) {
    connectWiFi();
  }

  if (!mqtt.connected()) {
    connectMqtt();
  } else {
    mqtt.loop();
  }

  unsigned long now = millis();
  if (mqtt.connected() && (now - lastPublishMs >= PUBLISH_INTERVAL_MS)) {
    lastPublishMs = now;
    publishMeterData();
  }
}
