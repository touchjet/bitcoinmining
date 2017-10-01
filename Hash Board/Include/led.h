/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __LED_H
#define __LED_H

#ifdef __cplusplus
 extern "C" {
#endif

#define LED_UART_PIN 	GPIO_Pin_11
#define LED_CHIP_PIN 	GPIO_Pin_12

#define LED_UART_ON		GPIOA->BSRR = GPIO_Pin_11;
#define LED_UART_OFF	GPIOA->BRR = GPIO_Pin_11;
#define LED_CHIP_ON		GPIOA->BSRR = GPIO_Pin_12; 
#define LED_CHIP_OFF	GPIOA->BRR = GPIO_Pin_12;

void LED_Init(void);

#ifdef __cplusplus
}
#endif
   
#endif
