module busy_out(busy_input,busy_output);
	input [3:0] busy_input;
	output busy_output;
	
assign busy_output = ((busy_input[0]&busy_input[1])&(busy_input[2]&busy_input[3]));

endmodule
