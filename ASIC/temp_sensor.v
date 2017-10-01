
`timescale 1ns/100ps

module temp_sensor (reset_n,cs_n_1_4,temp_clk,temp_data,
TA,TB,TC,TD,TE,TF,TG,TH,TI,TJ,TLSA,TLSB,TLSC,TLSD,TLSE,TVREF,
xvrt,gnd33a_adc,gnd33d_adc,vcc33a_adc,vcc33d_adc
);
	input reset_n;
	input temp_clk;
	input cs_n_1_4;
	inout TA;
	inout TB;
	inout TC;
	inout TD;
	inout TE;
	inout TF;
	inout TG;
	inout TH;
	inout TI;
	inout TJ;
	inout TLSA;
	inout TLSB;
	inout TLSC;
	inout TLSD;
	inout TLSE;
	inout TVREF;
	input xvrt;
	input gnd33a_adc;
	input gnd33d_adc;
	input vcc33a_adc; 
	input vcc33d_adc;
	
	output reg [9:0] temp_data;
	
parameter du=3'd1;
	
	
reg [4:0] current_st;
reg [4:0] next_st;
reg [9:0] temp_data_tmp;


parameter st0=4'b0000;
parameter st1=4'b0001;
parameter st2=4'b0011;
parameter st3=4'b0010;
parameter st4=4'b0110;
parameter st5=4'b0100;
parameter st6=4'b0101;	
parameter st7=4'b0111;	
parameter st8= 4'b1111;	
parameter st9= 4'b1110;
parameter st10=4'b1100;	
parameter st11=4'b1101;	
parameter st12=4'b1001;
parameter st13=4'b1011;	
parameter st14=4'b1010;	

reg cs_sync1;
reg cs_sync2;
wire cs_sync_out;
always@(posedge temp_clk)
begin
	cs_sync1 <= #du cs_n_1_4;
	cs_sync2 <= #du cs_sync1;
end
assign cs_sync_out = cs_sync2;


reg [10:0] delay_counter;
reg reset_delay_n;


always@(posedge temp_clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		reset_delay_n <= #du 1'b0;
		delay_counter <= #du 11'b0;
	end
	else
	begin
		if((delay_counter == 11'b111111))//||(delay_counter == 11'b11111111110))//???
		begin
			reset_delay_n <= #du 1'b1;
			delay_counter  <= #du delay_counter;
		end
		else
		begin
			delay_counter <= #du delay_counter+1'b1;
			reset_delay_n <= #du 1'b0;
		end
	end
end

reg pwdb;
wire xeoc;
wire [9:0] xd;



//state transition 
always@(posedge temp_clk or negedge reset_delay_n)
begin
	if(!reset_delay_n)
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
			if(reset_delay_n)
			begin
				next_st <= st1;
			end
			else
			begin
				next_st <= st0;
			end
		end
	st1:begin
			next_st <= st2;
		end
	st2:begin
			if(xeoc)
			begin
				next_st <= st3;
			end
			else
			begin
				next_st <= st2;
			end
		end
	st3:begin
			if(!xeoc)
			begin
				next_st <= st4;
			end
			else
			begin
				next_st <= st3;
			end
		end		
	st4:begin
			if(xeoc)
			begin
				next_st <= st5;
			end
			else
			begin
				next_st <= st4;
			end
		end
	st5:begin
			if(!xeoc)
			begin
				next_st <= st6;
			end
			else
			begin
				next_st <= st5;
			end
		end
	st6:begin
			if(xeoc)
			begin
				next_st <= st7;
			end
			else
			begin
				next_st <= st6;
			end
		end
	st7:begin
			if(!xeoc)
			begin
				next_st <= st8;
			end
			else
			begin
				next_st <= st7;
			end
		end
	st8:begin
			if(xeoc)
			begin
				next_st <= st9;
			end
			else
			begin
				next_st <= st8;
			end
		end
	st9:begin
			//if(!xeoc)
			//begin
				next_st <= st10;
			//end
			//else
			//begin
			//	next_st <= st9;
			//end
		end
/*	st14:begin
			next_st <= st10;
		end*/
	st10:begin
			if(cs_sync_out)
			begin
				next_st <= st11;
			end
			else
			begin
				next_st <= st10;
			end
		end
	st11:begin
			if(!cs_sync_out)
			begin
				next_st <= st12;
			end
			else
			begin
				next_st <= st11;
			end
		end
	st12:begin
			if(cs_sync_out)
			begin
				next_st <= st13;
			end
			else
			begin
				next_st <= st12;
			end
		end
	st13:begin
			next_st <= st1;
		end
	default:begin
				next_st <= st1;
			end
	endcase
end

//output logic
always@(posedge temp_clk)
begin
	case(current_st)
	st0:begin
			pwdb <= #du 1'b1;
			temp_data <= #du 10'b0;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st1:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st2:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st3:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st4:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st5:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st6:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st7:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st8:begin
			pwdb <= #du 1'b1;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st9:begin
			pwdb <= #du 1'b1;
			temp_data_tmp <= #du xd;//sample temp_sensor XD data
			temp_data <= #du temp_data;
		end
/*	st14:begin
			pwdb <= #du 1'b0;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end*/
	st10:begin
			pwdb <= #du 1'b0;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st11:begin
			pwdb <= #du 1'b0;
			temp_data <= #du temp_data_tmp;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st12:begin
			pwdb <= #du 1'b0;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	st13:begin
			pwdb <= #du 1'b0;
			temp_data <= #du temp_data;
			temp_data_tmp <= #du temp_data_tmp;
		end
	default:begin
				pwdb <= #du 1'b1;
				temp_data <= #du temp_data;
				temp_data_tmp <= #du temp_data_tmp;
			end
	endcase
end

wire VBG;
/*wire TA;
wire TB;
wire TC;
wire TD;
wire TE;
wire TF;
wire TG;
wire TH;
wire TI;
wire TJ;
wire TLSA;
wire TLSB;
wire TLSC;
wire TLSD;
wire TLSE;
wire TVREF;
*/
reg xvrefen;

FXADC880HH0L_FTCM8A FXADC880HH0L_FTCM8A_inst(
.VBG(VBG),//ouput Bandgap output for trimming
.XD0(xd[0]),.XD1(xd[1]),.XD2(xd[2]),.XD3(xd[3]),.XD4(xd[4]),
.XD5(xd[5]),.XD6(xd[6]),.XD7(xd[7]),.XD8(xd[8]),.XD9(xd[9]), 
.XEOC(xeoc), 
.TA(TA),.TB(TB),.TC(TC),.TD(TD),.TE(TE),.TF(TF),.TG(TG),.TH(TH),.TI(TI),.TJ(TJ),// to top
.TLSA(TLSA),.TLSB(TLSB),.TLSC(TLSC),.TLSD(TLSD),.TLSE(TLSE),.TVREF(TVREF),// to top
.XVRT(xvrt),//to top 
.GND11K(1'b0), 
.GND33A(gnd33a_adc), .GND33D(gnd33d_adc), //to top
.TEMP(1'b1), //1'b1 for temp mode
.VCC11K(1'b1), 
.VCC33A(vcc33a_adc), .VCC33D(vcc33d_adc),//to top
.XAIN0(1'b0),.XAIN1(1'b0),.XAIN2(1'b0),.XAIN3(1'b0),
.XAIN4(1'b0),.XAIN5(1'b0),.XAIN6(1'b0),.XAIN7(1'b0), 
.XAINSEL0(1'b0), .XAINSEL1(1'b0), .XAINSEL2(1'b0), .XAINSEL3(1'b0), 
.XAINSEL4(1'b0), .XAINSEL5(1'b0), .XAINSEL6(1'b0), .XAINSEL7(1'b0),
.XMCLK(temp_clk),
.XPWDB(pwdb), 
.XRST(~reset_delay_n), //1'b0 enter the normal operating mode
.XSAMPCLK(1'b0), 
.XSAMPSEL(), //1'b0 to select the internal sampling clock
.XVREFEN(xvrefen));//?????1'b1 to enter the internal mode
/*inout VBG;
  output XD0;
  output XD1;
  output XD2;
  output XD3;
  output XD4;
  output XD5;
  output XD6;
  output XD7;
  output XD8;
  output XD9;
  output XEOC;
  inout TA;
  inout TB;
  inout TC;
  inout TD;
  inout TE;
  inout TF;
  inout TG;
  inout TH;
  inout TI;
  inout TJ;
  inout TLSA;
  inout TLSB;
  inout TLSC;
  inout TLSD;
  inout TLSE;
  inout TVREF;
  input XVRT;
  input GND11K;
  input GND33A;
  input GND33D;
  input TEMP;
  input VCC11K;
  input VCC33A;
  input VCC33D;
  input XAIN0;
  input XAIN1;
  input XAIN2;
  input XAIN3;
  input XAIN4;
  input XAIN5;
  input XAIN6;
  input XAIN7;
  input XAINSEL0;
  input XAINSEL1;
  input XAINSEL2;
  input XAINSEL3;
  input XAINSEL4;
  input XAINSEL5;
  input XAINSEL6;
  input XAINSEL7;
  input XMCLK;
  input XPWDB;
  input XRST;
  input XSAMPCLK;
  input XSAMPSEL;
  input XVREFEN;
*/

endmodule

			
			
			
			
			
				