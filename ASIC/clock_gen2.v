
`timescale 1ns/100ps
module clock_gen2(reset_n,osc_clk,temp_clk);
	input reset_n;
	input osc_clk;
	output reg temp_clk;
	
	reg osc_clk_1_2;
	parameter du=3'd1;
	
always@(posedge osc_clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		osc_clk_1_2 <= #du 1'b0;
	end
	else
	begin
		osc_clk_1_2 <= #du ~osc_clk_1_2;
	end
end

always@(posedge osc_clk_1_2 or negedge reset_n)
begin
	
	if(!reset_n)
	begin
		temp_clk <= #du 1'b0;
	end
	else
	begin
		temp_clk <= #du ~temp_clk;
	end
	
end

endmodule

