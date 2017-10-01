
`timescale 1ns/100ps

module spi_to_nonce_core_x4(clk,reset_n,cs_n,mosi_data,start,hash_id1,rx_m_data1,rx_intial_h1,hash_id2,rx_m_data2,rx_intial_h2,mark,mark_counter,current_st);
	input clk;
	input reset_n;
	input cs_n;
	input [359:0] mosi_data;
	input start;
	output reg [3:0] hash_id1;
	output reg [95:0] rx_m_data1;
	output reg [255:0] rx_intial_h1;
	output reg [3:0] hash_id2;
	output reg [95:0] rx_m_data2;
	output reg [255:0] rx_intial_h2;
	output reg mark;
	output reg [1:0] mark_counter;
	output [2:0] current_st;

reg [2:0] current_st_tmp;	
reg [2:0] next_st;

parameter du = 3'd2;	

parameter st0 = 3'b000;
parameter st1 = 3'b001;
parameter st2 = 3'b011;
parameter st3 = 3'b010;
parameter st4 = 3'b110;
parameter st5 = 3'b111;
parameter st6 = 3'b101;
parameter st7 = 3'b100;


assign current_st = current_st_tmp; //output 

wire cs_n_sync;
assign cs_n_sync = cs_n;

//state transition 
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		current_st_tmp <= #du 3'b000;
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
			if((reset_n)&&(cs_n_sync))
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st0;
			end
		end
	st1:begin
			if((reset_n)&&(!cs_n_sync))
			begin
				next_st <= st2;
			end
			else
			begin
				next_st <= st1;
			end
		end
	st2:begin
			if((reset_n)&&(cs_n_sync))
			begin
				next_st <= st6;
			end
			else
			begin
				next_st <= st2;
			end
		end
	/*
	st3:begin
			if((cs_n_sync))//&&(mosi_data[359:356]==4'b01))
			begin
				next_st <= st6;
			end
			else
			begin
				next_st <= st1;
			end
		end
		*/
	st6:begin
			next_st <= st4;
		end
	//st7:begin
	//		next_st <= st4;
	//	end
	st4:begin
			next_st <= st5;
		end
	st5:begin
			next_st <= st1;
		end
	
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
			hash_id1 <= #du 4'b0;
			rx_m_data1 <= #du 96'b0;
			rx_intial_h1 <= #du 256'b0;
			hash_id2 <= #du 4'b0;
			rx_m_data2 <= #du 96'b0;
			rx_intial_h2 <= #du 256'b0;			
		end
	st1:begin
			hash_id1 <= #du hash_id1;
			rx_m_data1 <= #du rx_m_data1;
			rx_intial_h1 <= #du rx_intial_h1;
			hash_id2 <= #du hash_id2;
			rx_m_data2 <= #du rx_m_data2;
			rx_intial_h2 <= #du rx_intial_h2;
		end
	st2:begin
			hash_id1 <= #du hash_id1;
			rx_m_data1 <= #du rx_m_data1;
			rx_intial_h1 <= #du rx_intial_h1;
			hash_id2 <= #du hash_id2;
			rx_m_data2 <= #du rx_m_data2;
			rx_intial_h2 <= #du rx_intial_h2;
		end
	/*	
	st3:begin
			hash_id1 <= #du hash_id1;
			rx_m_data1 <= #du rx_m_data1;
			rx_intial_h1 <= #du rx_intial_h1;
			hash_id2 <= #du hash_id2;
			rx_m_data2 <= #du rx_m_data2;
			rx_intial_h2 <= #du rx_intial_h2;
		end
	*/
	st6:begin
			hash_id1 <= #du hash_id1;
			rx_m_data1 <= #du rx_m_data1;
			rx_intial_h1 <= #du rx_intial_h1;
			hash_id2 <= #du hash_id2;
			rx_m_data2 <= #du rx_m_data2;
			rx_intial_h2 <= #du rx_intial_h2;
		end
	/*
	st7:begin
			hash_id1 <= #du hash_id1;
			rx_m_data1 <= #du rx_m_data1;
			rx_intial_h1 <= #du rx_intial_h1;
			hash_id2 <= #du hash_id2;
			rx_m_data2 <= #du rx_m_data2;
			rx_intial_h2 <= #du rx_intial_h2;
		end*/
	st4:begin
			if(mark==1'b0)
			begin
				hash_id1 <= #du mosi_data[355:352];
				rx_m_data1 <= #du mosi_data[95:0];
				rx_intial_h1 <= #du mosi_data[351:96];
				hash_id2 <= #du hash_id2;
				rx_m_data2 <= #du rx_m_data2;
				rx_intial_h2 <= #du rx_intial_h2;
			end
			else if(mark==1'b1)
			begin
				hash_id2 <= #du mosi_data[355:352];
				rx_m_data2 <= #du mosi_data[95:0];
				rx_intial_h2 <= #du mosi_data[351:96];
				hash_id1 <= #du hash_id1;
				rx_m_data1 <= #du rx_m_data1;
				rx_intial_h1 <= #du rx_intial_h1;
			end
		end
	st5:begin
				hash_id1 <= #du hash_id1;
				rx_m_data1 <= #du rx_m_data1;
				rx_intial_h1 <= #du rx_intial_h1;
				hash_id2 <= #du hash_id2;
				rx_m_data2 <= #du rx_m_data2;
				rx_intial_h2 <= #du rx_intial_h2;
		end
	default:begin
				hash_id1 <= #du hash_id1;
				rx_m_data1 <= #du rx_m_data1;
				rx_intial_h1 <= #du rx_intial_h1;
				hash_id2 <= #du hash_id2;
				rx_m_data2 <= #du rx_m_data2;
				rx_intial_h2 <= #du rx_intial_h2;
			end
	endcase
end

always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		mark <= #du 1'b0;
	end
	else
	begin
		if(current_st_tmp==st4)
		begin
			mark <= #du ~mark;
		end
		else
		begin
			mark <= #du mark;
		end
	end
end

always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		mark_counter <= #du 2'b0;
	end
	else
	if((current_st_tmp==st4)&&(!start))
	begin
		mark_counter <= #du mark_counter+2'b1;
	end
	else if(((current_st_tmp != st4)&&(start)))
	begin
		mark_counter <= #du mark_counter-2'b1;
	end
	else
	begin
		mark_counter <= #du mark_counter;
	end
end

endmodule

