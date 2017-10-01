/**
  ******************************************************************************
  * @file    stm32_it.c
  * @author  MCD Application Team
  * @version V4.0.0
  * @date    21-January-2013
  * @brief   Main Interrupt Service Routines.
  *          This file provides template for all exceptions handler and peripherals
  *          interrupt service routine.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; COPYRIGHT 2013 STMicroelectronics</center></h2>
  *
  * Licensed under MCD-ST Liberty SW License Agreement V2, (the "License");
  * You may not use this file except in compliance with the License.
  * You may obtain a copy of the License at:
  *
  *        http://www.st.com/software_license_agreement_liberty_v2
  *
  * Unless required by applicable law or agreed to in writing, software 
  * distributed under the License is distributed on an "AS IS" BASIS, 
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
  *
  ******************************************************************************
  */

/* Includes ------------------------------------------------------------------*/
#include "stm32f10x_it.h"
#include "stm32f10x_dma.h"
#include "stm32f10x_usart.h"
#include "stm32f10x.h"
#include "uart.h"
#include "stdlib.h"
#include "string.h"
#include "led.h"
#include "power.h"
#include "parcel.h"

volatile uint32_t SysTickCounter = 0;

/******************************************************************************/
/*            Cortex-M3 Processor Exceptions Handlers                         */
/******************************************************************************/

/*******************************************************************************
* Function Name  : NMI_Handler
* Description    : This function handles NMI exception.
* Input          : None
* Output         : None
* Return         : None
*******************************************************************************/

void NMI_Handler(void)
{
  if (CoreDebug->DHCSR & 1) {  //check C_DEBUGEN == 1 -> Debugger Connected  
      __breakpoint(0);  // halt program execution here         
  }
  POWER_1_OFF;
  POWER_2_OFF;
  NVIC_SystemReset();
}

/*******************************************************************************
* Function Name  : HardFault_Handler
* Description    : This function handles Hard Fault exception.
* Input          : None
* Output         : None
* Return         : None
*******************************************************************************/

void HardFault_Handler(void)
{
  if (CoreDebug->DHCSR & 1) {  //check C_DEBUGEN == 1 -> Debugger Connected  
      __breakpoint(0);  // halt program execution here         
  }
  POWER_1_OFF;
  POWER_2_OFF;
  NVIC_SystemReset();
}

/*******************************************************************************
* Function Name  : SysTick_Handler
* Description    : This function handles SysTick Handler.
* Input          : None
* Output         : None
* Return         : None
*******************************************************************************/
void SysTick_Handler(void)
{
  SysTickCounter ++;
}

void DMA1_Channel4_IRQHandler(void)
{
  if(DMA_GetITStatus(DMA1_IT_TC4) == SET) 
  {
    DMA_ClearITPendingBit(DMA1_IT_TC4);
    UART_Sending = 0;
  }
}

/**
  * @brief  This function handles UART1 interrupt request.
  * @param  None
  * @retval None
  */

void USART1_IRQHandler(void)
{
  if(USART_GetITStatus(USART1, USART_IT_RXNE) != RESET)
  {
    USART_ClearITPendingBit(USART1, USART_IT_RXNE);
		if ((PARCEL_PORT->IDR & PARCEL_CS_PIN) == (uint32_t)Bit_RESET) {
    	LED_UART_ON;
	    UART_Rx_Buffer[UART_Rx_Buffer_WrPtr] = USART1->DR;
	    UART_Rx_Buffer_WrPtr ++;
	    UART_Rx_Buffer_WrPtr %= UART_RX_BUF_BYTES;
	  } else {
    	LED_UART_OFF;
		}
  }
}

/******************* (C) COPYRIGHT 2011 STMicroelectronics *****END OF FILE****/
