#include "fb1600.h"
#include "stm32f10x.h"
#include "led.h"
#include "testdata.h"
#include "power.h"
#include "parcel.h"

#define Low     0x00  /* Chip Select line low */
#define High    0x01  /* Chip Select line high */

#define SPIA_PORT   GPIOA
#define SPIB_PORT   GPIOB

#define CS_PORT     GPIOA
#define CS_EN_PORT  GPIOC
#define CS_PINS			GPIO_Pin_0 | GPIO_Pin_1 | GPIO_Pin_2 | GPIO_Pin_3
#define CS_EN_PINS	CS_EN_A_PIN | CS_EN_B_PIN | CS_EN_C_PIN | CS_EN_D_PIN | CS_EN_E_PIN
#define CS_EN_A_PIN GPIO_Pin_0
#define CS_EN_B_PIN GPIO_Pin_1
#define CS_EN_C_PIN GPIO_Pin_2
#define CS_EN_D_PIN GPIO_Pin_3
#define CS_EN_E_PIN GPIO_Pin_4

#define RST_PORT_1    GPIOD
#define RST_PORT_2    GPIOC

#define RST_PINS_1		GPIO_Pin_0 | GPIO_Pin_1 | GPIO_Pin_2 | GPIO_Pin_3 | GPIO_Pin_4 | GPIO_Pin_5 | GPIO_Pin_6 | GPIO_Pin_7 | GPIO_Pin_8 | GPIO_Pin_9 | GPIO_Pin_10 | GPIO_Pin_11 | GPIO_Pin_12 | GPIO_Pin_13 | GPIO_Pin_14 | GPIO_Pin_15
#define RST_PINS_2		GPIO_Pin_8 | GPIO_Pin_9 | GPIO_Pin_10 | GPIO_Pin_11

#define IRQ_PORT    GPIOE
#define IRQ_CPPE_PORT GPIOC

#define IRQ_CP_PIN  GPIO_Pin_6
#define IRQ_PE_PIN  GPIO_Pin_7

static volatile uint16_t FB1600_Temp = 0;

static volatile uint8_t COMMAND[45] = {
  0x00,
  0x00,0x01,                                            //PLL Setup
  0xff,0xff,0xff,0x7f,                                  //Difficulty
  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,    //Reserved
  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
};

volatile uint8_t ChipStats[NUMBER_OF_CHIPS];
volatile uint8_t CoreOK[NUMBER_OF_CORES];
volatile uint8_t RxBuffer[45];
volatile uint8_t CoreReady[NUMBER_OF_CORES];
volatile uint8_t OverHeated[NUMBER_OF_CHIPS];

volatile uint8_t localPll = 0;
volatile uint32_t localDiff = 0;

void Delay(int16_t ms);

