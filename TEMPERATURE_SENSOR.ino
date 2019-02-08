#define NUMBER_OF_MESUARMENTS 8
#define PULSEIN_TIME 1000 // microseconds
#define DELAY_TIME 1000 // milliseconds

byte SENSOR_PIN = 3;
unsigned long startTime;
unsigned long currentTime;
float HIGH_STATE[NUMBER_OF_MESUARMENTS];
float LOW_STATE[NUMBER_OF_MESUARMENTS];
float INDIVIDUAL_DUTY_CYCLE[NUMBER_OF_MESUARMENTS];
float INDIVIDUAL_DUTY_CYCLES_SUM = 0;
float VALID_DUTY_CYCLE = 0.0;
float temperature = 0.0;

//void PrintSMT172(float tmp, float dc);

void setup() {
  Serial.begin(115200);
  pinMode(SENSOR_PIN, INPUT);
  startTime = millis();
}

void loop() {  
  // SMT172---------------
  currentTime = millis();
  
  if(currentTime - startTime >=  DELAY_TIME){
      INDIVIDUAL_DUTY_CYCLES_SUM = 0;
  
    // Gets the time in microseconds for the low and high states of the PWM signal
    for(int i = 0; i < NUMBER_OF_MESUARMENTS; i++){
      HIGH_STATE[i] = (float)(pulseIn(SENSOR_PIN, HIGH, PULSEIN_TIME)); // 1000000;
      LOW_STATE[i] = (float)(pulseIn(SENSOR_PIN, LOW, PULSEIN_TIME)); // 1000000;
    }

    // Gets all 8 individual duty cycles and then adds them
    for(int i = 0; i < NUMBER_OF_MESUARMENTS; i++){
      INDIVIDUAL_DUTY_CYCLE[i] = HIGH_STATE[i] / (HIGH_STATE[i] + LOW_STATE[i]);
      INDIVIDUAL_DUTY_CYCLES_SUM += INDIVIDUAL_DUTY_CYCLE[i];
    }

    VALID_DUTY_CYCLE = INDIVIDUAL_DUTY_CYCLES_SUM / NUMBER_OF_MESUARMENTS;
    temperature = (VALID_DUTY_CYCLE - 0.32) / 0.0047;

    Serial.println(temperature);
    //PrintSMT172(temperature, VALID_DUTY_CYCLE); 
    startTime = currentTime;
  }
  //delay(DELAY_TIME);
}
/*
void PrintSMT172(float tmp, float dc){
  // For printing Duty Cycle
  float dcInPerc = dc * 100;
  
  Serial.println("--------------------------------");
  Serial.print("|SMT172| Duty Cycle : ");
  Serial.print(dcInPerc);
  Serial.println(" %");
  
  Serial.print("|SMT172| Temperature: ");
  Serial.print(tmp);
  Serial.println(" *C");
  Serial.println("--------------------------------");
}*/
