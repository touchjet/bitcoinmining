/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __PROTOCOL_H
#define __PROTOCOL_H

#ifdef __cplusplus
 extern "C" {
#endif
#include "stm32f10x.h"

#define DB_DATA_SIZE                52
#define DB_START_SENTINEL           0xaa
#define DB_FINISH_SENTINEL          0x55

#define DB_DATA_TYPE_CONFIG			0x00
#define DB_DATA_TYPE_HASH_DATA		0x01
#define DB_DATA_TYPE_HASH_RESULT	0x10

typedef __packed struct {
     uint8_t StartSentinel;		//0xaa
     uint8_t DataType;			//0x00
     uint8_t PLL;				//ASIC PLL Setting
     uint8_t Voltage;			//ASIC Core Voltage
     uint8_t Difficulty[4];		//Share Difficulty, should always be power of 2
     uint8_t Reserved[43];		//Reserved for future use
     uint8_t FinishSentinel;	//0x55
   } StructHardwareConfig;

typedef __packed struct {
     uint8_t StartSentinel;		//0xaa
     uint8_t BlockType;				//0x01
     uint32_t UniqueId;				//32 bit unique Hash Job Id
     uint8_t MidState[32];		//Bitcoin Block Header Midstate
     uint8_t RestData[12];		//Bitcoin Block Header rest data
     uint8_t Core;						//Nonce Core to process the data
     uint8_t FinishSentinel;	//0x55
   } StructHashData;

typedef __packed struct {
     uint8_t StartSentinel;		//0xaa
     uint8_t DataType;			//0x10
     uint32_t UniqueId;			//32 bit unique Hash Job Id
     uint32_t Nonce;			//Nonce
     uint32_t Difficulty;		//Share difficulty calculated with the result Nonce. 0 means invalid result or no result.
     uint8_t Board;				//Board Number of the result.
     uint8_t Chip;				//Chip Number of the result.
     uint8_t Core;				//Core Number of the result.
     uint8_t BoardTemp;			//Board temprature
     uint8_t ChipTemp[8];		//Chip tempratures
     uint8_t FanSpeed[2];		//Fan speeds
     uint8_t NeedConfig;		//Require Configuration Data
     uint8_t Chips[8];			//Chip Status
     uint8_t Reserved[14];		//Reserved for future use
     uint8_t FinishSentinel;	//0x55
   } StructHashResult;

#ifdef __cplusplus
}
#endif
   
#endif