void FB1600_Init()
{
  SPI_InitTypeDef  SPI_InitStructure;
  GPIO_InitTypeDef GPIO_InitStructure;
  
  
  /* Configure SPIA pins: SCK, MISO and MOSI */
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_5 | GPIO_Pin_6 | GPIO_Pin_7;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(SPIA_PORT, &GPIO_InitStructure);

  /* Configure SPIB pins: SCK, MISO and MOSI */

  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_13 | GPIO_Pin_14 | GPIO_Pin_15;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;
  GPIO_Init(SPIB_PORT, &GPIO_InitStructure);

  /* Configure Reset as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = RST_PINS_1;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(RST_PORT_1, &GPIO_InitStructure);
  GPIO_SetBits(RST_PORT_1, RST_PINS_1);
  
  GPIO_InitStructure.GPIO_Pin = RST_PINS_2;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(RST_PORT_2, &GPIO_InitStructure);
  GPIO_SetBits(RST_PORT_2, RST_PINS_2);
  
  /* Configure CS as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = CS_PINS;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(CS_PORT, &GPIO_InitStructure);
  GPIO_SetBits(CS_PORT, CS_PINS);

  /* Configure CS_EN as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = CS_EN_PINS;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(CS_EN_PORT, &GPIO_InitStructure);
  GPIO_SetBits(CS_EN_PORT, CS_EN_PINS);

  /* Configure Irq as Input with PullDown */
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_0 | GPIO_Pin_1 | GPIO_Pin_2 | GPIO_Pin_3 | GPIO_Pin_4 | GPIO_Pin_5 | GPIO_Pin_6 | GPIO_Pin_7 | GPIO_Pin_8 | GPIO_Pin_9;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPD;
  GPIO_Init(IRQ_PORT, &GPIO_InitStructure);

  /* Configure IRQ_CE/CP/PE as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = IRQ_CP_PIN | IRQ_PE_PIN;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(IRQ_CPPE_PORT, &GPIO_InitStructure);
  GPIO_SetBits(IRQ_CPPE_PORT, IRQ_PE_PIN);
  GPIO_ResetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);


  /* SPI configuration */ 
  SPI_InitStructure.SPI_Direction = SPI_Direction_2Lines_FullDuplex;
  SPI_InitStructure.SPI_Mode = SPI_Mode_Master;
  SPI_InitStructure.SPI_DataSize = SPI_DataSize_8b;
  SPI_InitStructure.SPI_CPOL = SPI_CPOL_High;
  SPI_InitStructure.SPI_CPHA = SPI_CPHA_2Edge;
  SPI_InitStructure.SPI_NSS = SPI_NSS_Soft;
  SPI_InitStructure.SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_16;
  SPI_InitStructure.SPI_FirstBit = SPI_FirstBit_MSB;
  SPI_InitStructure.SPI_CRCPolynomial = 7;
  SPI_Init(SPI1, &SPI_InitStructure);


  SPI_InitStructure.SPI_Direction = SPI_Direction_2Lines_FullDuplex;
  SPI_InitStructure.SPI_Mode = SPI_Mode_Master;
  SPI_InitStructure.SPI_DataSize = SPI_DataSize_8b;
  SPI_InitStructure.SPI_CPOL = SPI_CPOL_High;
  SPI_InitStructure.SPI_CPHA = SPI_CPHA_2Edge;
  SPI_InitStructure.SPI_NSS = SPI_NSS_Soft;
  SPI_InitStructure.SPI_BaudRatePrescaler = SPI_BaudRatePrescaler_16;
  SPI_InitStructure.SPI_FirstBit = SPI_FirstBit_MSB;
  SPI_InitStructure.SPI_CRCPolynomial = 7;
  SPI_Init(SPI2, &SPI_InitStructure);
  
  /* Enable SPIA  */
  SPI_Cmd(SPI1, ENABLE);
  /* Enable SPIB  */
  SPI_Cmd(SPI2, ENABLE);
#ifdef PRODUCTION
	FB1600_Reset(32,1);
#else
	FB1600_Reset(16,1);
#endif
}

__inline void FB1600_CS(uint8_t core, uint8_t level)
{	
	uint16_t cs_setter;
	if (level == Low) {
		GPIO_ResetBits(CS_PORT,CS_PINS);
		if (core < 16) {
			cs_setter = core;
			GPIO_SetBits(CS_PORT,cs_setter);
			GPIO_ResetBits(CS_EN_PORT, CS_EN_A_PIN);
		} else if (core < 32){
			cs_setter = core - 16;
			GPIO_SetBits(CS_PORT,cs_setter);
			GPIO_ResetBits(CS_EN_PORT, CS_EN_B_PIN);
		} else if (core < 48){
			cs_setter = core - 16;
			GPIO_SetBits(CS_PORT,cs_setter);
			GPIO_ResetBits(CS_EN_PORT, CS_EN_C_PIN);
		} else if (core < 64){
			cs_setter = core - 16;
			GPIO_SetBits(CS_PORT,cs_setter);
			GPIO_ResetBits(CS_EN_PORT, CS_EN_D_PIN);
		} else {
			cs_setter = core - 32;
			GPIO_SetBits(CS_PORT,cs_setter);
			GPIO_ResetBits(CS_EN_PORT, CS_EN_E_PIN);
		}
	} else {
		if (core < 16) {
			GPIO_SetBits(CS_EN_PORT, CS_EN_A_PIN);
		} else if (core < 32){
			GPIO_SetBits(CS_EN_PORT, CS_EN_B_PIN);
		} else if (core < 48){
			GPIO_SetBits(CS_EN_PORT, CS_EN_C_PIN);
		} else if (core < 64){
			GPIO_SetBits(CS_EN_PORT, CS_EN_D_PIN);
		}	else {
			GPIO_SetBits(CS_EN_PORT, CS_EN_E_PIN);
		}
		GPIO_SetBits(CS_PORT,CS_PINS);
	}	
}

