/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __WORKPOOL_H
#define __WORKPOOL_H

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f10x.h"
#include "parcel.h"
#include "protocol.h"
#include "fb1600.h"

#define WORK_BUFFER_SIZE 				16	 
#define NONCE_CORE_POOL_SIZE  	7  
   
typedef __packed struct {
     uint32_t UniqueId;
		 uint32_t NTime;
		 uint8_t WorkId;
		 uint8_t MidState[32];
     uint8_t RestData[12];
   } HashWork;
   
typedef __packed struct {
     uint32_t UniqueIds[NONCE_CORE_POOL_SIZE];
		 uint8_t WorkIds[NONCE_CORE_POOL_SIZE];
     uint8_t Write_Ptr;
     uint8_t Available;
   } NonceCore;

extern volatile uint32_t lastWorkTick;
extern volatile uint8_t WorkMaxRollCount;

void Pool_Init(uint32_t difficulty);
void Pool_AddWork(StructHashData *dataDown);
void Pool_Task(void);

#ifdef __cplusplus
}
#endif

#endif
