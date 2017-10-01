#include "stm32f10x.h"
#include "led.h"

void LED_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;
  
  /* Configure LED as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = LED_UART_PIN | LED_CHIP_PIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_2MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
  GPIO_ResetBits(GPIOA, LED_UART_PIN | LED_CHIP_PIN);
}