void FB1600_RST(uint8_t core, uint8_t level)
{
	uint16_t rst_mask;
	if (level == Low) {
		if (core < 64) {
    	rst_mask = ((uint16_t)0x0001) << (core >> 2);
	  	GPIO_ResetBits(RST_PORT_1, rst_mask);
		} else {
    	rst_mask = ((uint16_t)0x0100) << ((core-64) >> 2);
	  	GPIO_ResetBits(RST_PORT_2, rst_mask);
		}
	} else {
		GPIO_SetBits(RST_PORT_1, RST_PINS_1);
		GPIO_SetBits(RST_PORT_2, RST_PINS_2);
	}
}

void FB1600_Reset(uint8_t pll,uint32_t difficulty)
{
	uint16_t core;
  uint16_t ptr = 1;
  uint32_t diff = 0xffffffff;
  uint16_t result_base;
  uint16_t config_ok = 0;
  uint16_t core_ok = 0;
	
	if ((localPll == pll)&&(localDiff == difficulty)) {
		return;
	}
	
	if ((pll > 36) || (pll < 16)) {
		pll = 25;
	}
  COMMAND[1] = pll << 1;
  COMMAND[2] = 0xa1;
	
	if (difficulty > 8192) {
		difficulty = 8192;
	}
	
  while (ptr < difficulty) {
    diff >>=1;
    ptr <<= 1;
  }
  COMMAND[3] = diff & 0x000000ff;
  COMMAND[4] = (diff & 0x0000ff00) >> 8; 
  COMMAND[5] = (diff & 0x00ff0000) >> 16; 
  COMMAND[6] = (diff & 0xff000000) >> 24; 
	
	for (core =0;core<NUMBER_OF_CORES;core++){
		OverHeated[core >> 2] = 0;
		core_ok = 0;
		config_ok = 0;
		result_base = 6 + ((core % 4) * 5);
    if ((core % 4) == 0) {
      FB1600_RST(core,Low);
      Delay(1);
      FB1600_Comm(core,(uint8_t *)COMMAND);
      Delay(10);
      FB1600_RST(core,High);
      Delay(1);
			if (difficulty == 1) {
        FB1600_RST(core,Low);
        Delay(1);
        FB1600_Comm(core,(uint8_t *)COMMAND);
        Delay(10);
        FB1600_RST(core,High);
        Delay(1);
			}
    }
    if (difficulty == 1) {
      for (ptr=0;ptr<9;ptr++) {
        FB1600_Comm(core,(uint8_t *)HASH_DATA[ptr]);
        Delay(1);
	      if (ptr == 0) {
	        if ((RxBuffer[2] == COMMAND[3]) &&  (RxBuffer[3] == COMMAND[4]) &&
              (RxBuffer[4] == COMMAND[5]) &&  (RxBuffer[5] == COMMAND[6])) {
            config_ok = 1;
	  			}
        } else {
	        if ( (RxBuffer[result_base+1] == HASH_DATA[ptr-1][45]) && (RxBuffer[result_base+2] == HASH_DATA[ptr-1][46]) && 
	           	 (RxBuffer[result_base+3] == HASH_DATA[ptr-1][47]) && (RxBuffer[result_base+4] == HASH_DATA[ptr-1][48]) ) {
    			  CoreOK[core] |= 0x01 << (ptr-1);
	  	      core_ok ++;
  	      }
	      }
      }
      if ((config_ok == 1) &&(core_ok == 8)) {
	      ChipStats[core >> 2] |= 1 << (core % 4);
      } else {
        ChipStats[core >> 2] &= ~(1 << (core % 4));
      }
	  }
  }		
}

