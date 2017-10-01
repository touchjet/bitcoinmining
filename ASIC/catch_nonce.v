//modify the irq_gen to the data_gen ,remove the cs_n from catch_nonce@2013102223

`timescale 1ns/100ps

module catch_nonce(clk,reset_n,busy,start,hash_id,success,nonce_input,data_gen_st,nonce1_output,nonce2_output,nonce_mark,nonce_mark_counter,current_st);
	input clk;
	input reset_n;
	input busy;
	//input cs_n;
	input success;
	input start;
	input [3:0] hash_id;
	input [31:0] nonce_input;
	input [3:0] data_gen_st;
	
	output reg [39:0] nonce1_output;
	output reg [39:0] nonce2_output;
	output reg nonce_mark;
	output reg [1:0] nonce_mark_counter;
	//output reg receive;
	output [3:0] current_st;
	
	reg [3:0] current_st_tmp;
	reg [3:0] next_st;	

parameter du = 3'd1;	
parameter st0 = 4'b0000;
parameter st1 = 4'b0001;
parameter st2 = 4'b0011;
parameter st3 = 4'b0010;
parameter st4 = 4'b0110;
parameter st5 = 4'b0111;
parameter st6 = 4'b0101;
parameter st7 = 4'b0100;

assign current_st = current_st_tmp;

//state transition 
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		current_st_tmp <= #du st6;
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
	st6:begin
			if(reset_n)
			begin
				next_st <= st0;
			end
			else
			begin
				next_st <= st6;
			end
		end
	st0:begin
			if(start)
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st0;
			end
		end
	st1:begin
			if(busy&&reset_n)
			begin
				next_st <= st7;
			end
			else
			begin
				next_st <= st1;
			end
		end
	st7:begin
			next_st <= st2;
		end
	st2:begin
			if((!busy))//&&(!success))
			begin
				next_st <= st5;//get no nonce
			end
			/*else if((!busy)&&success)
			begin
				next_st <= st4;//get one nonce
			end*/
			else
			begin
				next_st <= st2;
			end
		end
		/*
	st3:begin//get no nonce
			next_st <= st5;
		end
	st4:begin//get nonce
			next_st <= st5;
		end	
		*/
	st5:begin
			next_st <= st0;
		end		
	
	default:begin
				next_st <= st0;
			end
	endcase
end
	
//output logic
always@(posedge clk)
begin
	case(current_st_tmp)
	st6:begin
			nonce1_output <= #du 40'b0;
			nonce2_output <= #du 40'b0;
			//receive <= #du 1'b0;
			nonce_mark <= #du 1'b0;
		end
	st0:begin
			nonce1_output <= #du nonce1_output;
			nonce2_output <= #du nonce2_output;
			//receive <= #du 1'b0;
			nonce_mark <= #du nonce_mark;
		end
	st1:begin	
			nonce1_output <= #du nonce1_output;
			nonce2_output <= #du nonce2_output;
			//receive <= #du receive;
			nonce_mark <= #du nonce_mark;
		end
	/*st2:begin
			nonce1_output <= #du nonce1_output;
			nonce2_output <= #du nonce2_output;
			receive <= #du receive;
		end
		*/
	st2:begin//get no nonce
			if (!busy)
			begin
			    nonce_mark <= #du ~nonce_mark;
				if(nonce_mark==1'b0)
				begin
					nonce1_output <= #du {3'b0,success,hash_id,nonce_input};
					nonce2_output <= #du nonce2_output;
					//receive <= #du 1'b0;
				end
				else 
				begin
					nonce2_output <= #du {3'b0,success,hash_id,nonce_input};
					nonce1_output <= #du nonce1_output;
					//receive <= #du 1'b0;
				end
			end
			else
			begin
				nonce1_output <= #du nonce1_output;
				nonce2_output <= #du nonce2_output;
				//receive <= #du 1'b0;
				nonce_mark <= #du nonce_mark;
			end
		end
		/*
	st4:begin//get nonce
			if(nonce_mark==1'b0)
			begin
				nonce1_output <= #du {4'b1,hash_id,nonce_input};
				nonce2_output <= #du nonce2_output;
				receive <= #du 1'b0;
			end
			else if(nonce_mark==1'b1)
			begin
				nonce2_output <= #du {4'b1,hash_id,nonce_input};
				nonce1_output <= #du nonce1_output;
				receive <= #du 1'b0;
			end
		end
		*/
	st5:begin
			nonce1_output <= #du nonce1_output;
			nonce2_output <= #du nonce2_output;
			//receive <= #du 1'b1;
			nonce_mark <= #du nonce_mark;
		end
	default:begin
				nonce1_output <= #du nonce1_output;
				nonce2_output <= #du nonce2_output;
				//receive <= #du 1'b0;
				nonce_mark <= #du nonce_mark;
			end	
	endcase
end

/*
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		nonce_mark <= #du 1'b0;
	end
	else
	begin
		if((current_st_tmp==st2)||(current_st_tmp==st4))
		begin
			nonce_mark <= #du ~nonce_mark;
		end
		else
		begin
			nonce_mark <= #du nonce_mark;
		end
	end
end	
	*/
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		nonce_mark_counter <= #du 2'b0;
	end
	else
	if(((current_st_tmp==st2)&&(!busy))&&((data_gen_st != 4'b0110)&&(data_gen_st != 4'b0111)))
	begin
		nonce_mark_counter <= #du nonce_mark_counter+2'b1;
	end
	else if(((current_st_tmp != st2)||(busy))&&((data_gen_st == 4'b0110)||(data_gen_st == 4'b0111)))
	begin
		nonce_mark_counter <= #du nonce_mark_counter-2'b1;
	end
	else 
	begin
		nonce_mark_counter <= #du nonce_mark_counter;
	end
end
endmodule
