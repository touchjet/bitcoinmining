
`timescale 1ns/100ps
//`timescale 1ns/1ps


module nonce_core_x8 (clk,reset_n,start,rx_m_data,rx_initial_h,hash_id_input,hash_dify_input,busy,success,nonce,hash_id,hash_dify);
	input clk;
	input reset_n;
	input start;
	input [95:0] rx_m_data;//block data need to be Hashed
	input [255:0] rx_initial_h;//initial hash data from the Software
	input [3:0] hash_id_input;
	input [31:0] hash_dify_input;
	output reg busy;// 1'b1: computing 1'b0: no computing
	output reg success;// 1'b1: get a nonce 1'b0:there no nonce
	output reg [31:0] nonce;
	output [3:0] hash_id;
	output [31:0] hash_dify;
	//output reg [30:0] nonce_add;//output is for simulation
	
parameter du = 4'd1;

//reg busy;
//reg success;
//reg [1:0] core_id;//the core who get the nonce
reg [29:0] nonce_add;
wire [63:0] hash_dify_com;
parameter nonce_start = 30'h0;
//parameter nonce_start = 31'hf02b3b0;
//parameter nonce_start = 31'h3943b3a0;//from liu
//parameter nonce_start = 31'h0b21bc30;
//parameter nonce_start = 31'h2d35c2f1; //from liu
//parameter nonce_start = 31'h000007f0;

//reg [30:0] nonce_tmp;
//wire [255:0] initila_h_reg_core_1_4;

//rx_m_data_reg <= #du {rx_m_data,nonce_start,1'b1,319'b0,64'h280};
//rx_m_data_reg <= #du {rx_m_data_tmp,nonce[31:0],1'b1,319'b0,64'h280};
//reg [511:0] m_data_reg_core1;
//reg [511:0] m_data_reg_core2;
//reg [511:0] m_data_reg_core3;
//reg [511:0] m_data_reg_core4;

wire core_1_com;
wire core_2_com;
wire core_3_com;
wire core_4_com;
wire core_5_com;
wire core_6_com;
wire core_7_com;
wire core_8_com;

wire [255:0] hash_result_core1_1;
wire [255:0] hash_result_core1_2;
wire [255:0] hash_result_core2_1;
wire [255:0] hash_result_core2_2;
wire [255:0] hash_result_core3_1;
wire [255:0] hash_result_core3_2;
wire [255:0] hash_result_core4_1;
wire [255:0] hash_result_core4_2;
wire [255:0] hash_result_core5_1;
wire [255:0] hash_result_core5_2;
wire [255:0] hash_result_core6_1;
wire [255:0] hash_result_core6_2;
wire [255:0] hash_result_core7_1;
wire [255:0] hash_result_core7_2;
wire [255:0] hash_result_core8_1;
wire [255:0] hash_result_core8_2;

//capture the new computing data and start to nonce ++
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		nonce_add <= #du nonce_start;
		//initila_h_reg_core_1_4 <= #du 256'b0;
		//m_data_reg_core1 <= #du 512'b0;
		//m_data_reg_core2 <= #du 512'b0;
		//m_data_reg_core3 <= #du 512'b0;
		//m_data_reg_core4 <= #du 512'b0;
	end
	else if(reset_n&&start)//&&(!busy))
	begin
		nonce_add <= #du (nonce_start+30'b1);
		//initila_h_reg_core_1_4 <= #du rx_initial_h;
		//m_data_reg_core1 <= #du {rx_m_data,2'b00,nonce_start[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core2 <= #du {rx_m_data,2'b01,nonce_start[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core3 <= #du {rx_m_data,2'b10,nonce_start[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core4 <= #du {rx_m_data,2'b11,nonce_start[29:0],1'b1,319'b0,64'h280};
	end
	else if(reset_n&&(!start)&&busy)
	begin
		nonce_add <= #du nonce_add + 30'b1;
		//initila_h_reg_core_1_4 <= #du initila_h_reg_core_1_4;
		//m_data_reg_core1 <= #du {m_data_reg_core1[511:414],nonce_add[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core2 <= #du {m_data_reg_core2[511:414],nonce_add[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core3 <= #du {m_data_reg_core3[511:414],nonce_add[29:0],1'b1,319'b0,64'h280};
		//m_data_reg_core4 <= #du {m_data_reg_core4[511:414],nonce_add[29:0],1'b1,319'b0,64'h280};
	end
	else if(reset_n&&(!start)&&(!busy)&&(success))
	begin
		nonce_add <= #du nonce_start;
	end
	else
	begin
		nonce_add <= #du nonce_start;
	end
end

//generate the busy and success signal and nonce_tmp
//assign #1 nonce_tmp = (nonce_add - 31'd133);

