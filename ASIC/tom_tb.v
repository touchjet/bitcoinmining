
`timescale 1ns/100ps

module tom_tb();

reg sclk;
reg cry_clk;
reg clk;
reg reset_n;
reg mosi_tb;
wire miso_tb;
reg cs_n_tb;
wire irq_tb;

wire [7:0] pll_n_tom_tb;
wire [7:0] pll_m_tom_tb;

defparam tom_inst1.inst005.nonce_start = 31'h000007d0;
tom tom_inst1(
.sclk(sclk_tb),
.cry_clk(cry_clk),
.clk(clk),
.reset_n(reset_n),
.mosi(mosi_tb),
.miso(miso_tb),
.cs_n(cs_n_tb),
.irq(irq_tb),
.pll_n_tom(pll_n_tom_tb),
.pll_m_tom(pll_m_tom_tb)
);
/*	input sclk;//spi clk
	input cry_clk;//crystall clk
	input clk;// pll output clk
	input reset_n;
	input mosi;
	output miso;
	input cs_n;
	output irq;
	
	output [7:0] pll_n_tom;
	output [7:0] pll_m_tom;
*/
//*********************************************************
//  To generate reset_n
//*********************************************************
initial
begin
	reset_n = 1'b0;
	#17000 reset_n = 1'b1;
end
//*********************************************************


//*********************************************************
//  To generate sclk_tb 1MHz
//*********************************************************
initial
begin
	sclk = 1'b0;
end

always
begin
	#500 sclk = ~sclk;
end

reg control;

initial
begin
	control = 1'b1;
	#35500 control = 1'b0;
	#32000 control =1'b0;
	#328000 control = 1'b1;
	//#1000 control = 1'b0; 
	//#32000 control =1'b0;
	//#328000 control =1'b1;
	
end

assign sclk_tb = (sclk||control);
//*********************************************************


//*********************************************************
//  To generate cs_n_tb
//*********************************************************
initial
begin
	cs_n_tb = 1'b1;
	#5000 cs_n_tb = 1'b0;
	#10000 cs_n_tb = 1'b1;
	#20500 cs_n_tb =1'b0;
	#32000 cs_n_tb = 1'b0;
	#328500 cs_n_tb = 1'b1;
	//#500 cs_n_tb = 1'b0;
	//#32000 cs_n_tb = 1'b0;
	//#328500 cs_n_tb = 1'b1;
end
//*********************************************************


//*********************************************************
//  To generate mosi_tb
//*********************************************************
reg [359:0] mosi_data_reg_tb;
initial
begin
	//mosi_data_reg_tb = {8'h19,256'hd5b0cdcf58000919c4aed128dc474f5e6ebb2215480ba01ee9d54a615329c26f,96'h2d7a0dfdc808f9516889001a};
	//nonce = 0x7943b3a6 =0x 0111 943b3a6;
	//nonce_start = 0h3943b3a6;
    mosi_data_reg_tb = {8'h1c,256'h75d844270b260f4b274d5a4db7576d4321194f9168bb4063f3840858784dd5ae,96'hecbbc99396cf29529c673119};
end

reg [8:0] counter_tb;
reg [4:0] teset_cycle;

initial
begin
	counter_tb = 9'b0;
end

initial
begin
	mosi_tb = 1'b1;
end

initial
begin
	teset_cycle = 5'b0;
end

always@(negedge sclk_tb)
begin
	
	mosi_tb <= #2 mosi_data_reg_tb[(9'd359-counter_tb)];
	counter_tb <= #2(counter_tb + 9'b1);
	if(counter_tb==9'd359)
	begin
		counter_tb <= #2 9'b0;
		teset_cycle <= #2 teset_cycle + 5'd1; 
	end
end
//*********************************************************


//*********************************************************
//To get miso dat 
//*********************************************************
reg [359:0] miso_data_reg_tb;
reg [8:0] counter_tb1;

initial
begin
	counter_tb1 <=9'b0;
	miso_data_reg_tb <= 360'b0;
end

always@(posedge sclk_tb)
begin
	miso_data_reg_tb[(9'd359-counter_tb1)] <= #2 miso_tb;
	counter_tb1 <= #2(counter_tb1 + 9'b1);
	if(counter_tb1==9'd359)
	begin
		counter_tb1 <= #2 9'b0;
		//teset_cycle <= #2 teset_cycle + 5'd1; 
	end
end
//*********************************************************



//*********************************************************
//To check the spi_slave receive data 
//*********************************************************
always@(posedge cs_n_tb)
begin
	begin
		if(tom_inst1.inst001.mosi_data == mosi_data_reg_tb)
		begin
			$display($time,"\n ***Get the right mosi data;\n",);
			$display($time,"\n ***mosi_data = %h; \n",tom_inst1.inst001.mosi_data);
			$display(" ***mode = %h; \n",tom_inst1.inst001.mode);
			//$stop;
		end
		else
		begin
			$display($time,"\n ***Don't get the right mosi data;\n",);
			$display($time,"\n ***mosi_data = %h; \n",tom_inst1.inst001.mosi_data);
		end
	end
end
//*********************************************************


//*********************************************************
//To generate  cry_clk  25MHz
//*********************************************************
initial
begin
	cry_clk = 1'b0;
end

always
begin
	#20 cry_clk = ~cry_clk;
end
//*********************************************************

//*********************************************************
//To generate  clk  250MHz
//*********************************************************
initial
begin
	clk = 1'b0;
end

always
begin
	#2 clk = ~clk;
end
//*********************************************************


//*********************************************************
//To spi_to_nonce_core_x4 inst003
//*********************************************************
always@(tom_inst1.inst003.start or tom_inst1.inst003.current_st or tom_inst1.inst003.hash_id2 or tom_inst1.inst003.rx_m_data2 or tom_inst1.inst003.rx_intial_h2 or tom_inst1.inst003.hash_id1 or tom_inst1.inst003.rx_m_data1 or tom_inst1.inst003.rx_intial_h1 or tom_inst1.inst003.mark or tom_inst1.inst003.mark_counter)
begin
	$display($time,"\n start=%b; spi_to_nonce_core_x4 inst003 current_st=%b ;\n",tom_inst1.inst003.start,tom_inst1.inst003.current_st);
	$display(" hash_id1=%h;\n rx_m_data1=%h;\n rx_intial_h1=%h; \n",tom_inst1.inst003.hash_id1,tom_inst1.inst003.rx_m_data1,tom_inst1.inst003.rx_intial_h1);
	$display(" hash_id2=%h;\n rx_m_data2=%h;\n rx_intial_h2=%h; \n",tom_inst1.inst003.hash_id2,tom_inst1.inst003.rx_m_data2,tom_inst1.inst003.rx_intial_h2);
	$display(" mark=%b; mark_counter=%h; \n",tom_inst1.inst003.mark,tom_inst1.inst003.mark_counter);
end
//*********************************************************


//*********************************************************
//For nonce_core_x4_ctrl inst004
//*********************************************************
always@(tom_inst1.inst004.ct_catch_nonce or tom_inst1.inst004.ct_spi_to_nonce or tom_inst1.inst004.start or tom_inst1.inst004.hash_id or tom_inst1.inst004.rx_m_data or tom_inst1.inst004.rx_intial_h)
begin
	$display($time," \n ct_catch_nonce =%b; ct_spi_to_nonce=%b; start=%b; hash_id=%h; rx_m_data =%h; rx_intial_h=%h \n ",tom_inst1.inst004.ct_catch_nonce,tom_inst1.inst004.ct_spi_to_nonce,tom_inst1.inst004.start,tom_inst1.inst004.hash_id,tom_inst1.inst004.rx_m_data,tom_inst1.inst004.rx_intial_h);
end
//*********************************************************


//*********************************************************
//For nonce_core_x4 inst005
//*********************************************************
always@(tom_inst1.inst005.nonce_start, tom_inst1.inst005.start, tom_inst1.inst005.rx_m_data, tom_inst1.inst005.rx_initial_h, tom_inst1.inst005.hash_id_input, tom_inst1.inst005.hash_dify_input, tom_inst1.inst005.busy, tom_inst1.inst005.success, tom_inst1.inst005.nonce, tom_inst1.inst005.nonce_add, tom_inst1.inst005.hash_id, tom_inst1.inst005.hash_dify)
begin
	$display($time,"\n nonce_start=%h; \n start=%b; rx_m_data=%h; rx_initial_h=%h; hash_id_input=%h; hash_dify_input=%h; \n busy=%b; success=%b; nonce=%h;\n",tom_inst1.inst005.nonce_start, tom_inst1.inst005.start, tom_inst1.inst005.rx_m_data, tom_inst1.inst005.rx_initial_h, tom_inst1.inst005.hash_id_input, tom_inst1.inst005.hash_dify_input, tom_inst1.inst005.busy, tom_inst1.inst005.success, tom_inst1.inst005.nonce);
end
//*********************************************************

//*********************************************************
//For irq_gen inst007
//*********************************************************

always@(posedge tom_inst1.inst007.irq)
begin
	#1000 control = 1'b0;
	#32000 control =1'b0;
	#328000 control = 1'b1;
end

always@(posedge tom_inst1.inst007.irq)
begin
	#1000 cs_n_tb =1'b0;
	#32000 cs_n_tb = 1'b0;
	#328500 cs_n_tb = 1'b1;
end

//*********************************************************

	

/*
initial
begin
	$monitor($time,"\n reset_n = %b ; start = %b ; busy = %b ; success = %b ;\n",reset_n_tb,start_tb,busy_tb,success_tb);
end
*/
initial
begin
	#950000 $stop;
end


endmodule
