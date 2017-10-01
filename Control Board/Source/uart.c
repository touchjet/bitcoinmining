/*
 * uart.c
 *
 *  Created on: 22/11/2012
 *      Author: Zhen
 */
#include <string.h>
#include "misc.h"
#include "stm32f10x.h"
#include "stm32f10x_gpio.h"
#include "stm32f10x_rcc.h"
#include "stm32f10x_dma.h"
#include "stm32f10x_usart.h"
#include "uart.h"
#include "raspi.h"
#include "led.h"

#define NUMBER_OF_BOARDS  12

#define HASH_0_CS_PIN			GPIO_Pin_0
#define HASH_1_CS_PIN			GPIO_Pin_1
#define HASH_2_CS_PIN			GPIO_Pin_2
#define HASH_3_CS_PIN			GPIO_Pin_3
#define HASH_4_CS_PIN			GPIO_Pin_4
#define HASH_5_CS_PIN			GPIO_Pin_5
#define HASH_6_CS_PIN			GPIO_Pin_6
#define HASH_7_CS_PIN			GPIO_Pin_7
#define HASH_8_CS_PIN			GPIO_Pin_8
#define HASH_9_CS_PIN			GPIO_Pin_9
#define HASH_10_CS_PIN		GPIO_Pin_10
#define HASH_11_CS_PIN		GPIO_Pin_11
#define HASH_CS_PORT			GPIOC


volatile uint8_t UART_Rx_Buffer[UART_RX_BUF_BYTES];
volatile uint16_t UART_Rx_Buffer_RdPtr;
volatile uint16_t UART_Rx_Buffer_WrPtr;
volatile uint8_t UART_Board;

__inline void UART_Delay(uint32_t ms)
{
	volatile uint16_t i,j;
	for (i=0;i<ms;i++){
		for (j=0;j<4000;j++){
			;
		}
	}	
}

void UART_Init(uint32_t BaudRate)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;

	UART_Board = 0;

	//CS
  GPIO_InitStructure.GPIO_Pin = HASH_0_CS_PIN | HASH_1_CS_PIN | HASH_2_CS_PIN | HASH_3_CS_PIN | HASH_4_CS_PIN | HASH_5_CS_PIN | HASH_6_CS_PIN | HASH_7_CS_PIN | HASH_8_CS_PIN | HASH_9_CS_PIN | HASH_10_CS_PIN | HASH_11_CS_PIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_10MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(HASH_CS_PORT, &GPIO_InitStructure);

	/*UART 1 RX*/
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_Init(GPIOA, &GPIO_InitStructure);

	/*UART 1 TX*/
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(GPIOA, &GPIO_InitStructure);

	/*UART 3 RX*/
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;
  GPIO_Init(GPIOB, &GPIO_InitStructure);

	/*UART 3 TX*/
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(GPIOB, &GPIO_InitStructure);

	/* Configure USART1 */
	USART_InitStructure.USART_BaudRate = BaudRate;  
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;
	USART_InitStructure.USART_StopBits = USART_StopBits_1;
	USART_InitStructure.USART_Parity = USART_Parity_No;
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;
	USART_Init(USART1, &USART_InitStructure);
	USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);
	USART_Cmd(USART1, ENABLE);

	/* Configure USART3 */
	USART_InitStructure.USART_BaudRate = BaudRate;  
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;
	USART_InitStructure.USART_StopBits = USART_StopBits_1;
	USART_InitStructure.USART_Parity = USART_Parity_No;
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;
	USART_Init(USART3, &USART_InitStructure);
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);
	USART_Cmd(USART3, ENABLE);	
}

void UART1_Write(uint8_t *Data, uint8_t Block)
{
	uint16_t i;
	for (i=0;i<DB_DATA_SIZE;i++) {
    USART_SendData(USART1, *Data);
		Data ++;
    while(USART_GetFlagStatus(USART1, USART_FLAG_TXE) == RESET)
    {
    }
  }
}

