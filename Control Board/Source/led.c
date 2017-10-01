#include "stm32f10x.h"
#include "led.h"

void LED_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;
  
  /* Configure LED as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = LED_PIN_1 | LED_PIN_2;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(GPIOB, &GPIO_InitStructure);
  GPIO_ResetBits(GPIOB, LED_PIN_1 | LED_PIN_2);
	
	LED_RUN_OFF;
	LED_SPI_OFF;
}
