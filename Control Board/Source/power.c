#include "stm32f10x.h"
#include "power.h"

void POWER_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;
  
  /* Configure POWER as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_12;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_10MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
  GPIO_SetBits(GPIOA, GPIO_Pin_12);
	
}	
