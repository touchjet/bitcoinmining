/*
 * uart.h
 *
 *  Created on: 22/11/2012
 *      Author: Zhen
 */

#ifndef UART_H_
#define UART_H_

#include "stm32f10x.h"
#include "protocol.h"

#define UART_RX_BUF_BYTES  DB_DATA_SIZE*20

extern volatile uint8_t UART_Rx_Buffer[UART_RX_BUF_BYTES];
extern volatile uint16_t UART_Rx_Buffer_RdPtr;
extern volatile uint16_t UART_Rx_Buffer_WrPtr;

void UART_Init(uint32_t BaudRate);
void UART_Work(void);

#endif /* UART_H_ */