void UART3_Write(uint8_t *Data, uint8_t Block)
{
	uint16_t i;
	for (i=0;i<DB_DATA_SIZE;i++) {
    USART_SendData(USART3, *Data);
		Data ++;
    while(USART_GetFlagStatus(USART3, USART_FLAG_TXE) == RESET)
    {
    }
  }
}

__inline void UART_CS(uint8_t cs, uint8_t level)
{	uint16_t mask = 0x0001 << cs;
	if (level ==0) {
		GPIO_ResetBits(HASH_CS_PORT,mask);
	} else {
		GPIO_SetBits(HASH_CS_PORT,0x0fff);
	}
}

void UART_Work(void)
{
	uint16_t i;
	uint8_t pingpong = RASPI_PingPong == 0 ? 1:0;
	uint16_t ptr = 0;
	uint16_t cnt;
	while (UART_Board < NUMBER_OF_BOARDS) {
  	if (RASPI_Buff_Switch == 1) {
    	if ((RASPI_Rx_Buff[pingpong][0][1]!=0x00)&&(RASPI_Rx_Buff[pingpong][0][1]!=0x01)&&(RASPI_Rx_Buff[pingpong][0][1]!=0x02)&&(RASPI_Rx_Buff[pingpong][0][1]!=0x88)) {
	  	  UART_Delay(100);
	      NVIC_SystemReset();
		  }
		  RASPI_Buff_Switch = 0;
	    RASPI_Reset_DMA();
		  return;
    }
  	UART_Rx_Buffer_RdPtr = 0;
	  UART_Rx_Buffer_WrPtr = 0;
		
		UART_CS(UART_Board,0);
		UART_Delay(1);
		cnt = 0;
		for (i=0;i<RASPI_BUFF_SIZE;i++) {
		  if ((RASPI_Rx_Buff[pingpong][i][0] == DB_START_SENTINEL)&&(RASPI_Rx_Buff[pingpong][i][DB_DATA_SIZE-1] == UART_Board))	{
			  RASPI_Rx_Buff[pingpong][i][DB_DATA_SIZE-1] = DB_FINISH_SENTINEL;
				if (UART_Board < (NUMBER_OF_BOARDS >> 1)) {
  			  UART1_Write((uint8_t *)&RASPI_Rx_Buff[pingpong][i][0],1);	
				} else {
	  		  UART3_Write((uint8_t *)&RASPI_Rx_Buff[pingpong][i][0],1);	
				}
				RASPI_Rx_Buff[pingpong][i][0] = 0x88;
  			if (cnt>10) {
	  			break;
		  	}
			  cnt ++;
		  }
	  }
		if (cnt < 5) {
			UART_Delay(1);
		}
		UART_Delay(1);
		UART_CS(UART_Board,1);
		//Wait 2 miliseconds so hash board can finish started transmission 	
		UART_Delay(2);
  	ptr = 0;
	  if (UART_Rx_Buffer_WrPtr != 0) {
      while(ptr+DB_DATA_SIZE-2<UART_Rx_Buffer_WrPtr){
		    while(UART_Rx_Buffer[ptr] != DB_START_SENTINEL){
			    ptr++;
	    	}
			  if((ptr+DB_DATA_SIZE-2<UART_Rx_Buffer_WrPtr)&&(UART_Rx_Buffer[ptr+DB_DATA_SIZE-1] == DB_FINISH_SENTINEL)) {
				  UART_Rx_Buffer[ptr+DB_DATA_SIZE-1] = UART_Board;
			    memcpy((void *)RASPI_Tx_Buff[pingpong][RASPI_Tx_Ptr], (void *)&UART_Rx_Buffer[ptr],DB_DATA_SIZE);
				  if (RASPI_Tx_Ptr < RASPI_BUFF_SIZE - 1) {
					  RASPI_Tx_Ptr++;
					}
			  	ptr += DB_DATA_SIZE;
			  } else {
			    ptr++;
		    }
	    }
    }
		UART_Board ++;
	}	
	if (UART_Board >= NUMBER_OF_BOARDS) {
		UART_Board = 0;
	}
}
