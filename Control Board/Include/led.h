/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __LED_H
#define __LED_H

#ifdef __cplusplus
 extern "C" {
#endif

#define LED_PIN_1 	GPIO_Pin_0
#define LED_PIN_2 	GPIO_Pin_1

#define LED_RUN_ON 		GPIO_SetBits(GPIOB, LED_PIN_1);
#define LED_RUN_OFF 	GPIO_ResetBits(GPIOB, LED_PIN_1);
#define LED_SPI_ON		GPIO_SetBits(GPIOB, LED_PIN_2);
#define LED_SPI_OFF		GPIO_ResetBits(GPIOB, LED_PIN_2);


void LED_Init(void);

#ifdef __cplusplus
}
#endif
   
#endif
