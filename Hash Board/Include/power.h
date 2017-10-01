/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __POWER_H
#define __POWER_H

#ifdef __cplusplus
 extern "C" {
#endif

#define POWER_1_PORT		GPIOB
#define POWER_1_PORT_EN	GPIOB
#define POWER_2_PORT		GPIOE
#define POWER_2_PORT_EN	GPIOB

	 
#define POWER_1_PIN_VID0 	GPIO_Pin_8
#define POWER_1_PIN_VID1 	GPIO_Pin_9
#define POWER_1_PIN_VID2 	GPIO_Pin_10
#define POWER_1_PIN_VID3 	GPIO_Pin_11
#define POWER_1_PIN_VID4 	GPIO_Pin_12
	 
#define POWER_2_PIN_VID0 	GPIO_Pin_10
#define POWER_2_PIN_VID1 	GPIO_Pin_11
#define POWER_2_PIN_VID2 	GPIO_Pin_12
#define POWER_2_PIN_VID3 	GPIO_Pin_13
#define POWER_2_PIN_VID4 	GPIO_Pin_14
	 
#define POWER_1_PIN_VIDS 	POWER_1_PIN_VID0 | POWER_1_PIN_VID1	| POWER_1_PIN_VID2 | POWER_1_PIN_VID3 | POWER_1_PIN_VID4
#define POWER_1_PIN_EN 	GPIO_Pin_6

#define POWER_2_PIN_VIDS 	POWER_2_PIN_VID0 | POWER_2_PIN_VID1	| POWER_2_PIN_VID2 | POWER_2_PIN_VID3 | POWER_2_PIN_VID4
#define POWER_2_PIN_EN 	GPIO_Pin_7
	 
#define POWER_1_ON 		GPIO_WriteBit(POWER_1_PORT_EN, POWER_1_PIN_EN , Bit_SET); 
#define POWER_1_OFF 		GPIO_WriteBit(POWER_1_PORT_EN, POWER_1_PIN_EN , Bit_RESET); 

#define POWER_2_ON 		GPIO_WriteBit(POWER_2_PORT_EN, POWER_2_PIN_EN , Bit_SET); 
#define POWER_2_OFF 		GPIO_WriteBit(POWER_2_PORT_EN, POWER_2_PIN_EN , Bit_RESET); 

void POWER_Init(void);
void POWER_SetVolt(uint8_t volt);

#ifdef __cplusplus
}
#endif
   
#endif
