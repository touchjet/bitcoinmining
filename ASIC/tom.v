module tom(sclk,cry_clk,clk,reset_n,mosi,miso,cs_n,irq,pll_n_tom,pll_m_tom);
	input sclk;//spi clk
	input cry_clk;//crystall clk
	input clk;// pll output clk
	input reset_n;
	input mosi;
	output miso;
	input cs_n;
	output irq;
	
	output [7:0] pll_n_tom;
	output [7:0] pll_m_tom;


wire [359:0] mosi_data_tom;
wire [359:0] miso_date_tom;
wire [1:0] mode_tom;
wire receiving_tom;
wire [8:0] bit_counter_si_tom;
wire [8:0] bit_counter_so_tom;

spi_slave inst001(
.reset_n(reset_n),
.cs_n(cs_n),
.sclk(sclk),
.mosi(mosi),
.miso(miso),
.mosi_data(mosi_data_tom),
.miso_date(miso_date_tom),
.mode(mode_tom),
.receiving(receiving_tom),
.bit_counter_si(bit_counter_si_tom),
.bit_counter_so(bit_counter_so_tom));
/*	input reset_n;
	input cs_n;
	input sclk;
	input mosi;
	input [359:0] miso_date;
	output reg miso;
	output reg [1:0] mode; //2'b01 for configure ,2'b10 for hash data.
	output reg receiving;//1'b1 for is receiving data 1'b0 is idle
	output reg [359:0] mosi_data;
	output reg [8:0] bit_counter_si;
	output reg [8:0] bit_counter_so
*/

wire [31:0] nonce_dify_tom;

spi_to_configure_pn inst002(
.reset_n(reset_n),
.cry_clk(cry_clk),
.cs_n(cs_n),
.mode(mode_tom),
.bit_counter_si(bit_counter_si_tom),
.mosi_data(mosi_data_tom),
.pll_n(pll_n_tom),
.pll_m(pll_m_tom),
.nonce_dify(nonce_dify_tom));
/*	input reset_n;
	input cry_clk;
	input cs_n;
	input [1:0] mode;
	input [8:0] bit_counter_si;
	input [359:0] mosi_data;
	output reg [7:0] pll_n;
	output reg [7:0] pll_m;
	output reg [31:0] nonce_dify;
*/

wire start_tom;
wire start1_tom;
wire [3:0] hash_id1_tom;
wire [95:0] rx_m_data1_tom;
wire [255:0] rx_intial_h1_tom;
wire start2_tom;
wire [3:0] hash_id2_tom;
wire [95:0] rx_m_data2_tom;
wire [255:0] rx_intial_h2_tom;
wire [1:0] mark_tom;
wire [1:0] mark_counter_tom;
wire [2:0] current_st_tom;

spi_to_nonce_core_x4 inst003(
.clk(clk),
.reset_n(reset_n),
.cs_n(cs_n),
.mosi_data(mosi_data_tom),
.start(start_tom),
.start1(start1_tom),
.hash_id1(hash_id1_tom),
.rx_m_data1(rx_m_data1_tom),
.rx_intial_h1(rx_intial_h1_tom),
.start2(start2_tom),
.hash_id2(hash_id2_tom),
.rx_m_data2(rx_m_data2_tom),
.rx_intial_h2(rx_intial_h2_tom),
.mark(mark_tom),
.mark_counter(mark_counter_tom),
.current_st(current_st_tom));
/*	input clk;
	input reset_n;
	input cs_n;
	input [359:0] mosi_data;// from spi_slave
	input start;//from nonce_core_x4_ctrl
	output reg start1;
	output reg [3:0] hash_id1;
	output reg [95:0] rx_m_data1;
	output reg [255:0] rx_intial_h1;
	output reg start2;
	output reg [3:0] hash_id2;
	output reg [95:0] rx_m_data2;
	output reg [255:0] rx_intial_h2;
	output reg [1:0] mark;// to nonce_core_x4_ctrl
	output reg [1:0] mark_counter; // to nonce_core_x4_ctrl
	output reg [2:0] current_st; // to nonce_core_x4_ctrl
*/

