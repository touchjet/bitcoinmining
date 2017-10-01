`timescale 1ns/1ps

module clock_gen1(osc_clk,pll_n,pll_m,pll_pdn,gnd11a_pll,gnd11d_pll,vcc11a_pll,vcc11d_pll,pll_clk_out);
	input osc_clk;
	input [7:0] pll_n;
	input [7:0] pll_m;
	input pll_pdn;
	input gnd11a_pll;
	input gnd11d_pll;
	input vcc11a_pll;
	input vcc11d_pll;
	output pll_clk_out;
/*
wire gnd11a_pll;
assign gnd11a_pll=1'b0;

wire gnd11d_pll;
assign gnd11d_pll =1'b0;

wire vcc11a_pll;
assign vcc11a_pll=1'b1;

wire vcc11d_pll;
assign vcc11d_pll=1'b1;
*/

FXPLL110HH0L FXPLL110HH0L_inst(
.CKOUT(pll_clk_out), 
.CC0(1'b0), 
.CC1(1'b1), 
.F0(pll_m[6]), 
.F1(pll_m[7]), 
.FREF(osc_clk), 
.GND11A(gnd11a_pll), 
.GND11D(gnd11d_pll), 
//M
.MS0(pll_m[0]), 
.MS1(pll_m[1]), 
.MS2(pll_m[2]), 
.MS3(pll_m[3]), 
.MS4(pll_m[4]),
//N 
.NS0(pll_n[0]), 
.NS1(pll_n[1]), 
.NS2(pll_n[2]), 
.NS3(pll_n[3]), 
.NS4(pll_n[4]), 
.NS5(pll_n[5]), 
.NS6(pll_n[6]), 
.PDN(pll_pdn), 
.VCC11A(vcc11a_pll), 
.VCC11D(vcc11d_pll));
/*  output CKOUT;
  //output TCKO;
  input CC0;
  input CC1;
  input GND11A;
  input GND11D;
  input VCC11A;
  input VCC11D;
  //input CIN;
  input F0;
  input F1;
  input FREF;
  input MS0;
  input MS1;
  input MS2;
  input MS3;
  input MS4;
  input NS0;
  input NS1;
  input NS2;
  input NS3;
  input NS4;
  input NS5;
  input NS6;
  //input NS7;
  //input NS8;
  input PDN;
  //input TEST;
*/ 
 endmodule
 