//gen irq signal

`timescale 1ns/100ps

module irq_gen(clk,reset_n,mark_counter,irq);
	input clk;
	input reset_n;
	input [1:0] mark_counter;
	output reg irq;

parameter du=3'd1;	

always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		irq <= #du 1'b0;
	end
	else
	begin
		if(mark_counter != 2'b10)
		begin
			irq <= #du 1'b1;
		end
		else
		begin
			irq <= #du 1'b0;
		end
	end
end

endmodule
