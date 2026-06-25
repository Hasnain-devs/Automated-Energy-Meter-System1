// Clean relay hardware test only (no WiFi, no MQTT)
// NodeMCU D1 (GPIO5) -> Relay IN
// Active LOW relay: LOW = ON, HIGH = OFF

#define RELAY_PIN D1
#define RELAY_ON LOW
#define RELAY_OFF HIGH

void setup() {
  Serial.begin(115200);
  delay(200);

  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, RELAY_OFF);  // Safe startup: relay OFF

  Serial.println();
  Serial.println("=== RELAY TEST START ===");
  Serial.println("[BOOT] Relay OFF at startup (safe state)");
}

void loop() {
  digitalWrite(RELAY_PIN, RELAY_ON);
  Serial.println("[RELAY] ON - Current Passing");
  delay(3000);

  digitalWrite(RELAY_PIN, RELAY_OFF);
  Serial.println("[RELAY] OFF - No Current");
  delay(3000);
}
