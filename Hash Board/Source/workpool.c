#include <string.h>
#include "workpool.h"
#include "parcel.h"
#include "fb1600.h"
#include "led.h"

#define START_CORE	0
#define STOP_CORE NUMBER_OF_CORES - 1
static volatile HashWork WorkBuffer[WORK_BUFFER_SIZE];
static volatile uint8_t WorkBuffer_WrPtr;
static volatile uint8_t WorkBuffer_RdPtr;
static volatile uint8_t WorkBuffer_RdPtrEx;
static volatile uint8_t WorkBuffer_Depth;

volatile uint8_t TxBuffer[45];

static volatile NonceCore WorkPool[NUMBER_OF_CORES];

volatile uint8_t WorkMaxRollCount;

extern volatile uint32_t SysTickCounter;
extern volatile uint8_t RxBuffer[45];

static volatile uint32_t LastEmptyTick = 0;
static volatile uint32_t Difficulty = 16;

volatile uint32_t lastWorkTick;

void Pool_Init(uint32_t difficulty)
{
  uint8_t i,j;
	WorkMaxRollCount = 4;
	WorkBuffer_WrPtr = 0;
	WorkBuffer_RdPtr = 0;
	WorkBuffer_RdPtrEx = 0;
	WorkBuffer_Depth = 0;
  Difficulty = difficulty;
  for (i=0;i<NUMBER_OF_CORES;i++) {
    if ((ChipStats[i >> 2] & (1 << (i % 4)))!=0) {
      WorkPool[i].Available = 1;
      WorkPool[i].Write_Ptr = 0;
      for (j=0;j<NONCE_CORE_POOL_SIZE;j++) {
        WorkPool[i].WorkIds[j] = 0xff;
      }
    } else {
      WorkPool[i].Available = 0;
    }
  }
}

__inline uint32_t Pool_GetUniqueIdById(NonceCore *pool, uint8_t id)
{
	if (id >= NONCE_CORE_POOL_SIZE) {
		return 0xffffffff;
	}
	if (pool->WorkIds[id] == id) {
		pool->WorkIds[id] = 0xff;
		return pool->UniqueIds[id];
	} else {
		return 0xffffffff;
	}
}

void Pool_Task()
{
  uint8_t i;
  uint8_t rxBase;
  uint8_t id;
  NonceCore *pool;
	uint32_t resultUniqueId;
	uint32_t ntime;
	
	FB1600_ScanIRQ();
  for (i=START_CORE;i<=STOP_CORE;i++) {
    pool = (NonceCore *)&WorkPool[i];
    if (pool->Available) {
      if ((WorkBuffer_Depth != 0) && (CoreReady[i]) && ((OverHeated[i >> 2] == 0)||((i % 4) == 0))) {
				pool->UniqueIds[pool->Write_Ptr] = WorkBuffer[WorkBuffer_RdPtr].UniqueId;
				pool->WorkIds[pool->Write_Ptr] = pool->Write_Ptr;
				WorkBuffer[WorkBuffer_RdPtr].WorkId = pool->Write_Ptr;
        FB1600_Comm(i, (uint8_t *)&(WorkBuffer[WorkBuffer_RdPtr].WorkId));
				
				pool->Write_Ptr ++;
				pool->Write_Ptr %= NONCE_CORE_POOL_SIZE;
				
				WorkBuffer_RdPtrEx ++;
				WorkBuffer[WorkBuffer_RdPtr].UniqueId ++;
				ntime = WorkBuffer[WorkBuffer_RdPtr].NTime+1;
				WorkBuffer[WorkBuffer_RdPtr].RestData[7] = (uint8_t)((ntime>>24) & 0x000000ff);
				WorkBuffer[WorkBuffer_RdPtr].RestData[6] = (uint8_t)((ntime>>16) & 0x000000ff);
				WorkBuffer[WorkBuffer_RdPtr].RestData[5] = (uint8_t)((ntime>>8) & 0x000000ff);
				WorkBuffer[WorkBuffer_RdPtr].RestData[4] = (uint8_t)(ntime & 0x000000ff);
				WorkBuffer[WorkBuffer_RdPtr].NTime=ntime;
				if (WorkBuffer_RdPtrEx >= WorkMaxRollCount) {
					WorkBuffer_RdPtrEx = 0;
					WorkBuffer_RdPtr ++;
					WorkBuffer_RdPtr %= WORK_BUFFER_SIZE;
					WorkBuffer_Depth --;
				}
				
        rxBase = (i%4)*5+6;
        id = RxBuffer[rxBase] & 0x0f;
        resultUniqueId = Pool_GetUniqueIdById(pool,id);
        if (resultUniqueId != 0xffffffff) {
          if ((RxBuffer[rxBase]&0xf0)!=0) {
            uint32_t nonce = ((uint32_t)RxBuffer[rxBase+1] << 24); 
            nonce += ((uint32_t)RxBuffer[rxBase+2] << 16);
            nonce += ((uint32_t)RxBuffer[rxBase+3] << 8);
            nonce += (uint32_t)RxBuffer[rxBase+4];
            Parcel_SendData(resultUniqueId,nonce,Difficulty,i);
          } else {
            Parcel_SendData(resultUniqueId,0,0,i);
          }
        }
      } 
    }
  }  
  if (WorkBuffer_Depth<WORK_BUFFER_SIZE-1) {
    if (SysTickCounter > LastEmptyTick) {
			if (parcelDataUp.NeedConfig != 0x00) {
        LastEmptyTick = SysTickCounter+200;
			} else {
        LastEmptyTick = SysTickCounter+50;
			}
      Parcel_SendData(0,0,0,0xff);
    }
  }
}

void Pool_AddWork(StructHashData *dataDown)
{
	uint8_t i,j;
	lastWorkTick = SysTickCounter;
	
	if (dataDown->BlockType == DB_DATA_TYPE_HASH_DATA_REF)
	{
	  WorkBuffer_WrPtr = 0;
	  WorkBuffer_RdPtr = 0;
	  WorkBuffer_RdPtrEx = 0;
	  WorkBuffer_Depth = 0;
    for (i=0;i<NUMBER_OF_CORES;i++) {
      WorkPool[i].Write_Ptr = 0;
      for (j=0;j<NONCE_CORE_POOL_SIZE;j++) {
        WorkPool[i].WorkIds[j] = 0xff;
      }
    }
	}
	
	if (WorkBuffer_Depth < WORK_BUFFER_SIZE) {
	  WorkBuffer[WorkBuffer_WrPtr].UniqueId = dataDown->UniqueId;
		if (dataDown->MaxRoll > 0) {
	    WorkMaxRollCount = dataDown->MaxRoll;
		}
    memcpy((void *)(&(WorkBuffer[WorkBuffer_WrPtr].MidState)), (void *)(&(dataDown->MidState)),44); //UniqueId and BlockHeader data
	  WorkBuffer[WorkBuffer_WrPtr].NTime = (uint32_t)dataDown->RestData[4] + ((uint32_t)dataDown->RestData[5] << 8) + ((uint32_t)dataDown->RestData[6] << 16) + ((uint32_t)dataDown->RestData[7] << 24);
	
	  WorkBuffer_WrPtr ++;
	  WorkBuffer_WrPtr %= WORK_BUFFER_SIZE;
	  WorkBuffer_Depth++;
	}
}

