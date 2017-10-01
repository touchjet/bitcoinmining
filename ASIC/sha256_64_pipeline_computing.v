`timescale 1ns/100ps

module sha256_64_pipeline_computing(reset_n,clk,rx_w,rx_initial_h,tx_h);
  
  input reset_n;
  input clk;
  input [511:0] rx_w;//the block M which is going to be SHA256ed
  input [255:0] rx_initial_h;//the initial hash value;
  output reg [255:0] tx_h;//the hash value of rx_w;
  
  parameter du =5'd1; //delay unit
  
  reg [255:0] rx_w_next;
  //reg [255:0] rx_initial_h_next;
    
  localparam ks = {
		32'h428a2f98, 32'h71374491, 32'hb5c0fbcf, 32'he9b5dba5,
		32'h3956c25b, 32'h59f111f1, 32'h923f82a4, 32'hab1c5ed5,
		32'hd807aa98, 32'h12835b01, 32'h243185be, 32'h550c7dc3,
		32'h72be5d74, 32'h80deb1fe, 32'h9bdc06a7, 32'hc19bf174,
		32'he49b69c1, 32'hefbe4786, 32'h0fc19dc6, 32'h240ca1cc,
		32'h2de92c6f, 32'h4a7484aa, 32'h5cb0a9dc, 32'h76f988da,
		32'h983e5152, 32'ha831c66d, 32'hb00327c8, 32'hbf597fc7,
		32'hc6e00bf3, 32'hd5a79147, 32'h06ca6351, 32'h14292967,
		32'h27b70a85, 32'h2e1b2138, 32'h4d2c6dfc, 32'h53380d13,
		32'h650a7354, 32'h766a0abb, 32'h81c2c92e, 32'h92722c85,
		32'ha2bfe8a1, 32'ha81a664b, 32'hc24b8b70, 32'hc76c51a3,
		32'hd192e819, 32'hd6990624, 32'hf40e3585, 32'h106aa070,
		32'h19a4c116, 32'h1e376c08, 32'h2748774c, 32'h34b0bcb5,
		32'h391c0cb3, 32'h4ed8aa4a, 32'h5b9cca4f, 32'h682e6ff3,
		32'h748f82ee, 32'h78a5636f, 32'h84c87814, 32'h8cc70208,
		32'h90befffa, 32'ha4506ceb, 32'hbef9a3f7, 32'hc67178f2};//,32'h00000000};
  
  genvar i;
  generate
  for (i = 0; i < 64 ; i = i+1) begin : hash_pipeline
	wire [511:0] w_wire;
	wire [255:0] tx_8_variable_wire;
	wire [31:0] k;
	//wire [31:0] t1_tmp2_next;
	//wire [31:0] t1_tmp3_next;
	assign k = ks[32*(63-i) +: 32];
	if(i == 0)//the first 1/64 computing block
	  sha256_1_of_64_computing ins(
		.reset_n(reset_n),
		.clk(clk),
		.k(k),
		//.t1_tmp2(t1_tmp2_next_0),
		//.t1_tmp3(t1_tmp3_next_0),
		.rx_w(rx_w),
		.rx_8_variable(rx_initial_h),
		.tx_w(w_wire),
		.tx_8_variable(tx_8_variable_wire)
		//.t1_tmp2_next(t1_tmp2_next),
		//.t1_tmp3_next(t1_tmp3_next)
		);
	else
	  sha256_1_of_64_computing ins(
		.reset_n(reset_n),
		.clk(clk),
		.k(k),
		//.t1_tmp2(hash_pipeline[i-1].t1_tmp2_next),
		//.t1_tmp3(hash_pipeline[i-1].t1_tmp3_next),
		.rx_w(hash_pipeline[i-1].w_wire),
		.rx_8_variable(hash_pipeline[i-1].tx_8_variable_wire),
		.tx_w(w_wire),
		.tx_8_variable(tx_8_variable_wire)
		//.t1_tmp2_next(t1_tmp2_next),
		//.t1_tmp3_next(t1_tmp3_next)
		);
	end
  endgenerate
  	
  
  //output the hash value tx_h[255:0];
  always@(posedge clk)// or negedge reset_n)
  begin
/*	if(!reset_n)
	begin
		tx_h <= #du 256'b0;
	end
	else
	//// tx_8_variable={a,b,c,d,e,f,g,h}
	//// tx_h= {h0,h1,h2,h3,h4,h5,h6,h7}
	begin */
	  tx_h[255:224] <= #du (rx_initial_h[255:224] + hash_pipeline[63].tx_8_variable_wire[255:224]);//a+h0;
	  tx_h[223:192] <= #du (rx_initial_h[223:192] + hash_pipeline[63].tx_8_variable_wire[223:192]);//b+h1;
	  tx_h[191:160] <= #du (rx_initial_h[191:160] + hash_pipeline[63].tx_8_variable_wire[191:160]);//c+h2;
	  tx_h[159:128] <= #du (rx_initial_h[159:128] + hash_pipeline[63].tx_8_variable_wire[159:128]);//d+h3;
	  tx_h[127:96] <= #du (rx_initial_h[127:96] + hash_pipeline[63].tx_8_variable_wire[127:96]);//e+h4;
	  tx_h[95:64] <= #du (rx_initial_h[95:64] + hash_pipeline[63].tx_8_variable_wire[95:64]);//f+h5;
      tx_h[63:32] <= #du (rx_initial_h[63:32] + hash_pipeline[63].tx_8_variable_wire[63:32]);//g+h6;
	  tx_h[31:0] <= #du (rx_initial_h[31:0] + hash_pipeline[63].tx_8_variable_wire[31:0]);//h+h7;
//	end
  end
endmodule
  
