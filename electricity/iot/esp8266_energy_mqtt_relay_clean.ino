#include <ESP8266WiFi.h>
#include <PubSubClient.h>

// ---------------- WIFI SETTINGS ----------------
const char* ssid = "Hasnain";
const char* password = "11223344";

// ---------------- MQTT SETTINGS ----------------
const char* mqtt_server_primary = "broker.emqx.io";
const char* mqtt_server_fallback = "35.172.255.228";
const int mqtt_port = 1883;

WiFiClient espClient;
PubSubClient client(espClient);

// ---------------- RELAY SETTINGS ----------------
#define RELAY_PIN D1

// Active LOW relay:
// LOW = ON (power connected)
// HIGH = OFF (power disconnected)
#define RELAY_ON LOW
#define RELAY_OFF HIGH

// ---------------- METER SETTINGS ----------------
String meterNumber = "7700";
unsigned long lastPublish = 0;
const unsigned long publishIntervalMs = 5000;

// Control lock: ignore ON/OFF commands until start marker arrives.
bool controlEnabled = false;

void setup_wifi()
{
  Serial.println();
  Serial.print("[WiFi] Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }

  Serial.println();
  Serial.println("[WiFi] Connected");
  Serial.print("[WiFi] IP Address: ");
  Serial.println(WiFi.localIP());
}

void callback(char* topic, byte* payload, unsigned int length)
{
  String message = "";
  for (unsigned int i = 0; i < length; i++)
  {
    message += (char)payload[i];
  }
  message.trim();

  Serial.print("[MQTT RX] topic: ");
  Serial.print(topic);
  Serial.print(" payload: ");
  Serial.println(message);

  String t = String(topic);

  if (t == "energy/control/start")
  {
    if (message == "1")
    {
      controlEnabled = true;
      Serial.println("[CONTROL] Start received. Control unlocked.");
    }
    return;
  }

  if (t == "energy/control")
  {
    if (!controlEnabled)
    {
      Serial.println("[CONTROL] Ignored. Waiting for energy/control/start=1");
      return;
    }

    if (message == "1")
    {
      digitalWrite(RELAY_PIN, RELAY_OFF); // OFF
      Serial.println("Relay OFF - Power Disconnected");
    }
    else if (message == "0")
    {
      digitalWrite(RELAY_PIN, RELAY_ON); // ON
      Serial.println("Relay ON - Power Restored");
    }
    else
    {
      Serial.println("[CONTROL] Unknown payload. Use 1 or 0.");
    }
  }
}

bool connectMqttHost(const char* host)
{
  client.setServer(host, mqtt_port);
  String clientId = "ESP8266EnergyMeter-" + String(ESP.getChipId(), HEX);

  Serial.print("[MQTT] Connecting to ");
  Serial.print(host);
  Serial.print(":");
  Serial.println(mqtt_port);

  if (!client.connect(clientId.c_str()))
  {
    Serial.print("[MQTT] failed, rc=");
    Serial.println(client.state());
    return false;
  }

  Serial.println("[MQTT] connected");
  client.subscribe("energy/control");
  client.subscribe("energy/control/start");
  Serial.println("[MQTT] subscribed: energy/control, energy/control/start");
  return true;
}

void reconnect()
{
  while (!client.connected())
  {
    if (connectMqttHost(mqtt_server_primary)) return;
    if (connectMqttHost(mqtt_server_fallback)) return;

    Serial.println("[MQTT] retrying in 5 seconds");
    delay(5000);
  }
}

void setup()
{
  Serial.begin(115200);

  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, RELAY_ON); // Default ON as requested for demo startup
  Serial.println("[BOOT] Relay ON at startup");

  setup_wifi();
  client.setCallback(callback);
}

void loop()
{
  if (WiFi.status() != WL_CONNECTED)
  {
    setup_wifi();
  }

  if (!client.connected())
  {
    reconnect();
  }

  client.loop();

  unsigned long now = millis();
  if (now - lastPublish >= publishIntervalMs)
  {
    lastPublish = now;
    String meterData = meterNumber + "|650";
    bool ok = client.publish("energy/meter", meterData.c_str(), false);

    Serial.print("Published Meter Data: ");
    Serial.print(meterData);
    Serial.print(" | ");
    Serial.println(ok ? "OK" : "FAILED");
  }
}
