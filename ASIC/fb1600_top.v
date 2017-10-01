
//FB1600 Logic Top Module
`timescale 1ns/100ps

module fb1600_top(reset_n,osc_clk,sclk,mosi,cs_n,gnd11a_pll,gnd11d_pll,vcc11a_pll,vcc11d_pll,
TA,TB,TC,TD,TE,TF,TG,TH,TI,TJ,TLSA,TLSB,TLSC,TLSD,TLSE,TVREF,
xvrt,gnd33a_adc,gnd33d_adc,vcc33a_adc,vcc33d_adc,
miso,irq,busy_1_4);
	input reset_n;
	//input clk;
	input osc_clk;
	input sclk;
	input mosi;
	input [3:0] cs_n;
	//pll power
	input gnd11a_pll;
	input gnd11d_pll;
	input vcc11a_pll;
	input vcc11d_pll;
	//adc trimming pad and power
	inout TA;
	inout TB;
	inout TC;
	inout TD;
	inout TE;
	inout TF;
	inout TG;
	inout TH;
	inout TI;
	inout TJ;
	inout TLSA;
	inout TLSB;
	inout TLSC;
	inout TLSD;
	inout TLSE;
	inout TVREF;
	input xvrt;
	input gnd33a_adc;
	input gnd33d_adc;
	input vcc33a_adc; 
	input vcc33d_adc;
	
	output miso;
	output [3:0] irq;
	output busy_1_4;
	
wire [3:0] busy;
wire cs_n_1_4_top;
wire clk;
//wire clk_pll_out;//for simulation
	
cs_n_hub cs_n_hub_inst(
.cs_n_1(cs_n[0]),
.cs_n_2(cs_n[1]),
.cs_n_3(cs_n[2]),
.cs_n_4(cs_n[3]),
.cs_n_1_4(cs_n_1_4_top));
/*	input cs_n_1;
	input cs_n_2;
	input cs_n_3;
	input cs_n_4;
	output cs_n_1_4;
*/

wire [359:0] mosi_data_top;
wire [7:0] pll_n_top;
wire [7:0] pll_m_top;
wire pll_pdn_top;
wire [31:0] nonce_dify_top;

wire [39:0] id_nonce_out_1_top;
wire [39:0] id_nonce_out_2_top;
wire [39:0] id_nonce_out_3_top;
wire [39:0] id_nonce_out_4_top;

wire [31:0] hash_dify_out_1_top;
wire [31:0] hash_dify_out_2_top;
wire [31:0] hash_dify_out_3_top;
wire [31:0] hash_dify_out_4_top;

wire [9:0] temp_data_top;

spi_slave spi_slave_inst(
//.reset_n(reset_n),
.cs_n(cs_n_1_4_top),
.sclk(sclk),
.mosi(mosi),
.miso_data({pll_n_top,pll_m_top,nonce_dify_top,
id_nonce_out_1_top,id_nonce_out_2_top,id_nonce_out_3_top,id_nonce_out_4_top,
hash_dify_out_1_top,hash_dify_out_2_top,hash_dify_out_3_top,hash_dify_out_4_top,14'b0,temp_data_top}),
.miso(miso),
.mosi_data(mosi_data_top)
);
/*	input reset_n;
	input cs_n;
	input sclk;
	input mosi;
	input [359:0] miso_data;
	output reg miso;
	output reg [359:0] mosi_data;
*/

spi_to_configure_pn spi_to_configure_pn_inst(
.reset_n(reset_n),
.osc_clk(osc_clk),
.cs_n(cs_n_1_4_top),
.mosi_data(mosi_data_top),
.pll_n(pll_n_top),
.pll_m(pll_m_top),
.pll_pdn(pll_pdn_top),
.nonce_dify(nonce_dify_top));
/*	input reset_n;
	input osc_clk;
	input cs_n;
	input [359:0] mosi_data;
	output reg [7:0] pll_n;
	output reg [7:0] pll_m;
	output reg pll_pdn;
	output reg [31:0] nonce_dify;
*/

clock_gen1 clock_gen1_inst(
.osc_clk(osc_clk),
.pll_n(pll_n_top),
.pll_m(pll_m_top),
.pll_pdn(pll_pdn_top),
.gnd11a_pll(gnd11a_pll),
.gnd11d_pll(gnd11d_pll),
.vcc11a_pll(vcc11a_pll),
.vcc11d_pll(vcc11d_pll),
.pll_clk_out(clk));
//.pll_clk_out(clk_pll_out));
/*	input osc_clk;
	input [7:0] pll_n;
	input [7:0] pll_m;
	input pll_pdn;
	output pll_clk_out;
*/

wire temp_clk_top;

clock_gen2 clock_gen2_inst(
.reset_n(reset_n),
.osc_clk(osc_clk),
.temp_clk(temp_clk_top));
/*	input reset_n;
	input osc_clk;
	output reg temp_clk;
*/



temp_sensor temp_sensor_inst(
.reset_n(reset_n),
.cs_n_1_4(cs_n_1_4_top),
.temp_clk(temp_clk_top),
.temp_data(temp_data_top),
.TA(TA),.TB(TB),.TC(TC),.TD(TD),.TE(TE),.TF(TF),.TG(TG),.TH(TH),.TI(TI),.TJ(TJ),// to top
.TLSA(TLSA),.TLSB(TLSB),.TLSC(TLSC),.TLSD(TLSD),.TLSE(TLSE),.TVREF(TVREF),// to top
.xvrt(xvrt),
.gnd33a_adc(gnd33a_adc),.gnd33d_adc(gnd33d_adc),.vcc33a_adc(vcc33a_adc),.vcc33d_adc(vcc33d_adc)
);
/*	input reset_n;
	input temp_clk;
	input cs_n_1_4;
	output reg [9:0] temp_data;
*/

hyper_threading_core hyper_threading_core_inst_1(
.reset_n(reset_n),
.clk(clk),
.cs_n(cs_n[0]),
.nonce_dify(nonce_dify_top),
.mosi_data(mosi_data_top),
.irq(irq[0]),
.busy(busy[0]),
.hash_dify_out(hash_dify_out_1_top),
.id_nonce_out(id_nonce_out_1_top));
/*	input clk;// pll output clk
	input reset_n;
	input cs_n;
	input [31:0] nonce_dify;
	input [359:0] mosi_data;
	output irq;
	output busy;
	output [31:0] hash_dify_out;
	output [39:0] id_nonce_out;
*/

hyper_threading_core hyper_threading_core_inst_2(
.reset_n(reset_n),
.clk(clk),
.cs_n(cs_n[1]),
.nonce_dify(nonce_dify_top),
.mosi_data(mosi_data_top),
.irq(irq[1]),
.busy(busy[1]),
.hash_dify_out(hash_dify_out_2_top),
.id_nonce_out(id_nonce_out_2_top));
/*	input clk;// pll output clk
	input reset_n;
	input cs_n;
	input [31:0] nonce_dify;
	input [359:0] mosi_data;
	output irq;
	output busy;
	output [31:0] hash_dify_out;
	output [39:0] id_nonce_out;
*/

hyper_threading_core hyper_threading_core_inst_3(
.reset_n(reset_n),
.clk(clk),
.cs_n(cs_n[2]),
.nonce_dify(nonce_dify_top),
.mosi_data(mosi_data_top),
.irq(irq[2]),
.busy(busy[2]),
.hash_dify_out(hash_dify_out_3_top),
.id_nonce_out(id_nonce_out_3_top));
/*	input clk;// pll output clk
	input reset_n;
	input cs_n;
	input [31:0] nonce_dify;
	input [359:0] mosi_data;
	output irq;
	output busy;
	output [31:0] hash_dify_out;
	output [39:0] id_nonce_out;
*/

hyper_threading_core hyper_threading_core_inst_4(
.reset_n(reset_n),
.clk(clk),
.cs_n(cs_n[3]),
.nonce_dify(nonce_dify_top),
.mosi_data(mosi_data_top),
.irq(irq[3]),
.busy(busy[3]),
.hash_dify_out(hash_dify_out_4_top),
.id_nonce_out(id_nonce_out_4_top));
/*	input clk;// pll output clk
	input reset_n;
	input cs_n;
	input [31:0] nonce_dify;
	input [359:0] mosi_data;
	output irq;
	output busy;
	output [31:0] hash_dify_out;
	output [39:0] id_nonce_out;
*/

//assign busy_1_4 = (busy[0]&busy[1]&busy[2]&busy[3]);

busy_out busy_out_inst(
.busy_input(busy),
.busy_output(busy_1_4));

endmodule

