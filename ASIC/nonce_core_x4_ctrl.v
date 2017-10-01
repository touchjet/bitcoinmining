
`timescale 1ns/100ps

module nonce_core_x4_ctrl(clk,reset_n,ct_catch_nonce,ct_spi_to_nonce,hash_id1,rx_m_data1,rx_intial_h1,hash_id2,rx_m_data2,rx_intial_h2,mark,mark_counter,start,hash_id,rx_m_data,rx_intial_h);
	input clk;
	input reset_n;
	input [3:0] ct_catch_nonce;
	input [2:0] ct_spi_to_nonce;
	input [3:0] hash_id1;
	input [95:0] rx_m_data1;
	input [255:0] rx_intial_h1;
	input [3:0] hash_id2;
	input [95:0] rx_m_data2;
	input [255:0] rx_intial_h2;
	input mark;
	input [1:0] mark_counter;
	
	output reg start;
	output reg [3:0] hash_id;
	output reg [95:0] rx_m_data;
	output reg [255:0] rx_intial_h;

reg [1:0] current_st;
reg [1:0] next_st;

parameter du=4'd1;

parameter st0=2'b00;
parameter st1=2'b01;
parameter st2=2'b11;
parameter st4=2'b10;


reg st_en;

always@(*)//(posedge clk)
begin
	if((ct_spi_to_nonce==3'b001)&&(ct_catch_nonce==4'b0000))//&&(!busy))
	begin
		st_en <= #du 1'b1;
	end
	else
	begin
		st_en <= #du 1'b0;
	end
end

//state transition 
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		current_st <= #du st0;
	end
	else
	begin
		current_st <= #du next_st;
	end
end

//next_st logic
always@(*)
begin
	case(current_st)
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
			/*
			if((mark_counter == 2'b01)&&(st_en))
			begin
				next_st <= st2;//011
			end
			else if((mark_counter == 2'b10)&&(st_en))
			begin
				next_st <= st3;
			end*/
			if((mark_counter != 2'b0)&&(st_en))
			begin
				next_st <= st2;
			end
			else
			begin
				next_st <= st1;
			end
		end
	st2:begin//counter = 2'b1
			next_st <= st4;
		end
	//st3:begin//countter = 2'b10
	//		next_st <= st4;//110
	//	end
	st4:begin
			next_st <= st1;//0100
		end
		/*
	st5:begin
			next_st <= st6;
		end
	st6:begin
			next_st <= st1;
		end*/
	default:begin
				next_st <= st1;
			end
	endcase
end
		
			




//output logic
always@(posedge clk)
begin
	case(current_st)
	st0:begin
			start <= #du 1'b0;
			hash_id <= #du 4'b0;
			rx_m_data <= #du 96'b0;
			rx_intial_h <= #du 256'b0;
		end
	st1:begin
			start <= #du 1'b0;
			hash_id <= #du hash_id;
			rx_m_data <= #du rx_m_data;
			rx_intial_h <= #du rx_intial_h;
		end
	st2:begin//mark_counter = 1
			if(mark_counter == 2'b01)
			begin
				if(mark==1'b1)
				begin
					start <= #du 1'b1;
					hash_id <= #du hash_id1;
					rx_m_data <= #du rx_m_data1;
					rx_intial_h <= #du rx_intial_h1;
				end
				else
				begin
					start <= #du 1'b1;
					hash_id <= #du hash_id2;
					rx_m_data <= #du rx_m_data2;
					rx_intial_h <= #du rx_intial_h2;
				end
			end
			else
			begin//mark_counter = 2
				if(mark==1'b0)
				begin
					start <= #du 1'b1;
					hash_id <= #du hash_id1;
					rx_m_data <= #du rx_m_data1;
					rx_intial_h <= #du rx_intial_h1;
				end
				else
				begin
					start <= #du 1'b1;
					hash_id <= #du hash_id2;
					rx_m_data <= #du rx_m_data2;
					rx_intial_h <= #du rx_intial_h2;
				end	
			end
		end
		/*
	st3:begin////mark_counter = 2
			if(mark==2'b10)
			begin
				start <= #du 1'b1;
				hash_id <= #du hash_id1;
				rx_m_data <= #du rx_m_data1;
				rx_intial_h <= #du rx_intial_h1;
			end
			else if(mark==2'b01)
			begin
				start <= #du 1'b1;
				hash_id <= #du hash_id2;
				rx_m_data <= #du rx_m_data2;
				rx_intial_h <= #du rx_intial_h2;
			end
		end
		*/
	st4:begin
			start <= #du 1'b0;
			hash_id <= #du hash_id;
			rx_m_data <= #du rx_m_data;
			rx_intial_h <= #du rx_intial_h;
		end
		/*
	st5:begin
			start <= #du 1'b0;
			hash_id <= #du hash_id;
			rx_m_data <= #du rx_m_data;
			rx_intial_h <= #du rx_intial_h;
		end
	st6:begin
			start <= #du 1'b0;
			hash_id <= #du hash_id;
			rx_m_data <= #du rx_m_data;
			rx_intial_h <= #du rx_intial_h;
		end*/
	default:begin
				start <= #du 1'b0;
				hash_id <= #du hash_id;
				rx_m_data <= #du rx_m_data;
				rx_intial_h <= #du rx_intial_h;
			end
	endcase
end

endmodule
		
			
	
			
	