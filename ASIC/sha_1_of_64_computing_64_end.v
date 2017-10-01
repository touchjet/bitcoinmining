`timescale 1ns/100ps

module sha256_1_of_64_computing_64_end(reset_n,clk,k,rx_w,rx_8_variable,t1_tmp2,t1_tmp3,tx_w,tx_8_variable,t1_tmp2_next,t1_tmp3_next);
  input reset_n;
  input clk;
  input [31:0] k;//the next stage k input;
  input [511:0] rx_w;//first is the 512bits M data.
  input [255:0] rx_8_variable;//first is the initial H0
  input [31:0] t1_tmp2;
  input [31:0] t1_tmp3;
  output reg [511:0] tx_w;//output t+1 cycle W
  output reg [255:0] tx_8_variable;//output t+1 cycle 8 variables
  //output reg [31:0] t1_tmp2_next;
  //output reg [31:0] t1_tmp3_next;
  
  
parameter du =5'd1;//delay unit
 
  wire [31:0] s0_o;
  wire [31:0] s1_o;
  wire [31:0] e0_o;
  wire [31:0] e1_o;
  wire [31:0] ch_o;
  wire [31:0] maj_o;
  
  wire [31:0] a;
  wire [31:0] b;
  wire [31:0] c;
  //wire [31:0] d;
  wire [31:0] e;
  wire [31:0] f;
  wire [31:0] g;
  //wire [31:0] h;
  
  wire [31:0] t1;
  wire [31:0] t2;
  
  //assign  h = rx_8_variable[31:0];
  assign  g = rx_8_variable[63:32];
  assign  f = rx_8_variable[95:64];
  assign  e = rx_8_variable[127:96];
  //assign  d = rx_8_variable[159:128];
  assign  c = rx_8_variable[191:160];
  assign  b = rx_8_variable[223:192];
  assign  a = rx_8_variable[255:224];
  

  wire [31:0] w_16;//also is W_0
  wire [31:0] w_t_2;//Wt-2
  wire [31:0] w_t_7;//Wt-7
  wire [31:0] w_t_15;//Wt-15
  wire [31:0] w_t_16;//Wt-16
  
  assign  w_t_2=rx_w[63:32];//als0 is w_14
  assign  w_t_7=rx_w[223:192];//also is w_9
  assign  w_t_15=rx_w[479:448];//also is w_1
  assign  w_t_16=rx_w[511:480];//also is w_0
  
  e0s e0s_ins1(a,e0_o);
  e1s e1s_ins1(e,e1_o);
  chs chs_ins1(e,f,g,ch_o);
  majs majs_ins1(a,b,c,maj_o);
  
  s0s s0s_ins1(w_t_15,s0_o);
  s1s s1s_ins1(w_t_2,s1_o);
  
  //optimizate for t1 computing
  wire [31:0] t1_tmp1;
  //wire [31:0] t1_tmp2;
  //wire [31:0] t1_tmp3;
  /*
  assign t1_tmp1 = (h + e1_o);
  assign t1_tmp2 = (w_t_16 + k);
  assign t1_tmp3 = (t1_tmp1 + t1_tmp2);
  assign t1 = (t1_tmp3 + ch_o);
  */
  assign t1_tmp1 = (ch_o + e1_o);
  assign t1      = (t1_tmp2 + t1_tmp1);
  
  // compute t2;
  assign t2 = (e0_o + maj_o);

  // compute w16,optimizate for w16,also called new w
  wire [31:0] w_16_tmp1;  
  wire [31:0] w_16_tmp2;
  //wire [31:0] w_16;
  assign w_16_tmp1 = (s1_o + w_t_7);
  assign w_16_tmp2 = (s0_o + w_t_16);
  assign w_16 = (w_16_tmp1 + w_16_tmp2);
  
  always@(posedge clk)
  begin
  //M1={m0,m1,m2,m3,m4,m5,m6,m7,m8,m9,m10,m11,m12,m13,m14,m15}
    tx_w [511:0] <= #du {rx_w[479:0],w_16};
    // rx_8_variable={a,b,c,d,e,f,g,h}
    // tx_8_variable={a,b,c,d,e,f,g,h}
    tx_8_variable[255:224]<= #du t1+t2;//new_a
    tx_8_variable[223:192]<= #du a;//new_b = a
    tx_8_variable[191:160]<= #du b;//new_c = b
    tx_8_variable[159:128]<= #du c;//new_d = c
    tx_8_variable[127:96]<= #du (t1_tmp3 + t1_tmp1);//new_e = d +t1
    tx_8_variable[95:64]<= #du e;//new_f = e
    tx_8_variable[63:32]<= #du f;//new_g = f
    tx_8_variable[31:0]<= #du g;//new_h = g
  end
  
  //generate the t1_tmp2_next for next pipeline
  //wire [31:0] t1_tmp4_next;
  //wire [31:0] t1_tmp5_next;
  
  //assign t1_tmp4_next = (w_t_15 + g);//the next h = g;
  //assign t1_tmp5_next = (k + c);//the next d = c;
  //
  //always@(posedge clk)
  //begin
	//t1_tmp2_next <= #du (t1_tmp4_next + k);
	//t1_tmp3_next <= #du (t1_tmp5_next + t1_tmp4_next);
  //end

 
endmodule
