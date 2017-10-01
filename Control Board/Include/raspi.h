#ifndef RASPI_H_
#define RASPI_H_

#include "protocol.h"

#define RASPI_BUFF_SIZE 192

extern volatile uint8_t RASPI_Rx_Buff[2][RASPI_BUFF_SIZE][DB_DATA_SIZE];
extern volatile uint8_t RASPI_Tx_Buff[2][RASPI_BUFF_SIZE][DB_DATA_SIZE];

extern volatile uint8_t RASPI_PingPong;
extern volatile uint16_t RASPI_Rx_Ptr;
extern volatile uint16_t RASPI_Tx_Ptr;
extern volatile uint8_t RASPI_Buff_Switch;
extern volatile uint64_t RASPI_NextTick;

#define RASPI_ON 		GPIO_SetBits(GPIOA, GPIO_Pin_11);
#define RASPI_OFF 	GPIO_ResetBits(GPIOA, GPIO_Pin_11);

void RASPI_Init(void);
void RASPI_Reset_DMA(void);
void RASPI_Work(void);

#endif /* RASPI_H_ */
