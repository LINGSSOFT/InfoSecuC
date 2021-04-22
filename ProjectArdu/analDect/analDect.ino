/*
 Name:		analDect.ino
 Created:	4/22/2021 8:54:58 PM
 Author:	jjang
*/

// the setup function runs once when you press reset or power the board
#include <HID.h>
#include <EEPROM.h>
int nBoardLed = 13;

void setup()
{
	// put your setup code here, to run once:
	pinMode(nBoardLed, OUTPUT);
}

// the loop function runs over and over again until power down or reset
void loop()
{
	// put your main code here, to run repeatedly:
	digitalWrite(nBoardLed, HIGH);
	delay(1000);
	digitalWrite(nBoardLed, LOW); 
	delay(1000);
}
