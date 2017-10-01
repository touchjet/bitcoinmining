#include "stm32f10x.h"
#include "lcd.h"

#define LCD_PIN_D0 GPIO_Pin_0
#define LCD_PIN_D1 GPIO_Pin_1
#define LCD_PIN_D2 GPIO_Pin_2
#define LCD_PIN_D3 GPIO_Pin_3
#define LCD_PIN_RW GPIO_Pin_8
#define LCD_PIN_E  GPIO_Pin_9
#define LCD_PIN_RS GPIO_Pin_10
#define LCD_PORT GPIOC

#define SET_RS()	    GPIO_SetBits(LCD_PORT,LCD_PIN_RS)
#define CLR_RS()	    GPIO_ResetBits(LCD_PORT,LCD_PIN_RS)
#define SET_EN()	    GPIO_SetBits(LCD_PORT,LCD_PIN_E)
#define CLR_EN()	    GPIO_ResetBits(LCD_PORT,LCD_PIN_E)

void LCD_Delay(uint32_t us)
{
	volatile uint16_t i;
	for (i=0;i<us;i++){
			;
	}	
}

void LCD_Write_HB(unsigned char half_byte)
{  
    u16 temp_io = 0x0000;
    temp_io = GPIO_ReadOutputData(LCD_PORT);   
    temp_io &= 0xfff0;                      
    temp_io |= (u16)(half_byte&0x0f);       
    GPIO_Write(LCD_PORT,temp_io);              
    SET_EN();
    LCD_Delay(2000);
    CLR_EN(); 
    LCD_Delay(2000);
}

void LCD_write_cmd(unsigned char cmd)
{
    CLR_RS();
    LCD_Write_HB(cmd >> 4);
    LCD_Write_HB(cmd);
    LCD_Delay (10000);
}

void LCD_write_data(unsigned char w_data)
{
    SET_RS();
    LCD_Write_HB(w_data >> 4);
    LCD_Write_HB(w_data);
    LCD_Delay (10000);
}


void LCD_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStructure;
  
  /* Configure LED as Output push-pull */
  GPIO_InitStructure.GPIO_Pin = LCD_PIN_RW | LCD_PIN_E | LCD_PIN_RS | LCD_PIN_D0 | LCD_PIN_D1 | LCD_PIN_D2 | LCD_PIN_D3;
  GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
  GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
  GPIO_Init(LCD_PORT, &GPIO_InitStructure);
  GPIO_ResetBits(LCD_PORT, LCD_PIN_RW);
	
  LCD_Delay (15000);
  CLR_RS();
  LCD_Write_HB(0x3);                 
  LCD_Delay (15000);
  LCD_Write_HB(0x3);
  LCD_Delay (15000);
  LCD_Write_HB(0x3);
  LCD_Write_HB(0x2);
  LCD_write_cmd(0x28); 	 
  LCD_Delay (20000);  
  LCD_write_cmd(0x08);       
  LCD_Delay (20000); 
  LCD_write_cmd(0x01);         
  LCD_Delay (20000); 
  LCD_write_cmd(0x06);        
  LCD_Delay (20000);
  LCD_write_cmd(0x0C);    
  LCD_Delay (20000);
}

