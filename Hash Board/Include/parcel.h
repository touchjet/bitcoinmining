/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __PARCEL_H
#define __PARCEL_H

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f10x.h"
#include "protocol.h"

#define PARCEL_PORT	   GPIOA
#define PARCEL_CS_PIN  GPIO_Pin_4

extern volatile StructHashResult parcelDataUp;
   
void ParseDownloadParcel(void);
void Parcel_Init(void);
void Parcel_SendData(uint32_t uniqueId, uint32_t nonce, uint32_t difficulty, uint8_t core);
void Parcel_Work(void);

#ifdef __cplusplus
}
#endif
   
#endif