wire [3:0] ct_catch_nonce_tom;
wire [3:0] hash_id_from_core_x4_tom;
wire [95:0] rx_m_data_tom;
wire [255:0] rx_intial_h_tom;

nonce_core_x4_ctrl inst004(
.clk(clk),
.reset_n(reset_n),
.start1(start1_tom),
.ct_catch_nonce(ct_catch_nonce_tom),
.ct_spi_to_nonce(current_st_tom),
.hash_id1(hash_id1_tom),
.rx_m_data1(rx_m_data1_tom),
.rx_intial_h1(rx_intial_h1_tom),
.start2(start2_tom),
.hash_id2(hash_id2_tom),
.rx_m_data2(rx_m_data2_tom),
.rx_intial_h2(rx_intial_h2_tom),
.mark(mark_tom),
.mark_counter(mark_counter_tom),
.start(start_tom),
.hash_id(hash_id_from_core_x4_tom),
.rx_m_data(rx_m_data_tom),
.rx_intial_h(rx_intial_h_tom));
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

wire busy_tom;
wire success_tom;
wire [31:0] nonce_tom;
wire [3:0] hash_id_tom;
wire [31:0] hash_dify_tom;
wire [30:0] nonce_add_tom;

nonce_core_x4 inst005(
.clk(clk),
.reset_n(reset_n),
.start(start_tom),
.rx_m_data(rx_m_data_tom),
.rx_initial_h(rx_intial_h_tom),
.hash_id_input(hash_id_from_core_x4_tom),
.hash_dify_input(nonce_dify_tom),
.busy(busy_tom),
.success(success_tom),
.nonce(nonce_tom),
.nonce_add(nonce_add_tom),
.hash_id(hash_id_tom),
.hash_dify(hash_dify_tom));
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
	output reg [30:0] nonce_add;//output is for simulation
*/

wire [3:0] irq_gen_st_tom;
wire [39:0] nonce1_output_tom;
wire [39:0] nonce2_output_tom;
wire [1:0] nonce_mark_tom;
wire [1:0] nonce_mark_counter_tom;
wire receive_tom;

catch_nonce inst006(
.clk(clk),
.reset_n(reset_n),
.busy(busy_tom),
.start(start_tom),
.hash_id(hash_id_tom),
.cs_n(cs_n),
.success(success_tom),
.nonce_input(nonce_tom),
.irq_gen_st(irq_gen_st_tom),
.nonce1_output(nonce1_output_tom),
.nonce2_output(nonce2_output_tom),
.nonce_mark(nonce_mark_tom),
.nonce_mark_counter(nonce_mark_counter_tom),
.receive(receive_tom),
.current_st(ct_catch_nonce_tom));
/*	input clk;
	input reset_n;
	input busy;
	input cs_n;
	input success;
	input start;
	input [3:0] hash_id;
	input [31:0] nonce_input;
	input [3:0] irq_gen_st;
	
	output reg [39:0] nonce1_output;
	output reg [39:0] nonce2_output;
	output reg [1:0] nonce_mark;
	output reg [1:0] nonce_mark_counter;
	output reg receive;
	output reg [3:0] current_st;
*/

irq_gen inst007(
.clk(clk),
.reset_n(reset_n),
.busy(busy_tom),
.cs_n(cs_n),
.nonce1_output(nonce1_output_tom),
.nonce2_output(nonce2_output_tom),
.receive(receive_tom),
.pll_n(pll_n_tom),
.pll_m(pll_m_tom),
.nonce_dify(hash_dify_tom),
.nonce_mark(nonce_mark_tom),
.nonce_mark_counter(nonce_mark_counter_tom),
.irq(irq),
.miso_data(miso_date_tom),
.current_st(irq_gen_st_tom));
/*	input clk;
	input reset_n;
	input busy;
	input cs_n;
	input [39:0] nonce1_output;
	input [39:0] nonce2_output;
	input receive;
	input [7:0] pll_n;
	input [7:0] pll_m;
	input [31:0] nonce_dify;
	input [1:0] nonce_mark;
	input [1:0] nonce_mark_counter;
	
	output reg irq;
	output reg [359:0] miso_data;
	output reg [3:0] current_st;
*/
endmodule

	
	
