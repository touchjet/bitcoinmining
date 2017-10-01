#include "raspi.h"
#include "stm32f10x.h"
#include "stm32f10x_dma.h"
#include "misc.h"
#include "string.h"
#include "uart.h"
#include "led.h"
#include "stm32f10x_it.h"

volatile uint8_t RASPI_Rx_Buff[2][RASPI_BUFF_SIZE][DB_DATA_SIZE];
volatile uint8_t RASPI_Tx_Buff[2][RASPI_BUFF_SIZE][DB_DATA_SIZE];

volatile uint8_t RASPI_PingPong;
volatile uint16_t RASPI_Rx_Ptr;
volatile uint16_t RASPI_Tx_Ptr;
volatile uint8_t RASPI_Buff_Switch;
volatile uint8_t raspi_led = 0;
volatile uint64_t RASPI_NextTick = 3600000;

void RASPI_Init(void)
{
  SPI_InitTypeDef  SPI_InitStructure;
  GPIO_InitTypeDef GPIO_InitStructure;
  DMA_InitTypeDef DMA_InitStructure;

  memset((void *)RASPI_Tx_Buff,0x00,2*RASPI_BUFF_SIZE*DB_DATA_SIZE);
  memset((void *)RASPI_Rx_Buff,0x88,2*RASPI_BUFF_SIZE*DB_DATA_SIZE);
  RASPI_PingPong = 0;
  RASPI_Rx_Ptr = 0;
  RASPI_Tx_Ptr = 0;
	RASPI_Buff_Switch = 0;

  /* Configure SPI pins: SCK, MISO and MOSI */
  // Configure GPIO for SPI slave: SCK, MOSI, SS as inputs
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_4 | GPIO_Pin_5 | GPIO_Pin_7; 
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;           // Configure SCK and MOSI pins as Input Pull-Up 
  GPIO_Init(GPIOA, &GPIO_InitStructure);

  GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_6;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
  
	/* Raspberry Pi Power */
 	GPIO_SetBits(GPIOA, GPIO_Pin_11);
	GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_11;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_OD;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
 	GPIO_SetBits(GPIOA, GPIO_Pin_11);
   
	/* Raspberry Pi Heart Beat */
	GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_0;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPD;
  GPIO_Init(GPIOA, &GPIO_InitStructure);
  
	RASPI_ON;
	
  /* SPI configuration */ 
  SPI_InitStructure.SPI_Direction = SPI_Direction_2Lines_FullDuplex;
  SPI_InitStructure.SPI_Mode = SPI_Mode_Slave;
  SPI_InitStructure.SPI_DataSize = SPI_DataSize_8b;
  SPI_InitStructure.SPI_CPOL = SPI_CPOL_High;
  SPI_InitStructure.SPI_CPHA = SPI_CPHA_2Edge;
  SPI_InitStructure.SPI_NSS = SPI_NSS_Hard;
  SPI_InitStructure.SPI_FirstBit = SPI_FirstBit_MSB;
  SPI_Init(SPI1, &SPI_InitStructure);
//  SPI_NSSInternalSoftwareConfig(SPI1,SPI_NSSInternalSoft_Set);


  DMA_DeInit(DMA1_Channel2);
  DMA_DeInit(DMA1_Channel3); 

  DMA_InitStructure.DMA_PeripheralBaseAddr = (uint32_t)&(SPI1->DR);
  DMA_InitStructure.DMA_MemoryBaseAddr = (uint32_t)&RASPI_Rx_Buff[0][0][0];
  DMA_InitStructure.DMA_BufferSize = RASPI_BUFF_SIZE*DB_DATA_SIZE;
  DMA_InitStructure.DMA_DIR = DMA_DIR_PeripheralSRC;
  DMA_InitStructure.DMA_PeripheralInc = DMA_PeripheralInc_Disable;
  DMA_InitStructure.DMA_MemoryInc = DMA_MemoryInc_Enable;
  DMA_InitStructure.DMA_PeripheralDataSize = DMA_PeripheralDataSize_Byte;
  DMA_InitStructure.DMA_MemoryDataSize = DMA_PeripheralDataSize_Byte;
  DMA_InitStructure.DMA_Mode = DMA_Mode_Normal;
  DMA_InitStructure.DMA_Priority = DMA_Priority_High;
  DMA_InitStructure.DMA_M2M = DMA_M2M_Disable;
  DMA_Init(DMA1_Channel2, &DMA_InitStructure);

  DMA_InitStructure.DMA_PeripheralBaseAddr = (uint32_t)&(SPI1->DR);
  DMA_InitStructure.DMA_MemoryBaseAddr = (uint32_t)&RASPI_Tx_Buff[0][0][0];
  DMA_InitStructure.DMA_BufferSize = RASPI_BUFF_SIZE*DB_DATA_SIZE;
  DMA_InitStructure.DMA_DIR = DMA_DIR_PeripheralDST;
  DMA_InitStructure.DMA_PeripheralInc = DMA_PeripheralInc_Disable;
  DMA_InitStructure.DMA_MemoryInc = DMA_MemoryInc_Enable;
  DMA_InitStructure.DMA_PeripheralDataSize = DMA_PeripheralDataSize_Byte;
  DMA_InitStructure.DMA_MemoryDataSize = DMA_PeripheralDataSize_Byte;
  DMA_InitStructure.DMA_Mode = DMA_Mode_Normal;
  DMA_InitStructure.DMA_Priority = DMA_Priority_High;
  DMA_InitStructure.DMA_M2M = DMA_M2M_Disable;
  DMA_Init(DMA1_Channel3, &DMA_InitStructure);

  SPI_I2S_DMACmd(SPI1, SPI_I2S_DMAReq_Rx | SPI_I2S_DMAReq_Tx, ENABLE);

// Start the devices
  DMA_ITConfig(DMA1_Channel2, DMA_IT_TC, ENABLE);                 // Enable DMA1 Channel Transfer Complete interrupt

  DMA_Cmd(DMA1_Channel2, ENABLE);                                 // Enable DMA channels}
  DMA_Cmd(DMA1_Channel3, ENABLE);                                 // Enable DMA channels}

  while (GPIO_ReadInputDataBit(GPIOA,GPIO_Pin_4)==Bit_RESET);
	
  SPI_Cmd(SPI1, ENABLE);                                          // Enable SPI device
  SPI_I2S_ClearITPendingBit(SPI1, SPI_I2S_IT_RXNE);
}

