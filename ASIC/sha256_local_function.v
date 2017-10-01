`timescale 1ns/100ps

//For ch(x,y,z) function
//ch(x,y,z)=(x&y)^(~x&z)
module chs (x,y,z,o);
  input  [31:0] x;
  input  [31:0] y;
  input  [31:0] z;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  
  assign tmp1 = x&y;
  assign tmp2 = (~x)&z;
  assign o    = tmp1^tmp2;
  
endmodule

//For maj(x,y,z) function
//maj(x,y,z)=(x&y)^(x&z)^(y&z)
module majs(x,y,z,o);
  input [31:0] x;
  input [31:0] y;
  input [31:0] z;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  wire [31:0] tmp3;
  
  assign tmp1 = x&y;
  assign tmp2 = x&z;
  assign tmp3 = y&z;
  assign o    = tmp1^tmp2^tmp3;
  
endmodule

//For e0(x) function
module e0s(x,o);
  input [31:0] x;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  wire [31:0] tmp3;
  
  assign tmp1 = {x[1:0],x[31:2]};
  assign tmp2 = {x[12:0],x[31:13]};
  assign tmp3 = {x[21:0],x[31:22]};
  assign o    = tmp1^tmp2^tmp3;
  
endmodule
  
//For e1(x) function
module e1s(x,o);
  input [31:0] x;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  wire [31:0] tmp3;
  
  assign tmp1 = {x[5:0],x[31:6]};
  assign tmp2 = {x[10:0],x[31:11]};
  assign tmp3 = {x[24:0],x[31:25]};
  assign o    = tmp1^tmp2^tmp3;
  
endmodule

//For s0 functoins
module s0s(x,o);
  input [31:0] x;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  wire [31:0] tmp3;
  
  assign tmp1 = {3'b000,x[31:3]};
  assign tmp2 = {x[6:0],x[31:7]};
  assign tmp3 = {x[17:0],x[31:18]};
  assign o    = tmp1^tmp2^tmp3;
  
endmodule

//For s1 functoins
module s1s(x,o);
  input [31:0] x;
  output [31:0] o;
  
  wire [31:0] tmp1;
  wire [31:0] tmp2;
  wire [31:0] tmp3;
  
  assign tmp1 = {10'b0000000000,x[31:10]};
  assign tmp2 = {x[16:0],x[31:17]};
  assign tmp3 = {x[18:0],x[31:19]};
  assign o    = tmp1^tmp2^tmp3;
  
endmodule

/*
module e0 (x, y);

	input [31:0] x;
	output [31:0] y;

	assign y = {x[1:0],x[31:2]} ^ {x[12:0],x[31:13]} ^ {x[21:0],x[31:22]};

endmodule


module e1 (x, y);

	input [31:0] x;
	output [31:0] y;

	assign y = {x[5:0],x[31:6]} ^ {x[10:0],x[31:11]} ^ {x[24:0],x[31:25]};

endmodule


module ch (x, y, z, o);

	input [31:0] x, y, z;
	output [31:0] o;

	assign o = z ^ (x & (y ^ z));

endmodule


module maj (x, y, z, o);

	input [31:0] x, y, z;
	output [31:0] o;

	assign o = (x & y) | (z & (x | y));

endmodule


module s0 (x, y);

	input [31:0] x;
	output [31:0] y;

	assign y[31:29] = x[6:4] ^ x[17:15];
	assign y[28:0] = {x[3:0], x[31:7]} ^ {x[14:0],x[31:18]} ^ x[31:3];

endmodule


module s1 (x, y);

	input [31:0] x;
	output [31:0] y;

	assign y[31:22] = x[16:7] ^ x[18:9];
	assign y[21:0] = {x[6:0],x[31:17]} ^ {x[8:0],x[31:19]} ^ x[31:10];

endmodule

*/