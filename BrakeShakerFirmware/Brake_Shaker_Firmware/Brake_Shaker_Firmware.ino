/*
 Name:		Brake_Shaker_Firmware.ino
 Created:	5/30/2016 10:49:53 PM
 Author:	Dusan Milojkovic - d00mil
*/

////// START SETUP //////
const int pwmOutPin = 9;     //The pin number of the signal for the motor
const int switchPin = 2;     //The pin number of the switch
							 //Pushbutton attached between switchPin and +5V, 10K resistor attached between switchPin and ground
const int potPin = A0;       //The pin number for the potentiometer
							 //Middle of the pot to analog pin potPin, oneside to +5V, other side to gnd
const char startDelimiter = '<'; //The character that defines the start of the incoming serial data
const char endDelimiter = '>'; //The character that defines the end of the incoming serial data
const int minMotorPWM = 25;  //Should be 25% PWM since DC motors have hard time with lower PWM values
const int maxMotorPWM = 50;  //Should be 50% PWM if DC motor will be run on double voltage
////// END SETUP //////

int switchState = 0; //Variable for reading the status of a switch
float potValue = 0; //Variable that will hold the value read from pot
int processedBrakePWM = 0; //PWM value that will be sent to the motor
int minFinalBrakePWMperc = 0; //Initialize min PWM percentage
int maxFinalBrakePWMperc = 100; //Initialaize max PWM percentage
char inChar = 0; //Variable for holding the single charcter from the serial port
String inString; //Variable that holds the incoming serial data

// the setup function runs once on power up or when reset is pressed
void setup() {
	pinMode(pwmOutPin, OUTPUT); //Initialize the pwmOutPin pin as an output
	pinMode(switchPin, INPUT);  //Initialize the switchPin pin as an input
	Serial.begin(115200);

	checkForUserSettings();
	printToSerial();

	// Pins 5 and 6: controlled by Timer 0 in fast PWM mode (cycle length = 256)
	// Setting   	        Divisor 	Frequency
	// 0x01(00000001)	    1         62500
	// 0x02(00000010)  	  8         7812.5
	// 0x03(00000011)  	  64        976.5625   <--DEFAULT
	// 0x04(00000100) 	  256       244.140625
	// 0x05(00000101)     1024      61.03515625
	// TCCR0B = (TCCR0B & 0b11111000) | <setting>;

	// Pins 9 and 10: controlled by timer 1 in phase-correct PWM mode (cycle length = 510)
	// Setting 	          Divisor 	Frequency
	// 0x01(00000001) 	  1         31372.55
	// 0x02(00000010) 	  8         3921.16
	// 0x03(00000011)  	  64        490.20   <--DEFAULT
	// 0x04(00000100)  	  256       122.55
	// 0x05(00000101) 	  1024      30.64
	// TCCR1B = (TCCR1B & 0b11111000) | <setting>;
	TCCR1B = TCCR1B & 0b11111000 | 0b00000001;  // set pin 9 & 10 to frequency 31250

	// Pins 11 and 3: controlled by timer 2 in phase-correct PWM mode (cycle length = 510)
	// Setting 	          Divisor 	Frequency
	// 0x01(00000001) 	  1         31372.55
	// 0x02(00000010) 	  8         3921.16
	// 0x03(00000011)     32        980.39
	// 0x04(00000100)     64        490.20   <--DEFAULT
	// 0x05(00000101)     128       245.10
	// 0x06(00000110)     256       122.55
	// 0x07(00000111)     1024      30.64
	// TCCR2B = (TCCR2B & 0b11111000) | <setting>;

	// All frequencies are in Hz and assume a 16000000 Hz system clock. 
}

// the loop function runs over and over again until power down or reset
void loop() {
	checkForUserSettings();

	if (Serial.available() > 0) //check if Arduino is connected
	{
		Serial.println("Serial is available!");
		inChar = Serial.read();

		if (inChar == startDelimiter)
		{
			processedBrakePWM = processBrakeValue(); //Converts incoming string to int and assingns it to the output variable
			Serial.print("Value received:");
			Serial.println(processedBrakePWM);

			//Convert input data from serial port to the minFinalBrakePWMval <-> maxFinalBrakePWMval range
			if (processedBrakePWM <= minFinalBrakePWMperc) //When PWM value is very low, make sure it drops to zero
			{
				processedBrakePWM = 0;
			}
			if (processedBrakePWM >= maxFinalBrakePWMperc) //If PWM value is too high, keep it set at defined max
			{
				processedBrakePWM = maxFinalBrakePWMperc;
			}
			processedBrakePWM = int(abs(processedBrakePWM*2.55)); //convert processed data to 0-255 range
			Serial.print("Processed PWM value:");
			Serial.println(processedBrakePWM);
		}
		else
		{
			Serial.println("No start delimiter identified");
		}
	}
	analogWrite(pwmOutPin, processedBrakePWM); //finally send the PWM value to the motor
}

int processBrakeValue()
{
	inString = "0";
	inChar = '0';

	while (Serial.available() > 0)
	{
		inChar = Serial.read();
		switch (inChar)
		{
		case endDelimiter:
			break;
		default:
			inString += inChar;
			break;
		}
	}
	return inString.toInt();
}

void checkForUserSettings()
{
	switchState = digitalRead(switchPin); // read the state of the switch pin (0|1)
	if (switchState == HIGH)  // check if the switch is on. if it is, the switchState is HIGH
	{
		//If the switch is on - appply the user settings
		potValue = analogRead(potPin);  // read the value from the potentioneter (0-1023)
		minFinalBrakePWMperc = minMotorPWM;
		maxFinalBrakePWMperc = int(potValue * 100 / 1024); //convert value from 0-1023 to 0-100
	}
	else
	{
		//if the switch is off - keep the full range
		minFinalBrakePWMperc = 0;
		maxFinalBrakePWMperc = 100;
	}
}

void printToSerial()
{
	Serial.print("Minimum pwm output % is: ");
	Serial.println(minFinalBrakePWMperc);
	Serial.print("Maximum pwm output % is: ");
	Serial.println(maxFinalBrakePWMperc);
}
