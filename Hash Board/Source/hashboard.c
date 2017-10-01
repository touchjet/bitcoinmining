#include <string.h>
#include "stm32f10x.h"
#include "stm32f10x_it.h"
#include "uart.h"
#include "parcel.h"
#include "workpool.h"
#include "fb1600.h"
#include "power.h"
#include "led.h"
#include "temp.h"

void Delay(uint16_t ms)
{
	volatile uint16_t i,j;
	for (i=0;i<ms;i++){
		for (j=0;j<11000;j++){
			;
		}
	}
}

int main()
{	
	uint16_t i;
#ifdef PRODUCTION
	int j,k;
#else
	uint8_t reset;
	uint8_t reset_counter;
#endif	
    RCC_AHBPeriphClockCmd(RCC_AHBPeriph_DMA1, ENABLE);
    RCC_APB1PeriphClockCmd(RCC_APB1Periph_SPI2, ENABLE);
    RCC_APB2PeriphClockCmd(RCC_APB2Periph_ADC1 | RCC_APB2Periph_USART1 | RCC_APB2Periph_SPI1 | RCC_APB2Periph_GPIOA | RCC_APB2Periph_GPIOB | RCC_APB2Periph_GPIOC | RCC_APB2Periph_GPIOD | RCC_APB2Periph_GPIOE | RCC_APB2Periph_AFIO, ENABLE);
    
	// Disable JTAG
    GPIO_PinRemapConfig(GPIO_Remap_SWJ_JTAGDisable,ENABLE);

    SysTick_Config(72000); //1ms
	  TEMP_Init();
    LED_Init();
#ifdef PRODUCTION
    POWER_Init();
	  POWER_SetVolt(105);
    FB1600_Init();
	  if((ChipStats[0] == 0x0f)&&(ChipStats[1] == 0x0f)&&(ChipStats[2] == 0x0f)&&(ChipStats[3] == 0x0f)&&(ChipStats[4] == 0x0f)&&(ChipStats[5] == 0x0f)&&(ChipStats[6] == 0x0f)&&(ChipStats[7] == 0x0f)&&(ChipStats[8] == 0x0f)&&(ChipStats[9] == 0x0f)) {
			 LED_UART_ON;
		}
	  if((ChipStats[10] == 0x0f)&&(ChipStats[11] == 0x0f)&&(ChipStats[12] == 0x0f)&&(ChipStats[13] == 0x0f)&&(ChipStats[14] == 0x0f)&&(ChipStats[15] == 0x0f)&&(ChipStats[16] == 0x0f)&&(ChipStats[17] == 0x0f)&&(ChipStats[18] == 0x0f)&&(ChipStats[19] == 0x0f)) {
			 LED_CHIP_ON;
		}
	  POWER_1_OFF;
	  POWER_2_OFF;
    UART_Init(115200);
		USART_SendData(USART1, 27);
		Delay(1);
		USART_SendData(USART1, '[');
		Delay(1);
		USART_SendData(USART1, '2');
		Delay(1);
		USART_SendData(USART1, 'J');
		Delay(1);
		USART_SendData(USART1, 10);
		Delay(1);
		USART_SendData(USART1, 13);
		Delay(1);
		for (i=0;i<20;i++)
		{
		  USART_SendData(USART1, 0x30+(i/10));
			Delay(1);
		  USART_SendData(USART1, 0x30+(i%10));
			Delay(1);
		  USART_SendData(USART1, ' ');
			Delay(1);
			for (j=0;j<4;j++) 
			{
				for (k=0;k<8;k++) 
				{
				  if (((0x01 << k) & CoreOK[(i*4)+j]) == (0x01 << k)) {
						USART_SendData(USART1, '.');
					} else {
						USART_SendData(USART1, 'X');
					}
					Delay(1);
				}
		  	USART_SendData(USART1, ' ');
			  Delay(1);
			}
			USART_SendData(USART1, 10);
			Delay(1);
			USART_SendData(USART1, 13);
			Delay(1);
		}
	  while(1) 
		{
		}
#else	//NOT PRODUCTION	
    POWER_Init();
	  POWER_SetVolt(90);
		Delay(100);
    FB1600_Init();
    Parcel_Init();
    Pool_Init(1);
    UART_Init(1843200);
	  POWER_1_OFF;
	  POWER_2_OFF;
		reset_counter = 0;
    while(1) {
	    Parcel_Work();
      Pool_Task();
			//Restart if no work received in 20 seconds
			if (SysTickCounter > lastWorkTick+20000) {
          NVIC_SystemReset();
			}
			reset = 1;
			for (i=0;i<NUMBER_OF_CHIPS;i++) {
				if (OverHeated[i] == 0) {
					reset = 0;
				}
			}
			if (reset == 1) {
				reset_counter ++;
				if (reset_counter > 10) {
					POWER_1_OFF;
					POWER_2_OFF;
					Delay(60000);
					NVIC_SystemReset();
				}
			}
    }
#endif //PRODUCTION	
}


#ifdef USE_FULL_ASSERT
/*******************************************************************************
 * Function Name  : assert_failed
 * Description    : Reports the name of the source file and the source line number
 *                  where the assert_param error has occurred.
 * Input          : - file: pointer to the source file name
 *                  - line: assert_param error line source number
 * Output         : None
 * Return         : None
 *******************************************************************************/
void assert_failed(uint8_t* file, uint32_t line)
{
    /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
    /* Infinite loop */
    while (1)
    {}
}
#endif

