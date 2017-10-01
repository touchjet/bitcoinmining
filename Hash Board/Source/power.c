#include "stm32f10x.h"
#include "power.h"
#include "led.h"
#include "parcel.h"

const uint8_t VCODE[31] = {
	0x1d, //0.83
	0x1d, //0.84
  0x1c, //0.850V
  0x1c, //0.86
  0x1b, //0.875V
  0x1b, //0.88
  0x1b, //0.89
  0x1a, //0.900V
  0x1a, //0.91
  0x19, //0.925V
  0x19, //0.93
  0x19, //0.94
  0x18, //0.950V
  0x18, //0.96
  0x17, //0.975V
  0x17, //0.98
  0x17, //0.99
  0x16, //1.000V
  0x16, //1.01
  0x15, //1.025V
  0x15, //1.03
  0x15, //1.04
  0x14, //1.050V
  0x14, //1.06
  0x13, //1.075V
  0x13, //1.08
  0x13, //1.09
  0x12, //1.100V
  0x12, //1.100V
  0x12, //1.100V
  0x12, //1.100V
};

static volatile uint8_t volt_set;

void Delay(int16_t ms);

void POWER_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;
  
	volt_set = 0;
	
  /* Configure VID as Output */
  GPIO_InitStructure.GPIO_Pin = POWER_1_PIN_VIDS;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_OD;
  GPIO_Init(POWER_1_PORT, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin = POWER_2_PIN_VIDS;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_OD;
  GPIO_Init(POWER_2_PORT, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin =  POWER_1_PIN_EN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(POWER_1_PORT_EN, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin =  POWER_2_PIN_EN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(POWER_2_PORT_EN, &GPIO_InitStructure);
}

void POWER_SetVolt(uint8_t volt)
{
	uint8_t vcode;
  POWER_1_OFF;
  POWER_2_OFF;
  Delay(10);
	volt_set = volt;
  GPIO_SetBits(POWER_1_PORT, POWER_1_PIN_VIDS); // Set to lowest voltage
  GPIO_SetBits(POWER_2_PORT, POWER_2_PIN_VIDS); // Set to lowest voltage
	if ((volt > 110)||(volt < 80)) {
		volt = 100;
	}
	vcode = VCODE[volt-80];
	if ((vcode & 0x01) != 0x01){
	  GPIO_ResetBits(POWER_1_PORT, POWER_1_PIN_VID0);
	  GPIO_ResetBits(POWER_2_PORT, POWER_2_PIN_VID0);
	}
	if ((vcode & 0x02) != 0x02){
	  GPIO_ResetBits(POWER_1_PORT, POWER_1_PIN_VID1);
	  GPIO_ResetBits(POWER_2_PORT, POWER_2_PIN_VID1);
	}
	if ((vcode & 0x04) != 0x04){
	  GPIO_ResetBits(POWER_1_PORT, POWER_1_PIN_VID2);
	  GPIO_ResetBits(POWER_2_PORT, POWER_2_PIN_VID2);
	}
	if ((vcode & 0x08) != 0x08){
	  GPIO_ResetBits(POWER_1_PORT, POWER_1_PIN_VID3);
	  GPIO_ResetBits(POWER_2_PORT, POWER_2_PIN_VID3);
	}
	if ((vcode & 0x10) != 0x10){
	  GPIO_ResetBits(POWER_1_PORT, POWER_1_PIN_VID4);
	  GPIO_ResetBits(POWER_2_PORT, POWER_2_PIN_VID4);
	}
  Delay(2);
	POWER_1_ON;
	POWER_2_ON;
  Delay(200);
	parcelDataUp.Volt = volt;
}
