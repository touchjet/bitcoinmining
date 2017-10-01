#include <string.h>
#include "stm32f10x.h"
#include "stm32f10x_it.h"
#include "uart.h"
#include "raspi.h"
#include "led.h"
#include "power.h"

void Delay(uint32_t ms)
{
  volatile uint64_t endtick = SysTickCounter + ms;
	while (SysTickCounter < endtick) ;
}

int main() 
{
	uint32_t ledNextTick = 500;
	uint32_t ledOn = 0;
	
	NVIC_InitTypeDef NVIC_InitStructure;

  SysTick_Config(72000); //1ms
	
	/* DMA clock enable */
	RCC_AHBPeriphClockCmd(RCC_AHBPeriph_DMA1, ENABLE);

  /* Enable SPI, UART and GPIO clocks */
  RCC_APB2PeriphClockCmd(RCC_APB2Periph_USART1 | RCC_APB2Periph_SPI1 | RCC_APB2Periph_GPIOA | RCC_APB2Periph_GPIOB | RCC_APB2Periph_GPIOC | RCC_APB2Periph_AFIO, ENABLE);
  RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);

  // Disable JTAG
  GPIO_PinRemapConfig(GPIO_Remap_SWJ_JTAGDisable,ENABLE);

	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,DISABLE);
	
	NVIC_PriorityGroupConfig(NVIC_PriorityGroup_1);
	
  //Enable DMA1 channel IRQ Channel 
  NVIC_InitStructure.NVIC_IRQChannel = DMA1_Channel2_IRQn;
  NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
  NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;
  NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
  NVIC_Init(&NVIC_InitStructure);

	/* Enable the USART1 Interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USART1_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
	NVIC_Init(&NVIC_InitStructure);

	/* Enable the USART3 Interrupt */
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 1;
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
	NVIC_Init(&NVIC_InitStructure);

  POWER_Init();
	LED_Init();
  UART_Init(1843200);
  RASPI_Init();
	
  while (1) {
		UART_Work();
		RASPI_Work();
		if (SysTickCounter > ledNextTick) {
			ledNextTick = SysTickCounter + 500;
			if (ledOn){
				ledOn = 0;
				LED_RUN_OFF;
			} else {
				ledOn = 1;
				LED_RUN_ON;
			}
		}
		if (SysTickCounter > RASPI_NextTick) {
			RASPI_NextTick = SysTickCounter + 300000;
			RASPI_OFF;
			Delay(5000);
			RASPI_ON;
		}
  }
}
