const int cuttingKnockSensor = A0;

int knockReading = 0;
void setup() {
  Serial.begin(9600);       // use the serial port
}

void loop() {
  knockReading = analogRead(cuttingKnockSensor);

  Serial.println(knockReading);
  delay(20); 
}
