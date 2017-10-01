/* module spi_slave for 328 bits.
For CPOL = 1; CPHA = 1;
*/

`timescale 1ns/100ps

module spi_slave (cs_n,sclk,mosi,miso,mosi_data,miso_data);
	//input reset_n;
	input cs_n;
	input sclk;
	input mosi;
	input [359:0] miso_data;
	output reg miso;
	//output reg [1:0] mode; //2'b01 for configure ,2'b10 for hash data.
	//output reg receiving;//1'b1 for is receiving data 1'b0 is idle
	output reg [359:0] mosi_data;
	//output reg [8:0] bit_counter_si;

reg [8:0] bit_counter_so;
parameter N = 9'd360;
parameter du = 5'd1;

wire sclk_miso;
wire cs;
assign sclk_miso = ~sclk;
assign cs = ~cs_n;

//simple mosi data
always@(posedge sclk)// or negedge reset_n)
begin
	/*
	if(!reset_n)
	begin
		mosi_data <= #du 360'b0;
		//receiving <= #du 1'b0;
	end
	else */ 
	if(!cs_n)//&&reset_n)
	begin
		mosi_data <= #du {mosi_data[N-2:0],mosi};		
		//receiving <= #du 1'b1;
	end
	else
	begin
		mosi_data <= #du mosi_data;
		//receiving <= #du 1'b0;
	end
end

/*
//genterate bit_counter_si
always@(posedge sclk or negedge cs)
begin
	if(!cs)
	begin
		bit_counter_si <= #du 9'b0;
	end
	else
	begin
		bit_counter_si <= #du (bit_counter_si + 9'b01);
	end
end

//generate mode  2'b01 for configure mode   2'b10 for hash data mode
always@(posedge sclk or negedge reset_n)
begin
	if(!reset_n)
	begin
		mode <= #du 2'b0;
	end
	else if(reset_n&&!cs_n)
	begin
		if((bit_counter_si==9'd8)&&(mosi_data[7:4]==4'b0000))
		begin
			mode <= #du 2'b01; //configure mode
		end
		else if((bit_counter_si==9'd8)&&(mosi_data[7:4]==4'b0001))
		begin
			mode <= #du 2'b10; //hash data mode
		end
		else
		begin
			mode <= #du mode;
		end
	end
	else 
	begin
		mode <= #du 2'b0;
		//bit_counter_si <= #du 9'b0;
	end
end
*/

//output miso data
always@(posedge sclk_miso)// or negedge reset_n)
begin
    /*
	if(!reset_n)
	begin
		miso <= #du 1'b0;
	end
	else */ 
	if(!cs_n)//&&reset_n)
	begin
		miso <= #du miso_data[N-1-bit_counter_so];
	end
	else
	begin
		miso <= #du 1'b1;
	end		
end

//genterate bit_counter_so
always@(posedge sclk_miso or negedge cs)
begin
	if(!cs)
	begin
		bit_counter_so <= #du 9'b0;
	end
	else
	begin
		bit_counter_so <= #du bit_counter_so + 9'b01;
	end
end

endmodule
