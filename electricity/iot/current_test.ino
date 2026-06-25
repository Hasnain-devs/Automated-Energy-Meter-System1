// Step 1: Basic relay current test (no Wi-Fi, no IoT)
// Relay IN pin -> NodeMCU D1 (GPIO5)

#define RELAY_PIN D1

// If your relay logic is reversed, swap these two values.
#define RELAY_ON LOW
#define RELAY_OFF HIGH

void setup() {
  Serial.begin(115200);
  pinMode(RELAY_PIN, OUTPUT);

  // Start in OFF state for safety.
  digitalWrite(RELAY_PIN, RELAY_OFF);
  Serial.println("Relay OFF - No Current");
}

void loop() {
  digitalWrite(RELAY_PIN, RELAY_ON);
  Serial.println("Relay ON - Current Passing");
  delay(3000);

  digitalWrite(RELAY_PIN, RELAY_OFF);
  Serial.println("Relay OFF - No Current");
  delay(3000);
}