void FB1600_CommDelay()
{
	volatile int d=0;
	while (d<100) d++;
}

void FB1600_ScanDelay()
{
	volatile uint16_t d=0;
	while (d<500) d++;
}

void FB1600_ScanIRQ(void)
{
	uint8_t i;
	uint16_t val;
	GPIO_ResetBits(IRQ_CPPE_PORT, IRQ_PE_PIN);
	GPIO_ResetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);
	FB1600_ScanDelay();
	GPIO_SetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);
	FB1600_ScanDelay();
	GPIO_ResetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);
	GPIO_SetBits(IRQ_CPPE_PORT, IRQ_PE_PIN);
	FB1600_ScanDelay();
	for (i=0;i<8;i++) {
		val = IRQ_PORT->IDR;
  	GPIO_SetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);
	  FB1600_ScanDelay();
		CoreReady[7-i] = (val&0x0001)==0 ? 0 : 1;
		CoreReady[15-i] = (val&0x0002)==0 ? 0 : 1;
		CoreReady[23-i] = (val&0x0004)==0 ? 0 : 1;
		CoreReady[31-i] = (val&0x0008)==0 ? 0 : 1;
		CoreReady[39-i] = (val&0x0010)==0 ? 0 : 1;
		CoreReady[47-i] = (val&0x0020)==0 ? 0 : 1;
		CoreReady[55-i] = (val&0x0040)==0 ? 0 : 1;
		CoreReady[63-i] = (val&0x0080)==0 ? 0 : 1;
		CoreReady[71-i] = (val&0x0100)==0 ? 0 : 1;
		CoreReady[79-i] = (val&0x0200)==0 ? 0 : 1;
	  GPIO_ResetBits(IRQ_CPPE_PORT, IRQ_CP_PIN);
  	FB1600_ScanDelay();
	}
}

void FB1600_Comm(uint8_t core, uint8_t *sendbuffer)
{
  uint16_t i;
	uint8_t temp;
	uint8_t chip;
  LED_CHIP_ON;
  FB1600_CS(core,Low);
  FB1600_CommDelay();
  for (i=0;i<45;i++)
  {
//	if (1) { 
	if (core < (NUMBER_OF_CORES / 2)) {
      while(SPI_I2S_GetFlagStatus(SPI1, SPI_I2S_FLAG_TXE) == RESET);
      SPI_I2S_SendData(SPI1, *sendbuffer);
      sendbuffer++;
      while(SPI_I2S_GetFlagStatus(SPI1, SPI_I2S_FLAG_RXNE) == RESET);
      RxBuffer[i] = SPI_I2S_ReceiveData(SPI1);
	} else {
      while(SPI_I2S_GetFlagStatus(SPI2, SPI_I2S_FLAG_TXE) == RESET);
      SPI_I2S_SendData(SPI2, *sendbuffer);
      sendbuffer++;
      while(SPI_I2S_GetFlagStatus(SPI2, SPI_I2S_FLAG_RXNE) == RESET);
      RxBuffer[i] = SPI_I2S_ReceiveData(SPI2);
	}
  }
  if ((core % 4) == 0) {
		chip = core >> 2;
		temp = (RxBuffer[43] << 7) + (RxBuffer[44] >> 1);
  	parcelDataUp.ChipTemp[chip] = temp;
		if (OverHeated[chip] == 0) {
			if (temp > 199) {  //Over Heat above 125C
				OverHeated[chip] = 1;
			}
		} else {
			if (temp < 180) {  //Recover below 115C
				OverHeated[chip] = 0;
			}		
		}
  }
  FB1600_CS(core,High);
  FB1600_CommDelay();
  LED_CHIP_OFF;
}

void FB1600_DryRun(void)
{
	int core;
  while (1) {
		FB1600_ScanIRQ();
		for (core =0;core<NUMBER_OF_CORES;core++) {
			if (CoreReady[core]) {
				FB1600_Comm(core,(uint8_t *)HASH_DATA[9]);
			}
		}
  }
}
