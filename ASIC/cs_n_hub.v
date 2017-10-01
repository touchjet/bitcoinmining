//cs_n_1 cs_n_2 cs_n_3 cs_n_4 hub module

`timescale 1ns/100ps

module cs_n_hub(cs_n_1,cs_n_2,cs_n_3,cs_n_4,cs_n_1_4);
	input cs_n_1;
	input cs_n_2;
	input cs_n_3;
	input cs_n_4;
	output cs_n_1_4;
	
assign cs_n_1_4 = ((cs_n_1&cs_n_2)&(cs_n_3&cs_n_4));

endmodule
