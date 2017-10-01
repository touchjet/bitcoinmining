`timescale 1ns/100ps

module data_gen(clk,reset_n,cs_n,nonce1_output,nonce2_output,nonce_mark,nonce_mark_counter,id_nonce_out,current_st);
	input clk;
	input reset_n;
	//input busy;
	input cs_n;
	input [39:0] nonce1_output;
	input [39:0] nonce2_output;
	//input receive;
	//input [7:0] pll_n;
	//input [7:0] pll_m;
	//input [31:0] nonce_dify;
	input nonce_mark;
	input [1:0] nonce_mark_counter;
	
	//output reg irq;
	//output [359:0] miso_data;
	output reg [39:0] id_nonce_out;
	output [3:0] current_st;

reg [3:0] current_st_tmp;	
reg [3:0] next_st;	

//reg [39:0] miso_data_out;
reg [39:0] miso_data_tmp;

reg nonce_mark_tm;
reg [1:0] nonce_mark_counter_tm;

parameter du = 3'd1;	

parameter st0 = 4'b0000;//0h
parameter st1 = 4'b0001;//1h
//parameter st2 = 4'b0011;//3h
parameter st3 = 4'b0011;//2h
parameter st4 = 4'b0110;//6h
parameter st5 = 4'b0111;//7h
parameter st6 = 4'b0101;//5h
parameter st7 = 4'b0100;//4h
//parameter st8 = 4'b1100;//ch
parameter st9 = 4'b1101;//dh
parameter st10= 4'b1001;//9h
parameter st11= 4'b1011;//bh

assign current_st = current_st_tmp;

//state transition 
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		current_st_tmp <= #du 4'b000;
	end
	else
	begin
		current_st_tmp <= #du next_st;
	end
end

//next state logic
always@(*)
begin
	case(current_st_tmp)
	st0:begin
			if(reset_n)
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st0;
			end
		end
	st1:begin
				next_st <= st3;
		end
	/*	
	st2:begin
			next_st <= st3;
		end
	*/
	st3:begin
			if(nonce_mark_counter_tm == 2'b01)
			begin
				next_st <= st4;
			end
			else if(nonce_mark_counter_tm == 2'b10)
			begin
				next_st <= st5;
			end
			else
			begin
				next_st <= st1;
			end
		end
	st4:begin
			next_st <= st9;
		end
	st5:begin
			next_st <= st9;
		end
	st9:begin
			if(cs_n == 1'b1)
			begin
				next_st <= st11;
			end
			else
			begin
				next_st <= st9;
			end
		end
	st11:begin
			next_st <=st10;
		end
	st10:begin
			next_st <= st6;
		end

	st6:begin
			if(cs_n == 1'b0)
			begin
				next_st <= st7;
			end
			else
			begin
				next_st <= st6;
			end
		end
	st7:begin
			if(cs_n == 1'b1)
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st7;
			end
		end
	/*
	st8:begin
			next_st <= st1;
		end
		*/
	default:begin
				next_st <= st1;
			end
	endcase
end
	
//output logic
always@(posedge clk)
begin
	case(current_st_tmp)
	st0:begin
			//irq <= #du 1'b0;
			id_nonce_out <= #du 40'b0;
			miso_data_tmp <= #du 40'b0;
			nonce_mark_tm <= #du 1'b0;
			nonce_mark_counter_tm <= #du 2'b0;
		end
	st1:begin
			id_nonce_out <= #du id_nonce_out;
			miso_data_tmp <= #du miso_data_tmp;
			nonce_mark_tm <= #du nonce_mark;
			nonce_mark_counter_tm <= #du nonce_mark_counter;
			//irq <= #du 1'b0;
		end
	/*
	st2:begin
			id_nonce_out <= #du id_nonce_out;
			nonce_mark_tm <= #du nonce_mark_tm;
			nonce_mark_counter_tm <= #du nonce_mark_counter_tm;
			//irq <= #du 1'b0;
		end*/
		
	st3:begin
			id_nonce_out <= #du id_nonce_out;
			nonce_mark_tm <= #du nonce_mark_tm;
			nonce_mark_counter_tm <= #du nonce_mark_counter_tm;
			//irq <= #du 1'b0;
		end
	st4:begin// nonce_mark_counter_tm = 1'b01;
			//irq <= 1'b0;
			id_nonce_out <= #du id_nonce_out;
			if(nonce_mark_tm==1'b1)
			begin
				miso_data_tmp <= #du nonce1_output;//,pll_m,pll_n,nonce_dify,272'b00};
			end
			else if(nonce_mark_tm==1'b0)
			begin
				miso_data_tmp <= #du nonce2_output;//,pll_m,pll_n,nonce_dify,272'b00};
			end
		end
	st5:begin// nonce_mark_counter_tm = 1'b10;
			//irq <= 1'b0;
			id_nonce_out <= #du id_nonce_out;
			if(nonce_mark_tm==1'b1)
			begin
				miso_data_tmp <= #du nonce2_output;//,pll_m,pll_n,nonce_dify,272'b00};
			end
			else if(nonce_mark_tm==1'b0)
			begin
				miso_data_tmp <= #du nonce1_output;//,pll_m,pll_n,nonce_dify,272'b00};
			end
		end
	st9:begin
			miso_data_tmp <= #du miso_data_tmp ;
			id_nonce_out <= #du id_nonce_out;
			//irq <= #du 1'b0;
		end
	st11:begin
			id_nonce_out <= #du miso_data_tmp ;
			//irq <= #du 1'b0;
		end
	st10:begin
			id_nonce_out <= #du id_nonce_out ;
			//irq <= #du 1'b1;
		end
	st6:begin
			//irq <= #du 1'b1;
			id_nonce_out <= #du id_nonce_out;
		end
	st7:begin
			//irq <= #du 1'b1;
			id_nonce_out <= #du id_nonce_out;
		end
	/*
	st8:begin
			//irq <= #du 1'b0;
			id_nonce_out <= #du id_nonce_out;
		end		*/
	default:begin
				//irq <= #du 1'b0;
				miso_data_tmp <= #du miso_data_tmp ;
				id_nonce_out <= #du id_nonce_out;
				nonce_mark_tm <= #du 1'b0;
				nonce_mark_counter_tm <= #du 2'b0;
			end
	endcase
end

//Generate miso_data

//assign miso_data = {id_nonce_out,pll_m,pll_n,nonce_dify,272'b00};

endmodule
