`timescale 1ns/100ps

module cs_sync(clk,cs_n_in,cs_sync_out);

	input clk;
	input cs_n_in;
	output reg cs_sync_out;

parameter du=3'd1;

reg cs_sync1;
//reg cs_sync2;
	
always@(posedge clk)
begin
	cs_sync1 <= #du cs_n_in;
	//cs_sync2 <= #du cs_sync1;
	cs_sync_out <= #du cs_sync1;
end

endmodule
	