
//ht_core Hyper-Threading nonce core
`timescale 1ns/100ps

module hyper_threading_core(reset_n,clk,cs_n,nonce_dify,mosi_data,irq,busy,hash_dify_out,id_nonce_out);
	input clk;// pll output clk
	input reset_n;
	input cs_n;
	input [31:0] nonce_dify;
	input [359:0] mosi_data;
	output irq;
	output busy;
	output [31:0] hash_dify_out;
	output [39:0] id_nonce_out;


wire cs_sync;
cs_sync cs_sync_inst(
.clk(clk),
.cs_n_in(cs_n),
.cs_sync_out(cs_sync)
);
/*	input clk;
	input cs_n_in;
	output reg cs_sync_out;
*/

wire start_ht;
wire [3:0] hash_id1_ht;
wire [95:0] rx_m_data1_ht;
wire [255:0] rx_intial_h1_ht;
wire [3:0] hash_id2_ht;
wire [95:0] rx_m_data2_ht;
wire [255:0] rx_intial_h2_ht;
wire mark_ht;
wire [1:0] mark_counter_ht;
wire [2:0] current_st_ht;

spi_to_nonce_core_x4 spi_to_nonce_core_x4_inst(
.clk(clk),
.reset_n(reset_n),
.cs_n(cs_sync),
.mosi_data(mosi_data),
.start(start_ht),
.hash_id1(hash_id1_ht),
.rx_m_data1(rx_m_data1_ht),
.rx_intial_h1(rx_intial_h1_ht),
.hash_id2(hash_id2_ht),
.rx_m_data2(rx_m_data2_ht),
.rx_intial_h2(rx_intial_h2_ht),
.mark(mark_ht),
.mark_counter(mark_counter_ht),
.current_st(current_st_ht));
/*	input clk;
	input reset_n;
	input cs_n;
	input [359:0] mosi_data;// from spi_slave
	input start;//from nonce_core_x4_ctrl
	output reg [3:0] hash_id1;
	output reg [95:0] rx_m_data1;
	output reg [255:0] rx_intial_h1;
	output reg [3:0] hash_id2;
	output reg [95:0] rx_m_data2;
	output reg [255:0] rx_intial_h2;
	output reg [1:0] mark;// to nonce_core_x4_ctrl
	output reg [1:0] mark_counter; // to nonce_core_x4_ctrl
	output reg [2:0] current_st; // to nonce_core_x4_ctrl
*/

wire [3:0] ct_catch_nonce_ht;
wire [3:0] hash_id_from_core_x4_ht;
wire [95:0] rx_m_data_ht;
wire [255:0] rx_intial_h_ht;

nonce_core_x4_ctrl nonce_core_x4_ctrl_inst(
.clk(clk),
.reset_n(reset_n),
.ct_catch_nonce(ct_catch_nonce_ht),
.ct_spi_to_nonce(current_st_ht),
.hash_id1(hash_id1_ht),
.rx_m_data1(rx_m_data1_ht),
.rx_intial_h1(rx_intial_h1_ht),
.hash_id2(hash_id2_ht),
.rx_m_data2(rx_m_data2_ht),
.rx_intial_h2(rx_intial_h2_ht),
.mark(mark_ht),
.mark_counter(mark_counter_ht),
.start(start_ht),
.hash_id(hash_id_from_core_x4_ht),
.rx_m_data(rx_m_data_ht),
.rx_intial_h(rx_intial_h_ht));
/*	input clk;
	input reset_n;
	input start1;
	input [3:0] ct_catch_nonce;
	input [2:0] ct_spi_to_nonce;
	input [3:0] hash_id1;
	input [95:0] rx_m_data1;
	input [255:0] rx_intial_h1;
	input start2;
	input [3:0] hash_id2;
	input [95:0] rx_m_data2;
	input [255:0] rx_intial_h2;
	input [1:0] mark;
	input [1:0] mark_counter;
	
	output reg start;
	output reg [3:0] hash_id;
	output reg [95:0] rx_m_data;
	output reg [255:0] rx_intial_h;
*/	


wire success_ht;
wire [31:0] nonce_ht;
wire [3:0] hash_id_ht;

nonce_core_x8 nonce_core_x8_inst(
.clk(clk),
.reset_n(reset_n),
.start(start_ht),
.rx_m_data(rx_m_data_ht),
.rx_initial_h(rx_intial_h_ht),
.hash_id_input(hash_id_from_core_x4_ht),
.hash_dify_input(nonce_dify),
.busy(busy),
.success(success_ht),
.nonce(nonce_ht),
.hash_id(hash_id_ht),
.hash_dify(hash_dify_out));
/*	input clk;
	input reset_n;
	input start;
	input [95:0] rx_m_data;//block data need to be Hashed
	input [255:0] rx_initial_h;//initial hash data from the Software
	input [3:0] hash_id_input;
	input [31:0] hash_dify_input;
	output reg busy;// 1'b1: computing 1'b0: no computing
	output reg success;// 1'b1: get a nonce 1'b0:there no nonce
	output reg [31:0] nonce;
	output reg [3:0] hash_id;
	output reg [31:0] hash_dify;
*/

wire [3:0] data_gen_st_ht;
wire [39:0] nonce1_output_ht;
wire [39:0] nonce2_output_ht;
wire nonce_mark_ht;
wire [1:0] nonce_mark_counter_ht;


catch_nonce catch_nonce_inst(
.clk(clk),
.reset_n(reset_n),
.busy(busy),
.start(start_ht),
.hash_id(hash_id_ht),
//.cs_n(cs_sync),
.success(success_ht),
.nonce_input(nonce_ht),
.data_gen_st(data_gen_st_ht),
.nonce1_output(nonce1_output_ht),
.nonce2_output(nonce2_output_ht),
.nonce_mark(nonce_mark_ht),
.nonce_mark_counter(nonce_mark_counter_ht),
.current_st(ct_catch_nonce_ht));
/*	input clk;
	input reset_n;
	input busy;
//	input cs_n;
	input success;
	input start;
	input [3:0] hash_id;
	input [31:0] nonce_input;
//	input [3:0] data_gen_st;
	output reg [39:0] nonce1_output;
	output reg [39:0] nonce2_output;
	output reg [1:0] nonce_mark;
	output reg [1:0] nonce_mark_counter;
	output reg [3:0] current_st;
*/

data_gen data_gen_inst(
.clk(clk),
.reset_n(reset_n),
.cs_n(cs_sync),
.nonce1_output(nonce1_output_ht),
.nonce2_output(nonce2_output_ht),
.nonce_mark(nonce_mark_ht),
.nonce_mark_counter(nonce_mark_counter_ht),
//.irq(irq),
.id_nonce_out(id_nonce_out),
.current_st(data_gen_st_ht));
/*	input clk;
	input reset_n;
	//input busy;
	input cs_n;
	input [39:0] nonce1_output;
	input [39:0] nonce2_output;
	//input receive;
	//input [7:0] pll_n;
	//input [7:0] pll_m;
	//input [31:0] nonce_dify;
	input [1:0] nonce_mark;
	input [1:0] nonce_mark_counter;
	
	output reg irq;
	//output [359:0] miso_data;
	output reg [39:0] id_nonce_out;
	output [3:0] current_st;
*/

irq_gen irq_gen_inst(
.clk(clk),
.reset_n(reset_n),
.mark_counter(mark_counter_ht),
.irq(irq));
/*	input clk;
	input reset_n;
	input [1:0] mark_counter;
	output reg irq;
*/

endmodule