always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		busy <= #du 1'b0;
		success <= #du 1'b0;
		//core_id <= #du 2'b0;
		//nonce_tmp <= #du 31'b0;
		nonce <= #du 32'b0;
	end
	else if(reset_n&&start)
	begin
		busy <= #du 1'b1;
		success <= #du 1'b0;
		nonce <= #du 32'b0;
	end
	else if((nonce_start<nonce_add)&&(nonce_add < (30'h83+nonce_start)))
	begin
		busy <= #du 1'b1;
		success <= #du 1'b0;
		nonce <= #du 32'b0;
	end
	else if(((30'h83+nonce_start) <= nonce_add)&&(nonce_add <= (31'h1fffffff+30'h83)))//31'h0f02b3b0
	begin
		if((core_1_com||core_2_com||core_3_com||core_4_com||core_5_com||core_6_com||core_7_com||core_8_com))
		begin
			busy <= #du 1'b0;
			success <= #du 1'b1;
			nonce[28:0] <= #du (nonce_add - 9'd130);
			//generate nonce
			if(core_1_com)
			begin
				nonce[31:29] <= #du 3'b000;//0
			end
			else if(core_2_com)
			begin
				nonce[31:29] <= #du 3'b001;//1
			end
			else if(core_3_com)
			begin
				nonce[31:29] <= #du 3'b010;//2
			end
			else if(core_4_com)
			begin
				nonce[31:29] <= #du 3'b011;//3
			end
			else if(core_5_com)
			begin
				nonce[31:29] <= #du 3'b100;//4
			end
			else if(core_6_com)
			begin
				nonce[31:29] <= #du 3'b101;//5
			end
			else if(core_7_com)
			begin
				nonce[31:29] <= #du 3'b110;//6
			end
			else 
			begin
				nonce[31:29] <= #du 3'b111;//7
			end
		end
		else
		begin
			busy <= #du busy;
			success <= #du 1'b0;
			nonce <= #du 32'b0;
		end
	end
	else if(((31'h1fffffff+31'h83) < nonce_add ))
	begin
		busy <= #du 1'b0;
		success <= #du 1'b0;
		nonce <= #du 32'b0;
	end
	else
	begin
		busy <= #du 1'b0;
		success <= #du 1'b0;
		nonce <= #du 32'b0;
	end
	
end		

//generate hash id
/*
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		hash_id <= #du 4'b0;
	end
	else if(reset_n&&start)//&&(!busy))
	begin
		hash_id <= #du hash_id_input;
	end
	else
	begin
		hash_id <= #du hash_id;
	end
end
*/

assign hash_id = hash_id_input;
/*
//load hash_dify_input generate hash_dify
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		hash_dify_com <= #du {32'h00000000,32'hffffffff};
		hash_dify <= #du 32'hffffffff;
	end
	else
	begin
		hash_dify <= #du(~hash_dify_com[63:32]);
		hash_dify_com <= #du {~hash_dify_input,32'hffffffff};
	end
end
*/
assign hash_dify = (~hash_dify_com[63:32]);
assign hash_dify_com = {~hash_dify_input,32'hffffffff};

//generate busy success nonce output
/*
always@(posedge clk or negedge reset_n)
begin
	if(!reset_n)
	begin
		busy <= #du 1'b0;
		success <= #du 1'b0;
		nonce <= #du 32'b0;
	end
	else
	begin
		busy <= #du busy;
		success <= #du success;
		nonce <= #du {core_id,nonce_tmp[29:0]};
	end
end
*/



assign core_1_com = ((hash_result_core1_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_2_com = ((hash_result_core2_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_3_com = ((hash_result_core3_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_4_com = ((hash_result_core4_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_5_com = ((hash_result_core5_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_6_com = ((hash_result_core6_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_7_com = ((hash_result_core7_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||
assign core_8_com = ((hash_result_core8_2[63:0] & hash_dify_com) == 64'b0) ? 1'b1 : 1'b0 ;//||		

//core1 nonce{00,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core1_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b000,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core1_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core1_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core1_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core1_2)); 

//core2 nonce{01,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core2_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b001,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core2_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core2_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core2_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core2_2)); 

//core3 nonce{10,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core3_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b010,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core3_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core3_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core3_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core3_2)); 
    
//core4 nonce{11,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core4_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b011,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core4_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core4_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core4_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core4_2)); 

//core1 nonce{00,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core5_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b100,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core5_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core5_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core5_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core5_2)); 

//core2 nonce{01,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core6_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b101,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core6_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core6_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core6_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core6_2)); 

//core3 nonce{10,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core7_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b110,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core7_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core7_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core7_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core7_2)); 
    
//core4 nonce{11,nonce_add[29:0];}
//SHA256 SHA1 
  sha256_64_pipeline_computing core8_inst1(
  .reset_n(reset_n),.clk(clk),.rx_w({rx_m_data,3'b111,nonce_add[28:0],1'b1,319'b0,64'h280}),.rx_initial_h(rx_initial_h),
  .tx_h(hash_result_core8_1));
  //SHA256 SHA2
  sha256_64_pipeline_computing core8_inst2(
  .reset_n(reset_n),.clk(clk),.rx_w({hash_result_core8_1,1'b1,191'b0,64'h0100}),
  .rx_initial_h(256'h6a09e667bb67ae853c6ef372a54ff53a510e527f9b05688c1f83d9ab5be0cd19),
  .tx_h(hash_result_core8_2));   
  
endmodule  
  

  