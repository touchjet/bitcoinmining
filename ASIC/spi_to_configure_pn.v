
`timescale 1ns/100ps

module spi_to_configure_pn (reset_n,osc_clk,cs_n,mosi_data,pll_n,pll_m,pll_pdn,nonce_dify);
	input reset_n;
	input osc_clk;
	input cs_n;
	//input [1:0] mode;
	//input [8:0] bit_counter_si;
	input [359:0] mosi_data;
	output reg [7:0] pll_n;
	output reg [7:0] pll_m;
	output reg pll_pdn;
	output reg [31:0] nonce_dify;
	
parameter n_default = 8'b00010000;//for PLL NS[5:0]
parameter m_default = 8'b00000001;//for PLL MS[3:0]
parameter nonce_dify_default = 32'hffffffff;
	
parameter st0 = 2'b00;
parameter st1 = 2'b01;
parameter st2 = 2'b11;
parameter st3 = 2'b10;
//parameter st4 = 3'b110;
//parameter st5 = 3'b111;
parameter du = 3'd1;

reg cs_sync1;
reg cs_sync2;
wire cs_sync_out;
	
always@(posedge osc_clk)
begin
	cs_sync1 <= #du cs_n;
	cs_sync2 <= #du cs_sync1;
end

assign cs_sync_out = cs_sync2;

reg [1:0] current_st;
reg [1:0] next_st;

wire reset;
assign reset = (reset_n||cs_sync_out);

//state transition 
always@(posedge osc_clk or negedge reset)
begin
	if(!reset)
	begin
		current_st <= #du 2'b00;
	end
	else
	begin
		current_st <= #du next_st;
	end
end

//next state logic
always@(*)
begin
	case(current_st)
	st0:begin
			if((!reset_n)&&(cs_sync_out))
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st0;
			end
		end
	st1:begin
				next_st <= st2;
		end
	st2:begin
				next_st <= st3;
		end
	st3:begin
			    next_st <= st3;
		end
	default:begin
				next_st <= st3;
			end
	endcase
end
		

//output logic
always@(posedge osc_clk)
begin
	case(current_st)
	st0:begin
			pll_n <= #du n_default;
			pll_m <= #du m_default;
			pll_pdn <= #du 1'b0;
			nonce_dify <= #du nonce_dify_default;
		end
	st1:begin
			pll_n <= #du pll_n;
			pll_m <= #du pll_m;
			pll_pdn <= #du 1'b0;
			nonce_dify <= #du nonce_dify;
		end
	st2:begin
			pll_n <= #du mosi_data[351:344];//8 bits
			pll_m <= #du mosi_data[343:336];//8 bits
			pll_pdn <= #du 1'b0;
			nonce_dify <= #du mosi_data[335:304];//32 bits
		end
	st3:begin
			pll_n <= #du pll_n;
			pll_m <= #du pll_m;
			pll_pdn <= #du 1'b1;
			nonce_dify <= #du nonce_dify;
		end
	default:begin
				pll_n <= #du pll_n;
				pll_m <= #du pll_m;
				pll_pdn <= #du 1'b1;
				nonce_dify <= #du nonce_dify;
			end
	endcase
end

endmodule
