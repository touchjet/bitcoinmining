#include <string.h>
#include "parcel.h"
#include "workpool.h"
#include "power.h"
#include "fb1600.h"
#include "uart.h"
#include "temp.h"
#include "stm32f10x_it.h"

volatile uint8_t ParcelRxBuffer[DB_DATA_SIZE];
volatile uint8_t ParcelRxBufferPtr;

volatile StructHashResult parcelDataUp = {
		DB_START_SENTINEL,
		DB_DATA_TYPE_HASH_RESULT,
		0x00000000,
		0x00000000,
		0x00000000,
    	0x00,
    	0x00,
    	0x00,
    	0x00,
		{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
		 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,},
		0x01, //NeedConfig
		0x00,
		{ 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,},
		0x00,
		DB_FINISH_SENTINEL
};

volatile StructRequestData parcelReqeustData = {
		DB_START_SENTINEL,
		DB_DATA_TYPE_REQUEST_DATA,
		{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
		 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
		 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
		 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,},
		0x01,
		{0x00,0x00,0x00,0x00,0x00,0x00,0x00},
		0x00,
		DB_FINISH_SENTINEL
};

volatile uint8_t ReqDataCounter;
volatile uint8_t serial;

#define PARCEL_TX_BUF_SIZE 32

volatile StructHashResult PARCEL_Tx_Buffer[PARCEL_TX_BUF_SIZE];
volatile uint8_t PARCEL_Tx_Buffer_WrPtr = 0;
volatile uint8_t PARCEL_Tx_Buffer_RdPtr = 0;

void Parcel_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;

  /* Configure CS as Input pull_up */
  GPIO_InitStructure.GPIO_Pin = PARCEL_CS_PIN;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_Init(PARCEL_PORT, &GPIO_InitStructure);

  ParcelRxBufferPtr = 0;
	ReqDataCounter = 0;
}


__inline void ParcelGotChar(uint8_t Char)
{
  if (ParcelRxBufferPtr == 0) {
    if (Char == DB_START_SENTINEL) {
      ParcelRxBuffer[0] = Char;
      ParcelRxBufferPtr = 1;
    } 
  } else if (ParcelRxBufferPtr >= DB_DATA_SIZE - 1 ) {
    if (Char == DB_FINISH_SENTINEL) {
			if ((ParcelRxBuffer[1] == DB_DATA_TYPE_HASH_DATA)||(ParcelRxBuffer[1] == DB_DATA_TYPE_HASH_DATA_REF)) {
		    Pool_AddWork((StructHashData *)(ParcelRxBuffer));
			} else if  (ParcelRxBuffer[1] == DB_DATA_TYPE_CONFIG) {
        	StructHardwareConfig *config = (StructHardwareConfig *)ParcelRxBuffer;
        	uint8_t pll = config->PLL;
        	uint32_t diff = (uint32_t)config->Difficulty[0] << 24;
        	diff += (uint32_t)config->Difficulty[1] << 16;
        	diff += (uint32_t)config->Difficulty[2] << 8;
        	diff += (uint32_t)config->Difficulty[3];
        	POWER_SetVolt(config->Voltage1);
					FB1600_Reset(pll,diff);
					Pool_Init(diff);
					parcelDataUp.NeedConfig = 0;
				  parcelReqeustData.NeedConfig = 0;
      }
    }
    ParcelRxBufferPtr = 0;
  } else {
  	ParcelRxBuffer[ParcelRxBufferPtr] = Char;
    ParcelRxBufferPtr ++;
  }
}

__inline uint32_t swap_uint32( uint32_t val )
{
    val = ((val << 8) & 0xFF00FF00 ) | ((val >> 8) & 0xFF00FF ); 
    return (val << 16) | (val >> 16);
}

void Parcel_SendData(uint32_t uniqueId, uint32_t nonce, uint32_t difficulty, uint8_t core)
{
	uint8_t i;
	uint8_t nextPtr;
	ReqDataCounter ++;
	if (core != 0xff) {
	  if (difficulty == 0) 
  	{
	  	if ((core % 2) == 0) {
		  	parcelReqeustData.CoreResult[core >> 1] += 0x01;
		  } else {
			  parcelReqeustData.CoreResult[core >> 1] += 0x10;
		  }
	  } else {
      nextPtr = (PARCEL_Tx_Buffer_WrPtr + 1) % PARCEL_TX_BUF_SIZE;
      if ( nextPtr != PARCEL_Tx_Buffer_RdPtr) {
        parcelDataUp.UniqueId = uniqueId;
        parcelDataUp.Nonce = nonce;
        parcelDataUp.Difficulty = difficulty;
        parcelDataUp.Core = core;
        parcelDataUp.Chip = core >> 2;
	  	  parcelDataUp.BoardTemp = TEMP_GetTemp();
				parcelDataUp.SN = serial;
   			serial ++;
        memcpy((void *)&PARCEL_Tx_Buffer[PARCEL_Tx_Buffer_WrPtr],(void *)&parcelDataUp, DB_DATA_SIZE);
        PARCEL_Tx_Buffer_WrPtr = nextPtr; 
      }
	  }
  }
	if ((ReqDataCounter >= WorkMaxRollCount)||(core == 0xff))
	{
		ReqDataCounter= 0;
    nextPtr = (PARCEL_Tx_Buffer_WrPtr + 1) % PARCEL_TX_BUF_SIZE;
    if ( nextPtr != PARCEL_Tx_Buffer_RdPtr) {
			parcelReqeustData.SN = serial;
			serial ++;
      memcpy((void *)&PARCEL_Tx_Buffer[PARCEL_Tx_Buffer_WrPtr],(void *)&parcelReqeustData, DB_DATA_SIZE);
      PARCEL_Tx_Buffer_WrPtr = nextPtr; 
			for(i=0;i<40;i++) {
			  parcelReqeustData.CoreResult[i] = 0;
			}
	  }
	}
}

void Parcel_Work(void)
{
  uint16_t wrptr = UART_Rx_Buffer_WrPtr;
  uint16_t rdptr = UART_Rx_Buffer_RdPtr;
	uint16_t i;
	while (rdptr != wrptr) {
	  ParcelGotChar(UART_Rx_Buffer[rdptr]);
	  rdptr ++;
	  rdptr %= UART_RX_BUF_BYTES;
	}
	UART_Rx_Buffer_RdPtr = rdptr;
	for ( i=0;i<5;i++) { 
	  if ((PARCEL_PORT->IDR & PARCEL_CS_PIN) == (uint32_t)Bit_RESET) {
		  if (PARCEL_Tx_Buffer_WrPtr != PARCEL_Tx_Buffer_RdPtr) {
  		  UART_Write((uint8_t *)&PARCEL_Tx_Buffer[PARCEL_Tx_Buffer_RdPtr],DB_DATA_SIZE,0);
			  PARCEL_Tx_Buffer_RdPtr = (PARCEL_Tx_Buffer_RdPtr + 1) % PARCEL_TX_BUF_SIZE;
		  } else {
				break;
			}
	  } else {
		  break;
		}
  }
}