void RASPI_Reset_DMA(void)
{
	RASPI_NextTick = SysTickCounter + 60000;
	if (raspi_led) {
		raspi_led = 0;
		LED_SPI_OFF;
	}	else {
		raspi_led = 1;
		LED_SPI_ON;
	}		
  memset((void *)RASPI_Tx_Buff[RASPI_PingPong],0x00,RASPI_BUFF_SIZE*DB_DATA_SIZE);
  RASPI_PingPong = RASPI_PingPong == 0 ? 1 : 0;
	RASPI_Rx_Ptr = 0;
	RASPI_Tx_Ptr = 0;

  SPI_I2S_ClearITPendingBit(SPI1, SPI_I2S_IT_RXNE);

  //Disable DMA1 Channel2
  DMA1_Channel2->CCR &= ((uint32_t)0xFFFFFFFE);
  //Disable DMA1 Channel3
  DMA1_Channel3->CCR &= ((uint32_t)0xFFFFFFFE);

  //Write to DMA2 Channel7 CPAR 
  DMA1_Channel2->CMAR = (uint32_t)&RASPI_Rx_Buff[RASPI_PingPong];
  //Reset DMA1 Channel2 remaining bytes register 
  DMA1_Channel2->CNDTR = RASPI_BUFF_SIZE*DB_DATA_SIZE;
  //Enable DMA1 Channel2
  DMA1_Channel2->CCR |= ((uint32_t)0x00000001);

  //Write to DMA1 Channel3 CPAR 
  DMA1_Channel3->CMAR = (uint32_t)&RASPI_Tx_Buff[RASPI_PingPong];
  //Reset DMA1 Channel3 remaining bytes register 
  DMA1_Channel3->CNDTR = RASPI_BUFF_SIZE*DB_DATA_SIZE;
  //Enable DMA1 Channel3
  DMA1_Channel3->CCR |= ((uint32_t)0x00000001);
	
}

void RASPI_Work()
{
	
}

