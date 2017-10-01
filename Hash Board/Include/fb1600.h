/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __FB1600_H
#define __FB1600_H

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f10x.h"

#define NUMBER_OF_CHIPS         20   
#define NUMBER_OF_CORES         NUMBER_OF_CHIPS*4  
	 
extern volatile uint8_t ChipStats[NUMBER_OF_CHIPS];
extern volatile uint8_t CoreOK[NUMBER_OF_CORES];
extern volatile uint8_t CoreReady[NUMBER_OF_CORES];
extern volatile uint8_t OverHeated[NUMBER_OF_CHIPS];

void FB1600_Init(void);
void FB1600_Reset(uint8_t pll,uint32_t difficulty);
void FB1600_ScanIRQ(void);
void FB1600_Comm(uint8_t core, uint8_t *sendbuffer);

#ifdef __cplusplus
}
#endif

#endif
