using System;

namespace FusionMiner
{
	public class TestDataPool:MiningPool
	{
		private int _testDataIndex = 0;

		public TestDataPool ()
		{
			_status = PoolConnectionStatus.Active;
			_difficulty = 1.0;
		}

		public override void ConnectPool ()
		{
		}

		public override bool GetHashJob (ref HashData hashData)
		{
			hashData.JobId = _testDataIndex.ToString ();
			hashData.ExtraNonce2 = "00000000";
			hashData.BlockHeaderBin = Utility.HexStringToByteArray (slefTestData [_testDataIndex].BlockHeader + "00000000");
			Utility.ReverseWord (hashData.BlockHeaderBin);
			Sha256LibManaged.CalcMidstate (hashData.BlockHeaderBin, hashData.MidStateBin);
			Utility.ReverseWord (hashData.BlockHeaderBin);

			if (_testDataIndex < slefTestData.Length - 1) {
				_testDataIndex++;
			} else {
				_testDataIndex = 0;
			}
			_lastRecepitonTime = DateTime.UtcNow;
			return true;
		}


		public override void SubmitHashResult (HashData hashData)
		{
			byte[] hash = new byte[32];
			hashData.BlockHeaderBin [76] = (byte)(hashData.Nonce >> 24 & 0xff);
			hashData.BlockHeaderBin [77] = (byte)(hashData.Nonce >> 16 & 0xff);
			hashData.BlockHeaderBin [78] = (byte)(hashData.Nonce >> 8 & 0xff);
			hashData.BlockHeaderBin [79] = (byte)(hashData.Nonce >> 0 & 0xff);

			Sha256LibManaged.DoubleSha256 (hashData.BlockHeaderBin, 80, hash);

			string hashstr = Utility.ByteArrayToHexString (hash);
			Utility.Log (LogLevel.Debug, "Unique Id:" + hashData.UniqueId.ToString ());
			Utility.Log (LogLevel.Debug, "Nonce:" + hashData.Nonce.ToString ("x"));
			Utility.Log (LogLevel.Debug, "Hash:" + hashstr);
			_accepted++;
		}

		public struct SelfTestData
		{
			public int Index;
			public string BlockHeader;
			public string MidState;
			public string RestHeader;
			public string Nonce;
		}

		public static SelfTestData[] slefTestData = new SelfTestData[] {
			new SelfTestData () {
				Index = 0,
				BlockHeader = "972b89e8972b89e893a742664bdb8c50131780fa4f3a86a817a45feb4304c64fe6b253dfe5c8eb5c1d11476b6b04e81673a659e7a6ae2a063ececd46ed664f0d1042b9253289ed0c972b89e8",
				MidState = "5ea4b95da029ca07dec6c633607be238bb4543a00102861f1f7a7be0140a17c3",
				RestHeader = "25b942100ced8932e8892b97",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 1,
				BlockHeader = "000000025696cabdbcca5ffba18b75058314fff4766553e7daf6b3150000000400000000f9bb89752f599157e3fa84a076f1d54ba27926d37d3e39cecc4134b1c921660f9a017cf8529eca70",
				MidState = "a7285e3fded48a3533923403b1b92bba23941f4c3428a814fbf4905acee8a4c3",
				RestHeader = "0f6621c9f87c019a70ca9e52",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 2,
				BlockHeader = "000000025696cabdbcca5ffba18b75058314fff4766553e7daf6b3150000000400000000f84969fb739e7e65fc854a501020634e68d4a2ce9d7baefe9b487aa619a68533413fd23a529eca72",
				MidState = "4efdf0adae00e6a96162a02048c8d134117e85a26d33d7c96105b766eb4ae60b",
				RestHeader = "3385a6193ad23f4172ca9e52",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 3,
				BlockHeader = "188fda3d188fda3d3efce1ea1a838263fe4abc77c856c14e09bccc658add4b86ff42144da02841016a307e5a88db3dbe7ccb80bf0ca96c5bf6d0525b6291f09b17fa025374d7b6e5188fda3d",
				MidState = "e9d2eff8f2d640788463c23bc245f855a62d4db9549bef3432ddba641b9c0e36",
				RestHeader = "5302fa17e5b6d7743dda8f18",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 4,
				BlockHeader = "e9b99100e9b99100361f84d63897ff06ee93848730b9b4791e00249443c0a21c433fc9af7708279a062054342b9c4225044cf6efabf2d5fbee8a77525e4f271cc058326af783df09e9b99100",
				MidState = "52e53255d4abd02efb937bf47824b3119e9dcc0e0c9b09a67d1acf0b1d5b5fcf",
				RestHeader = "6a3258c009df83f70091b9e9",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 5,
				BlockHeader = "2e07cec22e07cec2ad24ad911cebf50355f51696a162f8e947a7836a390f68a18acf5a55233940447cca7612753cf2fe751aabc9b05ab66c872a035ce3b3558dc0cd033459b4cc922e07cec2",
				MidState = "fe39ea0097514b1c72e84b1c73b83fab2b2f81e185b91dcf70e7aaa254e9d58c",
				RestHeader = "3403cdc092ccb459c2ce072e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 6,
				BlockHeader = "972b89e8972b89e893a742664bdb8c50131780fa4f3a86a817a45feb4304c64fe6b253dfe5c8eb5c1d11476b6b04e81673a659e7a6ae2a063ececd46ed664f0d1042b9253289ed0c972b89e8",
				MidState = "5ea4b95da029ca07dec6c633607be238bb4543a00102861f1f7a7be0140a17c3",
				RestHeader = "25b942100ced8932e8892b97",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 7,
				BlockHeader = "33b5f24633b5f2468682a09414b37e7fac8cca5a7fd8bbf9d0649eaa8b36d288e9b48f9207d32399773de4d74d68fc8c9e0752c74580d43907f539556b6d776a53aa8f75d36a2ed333b5f246",
				MidState = "19407d7bbd636e50f9192ee83f648dc4f28739f3015a4dd559a1f440b9e0e213",
				RestHeader = "758faa53d32e6ad346f2b533",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 8,
				BlockHeader = "52341a6252341a62925bcb8d7adc82124a023731294a9a52ac11d214164c8d55b9ce448480a84df52bc35e031d1fe0dfffcd6f1e3119c357a46772b738062ed229172a57ac44b3b252341a62",
				MidState = "06553aa87c279676941b3a2aa1205476cbdaf2ac7da9bda77d693c5d22272524",
				RestHeader = "572a1729b2b344ac621a3452",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 9,
				BlockHeader = "41ef291141ef29113a1f1d7351bacf312d0847878a7544db714987a64ea026cce3129221cfc4a1ae205fa0116cae6b5ffd03e55bd1d3d3db350dd048d5d49424e58f2114de6d893041ef2911",
				MidState = "8a193a77ed81e174143f36d16d830ef3cf9ed55cf2798008c0507ee420713cad",
				RestHeader = "14218fe530896dde1129ef41",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 10,
				BlockHeader = "340729d6340729d6ecb89331da3ecb2b797aa66d39c35f00a16e4d51cf09ddeb143b46455b584896e371d7502d31f1e8e0d0f8d53a99c8c5fdd4dec77edd1e767bdbd02b25b0bffe340729d6",
				MidState = "5da878e8ff34210e2e5cd70a9e8c425e6a9be54b8bdea25d8bf46883d09cef29",
				RestHeader = "2bd0db7bfebfb025d6290734",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 11,
				BlockHeader = "072e5ab1072e5ab1ed39042e5bb4dd9acb785acbc401ca65e3dc80731272ebf8b33bb20489bf28cfbb69e37b8eb5e0147d072b568e86509848a6842d3e674a86c87a970035e7ea39072e5ab1",
				MidState = "257660311bf57731c3f59a4cd8ae12355230e3590a29f2fca061b92797d53127",
				RestHeader = "00977ac839eae735b15a2e07",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 12,
				BlockHeader = "be708c8cbe708c8c2dfc25d345a1cdaac3fb4bfafd1cc4e6e4b2ef91587921ce3965bbc5ff9aad408ab126d68c134998dd9a94043542bf409ebb6c0f7ec88fc68e843c1472fd45fdbe708c8c",
				MidState = "1490e2049a88a1cd3a0755f337d1d125d253c3bce545880abfa50dde2d9db521",
				RestHeader = "143c848efd45fd728c8c70be",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 13,
				BlockHeader = "21de80fe21de80fe3e13c3e881a5eb9de3982ba9df34545c842adb33fbbbda179f20037f858078d38afdc90d1dc92e8b28cb9241c7dc8a69e7a9260a113ebe8b3ad79b2c12fcb6d721de80fe",
				MidState = "3fb65b547a9e4b2e06834614267f011acb077d52284c22e2d181f6c07ee62e94",
				RestHeader = "2c9bd73ad7b6fc12fe80de21",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 14,
				BlockHeader = "2ffb8f272ffb8f27d910ce4a7e59c7ea8c7d3d376ea671100c90772831eb64e0e316759152f63f27c3c6231b223d8f8c7ea62aa105b3af333c3b5c64dcd80b43d3cd4243da1bb4622ffb8f27",
				MidState = "3dc4899c286525136a4551da5cc4e1aa8cbba9deccce6cb6011d80fb6c371028",
				RestHeader = "4342cdd362b41bda278ffb2f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 15,
				BlockHeader = "0a1c27940a1c279479f2df9c73cf3ef208bfb64e8ab46e508cfc2e3754efffcba62afa7261f68a18fbef8a8c457f2e919dab50113e32ba3319bf076d87f2b9b04dc95c5a69db86e90a1c2794",
				MidState = "d45f6eeb8af687de156ae39b4bbc8b89d36851d606bf77e43e1025bc37cd7812",
				RestHeader = "5a5cc94de986db6994271c0a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 16,
				BlockHeader = "c6e3052bc6e3052bd9c239e1d4220c0ee56a5a081f0c18f948445c94a319638d3e2129ea131aa4f34348f9cde5d35c63411acc75a6b0f240d3a2f8e4f7d1697c456aa8006939f46ac6e3052b",
				MidState = "df6e28131ca9fe46287642add08e0145227dbf7b69cfb13742b3d7984ab3c365",
				RestHeader = "00a86a456af439692b05e3c6",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 17,
				BlockHeader = "610c203a610c203aab014c59ca3eee47fd2f0c76e70c7f9cf77be94189917c05427e22e15448af4bdf1f1cc07d0e74ef675c287378c369d2dca853aeb168929ee067051cfa7948b7610c203a",
				MidState = "a909ae8377996790ceec545a69289257e10fe014e1361210bd7984bba5771db6",
				RestHeader = "1c0567e0b74879fa3a200c61",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 18,
				BlockHeader = "34b57edc34b57edcea92fc697bbcde748c4a902a4e127a7fc4710fa20ac1858e8ad1ebe4ea3c271b22d91ac5b47228dbeb349e81111ecf59cec200344b04c78658e41a6b1fc222a034b57edc",
				MidState = "cf286d9038e30579f606ec55bcbc2cca5e1a0ce6db730dced112631b64f41072",
				RestHeader = "6b1ae458a022c21fdc7eb534",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 19,
				BlockHeader = "1df226731df226732571181c6ce531c8b1711bcff9919573cc4362a664f5aa4291dce2d68ac4940db9e35ead127f7498e85820663102e4fc7bd3c3e81cccd22fbd677c1c7cd3f6b71df22673",
				MidState = "46c2ed23907196eb1728e69b387a613e34c816cbc89c5bfa6fee1e718a59f37b",
				RestHeader = "1c7c67bdb7f6d37c7326f21d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 20,
				BlockHeader = "28aeb8ac28aeb8ac621e2668adc6e2af008ca467f28398ae360da6b5293bb2ab7dc7a50ebcac1e9bedca50dff50b4d9d5ff1afae5d794693bea7077601c91b71fb73af2b935eabb428aeb8ac",
				MidState = "62693e32e30feef9aa5bc6812fae54563302290c7fc43e0d08bc32aa593ea853",
				RestHeader = "2baf73fbb4ab5e93acb8ae28",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 21,
				BlockHeader = "abc280461db5745f8fb2aa79de8e29677f0153f379a394999c868bf6eab78c370c7123868f6ec779f80c62aa656bc5e1ccdd147de3def249e20f9ff1e8367bfb80f410104b10bf9e1db5745f",
				MidState = "d0192daf7a3f0d1856e955817ad7e24fcbeeaa42ee61bca65c68e1cb71ff2830",
				RestHeader = "1010f4809ebf104b5f74b51d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 22,
				BlockHeader = "af51162faf51162fd8a26dc40f4cf507010754f1b6324721580606346a7914bf063de55e6ba453bd00c897736b5210e2db942c66241d37829ee1c15af9e0c22075ca1a676fa3f9b6af51162f",
				MidState = "ec04d93f5e59bacd022a1fa621aab59a74dfd7e06acbdd2a9546a51a3fc833fe",
				RestHeader = "671aca75b6f9a36f2f1651af",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 23,
				BlockHeader = "ea13dbb0ea13dbb0c65847542d43d46ad3ab34909b6cd6768df6539afb124a53433c3ee540399d42d5f2ea385b741259100db2fe1d931ef2a60c880aa2bde53930810d4d86381d8dea13dbb0",
				MidState = "4c4daa469c4e4212c9db49cb2a27872d599502d5d96388ce4ed342720615c418",
				RestHeader = "4d0d81308d1d3886b0db13ea",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 24,
				BlockHeader = "4ca2d8f44ca2d8f46e648b8e052270ddd3c5c0aa174290866c32bffa754d5ee8a6fb2f43145c5f51526288e94c549b0f4beeb1dfc7502a356287a1fd57eecf005e27c140f84084d54ca2d8f4",
				MidState = "9d44e68a22756143c9fa641ce572f25b93e6cd8ee59c502a08a56fc080aa2a83",
				RestHeader = "40c1275ed58440f8f4d8a24c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 25,
				BlockHeader = "ede5f8dcede5f8dc3a377f0b492fa65567f7b729b7455b46e5b67b173f7a19d05f63cee6c06b7773012d7aa05135d81d32e5307bca76f5ce7f8eeda5d875c5f8b5fed465c369d263ede5f8dc",
				MidState = "9bedb17d5eb6c92a3721edbd61cc3e9ec31429502d2481eb1ab91a38f2ed3890",
				RestHeader = "65d4feb563d269c3dcf8e5ed",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 26,
				BlockHeader = "f53625f8f53625f828f30eac0e50452e2e4a093a74f48c9858a79afaca7ed4edceabcf097e75109460bcff474bf474cb5f253c3e0e5a81ce37c129ed542d7dd98e0bec1777b860d3f53625f8",
				MidState = "17ea4dac15e4d7136694c6a0e858a844ede42419f508451e4c7c41185486b99b",
				RestHeader = "17ec0b8ed360b877f82536f5",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 27,
				BlockHeader = "d057be65d057be65c8d51ffe03c6bc37aa8b8251900289d8d9125209ed826fd891bf54e93b6efecbeb61de030f6456c53e9c60369d8bec83a33596c274ef647c3f35a3624089a46fd057be65",
				MidState = "925c2f3ea2ceca9e430137e89516541a6a20826cf653136f73103894a5e5ee6e",
				RestHeader = "62a3353f6fa4894065be57d0",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 28,
				BlockHeader = "1e6c4bed1e6c4bed3174283afed42d53b7ed5b8874823c2a31ad1860603e7b14936e50e532d518b84b2c93c21b0def74c9996f59f1a218f7130f7a515c35c7d8f7035d2f1683d7a91e6c4bed",
				MidState = "eafd37d448e9139c4793c388ffea50098272caed162a1deabc54c14e0c7a7b29",
				RestHeader = "2f5d03f7a9d78316ed4b6c1e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 29,
				BlockHeader = "c4ae2a0fc4ae2a0f23be45c5952ce90c6a2c1b13cb8d90c6afe667d9403ef9cc6bb83392da1e061adbd94e5f44a221ccfd864db7dcdda819af5b668f6337fb2c9e29a00c7f068692c4ae2a0f",
				MidState = "0e5035073f3eafabd7d1fd20193d686193be8182b0cd56e74cfefeb9f54890b3",
				RestHeader = "0ca0299e9286067f0f2aaec4",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 30,
				BlockHeader = "76641ad076641ad0026f0f14512b2dbc1999b7930294a91c8ad91da4c486e6a7e6d21f4dc8e297cfaaad0f69e14563935d230d4c6769a0cad6e3eaadc0d4c011e4965237b035618676641ad0",
				MidState = "51806b07161901ab4a546be76fdf326c496221246e2173a6d64b975986ed634a",
				RestHeader = "375296e4866135b0d01a6476",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 31,
				BlockHeader = "30d91e2030d91e2086be7a76a9dd4d1393f1ab521d6fe764c887ab02a322854ef7e167981f243e502e8f77bccc83e1a9fdeb8fb67f9e55e223e58776899d2fa2d95e4f515d3980b530d91e20",
				MidState = "423c4973d246daca62165089175a0dc440775f8e74b01ae116961972de246a2a",
				RestHeader = "514f5ed9b580395d201ed930",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 32,
				BlockHeader = "7dedaba77dedaba7ef5e83b2a4ebbe2f9f54848900ee9ab62022715916de9289f98f6494345dc9cd88f6848e0b1d5d3e7e290e1b3e6be7b620ac04ac5b3f68e697b2b0148b28dd427dedaba7",
				MidState = "b7edc8aca8ab382542370b462d3663b593333fb84b428d4f420d681080ccd2e5",
				RestHeader = "14b0b29742dd288ba7abed7d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 33,
				BlockHeader = "9ba180039ba18003693a090aa6711e6233b942de77d044af5763ebf530434dc4c494aa383bb182eebf128d28b6e46efea6af6e0cb925553a052f1a9a8d6d2f2246c65132f5c3d3119ba18003",
				MidState = "533f5109f12e78306b132f5ebcc58d80a84943ceec80d0cd3b5b02baec7c2adb",
				RestHeader = "3251c64611d3c3f50380a19b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 34,
				BlockHeader = "36ca9a1236ca9a123b791b829d8d009b4c7df44c3fcfab52079978a217bb663cc9f0a32ff56634573298b7df5bcf88a5488d5c6c6f121fa718a2971e1039409e054332217dcf691c36ca9a12",
				MidState = "f1a658ae690de8836c541fa8130a90543077083e9bf60fe4b836cd2376a2df9e",
				RestHeader = "213243051c69cf7d129aca36",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 35,
				BlockHeader = "c61a635fc61a635ffd2997f113fc7ef68b2b11681990e568c24d616edde812856b999e3c0f10f3664c6e7ee3bebd1c736e663503437065904efb29eeec14a57bb8b5f561dae61c40c61a635f",
				MidState = "9fa8875d39bf10123dc5979ba1648dcbedee65512f7c622da9253a9959768f0d",
				RestHeader = "61f5b5b8401ce6da5f631ac6",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 36,
				BlockHeader = "62427e6e62427e6ecf68a9680918602fa3f0c3d6e08f4c0b7284ee1bc4602afd6ff698332f8b6cd39974b7a715f07ed2e5c560f01fee2ca1e1b9fe2356a6498f6d501733a3dc9c9c62427e6e",
				MidState = "a049a33e97c1cd89cb7a3397c002174dda2b4821155825a415ab37e921df9cb0",
				RestHeader = "3317506d9c9cdca36e7e4262",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 37,
				BlockHeader = "a9fece44a9fece44380296f6ae16ae67e8044c15a43ed0e5a106ac789b2f75cf3497ef63a32f9df80a502b7a1c73fae9845f226c8bc50169a9209932e4028f8a82629006d115be42a9fece44",
				MidState = "01e3fedce7d7a79f46bcce02f505b0ad4138924d881bde683588b5987a3e10d6",
				RestHeader = "0690628242be15d144cefea9",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 38,
				BlockHeader = "00ead61500ead615688564247c3b93cd803c80d25cf2480b7c47da03b489e8bd11ad65b33776a6683c653dc3e537cebdeb0286338c3f41f162796ee84c302e84fe5e9c38dcca457800ead615",
				MidState = "0dc94f87556c1076226a5bf115fc0c3e56731d45402b3e3f81f6fcf1b82b613e",
				RestHeader = "389c5efe7845cadc15d6ea00",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 39,
				BlockHeader = "9c13f1249c13f1243ac4779c725775069901324023f1afae2c7d67b09b01013515095faaa7a2c097b8553b89d77cfb6007b5ff18ea8e75584e652f6def1d82cf36857950687389eb9c13f124",
				MidState = "43c223d04035238fa7fad6f93dc5633f6bde2e94e4e96f7535f985fa2a01cb49",
				RestHeader = "50798536eb89736824f1139c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 40,
				BlockHeader = "ea287eacea287eaca36480d86d65e622a5630b77077163ff84192e070ebd0d7117b75ca5540dd27ad636326cc8ba7f1e9a0b37ee24864c5cce21e174cc2e77e702c0e1693dd4b082ea287eac",
				MidState = "0950578a986c998f9210bf059b6f2db1faa888f9f774859061756c1327a07201",
				RestHeader = "69e1c00282b0d43dac7e28ea",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 41,
				BlockHeader = "2deb4c7e2deb4c7e338383c43a1e4d92a57fb993a219aaec7821e8f3f2d12a5fe3a18192a9c44323308700f1b4ef95cd2d4b2d8775ed59f189a8f3cc62a1a612c042482409fdb5bc2deb4c7e",
				MidState = "30e7a8183667417623bd98274e5ea8bd3e32a64211fe1f6cdfbc47ae87be50d2",
				RestHeader = "244842c0bcb5fd097e4ceb2d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 42,
				BlockHeader = "7b00d9057b00d9059c228dff362bbeaeb1e192ca86995e3dd0bdaf4a668d369be54f7e8ee7b851224cabc08707554c3ff0ef9c1178ba268baed3de0a4b43fe7358497270ac1906a47b00d905",
				MidState = "a940422cfc81422252905461403aeb000819e026cc2c5fee047bc6c465608690",
				RestHeader = "70724958a40619ac05d9007b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 43,
				BlockHeader = "56217273562172733c049d522ba236b62d230be1a2a65b7d502866598991d186a764036e64c9ea00f6981013ea77d94b09c5213052fb0525a0985c868799a4107894521f822d64c856217273",
				MidState = "362c87dfc01fff1284d74e8bbfca6e0c9e557a4dac6742fe89a276b1e9eadc8c",
				RestHeader = "1f529478c8642d8273722156",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 44,
				BlockHeader = "a435fffaa435fffaa5a3a78e26afa7d33985e41886260ecfa8c32caffc4ddec2aa12006aea5d170cee84d5611cff770eaa57990f435cdf55a2a1df97bb0697d5298ee600a46db0091629f314",
				MidState = "191331691bfc70c1e988be695e9fed43a0708d12b7a5201dc4cd847b170bee85",
				RestHeader = "00e68e2909b06da414f32916",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 45,
				BlockHeader = "6bd80b376bd80b37c6d92df3ccf30d288cbc96de38582161fd3c0a61ec59da6af37473aa5e122752c4a198eb81bebb3c99187351910c2da5e96728e9e5eac3cb5a870f056571bfcf6bd80b37",
				MidState = "f8ff004b93dbbd46ed311b5d72b8477022dbd993c88c8b9df78f7bcbf72754d8",
				RestHeader = "050f875acfbf7165370bd86b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 46,
				BlockHeader = "3a915dd03a915dd06c586821d2017c9f7b20848b5ad0fc0259fa4ce2a3003951b61849bdfc10ede352edc698fc270192bf9d36790769f073f1179f2567bfc5725bee5746fb0e05613a915dd0",
				MidState = "07eeb1c572a8525fe8131e46ed9103fdaf57483024afc116fe30bc66ed5e1ce2",
				RestHeader = "4657ee5b61050efbd05d913a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 47,
				BlockHeader = "d6ba77dfd6ba77df3e977a99c91d5ed793e536f922cf63a50830d98f8a7851c9ba7542b46558d34c877923193c34a620b1cad0ef5a0d16776e82d34af2aed80db2acae1da5ec2662d6ba77df",
				MidState = "52b16569ab33241b038ef133bedae009c1b6899de6f320a57fb62c935b0182cc",
				RestHeader = "1daeacb26226eca5df77bad6",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 48,
				BlockHeader = "0962a4ed0962a4ed48f5fc7430ee7f29a429f97ea4de52ca84310bff32b9b0bd7beb1fa37ffe37085b1a7ea67e96e1f29dc2a0c3b6dd936a0d047a10f5e4695bd078ec23e70621a20962a4ed",
				MidState = "97f89c060135ec9eacab542a036c8a341fe00e08c691610019d531bc6bb8c841",
				RestHeader = "23ec78d0a22106e7eda46209",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 49,
				BlockHeader = "60bfb3f460bfb3f43d18cc94de4b5a78e4bc0fe54033259176244ed31203175b8d34e3e7a3514f81c46b89da422d9cb7a43fb284e97826d0f1bc1300570b595372794a7b3d5eb0a460bfb3f4",
				MidState = "e4ba280f903f81c015d768ab7212872220b420fe39f0cdcfdd14529f23961c30",
				RestHeader = "7b4a7972a4b05e3df4b3bf60",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 50,
				BlockHeader = "3ce04c623ce04c62ddfadde6d3c1d18060fe88fc5c4122d1f68f05e23507b346504967c8c5ba8f4da688f19bc8a64c36747b3e91aca0c6de1e9ff775f5f41e4e4dc6e83cb5038fc33ce04c62",
				MidState = "6bd8310d4dcbda9dc30aa67b7bf4ca837aae619917762baa5ede2b2d540ee468",
				RestHeader = "3ce8c64dc38f03b5624ce03c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 51,
				BlockHeader = "4afd5a8b4afd5a8b78f7e747d075adcd09e39a8aebb23f857ef6a1d76c373c0f933fd9da0de5a3e0e35431a91d65c703f7af5e614349110ddf689993767c496c2a0c8d6ece79942e4afd5a8b",
				MidState = "215a6aab9ddfb0588f138cf7b5e067e60a56fc4d288fd66dfe74514e2b706809",
				RestHeader = "6e8d0c2a2e9479ce8b5afd4a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 52,
				BlockHeader = "d761009fd761009f82132ce0dd6e3e40f1c153bc298bc0201df5eef5ccfca56e4c83baa4c5cd935989c286d8b7de29af4874c527fa70548b3e4deb74a318467539c0c979998abd52d761009f",
				MidState = "cba4d4fa0aa4c0900a2caeffcfcb1a658ff5bfa109c60d6827c6246c4924c72a",
				RestHeader = "79c9c03952bd8a999f0061d7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 53,
				BlockHeader = "fb666f7dfb666f7dffe8ca293be0d17a06868919d3f25e58d6869bec6cc2e794bf522a8079701b7616d8e5376631245f2f21294cfee3f74d433ddcfb0013e3991b2631422aa7f353fb666f7d",
				MidState = "5c8e61f5368289d2ce27811779aaf5a5fa6715d84dbaf17a55ff5ecce2d18d74",
				RestHeader = "4231261b53f3a72a7d6f66fb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 54,
				BlockHeader = "249c9572249c95720869e4b82b64ba9e8f2adb67d3800feaaf8c195102828fbb8415ac5cd629d80ca45e79f8308bfd9e1c90e46b85a9dcd923143f828983c5b58a22df3a9feadaf8249c9572",
				MidState = "673427cc8d3ad313ba6fdc3b381f8634a187ba965d1d5c8dbbc5c57670a82b04",
				RestHeader = "3adf228af8daea9f72959c24",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 55,
				BlockHeader = "91d8e5f391d8e5f34ce54fca7acd05fde65138179f988b56b2892d4769f130ef7ecf2fc9c005c1bfa63386bb5b0af42d8be0f2331075a4b8a42f71a4e089a67f9f5ab91c71da5f7891d8e5f3",
				MidState = "2983b5f557d5208de4619ffc5f7fc6b8c4adf71027b6bd6d74d73422903702d1",
				RestHeader = "1cb95a9f785fda71f3e5d891",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 56,
				BlockHeader = "7ef3386d7ef3386d2abca7f1750e1149d678769f0be77b68c78ae1a36f3802d1ff9d0f26550dd25b5c06284c8c46ebc708c70092744fd7d146e862173752454ca8cac2496b3752937ef3386d",
				MidState = "befb8f9caeb762dc15feda3595fac7da4ab26c2e5b39ed5a33f95d24e11763a7",
				RestHeader = "49c2caa89352376b6d38f37e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 57,
				BlockHeader = "cc08c5f5cc08c5f5935cb02c701b8265e2db4fd6ef662eb91f25a7fae2f40f0d014b0c21f3224ea21d02b8be174c12392766c09bb35c24448ab6e7a5885e9314cefba70ece9bd7b5cc08c5f5",
				MidState = "78bc9a8073a8b330da9f0a27adf21b626ca64c0475bc11c6a87f5c64e70b0d20",
				RestHeader = "0ea7fbceb5d79bcef5c508cc",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 58,
				BlockHeader = "6830e0046830e004659ac2a46737649efb9f0144b666955ccf5b34a7c96c278505a70618f4db9a6aab6a0f7ae2fc9769b8d1ac37859b69fc042df2d81e5c7f16d474b9060419141a6830e004",
				MidState = "8fd5fec2e2fcff45d8532450035595468bd351c07ef9160f10266447cfbc81ba",
				RestHeader = "06b974d41a14190404e03068",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 59,
				BlockHeader = "ea99b7e7ea99b7e795ec9ad7a9c8027c68bfaf1589e6f6cef48cbea96df1a95c7779398f5d1b74b2224bd2dff12770eb691c8a987fcd884adc286a1097de741f5cbc8b1b22532cf7ea99b7e7",
				MidState = "1b695fd98d56fa582632214dfc70730ab508aff7d18be0ebd38c61e1d2f91ed0",
				RestHeader = "1b8bbc5cf72c5322e7b799ea",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 60,
				BlockHeader = "96fccb4896fccb4811bdf1cdc5798e668d7e6fcc0084ef40cbf4f0f3f7cf3e018cfa4d683ff15bfbc18f3466d1649e0a79e775b794eaa6c774450dd7327daf5473013e2fe97f3b2a96fccb48",
				MidState = "8a78c4d5caa0f550c80a03972d57ca992b0a7267995062e93c238f6e341630bb",
				RestHeader = "2f3e01732a3b7fe948cbfc96",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 61,
				BlockHeader = "3225e5573225e557e3fc0445bc95709ea643213ac78456e37b2b7da1dd4757789056475ff9e126e179e9516c27d177986ff6775b8726bc45d2de43df9d237b48e96e102cc768bad63225e557",
				MidState = "282219f928541512b4084ba4cd931aeca63019498a44a2c28d6564cb009d4010",
				RestHeader = "2c106ee9d6ba68c757e52532",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 62,
				BlockHeader = "803972de803972de4c9c0d81b7a3e1bab3a5fa71ab030935d3c644f7510363b49204445b044eaa30726df9383bd8cea660abf7e501600a72ab3d4ea5774971741b8f9e7ba75ee7cc803972de",
				MidState = "06520775250b7571863991879155e7b5a1c3da67c9fc53a78d5667e63b75fe9c",
				RestHeader = "7b9e8f1bcce75ea7de723980",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 63,
				BlockHeader = "8edd41268edd4126eeb71a58f64a2b53f24913781378ff296303d3fe00c0fa68804f718dad8d1de260e2b83823c87a5c7504c54d1de1fa6886b1e57c66247766da2d4d18a46f06148edd4126",
				MidState = "03aacd34302abf2297d78c458b592d33c750523c3123e8e3a451bcdd539dcfd3",
				RestHeader = "184d2dda14066fa42641dd8e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 64,
				BlockHeader = "6af48a6c6af48a6ceee7e3c6abb46d4fd3b3f5f62b7954b02ea37a1467fa22d5acb3766504432d79889b1cf9f66470b83a3f21500b5ac200725754d3616506810f4f2272910d45406af48a6c",
				MidState = "25bf1c7daa49be82777dc2a27726b29d3fd00b9adbaac4631e5b7939648f6956",
				RestHeader = "72224f0f40450d916c8af46a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 65,
				BlockHeader = "8633a1178633a11713f6dd8edbbf3fa0f644a63a4b7cae62866ff1c58269f8089bf9bca568e11fb09d249b95b4b69d9278da4cace976c83d67cc72d6aef1cc259b9b4d19211a31e28633a117",
				MidState = "2244412b733bc9aaea1aa06c9f00a9ab53aae983de7fa3a030534a1e168ce2c7",
				RestHeader = "194d9b9be2311a2117a13386",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 66,
				BlockHeader = "463b22b9463b22b94553dfb4dc65a9d193c7df91f66d18c5b53ac76345dd7695dc4132bb332c50cf51dd8a19a446c68619c0fe14bd347f3550ae5c96fd28c5dcdd44db450f540bff463b22b9",
				MidState = "4f76498ba4869f5898f58f2587eacb788500d947385b11a33ff887f3e1603b76",
				RestHeader = "45db44ddff0b540fb9223b46",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 67,
				BlockHeader = "19b8b4c319b8b4c319f771820319d43ae663569cd94e78524619bf916e4f9246a826ce4b6d8d39db1a6ac52e1d92b413c10c55d8a679368a6862a350cbc435afd7025442a49f109019b8b4c3",
				MidState = "f9ca703f100eac3397ce77d1c5b96ac7d7783c3e9a50046631e7bbf899f3a7d1",
				RestHeader = "425402d790109fa4c3b4b819",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 68,
				BlockHeader = "7221b77c7221b77cec450aad1fbfe6fbc884855579e28cafca0a80f3a9b77e28892355c6c9dab4ed23fad292c7c1783e542901bf1a8d43e07273d4a15dda382ceaf1470b083b84f57221b77c",
				MidState = "27ead0ef5dbed8a2a0bb2a926eed20e23270487ab459cffbcfd89ce253452bbb",
				RestHeader = "0b47f1eaf5843b087cb72172",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 69,
				BlockHeader = "597e68e5597e68e5bf19cc86abc3b6cece28bc64bd20d03e11a054b5dbe4ff288c31f55c7c8fdce6a1f5f99615fe1dd971499b8a9f1c3a6d6fc61b88b9d210b66df3b0368b0c0283597e68e5",
				MidState = "55ba20e51bda48120bc410fc5beb9bac515c4decd67a8e79d9b7237327a7291e",
				RestHeader = "36b0f36d83020c8be5687e59",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 70,
				BlockHeader = "04df3cb504df3cb5ad8269a5195790b6590be36992d3211ce8645d1db9e1289bd8c62fd5e05d0524162fb552ab14f1b813cf52d758937b266dd0b290c5b7cf2983ab65406e822b6004df3cb5",
				MidState = "206b6b5dc844842b8a440808e9182e93edd35f8911bb46491ae7f46c87481db3",
				RestHeader = "4065ab83602b826eb53cdf04",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 71,
				BlockHeader = "13903e0213903e0272f4571a6d7791d22c5ba0763ec2037ba2172490d83080564ee2b60e02b3b31666992cd3ae68c97340b5ff68d8c95460c3e238a6b7853080cd2f0252005d5e4113903e02",
				MidState = "f48a820ba0eebf7b91e14efdc2d1f8140db0f483cde812bbeddf98d9afce28f0",
				RestHeader = "52022fcd415e5d00023e9013",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 72,
				BlockHeader = "508b72c3508b72c34a675c1ec867b39a31f666b8c122db3eb41f5811c2e45f06d67303b44947f7bfa77f7f338a208eec3bbfdbf3d697e2929135f70840c0464b7c9cff08fe2d7ed6508b72c3",
				MidState = "275a388ab00546fc33c2c5fc4ef20e01f0064344cc54d47b2d7f08f0540ef6ab",
				RestHeader = "08ff9c7cd67e2dfec3728b50",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 73,
				BlockHeader = "ecb48cd2ecb48cd21ca56f95be8395d24aba1826892142e06455e5bea85c787ddad0fdab3203a56bc3363e51f40719d9f70ffe596e3f8972b9329e91e525a90b48ae4455a434f2c3ecb48cd2",
				MidState = "e9f1d42485761cc21b145a5cf50fadd65b09115b3fc2a5e5743ca843a63cfb1a",
				RestHeader = "5544ae48c3f234a4d28cb4ec",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 74,
				BlockHeader = "39c8195a39c8195a854578d1b99106ee561df15d6ca1f532bcf1ab151c1884b9dc7efaa64ac5c573c3b43dc4fae184cf15ce542d323f505ce0b8d8c821ff223d63dc0c7759ac467039c8195a",
				MidState = "85721e04b20a7f88473901be395abcac56d65ee5d1482f9f99259fb0aeeb5f14",
				RestHeader = "770cdc637046ac595a19c839",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 75,
				BlockHeader = "5836b0f85836b0f80dc3f708e43ec1f29fe462d0586e8e4e665e9a188a8979f81d3736cc3e75d7b3ab20d73e62177f08224ff2c0ca645f9f151607a3361a3496dfee2e233668db305836b0f8",
				MidState = "be55bb2650d974194f9f9cf9c9d4009365d63b98607ef36479da077a66a6d021",
				RestHeader = "232eeedf30db6836f8b03658",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 76,
				BlockHeader = "55927354559273545586e70a16214b5e09da949034c5009e354720db389175e5ed064081e8baa252d570e098eb5f3983e4dd0d18bbdf75dfe0bc90129552a1fb0955735f636cc4d155927354",
				MidState = "3996e9546a9520352ab82f48733428087a4c2265529b7d5c0dfecb39f91a421c",
				RestHeader = "5f735509d1c46c6354739255",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 77,
				BlockHeader = "e9508a27e9508a270187bdb17aee60ecdf54ccaed7ded11f4442ea8d4d5e53890425f07b4ed4dc914ebb78f82166d3c5f5589010142a65d916abc3ad2da7470d76c3f167e73f0be1e9508a27",
				MidState = "fbbd80f51fad613b6ffa57aafc766000bd5ede00249ddc453733dd463d7d0349",
				RestHeader = "67f1c376e10b3fe7278a50e9",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 78,
				BlockHeader = "422017b9422017b93f1d7dec6971a9c02d4bb5a14c3778d54d92fa31a4910960616d09a272c654eb41c100df02cfafedb1a72c769c50badd4c5c95c6c3cc521e42ca5d4fb1fe23c5422017b9",
				MidState = "917c104b8c17aa96f08b6654c102cb4ea01bd805991544e12df6f99a9cc63bd9",
				RestHeader = "4f5dca42c523feb1b9172042",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 79,
				BlockHeader = "4f07a3294f07a329226b5bd81c313ad992003ce6f62e623007871347d3c68d40222b656bdda7912628dcebfbf9d73da3ec9cd901a3d364038563fa1d16a245567d6c2f54165915104f07a329",
				MidState = "6f04f62444d4adf5f6b876dc4e3596047637f8436b46aa0f5fd143be616125d2",
				RestHeader = "542f6c7d1015591629a3074f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 80,
				BlockHeader = "9d1c30b19d1c30b18b0a6514173fabf59e62151dd9ae16825f22d99e4682997c24da6267dc20c1634635733ca6cfa7432954c31cbf8ec04fca6b864477e9e60011bf32767199d5e59d1c30b1",
				MidState = "4b6c3803ad9ce2a47b0f412d18793cbb290f444ae46a19360ae777bbfe9ff6a1",
				RestHeader = "7632bf11e5d59971b1301c9d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 81,
				BlockHeader = "eb30bd39eb30bd39f4aa6e50124d1c11aac5ee54bd2ec9d3b7bda0f5b93ea6b826885e624f208a59499cf25b46f253bcd935fc86cab5602f29f5c67608789a377e34715f1ed457fdeb30bd39",
				MidState = "e64d566e26dc09edf910c4cd064a75097483c81ec82c79da343479aaa4122c5d",
				RestHeader = "5f71347efd57d41e39bd30eb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 82,
				BlockHeader = "38454ac038454ac05d49778c0e5b8d2db727c78ba1ae7d250f58664b2dfab2f428365b5e0fe97d91d20dba6982563acc001bc457b6fbc4a8b514345b00058fd4312e7a0e2e50054c38454ac0",
				MidState = "6be4bb6015d9c67a9e727c2f39b2e9e97fd2b6d60641f8e63e5795350d94e479",
				RestHeader = "0e7a2e314c05502ec04a4538",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 83,
				BlockHeader = "a64f66eba64f66eb33875707068c63e246fa637372a979037edff9d0933ed7e006a527289197097ff12f245869671fc397ed415a66bf1712128128afe6fcaa7c6b97f221500acabca64f66eb",
				MidState = "d053c4806988f7eb009dbcfad4353dcd39bc727129fe086bfdc0fdaf4c5984f6",
				RestHeader = "21f2976bbcca0a50eb664fa6",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 84,
				BlockHeader = "3f165b333f165b339a7e4cb27c56459090ce4324b8ffaa40b3b097bf9f4ff61c840e59cd2ecc974bcadfdda3da485d3f6b86a0bc2ec3684f5d7a5b315746e6e085d50e7c415d4def3f165b33",
				MidState = "015f84f3bcc497589985d53243e013cdc1643cd46ee8f39d49bfd50c7d011040",
				RestHeader = "7c0ed585ef4d5d41335b163f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 85,
				BlockHeader = "8d2ae9ba8d2ae9ba031d55ee7864b6ac9d301c5b9c7f5e910b4b5e15120b025886bd55c979ef933bbe2ecdc798933fdeede1ad5dc9a6c8712ef3bdabc960a037045c382e6e47e5b78d2ae9ba",
				MidState = "c75c16953b98937e836f7a89f705ce74ea5a24c6c543c6a0ae5ad0594ff76ad9",
				RestHeader = "2e385c04b7e5476ebae92a8d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 86,
				BlockHeader = "9ab6ef489ab6ef48a8b17c3910b264590b70dec4c51fcbaf6f0c33c9b33c626160e690891571a4eb804020419c474db355a70305c669ff39df2b5bf71e61e1efb8077a6b620cd04c9ab6ef48",
				MidState = "4a96769f3a6f9aff36899c2d2ba38635f049f7fb0d0abba80f3fcee2bbc3bfff",
				RestHeader = "6b7a07b84cd00c6248efb69a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 87,
				BlockHeader = "76d788b576d788b547928d8c0528dc6187b257dbe12dc8eff077ead8d63ffd4d23fb146a4cc5d98ee4c570488e54a92b97ba34bc4e13c5be00c00fbb31acc1958d932a7e5105168c76d788b5",
				MidState = "70f7a892127c21ec46b8f80f5a73b1a736b54dba2cddca9215a3c0364bac7b0c",
				RestHeader = "7e2a938d8c160551b588d776",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 88,
				BlockHeader = "e5da934ae5da934a0d87defa6c6a2d0c168af6dc5007c5a8aad795442b9b1bec32a53b8dc4074680303c2aa59c3baafb45310282d46f538a2af9573d2f795ef74bdf3a4231845020e5da934a",
				MidState = "08e758424f259a5da087605c5fa229c846641caf0b4f282aef8e1c67e5eb3d88",
				RestHeader = "423adf4b205084314a93dae5",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 89,
				BlockHeader = "33ee21d233ee21d27626e73667789e2823edcf13348778f902725b9b9e57282835533888956c2880c577a2531c0dea10a2e5a601a8664c33d03f323605033b6e933df4583a549f1e33ee21d2",
				MidState = "d0f76385c2390138dd8c49977ffbedc7b9357c5346ac684ce01b067a40ca0985",
				RestHeader = "58f43d931e9f543ad221ee33",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 90,
				BlockHeader = "e8bda7ede8bda7edb4d087c243f097ea6c5f5ddf89ce7ffd0f6448db7f1a578a4d44b027fcd51afde9685d3c379373b4c87b084d21c1a4cf8515dc4229c8255a910be854b16d4e1fe8bda7ed",
				MidState = "c84aa130ea62fcb9d1e05ec758136b92133ce588e0127e021aab618430546c00",
				RestHeader = "54e80b911f4e6db1eda7bde8",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 91,
				BlockHeader = "d2cb9118d2cb91188336e6a62207598cf8ceb9700b8366764ce0e12d6647861b17a3c88796d8401f5d05c3a0903ac23ab5108d198c153f5bd9825e413083ccf3bab7110944627ae4d2cb9118",
				MidState = "f43fb7ff57607ea2956093d79e8d7f02e5d13d5701adfd0069d445e70dfcce0a",
				RestHeader = "0911b7bae47a62441891cbd2",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 92,
				BlockHeader = "139b2f67139b2f671af8c43e8b9f22c80e92cecb951976d2f0a440979c34bfe9224604d3bc51966f9451ff9831bc08172b531e5f42f9b332de06440737240193d8ad7762c18e654c139b2f67",
				MidState = "da95561fc8ac496b75c827cf235f8829bb37583e24df4b2fe60c237d7c506425",
				RestHeader = "6277add84c658ec1672f9b13",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 93,
				BlockHeader = "287156c7287156c76b7e8caeec63bd02b09bad6e0640972d92c5765a7fa939db94d66e597d4515a23fd9f0fc7ef42f3c1a6befb8b508c6de542a3e02f67cf246221b8c5d8cf03586287156c7",
				MidState = "8195f648c7150a2567b416d08c06b9c01f78bcdc2cbbe493d7a416f9f7e24b13",
				RestHeader = "5d8c1b228635f08cc7567128",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 94,
				BlockHeader = "7685e34e7685e34ed41d95eae7702e1fbdfe86a5e9bf4b7eea603db1f265451796856b559a4c5869e99d341478a1c926998a40e3121896a26c304f17872b471d42627e67d5b402407685e34e",
				MidState = "bd87ccd12901ec70b6fbf7171fcc525773fcae2be2361b0ef59f2379ae0b24c6",
				RestHeader = "677e62424002b4d54ee38576",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 95,
				BlockHeader = "c49970d6c49970d63dbd9e25e37e9f3bc9605fdccd3ffed042fc030766215153983368506cb084ad46f0bbb96f8dcd26ae3fb216d22ae0921594f4fd03ce660ae08b6f1eb6d11390c49970d6",
				MidState = "b7b1f1260962a6bfb6a1a3a27dd34c7f78860f31ceff8d6cb004b85d7263b4cf",
				RestHeader = "1e6f8be09013d1b6d67099c4",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 96,
				BlockHeader = "12aefd5d12aefd5da65ca861de8c1057d6c23813b0bfb1219a97ca5ed9dd5e8f9ae1654c4b501e631aed218e0f51efa31d01a644a4b79360683cfaa750debf665b61ef7847d28d7312aefd5d",
				MidState = "f2909e1347c7a56ccf53c8b3ca0d5242e02aa2c90b87f38fa293b056ebdd717e",
				RestHeader = "78ef615b738dd2475dfdae12",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 97,
				BlockHeader = "67b62bc267b62bc21d057dcf485c1ee4280b2a2a4cce562b36fec0fe487b653ad4a25eddad4153b3282e72b00e524793171d278cc5217d8fdac523a830521a00040888237fdbbe4267b62bc2",
				MidState = "0679a4e2ab1ed8e09c8f34516e3ca491221ed01b6a7b100512fe5f25b1b0457d",
				RestHeader = "2388080442bedb7fc22bb667",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 98,
				BlockHeader = "42d7c43042d7c430bde78e223dd296eca44ca34168dc536cb669770d6c7e002597b7e3bee8826e42cb3d4ea340863f59b484a658b46f61cb31aad7578f71c8be644ebe4625625b6142d7c430",
				MidState = "a63ea0257e9e134b8128420698bc19b6a84929702c1500bb3347522bdfe7f7cb",
				RestHeader = "46be4e64615b622530c4d742",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 99,
				BlockHeader = "3cf79afd3cf79afdc34b60e2c2a6193d16e36bcaa2d7faa9d9934430dd42d49f66bf5a452d5b13ff6cc6edae4e8b2c567221098698c64b7f187d091b6662b91f32a1a65b4bc1f6813cf79afd",
				MidState = "4a3300b3804336840bf30b159e36fcaad4b0d68f325cfeda8112c441d4b4710d",
				RestHeader = "5ba6a13281f6c14bfd9af73c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 100,
				BlockHeader = "7fa72f7a7fa72f7ac21898558f6464fa4c2ad4bb93aee16b3e9323dd3b35b2217360ecee1be657cb30830a1ce36f150da7358bc0ac1703145bcbb0093f6ec405a8db135e7f0f9d5c7fa72f7a",
				MidState = "4346d88f775b24bb7f55eacf42c37e0e780e7facb6319ec89289d9be16533316",
				RestHeader = "5e13dba85c9d0f7f7a2fa77f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 101,
				BlockHeader = "8dc43da38dc43da35d15a3b68c174046f50fe6482120fe1fc6f9bfd272653beab7565e007e9f385ed2a0b4a6301bc028947d5c913dfe493d7e9e1ab9f9b3ffb235019325b59fcebf8dc43da3",
				MidState = "5664b357ba88989b22f2edb8727de47ca6c2abef5e2a0f262857f21ec4083455",
				RestHeader = "25930135bfce9fb5a33dc48d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 102,
				BlockHeader = "68e5d61168e5d611fdf6b408818eb84e71505f5f3e2efb5f466476e19569d7d57a6ae3e026c57ce822cd3f2a15bd322bb47ad396446eee7376839dceb1b4392a73fd506b01f98f1368e5d611",
				MidState = "ba13f3d7224e54d95be91ece906926b5bad7d127d895dd95e73beca917c2621c",
				RestHeader = "6b50fd73138ff90111d6e568",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 103,
				BlockHeader = "4d3792c84d3792c8f39fadd6a04a51b1d4677d1d7e2cff57fbfccd156b78d292afc4bf40db3d3ac34ca54ca2bb9de5ab437c7f7627d5a4b0ab9505dc95941dc2327695288dcd20bb4d3792c8",
				MidState = "dbcf0adea5e219076ef2e7ea6b3671dd63f510f1f2f9a5c02103e01917be8458",
				RestHeader = "28957632bb20cd8dc892374d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 104,
				BlockHeader = "c7501b73c7501b73e40b8a80e2fd851215a95398c4986bd251ea43a70790c40301053670855c6f30f4da4c120196ab98a1b431c53b6f0c7ba714a2a3c76ce076fbcbe4592df702afc7501b73",
				MidState = "54c659377cea01fb41c4188fe5ca344e630cb0bc7187590675a1c53cd9cd4dc6",
				RestHeader = "59e4cbfbaf02f72d731b50c7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 105,
				BlockHeader = "7741862c7741862ce422e3d387e725adb3b2dbbc9d4c254abb5dd795b606e64f8700aadcc011217d064b77449c4d0af983b3fa0bb79fc79d11c371902e29e8b4b129ec4b493f1f9b7741862c",
				MidState = "e04555a5736ebff2034217ca99fc354340f2f957dfdebf5f2784f0cb46b5b4d3",
				RestHeader = "4bec29b19b1f3f492c864177",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 106,
				BlockHeader = "3f4863263f4863261444ca2ef166d1bb29c421e7e7eadac39a3fdf5f27b8133457c3e8e3012cead409218b34ac6c0b547e415d1fcb8d0822801c791b54d7353c0db3010169bbb5193f486326",
				MidState = "0ebee73d1afe772e69b1ed226a12b2799a612acc5c788d2fff4c98ca1f7ca2b4",
				RestHeader = "0101b30d19b5bb692663483f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 107,
				BlockHeader = "b03ae8c2b03ae8c24268b750ca862aa1d87d2e42f4991865dc20f7acf136259b802845350060d7429fb6e3713216952e093a748d44fc5aa6c72e52401a0a1ac7cfbec4024e0b0094b03ae8c2",
				MidState = "183a6faba02de14d9664386d0dfabab99e267bd5164cbce4a5fee44350b4c4c6",
				RestHeader = "02c4becf94000b4ec2e83ab0",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 108,
				BlockHeader = "3cfe9d953cfe9d95dfb98cc9fb1d1a0aa72fe5b4fa26be1b8504cdf906d4158970268f5a133518e993c62830ca05b95ad18706562d91bf058fba6a287633f514652458365fe9512b3cfe9d95",
				MidState = "4067cb74e32ba411476219132777e8424580f0a5ecee2629da06334578294983",
				RestHeader = "365824652b51e95f959dfe3c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 109,
				BlockHeader = "fc061e37fc061e3711168deefcc3843a43b21e0ba517287db4cfa398c9489215b16e0471b23809e0fc242edcd40906cef2763cc9626e961fb5a3cb4e2dd822f3e1bb4f1d58ba1559fc061e37",
				MidState = "c2950c89639bb393c3fdbaee2116d86e238157c6021a229e0567672e5ff76ea9",
				RestHeader = "1d4fbbe15915ba58371e06fc",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 110,
				BlockHeader = "982f3946982f3946e355a066f3df66735c76d0796c178f2064053045b0c0ab8db5cbfe68648c28d94d0b505baff3fe62446da683fe5ede25d7e8f04322a261480108bd63f89dc36a982f3946",
				MidState = "79bd179c1c7cb6a9f6e74ce401d4d89ff9609815caf00c2af8ac1a2b74125087",
				RestHeader = "63bd08016ac39df846392f98",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 111,
				BlockHeader = "32d9ab7632d9ab766f26a4b95691d522d669c7433573ed0b5c89efc1cfeb1212b5d0e3bedd5218d94c845a4714762f39a56348d6f275e218d320d9bb8c7244dfd76fc02fde774dab32d9ab76",
				MidState = "0b5bffcf7add842bfb8d36e0bc436b42a9296798c84b09d21d3fa2e2e2c78613",
				RestHeader = "2fc06fd7ab4d77de76abd932",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 112,
				BlockHeader = "0baf49460baf4946379d024754fb3542aac7d533204cae3580d61fd1b9f1fc777079e5b125f20a056377b1ea9fbf7e27f85d2c8be65e3ac6d5b51f669537c032302aea74a6dac6620baf4946",
				MidState = "674ea93c18f682cc8fb94079f64e131d8decdbd2a7f71abda4af3f5d38101bec",
				RestHeader = "74ea2a3062c6daa64649af0b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 113,
				BlockHeader = "c81385abc81385abac18f8f6a220afa382778e3d0eb53a97b981f268c92bca29905b18c174591ea0a2b839f09370d29545a891c49b6c5c2b369cb53cb4bc79600b466721a03bc788c81385ab",
				MidState = "c8238ceeba7f972fb34642f04240f6804002a38040419880387876f1d3a2445e",
				RestHeader = "2167460b88c73ba0ab8513c8",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 114,
				BlockHeader = "162813321628133215b801329d2e20bf8eda6774f135eee9111cb8bf3ce7d765920915bc56687a1bade1f17e00e08cc0d11cdffdedc9a537050d8c1506b8a31135139f0bb2a3756316281332",
				MidState = "6ff8237dcd02d18952e75958868f06ce9de4b6e0de6e0c7763cc1af52d46d12f",
				RestHeader = "0b9f13356375a3b232132816",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 115,
				BlockHeader = "b1512d41b1512d41e7f714aa944a02f8a79e19e2b935558cc152456c235fefdd96660fb32ed4d877c59d94c5ad406b9ef937354eaa43e19ca9f5cb45b876aa7f2dd6a20ef807c2eeb1512d41",
				MidState = "9eb6bdba3da67309963a19d319e276ee56527afe9e485c0caecdc6eeb23f8f47",
				RestHeader = "0ea2d62deec207f8412d51b1",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 116,
				BlockHeader = "822ed32a822ed32ae57b5ac86007fe61bfd0050d8f612cc60d4f6d4b3f6b52426a8cfe9b7c7f6c32403475dace4d6b113cfda3c0660caf9267fe56c40c38095fbe98fc26f33efe66822ed32a",
				MidState = "598c8eb4d3a2e04782e39282ffd3e20f4b9dba4b06746b5d66caa572da7bc6dc",
				RestHeader = "26fc98be66fe3ef32ad32e82",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 117,
				BlockHeader = "30a09a2530a09a2535d1c769f3f09d7e9ec9486d940a2f1a7149d889645682bdbeb647636d32d389b0581cba0727978de8bc55e88019b0d166b050911e219b25ac069020f172ecd230a09a25",
				MidState = "4d5f3b608b62b0c81c8f17e8475395054630631946d832f3ce90ca727cd29017",
				RestHeader = "209006acd2ec72f1259aa030",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 118,
				BlockHeader = "cbc9b434cbc9b4340710dae1ea0c7fb6b78dfadb5b0a96bd217f65364acd9b35c212405acbd4628955bcfb558951d9b27a6891eeb6689e0188486b409374c4b16398597888b51f41cbc9b434",
				MidState = "161e99aff0cfd3643675b604a00deb52ae2b3851b9e78ad915c5fe078126415e",
				RestHeader = "78599863411fb58834b4c9cb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 119,
				BlockHeader = "2e370e9f2e370e9fc7373d46a62fed0d4304c2919c6a157c9d85978be182fa1198e6f3e1dd5db1071d127680505a6e790545f473df06a75b9148f180bc9dc36cab161c37dbc109ca2e370e9f",
				MidState = "f3928a494e7e3238c7dbb324afe0a2a34d16285819a8c274aa780db841903442",
				RestHeader = "371c16abca09c1db9f0e372e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 120,
				BlockHeader = "7c4c9b267c4c9b2630d74682a13d5e2a50669bc87fe9c9cdf5215de2543e064d9a94f0ddb5def26620c2ae19eb4f1e9850143d9428afd526752a356c1d81f20223eb0c4f8b7d5ea47c4c9b26",
				MidState = "d0f9cfb26edfb44d87ac7a13b8e2a0fd2464f0a5edd101e6b86e625cb151d504",
				RestHeader = "4f0ceb23a45e7d8b269b4c7c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 121,
				BlockHeader = "f5954637f59546373bcca64b752172a19739885d4b67580df3f2d3d956d09574999bb06d716c22cfad0e154a05837012388298006674e619c216faeb2970f407e715e002e85a94d6f5954637",
				MidState = "b0a08f3e3d94e6db86dfffc108a1426dc5ec2d1e95c4014b8479983241ac4409",
				RestHeader = "02e015e7d6945ae8374695f5",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 122,
				BlockHeader = "43a9d3be43a9d3bea46bb087712fe3bda49b61942fe70b5e4b8d9a30c98ca2af9b49ad69ae52457125977c80fb17e8cc6b373a408e0c660accdde123ff7d3fe858858922feb7259943a9d3be",
				MidState = "09c9c66899258bdc97f862af9318c710e281a0bf25bbf97f551d258e56646628",
				RestHeader = "228985589925b7febed3a943",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 123,
				BlockHeader = "26af82e726af82e770f5ec18d4d8cfa59d0c6b7dc9a77676b875e857ccec0d5357af98981134cec5d6d41c8a7cbc17a41f11c6f037f131611b3344589897d7e66068fa0593bf2b3526af82e7",
				MidState = "ebfda087322228907b5b2c56e6072e666ff3f3eb18464905808fe3b5fff71f10",
				RestHeader = "05fa6860352bbf93e782af26",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 124,
				BlockHeader = "1618f0c11618f0c134f4e682cec9acf5d8d9efa925a70a9e507e804b02d18fbdf070ee6bc16f9a166ccacad5256ba8079374e859883451f44f16b0dcd759403fc4c3b616f06e2b071618f0c1",
				MidState = "e8f17811edd60e99df6ce3d7a687aa1f488197949754bf2dec4a558fe9a84973",
				RestHeader = "16b6c3c4072b6ef0c1f01816",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 125,
				BlockHeader = "f239892ef239892ed4d5f7d4c33f24fd541b68c041b507ded0e9375a25d52aa8b385734b3938f0d3746909d489aff08c6e94381c7b02fc4061ab4d002b246ee4ef37d90e544def88f239892e",
				MidState = "eac35ddc482a110c32822b1c810bf81dd1891283f213fb42e8cf50144a3c00b9",
				RestHeader = "0ed937ef88ef4d542e8939f2",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 126,
				BlockHeader = "a47212a1a47212a1fb806ed69bf2a625ff93e24e1b03c111f3ce47d58cb2d2c1f23426c769e252bc930414e869467f4bab72e2cf1bd8d1169023838ec44bb92850690a779c0ae35ea47212a1",
				MidState = "839a9fb0a013c0f9bb9c3c78102728ef4276c58f2bd4441e113d30fe7c189ffc",
				RestHeader = "770a69505ee30a9ca11272a4",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 127,
				BlockHeader = "94187c4e94187c4e50432be68d16a6c60494aebe6b8df6d1d3c227b931859d7a52bbbb33cc8d758efaea5a6bc42f2bfc781919cf0b7c27b3395b94c5a36b316aa7e329418e8f1f3c94187c4e",
				MidState = "472d4af5686664c6a119865eedb52cca2904b7a73e8faefc7d62899c566dca4c",
				RestHeader = "4129e3a73c1f8f8e4e7c1894",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 128,
				BlockHeader = "cecdd433cecdd4335f096ca3fa9f69c1040ee4577b36f97daf9530d2d8dcc8a6e06777732b06b78af9121213d3f1a7b07735d323f05856de34f7218a8eb3644b5f0bf478b09de1efcecdd433",
				MidState = "b79f982b503d7737da8e6cfffa41574223615447ce2bbb2a1828b59f98f3f1df",
				RestHeader = "78f40b5fefe19db033d4cdce",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 129,
				BlockHeader = "1ce162bb1ce162bbc8a876dff5addadd1170bd8e5eb5acce0730f7294c98d4e2e215746fff526b865b4b769d90fc3369fc1bb2645cbfaead95fa5fc9bad9df72cd95397deb5005c91ce162bb",
				MidState = "29eba61bb9645a00f6d69856c3442550b8f8cebc46cca5f4e77df6af5cba63f8",
				RestHeader = "7d3995cdc90550ebbb62e11c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 130,
				BlockHeader = "0202b82f0202b82f44e092263dd6106965a863708bc0615d92a8fcd24624609facda58c4a0965104d7c407450125c68687e79a3d2d6039bbbe64849b098689225d2f8260d2774c720202b82f",
				MidState = "7b0d856885c5ab7b480bd2d941a7b7a8f31740a2a63d0a0d669ebd9ee8454f49",
				RestHeader = "60822f5d724c77d22fb80202",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 131,
				BlockHeader = "501645b6501645b6ad809b6138e48185710b3ca76f3f15afea43c329b9e06cdbae8854bf530d6947ceac5b208839e14d8030397a9c7e793551ada8d5b9d5b2eb3ec6694f1698487d501645b6",
				MidState = "fc8c8a445bb2a158bb873c10671389e61af0a6b941e0f3a0b76facce792d1f02",
				RestHeader = "4f69c63e7d489816b6451650",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 132,
				BlockHeader = "9e2bd33e9e2bd33e161fa49d33f2f2a17e6d15de52bfc80042df89802c9c7917b03651bba1f16b69ded38d484e99e56a47b6bd2dcab4130d3fb51c81af700f4a647a2e0019c2bcd39e2bd33e",
				MidState = "f1964a8089eb53f8b744fbcd263aa72cb62c07d35b0659c948d2f01a705f776b",
				RestHeader = "002e7a64d3bcc2193ed32b9e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 133,
				BlockHeader = "eb3f60c5eb3f60c57fbfaed92f0063be8acfee15363f7c529a7a50d6a0588553b2e54eb60c62466761e5cf7fe6818af0aeebac437a390c165e87669fafdb8eaa9c567d7ceb4479e2eb3f60c5",
				MidState = "b88d905dba2b17cc1a9ca8815fe8c1cbfd5bc5769e3d9dfd85ca5ca6317cad24",
				RestHeader = "7c7d569ce27944ebc5603feb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 134,
				BlockHeader = "4e54dc244e54dc240d2579559430c2ce96f83cb7e27299c01f15d5a011853935d10bf9ad3ede8f1b83b910c21a214ef0bf9f0ffe6ab36bc0e450850cdae348073f61f9403a01063e4e54dc24",
				MidState = "3ea3e49cd9352e02fb38763ac3ff105757a41f1c012e65ca353ebf026324c793",
				RestHeader = "40f9613f3e06013a24dc544e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 135,
				BlockHeader = "405c294fb24f1d69b5f8d151ac13a4a9ba517915ca26611599d8719f4799c5ce0224f1fd2ea27a216beee605c4823367b1c3b985b675ae169da993ca4871eadda411373081f20c6fb24f1d69",
				MidState = "2efef659c31c5d75af30d5f400ce91a895c4d0b7deb5ced30ec75e0affa8a99f",
				RestHeader = "303711a46f0cf281691d4fb2",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 136,
				BlockHeader = "04e0d27804e0d278fc1556e9deb85d8ee9b63d4ac267fc67af3671a2ed94d99b5f206a4296ef09b4ba952f54352674c5337d5b754eaa70c18fbb8890cce560705c9114351752456804e0d278",
				MidState = "cf6e34a9a2bfa5ad591a71db0ed13541dbf4a82db199087d7b3919a80bc517fb",
				RestHeader = "3514915c6845521778d2e004",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 137,
				BlockHeader = "69bf5fb969bf5fb9bd8f6c9e632add3df6a2d8ae1349229157025e155a69bbc6c38cd6dce5b1752b7320ba83213f3f9b2fe593e9473ebe4defb374acaaa8bc2893bafe0e57555c4569bf5fb9",
				MidState = "322242a9fb5aa2c01432f7f5b3284a8f9e68cd45cdae2ba640db4e96fba21754",
				RestHeader = "0efeba93455c5557b95fbf69",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 138,
				BlockHeader = "c49cea16c49cea1641cc1cf851861ba92349e584c64c65c67385acc87caddad36078335743a7d5b64605133038fc4a177b9493d11af2f9223d806d7d73e5e3f8b0eaaa3a40ad0735c49cea16",
				MidState = "b1e195b966c043e6e5eeda27f3f1c414c060145bd998eed49dea9e6dff5a7d3c",
				RestHeader = "3aaaeab03507ad4016ea9cc4",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 139,
				BlockHeader = "f4272dfbf4272dfb3b9564b68f756d95d077ab4d282edecf4ebad590b8b9466354c1677629e679e0207251aacd72e8707da5a18a30fcdb1f8d3dbe28affe6d1e1ab73a42a3234eabf4272dfb",
				MidState = "d13430629e7ab051c0e8f34636d17129cd6ec7f875165fe3bd802094354d018b",
				RestHeader = "423ab71aab4e23a3fb2d27f4",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 140,
				BlockHeader = "da144495da1444955f05adefd30dddade8cd27a9fda18cab8dfda8811f539e815a1ee42c8c5e886e54f4d6690f852d12b7d8b8be85d7cac313bd7f83b7ab87abcecd9053e83b4805da144495",
				MidState = "174055897663a43bc40248d589b14bcc122325718136a38bf68e3ac8822b7d80",
				RestHeader = "5390cdce05483be8954414da",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 141,
				BlockHeader = "41cf3e2841cf3e2833104440b477645225dddc3e6e68df5d4354ce6a8d5ac1a771615fcfae51df009f3925e54b1e9ae834fea0c614b11e7d64465a108db48b3b66002551125537a541cf3e28",
				MidState = "1b1050567afd25ce8eb8eca13b885e28be6bc9fdf88cc62a46e6708e7c704ad7",
				RestHeader = "51250066a5375512283ecf41",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 142,
				BlockHeader = "8c0e8f088c0e8f08739c69857c9fe2615c54b170bfee40afb0bb40ae10605cdc4904131cf9c8fb40e14e18842d3b12415c029c8e93839c66d3ce2bffc57412f312a2690f678d18d08c0e8f08",
				MidState = "52dc39f6d2d3000cc6baf6eeb571c72bf37c499c027fd45f2378b1aa656ed2ea",
				RestHeader = "0f69a212d0188d67088f0e8c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 143,
				BlockHeader = "0358428503584285e5bc8d4f68313ca1f05adcf5a2fba493e15d846b1adb103f107692f3967c90ec61fcbd2e959f89489b67fc75e75a6a53e349bf4bc51e5bb5f32a6279c59c7f5b03584285",
				MidState = "a196d91ab305bd703725d415e85260297dc7222cab2449ede7972adf877642d7",
				RestHeader = "79622af35b7f9cc585425803",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 144,
				BlockHeader = "9088a9bf9088a9bf9610ffda709807a19d566ba18a3d1d7a34269dcfe6af45ff06520b20d467ef885b812dcaa0d4362aea33810d943c0c34660240b12e27f3d0715ba7305eff9d109088a9bf",
				MidState = "e85717f136b1ad7c6a7d92fb9a39bdcac7b9ad5a539e4eba3e6872f4ad3678de",
				RestHeader = "30a75b71109dff5ebfa98890",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 145,
				BlockHeader = "0bbd120e0bbd120eaa88d0292dd4b99cce1fd219935b77eb711e6ac15b35707e7ea57a25f09f48afecca1768e608babefd99f83d950d7361a506b38e831be31afcb52439cd0403d80bbd120e",
				MidState = "80067798f41440845f922f2d113ce54b11c0b972ff4c4b4a726b0863b541e00f",
				RestHeader = "3924b5fcd80304cd0e12bd0b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 146,
				BlockHeader = "59d29f9659d29f961327d96528e22ab9da81ab5077da2a3cc9b93018cef17cba80537720f091e19674f25e91ecbe13779a3232350b47a7d28146ebafa365fad223f9e813222f3e7559d29f96",
				MidState = "3f05c63ff718c6c7c8f0754d90f6c9d4a64c2975ace8f4cd6ba4ca1ec8cb9985",
				RestHeader = "13e8f923753e2f22969fd259",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 147,
				BlockHeader = "0c139a4c0c139a4c2aad04347b5ead91855b42bd259b51cdfcbc451971016c9a06fb65c88c1854ba817e281ac3bc6a3c01acc587252f113cfa7219194d43ed49f42cfe1c922530fc0c139a4c",
				MidState = "ac4ec52777413e2ba8eb7a96ca2577126c290f9502164e504e4115f0911d8637",
				RestHeader = "1cfe2cf4fc3025924c9a130c",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 148,
				BlockHeader = "9f8b16d49f8b16d49c6ad16c9fe11654f19284e3f7e7f5cbd4260c45ee5201ecec225fbed9b21da80b3a68e4448a619faed54df1af8cabb3483c28698ea9bbdbc0bd1e50633a39519f8b16d4",
				MidState = "ab42d8c1aa8f69dbc74ebaea188e087a97ed3b7de21b3a51b09974deb34a1e77",
				RestHeader = "501ebdc051393a63d4168b9f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 149,
				BlockHeader = "ad56ba34ad56ba34f580f8b1c06be309198c393bc59d1c8c754000363b2e2782bf64046b21852903792fb76c4c0d3ec1862805bf7212e75ec31a4d252e5fe1203322b355a9729920ad56ba34",
				MidState = "617b4e26fe7481c736252b1a1fcdc7f4c958d601e7a45369977c1bd7673e6ab9",
				RestHeader = "55b32233209972a934ba56ad",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 150,
				BlockHeader = "8165627f8165627f44c3d87c7949fee6fa1fe1099661066994be176fca27e66e6db87244792f47224b2c641e5a5cb6c91a42a8d75537d2d4dbb90c85e9375a061854107d04bdc7978165627f",
				MidState = "ce81a0c43205eaeb7c7f7df7aa2cf7acbfd851f9df6c80ce9bf13e28efd1c667",
				RestHeader = "7d10541897c7bd047f626581",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 151,
				BlockHeader = "cf7aef06cf7aef06ad63e2b874566f020681ba407ae1b9bbec59ddc53ee3f2aa6f666f4023e7415951b07f223b213e11f89b38f979246f9a285636502fe7c6f96ee0c646be10ee51cf7aef06",
				MidState = "67177233659f2c09dfa9506283e9e53b3c7bab3bb9c4aaecae8abbcd717bf221",
				RestHeader = "46c6e06e51ee10be06ef7acf",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 152,
				BlockHeader = "1d8e7c8e1d8e7c8e1602ebf47064e01e12e393775e616d0c44f5a41cb19ffee672156c3ce23c5fe21360df48105152d0ffbafbc14cc1dead621c26e3e98bc885f176c11e6f36de2c1d8e7c8e",
				MidState = "6f91c93d7a0f4ffd028b5368f658dca2945a0c570fea556db7672d7a97ac4db9",
				RestHeader = "1ec176f12cde366f8e7c8e1d",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 153,
				BlockHeader = "6ba309156ba309157fa1f4306b72513b1f456cae41e0205d9c906a73245b0b2274c36837f67f449c18c6dc0cdcf85b2ee6a4b1265abd3c206ae9fd50ea7f5c4e6d02a5694fb2cca26ba30915",
				MidState = "ccb90ab979ccfe2846815ebc7a0862aefe83053a3aa022347d654d8f51fa482d",
				RestHeader = "69a5026da2ccb24f1509a36b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 154,
				BlockHeader = "a87ae283a87ae283dd018fbfb764aeefe7c34425158dc928839e7548f28444c1c6dfc3957a492efa24f193dfeed3423d3401eb336a2e11744d9a4970130733accd30dd79ead29aeca87ae283",
				MidState = "75bf4863208470d8bc8304b20d2226a004473450a2d67ec0d13b239ab5d228de",
				RestHeader = "79dd30cdec9ad2ea83e27aa8",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 155,
				BlockHeader = "531ead13531ead13c836b4273806353cd533df69bddaaeacb5c115a1c9a8a2c20c696fe5405d8343924d79620c8892117619ad3a33adbaaae297383252dc3c18b0ee6746ba26d238531ead13",
				MidState = "c0e5dbcec6c9cd9c078bf5d10218a0e6c23093e4ab80c9a97b5ae8bc275ef915",
				RestHeader = "4667eeb038d226ba13ad1e53",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 156,
				BlockHeader = "ece07d7aece07d7a8ac396e325c2ecc49c816dbd1a1b27551270b177a25605d7abc5c4054a4ec3cd7c1960c3b73eb28092a0df98daeb6ced939185efa0696d2bf125b773cc58c761ece07d7a",
				MidState = "cd221932570761b022672d4d09c6b0729bebe05c53966727d4a9978e4035003e",
				RestHeader = "73b725f161c758cc7a7de0ec",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 157,
				BlockHeader = "04c870c904c870c9575fa07aefb50f854e546a95c03ae4ed353caf00108ff266a11b99ba0cca3adbb5bf49c243bd9eedeb35984c1c8ed249ab233b51293afb613b9dc7635388bc4304c870c9",
				MidState = "3a6864868ecb819717a3e60724ab771fc756fb7f064ff953fbbee7143bb4162f",
				RestHeader = "63c79d3b43bc8853c970c804",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 158,
				BlockHeader = "d356271fd356271fe54271d4bbb1b50d81a8534f94f69558e2a31dde9994dad6dc116897f24a0cd4d4acc5247860d947799b87636c34d79fe2ed9da50b1e4bca0f78ae2295fee48dd356271f",
				MidState = "4aa2726f48ab92a95ba1cc668db5fe3351c1e7aa6fcaf792d0d7901807e5a133",
				RestHeader = "22ae780f8de4fe951f2756d3",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 159,
				BlockHeader = "83c23de283c23de2f7fe77cb8ded19a6db2c0c65d0cc3fcaeccfcc80487c5002519db27203020878e7211e1af39ed3bc552d8459c3074781df5f17a8e81f9e5dc951affefea16c6183c23de2",
				MidState = "49936e88f5c9d0467cdba223d4040f06abcdd9581c6ace72ff209a9075cb11b2",
				RestHeader = "feaf51c9616ca1fee23dc283",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 160,
				BlockHeader = "3f179d593f179d5991690158cae30be2e2e03cbfaf3143c1c9471ed442923befd5c1bbe664c9ba8f0c02c71bfb89c3f11fc08f46224f3dee3f958738e4332b9bde620e915664bbd03f179d59",
				MidState = "c3edd2b7ac79ab0243fe0f400385c93797eb2770d776fb3dd3bf27f9dfec5740",
				RestHeader = "910e62ded0bb6456599d173f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 161,
				BlockHeader = "ffff828effff828e400ee5a8c77a8ec059f2f0a2373c4776c8ac872d296a29fe5288b06d82827277cc7d7b2e54467deceaa59fc3536b87639579ec61fce25f329aa5d7f06c015b50ffff828e",
				MidState = "1cf573980c2ac240cd28dfaa4e76730de459fd3cfb558bd56f31b1fbe4b02351",
				RestHeader = "f0d7a59a505b016c8e82ffff",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 162,
				BlockHeader = "7649360b7649360bb22e0973b20ce701edf91b271a49ab59f84ecbea33e5dd6119f92f448200f2c810276ed4f53d74fbd5f7d6ac5178824c049d10f145991634ff5c48eca83508c57649360b",
				MidState = "2bba378688f19fce6dc47852a2df6cb2a79d47414b6a26cb2d2c0894e401efdf",
				RestHeader = "ec485cffc50835a80b364976",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 163,
				BlockHeader = "f6d429b1f6d429b1cd299043557e99f2fc581e11fb92922f47d457b649159817f8557aa16714b972a1d7e59825692134eee76200c53a09422ba15e3c06a074a175f2c8f91dcc18e6f6d429b1",
				MidState = "13eec156d17cefbf6d36143986bde374d269879caebd4b2047e5864a1bec1af4",
				RestHeader = "f9c8f275e618cc1db129d4f6",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 164,
				BlockHeader = "43e9b63843e9b63836c99a7f508c0a0e09bbf748de1245809f6f1d0dbdd1a453fa03779cd7a8e6f5ea04f7fd820709c0717b51ffce1dcc9622f9c94f435fb8c1534fe8c5bbb6e4e843e9b638",
				MidState = "2dd5a802d2d838f100483b12eb3218468d370ef5c77bbd96dfde27f366f1fdd1",
				RestHeader = "c5e84f53e8e4b6bb38b6e943",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 165,
				BlockHeader = "ce78c664ce78c664041353b8114e4454bc60ed5cf11f25b6efeeffb1d7716004dddfd9a3bda7c0083fed492d71e08e1be2aa787104cf1625b24c972eda850038932fe2d69b174e38ce78c664",
				MidState = "c8ce7c7ea38fe36eccdc1738682c00451106feab1fba9806d27e6a85f480a5c6",
				RestHeader = "d6e22f93384e179b64c678ce",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 166,
				BlockHeader = "e74e55d6e74e55d68a0863ed89dc3af4f39ee0d40479e8dbf48e5ea8393614a39d39a1ab082f2367962a2c9dcec232cf00a9823ab80e17e76bc478b72f33b96afa51bcde9787ddc7e74e55d6",
				MidState = "8308a318bbdabc082f5256c77b14a695681f645df436d8b99b8da827abe97a5e",
				RestHeader = "debc51fac7dd8797d6554ee7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 167,
				BlockHeader = "48d302f248d302f293635e4421aea05b58e1d5557c43fa321efebb0ff6a7b6b96a4713007b6a6312716469a5c53da93bfd651f1a551020709227125940412ad5c99d3faa49d5980348d302f2",
				MidState = "5c0afb821af3bbce68c9758b7b4f9702d8994346c239ab84ef9015fae04d2d6a",
				RestHeader = "aa3f9dc90398d549f202d348",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 168,
				BlockHeader = "23f49b6023f49b6033446f9616241863d4234e6c9850f7729e69721e1aab51a42c5b98e081bd4e5a98c69a181c309a665a6443f3b82c4432c1641b10bafe74cb63b5e4a5f3a5cdaa23f49b60",
				MidState = "fab438d77451b619c4bbb077dc2dc8c12c2eec371317dd6e5edd69b06fa76351",
				RestHeader = "a5e4b563aacda5f3609bf423",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 169,
				BlockHeader = "710928e7710928e79ce479d21132897fe18527a37bd0aac4f60538748d675de02e0a94dc3b6e885cedca021db40666e48c15b32dbc941e00a746d0320115bff590c75bc0637208d0710928e7",
				MidState = "4a4b0a521d3b81414a741f0619bbd31d71a972fabd225438ade712098edb6c0d",
				RestHeader = "c05bc790d0087263e7280971",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 170,
				BlockHeader = "4f41b2254f41b2255c46df3cf9e6b333ef16104db6ebfdd5ce3cb9646f91288e57f25f71712f612751ead7b02060a65d9ff9244aec37c5ffc0c0a8d2cba5cb933b30efc12b027b5c4f41b225",
				MidState = "fae6f3d80ebe0c5384ab5353e873f2c6540ee825035ea3daf15ec4c12a8822b7",
				RestHeader = "c1ef303b5c7b022b25b2414f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 171,
				BlockHeader = "f2621c44f2621c44eb7fe96b5d3026fd4ee09ab8a66d3279f9f9b969bb86502912e952fb745b501e7eab6c46b33c1a7edff5d84de80842b2be44355d5a703245b821e7c11d2d73e1f2621c44",
				MidState = "066ef21d8ee120ec6373d1737fc585b3264a0899e850316f54498d5803503db3",
				RestHeader = "c1e721b8e1732d1d441c62f2",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 172,
				BlockHeader = "007e2a6d007e2a6d867cf4cc59e3014af7c5ac4635df4f2d805f555ef1b6daf256dfc40df285cf36d92bd800ed8ee91725cb1aa5bea9cec94c3b9c979f9006e45f34bac022a69f0d007e2a6d",
				MidState = "ec2e4c3200a5bfa7b7d08c6dc63b9681bc1f52fbc29657de5cc21aeeee65f73e",
				RestHeader = "c0ba345f0d9fa6226d2a7e00",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 173,
				BlockHeader = "8845fee98845fee9ac2c354dfc739fce42df1266b4fca961c83ca4c2721833c776b20f0634265c7d209f028468f0e12bc8dbc555ab32dd74d0e744ed13b386f969275ee66f85c82d8845fee9",
				MidState = "f9c62f59e63eab556915cd333a03c7e52b186a21444e5fdcb8b3faa6b2ab67b5",
				RestHeader = "e65e27692dc8856fe9fe4588",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 174,
				BlockHeader = "c7c616bfc7c616bf170988e164d3d497c0618237c501d5ca5c3235604732585628c4801246a51eb7addd578f6365029248442a2cbbbc7e6893686aa02fb9c1659a9803e5dd7ddec1c7c616bf",
				MidState = "5f9ac0e23dc25bf5808c431ac309bdd20e87f17ec420f93a9a9ca4bd69b71801",
				RestHeader = "e503989ac1de7dddbf16c6c7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 175,
				BlockHeader = "b004be56b004be5653e8a49556fd27ece5870ddc7081efbe64038964a1667d0a2fcf770594624491a6fb5e94489184a6af1a471f9f01fb98392349ec6f988a4637d098c70c79d8ccb004be56",
				MidState = "53ee4f7a5ca3b584a90d14dcdf92c421c5781b77475b247442fc1da0cd5de80d",
				RestHeader = "c798d037ccd8790c56be04b0",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 176,
				BlockHeader = "87b66e3887b66e38b68920ffdd33572636d654f7217e45dd6cb5a33f6a3d8d2dec85b0c5c1379c2ec51da70c38b953658dd7ebf4a5bb8c39d2e8b14b22138501252db2b4da7ebda687b66e38",
				MidState = "cd686824405531509c5908b351eaaee77ec9b60e20427022240b2021811d8ce2",
				RestHeader = "b4b22d25a6bd7eda386eb687",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 177,
				BlockHeader = "d5cbfbbfd5cbfbbf1f28293bd841c84243382d2e04fef92ec4516a96ddf99a69ee33adc0fd9754cca8773b3862c48e1373d0129518320e6c039c60c006309cba995f8a9633466cf8d5cbfbbf",
				MidState = "8e7a649f9e520f4d6f3bde5f9073ec3457a380925a6f2638d505af588257c58f",
				RestHeader = "968a5f99f86c4633bffbcbd5",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 178,
				BlockHeader = "23df884723df884788c83277d34f395e4f9a0665e87dac801cec30ed50b5a6a5f0e2aabc920a469587d942f3d846c0eb02f1705da2d045ba173abf17818976d96b01328d40e9077623df8847",
				MidState = "0eb3da759f7ced0c3d51b5a7ad20de12f2189e5e8e70581738711d4999312e5f",
				RestHeader = "8d32016b7607e9404788df23",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 179,
				BlockHeader = "9eb9d2ac9eb9d2ac0c961276f6375852f7c79108cf8c79fce03dff2f2baa53e0f1b8628628bdf639f9757f925b3de9d0ec5ef481f3e8858e201914aa8d8faac8093fbbc842e116269eb9d2ac",
				MidState = "cad47a7cf9aefbe796103aae6314136dc72c91931d86d6bce4698ecc7e6e235b",
				RestHeader = "c8bb3f092616e142acd2b99e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 180,
				BlockHeader = "87f6794387f6794347752d29e761aba71cee1cad7a0b93f0e80f523385de7894f8c3597992910403ba07dd34f2c2daddc3efff0527e41019cbde37413cbfd1a4460e5cfcccaa89ae87f67943",
				MidState = "6c9767e7d64fdb44852c165b9b58967e3912ea110e3fd83ba494ac1f346638e2",
				RestHeader = "fc5c0e46ae89aacc4379f687",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 181,
				BlockHeader = "2b0b814a2b0b814ae448e5fd479651d21e1d66d0f177c311726fc1327a584f1dac835fb7b9320d4ea594a5370ea2c67295e63a45c199b3d6609f82d564e206313766119852624f542b0b814a",
				MidState = "7e355abeed171dc69ff7e404c40d82df14b9b1851fa573c1cb1d87bbdee8c0bc",
				RestHeader = "98116637544f62524a810b2b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 182,
				BlockHeader = "a7e1dadba7e1dadb658b266de034a5d86912d00933e1f868a58203f53a06f30eed1c6562ec3a3bb8f5f83fee46eadc939a0471d055db3e67593bf6cd2e15c525a7419c971b5a29cfa7e1dadb",
				MidState = "f8f7951fd6641d4d7b8852d76653a13cf983861a08d272fd62afabb059c94f23",
				RestHeader = "979c41a7cf295a1bdbdae1a7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 183,
				BlockHeader = "5756f7815756f781e18e930459b0bc558c424b342631c5e4d4811d33362c8bcdd034c34447a823ef8323a36180171dc13c7f27af384e62992a09c73ff76abeb9f09268c0cb743fe25756f781",
				MidState = "74414d7ded1e5f40209cad67ed4aef8e50b9a208c6f47ad9d09290f7344477cd",
				RestHeader = "c06892f0e23f74cb81f75657",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 184,
				BlockHeader = "6255807b6255807b1b062a99ac9f19a13eeb490cf2be5cc598aaf733f499c9c4b615f8534e38173bc4cfc446ecd3f79b60eb7f82d299d29f8fcc1ca00050d6d1f8b25fbe70482b406255807b",
				MidState = "52c8e32e9eb0ea0ad7803d7222365151faf520da51167b33c7ceac64f7f10dfc",
				RestHeader = "be5fb2f8402b48707b805562",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 185,
				BlockHeader = "cb8325cfcb8325cff22a42029b7e97962a0c6203475aa3f441e69efbc7e5f35e3a900418a78ee301db4312c9b1ffe9c90f32499b3a785e01bf90278daeb0a2be956abeae90e359c7cb8325cf",
				MidState = "3e7849e84e0e40e539f9cf2a1059023a6f6e0dcaaa2f30436a1c65179d9c6037",
				RestHeader = "aebe6a95c759e390cf2583cb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 186,
				BlockHeader = "ebdb07e1ebdb07e118118768ecc67e300ef03486c80c0cc31393c2bb58e9bff3e67dffa072013a375db2232a9a6a65783394829934309144a6d8762606f3a88bfc999df520a068fcebdb07e1",
				MidState = "dea13c38eab07ecdfa3fac7199d08284bd1262cd37bee2e4a161136e89357a95",
				RestHeader = "f59d99fcfc68a020e107dbeb",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 187,
				BlockHeader = "15102dd615102dd62191a1f6dc4a6754969386d4c899bd55ec9a4021eea8661aab40817cec2c5bfadeda85242f6f1b586c4a7d0c81f43372dfe873d11dd49cf7eaacd9e700df3c4615102dd6",
				MidState = "a921a6a47c915c469dfbae3e479cd7c6399a32a00da08330b283f5fbea5fedc6",
				RestHeader = "e7d9acea463cdf00d62d1015",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 188,
				BlockHeader = "d518ae78d518ae7853efa31cddf0d1843316bf2b738b26b71b6516bfb21de4a7ec88f693bfa7dcf0e2bd92455b7514d5a42bd3652cdac85a59322affbf25f1c5dbb641ca4e87d1bcd518ae78",
				MidState = "11672d5a49c662f4bc44f11d5298234e58f21e2f394c763f56ac7a6d715340d3",
				RestHeader = "ca41b6dbbcd1874e78ae18d5",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 189,
				BlockHeader = "1fbbfea51fbbfea5cfb2b3906155e07f4b755bf6e90eab788f7b6eb0bbf1b53eba818a809e6b20fb709ff94eb97d4a66b329f1eaef77a0545cef0156555f677729405388e94b85c01fbbfea5",
				MidState = "f691253e92308d28bb3af267a13237ec9d1f6e3c4cdb4f7e7c4110d9ac5a0fbf",
				RestHeader = "88534029c0854be9a5febb1f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 190,
				BlockHeader = "8539d2508539d250ef0018afffa92e14fc66ad528712909ad0973b1b0b7196fcb74b7347701fa2479579b76c67160782f7f7303acde21a93d2ba388cc2e99bef615836cecbb0d8ed8539d250",
				MidState = "806cac7c2b5c02810054be1622f9a3e5af4656e37b97e5f4237289d62d1da4ae",
				RestHeader = "ce365861edd8b0cb50d23985",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 191,
				BlockHeader = "61596bbe61596bbe8fe22902f41fa51c79a72669a41f8eda5102f22a2e7532e77a5ff827a84c7f9c32ad1a052ef3616497a4528f67dc8285ca7c1fa135bd310c5fedc4f4605c84c061596bbe",
				MidState = "5f253aa1acc039e505e2827ec6b7462dd8ecafec283ebe2a4a34a5d0d68c572d",
				RestHeader = "f4c4ed5fc0845c60be6b5961",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 192,
				BlockHeader = "ae6ef845ae6ef845f881323eef2d1638850affa0879f412ba99eb981a1313e237c0ef52332897ee59233ae32e7bb969a480ca843bff662b6680d7ad3c960dfca04f5a880eb19e170ae6ef845",
				MidState = "991d3dd3d7a89b894175cdc357bcdb138370706f208fcc4d04df93ab80891924",
				RestHeader = "80a8f50470e119eb45f86eae",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 193,
				BlockHeader = "f1e83bf7f1e83bf74bd18e2bf35205eebdab955aacd5c470d6d7e468d281097457c0b1ebba0108fd130208f1af77cdb94601f4af31b5ca40daa8639030ff17b0c64482eff63eb518f1e83bf7",
				MidState = "cd9f7fb27f459a944f1f2945a14373a5cd800f62abe81711946ce2ab62a45b2f",
				RestHeader = "ef8244c618b53ef6f73be8f1",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 194,
				BlockHeader = "94f6d1bb94f6d1bbf7c99231d7beab6abf78c1a04fcf88334f194de18cc89936c4867cfe7f2695820322e33338cd4faae79e13eaffe1a78267dc99db3dba665f7eeb6295fa3ef82694f6d1bb",
				MidState = "bc33621f015432f0256b28c0fdb3e3ade4e2f47c60894c91591cdd97342e5477",
				RestHeader = "9562eb7e26f83efabbd1f694",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 195,
				BlockHeader = "b7ff6059b7ff6059845b20f93dd5f8bf7800d379b7b48e6c2d935770222fd85f46dcdf62d13122a3cdca88d045d13b17b58cb83c6e547c68bafdce86f6ec35d7bcb4cdd13fa9aca3b7ff6059",
				MidState = "80a33f136b6bedb21f8c184eb93cebf8e47d0c869b37d4701f4fea83ee4496d5",
				RestHeader = "d1cdb4bca3aca93f5960ffb7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 196,
				BlockHeader = "04cafcc704cafcc7011abce6ef30a1bd500eab48c67a8dcce2d0490eefb4ec6be73b086b8221f7b369ccb8e184bb0b04c9f67ddfd22cdcb555deef68f27f823ff8daa08f5907f14104cafcc7",
				MidState = "902a448490923a0b9d113757fb8aff197940dc50ecc7c5a621ee62fe0a995915",
				RestHeader = "8fa0daf841f10759c7fcca04",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 197,
				BlockHeader = "6e76aef36e76aef3cdbda8c3f096e29f48da68ea7701cbfc1f4c04b97dbf6e8bde25949e83c2b748cef275dcb4ff81e7183a690ca8bc7a0539e86c6bfbebf201a25f0281ed5bb2986e76aef3",
				MidState = "76bc0db2a7f8684b12137da47b2dc9d45214d690467d6722d720b6cc6bd08995",
				RestHeader = "81025fa298b25bedf3ae766e",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 198,
				BlockHeader = "90f356b890f356b870a440f4fcedbb716a81051c3791899e0009a39f73c780f70535b23936e62c9fe4c41e35d444b76d5863730586073cbcc82eb7238e7f02e1d6a2e49d48e3c78e90f356b8",
				MidState = "8109d1a72fe49c6e65648c77dd97900c722f1fc348dd1778c03590d0ac383d76",
				RestHeader = "9de4a2d68ec7e348b856f390",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 199,
				BlockHeader = "9376287405691c8e36f918797c8f234d77e21778fc0d944776800580c570d6eb5a52fba4f962b2d6b6efc41ed97eaa3e68022eef5b08d1114bbc8670f53c2af0112fa8cc466b914d05691c8e",
				MidState = "a8c0369102f32041432eff1652f70c68ae8fc4499044f2f184e343c0a2f4e2d6",
				RestHeader = "cca82f114d916b468e1c6905",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 200,
				BlockHeader = "16c3806016c3806057122adc26dac72363c2f223b6babdea386c46496919eedee0ad54ad7a6bc7e0e3dc392599abf971211576c50af3b4077885ef14597ef6abb4c25bd81185ed7a16c38060",
				MidState = "1cab4c6f336bbd2f00f9a96894c19376ab764bd984a0a77d39e66af061810a71",
				RestHeader = "d85bc2b47aed85116080c316",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 201,
				BlockHeader = "f959d54af959d54a02a0a9a4ef33034f09c1dc0a839a3ae29da6bb814cc79dd2674ef65d115f06830e8237a6009a4deade371abfcfcb60fcb14029b039c9946c174f39faa92b4de0f959d54a",
				MidState = "2bcf966702ab1c570bbd216e9615db424817c6a1a7e53c44c423f50740fd5d26",
				RestHeader = "fa394f17e04d2ba94ad559f9",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 202,
				BlockHeader = "d2652aded2652ade25a6a80736540eda489da536975d4fd3d547e40957e240fbc5fe37fe272834308e020695c11091206efb2f68030d414780a87597055c9434adc7869072c761c8d2652ade",
				MidState = "2a714a6d225619cef3fb79f3b623d19044dc1b68102bc3795b712dbd99682c14",
				RestHeader = "9086c7adc861c772de2a65d2",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 203,
				BlockHeader = "8fa0e8428fa0e8422b6df9c5a6df337df9a7dc4554fa0d67cb5809df236b5e0cebfc22fef8917a20c66ca763854e9f2c455cee831794e1a764dd9ab9f2a38798943503bcb2d0beb68fa0e842",
				MidState = "4a6c32375dc450fa4521c5dba7c7a9c482473cb96def59fa58a986c48db31fc7",
				RestHeader = "bc033594b6bed0b242e8a08f",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 204,
				BlockHeader = "6ac181af6ac181afcb4f0a179b55ab8576e9555c70080aa74bc3c0ed466ff9f7ae10a7df1de674a27e7b2bb9452f9b53468c3b9f3c335250d6eb0c155158a005aa5ddce32362f1fb6ac181af",
				MidState = "ffe7bfee0e9f2e5364dca1ee35e80b25cbc5cf09ec61a29a5e51dadb7f0580f3",
				RestHeader = "e3dc5daafbf16223af81c16a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 205,
				BlockHeader = "b8d50e37b8d50e3734ee135396631ca1824b2e935488bdf9a35e8744b92b0533b0bfa4da7ade49848e9cc667db6b9e1ff4ffa17b25b1ed8ee4c3562e4084e79b3ed9a684e70a8087b8d50e37",
				MidState = "90bf41cd6984674098dd56be4ed497943a120dd607aee3f0e5086ddc295ee4e0",
				RestHeader = "84a6d93e87800ae7370ed5b8",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 206,
				BlockHeader = "5072afd25072afd233beba84c78a1f83be3b9250e90ba0f26437bdc0fba1bcbd9258821d9909537ad402a3e7fa37a0c1201d1e0cf0ea57d4c284e9a21eeefb6fef4892b061c621eb5072afd2",
				MidState = "4d7c412ca7ad96d430ce4c8c2ba9a3b24e6857aa9633ecdcd5ec2a9fc7eae864",
				RestHeader = "b09248efeb21c661d2af7250",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 207,
				BlockHeader = "ec9bc9e1ec9bc9e105fdccfcbea601bcd7ff44beb00a0795146d4a6de219d53596b57c146cae00c0e00e5987a9cf35ad2e757b466777e12b4fc316ce418ea6d18583a7a88fca9b4dec9bc9e1",
				MidState = "aee3de1f83e9286b5a3fca46bdb101d7b4b1c2a60620fc106cf31da11f2036ba",
				RestHeader = "a8a783854d9bca8fe1c99bec",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 208,
				BlockHeader = "1a0353e51a0353e53dcea6f7852586f9276b72a61d8036011becd39c15101a5dadda1365d73c088f1709dbcca4d6e1c44d46e5bbdd1ab55b28edb12adddcaf533b103c815fc849491a0353e5",
				MidState = "54e1f8f7aaeaf1f51b147d050ccff57d5f539e3a91182357a410f6f99a21640f",
				RestHeader = "813c103b4949c85fe553031a",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 209,
				BlockHeader = "ac32df53ac32df53c3aa07e05ff5269e5f4eb3651a12260e50f8c60feb2ab77f32b03969da62f2fd8998214d2a78c7efef107b29583df3cf2996488eb7ad115196f99aa154da2b1cac32df53",
				MidState = "84afbd92aa3662422e7311cfe4589fe0238fde5e96daaeb740e989fcefcb0145",
				RestHeader = "a19af9961c2bda5453df32ac",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 210,
				BlockHeader = "fa476cdbfa476cdb2c49101c5b0397ba6bb08c9cfd92d960a8938c665fe6c4bb345f3664123f419da01f89aa2a5db4798b469518699a385f4dc314e7ae4eaa3df4145bc4c91262edfa476cdb",
				MidState = "0579c8f0444973a05962f603b527cdc66b4c79b9c7b4adb7b0b12637acca6403",
				RestHeader = "c45b14f4ed6212c9db6c47fa",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 211,
				BlockHeader = "498c264c498c264c9cfce2ef969c38b7f232e6abc49e389b61d30569cbe226038aee2cbb50dd2168d1095e220a50e9d8d5509f449dd14886825ab811832d4be527caa5bed467d518498c264c",
				MidState = "710562b8db28278a625febfe9affe708099ed8573c7facc4f59b297bb9ecee5e",
				RestHeader = "bea5ca2718d567d44c268c49",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 212,
				BlockHeader = "b7ad8dbcb7ad8dbc5641965a53cfc0b20d9e716db575f16e4bf4f43a51f968ee962df90d2a36c0c78996c816fce13f131a970143c41d4eddbf413717601b8581e7b79f91c4ea550db7ad8dbc",
				MidState = "0221b226d53e179b585d120844799ef4d7fd8e559df2f0c8c2afd5f21482805a",
				RestHeader = "919fb7e70d55eac4bc8dadb7",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 213,
				BlockHeader = "6b56aeff6b56aeff2e4f85fd73efa496a564a6affe8d4254762d0586846939e8d28dd0686b0f993cc6f9dec24b73d9146ba82b8083f81163890531c65f7c654f7b78179f09280d8d6b56aeff",
				MidState = "c6a5d1fa198984f48817375f8891afdb2575b72d9517c177653ccd867ba0bc27",
				RestHeader = "9f17787b8d0d2809ffae566b",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 214,
				BlockHeader = "d8f62bb3d8f62bb3433ed1dd5957344819b0130c62dec8ca275fd600cd2af8951aa5da6f9773b3576053941232eefdc4654ae5652edea351cee8acbccb00ae662d9768f51e6e9672d8f62bb3",
				MidState = "00165fa9c95e08f73d99aadc78a551939db6ad3ad2dffa1628b33e1e1c6e2e52",
				RestHeader = "f568972d72966e1eb32bf6d8",
				Nonce = "00000001"
			},
			new SelfTestData () {
				Index = 215,
				BlockHeader = "7498f5467498f5462b19fc6fbad61228b03698e6d9d3944ac1f519e440309d5105f6ed40e606d45499c583464ab67b1293afb01819bff74b80ae10e3f41695bd74f26274c9f5d9497498f546",
				MidState = "426a9b8673a2d54c50839f8b3e7c725ec073a202ceb01ee3425af05ea6aaab34",
				RestHeader = "7462f27449d9f5c946f59874",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 216,
				BlockHeader = "cb2626b2cb2626b23bc54faefa64cd8ef659c567fb3b8bd55bcc5b1d87f4a2a4c304fae42b31ef3c257be4c6335d6e6874f3c789ac5b2143999a5463bb0396a8e0c7c26ce71ee183cb2626b2",
				MidState = "f834bfc2636a83c8959c1b54abc98572ff8877c485b026d2315070e04d8fa6fb",
				RestHeader = "6cc2c7e083e11ee7b22626cb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 217,
				BlockHeader = "76c2453476c24534cbbd30a4a8d453cb0dee7d9ed5034b008ea96c0b9df7bfe78075a20a469d54bd10a5272ccf19b40f4bf07866965b6760d5219c42374c07ee49696e6e7c31c26676c24534",
				MidState = "1584d7a923f9c651d99741acc7f866921a36e1546bd0259e82448fe487e2b55f",
				RestHeader = "6e6e694966c2317c3445c276",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 218,
				BlockHeader = "c3d7d2bbc3d7d2bb345d39e0a3e2c4e7195156d5b983fe51e644336210b3cb2382239f05b57a7f334bf32691fb62a821c4ff43e42f010738808d52397739e2f56a1bf2721b3098d7c3d7d2bb",
				MidState = "18024979a0e348c513b03e20d43137721ec72f0494146a51891be7b1bb7863d9",
				RestHeader = "72f21b6ad798301bbbd2d7c3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 219,
				BlockHeader = "44cf77c844cf77c8625b5ec21c24516e06fb31f97b81b858dfaf0146dd18caac665d1a69a88df0dcd5a75898cdc5161d2fda8bf49c253a3075e820d11bd7faa4b2925f701803156b44cf77c8",
				MidState = "6a6c750a3969a3b0774e1da41800482a04885cb1b0ad2f6769afc5a6ba454cf2",
				RestHeader = "705f92b26b150318c877cf44",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 220,
				BlockHeader = "129a8f80129a8f80ad0599af43a9d70838efead07fffcd677a040c02a6a8cc9aa30664cfb38ee079b95d49d19f9331d9b7095cfeff767140aef06aa8a0bd6d543403fe691ddb8b68129a8f80",
				MidState = "284d61436c7051a6b82e972af0c400e389600bb0b78a0ca852afa70f12e0a5db",
				RestHeader = "69fe0334688bdb1d808f9a12",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 221,
				BlockHeader = "420779d7420779d782fc1d00c1eac662c6fb8de74bdc0c67a7101aaa7b217bf6929ec8090d44028f318e120a417334798eda75b567e3173dfe77d35512aa93557b91e27272898616420779d7",
				MidState = "9ca761ee131aac04c888919ae3c8adcc1c0e111fd5aca6fc89ce9740fb62aaa9",
				RestHeader = "72e2917b16868972d7790742",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 222,
				BlockHeader = "3ced73fe3ced73fe89683bfb73f973224b8f48d772d9a74bdec3eb3a976dd886dd92225d24885ac82573f8dae95192e39652aeafcf4b9d1768688bf6dba2bd94c217d476a67294ad3ced73fe",
				MidState = "f9b09cb3f0e1bd7e943bf693b7b06ab737818fe618a70f7e9c683386e5b5c5b7",
				RestHeader = "76d417c2ad9472a6fe73ed3c",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 223,
				BlockHeader = "07c84b0807c84b08dd40f024bea72577f0628a5608ea1defba1227f3cd1d00a019333542a02442ccf39be348278fb5410e520de44c3220df0a366b6016ccfeda8d44716b42523ef307c84b08",
				MidState = "47291ad9c68f430a99eb58462fb505233a90ad598bbcd778c08ea32c1fbee142",
				RestHeader = "6b71448df33e5242084bc807",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 224,
				BlockHeader = "2c8037852c803785b58e84dfb28d303f944047f77fac8ab6e9c4eaa097edc2a21222cc93ecfc1c92a447ee84301bcd6a6c95cda60fec5cdd95d60ee397a3ce2e12f6597daf83b4852c803785",
				MidState = "29789454aadad2d62b5492155a03aff0de6119cd4bdf68f1851c482ed5fb76a3",
				RestHeader = "7d59f61285b483af8537802c",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 225,
				BlockHeader = "5ecc16a15ecc16a1ad71fe8cc796947308eaef00963afdd7dff5ed1345a9920a5f40a154f31b67ed6c7f934a7dbbd5f31813071eb3b956654e5b57c415c59c3ce9105042ee84b8cc5ecc16a1",
				MidState = "dff2dff6e64dad33ee43b8dcc09cd3f1246a6dfefb451f41c4c87f3acbf0c224",
				RestHeader = "425010e9ccb884eea116cc5e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 226,
				BlockHeader = "ce67351bce67351bab99228d8de9c40865f113fce00f263bf18b1e9677c57c9eaf97cf261a905ca07b07fcfec61c9add99687f338d4f05568c3f64f0e5955db438dce921ebdc5ae0ce67351b",
				MidState = "f88721f4b20e34f32a94ed31f189dd632b4b1d96d58f8c220f77545f63b63a87",
				RestHeader = "21e9dc38e05adceb1b3567ce",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 227,
				BlockHeader = "1c7bc2a21c7bc2a214382cc988f735257154ec33c48fda8d4926e4eceb8188dab145cc21aea4b3a3979614488cced64eb253959416cbc509713398bff5a9547d5b585473402a0df11c7bc2a2",
				MidState = "1b41f58c49a1ab16222bf93c4db7c6dc86ced04a9df4483e1c6d1b02cc5fb051",
				RestHeader = "7354585bf10d2a40a2c27b1c",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 228,
				BlockHeader = "b7a4dcb1b7a4dcb1e6773e417e13175d8a189ea18b8e4130f95d719ad1f8a152b5a2c618758d1bcd5a2e85bc3a6e1e0968da9935ab3b5126a9c2889a527d9e526ad3f864ac09ccc6b7a4dcb1",
				MidState = "7bb90118709a6b48bbb5115a9f03c98532e586cc72db3991f50d78490214dc82",
				RestHeader = "64f8d36ac6cc09acb1dca4b7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 229,
				BlockHeader = "10a02f0310a02f03a5c13a59c3e84d888feef520496d825bd268efe5552c496190c21e8c2ce4b2988d9b35db1125c6919a11e7504848484403048815de834a99bdef4331af55aa1810a02f03",
				MidState = "b417c5a5aa9868144fefd7ca8e0e724962b4800b0d3d3d9f7bc12612e8bb41ee",
				RestHeader = "3143efbd18aa55af032fa010",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 230,
				BlockHeader = "5eb4bc8b5eb4bc8b0e614395bef6bea49b51ce572ded35ac2a03b53cc9e8569d92701b883c7b38fc0601e7c21e447d2b92049ddb095cd9b393fae99066291cdf2d57ba29425c6f685eb4bc8b",
				MidState = "294f58b4e650d36430c74fd57075eea8245654b08f96df7cd7b4e128a85a4112",
				RestHeader = "29ba572d686f5c428bbcb45e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 231,
				BlockHeader = "acc94912acc9491277004dd1ba042fc0a8b3a78d116de9fe829f7c923ca462d9941e1883a68440ce0b84ca67aaed0f4cd4aef1c8928700dbff3a21df65646fd799f4100e7966b7e6acc94912",
				MidState = "f52f1e2d93af9dd466c965d420b25cb0b12cbe26e8f2e30617c80e05007bf39a",
				RestHeader = "0e10f499e6b766791249c9ac",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 232,
				BlockHeader = "b4b56b17b4b56b17e473678d23e255ebb76c6f360549d3eaa3acd37f63c0d3c665514e30656c05d57385cff4d9ee3aaba1de3ced790abf183a860d3037429bf0f85e7e0b0826c487b4b56b17",
				MidState = "c83f63660e3309e78da564b6db549cfeea176f4de68e00bfb962ca9c71a1e6c3",
				RestHeader = "0b7e5ef887c42608176bb5b4",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 233,
				BlockHeader = "55269b7355269b73771ad4b23519d4e7906bd31fb65366a4d9830df4fab54dd059f8c9f00abae9dc03c54285c63c50a4efe9b2aaa8ac56f6bce4ae352053a93d561cb515b7ace43455269b73",
				MidState = "f8f45d7e8e4fe5e32ad5505a2df81cb9c8a1f2ddef2360ed26dd45dbaeb1a174",
				RestHeader = "15b51c5634e4acb7739b2655",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 234,
				BlockHeader = "fd0c5240fd0c524073903bec1a70b7d06cda79e4d4bf57a8ab4490f738392707f25b361b4a16f23c49c82ef708ad8a54b66c0617e0fe9bb8409c00ab4252580e6894c8598c91b72efd0c5240",
				MidState = "5f55dde6ef23fff39cbd827a16fa427ae492b50cf077ba484ee248d945d5addc",
				RestHeader = "59c894682eb7918c40520cfd",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 235,
				BlockHeader = "9556e3879556e387f2007dd94232abb51f29feb3a955acd2a99eb1dbf207679a2b3e02cbb7845659c391780774ac3cff4a78d1480246a9af4bf681fcabc667c55c88bd6761e08e2c9556e387",
				MidState = "179382d9e97853fd53260a0414cfb08df215223747e4ec06d7d71531b5b8c6de",
				RestHeader = "67bd885c2c8ee06187e35695",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 236,
				BlockHeader = "e36a700fe36a700f5b9f86153d401cd12c8bd7ea8dd45f230139783165c373d62decffc68fdfbda49054fddec6addbadd5d9902f677acc466401da4c6c1a000fdfd5aa268e46b6f4e36a700f",
				MidState = "3e5d31d89e9f45f9c574611e46348cb447f95a52d040748cf926c8332eb59f8d",
				RestHeader = "26aad5dff4b6468e0f706ae3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 237,
				BlockHeader = "384b4f22384b4f220c1a17d89d59698efbe8c262fdd977450398b3950be5aea61b7d887ab821a1fac513451b65a12866bc5b7b7dc6b65639a7339a87d82552c168b0a26182ed9980384b4f22",
				MidState = "545dd50a3e495332885b4493fcd828271aa261d004e4159bdf1b78a76895c271",
				RestHeader = "61a2b0688099ed82224f4b38",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 238,
				BlockHeader = "a8d2003aa8d2003ad611fb0063ec1c3968cb723f586d1f123f41933e9fc5a6f38dbcf9a23cfe0a7d428825191dc54c993c6c85d20e8298a463ea951c1da643d15640dd00250e1cada8d2003a",
				MidState = "1a9dc4761ee36155c83406358b71d3377acef9567d009fe8489332e7017623e8",
				RestHeader = "00dd4056ad1c0e253a00d2a8",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 239,
				BlockHeader = "714dbb27714dbb276e07f9a04de446bc369e2ff0e61430b33a271c019a66297c1ba3460e38f2bf1dc36b7ed30db07c24219e714b91d123baf277e0ff03715c28514c0666ea315518714dbb27",
				MidState = "9171c755f4fce07e982e9b32694315825a3213bcd877cd846509e2fb0da7f50f",
				RestHeader = "66064c51185531ea27bb4d71",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 240,
				BlockHeader = "3f2e54313f2e54313f967dde392fe0d3197906964ddc232497faf9a39be3fb44a6ebbb5ff0332edfc754fc4d4e318938266c4d1bdb292497dd71fad143a8630be92f16243a45d0263f2e5431",
				MidState = "b5ac3da20de069ad550ef5f881dc169783c8c3669a4c25eece72e522175d18f3",
				RestHeader = "24162fe926d0453a31542e3f",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 241,
				BlockHeader = "3ddd27e13ddd27e106b9d2227477783c0b101743e9212343296194ffd1936e281f71d7a74766d79a1e4e0682f747bd62efab115a0f0174719071ad22ade87ecbb5215e103df9d2b23ddd27e1",
				MidState = "f377a8043067169cf7a88c090e171ec3e5b329c6c83c222a9daa2f7b5e090905",
				RestHeader = "105e21b5b2d2f93de127dd3d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 242,
				BlockHeader = "d90641f0d90641f0d8f8e49a6b935b7424d4c9b1b02089e6d99821acb80b87a023ced19e89ed83e4bc0c523a9989b3c629a340da16276f7735bfd27d8928763f42a4e85c818a8621d90641f0",
				MidState = "1d1ea9a340cfdba4cf923c701b6f80d840cccc885a1764ec89448aeecd7e1aaf",
				RestHeader = "5ce8a44221868a81f04106d9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 243,
				BlockHeader = "261ace78261ace784198edd666a1cc903036a2e894a03d373133e8032bc793dc257cce99921a27487ab16b6d06bdce38d735262b2abb8f5ae7d7b0a42085df08dca9654185dda62c261ace78",
				MidState = "6e092e907dd095739ae56f521283693fe2e187b0cf8057be7f927c9056bb1197",
				RestHeader = "4165a9dc2ca6dd8578ce1a26",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 244,
				BlockHeader = "9a7540479a754047bde68adc765c518c391c56150a4002b0a2ce5dbda2636d6aebe515f141af8d7643abdbd0ed7c169eaf029b0977e34c9d5686ae603326969225d7ea2fe2dcebc79a754047",
				MidState = "40f9b83a93ad1b63302af67e410588368ad19ae62cc2dfbf4aed58c592e1b55f",
				RestHeader = "2fead725c7ebdce24740759a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 245,
				BlockHeader = "b4f04ad5b4f04ad50421e270d37864c079e584b19d424997523979517623fd33c4abbaf0a52bd6bb92e519fb0d7feac57755df56b8ccc58e9e11dea7d8a648eb5f6d0e72acccc787b4f04ad5",
				MidState = "4ee630256f7a380c5091f5e13012a5bad131b0bf5d1eac9a7a2cf085c7f3b31f",
				RestHeader = "720e6d5f87c7ccacd54af0b4",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 246,
				BlockHeader = "0204d75c0204d75c6dc1ebacce86d5dc86475de881c2fce9aad540a8e9df096fc659b7ecfc4e91e509c4784363b3c06ba52b2d0319fa85762544252d63a199797e49331059cc7ee90204d75c",
				MidState = "b8f7994294fc9d053eb48927cc5c87e55e46b2a8cfcf1638368a3ec795aaf807",
				RestHeader = "1033497ee97ecc595cd70402",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 247,
				BlockHeader = "4f1964e44f1964e4d660f5e8c99446f892a9361f6541b03a027006fe5d9b16abc907b4e7e1bd4b38ba8b90e90002b085c5fd20b3baf51da27da7c62a5619e277e5baed444c7462be4f1964e4",
				MidState = "f558a170d0ca1b00101cf2b5e4a28ba02de00b441868fa4ebbfeacb6545234c9",
				RestHeader = "44edbae5be62744ce464194f",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 248,
				BlockHeader = "7134576071345760a99a7c128210c06ec6c7535a5fecafe5fd16ae20ff768dc3815924f923b6d9eef490767f0d5348e71be780bc09ce1ebb3bad8c2555a36a5dc6f1365e2ef8095b71345760",
				MidState = "93a16d48a84b4b3d5bcb0f518433d07e100d77d539055909636af632cd1cf50e",
				RestHeader = "5e36f1c65b09f82e60573471",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 249,
				BlockHeader = "b7635742b7635742e5cc8a48cbc6d7b4285e090af267a94da114189e05b24521aa17e296f59dc8b7e59a6a6b31ddc583cf75ec82976213d778694bd87843202d09f43210206c38dbb7635742",
				MidState = "444ce1a4829c1448cd28758664beb0e8f8189b8a6214028f0ab290d0f08f8c80",
				RestHeader = "1032f409db386c20425763b7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 250,
				BlockHeader = "0577e4ca0577e4ca4e6b9384c6d448d034c0e141d6e75c9ef9b0def5786e515dacc5df91037303e3adbee591f592b82b9896e6f69d4ab9fa643a23896fb855dfc5b3f56c07ec10220577e4ca",
				MidState = "a1b9ff5327eacb60ffc66daa5a59ad7ee7800256a818a42ab9db1d648f9a12ca",
				RestHeader = "6cf5b3c52210ec07cae47705",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 251,
				BlockHeader = "954c22ae954c22ae08eb50bd1b3a3a000aed5527838973eca0022ce3a3c7e39d62bae33c3a7fdb140c8f6ddc415413c571244997faa4f701cb4fec4532f312c14757f45814fbe0e3954c22ae",
				MidState = "fac80d44286aefaa21861bd6f9b21dfb25335b1fbec552e98075f80b381c683e",
				RestHeader = "58f45747e3e0fb14ae224c95",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 252,
				BlockHeader = "d3f5eba6d3f5eba69e7771a49b024e308e9dbf33462e55bccbbeb860a4275e1a7997637d8197c546bd52dd31e3314b83bf4adc6e29f3f27d9aa551d75b7f48dd4328995c23cde967d3f5eba6",
				MidState = "53b36981bdf1899f8a818831a7db86311098fc1c05f64db950263c2b83356e71",
				RestHeader = "5c99284367e9cd23a6ebf5d3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 253,
				BlockHeader = "f5ce5505f5ce5505d35d9c1adf29e15dc3f69d2344c5806ee3904c0bdfa59be27f44ce5ec0ba8a0cde06e2cf3612def3a0dfd7cacc86809731d0cae8f47bca2a47e20f7332e28b82f5ce5505",
				MidState = "e201096e648cb072d06f844f38f6e01ea93c055857b69b6af3ec3d23a7afc50f",
				RestHeader = "730fe247828be2320555cef5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 254,
				BlockHeader = "43e2e38d43e2e38d3cfca656db375279d059765a284534bf3b2b13625361a81d81f3cb59b32a868b2c3a1c4ff22801e4897dfe060725f3746e6df631ef7a3d833a18202841c619a543e2e38d",
				MidState = "0594c6ca8e87ba43fe4c1a776c89f8895205ea44c994c2e999682ffae7349aa9",
				RestHeader = "2820183aa519c6418de3e243",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 255,
				BlockHeader = "91f7701591f77015a59caf91d645c396dcbb4f910bc5e71093c6d9b9c61db45983a1c855454766dd91f92ef3fb8583eaef836bd7d53f92f0f60a9c49f881f1e23535f904b762144c91f77015",
				MidState = "fa48ac3226b084203ece521019d69b38db51a9629297e8701d12ca5145cfaef6",
				RestHeader = "04f935354c1462b71570f791",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 256,
				BlockHeader = "df0bfd9cdf0bfd9c0e3bb8cdd15334b2e81d28c8ef449a62eb62a00f39d9c095864fc55063b1eadd9633e98190c463bc3edac4bc4f4e5a00e03f751ff5cf94ca1400f23c31f13f99df0bfd9c",
				MidState = "57aed3d5aa339369876f179fec743a1ada5e10fb264230dd47bb781091abcc7d",
				RestHeader = "3cf20014993ff1319cfd0bdf",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 257,
				BlockHeader = "599e33cc599e33cc5600bf956ecbc0e3f76b2cbf3234ee49b002a50cef2ea159758dee05ff1567f18e45a9d756e2ecb43f9e32189ca8e8c051ebdc28c84972f5695c7117bcd9e2f3599e33cc",
				MidState = "8c4ecc5ce374a70aa43ea49f8c61ef478584176d910faa372cb2e0cdcdb026e3",
				RestHeader = "17715c69f3e2d9bccc339e59",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 258,
				BlockHeader = "f5c74ddbf5c74ddb283fd20d65e7a21c102fde2cf93355ec603932b9d5a6bad179eae7fc4ef236460b03e39f3db1ced6ad1c46bcfeea1697220fcac20ef2419a16902a2f97c29969f5c74ddb",
				MidState = "198a78e3efd503d1f393a8f2fcc5404f2d4012f9ac774cd446f65b78e7a78a93",
				RestHeader = "2f2a90166999c297db4dc7f5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 259,
				BlockHeader = "7a9817117a9817113c677d94607ddf1ab63ea9b1613c8ec7655dc0a9640bc3f25b47ee7579e0743d8d400650f3e3af480ba90b55e97dea214b59986a471bf98066981938ab669fab7a981711",
				MidState = "fd5e7e6d44c7cd65bbe2219fc35eb6308bd2e82e145e5c381f9d7d32100319a3",
				RestHeader = "38199866ab9f66ab1117987a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 260,
				BlockHeader = "c8ada499c8ada499a50786d05b8b5037c3a082e844bc4118bdf88600d7c7d02e5df5eb70054cc366e8c39ac58d23ca645b8de0e6353151aebf40a0fc1a3b1661ff17a8268917088dc8ada499",
				MidState = "1b65b7cb0334ce96ac58e815a55ba4ab26c392c123f5478a3aa1b6cd342f4ffe",
				RestHeader = "26a817ff8d08178999a4adc8",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 261,
				BlockHeader = "63d6bfa863d6bfa87746994851a6326fdb6534550bbba8bb6d2e13adbe3ee8a66151e5676dd9f49626a1dba58a9192b318b86505bc2151b9423da71a20dbb77a66cde66736c33c6b63d6bfa8",
				MidState = "035a38a4d2a08c67c0cefa0147c5424f6982a78a825a2b9f8e8c4b9618e2cbfe",
				RestHeader = "67e6cd666b3cc336a8bfd663",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 262,
				BlockHeader = "59de42f859de42f87e98f031fa62866fbc456c55b6598ef0dd95f98147914be4525280b60ad549d547bfe5185efe6c1f8e1437e9a592c6429d8177508001b8d849a52c17255cb02d59de42f8",
				MidState = "94e6e511d7a6a1c64afc9068d58c6934f368cf49b0b51fa0f4a4b09a54c49701",
				RestHeader = "172ca5492db05c25f842de59",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 263,
				BlockHeader = "7a5dfdad7a5dfdad743492d9cabff5d4a0aeb136ebf4a85ca9e31973d84a3be2e9e90c943797fde84bb5ace6f5bd9e8f80f1d4247c0fdca3202bda004d9df9dcb93b2600ea21622d7a5dfdad",
				MidState = "8d8d62d4abee55b4b697eedcec3455593ef6bd3e66c655db4468c7c3624ae5f7",
				RestHeader = "00263bb92d6221eaadfd5d7a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 264,
				BlockHeader = "b2d6d981b2d6d981dc35c21ab6b152e28a32d05d130357646e4560e7c9979d838d035070e53db720138d36bf2f19ba3fd118828a18acd588459b611c98bf092e48969865460a50abb2d6d981",
				MidState = "ec49410ec5fd14cd15dff3dbafcd3ebbf0304af8dfd88f09ce437295dafa03be",
				RestHeader = "65989648ab500a4681d9d6b2",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 265,
				BlockHeader = "0ef406d00ef406d0ada196deadb422605055460297b4a7d1785a7259d5474142f58f01d45d2f973d41506a999f6377be65d2c727fa0f644e98b3e8682c82c6e9d317bb3e1315529f0ef406d0",
				MidState = "8b2591021803c2b5d5706e3fdfcf0f9e0a48ebfc4d61c8f5d186873ab6607acb",
				RestHeader = "3ebb17d39f521513d006f40e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 266,
				BlockHeader = "35c2491735c249175481460014f62f39f35b5c3c8acb58b2da67449951b1da99c36a49a63efdf30c98727391e173fb849660bfb53593f9d68576085272682b8e901d6f49ee3911ea35c24917",
				MidState = "54fe44f4b92fdf053652c65e6ca503f5d2e1e9a59d53541619f3d74c723881b5",
				RestHeader = "496f1d90ea1139ee1749c235",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 267,
				BlockHeader = "83d7d69e83d7d69ebd20503c0f04a05500be35736d4b0c0332020bf0c46de6d5c51846a2bd60589f7630590a48e56ffd439556d296a02bbfbc0f56fff8ab18fa1f246a1ad9f7cd1783d7d69e",
				MidState = "e5b6ed1e4dffc08a5767d579d71a48a3c95dd9b33f393a58b91fb3b690a0fbac",
				RestHeader = "1a6a241f17cdf7d99ed6d783",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 268,
				BlockHeader = "561ee97c561ee97cf3cff8698bf784382558f844b959b708df341686467991241578e5c475a557d6585f21400fc9574c2a7152ad94275dad80f887d87a421cfa5fb8c84189d54baa561ee97c",
				MidState = "e5383d0bee8f01064ea9be12e05f2fc91c635e98086c2fa5d14cfc74b4dfeddc",
				RestHeader = "41c8b85faa4bd5897ce91e56",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 269,
				BlockHeader = "40c8a77340c8a773f0c1777fd30e028895a24328a3d694776e79f2544a55a13d9178768b410ff7c13e4f11b1dd0ec003d93321d62965a1507aaad2d22c7a7d9309cf4065caccf0e840c8a773",
				MidState = "3d2f91c1175c6f94f50ce428b6416123c8c407db6d26ac8029761b1456997b4c",
				RestHeader = "6540cf09e8f0ccca73a7c840",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 270,
				BlockHeader = "db9a5c60db9a5c609204c1d5b75dbb705bb50b38a9fbe5dcee6498379a6a93e1bdc74101fb86eb20a7d73bdfd14be93d627b9a2093815d2956b83070ca6aed96b80f2f576056fd62db9a5c60",
				MidState = "172f9faf93f4e163604ab511fef6d2d642002538b3a57379bdc1afad68a4c0e4",
				RestHeader = "572f0fb862fd5660605c9adb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 271,
				BlockHeader = "29aee9e829aee9e8fba4ca11b26b2c8c6718e46f8c7a992d46ff5f8e0d269f1dbf753efdaf8e21b0635511b65963dd0489ae0ae71afce98a8e9ee0856b948b38a199cf098623b04c29aee9e8",
				MidState = "84734b76f1cf01e37143c4f986f0ddcb65ec6a45c9893d2f5ca501a23bb82d4a",
				RestHeader = "09cf99a14cb02386e8e9ae29",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 272,
				BlockHeader = "0fafd5dc0fafd5dc00551a7375df67f3dc6bbef0af76b7ea37bdc80a066e8f141403e32ad1cc6ad5e79a2262afb9a45baee409b2876532d83ae07bef9c27b6b003d3152966fb15c70fafd5dc",
				MidState = "d7a6dc7bf23bee44834d7dea8cffeadbb3706ba43a610c458d34aa15767aa5d2",
				RestHeader = "2915d303c715fb66dcd5af0f",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 273,
				BlockHeader = "38e5fbd138e5fbd109d6340166635017650e103eae03687c10c346709c2e373bd9c66406d2dd25a0069ed0eaba78100a48e0b3b278bbe705f724a8a7b8502737efdaf210bc2990b038e5fbd1",
				MidState = "d9633a851931fc3a01f607f49e8f4fc6b64e5116cddcf31bbf0dae5e5b9f5dbf",
				RestHeader = "10f2daefb09029bcd1fbe538",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 274,
				BlockHeader = "86f9885986f9885972763d3d6171c1337171e97592831bcd675e0cc70fea4377db7461025c08fa53eac86920ec14f3beac3cf3880afb620515d70eeaf67c72343064415d46a7efce86f98859",
				MidState = "1f3e52b1ce6284e80be3a1b43a0ad184547582adf7ecc47c3f67b225e841d118",
				RestHeader = "5d416430ceefa7465988f986",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 275,
				BlockHeader = "cd2ff97fcd2ff97f9e82ff4025f1d92ad26abc03f97081931c7b7acc5159429c4a2b5acafa10d6b9ebce89fb8ef8ec1df150ace71d3313818a16d0a3c5df83783744036e1eec4083cd2ff97f",
				MidState = "e3a2d5600987ce000ddad6513ce93e7f2a41048d7df7d283d290092f521540ae",
				RestHeader = "6e0344378340ec1e7ff92fcd",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 276,
				BlockHeader = "1b4386061b4386060722087b20ff4a46decd953addf034e474164122c4144ed84dd957c67603add8a908179c96b02648a5fc1ee54ffae5d1e0fb76e4573569ace47ed13b67f77cda1b438606",
				MidState = "db4d10347182cb0f462ba950ed63eda7a0c99569be596959a32b98e438a9b083",
				RestHeader = "3bd17ee4da7cf7670686431b",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 277,
				BlockHeader = "8d65259b8d65259beaa3b4bd74a23e7946e27bbf5822c2a72c0b8a0fed22d7b958983f310f53c554f5b407931bbc8a016cf4046f5ce6e1ac430c7a9e50a6936f291e982c55f212658d65259b",
				MidState = "731ea53b07382c61b6c930993f697f798764f42d164ccdd6491942878c5cc522",
				RestHeader = "2c981e296512f2559b25658d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 278,
				BlockHeader = "760b6124760b6124fa461d95ca3b86071302ca2f13d1cadd3971edd6d213a9554cde48ec1f497d46596634ae7cc1c49a265f3d075132eb8f31d3606c9a9e39899994af3f209b6629760b6124",
				MidState = "33edff7551a5189b32b1517dcf0d995864d482349f46458cab45c460ad2238a5",
				RestHeader = "3faf949929669b2024610b76",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 279,
				BlockHeader = "a676d390a676d390ce77d31e0357906a7372dc910c5bbdca9e5e34bf1969d38c5bd7d217175b6378ee703e7eb84d6435895e49ef10d5fa33e306e7d07150e100eaa1470d8654680aa676d390",
				MidState = "293c61aa495147091622697f598fdfa55d715e05beebad054eeb3ec24dea14a4",
				RestHeader = "0d47a1ea0a68548690d376a6",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 280,
				BlockHeader = "f38b6018f38b60183717dc5afe65018680d4b5c8f0db711cf6f9fb158d25dfc85d85cf12c9df8663c7670209807e1ea09ecf6ec034f9d4e65e25d90f158ec2d6c0f00935530fbaf1f38b6018",
				MidState = "c81fbefb87d304005892ec6042dc3267443661bfaa8ccfdf9f4f028dfe00fb1b",
				RestHeader = "3509f0c0f1ba0f5318608bf3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 281,
				BlockHeader = "47fd833f47fd833fe7058a94da5422e3fd575e4821b02ee695b2401ae94a1d6f9d1c78c317c805484abe71ad0698316595e64623849b2972b0d221767ea721f64b95061dc024051847fd833f",
				MidState = "fbba2110a204a3172967c1c761655a31eb031a0674d1c84bdd4912549a310ca7",
				RestHeader = "1d06954b180524c03f83fd47",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 282,
				BlockHeader = "b91a3200b91a3200c7fb9747a5ad4b2808352263f73d2384bc69fd88f7109c05e99eaf0e09fb96d7bca2f9e0c30a119af1627956f1f8a41fe24a3d53ef69af0358733e25f1895200b91a3200",
				MidState = "fae520b2754ae5d485edef7c578d320119c5b3fcb508a4547e7e48ad77d4bdac",
				RestHeader = "253e7358005289f100321ab9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 283,
				BlockHeader = "d780575ad780575a5f9e63b15491067b529975f80f984f4255b8e6052a4e4a7ce35eb00963ab8bd742c487b5a7fbe8e70c9ffe26195d024678554384c6fd59b09327f20eb51d27add780575a",
				MidState = "c533977351bf94fa9087f51658b57aef7a80b46a5d23ce2876bafea83bc3b82c",
				RestHeader = "0ef22793ad271db55a5780d7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 284,
				BlockHeader = "4037d37c4037d37c5717a24ca10f28784766d45467e4630999a116357daca6b1a30583cace0535a74b33269ba9258d50c696b4f946b5823064f0f9dbe7a366a2b7199b6ed240689b4037d37c",
				MidState = "08fb34ab785dfe5fcbd9fe4135a8267ba693644e20d3f7f837a1652b97c460c6",
				RestHeader = "6e9b19b79b6840d27cd33740",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 285,
				BlockHeader = "7e5914ed7e5914eda21d79f17bd9324172d43de8e29d0cd7ec2dbc827f305da5360f8b2b85cdb0d7105816743d0c8c44db368ce81d5c023f2f2991016876740b9c2c89518d947aa27e5914ed",
				MidState = "e0ef6ec145d681f29c2785b9e1ce5f472afb55a56df39d01ad5fe4d2ec32bd8a",
				RestHeader = "51892c9ca27a948ded14597e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 286,
				BlockHeader = "5a7aad5b5a7aad5b42ff8a44704faa49ee16b6fffeab0a176d987391a333f890f9240f0b36a11003c4370019738d5025f4e81fdc672e9486c8cd27a24bff516c7ddc041da79f18065a7aad5b",
				MidState = "e3c9e78d5eb90f149b0255654a87ec1f3c034042a5c4d195a90de8f219f33f8e",
				RestHeader = "1d04dc7d06189fa75bad7a5a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 287,
				BlockHeader = "298cd849298cd849c8b29d681b43c3a5b8e9f683a54897bb079d47929163c7f64766e986462df22fd3f64ef396b62a3c4c00a5bd214928e7ab1cecd4978d659bd52ace27e35591fa298cd849",
				MidState = "0274dd062bdb7ae2c7ea5fd867cd71910998c3952eeb1e0b60234c76a267cb0c",
				RestHeader = "27ce2ad5fa9155e349d88c29",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 288,
				BlockHeader = "d0b0f176d0b0f17681ee6d355b2286a093f86b65f278ec62c84d66740e74baace27dd139d9ebdd382a15011d5b0906e303a0428cb7222848849f0eaac6ff4a1d5242631939bed99ad0b0f176",
				MidState = "c9aadc54937e69c64d591fe1ae85cc3e7315c5bbef53a872c4c52da5ea17f7b6",
				RestHeader = "196342529ad9be3976f1b0d0",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 289,
				BlockHeader = "d466c097d466c097bc8529f565ba45c27eb03509d5c7bf4f2aa325187d10844da00f4051807f73d6766bf6bad690ae8aee4bd172714738b177636305f28290e7f8d9ef00e0b46295d466c097",
				MidState = "96d9289161f4937ebca694a6f92360b702b9d91899b94bad875fcd5211876a3e",
				RestHeader = "00efd9f89562b4e097c066d4",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 290,
				BlockHeader = "ca9b117fca9b117f2dc59e1f973e5f869b1179b2028940d9d622391e5f21d4620e7e07122b5125ac1f66d3708ca047dcef8515e882f105dab1a517c25520878ee77008054ee9dd80ca9b117f",
				MidState = "42042606578dbf225dea549771213a2e70cc871ffef5b68bbad31660dbfc325d",
				RestHeader = "050870e780dde94e7f119bca",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 291,
				BlockHeader = "17af9e0617af9e069664a85b924cd0a2a77352e9e609f42a2ebd0074d2dde19e102d040ed17eeb92f817993d8fce2aa33f3d64213add80960be4c22ae421bc252aee837b892c12e017af9e06",
				MidState = "502ee87466c2486bbfa09a2c0b7ca15d378e838e923440195566eae0d1c43bc3",
				RestHeader = "7b83ee2ae0122c89069eaf17",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 292,
				BlockHeader = "65c42b8e65c42b8eff04b1978d5a41beb4d52b20c989a77c8658c6cb4599edda12db010a718d2424bad2e598cf78f5578bf59cd755959e7ad2544b545103848d2e854839e5df578465c42b8e",
				MidState = "18ee93effe2799d92aad509481e66c4020a82b3fc4f59a21e369aa5df07d204a",
				RestHeader = "3948852e8457dfe58e2bc465",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 293,
				BlockHeader = "c54c2e78c54c2e78b9fa10cc2e95c178e3292a11434040411c95f46412c53fcd0e3f904c8fb45115332faf0da86cbd3a216ab799c613c49b9edad574e3de2a8ab19607032fda1110c54c2e78",
				MidState = "48c594e1eb99d9b44fe3789737234a89c63bd79e2bb434ea4e9f15df39efe3d5",
				RestHeader = "030796b11011da2f782e4cc5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 294,
				BlockHeader = "0e6300e50e6300e591cf156a58c21f5db041b01c9b4ebe4820b5cde0ac70485dc291343bfd674da0548e597c427d52898a297877ef708925fafcb175e56e8deebffd27174fb1c44d0e6300e5",
				MidState = "564b21f72327c752fd47ca563d393b9c76c7162ebfa95b6abb45f84db617313b",
				RestHeader = "1727fdbf4dc4b14fe500630e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 295,
				BlockHeader = "f7a0a77bf7a0a77bccad301d49eb72b2d5683bc145cdd83c278620e406a46d11c89c2b2d50fb2342373f40c20c5dd8f24849c33f288447160127fb5bd2450b1a8855310dea36a8aaf7a0a77b",
				MidState = "113cd1bf3919ee85ad685fa3fdfea824290ab239f529fb1e439365de23474743",
				RestHeader = "0d315588aaa836ea7ba7a0f7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 296,
				BlockHeader = "9ade2fb89ade2fb888fc6654402f0817973ac60d5501f0e9b302b9a94c0ba6d192defe383e383775e7316f35e69f2e516931690ae95129c7790bd5e173ffb0fb1ca3f10f3e2c27169ade2fb8",
				MidState = "eb416bba7e8df2c198fea17152a31ba2a89666fd8b485b0e9a9a05c36ca36a56",
				RestHeader = "0ff1a31c16272c3eb82fde9a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 297,
				BlockHeader = "e7f2bc3fe7f2bc3ff19c6f903b3c7933a39c9f443980a43a0b9d80ffc0c7b20d948dfb343ab79d70e3dbf638e019b25e6bc2924cb7072ad0b9a41890143b5c42760eb56c7bc1cd6de7f2bc3f",
				MidState = "0dc81faca3cfff36707e40591766d5a0519109477537c31cb5375dcd60e04d47",
				RestHeader = "6cb50e766dcdc17b3fbcf2e7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 298,
				BlockHeader = "85ed587385ed5873e6ca800fcd637cbabd86bc5179a180f6ef227adcde1a40942528f0f95aff54346cbbc192448985837736841345558de5a19fe811845c1c0e27e8b26187edcd4585ed5873",
				MidState = "94310e09b45a541d5336bc0c46faccd136dfec07d112a63bc8d374a291d46b70",
				RestHeader = "61b2e82745cded877358ed85",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 299,
				BlockHeader = "0f87ba6c0f87ba6c7b79cd97eaa158e69dbf83b34d0a5bbb91d52eb2cfc54244508d594705a1859155a107108d87315a999cfc897d34c5c77c5b645c15c6df4f89f6a139d497ae450f87ba6c",
				MidState = "00eedf8a008aa4b3cf4de75ab1ab315b7330885d536dc84f9740b7858528bc7a",
				RestHeader = "39a1f68945ae97d46cba870f",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 300,
				BlockHeader = "5d9c47f45d9c47f4e419d7d3e6afc902aa225cea318a0f0de970f40943814e80523b564281d41eb0e3cf88df3a75031463db495b7a1e2d63ed096e207e80e63d1830e8738aca34665d9c47f4",
				MidState = "216f37a5a9411f132732d74ccb15ab0db3fbdaeafdb8a11a0b3f51735eacdf54",
				RestHeader = "73e830186634ca8af4479c5d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 301,
				BlockHeader = "39a5eb4539a5eb4593fbf27e52a3bba025a39d5e4293336c9278b9f3dd355490b898c024fc99c2e75727bc010a4ac8a570053740f960c8c4d7edd757aaf4da91f46b94674e0aaf6039a5eb45",
				MidState = "2f303283f0acc31b117f01225ab29d2b669f41d382c04fabe7145e3bf7a8abf5",
				RestHeader = "67946bf460af0a4e45eba539",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 302,
				BlockHeader = "22e392db22e392dbced90e3144cd0ef44bca2803ed124e609a490cf737697944bea3b6167526678119250ef781368ca2c15e4d3647460a9ba072975e73a0e55224b90810926a4d5822e392db",
				MidState = "c3415aeb44d21378237aa05680974a23e59e1b139982e65e0a5db0cf197953b3",
				RestHeader = "1008b924584d6a92db92e322",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 303,
				BlockHeader = "07bc0b2607bc0b26b2e6c7a1084b12f52c70762cb96dabc86df5bef5246feb65979e35bb864717d4e80d4b7cedca30c46da21d0208786063791ed7107ca2937a15694c0f3f087d8407bc0b26",
				MidState = "42c3bf2f1938822d5e5c380a0881a07e174a1987c66098b5fc436d67f4498a21",
				RestHeader = "0f4c6915847d083f260bbc07",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 304,
				BlockHeader = "f0fab2bdf0fab2bdedc4e355f9756549519701d164ecc5bc75c612f97ea310189da92cae092eeeb8f961f08d7f8539bb3a29d2bcda237a0c7d1ff6ee5c78d30071ac091c959000d5f0fab2bd",
				MidState = "1bcb308ce25265badce60b3bd2714384c6ac589ad3ea96eac44e41e8e3b932a0",
				RestHeader = "1c09ac71d5009095bdb2faf0",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 305,
				BlockHeader = "34b990ba34b990ba7a5847b23de301cbf47c8d835bef7483d9914066476f24bba9559f7b872ae182bed2f813ccb960f030ee700759d4073e4157f57a41f8f48171db2d7696b8df0234b990ba",
				MidState = "e08d3f9f7f6a498d41913e90a9cd66ae02646189c1be02756bf1f7a289e52d5d",
				RestHeader = "762ddb7102dfb896ba90b934",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 306,
				BlockHeader = "84f10b3084f10b3048ce3211af25a1cd83ee2e8f784b92e04dc61cf0dc5a0a39a8f564a9beabf89b94845688f7737d05da53b9ab9ce04867d228a6ef1eaf985ed75f332046892a4c84f10b30",
				MidState = "56459d59add7eb318b166ea2d11b83a67b2770579f23c5278258c04b8d685345",
				RestHeader = "20335fd74c2a8946300bf184",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 307,
				BlockHeader = "e3ed2740e3ed274019199daff59fae031b60b9d94baee30ee547dfdf08282e6f4a14ae22884cfc923ccc0dc622e2ebe8fc3286d7ee6f99ba6a209a1b56abb1d54658ad178ee41268e3ed2740",
				MidState = "94120aceed6bce7645d4d5424b7bc1bf0d3cbfd015b4f2f76be83efdc7ae3ff1",
				RestHeader = "17ad58466812e48e4027ede3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 308,
				BlockHeader = "30482273304822738149fdfb47e0b07cb8ec9704d41a76185f6002e7022ff55e2557f5054cba213960389aad2ea6ba3e40377e7c9e7327e634da900cdeb425e19f00b038731c9ab830482273",
				MidState = "6614b34685c5582528563c62103b29b1f27b450e8acd1b0c8de2f9a0c7f8479a",
				RestHeader = "38b0009fb89a1c7373224830",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 309,
				BlockHeader = "0ccefa040ccefa044553f893975d036a1215ca77fd6bda2c7993f787a1358d55a7506fc1be880b17f5e45cb4ca3519ee549b7a3df05df2d815a28bf19e480f0e5b140022e224f35e0ccefa04",
				MidState = "48a0dda8a49298641b3293f85d73ec0ac992083726b33023239b5b76bb7bcf15",
				RestHeader = "2200145b5ef324e204face0c",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 310,
				BlockHeader = "c997e220c997e22091ad994fa60a21516350d8efef3f0cfbed9cd72c8227aa6de7634e3555128d69b909ea32b1f85ced8f0968ee24961c80ea4bc888132b9d8c69ea55218e5c15dbc997e220",
				MidState = "eb8e5a0761c5d98cf1ddcf250cc25ab20eacea5298eac7c760f75ad44055e137",
				RestHeader = "2155ea69db155c8e20e297c9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 311,
				BlockHeader = "16ac6fa716ac6fa7fa4ca28ba118926d70b2b126d3bec04d45379e82f5e3b6a9e9114b30c9fdf0a5b59395702fbad3bac4342b4173668ba5c897979cf221f8cd0fdba654a7e729b416ac6fa7",
				MidState = "38d148cb50a691c59eb0fb4b5458f9824b4b0ccb451ae8232a5fe03e2c2c0036",
				RestHeader = "54a6db0fb429e7a7a76fac16",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 312,
				BlockHeader = "c8f54a07c8f54a0717e909770629ab23d4fc8d67cbc8172899b7cb83cf83b920ee359d17b1e474cb8c0bb491334748d447349d2f9bc48e7c943b537fc91b27ac46a7762cca1aff38c8f54a07",
				MidState = "7a99577a5e4d557b4ed941d5eab1c63565097b446e144c0954519dbdf8a6f6ae",
				RestHeader = "2c76a74638ff1aca074af5c8",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 313,
				BlockHeader = "631e6416631e6416e9281beffd458d5bedc03fd592c77ecb49ed5830b5fbd197f392970f9273b524374a437738981bf353b2c8787ba57fce98da4bef562924b526bdc612fecdde5c631e6416",
				MidState = "5610bef3c3c03b97ed709ead5368fa730bc0152ba437036abde089a0c191b556",
				RestHeader = "12c6bd265cdecdfe16641e63",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 314,
				BlockHeader = "b132f19db132f19d52c8252bf853ff77f923180c7647311ca1891f8729b7ded3f540940a7b89ed15bc92402762ceb735641254aadd77b4252bf6686b934c3024b1764c361ee557b2b132f19d",
				MidState = "d355f32e8fef5d2bae52444deb2773dcea4c01a0a5358923fd90271d7bc75e80",
				RestHeader = "364c76b1b257e51e9df132b1",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 315,
				BlockHeader = "5d6b154b5d6b154b1583f2f74f7ffaf97ed745d5995acb95e1872e11dc6cb158dd09bf4efd96e31ac5795983c9c891fc1dee66e4623e34b0ac35ac315fbd687a98039319ebad73b85d6b154b",
				MidState = "c584f3cd938a7c6b802165999aa6750d0fe03de0355c3c0e0edcc38165cfb6ea",
				RestHeader = "19930398b873adeb4b156b5d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 316,
				BlockHeader = "d0ebfda8d0ebfda83ed5620125187d714df6fec73602332df8b5f816d8afeb2d70711ca33ed51e10fbbefdb93a5b7196bac6d52428f72d2bed70595d1f4e3944b806dc2e584e34e9d0ebfda8",
				MidState = "32d45f87062a9a0b88d6cc157b475db1c6c5c52e4e4a8a88833d5810a440c7a8",
				RestHeader = "2edc06b8e9344e58a8fdebd0",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 317,
				BlockHeader = "b929a43eb929a43e79b47eb51741d0c6731d896ce0814d2100874c1a32e310e1777c1296a11b71e691172d504e2732af8473c7b4bc7399d785d94cdc1d593e7b7b7bc777ab273edcb929a43e",
				MidState = "2ea8dcfe0544c8c20499820e33d7be0be228f42b56f9deb4f1e1f44de0246635",
				RestHeader = "77c77b7bdc3e27ab3ea429b9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 318,
				BlockHeader = "23fb29a823fb29a8bf2ea4cd6bccbbb3d5a2c68e130e075c301cf6316b9dbd37837b682083eba4bed4266f671caaf8c5e18ed71a1b419a56f4bab17f03ee8a3f4ba10b2773fd6c4523fb29a8",
				MidState = "554d0479b66b30a0372e661b20e75c5ad734f44d0a34177afa246567543d34db",
				RestHeader = "270ba14b456cfd73a829fb23",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 319,
				BlockHeader = "be2443b7be2443b7916db64561e89debee6678fcda0d6dffe05283df5215d6af87d762170c3a0b45bacb02bfb0f0d2d5768fb0391bdd3ccc2433a2483079bf368e70f215c8dfd609be2443b7",
				MidState = "b818bc04195224da0aef4d330855a39439d67a5f95f681e3802e0ccc99352ff4",
				RestHeader = "15f2708e09d6dfc8b74324be",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 320,
				BlockHeader = "703f739c703f739c94466ad978edbc817bbccdd81e57ee28530d773a5260c095c1d644ae6888a9ed7fe23c0a401e6aca3e63609182dbe2273b4ec94910ccf75b394cf76ca5ad278e703f739c",
				MidState = "694beae64838f8b69ceb14e957f9f07ad70279e39f04e8af54a189fd0bad2d54",
				RestHeader = "6cf74c398e27ada59c733f70",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 321,
				BlockHeader = "b6d5c493b6d5c49347556bcce1b045417bfde6933320aa2d7c649f566d4f529219fe64693308c6d988f683114d4731b21e890e3d5bfc28cd04a1f05c980e25faae5cc274fc1067d8b6d5c493",
				MidState = "25fb77d0c7968afeb758b3dd1a6f94c3de54a1327c71c55dd1925399ddabda61",
				RestHeader = "74c25caed86710fc93c4d5b6",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 322,
				BlockHeader = "eb45249feb45249fd3f2a2caa47f51d5f28f7d1e2b4366b276e82efc91a1b416ce03bd1edbccc915f79e9baa8f870a7844788163affa45ebf542822f830d3da36d96e8481bc606cdeb45249f",
				MidState = "c0388c9bf2562e1b7453ce0188e7d76f5b836d75dc49ad5154b8565bf6e513b3",
				RestHeader = "48e8966dcd06c61b9f2445eb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 323,
				BlockHeader = "876e3eae876e3eaea530b4419b9b330d0b532f8cf242cc55261ebba97819cc8ed35fb71531df70f1f7bcabc01edc91db932181cf6460ad0a199d4ec96cbef4417ae25837558b0785876e3eae",
				MidState = "4d439b07eb22e522633eb1ffc147d7168530106d2d0d47b00c94963f998fe5f5",
				RestHeader = "3758e27a85078b55ae3e6e87",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 324,
				BlockHeader = "ca28884cca28884cf2c0fbe82883033a484a171b456e4b191d78f002864ac321e7c9339418ba3798f7a0957ebc2e6a78c781fccf403c759431274406669599454c7c502dedf26867ca28884c",
				MidState = "df169bc8ed2e531002358a8c23beb4bd4448c1ae3f5b01de79b983011a1f956a",
				RestHeader = "2d507c4c6768f2ed4c8828ca",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 325,
				BlockHeader = "183c15d4183c15d45b5f04242391745655adf05229eeff6b7513b659fa06d05de977308f9231732630434a2ee3f2d0bc1024fce7fef40fedf4e7ce3c93e5ba4746ae6d06883f7d98183c15d4",
				MidState = "295272b14462ffbfa369def49da2dcb12d0bf45f98e912012c0f31bd48e5b84a",
				RestHeader = "066dae46987d3f88d4153c18",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 326,
				BlockHeader = "2236a4742236a474a7c2b6b33fddb6ea368f6102f35b7500f03d51920a644bb368fd0383f355ab45a79fe02f92546b06cb470eb8808f52486eb5d13035239c142c015746fa6663142236a474",
				MidState = "4a76548cd3536cdc13b5c01b5adb5e15667c516aeeef5b2b0f6d5be72ce55cec",
				RestHeader = "4657012c146366fa74a43622",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 327,
				BlockHeader = "704a31fc704a31fc1061bfef3aea270642f13a39d7db295148d918e97d2057ef6aab007fed1aa7fcd2c11b462224ba07f680927875206a5aa3e44687140a996cd8faae2c598d51b6704a31fc",
				MidState = "a6827040dd4abcf3ab53375892ef3ab0729ca6aaa67a4d9893a140fb1d10ffad",
				RestHeader = "2caefad8b6518d59fc314a70",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 328,
				BlockHeader = "eb5c0f0aeb5c0f0a9e94d5db6acd26c1f011fad3186d85f0c52ceb16fbc2e9a65e413dd47608e398be003ed085df03e92b5110389f0295f5d3e3beef7c895c7dcf57886363a02ecaeb5c0f0a",
				MidState = "6646cdf6694e18754eead537558a44838414ddea286e658411b4035fd980daa7",
				RestHeader = "638857cfca2ea0630a0f5ceb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 329,
				BlockHeader = "878529198785291970d3e75360e908f908d6ac41df6dec93756378c4e13a021e629d37cb114758d69e98a7176689acd1b177b40847f0f80df47b0bde62880206e0256e3258ebe5c187852919",
				MidState = "b17533d9dd055537fed0e2ab2033cd00ce1cf7e7c03b22b43ac1aa61e19c8a52",
				RestHeader = "326e25e0c1e5eb5819298587",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 330,
				BlockHeader = "d499b6a1d499b6a1d973f08e5cf6791515388578c2ec9fe4cdfe3e1a54f60e59644b34c731a1dd2aa4f4bdfffbacc726e5ad67a1725357a9498eeaeea736174db9000376c023f775d499b6a1",
				MidState = "f4460f9cce34f503c7976441e6487feea3ee8d5bd81299f57dd12c919a5d200f",
				RestHeader = "760300b975f723c0a1b699d4",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 331,
				BlockHeader = "2b8cf1252b8cf1256b69b8b3f7c7ad2a12f63d61ad27dcd5b9b9494afea25dddc06e0be1c1ad884870c6764ec1db0695bbe47db260bf625de2dba4af90a8182344b34256d15a635b2b8cf125",
				MidState = "abaf3b8aa1f47ebfd1cd15cd6c7e2612a63a04b92332def9e852154b73823882",
				RestHeader = "5642b3445b635ad125f18c2b",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 332,
				BlockHeader = "f9b405a3f9b405a37f9c1eb6d94ac2615dadddc95db3d1f4844064e86384fd27ce0ab7efc32ec24cab103398e02d2002ec66f6322e0a47f51eb42ec1b67b78cfc5379e2b7cc96478f9b405a3",
				MidState = "87660601ceca657633c7e5649be2187d434c7ab145c0fd50d94403d773dd1767",
				RestHeader = "2b9e37c57864c97ca305b4f9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 333,
				BlockHeader = "e2f1ac3ae2f1ac3aba7b3969ca7415b582d4686e0832ebe88c11b7ecbdb822dbd515ade22111f3b5741a0de348f29e09b7ab99a93102ffd0a91325d20e5795f53b1ff9735d53bbf3e2f1ac3a",
				MidState = "9ba30c1bf4693afe06074bf746b07921aa0ffcadf6e7e4d02d9e4caa2b9e7514",
				RestHeader = "73f91f3bf3bb535d3aacf1e2",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 334,
				BlockHeader = "5edfcafa5edfcafa208b1e926d3900f887fe51373cc9294602dd1cba2a5c67ef231c8e23721e9fefd5365662895790d076183edf572fb99e845efa59e515e11c86ed7231c118c0a95edfcafa",
				MidState = "f2c836f57a7d7e9622db241f78ff99735775b80b1094b0eaeed46486d0fee6e8",
				RestHeader = "3172ed86a9c018c1facadf5e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 335,
				BlockHeader = "76c99a2d76c99a2d9dfdbd40fc705814d446c8cb12822239c95725b6192b7f366a2aa6b604eb48200901c317bd8acac0281652d2f578b1521a6b868c3551a94073addf13edd4170776c99a2d",
				MidState = "d37a707ebd2aa3d898fafedd340d12b256ca332a7d5287acbb60d53c02abd876",
				RestHeader = "13dfad730717d4ed2d9ac976",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 336,
				BlockHeader = "c4dd27b4c4dd27b4069dc67cf77dc931e0a8a102f602d58b21f2eb0d8ce78c726cd8a3b16bef6b03c1701c53205fbe696b703d80327719d553aa40a20d58aabbe6f7b838c5dc8d4ec4dd27b4",
				MidState = "827988cf8d7f23bb88128e533bcd363781773e68270e0d0c9a236b7a88644771",
				RestHeader = "38b8f7e64e8ddcc5b427ddc4",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 337,
				BlockHeader = "f2b4da0af2b4da0a53370d51daf10efc3dfb38d8167f243b9b757e1204389a92697a43196a045640e46a336030d0c7035467eb49953e068716d57c96837be6980ec7696719058748f2b4da0a",
				MidState = "77abb81480c0035f7e60264d5b93b52ba1d844c485169cc84d2455c332c80ad4",
				RestHeader = "6769c70e488705190adab4f2",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 338,
				BlockHeader = "69fe8d8669fe8d86c557301bc583683cd201625df98c881ecb17c2cf0eb44ef530ebc1f1f7a70b6a770ec765ab2547302f94da6098153b3eaa24f95eff0cead8e0258c02e6150ab069fe8d86",
				MidState = "00779ca48cd4a2c508eba5c7578bd7c1ab4e6f9b729d86c1f4e544a94e0652c6",
				RestHeader = "028c25e0b00a15e6868dfe69",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 339,
				BlockHeader = "151504d7151504d7b3a4d79b5399e5d96e82620484ae6020f01ee4074b1cb36ae3adfe63c14d1087a6ef3d63e5cc0a9a0dfa9744ed096f207334be47260b1f6c2f7dab656adfc3a8151504d7",
				MidState = "3719178c9d00ffe6784df0f2ed5743e779ffd80924518eca281a7cdf472ceefe",
				RestHeader = "65ab7d2fa8c3df6ad7041515",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 340,
				BlockHeader = "632a915f632a915f1c44e0d74ea756f57be43b3b682e137148b9aa5dbed8bfa5e55bfb5ea1cb1def33f9506582b66d9b551841ca9085e7fba16a7e7a53e7b1cce1fbae4e16990beb632a915f",
				MidState = "17f41c9cd2b19af0cc1436585529dfa533414aaa8a8198fa75fc107dd766ea21",
				RestHeader = "4eaefbe1eb0b99165f912a63",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 341,
				BlockHeader = "25a09dff25a09dff42fc7496551cfe5823b5fd77387709504580a642f0f046a485540f0f3775bb176f82411ea6063ef927891542705bdd7d7db2fb4817eda852d535253139aa0f3125a09dff",
				MidState = "8f9820a9d7a0fd668af70a5ea1c692937a0addb58806fe78d6f76fd256639244",
				RestHeader = "312535d5310faa39ff9da025",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 342,
				BlockHeader = "c0c9b70ec0c9b70e143b870e4b38e0913b7aafe5ff766ff3f5b633efd6685f1c89b0090694a63785701db17ac302465b547d47091324508fad0ae2218d4308ddae1f51520f4ae969c0c9b70e",
				MidState = "88af96303f6a1c4f4054160c75c3050232b6bde2f3d322d042f339d3294d1156",
				RestHeader = "52511fae69e94a0f0eb7c9c0",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 343,
				BlockHeader = "c5def79ac5def79ac198f96d63497a4f0c6e97c01bcad532bce898322528f1c6f73177fca144af9391a427c98f7d04275c05296ebb0176da0167e9ce090506f699ceda7a274d748bc5def79a",
				MidState = "0decfa2f619d2f9b181c3914970184d33e4ec35bb38dfb5ef8dc404fc8c5a791",
				RestHeader = "7adace998b744d279af7dec5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 344,
				BlockHeader = "ae1c9f31ae1c9f31fc7614215472cda431942265c649ef27c4baeb367f5c167afd3c6eeeec7bdc66d035e439f5aea22192b70880a51ef6cf754056f5a3b15056ef864e7add4b2a64ae1c9f31",
				MidState = "9fa9bec27690affb0bf16e2212f404d7ece9e0f714cd851f40d67ad88cac2e8c",
				RestHeader = "7a4e86ef642a4bdd319f1cae",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 345,
				BlockHeader = "b5922ac0b5922ac0fbf132a6a8fba29b14c7d9225d43a00413ddd94663ad36cb737f495664e9bea559bbb3b669e353c52658bf7377edf8aab53bb69fa1fb18f9579bd50dd46491c9b5922ac0",
				MidState = "04a2f10ef2f2870d988b6fbb5cd7e4ba54d28645905f3a27ef436a784a2079ab",
				RestHeader = "0dd59b57c99164d4c02a92b5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 346,
				BlockHeader = "03a6b74803a6b74864913be2a30913b72029b25941c354556a78a09dd7694307752e4651d8c6fe534f35c6c2405ad724c076df556dc896722d46a363103ee9c2b3c75500dcf17fd403a6b748",
				MidState = "d7e279385d291a730a0383d9d0849220ffd0bdd3c4d893617d95bf3ca4f780cf",
				RestHeader = "0055c7b3d47ff1dc48b7a603",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 347,
				BlockHeader = "51bb45cf51bb45cfcd30441e9e1784d32d8b8b90244307a7c21466f34a254f4377dc434d3aa7eb97ee93a344db2b62b207ccabb6725eb25834c95d307c1245974632cf67dee055ad51bb45cf",
				MidState = "5f0c6ca75d854c833aefe073279c9ad9f875cf14bfc9d3e7fadd7b3e9fb2dc41",
				RestHeader = "67cf3246ad55e0decf45bb51",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 348,
				BlockHeader = "dd0325eddd0325ed61dddd2dd142be2828ca14ce6d3546682586bbc983bfa03c62c30e9f78693e4376789c3d25f057c396862e81b4e0e806019eefe556ef8a63ec41b7cb622a01fedd0325ed",
				MidState = "dd84ce1fee2a13cff80c089bedb76deca81972cec39c31b103896359f20b9972",
				RestHeader = "cbb741ecfe012a62ed2503dd",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 349,
				BlockHeader = "792c3ffc792c3ffc331befa5c75ea060408fc63c3435ad0bd5bc48766936b9b4661f08960b4d9d8da491487e9872cdaaa42a44d19a0b6949f4f88b5d8b3a115734b9def26ac1e3f0792c3ffc",
				MidState = "c86ca1506d1f640c71bad9d46c12284cfa3dac19fa5a0f7c9bc00c2e2cbaed1c",
				RestHeader = "f2deb934f0e3c16afc3f2c79",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 350,
				BlockHeader = "ebf224a1ebf224a134056fe600cc4ff18ce2b8f1ea9f9c21f099d966ec266be36a7ba5930dbab8e167bbef28b0872d91877469f06469ebf75e2961e9989b55b0c0b93ac902a93160ebf224a1",
				MidState = "dcff6a21b4e3e0f6b4b41cfafeb28193440a8b5bc9f2ff2dd24f133485d030fe",
				RestHeader = "c93ab9c06031a902a124f2eb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 351,
				BlockHeader = "61ffae16d3f2a230257a35f72c75f62a47286b7c97c3f6f1d2f72e6aea118f0c868520acde8ef741292887d0b6af812fc5cf2dabcc9cd671559785650ced7a580bd41e970a29bb80d3f2a230",
				MidState = "32646be8aa78db8eaf43b44f5754a5fd37ac7c5e91112d29045d09645b6bd67f",
				RestHeader = "971ed40b80bb290a30a2f2d3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 352,
				BlockHeader = "578a631f578a631ffcf87b5a6b2f402926193864bfeed7a8d7911af312cd96a48b699ed3fff1b6bf3cea319ce96541f63450b492fc14b670477d93d4f393664e9c1b77d33d6e8af0578a631f",
				MidState = "5b10fd97a5169282d15ffc9a25602a81b2084327ed531e3179d99e14e68e6f0b",
				RestHeader = "d3771b9cf08a6e3d1f638a57",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 353,
				BlockHeader = "f3b37d2ef3b37d2ece378ed2614a22623fddead287ed3d4b87c7a7a1f945ae1c90c598ca7ac5d34b0405ac055920e4589b587f3d8ffba226e7d0375be7e4990ba73c73e0e0a265d2f3b37d2e",
				MidState = "01763333e0d79385a23a9fc49fe7696564d320222ed8f0a9330d37fec947d50f",
				RestHeader = "e0733ca7d265a2e02e7db3f3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 354,
				BlockHeader = "42b1eb1142b1eb119082737d816cfca699c0c5317873807379dcb8805e5f2bf9bc09ff5df32952983e39f57174100a2776708a93440da29c250e3254b656f947be3984ebc6633ec742b1eb11",
				MidState = "4a8072f26e4160d9055060ac2c16dcea3692ed09263f8ec43a05fda189c42971",
				RestHeader = "eb8439bec73e63c611ebb142",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 355,
				BlockHeader = "818e24d1818e24d18c08b8618420c477a0de11d7ab8738d1867847cfcd0acecce59727a5a5dfdda6283457bcecef3b2abba5a5b7d5fe1b816af6b91d5a5182b5cdcd79ceded8b310818e24d1",
				MidState = "43dfdac2cf806b91b78f60ccaedaa756923699ae5e3c0f7f5f6ab52b5f866b00",
				RestHeader = "ce79cdcd10b3d8ded1248e81",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 356,
				BlockHeader = "5dafbd3f5dafbd3f2ce9c9b479963c7f1c1f8aeec795361107e3fedef00d6ab7a8acac852fa85345bab0fe431cb966d3e455b785c1f99b6f7e6a9aa42254de9f2b31a285b3de7ae75dafbd3f",
				MidState = "6d5e0e519715637434f64516db19cfca3fb9c8b9a513cc35cc4b3987863bfc3d",
				RestHeader = "85a2312be77adeb33fbdaf5d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 357,
				BlockHeader = "1db73ee01db73ee05e47cbd97b3ca6afb9a2c34573869f7436aed47cb482e744eaf4219cbc44926777bbfa00129f05e7c79da7908d700051b2f356314dffe58f4c7261be2451d5051db73ee0",
				MidState = "262e729080c7ca316c640efcab4fa79d6748105c2e8c5548ba41b5550f784d29",
				RestHeader = "be61724c05d55124e03eb71d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 358,
				BlockHeader = "a20ff177a20ff177d55963811984c96f450a0067552caf073e57e8cef7749c5dedb521c5bcaa3784595ec0f18e6d2f784ee73d1308b482468420c46875db21f801fd15ed37929d02a20ff177",
				MidState = "91f191ba7cb32d354d2da6ed326518249cb8de95f6f1e30cbd1d1f1732541430",
				RestHeader = "ed15fd01029d923777f10fa2",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 359,
				BlockHeader = "3e380b863e380b86a79876f90fa0aba75eceb2d51c2c16aaee8d757bdeecb5d5f1121bbc99ae8a5c69ee7f8b01296f28b0c4a73c5c902c0d0baad649272e3b7c6f7a52c68148afa93e380b86",
				MidState = "b6e2b1519748ec9484de3b3b556a6f6b652d8614df724251b801302bcae1a005",
				RestHeader = "c6527a6fa9af4881860b383e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 360,
				BlockHeader = "8c4c980e8c4c980e10387f340aae1cc36a308b0b00abc9fb46283cd251a8c111f3c018b77139dbfb0efbe5999f270c6a1a0535b72ade62591f4b0d0f123097e6ea34dde0482546258c4c980e",
				MidState = "a28ec6765ba96b8547c3f9bec3af2c128967a5e9869fb570cf19ea58284dfd43",
				RestHeader = "e0dd34ea254625480e984c8c",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 361,
				BlockHeader = "e77ecec4e77ecec465c3cd82825119c8b39f8dca1554aab7499c596b0721b183c94c1cf75f8811b97d9fbc0c928c150f8c745dde290bfa39486dd3342145373338cfde87c0bf9b8ce77ecec4",
				MidState = "4bac961674662b31479213a237a02dbb3b65c246d8d03262a12ec1a0a229583c",
				RestHeader = "87decf388c9bbfc0c4ce7ee7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 362,
				BlockHeader = "35925b4c35925b4cce63d7be7d5f8ae4bf016601f8d45d09a13720c17bddbdbfcbfa19f31a47e7bb661e674946eec7d6befdd972c88572b65b7fd08175d24de8d424f6fd770a8b7435925b4c",
				MidState = "e1922a395d44499a3221eb6afeeee1c7b36966d913fc482fea7eb14ff8389b1e",
				RestHeader = "fdf624d4748b0a774c5b9235",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 363,
				BlockHeader = "83a7e8d483a7e8d43702e0fa786dfb00cc633f38dc54115af9d2e618ee99cafbcda816ee2b5ff3fd9e58cb0027d70afdcddaab0e00006fa5864cf7d18dd5d29d9b14a3b9f053c2d383a7e8d4",
				MidState = "3539e812efb84318c9dc54401dacdbaf1328a8569c2c79c013fb1d602e8e773a",
				RestHeader = "b9a3149bd3c253f0d4e8a783",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 364,
				BlockHeader = "d0bb765bd0bb765ba0a2e935747b6c1cd8c6186fbfd4c4ac516dad6f6155d637cf5613eaee7dc715ac887bdd057390668ee25bdc222a8fd45da872dd11088a3a30ee078958783264d0bb765b",
				MidState = "178ea758f2914042a9c8de440bed92fc8e388fcb36877b9c21e7331268052636",
				RestHeader = "8907ee30643278585b76bbd0",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 365,
				BlockHeader = "98236f7d98236f7ddbe2e3f978d98c01dcc74004fe0707307b76907c940b2671d05e59bd5d8c2d75824818ccb1caa387419b5e453b8a894ac1eec768881046c13d5f3f8bb5fe665598236f7d",
				MidState = "1f0cc095e2c081fca414d5a880eb0a3cc41295c76916cc3d86d2eb7fd12aefea",
				RestHeader = "8b3f5f3d5566feb57d6f2398",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 366,
				BlockHeader = "ec597421ec5974212df081063ca1520e64f97b298ffc8cfcad7c8ac926af1ce4ce113790f28fc309ed269b423371a1523be3537dc10dbf5e7b778a4bfc9ebeed237e7eae16b5557aec597421",
				MidState = "065d272af455fb14bdb60074c3d3f65cce56423642c6d6867e9514ffcf732763",
				RestHeader = "ae7e7e237a55b516217459ec",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 367,
				BlockHeader = "489672e9489672e923f3e7ff38776a1576dba6367a0361852edb7e1b271603fb1aed93e9a75b6ee5a1306484c0710c853e3ad37744cdd3cab2adad51451e345bd31999f14bd51aa3489672e9",
				MidState = "32f8265796fbfbac14b1ec8dd8047331db2dbf86b562f5253db4489750aae099",
				RestHeader = "f19919d3a31ad54be9729648",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 368,
				BlockHeader = "e3bf8cf8e3bf8cf8f532fa772e924c4d8e9f58a44103c827de110bc80d8e1c731e498de0b2daf23edd1cb2d6021fa8e54a6057947865ec5df585212365ebcfa852700ca9c890c53ee3bf8cf8",
				MidState = "23b0ab9f81e64a5d645a8919b9a9ba7ffc8d47878532ce7b0f8f5cc90e31f005",
				RestHeader = "a90c70523ec590c8f88cbfe3",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 369,
				BlockHeader = "e1175f80e1175f803b6a4bead82b0bcb9b5e68f979b5a2521dbb050ca04e0f162fdae57421a2932649eb4cf021cbedba874d427a097c47b6050f66e3f667fd62523e1f9d781305c2e1175f80",
				MidState = "503b494128f590b396dc9c1c732027c4d18d3d960644baeb0ef8da286aa2ef3b",
				RestHeader = "9d1f3e52c2051378805f17e1",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 370,
				BlockHeader = "8deade108deade102bec6a215333dc01d551f460313ce6aee7649152e38e3ffde8a6b00fb8642112fb4a8188a7c9dd6a2a834a4e5acda34661dd53ab1891a3a3bfdce6eca39eb1ad8deade10",
				MidState = "22528839a3b79558e4ea46a8a47f203f31971972a03710a9422c8f3ea1a65ee6",
				RestHeader = "ece6dcbfadb19ea310deea8d",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 371,
				BlockHeader = "dbff6b97dbff6b97948b735d4f414d1ee1b4cd9715bc9aff3fff58a9564a4b39ea55ad0bcd542026ad14dbe6df56cb8cde00cdc1b1dce21251be31ca9a6bc622eed2d5cc5ce10d9cdbff6b97",
				MidState = "e453beba4b23665af16787b52701369e08c5bc211ac5def1843980662e3627a0",
				RestHeader = "ccd5d2ee9c0de15c976bffdb",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 372,
				BlockHeader = "d14be6a8d14be6a898a9538e0a0b4e83252279d692f413d90ac4d46dd9c5e304d54bba31460ed29df4bc85e1b3e221199241a7d1dbc5131023b0f0c6913b1410268029db28adb8e8d14be6a8",
				MidState = "0f0a14ebbefbdacc3f070fbd098425498fca70e631a9c06b4d18c68419ca65d2",
				RestHeader = "db298026e8b8ad28a8e64bd1",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 373,
				BlockHeader = "b29af250b29af250644f85bf4d3196b50cf4ab1d7645f7ce29690ac57eb812a62a84a167cbd2f74f6a9f13a6348943cddf47d79efcfcd389d60e08a673c57ad30c29c79d4f02570eb29af250",
				MidState = "a6fa1d300492acf6e0a4188294115f1fb4ea1d38e0529207095cfa293497d10c",
				RestHeader = "9dc7290c0e57024f50f29ab2",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 374,
				BlockHeader = "21c0ef1f21c0ef1f5e9858e0c0ebea058b4eda020bf2e1a380fcf3aabe6b71a02e0465067d243bfc753f95aab9a307834223ca7a374cf745a63b9485dd13864cc4b343f9dd66b5e721c0ef1f",
				MidState = "fc1f6184544624c5ab2ce5dfdb10a074d7376c11c48f9f5f59c29ffb47d40945",
				RestHeader = "f943b3c4e7b566dd1fefc021",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 375,
				BlockHeader = "6fd47ca76fd47ca7c738621cbcf95b2198b0b339ef7294f4d897ba0131277ddb30b262028535affb32ad5d36996b89e719d07e7f69393c0c8d25c437907df6213403c6bc5a87d6966fd47ca7",
				MidState = "d7a967e610605309ee5b5ae9fef99ddec09485f89b68da1b5eddb4df49e83e34",
				RestHeader = "bcc6033496d6875aa77cd46f",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 376,
				BlockHeader = "634b1ec4634b1ec4f7c9df68d25020e410e8ea68f09c11470142f7636d0cf0ba5f13566cbdf1a591b7ffa7a5a4e10dfb917e1b6d4f1aaf4a0d99ef23807461d0ee6925ff5928e91d634b1ec4",
				MidState = "a7e61a7f9727e0779cfdfe4c6033605ac1711e6d902db89b92ae3a228c63ce45",
				RestHeader = "ff2569ee1de92859c41e4b63",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 377,
				BlockHeader = "231b0bbc231b0bbc1fc2aba1c6fbaa4da778eec8410aeb87780ac8168ed489cbae9d47f76f31ee2919e9267c99efe994933b030f9407394a11186f7613cf9beb91a0ff8507b582a5231b0bbc",
				MidState = "ede00494dbd13114803f0637e7ff904ceb09b83e393d53acb5f952071ce88807",
				RestHeader = "85ffa091a582b507bc0b1b23",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 378,
				BlockHeader = "b7ef09dbb7ef09db00b990e86eac22be9cb22cbaa6b58ab32411a962cfffdddb83748eab8a10078b630a776ffcf1bfafcedbbeddbf1d5542eaff1721b3b69774e81bc88419b22b7fb7ef09db",
				MidState = "2aa690a6efc9e56f0c8b5ccf5185b3f620550c9e6b3bce016422c3453c4c8ca3",
				RestHeader = "84c81be87f2bb219db09efb7",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 379,
				BlockHeader = "47e1d81247e1d812d37644e336903d195b2d1b833dd3aac67b94a31468021ca48bf891c8edc6b93579d5725dc4bf518a1368dc25868a4256d1c4e394ef600940e894c88c7a6db80547e1d812",
				MidState = "62df008e88bb0ff70bfd56881b73c737453f8a1b23cbc8b1163d473ed8226895",
				RestHeader = "8cc894e805b86d7a12d8e147",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 380,
				BlockHeader = "4046b7914046b791295ba86393790362f21401fabf734372e3ae86356c9e033fc47de8688652561b20b98f3ad26cbba276ad30ce7e089b45e02d1aa26c84e45741d076e3f9cd8dd64046b791",
				MidState = "231f9067283c3aaa83ceafe0eb322b1350f532ca129e470227ee91872aabd8f3",
				RestHeader = "e376d041d68dcdf991b74640",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 381,
				BlockHeader = "8e5b44188e5b441892fab19f8e87747fff77da31a2f2f6c43b494c8ce05a0f7bc62be5646946730a5b69e6ceabc3efb1d9f470a75cdb47fd03ced956b8cdcf22cf175bb7233f2fe38e5b4418",
				MidState = "34e13622bdf02ebe95d7285516b1cdcd591e9a87e4ef5a00263ebfe5725159e4",
				RestHeader = "b75b17cfe32f3f2318445b8e",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 382,
				BlockHeader = "dc6fd1a0dc6fd1a0fb99bbdb8995e59b0bd9b3688672aa1593e413e353161cb7c8d9e25f9e7e547e88fd9e524e6cd4ee45b1a49b430bd3426bdb50f24fa59e5f15b0f287a65dbd4adc6fd1a0",
				MidState = "c812af360ea1eb78dc533347718ee3fa67246eabe2df92a7de5873a365dd8b78",
				RestHeader = "87f2b0154abd5da6a0d16fdc",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 383,
				BlockHeader = "455bf71d455bf71dd6b81ac3aa8a9d21c208e8d5dac6a9b2aae0eb78bb5e95e832fe565e08e3a31fd959fddc038f2ddf02a6f0a32445479a4be2265b8b83de73cf33faf95a3c77c0455bf71d",
				MidState = "d46ad941236e47875d7914c497b8e4257f30aabc1a2e179d7f5133c4c2595d16",
				RestHeader = "f9fa33cfc0773c5a1df75b45",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 384,
				BlockHeader = "a62f630ba62f630b7d54b50ed89dcbf0544c7ae4fb89427aab9ab489bbe563306f5092a3d370ba80827ee9c44aa80bc12f65a388fcf922c11c25810f196fa1e1f390a8c5f51259b2a62f630b",
				MidState = "b08c0fa488c2296f6a694217b28dad24f2b7fd580f4b4b3310ac11b959a9293a",
				RestHeader = "c5a890f3b25912f50b632fa6",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 385,
				BlockHeader = "f9e36a03f9e36a03d858e21802930220739beff58bee3720b9210e727781f8d339573ee8bdb60fb9407e8dc7da6491015f6bb8c2439aab3f73e1d1757a2df545067e10949621d6acf9e36a03",
				MidState = "a0b06180a02515e17bf5d83d58464fb737f59759d20f8a363e72286c31d37e94",
				RestHeader = "94107e06acd62196036ae3f9",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 386,
				BlockHeader = "d5040370d5040370773af36bf70a7a28efdc680ca7fc3461398cc5819a8593befc6cc3c8ccf9ee68fad6590fae2099f5933cb3b68cc889fc8829294baaf36504b9d376de00e342f9d5040370",
				MidState = "9f9ffca0e58be80b8b6b400554804ba9afba82e232948598901068f0f9f83e5e",
				RestHeader = "de76d3b9f942e300700304d5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 387,
				BlockHeader = "6a23cf8c6a23cf8cf4e8d919acb0ef12b3df1f2211fbc3d92e9f87f7037e0bb5824a214c9e2bd7808f881e04123e38e89c7ddee456f7b709b65406029d8b73ac21860bc11b2a3dfa6a23cf8c",
				MidState = "042ab16f4d385f373d86f19db016239d52a153a63ae38582fc83b53e193d1393",
				RestHeader = "c10b8621fa3d2a1b8ccf236a",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 388,
				BlockHeader = "a8be247da8be247d45579f2c625da0111932a037e680445bcd6d016113b2578d226946ff04ff26203c75f660169eab4618045c3146ca7a5ff59421acb72ba761445bbe9ee09fb0d6a8be247d",
				MidState = "31e767b912d26523996591ba8f922ca7fca6d2cdf3b9b4da2247810e7d08b523",
				RestHeader = "9ebe5b44d6b09fe07d24bea8",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 389,
				BlockHeader = "7378b3247378b32472d2f258641324e543ab9c9a42e7e8d16953947bc688b729991cd5e04fd72f25f96f95444fb2da4b0ec6e06421d730a110da7bbb133338aa850396e02a447cd57378b324",
				MidState = "c785fb7f9675357fabc0724f6f5880b04bfabc1b65aaa84a99871acc09c4512c",
				RestHeader = "e0960385d57c442a24b37873",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 390,
				BlockHeader = "c18c40abc18c40abdb71fc945f219501500d75d125679b23c1ee5bd23944c3659bcad2db1617b36c18d26d117ee3e5a9c4cf0addbd22df9219a1174e1b466effa99d59834950553bc18c40ab",
				MidState = "1b7f587da7efc8e49b7027d28efdf88f372948779d7ce168d7d1d83336da31ae",
				RestHeader = "83599da93b555049ab408cc1",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 391,
				BlockHeader = "b538ff87b538ff8715a2e0053f4d5693a09c4f42aec25b85dc7cd6608ab1969add4a71c178f38d2c7e919dbdc7c957d1a715cad1731719cd6c0087fe3ab2fb213dc8bac3ca43661cb538ff87",
				MidState = "8293588f7a2129957ce6f353818b5b9215e3a546cde0e3880035212afaa0dc70",
				RestHeader = "c3bac83d1c6643ca87ff38b5",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 392,
				BlockHeader = "034c8c0e034c8c0e7e41ea413a5bc7afacff287992420fd634179db7fd6da2d6dff86ebdb1c35886307588e0b0c30b9377695186e9850b6b36f8a57e119783e9c4fd6ff8b30eb8c4034c8c0e",
				MidState = "c40cfac0061a43f9099ece7f7a8c3601c8a9482a8a150665978ce442b64ae250",
				RestHeader = "f86ffdc4c4b80eb30e8c4c03",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 393,
				BlockHeader = "9b1594479b15944737ede964752c40f1b895b0d94ca29ddd5479bcb3a32f9fa5cee90ea411f6737879f2a232a3b9fd94e7b276233f5b37a74eac732fa07f69103c327d88b4e515419b159447",
				MidState = "0b2544a88884e77435100adb216930ef5d6ddff609b5e8972c41178d5270a226",
				RestHeader = "887d323c4115e5b44794159b",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 394,
				BlockHeader = "373eae56373eae56092cfbdc6c482229d059624713a2037f04af49608aa7b81dd245089b03ae9e4f27479a1772130e80624503673b5760d3884dd8c074e581579bbd9483a74a1826373eae56",
				MidState = "c6ed808394a443bae4cc707424af472549b10ae6aca1bfc1966437b723b030b5",
				RestHeader = "8394bd9b26184aa756ae3e37",
				Nonce = "20000001"
			},
			new SelfTestData () {
				Index = 395,
				BlockHeader = "08a1d0f208a1d0f255709531a803a7471082c00e9bba87f76a2fa2afbaf15d8dd5375173220c2ba6b372271dea8b256375d3f245858e5931d6c14d575790fba341d52b3f311b94ab08a1d0f2",
				MidState = "d425b27dc74e4881a3180ffd8d52fd3faa21961941edfc71bcc98ffeaf250dc0",
				RestHeader = "3f2bd541ab941b31f2d0a108",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 396,
				BlockHeader = "7bbc07c97bbc07c934a0d41d2e37ea95edc3f3739ff53081c9c6985d3b9258fe4019aeaeed668857423f0752c2772a997d3f5979d5860ce2b80322152b731a491c503f0fe4f8dd487bbc07c9",
				MidState = "e2977a44c78f53c1f015aca99c8a054c309253c8eb49f7df22d6826dd6174dc5",
				RestHeader = "0f3f501c48ddf8e4c907bc7b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 397,
				BlockHeader = "50fe1edb50fe1edbe7ded7e66c01ba1251c37edbe128efedca25b16738d6403f011ec48e4b33b605f49b3a1b2248670a8071f3e5ec594fe2dfce2bed1b268897ec4ab05b086a267f50fe1edb",
				MidState = "8f7b9d12a7d1be34a386919d712fecf5c27c848a145d0f0a373436c83eca04b5",
				RestHeader = "5bb04aec7f266a08db1efe50",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 398,
				BlockHeader = "0e6050e80e6050e80f4abf95db83181b957a4142f3666fd5dd488dbc10a1db27a3aed517f9fb9b8065a2773f684f9c283e758ad28e05e462eee560bf9121fcd43b9f927f7e453aef0e6050e8",
				MidState = "b3b3f4be96461ee8c1efa55e1d32897233eaf63ad8fd1d3d5efdd8adc82fd132",
				RestHeader = "7f929f3bef3a457ee850600e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 399,
				BlockHeader = "3684b4733684b473883edd721030ca8483e3de191b750b50ef65f5a2ae64c9088ac79cbf89d65c14cd4431a55b4a9841492aaf06be7461ad3c5b1c11fe965cfff94f5055e75c01723684b473",
				MidState = "dedc6ff9f49dfb46bd148309a5b947b0bd36cc131c9ed779a2173272876ca0eb",
				RestHeader = "55504ff972015ce773b48436",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 400,
				BlockHeader = "9c67f82f9c67f82fd9e77dc688e5e02c5947a4e4e09526205ff3d1b6543f9b8a95bebbc903ae3660dff8d370048d159c08114080840c4d4f7f3068004f81e72ca0e7831d0e0132ad9c67f82f",
				MidState = "606eff27eeaf72783d767b19321e4f53b0322918131748819921763a0ab82673",
				RestHeader = "1d83e7a0ad32010e2ff8679c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 401,
				BlockHeader = "7888919c7888919c79c88e197d5b5834d5891dfbfca22360df5e88c57742367558d33fa9b4b15a49e4c9699620d837638247fe1b6a8c7c2c9b40aa5466918b470ab78a4bc511c1df7888919c",
				MidState = "50ff685d6374d8ff1a47a76aecea98d74af7d78304b4b01977cbce92970f538e",
				RestHeader = "4b8ab70adfc111c59c918878",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 402,
				BlockHeader = "5d1f89a95d1f89a91cb0298aa493871f495a39f7ac2596f028b4e01aaa817585720040b31bea4d927cbad8d05acee5449c4ce59b0b7e07ceb9064caaa88066abe27f0b007adc60405d1f89a9",
				MidState = "ddef305348de8df9e436862ebf8129aef5794994b929fd61284ecbb3253a46af",
				RestHeader = "000b7fe24060dc7aa9891f5d",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 403,
				BlockHeader = "f848a4b8f848a4b8eeef3c029bae6957611eeb657424fd93d8ea6dc790f88efc765d3aaa2b5c512b58452118c6d7f46f52a1c6e96a867fcc867fe99ea3f3bef7c3f418500f6520daf848a4b8",
				MidState = "6dd866bc3c11043cee0ede07d814b5d57ae13b7b3464e1dfb432e20e8ebd5a2a",
				RestHeader = "5018f4c3da20650fb8a448f8",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 404,
				BlockHeader = "a1c7a476a1c7a476c477ea2fdfd98c6232094519b1fb3cb04ac5ec7bc557e603703dd9e91881932e73f5813dfebaed85d5e0e0010546da67f27237b30b6c273c50ce9d7a4482edcaa1c7a476",
				MidState = "3dd21604645bdd63ecefcc42e4fce372c27931881d3199b6fdd16c8b8f3bbc9a",
				RestHeader = "7a9dce50caed824476a4c7a1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 405,
				BlockHeader = "efdb31feefdb31fe2e16f36bdae7fd7f3e6c1e50957bef02a260b2d23813f33f72ecd6e503870a24c18834b601b4f363307c521b315e6e730e6af85330a50a32dd5c66154d5384e9efdb31fe",
				MidState = "d7d5e8c71aab15e678ae24e03c7e85ea9dfa0186982523277a58a9e67533d1ac",
				RestHeader = "15665cdde984534dfe31dbef",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 406,
				BlockHeader = "2395ba7d2395ba7d18f0aba77d7005277c11ad4692bf03f508bdb40ee5de65e6a1f3023b47b6930f2cb56211a8013af5a8fbfb47e23b13e2d074877ab758e75406f9ca36989add462395ba7d",
				MidState = "ded7eb35bc89b4a5caa087d6c6911475d632bc23af2a6bc4a9fb1b0a85dce07e",
				RestHeader = "36caf90646dd9a987dba9523",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 407,
				BlockHeader = "71a9470571a947058190b4e3787e76438973867d763fb64760587a65589a7122a3a2ff367accb85c840788580b7af852de299096fc3058832f3d8bcbd84efd884413d658dfe65eac71a94705",
				MidState = "871563c932b1023b3935bc54432cfb2335e019c3a9d51d1700b229bee49fe8f3",
				RestHeader = "58d61344ac5ee6df0547a971",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 408,
				BlockHeader = "77ce061e77ce061e5f92c88d0704c1feec53c10ddb2e05f96c38a4022a4decc72ffd77b65dada4d45dc697fb9592fdce70f90138273cf3345e45fe4260c2312cb8b9407988681e6a77ce061e",
				MidState = "ceffbf125c2fa363fd56cd95d8e6a6f94bdc44467517c237fdf6b811295a4d76",
				RestHeader = "7940b9b86a1e68881e06ce77",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 409,
				BlockHeader = "600baeb5600baeb59a71e441f82e1452117a4cb286ad1fed740af8068481117b36086ea80f4fe62104abd4ace63d286a9e399bbcefcac42c9c17ed9b87c1a9fd380b0d094be8ea00600baeb5",
				MidState = "70933508618bd3f5b5736ee0bf0656bae577f6ea617ce8a4c29f47fdda731768",
				RestHeader = "090d0b3800eae84bb5ae0b60",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 410,
				BlockHeader = "c3e9cb77c3e9cb77c051ed305027f4d33dfd94501dcccea88686ed7f6b1d93bc8240bf791df1e1b2cd456f92d93c98a48168f2287f9d215b73ffe861e87ef56afc479b7aa9c9b8bfc3e9cb77",
				MidState = "73af8e6a540f4eb65576ca47a7050b46658b79fd87a8d6939e6cd4482b4b19fe",
				RestHeader = "7a9b47fcbfb8c9a977cbe9c3",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 411,
				BlockHeader = "60be37e060be37e0f1c2c22427b843b4cc8988c7962e14b3ce358102f33775b756193d60167f91e3a62924731d69e86afe36144e493096e8718da7bb00a50af95da3c6558792a1b260be37e0",
				MidState = "9c3a51cb47c355b913e7543f1def0bfb73a4c002791e05002d4cc776760521c3",
				RestHeader = "55c6a35db2a19287e037be60",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 412,
				BlockHeader = "20c6b88120c6b881241fc349285eaee4690cc11d41207e16fe0057a1b6abf2449761b27726eb3e3e903d65ae93776f12070528c8ab1e0241e120310495eb02dc3bc253303c84649020c6b881",
				MidState = "789c013268b7e3ee3851517d9e9c4289b4fc56023aa81e2780e97b04b6941fb5",
				RestHeader = "3053c23b9064843c81b8c620",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 413,
				BlockHeader = "fbe751effbe751efc301d49c1dd425ece54d3a345d2e7b567e6b0eb0d9af8e2f5a753757e922936a3a6697a463fb0e75b0285100502a0ee664a29bbfcfd954948c1b7d7901ccaba4fbe751ef",
				MidState = "a88755f2a6fe2b759d2876aed6811950aa1bb66947e7771772a5a04fd9c296d9",
				RestHeader = "797d1b8ca4abcc01ef51e7fb",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 414,
				BlockHeader = "ce4c85c4ce4c85c462e88802bb7f087ceb8e1ab6ad2ac126aa1f2984489daf508804641edc6c131b8a6e06d47cbf6b28fc2f585781065b0dde296eecebbd210260175a1577a8bd87ce4c85c4",
				MidState = "32cb540c27159e14da226f47320f7a8495584fb2989e76b1b9ccca73bbd5bf60",
				RestHeader = "155a176087bda877c4854cce",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 415,
				BlockHeader = "01b233fc01b233fc7041505c5567625fc7e0fab12ff068b893e08bc285db5cda2f24a8461fb33fea54e7164d87de209644da6a446201b699d9ddd26f442081161dca613204aac9c901b233fc",
				MidState = "08871ad206779a1d8eead79e65dcdec816f31f33bc6045458b1d0b2225406f01",
				RestHeader = "3261ca1dc9c9aa04fc33b201",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 416,
				BlockHeader = "e8839792e8839792ac59574e068d6e4bc8c9d4136d917367ff2baf272336098a68d49846cfb7309a84d048d6baedf17bc1a2a7b2f377ec297ef603134dbf4d792737926390b9e582e8839792",
				MidState = "060f899c21990506d23b62d91953452734216e8f4637f5c78c3a8ba723db2ca9",
				RestHeader = "6392372782e5b990929783e8",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 417,
				BlockHeader = "61bc58f561bc58f5cbf6d0bbbedcd49226c7a10cc0f8ef183c657d4ef2ca3f5f11205f13e7a899cf5aa5731fbbba1ce7c15e538b819171d2588be17072355ce1f0354d55c81266bb61bc58f5",
				MidState = "2263372476e25a515594c6df3fb7159c135f1d4d2df9b371dd1a816a45b65869",
				RestHeader = "554d35f0bb6612c8f558bc61",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 418,
				BlockHeader = "fed1cb49fed1cb49f7a68e691e3105eabba8e657674f31a34e341b4457c44d9d1fb19db17aaa6a14519a0803787a8269e5bb21eeef5c050ac1d14f628d51c05a3d0c5f7ab44fe648fed1cb49",
				MidState = "fa4c0b6638ac37bf0aef0118e2a789bf36a95160e72fe27d5f2aa7ec49e13677",
				RestHeader = "7a5f0c3d48e64fb449cbd1fe",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 419,
				BlockHeader = "9afae5589afae558c9e5a1e1144de722d36d98c52e4f9846fe6aa8f13e3c6614230d97a878a1949373559a87a35428825c5c34bcaf414e7d9fcd4011cf472ccd0ce79b4c4981a0e69afae558",
				MidState = "e7d3e6a0b9db7ac198fa8bf59f00cc5ac69df65229ee3fc560aef355f390249c",
				RestHeader = "4c9be70ce6a0814958e5fa9a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 420,
				BlockHeader = "97d1321e97d1321e19d84f186739fdbba6e473bb37c32e5ee0b4c99187187c0ce09097bf8bd0c875fe97ce703a37d43be6758c511facccee5a625cc9e8bf0ed270976d67c34e590d97d1321e",
				MidState = "7dcab6de55a1b4b0d8fdef28445828e76cc7a15fa4d83d5cc08a90f908dbfd31",
				RestHeader = "676d97700d594ec31e32d197",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 421,
				BlockHeader = "2b338f012b338f01e340f9bfafd0cea6609cb70a161561325397db45f6c682fef04bfc4cc019d70ade597baf9f48f2918fdfe4f4604796179dbb27e72be31a7abb7ca85cf7fbeb4a2b338f01",
				MidState = "ec2bc9166e7e71e82f9b1600883c297c6187f640adc41c7f2dba2b18f519662f",
				RestHeader = "5ca87cbb4aebfbf7018f332b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 422,
				BlockHeader = "53044a3253044a328ce678a85a37c176323e5ef9dc7f5f267ec37346ff549f7b9ec51d2f2b225e649f29be2c10f2b603a195508cdc64263826826936d4ceba2a56544a3a4cfea12f53044a32",
				MidState = "e399d19af4df6620734b3f4364dd1b9ac320286f8536023aa827e5e0599eed88",
				RestHeader = "3a4a54562fa1fe4c324a0453",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 423,
				BlockHeader = "ee2d6441ee2d64415e258a205153a3ae4a021067a37ec6c92ef900f3e6ccb8f3a2221626b8672ddd4d840c71592cf3f07677f375fcdcb484332590c140c77a3386b2a42a417b590aee2d6441",
				MidState = "7e2130a3863c77f7c95c8118689173b56af86b6f88e01fc80d50022f9a9dd517",
				RestHeader = "2aa4b2860a597b4141642dee",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 424,
				BlockHeader = "dcced429dcced42941c444c8f00af6f346d0ef81cedaae008e63b04ab9bbe9a766231a9860e67c4b4ad90ff663e758c0a113923284339757ca2098a40f619fd59a4aa432d20eec82dcced429",
				MidState = "8757f29a692a38800feaf4cc15b60007cf61c6755ca6d34554a7b56208d01820",
				RestHeader = "32a44a9a82ec0ed229d4cedc",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 425,
				BlockHeader = "13d38e5313d38e53a6e7232a8a6377bb53a6199141c21e7997c2ae20b89fc0afbe67f3414cf39d039ad3d0e7a9a994e65e025c9d1749eaccfed7403eaa4efacf2b1f2168274b222613d38e53",
				MidState = "c258ba3fc528a3839cdd62c075fd9279cac48f18a6d9bb1594394e773c261fa8",
				RestHeader = "68211f2b26224b27538ed313",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 426,
				BlockHeader = "aefca862aefca862782636a1807e59f36c6bcbff08c2851c47f83bcd9e16d827c2c3ed387d8308e301f50466fe37607c2cda471b80fc3d0d039b5947ad34a798572efc770d24ebc1aefca862",
				MidState = "3d0d385f4d640804119a8eceedd2556f1e65017364d08636ecfd626e6d692bcd",
				RestHeader = "77fc2e57c1eb240d62a8fcae",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 427,
				BlockHeader = "cae77fcccae77fcc12a37b3727cb2e1bc0d0e4414ffe8324459bcecd568544acb809e17db231bda24ce9e835a592f1224e76d2fa91b967ad88e49053314a894f1541b437949d371fcae77fcc",
				MidState = "7a243cfd8287bd23dc7503d6e4be3faaeb40ea664774f89270b4cf407633e18d",
				RestHeader = "37b441151f379d94cc7fe7ca",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 428,
				BlockHeader = "18fc0c5418fc0c547b43847322d89f37cc32bc78337e36769d369424c94150e8bbb7de78403edd5dd8af7249e132293f6eaa207618f399023438062f2970b4e165be3e322240399518fc0c54",
				MidState = "0962accc20cb8df08a18bc6d4aab2c0458741a1088d251ddfb2fc85063c472b8",
				RestHeader = "323ebe6595394022540cfc18",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 429,
				BlockHeader = "b3252663b32526634d8196ea18f4816fe5f76ee6fa7d9d184d6c21d1b0b96960bf14d76f9b93759ca65e92026f883aecac6dc22e8c0d13003b5b5e88be28c12ee8f1ae69d07dc974b3252663",
				MidState = "25a7881842c49e553fe175f796f528c29edbae3a6d64a6599ce78976a9d22a3f",
				RestHeader = "69aef1e874c97dd0632625b3",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 430,
				BlockHeader = "fed96c07fed96c07fbb0187158f4285dbeddee1c8cee575fcf4f04dccaa5a7178b8650f671b023e86510886fdd2c97c98eef59e22048e8e4fe16a9da4314067b201a59576bd45c9efed96c07",
				MidState = "517ac98a6670925fb5d1e4aad2bd0af91774be9e4d5b1fc9c714a0b41d7bc5ed",
				RestHeader = "57591a209e5cd46b076cd9fe",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 431,
				BlockHeader = "4cedfa8f4cedfa8f644f21ac53019979ca3fc753706e0ab127eacb333d60b3538d354df27b25dc6223c67cb07cc90ced27b1eedc247903b3d8a399b86dc13440271ed64abec0b6d24cedfa8f",
				MidState = "969789a3be8a919b50d435db72a5bc65b24331279d33648ca2b77f7d10766166",
				RestHeader = "4ad61e27d2b6c0be8ffaed4c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 432,
				BlockHeader = "9902871699028716cdee2ae84f0f0a95d7a2a08a54edbe027f859189b11cc08f8fe34aed0f09bafa5c984f180624de22f9e112b645b33f5ed91e41f154cf34b7cee86c4f3126301f99028716",
				MidState = "04b3e2d20644ecfae43662f03d1fdc10a5880608fc053d2a6047ffb588b28c3f",
				RestHeader = "4f6ce8ce1f30263116870299",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 433,
				BlockHeader = "8173a6e18173a6e1d3f32c9bdbd35754cc6a4db8876b83299b08b625808532d57009a72d087742a5df3eed13192f1688261d1311e8a0593aa6e18721bd51a9178301643215a357328173a6e1",
				MidState = "8442082ceab83384ba68a16d255582aba0b4035249fd37c386766b14686d1e5b",
				RestHeader = "326401833257a315e1a67381",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 434,
				BlockHeader = "3c49c3733c49c373d6006e53057d266ba124aa75c5756eb19c5b80faa7c112615fee07ce7ee7c6899f25ed747f62811c8d8c5033fe41f4b95f854ed04efa29aa1e64587918b1dc3a3c49c373",
				MidState = "236e1fb61381d9aaf80a3b48803af6d92362b7c3a3635ccf3f75fa8adad9064d",
				RestHeader = "7958641e3adcb11873c3493c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 435,
				BlockHeader = "05fceccd05fceccd93851387404af305309d9ef8b2a1f93d43b8749b88e4e5f95ec432a674d86a10fa5ef9b2dca22db9cc46b14912863246ec01e6a0966558261973d86a434a9e7f05fceccd",
				MidState = "c7a6a625644799fdf49dc7c02d4a7a9b8233f14b08cc587affd40fac3040366e",
				RestHeader = "6ad873197f9e4a43cdecfc05",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 436,
				BlockHeader = "b1980cc1b1980cc11b817031ae1b44ad8aec245c5515cee59c4171904193e33f45a2a41b84c687ef20b4bcc2e8147c65285b84b3e8b793fded04d28e4b271792fd8cb60abcc93e96b1980cc1",
				MidState = "173974dbc7da32f4327a86813d1c2eac0ba359a18e748a6b1d9ecd32f379624a",
				RestHeader = "0ab68cfd963ec9bcc10c98b1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 437,
				BlockHeader = "4dc126d04dc126d0edc083a9a53726e5a3b0d6ca1c1535884c78fe3d280afcb74afe9d1242216905d8fa0acdca27defb8d023f35bb8a1633f59a5b8485536ed5a6753d7aaf13da804dc126d0",
				MidState = "4aea41d48101505e4fbbe2de86afb24d72bb89775c8010de5c5698836b4b471a",
				RestHeader = "7a3d75a680da13afd026c14d",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 438,
				BlockHeader = "9bd5b3589bd5b358565f8ce5a0449701af13af010094e8d9a413c5949bc608f34cac9a0de304d92b8fa2caaad6332fbf660c16ecebb5c4d35053074ce32cf274c23dab035376221d9bd5b358",
				MidState = "33f15680ab1a9d5da1311f8352dbbeb667d3d8c1d323a5a0494c40c4569cf2a2",
				RestHeader = "03ab3dc21d22765358b3d59b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 439,
				BlockHeader = "8bed526f8bed526f2966dce23fee1756fd1deef13e9197c678416fd0f3e57f9804a1b9aeac0eeb1628b456a360df32f72bb4d7664fc26b91fa26aa2db24b50e8e34e50139a5949e98bed526f",
				MidState = "71bcfd380bd3add64de078d415836f3dc39ea46f9725de0e7a8f94c06f6c7d24",
				RestHeader = "13504ee3e949599a6f52ed8b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 440,
				BlockHeader = "0396414b0396414b6a6e41ec3835ebfb511008b815336d8ec6dae491c36a48285a320cfff7ed29d89d3201ad0133baee5d1fd583316fe77ae5aa49b4745a0de1c75ae82135cd57b30396414b",
				MidState = "15e52a38e138315fa893de857f01148691a8eaa9f8373dcd489a5879328bf2f5",
				RestHeader = "21e85ac7b357cd354b419603",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 441,
				BlockHeader = "94a09ded94a09ded2d8d654a5770d7fafe941fe04a07c6eb5f1070ac034b39bf2346bc24ceea3b3090d289ca2431fea0d29d7524b2f8443dda16178200e46cffae22724102014a8794a09ded",
				MidState = "d6e8752c2fb0dbfc4fc91c29b19707b5be6175a6176c9205ea9d037c03c32017",
				RestHeader = "417222ae874a0102ed9da094",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 442,
				BlockHeader = "41b1ab7f41b1ab7f17899f741717a5b00f426b685e32bab52bba3fa4974710c3362b6e664c39ecdafebbbcbc7308ce9614e6bbaa5ea7c9e82870aef71bea10d5c7f1cd7a9fc2699b41b1ab7f",
				MidState = "0e75dfadc2af3828e4de8a3dfdcf2d5352ee9d1cfd6c0f8a835db33e7770adf7",
				RestHeader = "7acdf1c79b69c29f7fabb141",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 443,
				BlockHeader = "8ec538078ec538078028a8b0122516cc1ca4449f42b26e06835506fb0a031dff38d96b62db4e232b88c2fceef4f30fe2445288297daa9baed559dee6cb4e6c970a181420185c5ee28ec53807",
				MidState = "4c197e7743f3e5d91fcf2bd25043d251a71447029f701690c03dabdfdc20f300",
				RestHeader = "2014180ae25e5c180738c58e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 444,
				BlockHeader = "e60971eee60971ee3ec9ca9ff353c34be04032c877d0dbfc47fea5b7a2a604e7cd2de6cdd38360520bdda8d2af1dbf68f469c4a8139e4e9597f4ac8f02a47dd68660b818299101dee60971ee",
				MidState = "93b203f12a8dd4dc52e37a6bbeef9a7328f4dfc4f74643b575f5e9035d65a1c1",
				RestHeader = "18b86086de019129ee7109e6",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 445,
				BlockHeader = "c8647ab4c8647ab44ec589dcbb6f88b8b7d3bc5076efd42c2b712862aaa2dd08b9855080baa9b58798db0ef3d4207e2f7304230622b15886c41d736dd7640fb94793fa0c7275b475c8647ab4",
				MidState = "d7e60a329c5d42cd17ecb0f5641d7ea0e20fd77a3ab1df3f47ef053ab592bc06",
				RestHeader = "0cfa934775b47572b47a64c8",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 446,
				BlockHeader = "b1a1214ab1a1214a89a4a590ac99db0ddcfa47f5216eee2033437b6604d602bcc0904673c493f5394c4b660ee15597f07758bc509b2a6304c540fb4e11b5e73f9b5baa00507870deb1a1214a",
				MidState = "9ec1c2b77c147ed5a0632864a16c439c880711cb8b747d533532527877113a0e",
				RestHeader = "00aa5b9bde7078504a21a1b1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 447,
				BlockHeader = "9504caf49504caf4c62ec9f8d995bb988c0adf5e22e09f783ce8aa6d5835b1434b46577074b4c6a2c6ceff9570f3249ad2d1138ce0ea7705e4123f62b4592b6606dd4967b9ef70c89504caf4",
				MidState = "d1ab4efbb2aba178cb517d0506b3891dc2ff5827efa9c1fb00592b283b724dc9",
				RestHeader = "6749dd06c870efb9f4ca0495",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 448,
				BlockHeader = "312de403312de403986ddc70cfb19dd1a5cf91cce9df061bec1e371a3fadcaba4fa2516776dcc742de5e96bdd5d65ebd7a0046352c51379c91a5f16d5bd1d485f2aa4f2054415450312de403",
				MidState = "95552146709b279e1be1d653e444ade91d2341bcb55e13e0c31e473ba27f334e",
				RestHeader = "204faaf25054415403e42d31",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 449,
				BlockHeader = "7f41718a7f41718a010ce5accabf0eedb1316a03cc5fb96c44b9fd71b269d6f652514e638c5b43c24dcd374ba4ce3c0a00a58bca6a70ea9383f04efa88e73d386a1f0842315dfa437f41718a",
				MidState = "bc0e8a26d65f172664eff5219e65c7767e5d5617b168310e8e77ee235838087b",
				RestHeader = "42081f6a43fa5d318a71417f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 450,
				BlockHeader = "3df8f5c93df8f5c906bf8c06870700b387d66d1a2d6df2c99c973f8387e6bb1f5b681f298678d3346dac627a449d60ee66cd8d81a650ced1d49b7e5fa2a506c3a4324d3cd8b6f6373df8f5c9",
				MidState = "2b89a96ca8a6810024b12fa0a7c24f3a1f12cdf02e3fb3543427279d10d4acef",
				RestHeader = "3c4d32a437f6b6d8c9f5f83d",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 451,
				BlockHeader = "aa98727daa98727d1caed7e66d6f9065fa22db7791be783f4dc910fed0a67acca37f292fcd9b8a8a0beb4220018cce1f04688c0e0a303f2190ffd51dfada6f483da3a5007e3e52c3aa98727d",
				MidState = "591a8dc8b2f5d7bc3362f525b9efcb8d6f50e1c6ddb6c22e26cff77bde081734",
				RestHeader = "00a5a33dc3523e7e7d7298aa",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 452,
				BlockHeader = "7f2590517f2590515f0e11696bff22f6fa1738332cbcf9b3963a9ad7fdcf744c616b856c48a8206510d0dc5f821934a1207161540275b7e2b8217576a535c7fcb081d7727064c5617f259051",
				MidState = "88f1904b0c545d313035ba8bc1ada40251d5e1dbbb48e9d4bf3f3e44bf9dcdf3",
				RestHeader = "72d781b061c564705190257f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 453,
				BlockHeader = "60239f9660239f96ff0ae8ce69c8f1bf1a0b343bc52aaba3ddf13ca3b3cc0a897f73c952d38755b839259644d0dbbd8cbf694e8c92de1763bb5e009defff17d13a79ae027fad810760239f96",
				MidState = "ecd55ed1aca0eb001184dfb291014668f9c9131b633b6d01f538d149e04bc241",
				RestHeader = "02ae793a0781ad7f969f2360",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 454,
				BlockHeader = "4393873b4393873bded1f3aa5f1bd345c22a86a86e4d9dd92ca00722df3d35da61180a7806961c21a23816fdf0f45e084c3cbe33cbfe3899e4a8fd7c9e721c940f39e82d805658ad4393873b",
				MidState = "b6681837ba4455fb7f7c7b049e83590cc2c47570328685bff725840132652bea",
				RestHeader = "2de8390fad5856803b879343",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 455,
				BlockHeader = "aa333a30aa333a30e0bfd00545df8deba2dac039be1a134d1e5e92f880637860e23122793af92d879ef6d6470b6dc320043adec6090fda168b526cb8fc1c4920bbd0a57ffca83ba5aa333a30",
				MidState = "7fb3c47f43de8507ab8f2447b19073d3b08b558e94edd0eb6658b4b91eb9a0c2",
				RestHeader = "7fa5d0bba53ba8fc303a33aa",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 456,
				BlockHeader = "adbdd6c0adbdd6c00358a834459137e6e33a942fc0fc5b9a680eddb7a3ceaf97b53dfa50e62b382b4be6dba1bbe4372de152c90cb79fa697f351c3ca56f42c10639ae559a025186fadbdd6c0",
				MidState = "d98e3e8d77c6ef65c8496c132f7d529224c5d757d8549d642ad10e9a3f7c296d",
				RestHeader = "59e59a636f1825a0c0d6bdad",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 457,
				BlockHeader = "fbd16348fbd163486cf8b170409ea802f09d6d65a47b0fecc0aaa40e178abcd3b7ebf74c6d993ef400d83054aab2c349e15c3056cc2dfbed9905e8f6311a1dc051b1c40077508d35fbd16348",
				MidState = "32c90296ab7bdf1285f2d9abd8a5ca758102094995606e059c200cd1bb55894e",
				RestHeader = "00c4b151358d50774863d1fb",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 458,
				BlockHeader = "28fed73528fed735ac15404007a416b897eb6be287a0dbfd8d46b04988e9de03549780069099fc3b1a808d9cb135f275c918cd507a23014fbd0d51c97102bab7c77d6b53bd8279f328fed735",
				MidState = "fa2b543ddabb7b22e42313bda9cf43a9f0cec55af7da340219c3b7beee3e797d",
				RestHeader = "536b7dc7f37982bd35d7fe28",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 459,
				BlockHeader = "6f99877e6f99877efc49eb88272a0994d6ccf9cdfcd0a796db2b5cdf455bda3383326daad0c72e9af7fb88735372d73149dd0af33dc63e9fdcfe6feb57115a293399ef233e45bb546f99877e",
				MidState = "1f1860f7aacfb9f01e8c2281dc42f480608352fada0486a016535c1beeac79a8",
				RestHeader = "23ef993354bb453e7e87996f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 460,
				BlockHeader = "4bba20ec4bba20ec9c2bfcdb1ca0809c520e72e418dea4d75c9613ee685e751e4546f28be4dee8e0e6a1969eeb8a3ad445ba0bde5d810c0cf1448889d0df832260dac12db1bb2a544bba20ec",
				MidState = "6d8ee2dabdebb0800777e314fa713d9161902c27d315a10b0ba85021e3580696",
				RestHeader = "2dc1da60542abbb1ec20ba4b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 461,
				BlockHeader = "a4250088a42500881e4f2a1dfc89c7b9d0a41a58e70df48f8434dfc3255e8db876fbbee3bf6ecde0e9b2178f645b92900781c6dbc9c20e3ad8ae6e473480bfc289fa332445b66461a4250088",
				MidState = "4838c7f8341a0eee9dacc18d20b13f549558175cdc1498be791092385faaa0cc",
				RestHeader = "2433fa896164b645880025a4",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 462,
				BlockHeader = "bffabc9abffabc9ae1f7ebb15e2dd8c7e0fbfdd32847047f9fe7e58f9853cff68ebaf1092908d043d7546c7279928f6a3244bf458edc59e1d0be9b9d657a3eb2e531f77e4188d421bffabc9a",
				MidState = "956e12903d332010b0ef6e3a8365108bf614d401c502fa6c8e5ad364760c9ff1",
				RestHeader = "7ef731e521d488419abcfabf",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 463,
				BlockHeader = "5b23d6aa5b23d6aab336fd295449bafff8c0af40ef466a224f1d723c7ecbe86e9217eb00221b9c56fd5e2bd56112fb2e9ede6bf17f0504aa4e3b133aff1167104414870036e16e555b23d6aa",
				MidState = "23005331c132c36424ee346839ec1203afd14be240de9dae9d68787056a5bed2",
				RestHeader = "00871444556ee136aad6235b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 464,
				BlockHeader = "2f608e692f608e69c94e579ddb501f601e20c67ad1121af92aef08cdda8d667c7ca933f7486e78c109f1e18ef0562d4b21a36fa71c680297575bda11f030e673efac7d480b5ebe6c2f608e69",
				MidState = "4e141094184a1ae9fbb943a13bd94c72f534cc4ebfb353967638368247af1c35",
				RestHeader = "487dacef6cbe5e0b698e602f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 465,
				BlockHeader = "7c751bf07c751bf032ed60d9d65e907c2a829fb1b592ce4a828ace244e4872b87e5830f289319fce06dc206d55ded2f2fc57537dfab38d7b92c5cc451ac413f4cf0ec77c4192d3317c751bf0",
				MidState = "399e9085558372fabeafc57250642c9325827f561341aa49d51050577f3c2c0d",
				RestHeader = "7cc70ecf31d39241f01b757c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 466,
				BlockHeader = "ca89a878ca89a8789b8d6915d26c019837e478e89911819cda25957ac1047ef480062dee71773ff431f9538b40c6d5ca78c1b215cb52ed23ce37f390f05c5480c73238763528fececa89a878",
				MidState = "a50eb129ebbcd29e17aaf78fdbc3dbec2852815b9adbf20fdec1cda6701ed606",
				RestHeader = "763832c7cefe283578a889ca",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 467,
				BlockHeader = "189e35ff189e35ff042c7350cd7972b54347511e7c9135ed32c05bd134c08b3082b42ae90c17068a2f32460df68fea99b934d3416056a8621eaeebad645e2b5d66d7d24a2b9217e1189e35ff",
				MidState = "e95909472b3cec2c1d1863755ab9f6e08a340b46591f56bab75ef199210e98f1",
				RestHeader = "4ad2d766e117922bff359e18",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 468,
				BlockHeader = "cc03c933cc03c933df0d5eed0db55dfa8b3621655f5f90c4974b36e94f7a09aa6046fe64e83b9bf284769814247c1140627057c06085f247a490966d1ec0e86245274810b3cfe50fcc03c933",
				MidState = "e5476151b2c20ac41e797e3834cc48574241ad473245ac098c81b7d145b9630d",
				RestHeader = "104827450fe5cfb333c903cc",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 469,
				BlockHeader = "1a1856ba1a1856ba48ad682908c3ce169898fa9c43df4416efe6fd40c23615e662f5fb60a4686b8e55a0dc2c82e398d27b8797189f692ff036a44c6ceb65331f83a1351060bb03331a1856ba",
				MidState = "08c6c7930a553b91946a45530c3638592807bafffb1fa5f162cf09b8610e0b1f",
				RestHeader = "1035a1833303bb60ba56181a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 470,
				BlockHeader = "682ce342682ce342b14c716503d13f32a4fbd3d2265ef7674781c49636f1222265a3f85ba02274ae74e5a667d1f976a625fd7a525e0acaf8986036de08fea3617d35566fc6dba104682ce342",
				MidState = "eb187d9bb97c2fea1699cffabe5a7360474d21485a8f7fbaa252405e54ccd4fb",
				RestHeader = "6f56357d04a1dbc642e32c68",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 471,
				BlockHeader = "b54170c9b54170c91aec7aa1fedfb04fb05dac090adeabb99f1c8aeda9ad2e5e6751f5570dc0e6bcbf7a75c0a4d05e2169ee43bd574837c74e9294e2ac30d15b4877811b55c29050b54170c9",
				MidState = "16b6e3cfb4acfa2d399e26e06c40b5cb97164c701f86ac3cd5c8b047a933d5da",
				RestHeader = "1b8177485090c255c97041b5",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 472,
				BlockHeader = "2ba9b42e2ba9b42e233b75c94026a217f745f244b457ec23476388a7fdfeb9e74b2643c2920430e5b3c3558d4b7ae3f877323752bf7f408bd3c291a62fcd4952a4d73b3d6c6dc3a22ba9b42e",
				MidState = "1f683717a3645e63ffe89395c23a1e422c3976391a30b2bab0fba3dbfcbd0ed3",
				RestHeader = "3d3bd7a4a2c36d6c2eb4a92b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 473,
				BlockHeader = "ee072d8cee072d8c7159b6bfbdcda06a2c6cee03ac4ca7d7971b31985c79927a2cf9be4411bb0ddf2aab21456b8d1c36ad7c59400afff401ae3f4c793d00b5accdf0ec3fbf3a0c63ee072d8c",
				MidState = "25e24f65931cad29395d5e5326dbcda3a7a869206dcb4dca9adfe49a438a4ef9",
				RestHeader = "3fecf0cd630c3abf8c2d07ee",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 474,
				BlockHeader = "ae0fae2dae0fae2da3b6b8e5bf730b9ac9ef2759573d1139c7e6073720ed0f066d41335bf6da7b801578740cfeb8592e8e89111d94c28ad866e91b645e23d7ac6bc95a26bb8c3e58ae0fae2d",
				MidState = "513c745b0343a994f02a099370af9ee5ce1419443d7f966b552713435e33d9d0",
				RestHeader = "265ac96b583e8cbb2dae0fae",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 475,
				BlockHeader = "b608cd50b608cd50547e5eadad47acffc869464467e81060acc2aa76aa112a73d593eb8655f74f1b505123865b827ba9998b7e770d526c34682809e111683cea3952a446f88fe67fb608cd50",
				MidState = "a438bc1416f4d85ac3d76c15b74d07fb1aaa26ddccdce41ad0ce1f53fcfedfff",
				RestHeader = "46a452397fe68ff850cd08b6",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 476,
				BlockHeader = "f2764a24f2764a245077bf082cd149707dad50f25ccc6f6756b84366d0bfd01d512734ca64bd36d60257f67aa8b38b4a5e2033a03e29ae581a8e62034526f4c8a5abf315e8cfbf5df2764a24",
				MidState = "deb0ceca223ba5b16fdf5f736faed14a1f79943c9b686be6f4051d705b10ded6",
				RestHeader = "15f3aba55dbfcfe8244a76f2",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 477,
				BlockHeader = "408ad7ac408ad7acb917c84427dfba8c8a1029283f4c22b8ae530abd437bdc5953d631c69d41d165d434265b90d5632db061faf7aad5c73cee1b46351285cf0bbe4f99393ca8dbb8408ad7ac",
				MidState = "fd90554fabbd53e26c5ee04025a04a2028b4547a261a38407ebf62c401ebb74f",
				RestHeader = "39994fbeb8dba83cacd78a40",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 478,
				BlockHeader = "7220a94a7220a94ae1f913be53f8f3866cf2203d4724ec0f3ff96c5fe7322799a6babe4e5f0896f1c8de3ddbd0ed5145c6311c715b46a7c20e2e1f2a690fc18b4b8aa613108373367220a94a",
				MidState = "b27684e3f4723d79f2a03bdcbdd255fa2ed2b1824d881ab77a2bb9c384b047d1",
				RestHeader = "13a68a4b367383104aa92072",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 479,
				BlockHeader = "b66951acb66951ac18fa1ec380c61d2e6f670d2914f6ff7cfc2ed12123e6d10ff40858a6697c4050fe9bb5c9b923d0d91966fa4f0ba0a26138d518f55dd8a7bf5b8a9b0dc8cb601ab66951ac",
				MidState = "1fdcf8dd3068b29841da8d9f2b5c8960029ae81c5e3742cefd76df1639b76410",
				RestHeader = "0d9b8a5b1a60cbc8ac5169b6",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 480,
				BlockHeader = "c080d3e2c080d3e2d91dcd70dca172ef0193ba841620c1d249f442c8a8646305092792b17278bded637517f4f96b85922e0922280b9fdb5682fcc7d960620fefe6d0f12b4cc93db7c080d3e2",
				MidState = "a32bfcfec8ac7ace615bd8ece20bb9bcfd56e073f93a78ea4d66ee671102d759",
				RestHeader = "2bf1d0e6b73dc94ce2d380c0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 481,
				BlockHeader = "6065867e6065867ecad62ebe939f40d68f4b09ada8b64cd20fd6fdc8d02e1508e4271c73a44dc05de960821d7d1bc7f26dc01abf8a5a1f48f9bf47bb741b750ca0d4992a2539f3936065867e",
				MidState = "1185d0b0443f23f4a2c79f2ad1f663f19727d1bae0e32ab50492ea3ef770a0e0",
				RestHeader = "2a99d4a093f339257e866560",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 482,
				BlockHeader = "2445b9aa2445b9aab017875068b74845250868be13576b0603d1f5f791a67e2039d718d8db6dfd23070ce84d3e287b104b2fa56d76256ee40d6ffdae8e2a6cc30c71113bcfe138032445b9aa",
				MidState = "ee2cb081b89a0bc033d616d1e35cde547d8db00eb978f7d185ca877d92a312c2",
				RestHeader = "3b11710c0338e1cfaab94524",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 483,
				BlockHeader = "ff665218ff66521850f898a35d2ebf4da149e1d62f656946843dac06b5aa190cfcec9db94a7178199cfe73ccbd0226133a5e404c169c66d53303c44c9ae2994e5a3ec7174e60c831ff665218",
				MidState = "3e015b3e147ad6b952ba868da6d916295fec6224ff1374d6c792bd1869d809c2",
				RestHeader = "17c73e5a31c8604e185266ff",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 484,
				BlockHeader = "9b8f6c279b8f6c272237aa1a5349a185b90e9343f765cfe9337339b39b223283004897b01483d25cf68c27e9a279afd1fe0da5745781b0aeea62eb3db809148064a4ec50d5cc6ab89b8f6c27",
				MidState = "70f15cccfffad82adbcdc84122084b77684bc42412f58510751a62748bcd7fb2",
				RestHeader = "50eca464b86accd5276c8f9b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 485,
				BlockHeader = "9a9bcbc50d8fbfdf6e621d59f9fb600858c3f780c9a58cc19830f5b81ed40742266b9a588e2378f75e654041002496e0ac679722a51b10f078bf0b17f8d6a6b99e84ab7c0b2799850d8fbfdf",
				MidState = "b3436e2425d5649b6a24ee57d53bc7405b9fc9f250ad2f8103f82c8931854483",
				RestHeader = "7cab849e8599270bdfbf8f0d",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 486,
				BlockHeader = "84d9725b84d9725be0834023e48db948edc92205adb3f0a4c8d239742850bba5eddc182f40154c2be44e4177e8517fdb27f92ebdf3ec94884324da0f435b0b23d09eae4aac5276c284d9725b",
				MidState = "a9b92d3a5668e099b4a1bea32e0c4ad090fea6505c16e5126295d995352c4796",
				RestHeader = "4aae9ed0c27652ac5b72d984",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 487,
				BlockHeader = "885d4133885d413351b9e4ac97b163f9367c5bd5da566571de96ed41afdcbdd9b1f2fa09aa052b7f729e663147659b02433f60f4e2136e0f22779733907866d30204e409730172a2885d4133",
				MidState = "8aff35258221fae1ae4ade81ea49b7ad87c14882bd469345cfab6463847fcd8c",
				RestHeader = "09e40402a272017333415d88",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 488,
				BlockHeader = "3a4177753a4177759b1d9c03219d5264500594b366d4652fbcc197fe19071cb1881aec484f25a5f6e8b46ce274ca0a32d2e54a3f5281b7c60b89f3f5f0bc100ce5b8500ec884e7c03a417775",
				MidState = "739c42641abf3be95cd34455a34490202459136d9b6cc3e983873efcf0752150",
				RestHeader = "0e50b8e5c0e784c87577413a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 489,
				BlockHeader = "875504fc875504fc04bda53f1cabc3805c676dea4a541981145c5d558cc329ed8ac8e943a0afae09dc93f26dab26ebe40cbee9dd6106cec76f06019e52cd238dad0b523451090eb4875504fc",
				MidState = "1473a9ff330ae2efd4e96b4ea14a7248457bb7a32dd17849631fe49cf97f3bc9",
				RestHeader = "34520badb40e0951fc045587",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 490,
				BlockHeader = "d56a9184d56a91846d5cae7b17b9349c69c946212dd3ccd26cf724abff7f35298c77e63f314446ab297efb40dfcf437bf7bf1a72ebf256fab84b3bb5dd63c0167e61267543fd0087d56a9184",
				MidState = "be3aeef323cf74a885b726f9cd3eea8e19f36ff1f8c63c896d599f2883011e1a",
				RestHeader = "7526617e8700fd4384916ad5",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 491,
				BlockHeader = "0a7a7f250a7a7f2588322fdacc0f6695fa20be76ecf2f604019f0cb344aecea4938d40155357e7606fdc9c5155570e01f534cc53ecfb075b0aa2bb6fcac0dfdda257a9727da5e9a60a7a7f25",
				MidState = "0f4f4b3ebeca1fc6c297f12aecee9b8dcb555e266bcfa7a36174a25d6bba2a8a",
				RestHeader = "72a957a2a6e9a57d257f7a0a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 492,
				BlockHeader = "b0a3232eb0a3232e94e9d8e6151ba419c58e6ebc807ef48776ff7360e99325137dc96f1b1c958a0dc57ea34d500ab94e763d1cb4b2b0da138e2fc1b94047004d3a7c17059dae4bf1b0a3232e",
				MidState = "5f2166ad18fefee2b71148c17b492452bc927b366d7c8b66c05ab75aaa43b77e",
				RestHeader = "05177c3af14bae9d2e23a3b0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 493,
				BlockHeader = "f46ec83cf46ec83cc2f455eedebb5c9e31e622bc4de0971a57b47786fc4e305a00c53a01a907b7685b55e801980685dcfbad2cc3a46059eddcab22da5038fabf949a6457fdfb0b74f46ec83c",
				MidState = "244dec7dc0ba072b2a9c3718a31a0131d31083559671722e4dac64d8b7207109",
				RestHeader = "57649a94740bfbfd3cc86ef4",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 494,
				BlockHeader = "428356c4428356c42b945e2ad9c9cdba3d48fbf331604a6caf503edd6f0a3c96027337fc43f4c77ecff96deb598d4039744299afc872d81af88e012e43af90f77815095960402ceb428356c4",
				MidState = "9a1f776c673199e7dc1e0d7e13a61e7c9f5f89493a2d94bb61efaa25cf0f0ef2",
				RestHeader = "59091578eb2c4060c4568342",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 495,
				BlockHeader = "c7449d4dc7449d4d776b44f6dc80e5b371a8fce123364841bd8c61f13db99e98f36f49ee6f6a1bd0a331272dbaf5b06d7dbaf5c3218ef604cf39cec4023ea52ea4c8314ef77c6c82c7449d4d",
				MidState = "b52dc6c5e48c1f52a723bf316f0ebb311d3cbd90507354274cfc8553260abf5a",
				RestHeader = "4e31c8a4826c7cf74d9d44c7",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 496,
				BlockHeader = "cc8a6fbd3f7e63d7a3aa3d26b9d2f7cab9b5adba47d0ffc49821cddbc9e59f306383942899ee55e679979ff2f21982b712944d0baa1040a160d31c71b823fec70466691e522b5d1e3f7e63d7",
				MidState = "7fcc2e18d34eed41c24fc6f4f611b28a32eeebb75ae9d28cf5ec48001dbbc4c5",
				RestHeader = "1e6966041e5d2b52d7637e3f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 497,
				BlockHeader = "4637e1074637e1070844bf839e00071d1fb262d686ccf2b4906aac5b699c35ec0135f76952ee314f49cb236dfea403caffecfe7c9cbc8e064161eda8fc05177a104b192102196d644637e107",
				MidState = "5b30cc02d87ffeeb954ea90d79a1e91fc1fcd71c6e120c8894dbbdabd6bf3491",
				RestHeader = "21194b10646d190207e13746",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 498,
				BlockHeader = "21587a7521587a75a825d0d693767f259bf4dbeda2daeff411d6636a8ca0d0d7c44a7c4a3776b03c939562d29b3e36036bfadb1063c51f9565ce42f9045305b1add6ff4ed3d5eef221587a75",
				MidState = "e7ccd6f6b02cc9963183803dde186383a98b5926d6b8ce3f2e10f599529812c0",
				RestHeader = "4effd6adf2eed5d3757a5821",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 499,
				BlockHeader = "952ddda7952ddda7dab4f1614359e0b17a332058a3e0c87e7160b8b492836a02e68024d038a6c9dbc9c52b0c5dd44ac2e267bf240aec258e07ee5719e03ea02ad2edf860a62701c3952ddda7",
				MidState = "87d95c37ee93b839fee91417a16256ce21e4b66c166ee88768730b68ac2b3ad1",
				RestHeader = "60f8edd2c30127a6a7dd2d95",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 500,
				BlockHeader = "0a783fff0a783fff7bd894663aba463797b085f16e177aad3a86d697bda139d500cc9593a461733e7a4268b4f6b37f850d599f679c0e6bda9e08e5a6fae0c79989b25d27257fb13c0a783fff",
				MidState = "8ceb0a48b8c9f88e039c9f4d648212374a36993d4c2c49ee10032c5d23766fe0",
				RestHeader = "275db2893cb17f25ff3f780a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 501,
				BlockHeader = "f3b6e796f3b6e796b6b7af1a2ce4998cbcd71096189694a142582a9b17d55e8906d78c852a33acbc10cdd268cb116ff76309eba0dc4b249b83976f39f6ee6fbf2c1a4a09591db022f3b6e796",
				MidState = "1695173bc07baa90a820e6968dccf5583cb92449da0cdccd5a84a598739dbb92",
				RestHeader = "094a1a2c22b01d5996e7b6f3",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 502,
				BlockHeader = "3979b11b3979b11b7657137563592d3d4e81f6a3c846597db4c9bb4331c1155edadddd289eb010b85192777c974ea735232a6a8ad1f48e385e37cf972d2d3e69dbbead4b6de7dd413979b11b",
				MidState = "b0f7c5f371773a7034da05647b768510f8de07dcb31fda182320eec39cc0cfba",
				RestHeader = "4badbedb41dde76d1bb17939",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 503,
				BlockHeader = "d0f9c83ad0f9c83a4422a5495578de6025c37d9baeed329231cd0687304c1afd687da91d24f9a273ad7aadcf2b1094b63f517ea2ae82f3736a2f9c524a1f8430e4e32963afaed624d0f9c83a",
				MidState = "9cc3a6d26438b78f625d7318e3b6b691debf42c6336a17a411c5bdd61ca7bd9a",
				RestHeader = "6329e3e424d6aeaf3ac8f9d0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 504,
				BlockHeader = "b93670d0b93670d07f01c1fd47a131b54aea083f596c4d86399f598b8a803fb16e889f10288268d36183a5de7c668b805eb41e59c504b7d0ff4a57fc5fcd773f28613749e93f52e9b93670d0",
				MidState = "5de2a12c7fdc074086977b590a71b4e325a08d3a67ae77011f6b0dca0dc3f116",
				RestHeader = "49376128e9523fe9d07036b9",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 505,
				BlockHeader = "152e65e7152e65e7268495356c6107c4387c165efccf31a0740d37def80f12a751e71c4f6084a44bc8ab5ce9b54390ffb1c5ffdcdfb9d150b9403fdace373dae62fcde1fce6c9506152e65e7",
				MidState = "5a12f33b4602469787405f8a2938640a606afec67c4961bb9e2b3cd6005bcc11",
				RestHeader = "1fdefc6206956ccee7652e15",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 506,
				BlockHeader = "bdeb3020bdeb3020fe8f96b60c9ccc4f53a29f16f248153d619d3da49d2deae38e1a8ae274364f299cf5ccb822d17549027c14d41b349c922fa69e81d4d607bd7c02194b03641f90bdeb3020",
				MidState = "6facfede1d049a50642e22f56ea3eb4e55cf59f9e0236acf75a01b7b64727cb8",
				RestHeader = "4b19027c901f64032030ebbd",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 507,
				BlockHeader = "b63a2953b63a29531f7deb9624a130b7ccca7eb9b255e03e2cab082b756a5b1209e74ac91c68cbf3d3c9b020c8210699e3e0991d46c20961f6edc345f2c1bf0457fa9c363861e78fb63a2953",
				MidState = "118771808c47ad2cbdd63277ef0382149b11017f595f1ea0f06cfaec6bbf1bef",
				RestHeader = "369cfa578fe7613853293ab6",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 508,
				BlockHeader = "044fb6da044fb6da881cf5d21fafa1d3d82c57f096d593908446cf82e926674e0b9547c5df237a6581193d5145009c1d635ffecee6cee76549ffd633aa102ad174aeb64c14a1c811044fb6da",
				MidState = "f351cceadaf9d32e56e5158bdd5a964a73a939ac5737aa034db2037d8c403785",
				RestHeader = "4cb6ae7411c8a114dab64f04",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 509,
				BlockHeader = "6052421660524216c4e490c74946a4b849a6d449f7be9d9aa7f4b376c64a3e3115fbdbb59173e734a6d5ee1dddc90d2b81a69205d3f2db7fe05148b38c372fb7f5f45d643f0f9b6460524216",
				MidState = "b15decfff75982ed4df8fc1a4b09646f52d183cecbc5b2c406ac8d73d24bff45",
				RestHeader = "645df4f5649b0f3f16425260",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 510,
				BlockHeader = "a22874c5a22874c59bab8d1568653848c4578dc7252cdef9b7290cf4b6d3940e17e10d4f748e57d6abe933a356da6425f1b89a966b5f48aa816430b2e11dfc2b17cbde2ad99f1d6aa22874c5",
				MidState = "058e529114100c7169e96784664b301d5f8076e72591289e67d0dbc4cfae41ac",
				RestHeader = "2adecb176a1d9fd9c57428a2",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 511,
				BlockHeader = "2059b5e62059b5e63b39abe5064eeb5f0429dbd2583dfcc69c67ad892dce9d4eb743cb86d11011a40cb346410b718f5f95882634ff680f48bd0b0312e96365769e2ed3760ec4a51d2059b5e6",
				MidState = "6f5b5b8359b62ec74cdfe1319aca899ae588df9fa4787406a31b66e8c0fc39fd",
				RestHeader = "76d32e9e1da5c40ee6b55920",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 512,
				BlockHeader = "bb82cff6bb82cff60d78be5dfd6acd971ced8d40203d63694c9e3a361346b6c6bc9fc57d89f71f32ac57082cad8a8114b5d1552109551de8389b41db463165509931ec29b30ec885bb82cff6",
				MidState = "2f2c046bcb2c69993a2e33ca870198e3144b41de2f038ebe8cd602ab43fad4a8",
				RestHeader = "29ec319985c80eb3f6cf82bb",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 513,
				BlockHeader = "59a0927159a09271d8dac4ef9ccc33fb4da66a24cba14dae1438e925360a378360e18124f88b37f1c7b0f909ac2cdf2fc480d35ccc63cdcd0df9dfad0c810b6566a6dd69f5dd648359a09271",
				MidState = "54044a1734bbc538fbb4e528ae4d9a0d71ac02092e2e82e214a5c4a8a0fd7ed3",
				RestHeader = "69dda6668364ddf57192a059",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 514,
				BlockHeader = "34c12bdf34c12bdf78bbd5429142ab03c9e8e23be7ae4aef94a3a034590ed26e23f50604d384d4115212e53a813fd953ce16b87725130e41211acfc7be6ae68cac496c408cfb726c34c12bdf",
				MidState = "430c587aaf29f037cc3dc42b69140a46961125e3e681c01bb6aa65d8a095721f",
				RestHeader = "406c49ac6c72fb8cdf2bc134",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 515,
				BlockHeader = "f090e0b3f090e0b37777a9a4ee7dc93412cf8cd9aec1503f3d9918cad6de248973fedb76bbc28fad6493701e84e3ba08ef0e3b63cfcf9fe285c947fa342dd1185eeee04f9638402ef090e0b3",
				MidState = "2a2e10e3911c0ac009c413bd1c0ef15035af24aa2e39ea0903d63c7adc99f3b9",
				RestHeader = "4fe0ee5e2e403896b3e090f0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 516,
				BlockHeader = "3ea56e3a3ea56e3ae017b2e0e98b3a501f316510924104909534de214a9a30c575acd871c27feb1000621a5070f365836e881afd3854bfcd53f177938859aef9cd575b68f5125ccc3ea56e3a",
				MidState = "1ba0800b0409222c99bbcf2f7c2aba648cf7c7808b7455cca98b9c0f63f4de40",
				RestHeader = "685b57cdcc5c12f53a6ea53e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 517,
				BlockHeader = "fe867fbdfe867fbd098a60e7886c70c84036ea1d35392b6f90b194886362c502411440db09a30301454a6ec2281acf5b5dc30064b46a2d660caeb921ab00d1b3f0bc96564b91c338fe867fbd",
				MidState = "11c466c4c63a2b6cf7a5680209ec61c848de84a51980f3ed8556f522f164b8af",
				RestHeader = "5696bcf038c3914bbd7f86fe",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 518,
				BlockHeader = "9aaf99cc9aaf99ccdbc9735f7e88520159fb9c8afd38921140e721354adade7a457039d293f4952819f19b468a06e588cb651a8d303efd545e4b1a26f49842a9a294d54bb13a575b9aaf99cc",
				MidState = "db2328aff6198fde414da8e5c63359b0e54fa5168762e0857b3be3fd8a57044b",
				RestHeader = "4bd594a25b573ab1cc99af9a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 519,
				BlockHeader = "c105639ac105639ace2e6ec38ac868405543b23d383c7cf57b24daa6c4203a5b971ef984655d24e6507a372c8ea2c8e5163492f821c106d90769e59870b075c16b4c2e23f2e4f7fdc105639a",
				MidState = "63ec60e39ab97f55bfd5330ce04a8e2ce7a6a981bccc04b8307708597a888f33",
				RestHeader = "232e4c6bfdf7e4f29a6305c1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 520,
				BlockHeader = "2ae527ff2ae527ffb7e67b4fa6aa5427afd8a862137a760ef1c32135d5ed637afa0b80385527a288910aa1435f750189ae21849b8c70db76e4788887c939ff31f5cfed025ee4f39a2ae527ff",
				MidState = "91c281b786301fae4f7b74394e4bdb68df57bc05bdd3f9eeb1f85b8b93378765",
				RestHeader = "02edcff59af3e45eff27e52a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 521,
				BlockHeader = "78f9b48778f9b4872085848ba1b8c544bc3a8199f6f92a5f495ee78c48a96fb6fcba7d33ff340ab8958aa61ba34eb82a955d49c8f355b5404512e63ea9103f1e7fcee2542c88206b78f9b487",
				MidState = "242991542c10e93c4caaa86ed3078c8621e2e516c603bd09119591ff273f503f",
				RestHeader = "54e2ce7f6b20882c87b4f978",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 522,
				BlockHeader = "215606e8215606e8209f0b6c1dcd6a6aa4f2ad2a9cdfa7059f4279ed07ba5aadc2932ea0cf41e7f3990b048390a95ddd6ad0932ea9143e5bdbe4815757e3f9b902cc2d3120d8fa43215606e8",
				MidState = "25533aa179549b31b27d84347df2ef5e7268ab412abb7b3571d4cf1668bd0357",
				RestHeader = "312dcc0243fad820e8065621",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 523,
				BlockHeader = "6f6b93706f6b9370893e15a718dadb86b0548661805f5a57f7de3f447a7667e9c4412b9cc081b88e223c6f4dffd7f0d4b3f225b642c0d92e3b1479bb1ff48b966222e9338f8e82106f6b9370",
				MidState = "05e004cbf68b8338c24200276a05ffc9636d6d31f5a3e4b5dae761e6f3b2d3ff",
				RestHeader = "33e9226210828e8f70936b6f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 524,
				BlockHeader = "bd7f21f7bd7f21f7f2de1ee313e84ca2bdb65f9863df0da84f79069bed327325c6f028978eed8ea8a7d45f4fa13ee0947dc60b1fa156c05c086eaac469a1e69165c4302846c6bcc2bd7f21f7",
				MidState = "d5d1cb973d47f26e8c3de9b20013af02ce5278cdca21ffa0a8864957c0eb0d2d",
				RestHeader = "2830c465c2bcc646f7217fbd",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 525,
				BlockHeader = "67ee04d667ee04d676525086cb793858f91d868ec86ad08bd69cdc63c9e824aa1a2a52d0d0ead27f16a4fd20929e879fdb93b80caaf474735db67511898468571d8eee2d02ad897067ee04d6",
				MidState = "660b7d7cd312db43199ed06a5f6e8736cfcb7e9de8538fce1e470dd13e8f3601",
				RestHeader = "2dee8e1d7089ad02d604ee67",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 526,
				BlockHeader = "c173823ac173823abe9b1707e289784f5eeb5f3ecd69e2966509ad73ea4ca407b0216753a688c5a6904847c294029015adbe0715a2663fa5b4faf4ab2ce760f8c4f7f324776d363dc173823a",
				MidState = "f86e694c0ef43954e660e704bae7444cc1b880ccecda59b2ba9cab3f4ef0b94a",
				RestHeader = "24f3f7c43d366d773a8273c1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 527,
				BlockHeader = "0f870fc10f870fc1273a2143de97e96b6b4e3875b1e896e7bda574c95d08b043b2cf644e028c734a331e3e81a4db0e338d0885268ef49910565af6bec6d00589c533130ca259ddec0f870fc1",
				MidState = "df51248bbdd12feb4df385d9d9455ac5b7f4dfdd92457fa7582a2f5070a4918f",
				RestHeader = "0c1333c5ecdd59a2c10f870f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 528,
				BlockHeader = "aab02ad0aab02ad0f97933bad4b3cba48312eae278e8fc8a6ddb0177447fc9bbb62c5e4519faf503e23d76572681a4d557de70ae34e20b0e09c985f4fa23665afd833071ef2d9cc8aab02ad0",
				MidState = "4b72d09acec19309302ed9a17ac72ae79cce6c9278ca001c884b2af0483fcdc4",
				RestHeader = "713083fdc89c2defd02ab0aa",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 529,
				BlockHeader = "ea37a2f9ea37a2f9017c30a4f3d56b8a3f34cfe2e9543887265e168fba1b577e3fb09c3aa590f72f506dc39319d7950b9d4a2395b922b572095a37de6ad820c51bd6d55fb109e193ea37a2f9",
				MidState = "f8e45b28e94d645f936509da6c4b3ca91fcf08037340020e7217ba5f6b9b17de",
				RestHeader = "5fd5d61b93e109b1f9a237ea",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 530,
				BlockHeader = "09a5d39e09a5d39edbe9ea8b9e64d7291c22584f76d9e15bf43dbfdf351aa52a11516d93cafa22b6d960374ce7212c2901f0506aea9313d4e2ba4e9658e46816f58a1d155359256909a5d39e",
				MidState = "ce74c8b9fcf9f91a8c469043f54896def358d3234bd0dc9f2d72b819189a95ce",
				RestHeader = "151d8af5692559539ed3a509",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 531,
				BlockHeader = "5c048864cef77c7d206450d57f2c75b03aa2eccf80dfd39e943ac35243bc62dcb26ad1b50b8ad8dd595d16df352c16925ed1fd6899e49d8295e4ffdb7beaebe8c1ce372e65f6e7bfcef77c7d",
				MidState = "84f9d40307e31dd6d0b9e082b1a4ae92d82041be510ebc159d6a569f18e3b441",
				RestHeader = "2e37cec1bfe7f6657d7cf7ce",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 532,
				BlockHeader = "6920978d6920978df3a3624d764857e852679e3d47df39414470500029347a54b7c6cbac09a9b077bf7ed25a2c68a92e183f5c5773d7fca00e7fca0e4e4ecb4b6874546848dac6ee6920978d",
				MidState = "88d64462d19dcd7661c144b17ef4b6cb50e902cf048bfdb6a544a5e8a3ab0961",
				RestHeader = "68547468eec6da488d972069",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 533,
				BlockHeader = "e780c507e780c5070adc8fdda69f89df727bd46fbc935a648aaf7c5fef8f6e6fe21ea8dd01cfbd6caf4a0c30b2516aad2b4f17bc165cde4538d8c59490b9c3ff39e44a1b7024833ee780c507",
				MidState = "3ee8d454b7c57939730c7519027df7703f0f3426a393f0ff17a1cf0e2a766a00",
				RestHeader = "1b4ae4393e83247007c580e7",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 534,
				BlockHeader = "c0cd6465c0cd64658609d261bae5faa1bffb50328d2d96790eadff7f4d940f25395458e0e7a761f8fa9bade9aa50f89340b39256380ad76d144cb6a23ff7a78b097438060cef8c55c0cd6465",
				MidState = "2e89f81b3712e4dbd99a0d491bc4e0077cf9a04b1b7ab25be8c872d6f292bd75",
				RestHeader = "06387409558cef0c6564cdc0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 535,
				BlockHeader = "f5613a74f5613a743a53bcef2d6df0c47491399200d02d12dda816e93fb537617b838538fb60c3e78c63825c59f80599f231014dbf7cb2f9f1b7a2350442ad53b6a2368594c7a0dcf5613a74",
				MidState = "b7a838f6e21cdd53731bda00a687e7b500f2b91848b94778c95f4289d2510877",
				RestHeader = "8536a2b6dca0c794743a61f5",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 536,
				BlockHeader = "4375c7fb4375c7fba3f3c52b287b61e180f312c9e34fe0643543dc3fb271439d7d31823419999de5e5ce81d350cda542ceb75afcc4358c4135540a73ab339fa4ed12caf971a45eb04375c7fb",
				MidState = "80b865727acb8ccba8f0da35db9b1568aabea5af8abb7982bc98fc1adc773bcf",
				RestHeader = "f9ca12edb05ea471fbc77543",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 537,
				BlockHeader = "de9ee10ade9ee10a7531d8a31f97431999b7c437ab4f4707e57969ed99e95c15818e7c2b2fb7d443350e40d11628c17eb78fba19610c0ff0445430aead384cf7674027ebfbe6b3b3de9ee10a",
				MidState = "87881fa71ef871bfa821d67ae79e1c445026a853f29ba218283a2d3bd164746b",
				RestHeader = "eb274067b3b3e6fb0ae19ede",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 538,
				BlockHeader = "3a6cb6813a6cb681566bd1c2ff779b939fb38655e24ffd4bf3d3546855f77a33e8edcf6dc61388019d5bd0a589af05c51249ae2b61ef1a8f6e84420ee1af3509675fa9f37eb171143a6cb681",
				MidState = "df6ea20fa0dabca7d8330a24c30930769abf3df66fc082734c6976dcd6d4d703",
				RestHeader = "f3a95f671471b17e81b66c3a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 539,
				BlockHeader = "8781430987814309bf0bdbfefa850cafab155f8bc6cfb09c4b6e1bbec8b3866fea9bcc68834d15bc5f1a3a1dafa8420f0c18339054778f3336edd7d547d57a6f19051fb81770821e87814309",
				MidState = "c7dc056f3c0b1ea378e1b484fca3ff7e1d00129be021e129e2040a77f477534c",
				RestHeader = "b81f05191e82701709438187",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 540,
				BlockHeader = "d595d090d595d09028aae43af5937dcbb87838c2a94e64eea309e1153c6e93abec49c9644a28458505d86ebe2dbe700d71160ba5ec770ca570f2194b1526c1330aa2d7d19f5c93e6d595d090",
				MidState = "e4b49310e22d8e05c23363a0d89aeff95892a65ecff653fc466f8fb611591e02",
				RestHeader = "d1d7a20ae6935c9f90d095d5",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 541,
				BlockHeader = "64d12b7c64d12b7cb6291fd06b427e3c0701dfbf95cf1d6b88d0780764604daeb0da06c7bc92bd6dba7245fbfe3468180b759112e30ff47c952d9ba46899a06ffb6f4ce3c25253b864d12b7c",
				MidState = "0111d9888b83de77557b1cc18bf02e8e8c480c026331abfac8d70ea9ab380c58",
				RestHeader = "e34c6ffbb85352c27c2bd164",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 542,
				BlockHeader = "da45d2f9da45d2f9b1df6c54b19d3ba3aa846542b75e6359d17408c70e2c28499be6427ebb9e19f92bfb03a2a43b6fb9635fd1007f2c285fa6da2c4594d1de6f332be4ccb4f0b2e2da45d2f9",
				MidState = "d1c89cb9c271e9c03a60423db9393a118c47cb4e907b9488339c401547e66d36",
				RestHeader = "cce42b33e2b2f0b4f9d245da",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 543,
				BlockHeader = "9eebf8929eebf8928cf6bf3233967d550bb100a7f62aa759444e354b9fd328c11e39e148c4ba24a8f4f9b557ee6202ece9a423d5b7d954898690ce3bbca186b4599c81d3c4bb04729eebf892",
				MidState = "5d4f5a114242211b92180cf6a4242826192c80c40fc2f72ca92aa057f2700cd0",
				RestHeader = "d3819c597204bbc492f8eb9e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 544,
				BlockHeader = "976866339768663324c9530a98e37aa4358035bb2587289ea73dc5de394cfea4445de71a87ebf13b58d46cb4261bdeeaa0ebdc0bb1f80a3c0493ed792416e1355b2eb0b315615e3497686633",
				MidState = "04a037796cdfc15d1aff3bb804b55d682e7d17ab413979542feb41456d42fbaf",
				RestHeader = "b3b02e5b345e611533666897",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 545,
				BlockHeader = "18c7be0218c7be02a73691691b0cf44ac13c93e0da23117b81f76fce45ae1945d0365b1fdd8fd77ecfcf4e1abd5e3a559e2800d80c0c28a1d98ece559aaa7e02bf7bfa88c6a1f82a18c7be02",
				MidState = "450eba70fcba61ba66fbc73665c9a6c17f1c757e5091363e9e502131d0d237fe",
				RestHeader = "88fa7bbf2af8a1c602bec718",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 546,
				BlockHeader = "0ee65daa0ee65daa855e6c5309700c63e62a00a78bc3e5c8d64de1d5147ba5caa0bdb78df21fd68597022c9a8ac14febac0e48f0a77ed5eee29a7b0d67147719eecbee9d284ebf900ee65daa",
				MidState = "c75a70ae21bf717ae76230097916a8b49e943b1b6699710bafe7193a535ccb56",
				RestHeader = "9deecbee90bf4e28aa5de60e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 547,
				BlockHeader = "5cfaea325cfaea32eefd758f047e7d7ff28dd9de6e42991a2ee9a82b8737b206a26bb488d840726f6b6c7471d22f1371a8747a5b576f3ec10796d57fd242159c74d1be82f95c810c5cfaea32",
				MidState = "09b2c1e2325a54a5667705f83a71775bdb6b753ef8af30e064c64fb6f8cb0bf8",
				RestHeader = "82bed1740c815cf932eafa5c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 548,
				BlockHeader = "6917f85b6917f85b8afa80f0013259cb9b72ec6cfdb4b6cdb54f4420be673bcfe561269a48ed3bc76cdbac28f63150bc77274fb55a0953561d4ee3986b45ded886c3c9a44adac0516917f85b",
				MidState = "2e5e49907ad6d0a12c503ce0ed10cfd3ca4a2ef756dfbd481db7a8005d9ed1c1",
				RestHeader = "a4c9c38651c0da4a5bf81769",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 549,
				BlockHeader = "cf6d2030cf6d20304f19b64c1d619ab8b5a70d9bd60f6799e13d40f1abc82db87643d57659198d57f3d1aa6c687d51f54788dc291568037561c600cb0c5187a7b68692f18340f9fbcf6d2030",
				MidState = "db02c8e18b192367284a7402802d40bb90ffd125fdd3e57a934e444c602b2ced",
				RestHeader = "f19286b6fbf9408330206dcf",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 550,
				BlockHeader = "eb6536a0eb6536a03fb7fa8c7f092e9c0af05f6b721719b85381d0f98f307361486edd5d796137fec37b13f5aca9087b8481b3528933c07e95c701ff24546c0cba4a61f14569efc1eb6536a0",
				MidState = "7e16f0694873a58a1eb4d26938ec2c43d9fc4a1397b6ad7c1e9bca11b0108e77",
				RestHeader = "f1614abac1ef6945a03665eb",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 551,
				BlockHeader = "5a6bfcfd5a6bfcfd645975296ed1eafed0b3f1e241268d28563b914ce6b9e871f7868c3ae0ba001259588d290a083f2dd36dc75c3dea4a597cecb8b1be00b0d8a04bb9ca6cfaf3045a6bfcfd",
				MidState = "cdf87e59dd99c0d8d829adcf22dbaf7de2d803464d066c65ddb7b1b7f4198c4c",
				RestHeader = "cab94ba004f3fa6cfdfc6b5a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 552,
				BlockHeader = "cfa70ed1cfa70ed155aa8cc0a732c69f6e42426d0a3244f4e6dc1c20eed9df90efd44366fbbc9e0ea8e4edc8cc20fdb1eda913806512d7e4e00785c6465e972d3819a7ad3b6d58dacfa70ed1",
				MidState = "309dcc8e942d9d1df2f129c5e65ab7b6e9a57f5debd40630fa1a25f61b863d10",
				RestHeader = "ada71938da586d3bd10ea7cf",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 553,
				BlockHeader = "25525d5b25525d5b916d0a077abaa5a0176e70ce4b4b1e87e072f81a2f0ff508bb2e318b21328a7cca868b459ada67ce2877faff5f492e28b12465cd320490aaf4f7d99e79e9f3db25525d5b",
				MidState = "bfbac2f34e9b6196949be33ddfcae117972496499d4bd9d1dd9f242eab7f6ac6",
				RestHeader = "9ed9f7f4dbf3e9795b5d5225",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 554,
				BlockHeader = "0e8f04f10e8f04f1cc4b26ba6ce3f9f43c95fb73f6ca387ce8444c1e89431abcc138277d5f29b96f5b01d46a14948dd75978c65640cde27e29cb37cd0ea6a1784b6bfd9d2cbe79900e8f04f1",
				MidState = "2e9d426f71e27accdff1a6ab42dc9423110ea2c6e84f26081f9ec1a71af8a27c",
				RestHeader = "9dfd6b4b9079be2cf1048f0e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 555,
				BlockHeader = "5f5bd8845f5bd8847247ba7eb5af0e2a614c2e048feb0bcc6a5597d8f9c075be62186da07d268f387036cf332b02685f49c7759bae0b20b8e24292985262962acc28dca9ba25a1ff5f5bd884",
				MidState = "f978750a65de0352f7f233dab27a86af1d84f7d4d33a74f5168d9c4d2d0a9905",
				RestHeader = "a9dc28ccffa125ba84d85b5f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 556,
				BlockHeader = "ad70650cad70650cdbe7c3bab0bd7f476eae073b736bbf1ec2f05e2f6c7b82fa64c6699c21e6a810a1c6ef6aa1cccadfe3d8d4f8da0a1e6ef6d29fc3ffd0b2ff4ce7d5c1a180b435ad70650c",
				MidState = "c8010cd136f13eff5729b2f89f2af8c070d98822e6728cd72507d401deaa2736",
				RestHeader = "c1d5e74c35b480a10c6570ad",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 557,
				BlockHeader = "1be5bab31be5bab3c56170803b984b9aac30eaa754c9421b143e6d0bfcd1669468738c5dd696348b3f3310a520d7c0df130e9c21457c62fccb2774293c0e6f41b0fe028e8ab780c81be5bab3",
				MidState = "de270fb8dd4221a870ba612b8474024539b6a8c7dda0208d6e1f56d0641901c5",
				RestHeader = "8e02feb0c880b78ab3bae51b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 558,
				BlockHeader = "aab6dc23aab6dc2340a3decb1e9e566c4c0b4b1f30e7747a173f88a6fbb161ea06c572cafd3e373c9257085bf7f62a762b950ecd043903674015be99e3510efa8db487fc436c8584aab6dc23",
				MidState = "a0fab235fb064864fa49dc2bda877862871c8be9840239c5e3ef1ffa778970b7",
				RestHeader = "fc87b48d84856c4323dcb6aa",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 559,
				BlockHeader = "d66c4e81d66c4e81cc0b86a4cfc7b3807b37dbca2d4ac5a0842a41d1f812ccc907e34d742b4d4a20d6bdb7d259ce1bd36e4ba5363f164a5a71fb4c03b823217d86f6509a695a1a0bd66c4e81",
				MidState = "bd0cd29f3db5652ca9da9276915060409cd334cbfdf8113a44f5c866e32d90d6",
				RestHeader = "9a50f6860b1a5a69814e6cd6",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 560,
				BlockHeader = "2480db082480db0835aa90e0cad5249c879ab40011ca78f1dcc507276bced90409914a704a926e3433cbf3e30899d3f03fb960a026511d646ffd1bf159fcf9c363b813f1fb6db8c02480db08",
				MidState = "48a1313214fd05fcbe09777b890f43c276812ba63d2f4387b4be26ec172912c8",
				RestHeader = "f113b863c0b86dfb08db8024",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 561,
				BlockHeader = "71956890719568909e4a991cc5e395b894fc8d37f44a2b433461ce7ede8ae5400b3f476b2da00c9390ed182c69676fc6f4887a59e3b89085176e50041e515ebff071c6fcfd3ee93f71956890",
				MidState = "968cf53b94dd122dbec6cf7ca968dd8f8687b7c7a54ee44c14d47f779bd2dfa7",
				RestHeader = "fcc671f03fe93efd90689571",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 562,
				BlockHeader = "bfa9f517bfa9f51707e9a258c0f006d4a05e656ed8c9df948cfc94d55246f17c0dee44678c4c9d15939748b8aae6618875cc5d0193d490c3ea753abe2c6f473e88602f8f8480838bbfa9f517",
				MidState = "5891cb6dd6f3ac844ca0c74827b1305c158ff763f80becde45a9cdbcd8ac6a3d",
				RestHeader = "8f2f60888b83808417f5a9bf",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 563,
				BlockHeader = "e98d4b4ae98d4b4a2073137a552d91fd3a3e7280b954aaeaa12824830b3e8edef2e58d11dc265a70ca9fe48b1e9e424d1e692244bd07ab7566ab34c1ac92660309576bc101dc5b87e98d4b4a",
				MidState = "8d78c4d45ffeefdb73921c501ba3f39247eaa5f5344a292fe7982c571c33f864",
				RestHeader = "c16b5709875bdc014a4b8de9",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 564,
				BlockHeader = "e5dca10ae5dca10a89466aa9d5a3fe00d30bfb39c019909115f367f9cf0480631460fb7e72c8411a9506b2627825995c052b83317b56a33e38f1c8d52a52fec6824f63f15a5612e8e5dca10a",
				MidState = "5b25e87cfc9aedf3c2e158d30a711b8c878d785636f14785651c503f24db65e8",
				RestHeader = "f1634f82e812565a0aa1dce5",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 565,
				BlockHeader = "6a99f8bf6a99f8bfd392b2e64e104b0aaa33dabd0d4bf64193f1ff8e826cd91b451e5b50d28a8a40d3bebc1e4234ebc0f7a8b33b19d3dff46c980a5f9a69e43214c60bc23068e0986a99f8bf",
				MidState = "ebd5df58e9b413cebb6133a3fb02927a8445f0efafb35c1b1969200bda1e94d6",
				RestHeader = "c20bc61498e06830bff8996a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 566,
				BlockHeader = "9b4e2d0f9b4e2d0fd8fe42c1049093c29d4a5bb1d1be7e92bfb46620839d4345922d7c2d83bf1a573d689e0f11f3ab3053a9ee4612d733bd3e20e721fefd6811710247d2c260241d9b4e2d0f",
				MidState = "bf544de0ea73ef98a5b58443957ec090a4ad295e88b5823dd440d3d23aa3acc7",
				RestHeader = "d24702711d2460c20f2d4e9b",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 567,
				BlockHeader = "1297e08c1297e08c4a1e668cef22ec0232508636b5cbe275ef56aadc8d19f7a8599efa04530ddd6ddf5eff36dc0be1d567afdfaab48b95d72b88414e48cf6164db166a8100828a891297e08c",
				MidState = "1ce439f716fcf6e27543a521e3a49fe85d7c310485fc66c10aa387316a7d8054",
				RestHeader = "816a16db898a82008ce09712",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 568,
				BlockHeader = "2ec289d52ec289d53cfd207b1f6e416966bec25be58dbb907135d0b30a4ddb9569cc839b92b929f7f1abdf854bd8316eed05e11d5b5ffa1701b300fa4a65c7b81c4585a29626031d2ec289d5",
				MidState = "5428945d5fb7dc8c6acddc5e85e2a05b677e5c5457e34a32b8de8c5ca2d94e0e",
				RestHeader = "a285451c1d032696d589c22e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 569,
				BlockHeader = "81846ace81846ace8a2d6ea9919e2a2f10420768bf5b00cce8388262a988466e1aa9f63ba1215ed7efd3899de3ea1256b76660484347588db3c5d61578c2172530a030eb5ffd4f9781846ace",
				MidState = "ca89b00bf47963fa8e01126f454633551302d39e26c406474560c07624f589d8",
				RestHeader = "eb30a030974ffd5fce6a8481",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 570,
				BlockHeader = "d1542e36d1542e3632b4a0fb63b40a5492315d63fd206c42fa875a9d354caabb7ac7ed7ecef49529bd532973cd003920637b13a2af092bc7e68377771a16fd4a8b45d8adf5bfcd2dd1542e36",
				MidState = "9e0875f58984a2e174eedad0e23d2827754467d8b16e3f283c2c2565401afc15",
				RestHeader = "add8458b2dcdbff5362e54d1",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 571,
				BlockHeader = "93e5a6dd93e5a6dd4a32f82937d01e19695ee9f7abc1861697d10e657dd74cba5616f53ed19ae2a77826529777eebf39b4ff3627a37d22be3d7ed4b8448551e274b4ddf9087b2c7493e5a6dd",
				MidState = "16297f198b5db41229d142b660fed60ae7f0b25a01b07f55d01d06e4aee20665",
				RestHeader = "f9ddb474742c7b08dda6e593",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 572,
				BlockHeader = "e0f93464e0f93464b3d1026532de8f3575c0c22e8f413967ef6cd5bcf09358f658c4f239f9495d1bc62174c6da3d2ba65f5244270c507932437f123a5b515293c57211affc33125ee0f93464",
				MidState = "4c46bdd64d52292d3a19e0bb4bf4b53f0e4d773c2c321b90fd81f9c6df887933",
				RestHeader = "af1172c55e1233fc6434f9e0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 573,
				BlockHeader = "2e0ec1ec2e0ec1ec1c700ba12dec005282229b6572c1ecb847079b13634f65325b72ef352d5555b4377b9d21dd8c06f7d583c55605566bf472b997250cb08c3a99efd4f6d09f08562e0ec1ec",
				MidState = "ba8ec2e5dc2d3cbaba6cf48f5f7a158219b9f8c1df7a292b666e61eeea3bb20e",
				RestHeader = "f6d4ef9956089fd0ecc10e2e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 574,
				BlockHeader = "fece1c17fece1c17b22c74597d98bcb25e3296acfc5b95211e32825d74b828d3c8e8917d2f594ddd9ebe532cc0f4b54b3172d9fee81842b0a33cf243bbcd67620e1ec9e0e8ba2cc5fece1c17",
				MidState = "db32b97e30ad25f13d39f1f9f2ba2498460cb09e8a292e1eba234ba23138224f",
				RestHeader = "e0c91e0ec52cbae8171ccefe",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 575,
				BlockHeader = "c0cc4625c0cc4625ddad6a992884ab0d12a9f9798bb181266561793d73780ea5aa15c944b28fd69c03c36a45ac5e3fac117b9cc6cc90902ccce189fb46fe11f3bd7de29494ebff21c0cc4625",
				MidState = "eba9dcb7630ff3743a35df4d7d016fc6dc9a4da93349a217c13a025b47fd7871",
				RestHeader = "94e27dbd21ffeb942546ccc0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 576,
				BlockHeader = "0ee1d3ac0ee1d3ac464d73d523921c2a1f0cd2b06f313577bdfc4093e7341ae1acc3c63f0905e47bcf6bfe8f4aa99125f3d3621f0e72b9e1d6841be5bacdc06516746ed3a22619df0ee1d3ac",
				MidState = "b85f8ec4f8e44cbdb920d836d58320241aaba64cecdda8e991bccb5c930775aa",
				RestHeader = "d36e7416df1926a2acd3e10e",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 577,
				BlockHeader = "5cf560345cf56034afec7c111ea08d462b6eabe752b1e8c9159806ea5af0261dae71c23b7fc5044fd0a5591aeeea8e94f092cc7a410d5c282a41dd5e6c0c8c9bb43ca58629042e9a5cf56034",
				MidState = "639d2e043729638f494895e9588527087238a327939277175b2903654ace5e7a",
				RestHeader = "86a53cb49a2e04293460f55c",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 578,
				BlockHeader = "0ac8725e0ac8725e8b3212f703dcc94fab550f5895616837d1e17eb8a2d48b3530bb65c1c3d7a71f7d11e8d688d312dab57d0c9f8071200d646375a908ea9529d698bba430b365310ac8725e",
				MidState = "6c2f30e61a9e410504e36e7c14f8501c5052d26ba218573fd76ab7f8b9c2b15e",
				RestHeader = "a4bb98d63165b3305e72c80a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 579,
				BlockHeader = "1f89f5501f89f55006a8242d641c866838ed4ae8d2d7e78b5733ed07cef35f06a263e839174774f730b5bab12311702e9d003363f5464539ab84eaf679fcb730a30949c76313ce2b1f89f550",
				MidState = "1cdf7577336928efbf05426752107e505509baf5e441070973f2e73b92eb5c87",
				RestHeader = "c74909a32bce136350f5891f",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 580,
				BlockHeader = "6d9e82d76d9e82d76f482e695f2af784444f231fb5569addafceb45e42af6c42a412e53428b3475e71e8a3b8edb67011c061f9c213fe0111542b6b6c06685b8c7f293fbdaad767d66d9e82d7",
				MidState = "69ac42b88b91dd117c992c8137c794df5b31a0e7fd6901b2c318e49983414e7f",
				RestHeader = "bd3f297fd667d7aad7829e6d",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 581,
				BlockHeader = "c032545cc032545cf5a302ef043097c6a906fac27f1c191f697ee7b5142217f91ab07db72b40917d078bd32ab05bd846ca0d524ae9aa8c058b4da947d0000cb0710217c9c5d03b93c032545c",
				MidState = "7d0200db3fe8c1cc2aa9796f0033fae93455408d39beecbcdcda628e14f21607",
				RestHeader = "c9170271933bd0c55c5432c0",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 582,
				BlockHeader = "9a02f0669a02f06669dab875b44895655e6a3d9a5047e55911fb0a38492e1980a4945e474b172f0a6ea281a4b547d47e9ec826e132c18d375d9c79386777b325ebbab4e92928a8889a02f066",
				MidState = "fe17299388514139c588db3f19632e62246c5b3344225a656b17bcf1a79cb697",
				RestHeader = "e9b4baeb88a8282966f0029a",
				Nonce = "40000001"
			},
			new SelfTestData () {
				Index = 583,
				BlockHeader = "c4faef24c4faef24ac61f03ed90f15909f330990448b22ba70dc2da7698fb719a3f2e052961b9db09c26422fc139f453dc2042268ada308f76b1ba07bc7a11887b991d3ec53c8176c4faef24",
				MidState = "e7867fc64c2a5902dfcb1a74cbabbb920de0901520fc4bb06955a268d8bc3ad8",
				RestHeader = "3e1d997b76813cc524effac4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 584,
				BlockHeader = "60230933602309337ea003b5cf2bf7c9b8f7bbfe0b8b895d2012ba554f07d091a74fd949ce04b0fd778fe4eac1b2dbda2dd00b62ae289817dc7e5efbfd4e1508d9af960290784b4f60230933",
				MidState = "325c6ee15c9f8658c8245bda11ffe76be8f495a9c527fcdfacc1615ace3ae34a",
				RestHeader = "0296afd94f4b789033092360",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 585,
				BlockHeader = "0a0ddd750a0ddd75b955b9dd27e2c00fcab87431fa7c5f51982570adba0442b3575564b29c40a1f67f4b6b96388abafde6ee6f54745bdc0a3e0f81c3256b4e13204a3b69ae06f3080a0ddd75",
				MidState = "61486a28c9df4ec2c799c7734549c4fad9966cb24e4c83048863ece4541bdedf",
				RestHeader = "693b4a2008f306ae75dd0d0a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 586,
				BlockHeader = "87dc344187dc3441e5575247722eb392f6b2125123379eb85068a5e3c4b8fec568294afae69c0a562e1b1ddcb3f9d465f40de808da13c4573b204921f33bab12193bb80656a2c22487dc3441",
				MidState = "53178dfc6c90c3481f128d58668fcf54c1a0ac333d7fd9b818aee81666300ef7",
				RestHeader = "06b83b1924c2a2564134dc87",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 587,
				BlockHeader = "4548ea824548ea824f53f73b2181a8dffc8e9b457ff2e7c95f3a07cb7168bf07c13904f0e8dd9e9b0c5af982cf068d8d1773a137d3677434781b45b79796b1eb618da02c75ed7be64548ea82",
				MidState = "1ae7d7973bc2d663ff06231c1dd634af6443e0e58f9f429b46f213bd01ee5544",
				RestHeader = "2ca08d61e67bed7582ea4845",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 588,
				BlockHeader = "dae2dc0edae2dc0e2420d7cf77125a91bc39138bb8c7a9bf8b201d6b82c2aa75c4b3ee09b37df5e5371e4521537117269cfda4d182c12712f6cde3add576025af4e33a3c49bcc5b6dae2dc0e",
				MidState = "f5d6ae5c1b3a168551d6c4ebd5fe826e6b48778e676ff2ce19060ccb637b37ca",
				RestHeader = "3c3ae3f4b6c5bc490edce2da",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 589,
				BlockHeader = "28f7699528f769958dbfe00b7220cbadc99becc29b475d11e3bbe4c1f57eb6b1c661eb0598584c5a8188a59e8d9a0a5be5d977064b2618dda53faee3f70b748125528346389126f728f76995",
				MidState = "60d5ad9447bf988be89520e2b28842ab321eff3f2445841122256250d6f66dbb",
				RestHeader = "46835225f72691389569f728",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 590,
				BlockHeader = "9fc5c96d9fc5c96dec1c79f3f0795e5bc14cc641a5ad6de800021e456a3d3f61edfda10725f27a81081fc94e7c2564fe7a8714f7ac78557c0d19749c6a25e782a2359c438b332e9a9fc5c96d",
				MidState = "6a6b8682a4ab637a62d4261af87aa7e6e9656127a7ede1c0f41e2fabde3e4a60",
				RestHeader = "439c35a29a2e338b6dc9c59f",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 591,
				BlockHeader = "ecda56f4ecda56f455bb822feb87cf78cdaf9f78892d213a589de49cddf94c9cefab9e033c2a85e49c0f29e7f39adbe82643160ba6ddb3020587138b831d02c0b94b354c77efb635ecda56f4",
				MidState = "fb8163282cdb46945f59b7bfeee487636b574920f8a34f0da46803a4b655e4c8",
				RestHeader = "4c354bb935b6ef77f456daec",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 592,
				BlockHeader = "941bc8b1941bc8b132c91669eb13f504c6e0380c6b61b0eac876209dc29cfd868f73579b84fedbf077667007e8041e79f064f3cf6554bc5fecfe88a23e20a8fb1984f84c59ff6b75941bc8b1",
				MidState = "8955b08eb3f7fbb16e61b11f55a875fb1a00cc1dd5ca5ddce64a0fb514ac3db8",
				RestHeader = "4cf88419756bff59b1c81b94",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 593,
				BlockHeader = "2af2483f2af2483f001010ef8643a3355d45717200d9224ef65d79ef23b39012ec05275bce84afa06ab8e030073dcec4329709b45bdb1da02cf63483bdea3da948d9745d01f5bd1f2af2483f",
				MidState = "51c4b4fffc3557e7ff41965a89fc7b6cd60a1cb18aa4bb2c6ad5a466f3bdd45e",
				RestHeader = "5d74d9481fbdf5013f48f22a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 594,
				BlockHeader = "875476d3875476d3bad0b3733dae6fe3fa85c615b39f8e861392d49aba5222b5f5a3fa914923b7ec97e3cf7af92515f61a228fdcb20c84e2ca5b2d979a9e3d5f8424ed4ed32a37bb875476d3",
				MidState = "af6e8c514f26b659678ce7a2329f3731b24f22978032b35fe0bee257794b46ff",
				RestHeader = "4eed2484bb372ad3d3765487",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 595,
				BlockHeader = "d569035ad569035a236fbcaf39bce0ff06e79f4c961f41d76b2d9af12d0e2ff1f751f78d8672af9558f692581dea37e929e904350ce7dc85a3dacacb4c8d5d25dce779059a5c8047d569035a",
				MidState = "f4b074c54c9b426ed8fdadb7bc53a7adf93576017f13d844a15a5b69c7ea9329",
				RestHeader = "0579e7dc47805c9a5a0369d5",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 596,
				BlockHeader = "c9bad658c9bad6586fd523307e586ec1f328ccdc41db94b353f88d69fc0536bde2240a9df696a4a582180218b4ea7dc4849f33c71871a9214a3168b28e1e1bec54041020e1db1f90c9bad658",
				MidState = "e7bd17cfd4c542f29fbec3d26b42f7e9be79c1beae4a4e27e0de3e977e582179",
				RestHeader = "20100454901fdbe158d6bac9",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 597,
				BlockHeader = "17ce64e017ce64e0d8752d6c7965dfde008ba513255a4804ab9354c06fc143f9e4d30798dd176c676ec83d4ba0326ef612cba6cd3d82e2fae9b05a5f53f04c4e3ac3e4320509b00b17ce64e0",
				MidState = "694fe972f022c5c817a243dfb6ee198803883cde8134da0a07a779e5e0353778",
				RestHeader = "32e4c33a0bb00905e064ce17",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 598,
				BlockHeader = "65e3f16765e3f167411436a8747350fa0ced7e4a08dafb55032e1b17e37d4f35e68104949434f70cf5c6867b7a114fe3a8c7509502ce7f17259e23e13959593924f3f8234409930465e3f167",
				MidState = "86ac9917a99b839931adda77a304748484c375b145d6e63475b62fb05a5441f6",
				RestHeader = "23f8f3240493094467f1e365",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 599,
				BlockHeader = "bb17aac1bb17aac1ce10b74edd2d4b7d3e9d18bdf35d80d98229f579f82d4e215cfa74755022ed23b7654d4344ea6a0c2a1a9ed40eb1b1d5c1374e3ac263f0616f891673f53bb5c2bb17aac1",
				MidState = "bc7e0a366d1bda5e6310025a86e952758627c734a4171f3f134d590e020c2338",
				RestHeader = "7316896fc2b53bf5c1aa17bb",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 600,
				BlockHeader = "f4975fd2f4975fd2d5fd61acc507f9a0955c3dc84e268c8834388626191f9e1cb7265e4ac75beced2ce4637e13aac6612ba1aa8e7b9df2f5e323fbddd3928aa18a8ab60a1bb0e1eaf4975fd2",
				MidState = "13d95129ac396362209a11cd2adf40ce7f376ef85ea9124a1bee2566291998e2",
				RestHeader = "0ab68a8aeae1b01bd25f97f4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 601,
				BlockHeader = "f1f4563bf1f4563bbf9fc50ef8aee44325a09f7b6eed1f464a048283df414de5a8c49263c64102e9c988b550778016e983dc610a0e926aca6c992ec5633081a9178ac06468437864f1f4563b",
				MidState = "f495acc20a0e0a1a0a2aa91f40a529a9b0b908690739845a04f3ed57e6d64483",
				RestHeader = "64c08a17647843683b56f4f1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 602,
				BlockHeader = "8d1d704a8d1d704a91ded786eec9c67c3e6451e936ec86e9fa3a0f30c6b9665dac218c5a27ed79363515d82ca91599771febbd500e590487c2a3dfdedb97522aaf5a3e47ea3d771c8d1d704a",
				MidState = "f60e0a653303063dd27a81864c3135ce313d057de3da98fa0cce6db35f7bdaa8",
				RestHeader = "473e5aaf1c773dea4a701d8d",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 603,
				BlockHeader = "db31fdd2db31fdd2fa7de1c1e9d737984ac62a20196c393a52d5d68739757299aecf8956bdbfb03ccb8177b3e05143ea673760f4700051f9b450d6458f9659fd528667628f01844edb31fdd2",
				MidState = "b10723218aafef8a9d42768d043ae264f80f1339e921eb2a9e08c4024fb9b948",
				RestHeader = "626786524e84018fd2fd31db",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 604,
				BlockHeader = "1b7307501b73075005ef869bb1c6442175bf092be5ec9480e9da8fee29c547a95ebf7c642627333871ac24bc514e3454f6d3bf899888c1433d73f5db45896aafaeac1a3f84c2c67a1b730750",
				MidState = "1e0e6cf95f90f3a9bfc393c915722d99d0fe7ceb1e5da0e6b860d2a7be343199",
				RestHeader = "3f1aacae7ac6c2845007731b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 605,
				BlockHeader = "9672efe89672efe861b72b75260eace96156e55b0990bb985860d70195506b3f53dad5218d297638a962a5dd0ec270944bce5b97f1de2631bbcb08538f21fb454f5daf0c9727cbef9672efe8",
				MidState = "70b3f58a9ebac3a69d7cbb75e06ac6251b6be3a0c8a17879438121d174cf6f5e",
				RestHeader = "0caf5d4fefcb2797e8ef7296",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 606,
				BlockHeader = "2ff100e02ff100e022e13d87f32bb03c4e8988731e035adb36098382d744a61a1ab31ea938a2ff359070cdb989a74591c9d52a732efadf1d651c33caa21fb7c92dd8537f5bae046c2ff100e0",
				MidState = "fa2cc27fd44d05681d091a8cd732d88072ba4df5476ddc7319e0d7bb8fa0b16b",
				RestHeader = "7f53d82d6c04ae5be000f12f",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 607,
				BlockHeader = "c1afea13c1afea13922e62cf6de1aa5c4be9cf8e953bdf9130f1a65fdbe0f61fd96e6385e9be312698ec6bf27e47f14e3fc241323c5f8fdac05c62386481586f9bb5b72a86066d7cc1afea13",
				MidState = "bb8792ba77b6568d9a339c842e57414761964921203afd8d2b23ec807f2f368e",
				RestHeader = "2ab7b59b7c6d068613eaafc1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 608,
				BlockHeader = "6f35515b6f35515b22e6613880e4b59060924020a3fd3258008233907a5f0c985af9a95f56b4584ede4a2cacf598aa33f0900609d7302c8f10fb4995b2ed3874e853fd402519a1426f35515b",
				MidState = "e641466953c2104b568f4c6470f345b5be0806d670a47e796d43f0261bc8500c",
				RestHeader = "40fd53e842a119255b51356f",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 609,
				BlockHeader = "7d515f847d515f84bde36c997c9890dc097752ae326e500c87e8cf85b08f95619eef1b71300e23517a0e83d263f668570e60b3b13b7c980972b570a5a15d30429600b277620c04b87d515f84",
				MidState = "461d109507eb93e5bce95d5c87309ac08a673a70b52aa0fa38c074cfc5cbe3fc",
				RestHeader = "77b20096b8040c62845f517d",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 610,
				BlockHeader = "5872f8f25872f8f25dc57deb710e08e485b8cbc54e7c4d4c08548694d493314c6104a052b9d6beb65cde69d166786e2266a30f2cf70d1142f104d74c67ae1e8744b6587ed0f167de5872f8f2",
				MidState = "bd1878c82adcef568f846139871778a2ab2b6bf7e75d39f01ee7ed7e84e7c624",
				RestHeader = "7e58b644de67f1d0f2f87258",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 611,
				BlockHeader = "71e7381471e73814c21d416a17e0ee8d3b46254bf93742c3046a337ad59df12b22b8259e87b00cc44423790dcd50986dce7116f6d66efe13912ec703e4db5101e4fa4a6b12b4bfe871e73814",
				MidState = "20cbaadc943b2bceceab11a093f8653a629c751234e6781589b41f661c4f8a0e",
				RestHeader = "6b4afae4e8bfb4121438e771",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 612,
				BlockHeader = "4c08d0824c08d08261fe52bc0c566695b7889e6215453f0485d6eb89f8a18d16e5ccaa7e9cc781f0bd1456ba2cdd3080803a2a6001c8e020d70a4f977930be638e98760558722f114c08d082",
				MidState = "a1b58e9aaf2ea3a1b9d2379ee7433806ce9ce4ea7396e95454b437ded788b895",
				RestHeader = "0576988e112f725882d0084c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 613,
				BlockHeader = "b5deaabab5deaaba5915dcb469496bd5e41ee6f54abe81ac213766d24016e94921211cf61076aa56e7db6f5a4a727d2e3d83cc68f94c008c2eaec997b13b8fde5ea93d1597e281b4b5deaaba",
				MidState = "0d667d1779f4ce4fc2e74bba33899b2086f89ea1cc3ded9648bfa7d61018f99f",
				RestHeader = "153da95eb481e297baaadeb5",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 614,
				BlockHeader = "ea55d87fea55d87f981446587aa577b4525c18a5162010b250a78d6a00477fcfe61b6ca8db0056665c511731807e0d7f54a723f4f93a163439d3024fc481522849bf5c213c92bc9eea55d87f",
				MidState = "0427f9a361222f2bc8355db87795df8c714d5d5a2a44cc40479301778e1bf303",
				RestHeader = "215cbf499ebc923c7fd855ea",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 615,
				BlockHeader = "376965073769650701b3509375b3e8d05fbff1dcfaa0c403a84254c173038c0be8ca68a4538c2acef53f200a72153362f53b202227d9d7135180d5ae9be9bf7f4aa88659621612a837696507",
				MidState = "656d8bb3a31eadf8a2965fec11ccaccf0f795e58d9b6d714d8750eb904db690c",
				RestHeader = "5986a84aa812166207656937",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 616,
				BlockHeader = "e60205a4e60205a422be1a7588b20eb73d3da89ab23278f406dcbc3198a3e0f0269e42c17cbe11ff465a020cc987f8123e8e5678b3df7ccad10287d3d5328d3136b8a41222c176a1e60205a4",
				MidState = "0366e8f1f0b585f03b4ce6a7bcbeb0d75e030e8f14628a7d29ba7301ca504489",
				RestHeader = "12a4b836a176c122a40502e6",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 617,
				BlockHeader = "d86db30fd86db30f7425ceb630c09d0913e8c578b01db5b09d897b06786acd8c874a73f50be6e3eb22539f51b7386daac9e549e1d4af81dabf8f9c7ab473ee822e98042d3a540ea8d86db30f",
				MidState = "5b144def41ff109c67cdf39b1639469fbaeff4a1a1f77c527aa0314f3742560a",
				RestHeader = "2d04982ea80e543a0fb36dd8",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 618,
				BlockHeader = "e177949ce177949cb57a7130da6a752aae9c99da9845c9ee4a4e33c5fb8ca33ecd46e554210b5a2cf1dd81f2a488b83185cd022c038d3172d99b514ad7efa32f9ad587775b1ecc86e177949c",
				MidState = "a698fb80e5d6c0e00cbc4801f014da22d35f0a285757a06deb45a3314c49b0ce",
				RestHeader = "7787d59a86cc1e5b9c9477e1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 619,
				BlockHeader = "2e8b21242e8b21241e197a6cd578e646bafe72117cc47c40a2e9fa1b6e48af7acff4e24f2dd7dd8455e925f1f3216a94977eff5cccbdfaf578deafd6671b446669dde6390bfa91352e8b2124",
				MidState = "bcfaed821f9e98879fdb55f3a40d9e78734d0b82d7918a5115d323de1acbe6c0",
				RestHeader = "39e6dd693591fa0b24218b2e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 620,
				BlockHeader = "5ceeb1cf5ceeb1cf67d5706166534fca39a413d0e61a8b6160694483f43032007fbf16858d196770a79bede8d0a89657230653e1648eeb905067d713b2319e4f1870d36d89c1171c5ceeb1cf",
				MidState = "eb6cd784cfd663052bee627b38764ae965d9500032cc723a378f8654c2c94a91",
				RestHeader = "6dd370181c17c189cfb1ee5c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 621,
				BlockHeader = "f8acff05f8acff05854ee022687d01810784b4ae753e2b91a565d65444628608a52e262b32f7d8cd77f6a794c6da75a194e39900c94cbf04c7f721e3979ca7ded1162e1c94539faef8acff05",
				MidState = "6de358c61d381490060e48d7deb6c2ffa2829e61632f1d45d2788977739037de",
				RestHeader = "1c2e16d1ae9f539405ffacf8",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 622,
				BlockHeader = "9cbc3e739cbc3e73056b89c0fb8f1a6cf0c9d98c4202dac4ab1252e3dabee8b31c7f1f693de0f4445687e29609a44f4fb4c01c3a78dfa54c06159c5f327b5042dd53586ff65bb8919cbc3e73",
				MidState = "a717abd99165b6c8d4537464387e20db5e7298f8163870e67a6092ae83744179",
				RestHeader = "6f5853dd91b85bf6733ebc9c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 623,
				BlockHeader = "38e5588238e55882d7aa9b38f1abfca5098e8bfa0a0140675b48df90c036002b20db196004b33cea53ded7ccb8105b158e3908299b22ce1898b7f1b2bb029a8296cf7056d6d29ce938e55882",
				MidState = "b5cab9600acd60e7732ef065d453d9eb6d67949881c12aae70e193f6fad723ff",
				RestHeader = "5670cf96e99cd2d68258e538",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 624,
				BlockHeader = "86f9e50986f9e5094049a474edb86dc115f06431ed81f4b8b3e3a6e734f20d672289165ce33a8b10d15be1f3633ab6d1d925d1502370f56582abf0a759d4180e0d52f171627dd91b86f9e509",
				MidState = "e3c9e9945e729442efe2b8c6b41ac2e106b049076e9ed92683cce0bb49b14fab",
				RestHeader = "71f1520d1bd97d6209e5f986",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 625,
				BlockHeader = "8afa1c2e8afa1c2e69869c4a8429d9f96399bff377dcc8a881b7ff03d7e9545d210a3275b1b2dc65ad446a1a660d357aafb6e24ac6e268646233f169e8dcaff84cbb8a4db8f922be8afa1c2e",
				MidState = "f4358d9bec4b10a341c8e15c2d07af01acc5a1f5d9b6718842e4c5a4927b9500",
				RestHeader = "4d8abb4cbe22f9b82e1cfa8a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 626,
				BlockHeader = "45888838458888380f1b2b8b133e91f39edcb5fa8217a7d4de501d0202d0a8eb778c98854bc7bc844cd551620f718280c85e96b1c69dd2817c7469d47187787ef65e9b46a9fc3b4b45888838",
				MidState = "e31deb6997765c3459f286ff29b4134851517dbd43c95e5e9d0e36405a3eea5e",
				RestHeader = "469b5ef64b3bfca938888845",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 627,
				BlockHeader = "939d15c0939d15c078bb34c70e4c020faa3e8e3166975a2536ebe359758cb427793a958099a686c003d0084061678cc1a0fc2aba3e033b85ba5fa4c839ff8118964056324be484ad939d15c0",
				MidState = "f956ea9ec738901baa3a72cb652d269974d5c65c3c6fb701ba1c8af410638eed",
				RestHeader = "32564096ad84e44bc0159d93",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 628,
				BlockHeader = "e1b1a347e1b1a347e15a3e03095a732cb7a167684a160d778e86aab0e948c1627ce8927c37989bf386e390eccd7eb4a22861e5ba0fe69e6e2f73db59d11dd1dbe83f24271aae9312e1b1a347",
				MidState = "d9acb5b30d37b355462838a76d156a9d952217ffd5326e61352e7fed9da04096",
				RestHeader = "27243fe81293ae1a47a3b1e1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 629,
				BlockHeader = "aacb77b9aacb77b943e8966f3073b72ded3b91feb1a2f8757b353d946a115b694a25b41daa3caa95ef272ad896e4d55327a1286eeceb20a19951d7e86aaeeac1c68fd60bb7afe6fcaacb77b9",
				MidState = "4467e14323942b4767494be718ea97b4b6711f0a700e079f0d1c2f73946bd963",
				RestHeader = "0bd68fc6fce6afb7b977cbaa",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 630,
				BlockHeader = "33afa3c433afa3c48d6a9a5d92b1b84357771bd1c5c48bb1713ca717520572706fa9d0fa0d4dd26126e034e5820f428eb077a20bcc30a035844612236a60870824ffbd27820aa88033afa3c4",
				MidState = "addec2c95d935d6686e1c8bc992d74e568ffa73637dad650b1999ff008b36518",
				RestHeader = "27bdff2480a80a82c4a3af33",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 631,
				BlockHeader = "81c3304c81c3304cf609a3998ebe295f63d9f407a9443f03c9d76e6dc5c17fac7157cdf61c8ed6c6090f8cdbc251572dc5fe066155c26b4d4d4cf2f961d45bcea1ca880b8383864081c3304c",
				MidState = "a67a9c762a8095d4d5fbba1f6566acff7a023a3c21781ef8c088af21eb441372",
				RestHeader = "0b88caa1408683834c30c381",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 632,
				BlockHeader = "cc1e99e4cc1e99e473758d3676cbc105bb72fa29a78897f9534fa4eebb26116a851d3eed2250f6b60283e0e8226ec880ccd7aa2809a39bae0fc5384bd3ba2711f0feec2aac612c74cc1e99e4",
				MidState = "fa5463c4c75f974433bb88cccedc21b86ee65ce3ffadce409f7e19e78a801eaa",
				RestHeader = "2aecfef0742c61ace4991ecc",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 633,
				BlockHeader = "bc56a27bbc56a27bbd03aa199a2a3a7fe861057aef94dcb9c138eb56148823d5b3499cb50a269306f7352b01bf25cbbf145ebc6021d12c989805e77f892081e0c1d499609a9c6673bc56a27b",
				MidState = "109b60108cd9e8af92c3c344b088bb29adef1d2eb80a6ce6e0706f1d87778a37",
				RestHeader = "6099d4c173669c9a7ba256bc",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 634,
				BlockHeader = "587fbd8a587fbd8a8f42bc9091461cb70026b7e8b793435c716f7804fa003c4db7a596ac223ea08c0b1459dd2dbbfa7b575679a3af0f4510cef4c3335a021dffaaad5d662501443a587fbd8a",
				MidState = "667630963f0260bb39dd87b35dbb892aa6414f4046213fe12d45f7d68a9aa914",
				RestHeader = "665dadaa3a4401258abd7f58",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 635,
				BlockHeader = "a4649512a4649512f394ccfd90ee43eac104a0fc8ad897d63084536e029f76a24611735c0521f0dc1e4d258312bdc4143d02683db15a39d662dc4d6ee92a6a7c46f7147b6fcc96e4a4649512",
				MidState = "afea489d0be0398ead07cc8892a907c2173ee5b7e1e94739c5fc92c9ae5c7d82",
				RestHeader = "7b14f746e496cc6f129564a4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 636,
				BlockHeader = "f2782299f27822995c34d5398cfcb406cd6779336e584b27882019c5755b83de48c06f573b58fe3c0e0774b00d847c2b5bb924db4bda0663f69d1190b80f729018b1570407dab36ff2782299",
				MidState = "981122bbcf57b8e04f28e1831cbbf41a32dc8ce0c2b7530be83a79808bc72d18",
				RestHeader = "0457b1186fb3da07992278f2",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 637,
				BlockHeader = "57e6a8a557e6a8a5570feafd31353ecc33f732ecdb9916b01939f2ee1c40705976f88efc310703dc0d8983c8df31bacce7db62776fb7d4714490f928b82445f50344636a5a12a20f57e6a8a5",
				MidState = "4b81d5261d5282ce27a3730948508e6918256ef791cfc8cd8237a17231d2887c",
				RestHeader = "6a6344030fa2125aa5a8e657",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 638,
				BlockHeader = "eef03198eef031989820deb401d64393a60c174267a8cd14555b7fe8a96a01ffd1acb62093a05808445d698b7ce79feb126fd41ed9a556d86568ec07830ad05d7a5c671b7341e7caeef03198",
				MidState = "e00a47e63741f8673578941bd46949519477676880ad3e95ead75e6304d2d8ef",
				RestHeader = "1b675c7acae741739831f0ee",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 639,
				BlockHeader = "219a7197219a7197f43369072eaefa77791282765dc01903bcee3164eb5d3e8602a9005239d1d09000958a64af017815b1833640c7c3505c4d74a37bd2eb1e21eaf964169dc266c6219a7197",
				MidState = "8370999a5bdaffb0c7bf4ce8e1a8166461af6b95cd33317ac930336951f09569",
				RestHeader = "1664f9eac666c29d97719a21",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 640,
				BlockHeader = "a5db686ba5db686beca420f8da7ccbc299797ef966c9f334b4aef170894376b8c066e52b4e649d19fc252a2e6be21872b1d6d026b2fa60ff698ef8686bab8a9c07bbea3d32e6bc31a5db686b",
				MidState = "9a5c622ee0f07b7c36873e3eeb0ef022958cfea041e5b2be2225349616f1b257",
				RestHeader = "3deabb0731bce6326b68dba5",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 641,
				BlockHeader = "8e180f018e180f0127823cabcba61e16bea0099e10480e28bc7f4574e3779b6bc671db1ec3b32e1d88d1f07209e6339d604f97a9998e2243286fe46cfc1362adde8f6620b3f7bab68e180f01",
				MidState = "a65157af25056aaf9b41ee58f51e5a6696a56ce389078b5f35a8de41fbf3f5f0",
				RestHeader = "20668fdeb6baf7b3010f188e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 642,
				BlockHeader = "aab069b2aab069b22c6494add2c24e13cf571cfc22bfb974e844ae6217e0c42bdc7c94bbfe1695cc7f39a1825350f70993fed6a84b33e80aae1b0d27e8e7fafd6f35451c1c654a82aab069b2",
				MidState = "82479d59b6f4e3e5c364988e418cdf0020ab8fc001def0336f2d9aea2b4381e9",
				RestHeader = "1c45356f824a651cb269b0aa",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 643,
				BlockHeader = "f750d38ff750d38f92254c097b37e3e9fe0d001b50182e3685db2d1b973444cd91542684e5a2431714e1c7ff56a20a9243b27e20bdf9c4a922038f9ce291196ceede1d30ba8b4fb3f750d38f",
				MidState = "c25ff6039509441408a34b0b9d344ac947758e668453bc91a44705adb135f22e",
				RestHeader = "301ddeeeb34f8bba8fd350f7",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 644,
				BlockHeader = "8a55d1f78a55d1f73d7c48887a5c70cc944ccee36b702b817e74193ec070614a62a7fa767c395cd81190df1d6a8c1290c0c0d57b206c17d4abaab1e35000f6d245bc394ace7f6d5a8a55d1f7",
				MidState = "11479afb9616a275b6927787471a93771a5ca117c30f5aa53049be769ecb979f",
				RestHeader = "4a39bc455a6d7fcef7d1558a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 645,
				BlockHeader = "d8695e7fd8695e7fa61c52c4756ae1e8a1aea71a4fefded3d60fe095342c6d866455f77116c74c2938cd86f2b17b537f0c6eb647ff2e230f9cb102875e72a0641a666d73999bc3b1d8695e7f",
				MidState = "06c6bb1a07af156a296240ad40efb4068df65023ed46c98c672e7af6886a6cfa",
				RestHeader = "736d661ab1c39b997f5e69d8",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 646,
				BlockHeader = "08ec2e2108ec2e21b009e5b5388f32244d7a5005dc5feb7ea12704d63505924e10a4f165664537e45b38b02aec198c9cf2e46d1c789a19eef433f8395c5b620506873f61bc1a784808ec2e21",
				MidState = "e68fc87a1fec6bb8c61ba3eacacd8121ddaa5636c150dda387607b24a0c480cb",
				RestHeader = "613f870648781abc212eec08",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 647,
				BlockHeader = "43a4097543a40975ad1f3db096982be1f11f81111bd4c262fdd986f6fc47fbf6260c36b068f7509370c4d28a08dfd774fdbdbcc9b3d2d02ea163e2cbbd847cc4f1ac0b454b3bd9d243a40975",
				MidState = "bf6b206da6e70d829e1388cfeb7e55252a9a3fbd71bf775b993646b9d9245ae2",
				RestHeader = "450bacf1d2d93b4b7509a443",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 648,
				BlockHeader = "ce0b5fcdce0b5fcd2a5e65007ebed6419b59c3fc5e67328f2c7337b2cc73016dd7a2ef3af0aa763480effde60f9767b62c2cc32b21559527f199df90f7e0e7223dd0637f1a209569ce0b5fcd",
				MidState = "46db0c368641a8857415e14a161a2b1f70bef3c9c2ec401d935c916df471ae91",
				RestHeader = "7f63d03d6995201acd5f0bce",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 649,
				BlockHeader = "c2613171c2613171345950189042527bc7d344c4f73a28c6dfd4923513a132f421b339b0afe56ec8619fdd33ac4ce0c408cebb21cb99fa7f094bf486f34ffe2eedbf96101d557825c2613171",
				MidState = "b4e213c536afdd0815764947cb8947adb7b836ad3b6e0ce1b6f931a055df346b",
				RestHeader = "1096bfed2578551d713161c2",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 650,
				BlockHeader = "9b3e98ad9b3e98addfb342bb45f6df27266c26cac95a3b60b77631f2d05deb43f36d5b55f71c1b58b1ba640aac64885b8ca1a5ce54355fb5220af8c85baca845e5d9f72377c405709b3e98ad",
				MidState = "acc7a9bbbb38827c5895de3758e59d333b04e14a9d4270387d4eb097ad1a711e",
				RestHeader = "23f7d9e57005c477ad983e9b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 651,
				BlockHeader = "3767b3bc3767b3bcb1f254333c12c15f3e31d838915aa10367acbe9fb6d503bbf8c9554d9c7ede9393a1a452aa572b89f9151cb654613516efbe07d59046ad43a8e46135e4419ea23767b3bc",
				MidState = "f343a4325aff1f3d28d59a1e96ab2f9d6ec9ca676eb80b3901d909fa24538d43",
				RestHeader = "3561e4a8a29e41e4bcb36737",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 652,
				BlockHeader = "b1fbc987b1fbc987fb1aaf0550dae74f8ed17e2713ebe193891de918dbbf4bb263cfc6082c2313e9b83559d501ac256a2c839a8d2dd22ac069dc4d713b11b34baaed1d2bebd1e2e6b1fbc987",
				MidState = "a143a55174f5f34c147e66c6e5fa163fe628dc67c5d7e885fcfe0a102354d487",
				RestHeader = "2b1dedaae6e2d1eb87c9fbb1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 653,
				BlockHeader = "9e9cd4769e9cd47630a9a35d6f73ea2f1dc6753ae0fed9820df954bcba3cd6d4b7b85fadd7807536099023f77e41396380185e3a0b47a87410feab073c980651097b4a6ff05097f59e9cd476",
				MidState = "a2ab59f0603430b6449408e1be57af5d842d7bdd8579be9e63c014e2233c0348",
				RestHeader = "6f4a7b09f59750f076d49c9e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 654,
				BlockHeader = "0e4f52260e4f5226648da5c6bfcf629ec62931c0bfb81ca484b96298a7dabbf8a565fd489b6da2894b4e242ef5b98dfa069c3c7f1d5b2831dd3f3ff4cc309567ded4d35cf28833a60e4f5226",
				MidState = "59b4ef709197a97709ccb113378952819c35e07c9dc0803fbe6c96080a95dce6",
				RestHeader = "5cd3d4dea63388f226524f0e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 655,
				BlockHeader = "600b39c6600b39c6c4a70feefa5b30ab9fe7eaa598664195b2bdd58098066330efe80ecdb43948061d02712e1d60c4a7c153c62a3becb4ccde8fa9507124070c9fb4027069aeace2600b39c6",
				MidState = "53665ba2c8311ec6b0fdae338a99fd10a458675d6addb9a891c450507ca0bca8",
				RestHeader = "7002b49fe2acae69c6390b60",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 656,
				BlockHeader = "ae20c74eae20c74e2d47182af568a1c7ab49c3dc7be6f5e60a599bd70cc26f6cf1960bc9cb269b361a7119eff1dae7fff2f3f1b9133b8cc13b870cd4ae8c81147edae94725d1a5c4ae20c74e",
				MidState = "d490faac72e16dc3fa39b8db022d6dea1d29e069b0f3d1fe49fb45f32aed90b2",
				RestHeader = "47e9da7ec4a5d1254ec720ae",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 657,
				BlockHeader = "6e2848ef6e2848ef5fa41950f60e0cf848ccfc3327d75f4939247175cf36edf933de80dfe90f70ad30b951dad6063d13084601535d6870d72f6b28d43ff3adb8301ed3557cd0d69a6e2848ef",
				MidState = "8fe87a40803dcbd29fa0b33c30cbac3a578fcb337429acf4277e4d5bedebcefb",
				RestHeader = "55d31e309ad6d07cef48286e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 658,
				BlockHeader = "567f919a567f919a2f266ed2b5a32be5c29d8023f31ebe9f285cf3f858f7df88960f9546edb05ef99da6550c553ed9e3eecd2e4e46aee9cf7e106959c2e629ea98f55817b4cb9c25567f919a",
				MidState = "21a6450582ae1446d4f53e81d7d4bb569cd780cec4869147cb1731d824a9e1ad",
				RestHeader = "1758f598259ccbb49a917f56",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 659,
				BlockHeader = "d46434abd46434ab3e30dc2dc5f1d0af79300d1e3cb3bbfb5a3a847a827eb899d5b27c140802d0233001c698a28fe274973b8ea1922a376b7c2b2e4a9b54939eb3d89b2289d2545cd46434ab",
				MidState = "66236d6b421905ca0ae106156b46dabf2a44679cdee2dd91534d0e46105f7f56",
				RestHeader = "229bd8b35c54d289ab3464d4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 660,
				BlockHeader = "b194b19fb194b19fbec85419b1fa4918f537c1f000a590f7fde145961be8e21226ba764c3770ccdd571e227182d7c9b823b9b68f3a96eb693f9675bba7dd047574f6c324689f9757b194b19f",
				MidState = "bbd67e5e61077111f94e10de82691d423563735c8b5e41b21b5151ce91d949d1",
				RestHeader = "24c3f67457979f689fb194b1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 661,
				BlockHeader = "0442bf3d0442bf3d2ab69c1ea34c668f42beaad88eea6552c16961b92975b4138966c175107001af4da056c85e4f8000c8a13fcb103364ecd07687a388f1c5cf86393b38ca1e0c710442bf3d",
				MidState = "383bc3315f8eb8c1120f7ed2e777e97ddf2a47fff62895475752b2ba2d250780",
				RestHeader = "383b3986710c1eca3dbf4204",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 662,
				BlockHeader = "79b8dda479b8dda42732b86a2ecc08ed14dcc11083ccf753d22cb739616515d35411d63c6c67f6d96ddb5dba3de8e1ff9827d70707f50fc4b8eb809e4860214c5640e97df714b94479b8dda4",
				MidState = "537088c95904b68fdaaef645c1fb9d713e582c84e603db8ea594729a18350354",
				RestHeader = "7de9405644b914f7a4ddb879",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 663,
				BlockHeader = "41ace64441ace64475139a9b196ee8ac2df1a91581e23bebff9329763f68aae0d6a4eccdd8d28b846fedd38c03e6be517569481f90cf54e0b0a59190ef1938aec965f041e2f375fe41ace644",
				MidState = "1255921e7f0593d22cdfd83163d1f840e7ccc97e2db1db144452d6d7025cb4f1",
				RestHeader = "41f065c9fe75f3e244e6ac41",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 664,
				BlockHeader = "a147afb8a147afb8554219f2f6e9112cd742d45ffeae3b5a0df311104153743d82381e400d19f12b9b730ba15c979658ef5fa189d07633b6c7839a7b831e691b6b89e41cc9b34b5da147afb8",
				MidState = "8828d19565d7c32326a9e547720f8286d978f9abc6082a64feb834d1c7eba37e",
				RestHeader = "1ce4896b5d4bb3c9b8af47a1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 665,
				BlockHeader = "1824e6db1824e6db563e130a0b6ff00eb44b579e4476ed7ada928850adb397a8639b9776097ca9164539e2519396848a4079448e7db87ff927a04f1f33bf8b42d7a20e1bdc091a431824e6db",
				MidState = "111ba6599d3544ae54152bf06795f4a1d7ec43485300b2bc93c64673b84b5ce4",
				RestHeader = "1b0ea2d7431a09dcdbe62418",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 666,
				BlockHeader = "13d8a93213d8a9328172ce67014b20f42385169a03100a126900f426a5538d4c7396d8696d47c2ff122831b6012ab42fe45649e33e642ef7a85be46a711d23e492293a500792012713d8a932",
				MidState = "1cfa867543ebbba2c96afb591ac2f41736288aaeff3cecac33f866089637102d",
				RestHeader = "503a29922701920732a9d813",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 667,
				BlockHeader = "fc1650c9fc1650c9bc50e91bf275734948aca13eae8f240771d2482aff87b20079a0ce5ba4f43e9c364ec46b5d81c9bd920a64c286c7756d67ef5b9c8fdaa383d652e053acd36f8cfc1650c9",
				MidState = "f3effa8f666246036a9c3785ee9a96c1bf5d2d495aa849dc0344dc496c2fdd2a",
				RestHeader = "53e052d68c6fd3acc95016fc",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 668,
				BlockHeader = "502b4115502b411589e09b8d6e096b0b7bd1c63a494af712ab540f71276a94d1d235538f4c9d7f8f2da4329756a7cf18069b838292c28a6c4e2ee57c4962f94513300b26985febe7502b4115",
				MidState = "2af5b4e56f0245bc4208a9052a446de9e9f39737313fcc0f8eaf4685c0d36abc",
				RestHeader = "260b3013e7eb5f9815412b50",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 669,
				BlockHeader = "5b664f8c5b664f8cd048079d583d740d90a51a7413ffb63bb9be79dda3ccc08e6b11be499ae06b4915fa13fba6cc43c77f7707091d1c0591fc169697bff6f37cadcc481376276aee5b664f8c",
				MidState = "eb016fd28c616c4d23a28ccddc825ea377d173536e93adfa66fcbf14f6f67a41",
				RestHeader = "1348ccadee6a27768c4f665b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 670,
				BlockHeader = "3687e8fa3687e8fa702918ef4db4eb150ce6938b300db37b392930ecc6d05b7a2e26432a1908d6910d45fa3e56a1a79e2863c51f3ee2e34d45a600e3b39ae081e8cd6f3a55e3c40c3687e8fa",
				MidState = "42526343cc181bac33a6d36da337498122e303f028e2ab7d8ca7c15577f8e261",
				RestHeader = "3a6fcde80cc4e355fae88736",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 671,
				BlockHeader = "f68f699bf68f699ba28719144e595645a869cce2dbfe1ddd69f4068a8944d806706eb840dd81ea43ab590c70d568c1559bec4e7a3a3d90e59cabe50085db98ed87d8cd4dc436cda6f68f699b",
				MidState = "d62689301e371f9914594580f8c927886333429ce9aa2a69c45398d669c548f8",
				RestHeader = "4dcdd887a6cd36c49b698ff6",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 672,
				BlockHeader = "4d8b8e4d4d8b8e4d251e64cdaa197002d426335d6cfd123f2eecdc01fc47f377f329a496a5966d87dfcada634513c90afda29c4869c0239c8f9ef2c4d38ea9c4694e2351c561d2ab4d8b8e4d",
				MidState = "974521d77c7f5657971ec23a8c14e4fe51e5d9e97678f59ee2641d8f73efd6f3",
				RestHeader = "51234e69abd261c54d8e8b4d",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 673,
				BlockHeader = "056e7b5d056e7b5d2fce80326fa3a0e5c088f75998f5c637dffa4b55755cefe321c81b971e4605b539978128922f8156698636f0a1accbc63b7a04e071acfa54616e7f4be1e15f3b056e7b5d",
				MidState = "00f235729ead90029b3ff1fb7b677b23c03a8d59a28a0f5a871f0bbcfd6adbdc",
				RestHeader = "4b7f6e613b5fe1e15d7b6e05",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 674,
				BlockHeader = "16616cfe16616cfe8be134f0be23089da832e8b400ef6898c03cd9f384e858efc77055608fbf966d5f0b34adcd12ec127dddf66acb8d8aafc7bdf49bc875425283632e6cb2844ef516616cfe",
				MidState = "265c763cc50290cca4d701917ede01d67b33387869fbc5d873fded3b38500e8c",
				RestHeader = "6c2e6383f54e84b2fe6c6116",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 675,
				BlockHeader = "6476f9856476f985f4803d2cb93179b9b595c1ebe46f1be918d7a04af8a4642bc91e525b327e159693c2b8f7feaf407bc76dba49ca6c2ef274611fb03e144eff0e7789109fdc8dee6476f985",
				MidState = "67064ebbee434f2291d4a169a377ac154bf8fd1c8ab21c131db0673629c5045b",
				RestHeader = "1089770eee8ddc9f85f97664",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 676,
				BlockHeader = "9b234d8d9b234d8d28a7546c41bcd0326fb9019c957d7624f1ebb8555f25135f4954fc84c8383efebc92e5661863249c0116b6adedf577206c5178da7765b89a55ceb9636e77c7849b234d8d",
				MidState = "93f335a8b334bd82a5eb23501f91623cab5c8ab5021bdb108a35a87d46c40150",
				RestHeader = "63b9ce5584c7776e8d4d239b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 677,
				BlockHeader = "83bec20583bec20597d2a6981223fdbc736529bf79cd8b8e81654463f928ee57c749ad27fba476c7e4dc30f5ed4487d1667c31a9aacc6f2df1c1a460a08fa0b535f68065104e8e2d83bec205",
				MidState = "203ea8131ec795683d395461eba16369179d5246f87dbe687a8706f50bd1a674",
				RestHeader = "6580f6352d8e4e1005c2be83",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 678,
				BlockHeader = "1fe7dc141fe7dc146911b810093fdff58b2adb2d40cdf231319cd110e0a007cfcba6a61ebcd9d08d24841a150cfcaa7c6f5a32c3236892d2c43276f07992eadbc5158f72d681056b1fe7dc14",
				MidState = "ee6a7e7fe7e4393e0ad12537fc4c3a1e07e5190c5fd131e1fa701358ae848ad1",
				RestHeader = "728f15c56b0581d614dce71f",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 679,
				BlockHeader = "a7747210a77472107765a3edda55c98e5b29c38e793ca15562ac85b056d8034bc9d353135a077238983fb9fd61b17f1bad2223f2e07cc987b2cf2ed5cd917a8d099c8c58ea588813a7747210",
				MidState = "ea95b46d5fc730180958b620dbc3c0a9322a3e2c5eb82265fcba6326a5c93622",
				RestHeader = "588c9c09138858ea107274a7",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 680,
				BlockHeader = "90b21aa690b21aa6b243bfa0cb7f1ce280504e3324bbbb496a7ed9b4b00c28ffd0de4a06fc232915a061b3ef7fe5dc682b574a7e49cf6bff599d5889f0690c61d7890f3d85ee53f390b21aa6",
				MidState = "54a5a3ccfab0dc414593462f777847036562ce9078d21de98f87a77918920e2a",
				RestHeader = "3d0f89d7f353ee85a61ab290",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 681,
				BlockHeader = "b3549834b3549834e9bdd4e0c62ba26d25de42f4b4f972c62718ff4cb25bfa2712b54bd3f224c7ea3faa8d6dbccee3faab664c59519e96a4b24e10dc8d73d79f858927196f67fe65b3549834",
				MidState = "1a1f893aca0e352b141e02ae439d768e8c9469fec0e4724bb4d6623d275ccdd3",
				RestHeader = "1927898565fe676f349854b3",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 682,
				BlockHeader = "006925bc006925bc525ddd1cc139148931401b2b977925177fb3c5a325160663146347cf805a212825d4e12068521d88b8339a65f90658fbb0b0217abc24d7f7b050dc223d738e7c006925bc",
				MidState = "c6964ad20ceb21e97c43b4ecdb85dda342fddad8b530e0b4cf884c20bdf7da2c",
				RestHeader = "22dc50b07c8e733dbc256900",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 683,
				BlockHeader = "4e7db2434e7db243bbfce658bc4785a53ea3f4617bf8d969d74e8cfa98d2129f161144cbb4ae115e9a4e23ef6933112b7e44f62227f9d3ac0c4356a6962714d692c3f03073a53b084e7db243",
				MidState = "f48d97e9c373b429cc64b8284606deb168d0d077f5860eb1ebf1a03de040032f",
				RestHeader = "30f0c392083ba57343b27d4e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 684,
				BlockHeader = "13f1a48613f1a48628d4ae9ee70751add77dcefebfa89ddab6b3380c294f96cf7d192cf28a7768ac147e2581566bb0f6a8c3c576cdaf08ce0babf78d4cc660076ee1b04e3c8a9ee513f1a486",
				MidState = "42c0752fe9fb2f8c4f1067f1d8546c595a7a53aa90d5a529dbb1acb188101052",
				RestHeader = "4eb0e16ee59e8a3c86a4f113",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 685,
				BlockHeader = "bd32a8ddbd32a8ddf0a6b88850c8e20c15a1419c2c04689e162e609512957ce9e45ebb768ff79c40924d0ca88c9d29919d2cfa433c54a2dcccc9be92e595c9322446a937f9d9f253bd32a8dd",
				MidState = "816c0ab33b0729365a93bca1abf825b4868570a14beeba0eda9f32b00b8156fc",
				RestHeader = "37a9462453f2d9f9dda832bd",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 686,
				BlockHeader = "0b4735650b4735655945c1c44bd6532822041ad310841bef6ec926eb85518925e60cb8717b33b08a27b9309f8daea741d6c6332c8c22f23c6120baa0d3eb411371012d215af73bd80b473565",
				MidState = "46712f685d320942f745fcd73407b76f55d111103a3aa095e07f4619309a95ea",
				RestHeader = "212d0171d83bf75a6535470b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 687,
				BlockHeader = "4319ba394319ba395129158f8ef75f466786831b88500cd9e4b3a450830ae4e52204da785822b2c75da002f14ee755b91068d89fe5fcd004cf40d0ed4bd6d33f70ed81549833eb204319ba39",
				MidState = "61669329f17fb65e80d21648f8877fb70890c1806060558d331568d94ac9fcf0",
				RestHeader = "5481ed7020eb339839ba1943",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 688,
				BlockHeader = "71ead82f71ead82f5ebf3daebae4d8bd5feb76cf04f62a88f1c63d4141bfd6f62864829287711e9818b7319471bc110b57e516dc5abd9d9259298b1cdc8c1f0f7164f469e45f7b0871ead82f",
				MidState = "fe1586dbab75b2ffb4aa8ec8cea1e6645ac7f50ed7fef5c6b7cfe8bdc10645fc",
				RestHeader = "69f46471087b5fe42fd8ea71",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 689,
				BlockHeader = "4ee3c17a4ee3c17ad3f27fae98f3715fd5fff5aa6f647062da6bfa72397c1bf386adf83ee5e202255d6d18846588567c428afab6e2042e9e28c1b40e5abc75cbd0b0a5268866b5dd4ee3c17a",
				MidState = "8999cd85b8c5b4219b41638a6278b1c3d48e4ebcf3091122f736f5f34da8b4fe",
				RestHeader = "26a5b0d0ddb566887ac1e34e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 690,
				BlockHeader = "5b7fb0da5b7fb0da27bbac23be18f42ab176badf2de0394ce2ff02761a3b6fa3e5ad7da0f24358b44d364a3f7fbd9026f577d3bf41c8acedac61ec152f86ed573e2e107e973e37065b7fb0da",
				MidState = "777d3dddd7ce1fd4039469eb40ed5b6baf916a8f288131a3c8070e1d6e74966a",
				RestHeader = "7e102e3e06373e97dab07f5b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 691,
				BlockHeader = "f6ff9405f6ff9405d8064d0d461f4f1789c53eb21401a5759e0fbba18c895682313011e420c8a0b57df80cae9f878c336ea7ae0dee5227beeae397a818b141946806ea74dd0ef066f6ff9405",
				MidState = "f8ab7b0999ed4599e3f98580e729b48fd074948c989b637e7f067a5ae3402b11",
				RestHeader = "74ea066866f00edd0594fff6",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 692,
				BlockHeader = "f619052cf619052c607835ec942f2d8397e47eea1df11bfcefcb1907e0be2fa63f0deef0e41193dc15ac46348faf05e4d631832798b078151acb8099e1df1efccc24fc5549676186f619052c",
				MidState = "4275102aa4b39dd9686a84438b5d34d8c06f6d15ff166d8891d7fb8f17327953",
				RestHeader = "55fc24cc866167492c0519f6",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 693,
				BlockHeader = "91421f3b91421f3b32b747648b4b0fbbb0a83058e4f0829f9f02a6b4c636481e4469e8e754a5d7c66ca6b3c43e096235219afd8a01f014afec2830c4d72f39421e18241a76493f4891421f3b",
				MidState = "b0e83cab5839558200aa9a996b9d44a3548631d1203c6e735d3d8abb98b1f164",
				RestHeader = "1a24181e483f49763b1f4291",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 694,
				BlockHeader = "df56adc2df56adc29b5651a0865980d8bc0a098fc77036f1f79d6d0b39f2545a4617e5e38ce26677b696c197f3048206248a780a2fb5dd8b5fb72a7fba5a277cc3d255531736e038df56adc2",
				MidState = "c3d6498d146c60e2786cf59fd71e5c716cab8467b8235f51cc547b03a1e2a675",
				RestHeader = "5355d2c338e03617c2ad56df",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 695,
				BlockHeader = "96e8377696e83776ca4b4dea8767e7ec74b5af444b3cc99af42f0220d09c11d0cc6e4dc6336216ee7a3a3ed87d3e3298a1a6d95b178cde2d31cdfb1daddcbfc9ca53dc08297a5fed96e83776",
				MidState = "7732253bb21a3981b6d3f814408777cc34003024865bee05d1c4789e93918794",
				RestHeader = "08dc53caed5f7a297637e896",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 696,
				BlockHeader = "9ceefcd39ceefcd326eae3b3af5b71a93b3dc949a93b93ba8f18b78e7098aa10fc69ddc06262f75fd393f011c99a9e1aaa050a612906aec3503f721786d21be0146d5f208ddc4cfc9ceefcd3",
				MidState = "86d124f066feb4ebd48d492df62e056ce1ecb82f0b7babc23ed73c983ea7a6d2",
				RestHeader = "205f6d14fc4cdc8dd3fcee9c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 697,
				BlockHeader = "b36b1c9eb36b1c9e8f59e55068c2ed2fab3a68a39d3fb97babac7c73a8337e843c99c64cbde6885421910adb0d0c6cb1560b828d45872d2fc814badc9f07d386baeb192067775388b36b1c9e",
				MidState = "16a27cae98faf4e99ba8bff2e11458f0bebc4d741d666a8ef303dfb34f3d80e2",
				RestHeader = "2019ebba885377679e1c6bb3",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 698,
				BlockHeader = "c0763350c0763350f8eb7f9fa8db3188929797c70fc3a1a925458e4ece4b9a2c821653c51c614da688d4feb5bc7be008c8a963cab4e5875604bf91fe7f8f7a7396d8550584082a29c0763350",
				MidState = "69fbb12b9e329f2a592dd4857e54305ceea8e6ff16c9ace5a973bed233168fbf",
				RestHeader = "0555d896292a0884503376c0",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 699,
				BlockHeader = "0e8ac1d80e8ac1d8618b88dba4e9a2a49ffa70fef24254fb7de055a54107a76884c450c096b7d957cca0b9c1ad5893e287ebf4e60382940e43643193ce6dcde73036d353dd08b89a0e8ac1d8",
				MidState = "dc025dcfaa33214c01fc004f55d1e91c07d8a266c925028c237548a01362904d",
				RestHeader = "53d336309ab808ddd8c18a0e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 700,
				BlockHeader = "77ed1cf077ed1cf0923c421adc7fcc03f599433171c7eff13b70f6e535698ac6aa469d34ba8f54a893ab833021031e33d4e1e7038c8a4ae4cab64307639937a654f4a73884eda25b77ed1cf0",
				MidState = "8cbb22abc3b0aeafc8371d6c4c4c9072d8f3050b82cb261388f912fa1ddb3847",
				RestHeader = "38a7f4545ba2ed84f01ced77",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 701,
				BlockHeader = "b03ea4eab03ea4ea5943a7c26ff3e9914489953527a41e38bc7c5123d0b3ddc18c33a26003f83c51974bc76cf4458b8a23a091d5388d1d1a808963b5f11373d6b8eb0840ef15c569b03ea4ea",
				MidState = "ae6e9f02a0bd3f8eee21baf46e33692fc6199f31ef046cbb61fb4de0e404e269",
				RestHeader = "4008ebb869c515efeaa43eb0",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 702,
				BlockHeader = "9a7b4c819a7b4c819421c276601d3ce669b020dad223392cc44da4272ae70274933e9953840cfe25977f08fe8bb2a85c8b56ec295de84ab622699077b3108174f3bcbf60d0c743d49a7b4c81",
				MidState = "bcf7ebda1968167307815e26cfaab030653db77ece1154207f05e2a51912156f",
				RestHeader = "60bfbcf3d443c7d0814c7b9a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 703,
				BlockHeader = "5e13630c5e13630cd82d80d098186bc91859ba4f81160b2887c8044bbed579da0eeb485cb9e891011b40bb6d1c3d2a7d1f11f6abcc96fb60c6ba655cce0397b2cb05236b90bd1abb5e13630c",
				MidState = "f8b208d1f1bb0fb8f88cacde51a6a31dd0343f8060c59bf7d0dbb486f1f137ed",
				RestHeader = "6b2305cbbb1abd900c63135e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 704,
				BlockHeader = "a36d9deba36d9deb938835970162dd00707da49f0d8e64d2753daf74172de9deeb9a5b80f15a4a7be5a92396c4aa2f3d1fbc44e3c58aeb981d1a733169a013f710cd7d3848e91aeea36d9deb",
				MidState = "6735e387a89db94bc3ac4640091e7fa7741319c8c258fe07c39d585185220562",
				RestHeader = "387dcd10ee1ae948eb9d6da3",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 705,
				BlockHeader = "f0822a73f0822a73fc273fd3fc704e1c7ddf7dd6f00e1723cdd876cb8be8f61aed49587b8a75d5cad3cfabed4e0903df4cbfaefbbba148be9c2c852006ae9ea37147492f3e43d1e0f0822a73",
				MidState = "2ff84e8a517798c1b9f10532cd8064ce11740bb12024f969173c328add415cec",
				RestHeader = "2f494771e0d1433e732a82f0",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 706,
				BlockHeader = "3e96b7fa3e96b7fa65c7480ff77ebf388942560dd48ecb7525743c22fea40256f0f755776154503a03a8c0f4040172c5f6a2cce5d1afdb19859715860c5a17dbe9fb9527706401b83e96b7fa",
				MidState = "af2812667bbdf93d9cb4419f2e27825263d23a8c3faee17170af4f40bea6fa74",
				RestHeader = "2795fbe9b8016470fab7963e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 707,
				BlockHeader = "8c75c2c98c75c2c916b724d6aa99e6215173a3fbd2944b6eaf9d7f9a696508a96f6d3c2951526187055b5459197d75b1bf52eae8720a85ad7b5d2e91d44988a1b2f5d57134f6fb9c8c75c2c9",
				MidState = "2ce610c7896c2649da0ca350ca2696ba6956914e71396e2327f770a2072e847b",
				RestHeader = "71d5f5b29cfbf634c9c2758c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 708,
				BlockHeader = "d211dd2cd211dd2caaa8b9decaa69b6cdcf090ccd0a459e38b491663570a6990dfa77d988198a93e4d03c0edcdc3fd30ebdccc0f6f545e9907664be4c17a74e82fe4294045e8747ed211dd2c",
				MidState = "ba9c62c1f4c1ad6d851eb8ee4272a3ade1b589579fa8b3a63371d9d57afb9584",
				RestHeader = "4029e42f7e74e8452cdd11d2",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 709,
				BlockHeader = "20256ab420256ab41348c21ac5b40c88e8536903b3240c34e3e4dcbacac676cce1567a93342e39098e953777177ed7e5e898bfcc91ef0295e65e5f3d7f723d6e01630b4815035ad220256ab4",
				MidState = "4c3ca2e08734b73f3c587af197d48ecf19d3956696b044e07fdb10b4ee967116",
				RestHeader = "480b6301d25a0315b46a2520",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 710,
				BlockHeader = "6e3af73b6e3af73b7ce7cb56c0c27da4f5b5423a97a4c0863b7fa3113e828208e304778f299520c4af7fe484b821a7116da828e0729da8c59b59c2ad016c1f04d464800a4d7e1b856e3af73b",
				MidState = "f4c1de8593d783f5526c4b8cc79c1a5fe1c9f46057438e9b96326fb4fb940368",
				RestHeader = "0a8064d4851b7e4d3bf73a6e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 711,
				BlockHeader = "d21d7451d21d74513fee809bcf7ec8425fd9ff078d2b7ed473891c04c6a06d7d06ae95486ed8a647de0e59b1ff9cd727a8e307429b9e52f9895beec59bdfd39d72fef2253fd00e2fd21d7451",
				MidState = "2a43563d14217dc1acf5e48124d95068e56d2eba268375bda7349ff16607b0fc",
				RestHeader = "25f2fe722f0ed03f51741dd2",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 712,
				BlockHeader = "f1177370f11773700cc402d576c36da9ae4230324a60fceda1bf187c7b2657b2687a6361bce33a939818346ef94e2ca8ba9f6b9464f7d07ce3cb400a8332a444f62552fd1b5693cdf1177370",
				MidState = "7766aa578c0ce8291421c7149daf61322a49b9b3292acfaa658c0c54b185ee7c",
				RestHeader = "fd5225f6cd93561b707317f1",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 713,
				BlockHeader = "a8dc07f2a8dc07f2f2a25bcd7ba03afb7c81d07be15376dca4a36cb08ca896ec9169087da52506533962339a0f74a4b55edb1cc354c1ab4e7dea64a3b6eb382c7a61af9c9127b076a8dc07f2",
				MidState = "1037fd0e30407e8a577cecd419fdcba6e41b82d53dbc1df2367ddfb764497eaf",
				RestHeader = "9caf617a76b02791f207dca8",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 714,
				BlockHeader = "878b2cc4878b2cc4b8a8e2ddfa1226ec965b3ff2a4e75490fefefafdee5f7158e7c71755198dbaf68dcbaf6413eabd06501ed7c7a2f179f2265b07d29e1f6efc422783e558860bdc878b2cc4",
				MidState = "4911a006f4588f7fd3f345fad679c122e8f8ccc113f1c430116c6574e760c0b2",
				RestHeader = "e5832742dc0b8658c42c8b87",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 715,
				BlockHeader = "7c1d157f7c1d157fba5619200b10b9ae6bf2f125c60bae22eb42d273f725c4d7ab516e621f84513b8a82dc3a001e2981547d7aa7baecdd992ea22aaf5f7fd234ecb92faacf3e5d1e7c1d157f",
				MidState = "dd285b4ec5fd77c1c1e5cb920004616aca985747b4c15b75aaea8d1506b9ada0",
				RestHeader = "aa2fb9ec1e5d3ecf7f151d7c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 716,
				BlockHeader = "496fb4be496fb4be4f859f4ed79348aef628aa741176fb335fc6c45fbe928a4e6bcd31a8c0beb78ec8c80d6dc499afefe44a2652e73bbb392d67a982b63f614201fb6d95ff30f634496fb4be",
				MidState = "a640ce2929558c89377b3ae39167472c24924bf8021b84ecb637b8aa37d86b86",
				RestHeader = "956dfb0134f630ffbeb46f49",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 717,
				BlockHeader = "9f50f9cb9f50f9cbae10f561b7cae5d0325e7ef3dfc3039e3db4447557266bb2c977242990d39fb6fee7f97a8c3c574f17a290d53408c6313bae252ed14fd20cd6318be018b35e4c9f50f9cb",
				MidState = "14f2a110c48f4ab2fc7beb985f2058d091f39dc8596551270e99c263b25fb338",
				RestHeader = "e08b31d64c5eb318cbf9509f",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 718,
				BlockHeader = "ccb1ee70ccb1ee70a6ddb1a6c7c39eb91edd07b8a7610107d8c2d48fd18294cbe95ac42c39a3e9bc274dd14215f0e1b8e291813ce2c64f0b8dfd8bcdbe04653c1c108dfb9e9b95c0ccb1ee70",
				MidState = "f7b72fe37c2228eed1688f31e67ded7821fb4666d47573efe6659e2d11562e4e",
				RestHeader = "fb8d101cc0959b9e70eeb1cc",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 719,
				BlockHeader = "92c813d292c813d2df6842751246908bd93245d7b23fcb032f7e4797212f63b6d877f1a3bc6b016ae5a278d8bef3ed7993932ef5bad7ccc98adfa3394487cff847b364efd91e704092c813d2",
				MidState = "ce6720efac5768fbed9ef68f5eacc4ce9e132a873005bd7202146baca8b49624",
				RestHeader = "ef64b34740701ed9d213c892",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 720,
				BlockHeader = "6c601a346c601a34483ac20eb464ae6288a354b72ae5081c1ef9652fb38f81c16f9c4ea8bfc168ab5a54e3588d65d2b00bf26ec6f5f9604c75a6b12e7af8fe2e5e98a0d3c7a8c4ec6c601a34",
				MidState = "5fd87c987e677fabb6463f546856784d9968bbbd7d66eb444e2798c98828e960",
				RestHeader = "d3a0985eecc4a8c7341a606c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 721,
				BlockHeader = "2b1739792b1739799fdca318f18e50c4f16f6fc82cac512f9948628e9499be57099bb5a157ccd39c50d94ad8ff9b70c9087733df8584886d1e57ac5473f6ccf4539324cd61e8f6a52b173979",
				MidState = "d50a7c89f23b18690d64f9ffdcc6f0442871f10e7d19402b5ea72cc58488484c",
				RestHeader = "cd249353a5f6e8617939172b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 722,
				BlockHeader = "390e0f6a390e0f6a10f0aeee1bff32a1c2c218cfdccf03905208ca652da77757c864f18ee6ca47bfac54e6c3857c1b87a861f185260ef96c8f445a59d38b7270b3982d85ad03583b390e0f6a",
				MidState = "87b3267e41b547adf3d7e10d6b933c7c9d1ed2e510cdd870c39b79d15995ec48",
				RestHeader = "852d98b33b5803ad6a0f0e39",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 723,
				BlockHeader = "512e9661512e9661e8f1ee72f4ec35287a884a9fdb7250492dd6ccda3d8d4862b1794bcff5ff6a1697af422259e9f7f7add496f5e611d5d6e76385bf78192ffc741c3fccc733e795512e9661",
				MidState = "771596924fff1b63290413d96eba0fcaa828e02c71e2fb8b88656c64f925478b",
				RestHeader = "cc3f1c7495e733c761962e51",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 724,
				BlockHeader = "ffb7b872ffb7b872d856170a8f76aa14c61aaea84a681bc846c26ff0df39b44bd2731963c44076ec0acd0923b79e2e01af22224106a8e3b75646f42867a39ff649c745980200da87ffb7b872",
				MidState = "e9cf8da5b13398d034ea95ec8e58309b68dc328f8fcea076ac3c145fc289034a",
				RestHeader = "9845c74987da000272b8b7ff",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 725,
				BlockHeader = "4ccc45f94ccc45f941f620468a841b30d27d87df2de8cf199e5d364752f5c187d421165fef29ba33d4c47756f658423a0e4ed60fef5673b148b9eb687aa403b77552b29a0aee63564ccc45f9",
				MidState = "8e10ffc29893765530d6a25e60627ab5bb06b70f822dfd01f96493d6697aea61",
				RestHeader = "9ab252755663ee0af945cc4c",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 726,
				BlockHeader = "befd357bbefd357b1869bc736c86a5f0f464cf16c6d5f98486ed9cb9edc6a5c4a5569e35e38e6454d9fbcc29bd5b92e5f7ae9b951b975712768a76d6f1024ba78b0f90b9e52e3c1bbefd357b",
				MidState = "bde3eadcd73ca74e5a19a4db6bae06518e4bc6e1b1129188188dcda5e3f2e436",
				RestHeader = "b9900f8b1b3c2ee57b35fdbe",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 727,
				BlockHeader = "0b12c3020b12c3028109c6af6794160d00c7a84ca954acd6de8862106182b100a8049b31b03f50aca36afae7f5e64cbabeb3754234a40e9b25360a68e7b54e2a3f856aec31e5b57c0b12c302",
				MidState = "b8de473df9142d92073d3c0ef97ac7d7505568b3c0fc215e886d9bab47a0ccec",
				RestHeader = "ec6a853f7cb5e53102c3120b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 728,
				BlockHeader = "3a6e925e3a6e925e904c0a871f2195282fdb91116d4d5067aa5e669957b89a4b426379dabb16cd04f2899ee73d31b0893b6df7256f888fdafea7a833ba5d678475d87e80ab7bda7a3a6e925e",
				MidState = "0870b89c7b2db88d411b5f89fb787219bbc4a1d7d44c3259a29e5e2fb1592b1b",
				RestHeader = "807ed8757ada7bab5e926e3a",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 729,
				BlockHeader = "0b0771730b0771739d38194d592ad3592b0ac21089b762d731a6c79f54f4bbd96cdc37531ae509c9e6eab6a8a140cf6b7f305aee3a0b82ad838844a6a004d634843368f26f1d06400b077173",
				MidState = "7804ce4ea77468ef1def0a8b712a8530358cec8badd6d5b684b010956f8d4f15",
				RestHeader = "f268338440061d6f7371070b",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 730,
				BlockHeader = "591bfffb591bfffb06d7228954384475386d9b476d37162889418ef6c8b0c7156e8b344f4aea5e8c1e391e5a6baa5cab8073e7181d6aa7d73ee124f3f96816da8a781e8d8a9992e9591bfffb",
				MidState = "56de34aba39fbf017475dcde31bcc603ecec0f080d10a289b90730ce2d0807d6",
				RestHeader = "8d1e788ae992998afbff1b59",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 731,
				BlockHeader = "f544190af544190ad81635014a5426ae50314db534377dcb39781ba3ae28e08d72e72e46c1d47df01b3a519595e07f116a4447ccfe26c15aacc6e9f799d28145052c2594c37f0760f544190a",
				MidState = "7e2f1200009456434dc3d31d5ce37269948ea40f565cc710be6ab6c698d93225",
				RestHeader = "94252c0560077fc30a1944f5",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 732,
				BlockHeader = "e4d3c01ce4d3c01cc5fe2ef61787fd6d64913a676f857b479f27e7b5823032bf8fd5b93da8708d3ab34674a38a324e811f8c4d2eee28fdf3842904d7d9db79a92deab0fd69a343d5e4d3c01c",
				MidState = "1abb4174cb56ee729fe7186698674b7a51804f34215020f543c54648f6ba8511",
				RestHeader = "fdb0ea2dd543a3691cc0d3e4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 733,
				BlockHeader = "31e84da331e84da32e9d373212956e8a70f3139e53042e98f7c3ae0cf5ec3ffb9283b638d371b9aa7a1643e4b3fbed60a2014b05e911d427e401ae4c6d5b46d4717749efa526ad1931e84da3",
				MidState = "8e2f42123f703badeb0460d90773a0f8aea434a60413b46cbdba501843737e35",
				RestHeader = "ef49777119ad26a5a34de831",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 734,
				BlockHeader = "b67ce9b1b67ce9b161b16aa5c7418b63cc5f8e28921b55bc243a987bea46896db75958dad927891647d312ed7fe9645c36099280a48e71cd4380ae52ed70bfe43c1f3cf5fa86afafb67ce9b1",
				MidState = "80fed2a1a44a3e984db9930b8582177b6da510d3091237a27a7c591e2f25d8da",
				RestHeader = "f53c1f3cafaf86fab1e97cb6",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 735,
				BlockHeader = "0490773904907739ca5074e1c24ffc7fd9c1675f759a090d7cd65ed25d0295a9b90755d6a1629eee36fd9c8e835e4be066a41540f91cb7c8f91ae4dc5c5c8343d8d1f1ec4ba63b5204907739",
				MidState = "120a5b0b42b2593962e44c48a31419eb1f84e269894710272b837335f971df98",
				RestHeader = "ecf1d1d8523ba64b39779004",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 736,
				BlockHeader = "f0912cecf0912cece30232c886680ff1e1b074acac1a37cd2c080df3fee400c8d593ec090e6e6e78d8ed325b17a830711b940187c8b21e5677ed0577a16d4fa71e75d3ff190ca242f0912cec",
				MidState = "95d91942ffa7d459e68a96c6c0443c87532c7eed68af053e073bea0bc5a3e6ab",
				RestHeader = "ffd3751e42a20c19ec2c91f0",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 737,
				BlockHeader = "df361f43df361f43b4f100ae179e98f77fa94f390742f33d0d513551f1bf70f82050798792e20574cccff4df11b5061182cdb391d0e0a7b82bab890f79d194b4454bf4e1e0eaa8afdf361f43",
				MidState = "e0b9149ede82dbc58af2d7a4bbeb1259c76f55913d7c7ebac1b5e008a88e587a",
				RestHeader = "e1f44b45afa8eae0431f36df",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 738,
				BlockHeader = "bb57b8b1bb57b8b153d311000c140ffffbeac8502350f07d8ebced6014c20be3e364fd67e0186f03d660c947e9ae09478eae60fccab497e4f69967e5e76ac69901e56abd502b0855bb57b8b1",
				MidState = "8acbe5430a55eb7fd29f657f512577af7fdd71380506bdd7f1c90edb1988ad87",
				RestHeader = "bd6ae50155082b50b1b857bb",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 739,
				BlockHeader = "f4933324f4933324ff0dd8af9e7e8cfce7ce165b1d9d3ce2643197f6c683540685cbc7baa7a7a00b9e560ec3b3c8e238b18810fef29fd24b82f88d697c4aff66ee8b2dc9286081dbf4933324",
				MidState = "e0acd90f045a49305e61d6a7d1d47659f3ec2c42d42f374a5404ece2724fe182",
				RestHeader = "c92d8beedb816028243393f4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 740,
				BlockHeader = "42a7c0ab42a7c0ab68ade1eb998cfd19f430ef92001cef34bccc5d4d3a3f60428779c4b645c0d8949c4b889b0bba121dc388727ad6a15dece15789b0bc6a346e37939ead6cbf0e8942a7c0ab",
				MidState = "08349460f1c305917ff47a71a009a7ebd31888a60103852cd8c76640f4caf700",
				RestHeader = "ad9e9337890ebf6cabc0a742",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 741,
				BlockHeader = "90bc4d3390bc4d33d14cea27949a6e350092c8c9e49ca385146724a4adfb6d7e8927c0b1d949ed16998275e419c326816f5540be38da3df91f091b5cc2e4bdcf82f7baab4423711490bc4d33",
				MidState = "5bb471eaf090b5def8f17e79b15d7efe62c5094d487a920278eddb2e2bf6fc6f",
				RestHeader = "abbaf78214712344334dbc90",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 742,
				BlockHeader = "4ece26684ece26680986f831ecf9543a8521d7a9ab2989452cced801352382c62a8a7317522537bb24c08ae1c5b639ab49756b336cb24f16de13a51dd7201b4aebacc1d54763ecea4ece2668",
				MidState = "165d18ce814b3b2092a545e72c1d0df5212c2ef82b989bc808ca9fb0a93d452a",
				RestHeader = "d5c1acebeaec63476826ce4e",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 743,
				BlockHeader = "c473f9cec473f9ce0b500e4c73df4182a3c2de03b1467a1dd5161a6dd930b36d6878a82a8b52c421b69446b64a2dc9abcc008f57b8e458e70d7206007ae3a77eed182daa6e2f4fefc473f9ce",
				MidState = "537339d3f1ed4cff21d8e4e9216fd8bc31f7a832500e85c2211c63f02d06ce32",
				RestHeader = "aa2d18edef4f2f6ecef973c4",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 744,
				BlockHeader = "36d7c2c836d7c2c8495c52ba0af834aa5ace191627f11bae3638e46ec4bba20de88647fc68cc495038f98f52933d89288d1fe69b1b123b7a7001d0db935703414d4c1da3bfe0a9ec36d7c2c8",
				MidState = "6ef13a424949e6fb88b1461f1ff06bc851edeef219393a021d2b4bd387cdff29",
				RestHeader = "a31d4c4deca9e0bfc8c2d736",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 745,
				BlockHeader = "b79f4370b79f43705cd1f57cf108e11b41e8dd2164ddb1f187bfb3ed2aa705e121fa7800e48b2cda5384a7c89e3db5fbf3a6b9ca502357a90671f4057735e29cce81e3854318cd0ab79f4370",
				MidState = "524771bf7ce44c28a6bcc19d5d5993d95013237169841db83b3164585240046a",
				RestHeader = "85e381ce0acd184370439fb7",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 746,
				BlockHeader = "561ddd51561ddd51490a77fe1f01727ec4b5c042a58d1dd290c34d2e2001f439e5429f53ba7841403881d506be2057b56566eec31ae83543e2a0fd49c0144a12e86fa7f19d143cfe561ddd51",
				MidState = "f8bfb5ce41e323f59b92bec3bf3f43d2539b7e3471731692e63f1d4b462d5a22",
				RestHeader = "f1a76fe8fe3c149d51dd1d56",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 747,
				BlockHeader = "eff7a7f4eff7a7f4924273d215a8227b6a86fd19386cc0f0770e81a9bec0ac4db2dcf86f37081cceadae19e7bc8c78aaaf8526a8aef27bcca72618d16dc9de904adb69a658940dd1eff7a7f4",
				MidState = "530aff8063da1223b3c5e5a8fb72eadee874a2503af375e5c24f8b7fed2a6ab1",
				RestHeader = "a669db4ad10d9458f4a7f7ef",
				Nonce = "60000001"
			},
			new SelfTestData () {
				Index = 748,
				BlockHeader = "ea209547ea209547c50c69eca798e003dfe08b2cf23205daade57e6e65173c3645cc79a36866f2d8af68d71a9f742cefb0b2b7a512995a26ea7b2d91b1f5571679cd0e4cabf08faaea209547",
				MidState = "95379c2438a50794977a81db6b7dbdc91ab82771e10437a6921d4e7da7a593aa",
				RestHeader = "4c0ecd79aa8ff0ab479520ea",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 749,
				BlockHeader = "8549af568549af56974b7c649db3c23cf8a53d9ab9316c7d5d1c0b1b4b8f55ad4928739a9551ec6606f4fda9a2e141ae232ae5013ded06284c3610bebe443f8853d1486bb3fce3b78549af56",
				MidState = "43928823c515305c37374d4a52bff7f0eaeceb41bd514181395728aacf332d89",
				RestHeader = "6b48d153b7e3fcb356af4985",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 750,
				BlockHeader = "8bf3b2198bf3b21962eb80e88cd6929dac7816e1d92ac88cc0f0cbc3a615c2929a87b54f7263d98bc3af6a4b91d6ba37b3a1c32be4ea1e4e01362e53b0e430efa51b781b7162fd998bf3b219",
				MidState = "4d3c0ce79ac6b962c6aeebb75343442e03421cbfdbde8b72bf0e7d54b87e0e94",
				RestHeader = "1b781ba599fd627119b2f38b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 751,
				BlockHeader = "2efe05ff2efe05ffb0fc29872ce1b358a6f7e164ee4161abb8f6e316eb527f0a487ed1b72701eb68f927037f19909a8dcba7562ffb3dc32de6243556c17581774cda5c34eeb63c462efe05ff",
				MidState = "30ce67c93af26196646d1ee8062a73de65017c3ad4bbb3f05c8b8b91cb8ac5f3",
				RestHeader = "345cda4c463cb6eeff05fe2e",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 752,
				BlockHeader = "ca271f0eca271f0e823b3cff22fd9591bebc93d2b641c84e682c70c4d2ca98824cdacbaea89a08c6821b916112a27d828c19c17e9ea038db34417eeafc32776c68aafc5688da8795ca271f0e",
				MidState = "49711e280dba2d928ca9274da69963b41b2befd92cba2ab48e5c818cde5a8e28",
				RestHeader = "56fcaa689587da880e1f27ca",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 753,
				BlockHeader = "173cac96173cac96ebda453b1e0b06adcb1e6c0999c07b9fc0c8371a4586a4be4e88c8aa415dc8ebdf391a0b3fa347f11a17d93ea637d1df7fab3c3f97a41c38eb62cc21004abb05173cac96",
				MidState = "bee23c9f37e8d10a21e9dcd4db1cbb87844901f58648cf5b180b43bcb260a9cd",
				RestHeader = "21cc62eb05bb4a0096ac3c17",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 754,
				BlockHeader = "8bc42fda8bc42fda2ffd5be9fbf0d62c18d18ea121680712edb62b2c89ea3d6e5030ea1e8d1e2e570a535d6f3a63cf7c8b8875975e94a5161e8661b7e490668e7965dd62d5d099fc8bc42fda",
				MidState = "5be9dc9665b0e11d652328936e0d962d608263f3b2962b4fec933ecd9d6f9f80",
				RestHeader = "62dd6579fc99d0d5da2fc48b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 755,
				BlockHeader = "a14f28a7a14f28a78c997da9fc9003481305fef960d57e6a73c648d7a524e719773699061ba1d965fee93143c6c4627133e3080710683045388d9d1c1f822b5ca75d0c02d69b3786a14f28a7",
				MidState = "8092b347beaaadb504d220da2c080ebecc2b29ae3c436fee2d8ef028ca641d8f",
				RestHeader = "020c5da786379bd6a7284fa1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 756,
				BlockHeader = "cc7384c6cc7384c69f62d8851a916fe7b33b5de0705546405212e2f73cfcafbe95e5a266da13aa33762f1d8c478bf5fe57cd8852f501f4dd4e414620f5261732feb8b849a390ac7fcc7384c6",
				MidState = "ee2c09fda6884b525910d63746fcd9028109c545dc1689a06be6d631f2e8d946",
				RestHeader = "49b8b8fe7fac90a3c68473cc",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 757,
				BlockHeader = "94883bc294883bc2c3b1e002ce48cc8bb4837408065b4b4f9a6f4287914db2db4c7fa7c90f92269715bc92f47bfe3c1315814a022b36d59d317e8a3d46613947eacd05603edc9f3e94883bc2",
				MidState = "97a0d26c8e844474ad4f2217d2e2860a8939e54966e565c92f501ddc6e0e6cd5",
				RestHeader = "6005cdea3e9fdc3ec23b8894",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 758,
				BlockHeader = "7faf967e7faf967e9c9646291738ff3e867d7000b40fd58d60322b05ef8490f35ae5cb1321d15d5f30ac7a5e3d3f72e810e65109a15464a221191f0da21086056336056cf6f42c267faf967e",
				MidState = "9a4bab79beb9ea8fb702b8486e1582f3e35172060b13a3d2288539358149b065",
				RestHeader = "6c053663262cf4f67e96af7f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 759,
				BlockHeader = "740cfc82740cfc82e69451f6e04247cc17e496eaf1bafcc680047f9cf04edd899b380cd7fccb9e7b08de8b5ba2b2646eda610646c0cca8b0938b5e0f3376eee0b435aa5067ded312740cfc82",
				MidState = "96cdd89c2f43d24daf74ee0b307ba684b23421b5c7990b0f23980bdedfd9d6c3",
				RestHeader = "50aa35b412d3de6782fc0c74",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 760,
				BlockHeader = "3842c5bd3842c5bd0903c399468267b846de986d370e001339f62eaf2482602d17407825417701e146362745f94fca1f831f031b824fbeeb04d5806c7ef41662a2b048419ba77dcf3842c5bd",
				MidState = "c6cc1fab13c4300ed2c812231b7ebb8d4dd85e0fc91b33591c7b213483632bc4",
				RestHeader = "4148b0a2cf7da79bbdc54238",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 761,
				BlockHeader = "13635e2a13635e2aa9e5d4ec3bf8dfc0c2201184531cfe54ba62e5be4786fb18da54fd058c3bf2ab161c286b44d2008de8b6394ddc9493ef8bab1c8f615ca1727275a555f619b97113635e2a",
				MidState = "4d009e9b9cd82b14d56684140fa1ba4154f232f2e20af57d8105e3f59ab42825",
				RestHeader = "55a5757271b919f62a5e6313",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 762,
				BlockHeader = "d1130a2cd1130a2c0fac0129fc5980722b890eb88170da092492dc97ed5df2e82d5a29d238e17ffdc5958802ecf9d0abcfaf5b3e3dfe2303d03a83e3b251f226ee681e588ecfaee9d1130a2c",
				MidState = "94257fdfefd24eaddf0d555cd54403e2dee680711d2826d3623441a371161846",
				RestHeader = "581e68eee9aecf8e2c0a13d1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 763,
				BlockHeader = "1f2797b31f2797b3784b0a64f866f18e37ebe7ef65f08e5b7c2ea3ee6019fe242f0826ce5234e51ea48cc57707cd34b232abc5085873cb53aa0563f5f7e71e9ad3123752a425ada51f2797b3",
				MidState = "a290d523a5c73662d3a37f6d2c4981b9e782387fb3d54218e010f156b4254a1b",
				RestHeader = "523712d3a5ad25a4b397271f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 764,
				BlockHeader = "ba50b1c2ba50b1c24a8a1ddcee82d3c750b0995c2cf0f4fe2b64309b4791179b33641fc5e812226d3b8e25fd16d3efa34885d61790b0ac5f4d3aaf44b8dbc168314da60525499cfbba50b1c2",
				MidState = "12eeec23d233813543e8cd32483eb1fe65ce79e57066dec8cf1dfba882ac163d",
				RestHeader = "05a64d31fb9c4925c2b150ba",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 765,
				BlockHeader = "41c2cd1441c2cd14e3e3da63c32e8e2a9d02d7c700f5a57452f8d38dd7ce819d7c983fb38f60ea095e691fed9aa10461df9a6499e50b2d999874af23f922bfe6599dd3088e0a676e41c2cd14",
				MidState = "7438a3d4643d5b3590b7464c24ed0d3f85332228a7ecc5ce4b397c94d7687bd5",
				RestHeader = "08d39d596e670a8e14cdc241",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 766,
				BlockHeader = "8fd65a9b8fd65a9b4c82e39fbe3cff46a964b0fee37559c5aa949ae44a8a8dd97e473cae892594cab965fe980bb7f0ba93072b0dabe57f5f6b93e2b0cec705175afdc55f02f6200b8fd65a9b",
				MidState = "fddd31c9c5f61451be08eed3202b4fea13d03daca87b0ef365e9aa1a9b1d6dbd",
				RestHeader = "5fc5fd5a0b20f6029b5ad68f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 767,
				BlockHeader = "2bff74aa2bff74aa1ec1f617b558e17ec229626bab74c0685aca27913002a65182a335a58a0000b1c0b10401afcd2cf9408fed5729cbd7687d4c1331792f5e481010bc05be6b57392bff74aa",
				MidState = "2a67af8cc3a71f6a52e7fc8d411f8b6c47cfa20e3110868fd8cde1c41c131b3b",
				RestHeader = "05bc101039576bbeaa74ff2b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 768,
				BlockHeader = "dcc33f6ddcc33f6df19ee189ba8fd7c4fdcccfe92de42a549e39342199e96963e494e8bcb387386f2b4159f04502860cab4793ae422fae5f08e57c697dc3165a44d466009e77be7edcc33f6d",
				MidState = "c1f18cec236b1fb83e019227e51e9023ea80eabb86f17ab36e51ca026834c7d2",
				RestHeader = "0066d4447ebe779e6d3fc3dc",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 769,
				BlockHeader = "a779134ca779134cbe6c0a8633bff1e0f05bd8d528175613f1c5b0564a91728fbbd8f0e2f0aa747db4b179a9ceeb27163fdd1fa7813510b49f0e797bb8a32544da2dbc76cfc81efba779134c",
				MidState = "80a03dd7b1c8b8cfeab14d925fa152a3e0c707c2f7e7f6172ac184aea3254a3a",
				RestHeader = "76bc2ddafb1ec8cf4c1379a7",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 770,
				BlockHeader = "f58ea1d3f58ea1d3270b13c22ecd62fdfdbeb10c0c970a64496077adbd4d7fcbbe86edde7507cc4ba93c537827eeeffc34de8967393c0ec45ffa38a515d3de7c937a7a375a06918df58ea1d3",
				MidState = "38b6ee04378c767abfbcb2a935928c9fc71bf6ea706ec8040bdd4561baab354c",
				RestHeader = "377a7a938d91065ad3a18ef5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 771,
				BlockHeader = "4505223545052235efc0e8db29d3e3769186a4ed57e8234187b65ca92f17918cf6de71c3e7708bd638d8c8107d9205fa7275413096c80034354136dc1367dc7e7f63c70925f3703d45052235",
				MidState = "c492038a4b9c2930299fd9af3aeab1839ba5193c488a92b1bda40e880e10a042",
				RestHeader = "09c7637f3d70f32535220545",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 772,
				BlockHeader = "d331fe94d331fe944c9a8227280e33eb4e23f835f8cdf34c26d02f41d41663949c94a39d88032919e2577418bb2175a04bd6f5dea866fdb6958bb8e2e0d869e2b3977e5d335622a4d331fe94",
				MidState = "aefb91c73e0537d80de9abc6cabb1b3e97eb1ad801763d68b5060b2fd0dd3eee",
				RestHeader = "5d7e97b3a422563394fe31d3",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 773,
				BlockHeader = "52e88f2952e88f291dd91b87c699546bb59ca9c68abf416dae38482ab2fe72e2aab815dec201ad54919e296006b74ec1303b55fcac6e26b52d15964095e6a64f61fd940ac2ace1a152e88f29",
				MidState = "72faa42a838d2b8c95f8cffdc70d566f738bdd17315f7decfd9107737d70e419",
				RestHeader = "0a94fd61a1e1acc2298fe852",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 774,
				BlockHeader = "ee11a938ee11a938ef182dffbcb536a4ce605b3451bea7105e6ed5d898768b5aae150fd539a69e88c0a32b61be28a352ea19c7ebaa10cfb542df4496046c7b8537a148703e667225ee11a938",
				MidState = "efd462d8dc8dbaf760188ee82bb8eb05087e7ef08a134bce85fcc93d942feaca",
				RestHeader = "7048a1372572663e38a911ee",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 775,
				BlockHeader = "1afe7c331afe7c33e40798657a151247dc32f0e2643567ea3d781d541f6928d9812244a6777aef794c2a5e217e37bd5d257414e2f3f82ed0a5aa5c67a3182776f7de8a4a49db79ad1afe7c33",
				MidState = "cd6b8d753f7d8f83547cd967f29eb8ea34c0b1c08b722a0605dfe5ff30ccf0cb",
				RestHeader = "4a8adef7ad79db49337cfe1a",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 776,
				BlockHeader = "2058bd042058bd047ab8d884528ea8358342b6bbaf9ee9f517cacbd0f917a96790b4d36efa4530de4cb402e8fc89533e1608254cf96df87098a30352fa12e5c46c1a5a4a3e82f63c2058bd04",
				MidState = "ecc0336f2965ba29c26e6537cd69cf21523062734172fcd59f4c18759ffb2380",
				RestHeader = "4a5a1a6c3cf6823e04bd5820",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 777,
				BlockHeader = "16ba835a16ba835a61dd8ca6d15a5be0524950d44bb020c25c29a3e19b645e31a97ae11bfe787656df41452165429cd15ff162b87ce34fcb26f8de9587d36041e1dc6d186cddab8816ba835a",
				MidState = "37afb53683c87265bccc1ddeee5514df7587884310f8b7bfadfb45362a007a0a",
				RestHeader = "186ddce188abdd6c5a83ba16",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 778,
				BlockHeader = "64cf10e164cf10e1ca7c96e2cc68ccfc5fac290b2f2fd314b4c56a370e206b6dab28de1665c9c8187d11a49f10fb84232e225fe28456637ad0b24a43c873bb767ebb494f0dfafd8564cf10e1",
				MidState = "bc57dfc808e16a022a226b1e8f5bf9390fec90dab0a0e251d8e416414cfa3e04",
				RestHeader = "4f49bb7e85fdfa0de110cf64",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 779,
				BlockHeader = "8ce6dd488ce6dd48ae2ced02fc3e518fc92d57a799b84a9ddea1cb7d3d4d54616b3b8e0de2d76b2f4603cfe6c905f98e070c96b237705b76c2c2cdab1539a071526ea76fbf97d11e8ce6dd48",
				MidState = "cbc0838f78d9c9a821eb0ac186e006035750ef5380731f75a50a1b3d78007558",
				RestHeader = "6fa76e521ed197bf48dde68c",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 780,
				BlockHeader = "d81d8692d81d86920275a6db572ed5bd77cf85f78a0183526092939e22b39a7cda73a3efde291d426e0a1b8575c379be76a9dc24eb3865c594bf827b4391ebf395b05b217ef7a537d81d8692",
				MidState = "8f7a662b802fde61614ce93b8a1f48fa938b168c8cbce6a916e8f390ad31bff5",
				RestHeader = "215bb09537a5f77e92861dd8",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 781,
				BlockHeader = "2ad96d322ad96d3262900f0493b9a3ca508d3fdc63afa9438e96068714df42b423f5b3747833b70ef03055f03b31e8b44bd7f2f11a19d2865dd62703c15aa112c784976da8bf554c2ad96d32",
				MidState = "0fb033ee3edd4b504cae2b0bd3cd68072e6e818abd24a799c90da9e65aa7b8aa",
				RestHeader = "6d9784c74c55bfa8326dd92a",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 782,
				BlockHeader = "f22fc621f22fc621d20d3cec50179281ea52bfd3c963bb89c4852ad5539dca81a42e0cc1ac95bae44ec392a457507d532ac10530390f2f168e0cf690786173f8a6a5ed3404db0959f22fc621",
				MidState = "e053806237a0296c93fafe24335e898bb4c3779067c0b3750f00394904fef35e",
				RestHeader = "34eda5a65909db0421c62ff2",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 783,
				BlockHeader = "de28a398de28a3984cd481b6184954de8505c73cce288da2860b8fbdc5d9484509a8fdfa81ebff2af9cb0f60fe60da56d2b27281171137aa0cb96383fb3573538bbb2a30925c4913de28a398",
				MidState = "cf2d90828d7e8c5373055b1f9f925db3f68336c06a49c650c66fb609f24f671f",
				RestHeader = "302abb8b13495c9298a328de",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 784,
				BlockHeader = "7951bea77951bea71e13932e0f6436179eca79aa9528f44536411c6aab5160bd0d05f7f1e6a2109c9d1eed8425846afa3d040e7c3bbbad4092e1b779bab00031bf081d708769b9f67951bea7",
				MidState = "4aa668d94aec2185458ac830e90a1432545126612778a789413ac26dabe11dc6",
				RestHeader = "701d08bff6b96987a7be5179",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 785,
				BlockHeader = "c5468222c5468222c746cd351c4da372aba2dc6a28e0f61f4964cfa731414fddf3ce083f36cb31dcc7dbac720377f0a0e74fcd46d80097305df671a80354161dc3cd5b562ec7d104c5468222",
				MidState = "9e04b8d1c6199af0b2e00f9bc837d5c3c1c6d05807481dda856673f11a5ab5cd",
				RestHeader = "565bcdc304d1c72e228246c5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 786,
				BlockHeader = "417f3df3417f3df3bddb63d44dad9d023809864348f355ee72914387eb5ac521a393bfe8ef58dfb5a68f5ffbfa3ea3a3d30c613e11d92a09d792ebf13de6826365665a4fa6fc63cf417f3df3",
				MidState = "0e589b010d68e1a45fd8a3545cff1b3c0741e5b8ab424ffd0ac78c0ce7d4530d",
				RestHeader = "4f5a6665cf63fca6f33d7f41",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 787,
				BlockHeader = "dca85802dca858028f1a754b43c97f3b51ce38b10ff3bb9122c7d035d2d1dd99a7f0b9df22ba4b68645183e2d94517dd57b9395dcdea231699b521e7a180b9e29e157a312935fa54dca85802",
				MidState = "a66ec7a751e888058c3df0f8429889e1f471ca21ce1443d4e6b49c0fa5c2f774",
				RestHeader = "317a159e54fa35290258a8dc",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 788,
				BlockHeader = "2abde5892abde589f8b97f873ed7f0575d3011e8f3726fe27962978b458dead5a99eb6da8f146919a9806594e62d11c8259b8ecc061b6afb291f826a9d1df4e0a04caf6414c77f2e2abde589",
				MidState = "cfdbdb57d145c5b91097f7eaa6131f8a8f2134d6af8c551cc9ac3e93761eb5f3",
				RestHeader = "64af4ca02e7fc71489e5bd2a",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 789,
				BlockHeader = "56985b1356985b130b2cb38cc17fe5a23ea1d9258bb30aee6e110f3bc8f673a10e199c444d61d54a0683fbd01f5bf0e77c8f1b184d28b11cca3a3c4c0aeaa2f59122d00f7843a02556985b13",
				MidState = "f44f6767c06a24486ab2a10920098f774b65fee42c30d1c92132c9016b8a4e21",
				RestHeader = "0fd0229125a04378135b9856",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 790,
				BlockHeader = "42c2f98042c2f9800a25f7fa39f53b608a3ae6af06ea8e52e6b1c51c0ed07d0fb5f284ec59118252c039e1f9d943f74ab00d6335771ca24738069424b511385f0d90e347a966f68142c2f980",
				MidState = "4a4f208bf7087467233f2a7008a229dfa4d5f97046ce667229dfd8b48ca63a1d",
				RestHeader = "47e3900d81f666a980f9c242",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 791,
				BlockHeader = "d5375446d53754466d24d5b354d3ea0852ad4c5919b9cb50983230b0957f155f8c918b5a0327b8f06727d552605ef546f2b98ec5117a6099aec03691ae76bb0e9b496a35fd5c9b8dd5375446",
				MidState = "9f14f62b887d986a1ebd571441df138d3db68fef18220009a13638f929799558",
				RestHeader = "356a499b8d9b5cfd465437d5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 792,
				BlockHeader = "953fd6e7953fd6e7a082d6d855795538ef3185afc4ab34b2c8fd064e58f393ecced90070caeb66aff3c8e3d56e979cd851592516a82ff08db8ad77c8ec32ac30b287273a40d93f11953fd6e7",
				MidState = "27da3b88e63cb345ec13c4c08ca4c36baa220a6c3a85666d687f5e0faffcc537",
				RestHeader = "3a2787b2113fd940e7d63f95",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 793,
				BlockHeader = "bf74fcddbf74fcdda902f06746fd3d5c78d4d7fdc438e544a00384b4efb33a13929c824c07fda208a8f6817e45e5da9b4a7f225f9e69d6fd719800af85109a33494b0322ef028667bf74fcdd",
				MidState = "05c4e635d21cfaf1f91ec3630bc9b78c4eae19b33ebd9f7f5a78eb4bc697450b",
				RestHeader = "22034b49678602efddfc74bf",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 794,
				BlockHeader = "a26bd003a26bd00381600cd4606d77ade50f11e9138fffc6a4707a160d74d07f682ca62039132c397633374b2a4b99700b1b5cd88f424d4d8ae575d1a08eec2a9ba7e978116e2e37a26bd003",
				MidState = "7d05fcd53afec35547c76935bd6ccc83a7d25637132b5ed4b4217c5867d3e9d9",
				RestHeader = "78e9a79b372e6e1103d06ba2",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 795,
				BlockHeader = "41128a8441128a8433e2696fd24587a64f72400ac1a2997cd989072fb44e75aea875f671028ae2a2c5c28fc705b1bca6462c006dafb502d4beb50145bdf5b57113283f0897da03b941128a84",
				MidState = "4d056bfafeff53737a7c0884184b4f66df9cd1500bc97b93c18ea1d271de886c",
				RestHeader = "083f2813b903da97848a1241",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 796,
				BlockHeader = "6cde1c876cde1c87669e36da5d5a7a91301dcd8cb1ff5bccacfb75d1c05170ec7efd6584c6b80b484dde606ed3e41d088a919ee39f6061be6eb70f0a3612c563065be130913776ea6cde1c87",
				MidState = "fb06e9bf4af4e4530dd3d9dec8f0c36d099a4270cf88c802f8b814e5feaa0469",
				RestHeader = "30e15b06ea763791871cde6c",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 797,
				BlockHeader = "cc8755e5cc8755e5f53ded948286933819096aecf1556af59510cafd75a6abcbd0c90a8839480cc0190096c3b1b40ecd044e033dee2df6cdffb2bcb35a86aba9cc3a6e3143e14511cc8755e5",
				MidState = "62fd1d238ba548616c75038c7b637926e7de1705412f9991444ba63708e0e149",
				RestHeader = "316e3acc1145e143e55587cc",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 798,
				BlockHeader = "68b06ff468b06ff4c77c000b79a2757131cd1c59b855d198454657aa5c1ec342d425047f7bd17662213e5acb638407b62d92c605cdc33413c66c34f1f9bc9a4a1242e95f96c6be6c68b06ff4",
				MidState = "a31f3675bff07ca43ea1365a472683098bcae4fdab845c6e761d53c0076e72dd",
				RestHeader = "5fe942126cbec696f46fb068",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 799,
				BlockHeader = "144c5adb144c5adbad99e855e7af66e165ce73ca165885d257ecde05fdb20fad9a344b8f4758c4d5ecb77b9a1d0e309218d9d9a71acd6db8f836f4066a76fabaa8785107b907444d144c5adb",
				MidState = "cbe27d3f1554c7e79cc6a926f452d8e143adf7867d40468787670e98ab8b53f9",
				RestHeader = "075178a84d4407b9db5a4c14",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 800,
				BlockHeader = "6160e8636160e8631639f191e2bdd7fd72304c01fad83923af88a55c706e1be99de2488a1019eee62df352469fc5c0115b72e29ae6cf378f9f0915285a5f76d946f81c4a8724130e6160e863",
				MidState = "5aeb40d8d4641faf48cc8c862c6147b4ca70a974567b6e00c0d543c0b4a34318",
				RestHeader = "4a1cf8460e13248763e86061",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 801,
				BlockHeader = "25144ef825144ef8938c2be22d0c1aa9cb6da069e155bd02676c6e58d928972b7624187d3fadf1e468538f2d15c098281b0614c73c1159dc91f3e3314311b93e26933140a4c932af25144ef8",
				MidState = "f85b6a62b288d72fd3a92077a3cb5566286e55519182063b472b7d001162e005",
				RestHeader = "40319326af32c9a4f84e1425",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 802,
				BlockHeader = "894f9529894f9529359f6d99cdb4ddfbf41c2e9cf7fe81d7ac4f14c9ed1b4f06e0f4cf84d0f53c0eb7cb6ebdeba82ef979b861ad4ce8d526a721bc135c424b5f82c3ab27c0e1dc24894f9529",
				MidState = "c57e0fdca21d0b8a3d2d3008d520f1ac4ad2c529493ca3787b57b8a70e031f8f",
				RestHeader = "27abc38224dce1c029954f89",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 803,
				BlockHeader = "40d635af40d635af73f3e74e0ed35e687ec826bed5065c10cfa4de98ba6e409040b2232c03c6219f8619bf639f191f4d6bef3a4abfedeb352812f88d8465df9a54f8367b3a596e9540d635af",
				MidState = "8cff2739fb99b8f9730f399af2dae45cd98d4d71fd5db8d226d69db3c7853f5d",
				RestHeader = "7b36f854956e593aaf35d640",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 804,
				BlockHeader = "66c3d8f066c3d8f0a5cf1dde4c70149b8c259e9b75b21c9e13ab9bbea554bb56dca8fa2ae10c809e7be2fa2676f7e3a5cba1390b30d3394f5dfec0fbc73433903739dc49de3d51e166c3d8f0",
				MidState = "871285a29047ee641c499d8a7c74e2a798056eb56dc7456d77cb16c784ba6db3",
				RestHeader = "49dc3937e1513ddef0d8c366",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 805,
				BlockHeader = "df89ee06df89ee06ba5890e8b7ad4ffe7ecbd45fadefbb4e83aed20eb24fccba4329f923f7ef402fcdd028aaa879934158dbd699a8ee8c038e97270f0adb33983a81fb4884d6f44edf89ee06",
				MidState = "d32e040bb9127b2fac19eafe073891a94308f9ffa94298fe6af319b26c36ba76",
				RestHeader = "48fb813a4ef4d68406ee89df",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 806,
				BlockHeader = "97492db997492db9ee9a8501b42101fc82f96a3741f6adcf18c65310b4165a163cc28152fb60a8e5f0a7dea87737b85cf5db079573056b87edcea9c1c775994f4ce526003f5f813297492db9",
				MidState = "5069191084cebf884cda91ed212867e92044903e7e7d05ac64fc9e994cff261e",
				RestHeader = "0026e54c32815f3fb92d4997",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 807,
				BlockHeader = "e55eba40e55eba4057398f3daf2f72188e5c436e2575602170611a6727d267523e707e4d1fe261d46d6742b780d380f87fa25e79fd1a7c212fd30c2eaec0c22d21a6725a856a813ee55eba40",
				MidState = "95ae3edfb500326f56198baf9643270057e10dfb7ed75e858cd73230e234592b",
				RestHeader = "5a72a6213e816a8540ba5ee5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 808,
				BlockHeader = "4706203e4706203e228bf756bd71a62e4f61d1d192abbb156e881667ee572fcfe7a18e70b870231d25b6d6221c8230013e9b0404ee53efcb9f6f291b2c8052ad22f2830928b14b5b4706203e",
				MidState = "0c2c80f40436124d8117719eb72a30b2ee959dbe24c80e6c9622b2fafcea3f15",
				RestHeader = "0983f2225b4bb1283e200647",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 809,
				BlockHeader = "ca49f4f6ca49f4f6cbd2445e2e83acd40b3c460eca4ed2a00bf6e1e40dd493b8f516b626b27003a8dd8c8fc92391d8c81b800fc11cfadf30f6c0bd8a6f4137b4f6fdfc534d076cb8ca49f4f6",
				MidState = "b245d86221b62159de64b1221414c0d9567a6cde7cc7dcc3c6b8905a1bb5d2c2",
				RestHeader = "53fcfdf6b86c074df6f449ca",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 810,
				BlockHeader = "185d827d185d827d34724d9a29911df1179f1f45aece85f26391a73b80909ff4f7c4b3222c4327ebc8674bd287bb95f66ef8f22345d0c7da13f64c4203685fb7692962703aaaf475185d827d",
				MidState = "6b72b86917133c876245d631bd1941dab8d20e2c1dfffcc73dd20a01d42d958a",
				RestHeader = "7062296975f4aa3a7d825d18",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 811,
				BlockHeader = "7f121bbe7f121bbe6b57399653393979160f5fabbf2ec910f35b49ab4d1559e6379561dcde40ee89f09caad53e7874021d88cfe976aaa15fe4ec19ed4296c71828a66941d2378bb67f121bbe",
				MidState = "f1afefc7bacb4a4f41aa4cf78e01ef1a651015b8c32e378e7451bdb057155f26",
				RestHeader = "4169a628b68b37d2be1b127f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 812,
				BlockHeader = "6850c2556850c255a635554945638dcd3b35ea506aade404fb2c9dafa7497e9a3d9f58cf13fe10236699bc2e2c1bff76bf68a72cf769e360d02845999eca2b4ff669230ff9a30c326850c255",
				MidState = "68ff4d8b9dc41439ba8f989992c8146f6dfd9b35460f0ad93f72c81d15d0f42e",
				RestHeader = "0f2369f6320ca3f955c25068",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 813,
				BlockHeader = "bb11a44ebb11a44ef465a378b7937693e5ba2f5e457a2941712f4f5e4684e973ee7cca6f9e58728cf6729ceccc2ff6aeb52abbd826e4333337dfdeba91e735b7b8b39012e87ffb65bb11a44e",
				MidState = "a3cbb2464104c4ca2fc9323e1dc64257c94e5dda5ea05e27572590bf2f731225",
				RestHeader = "1290b3b865fb7fe84ea411bb",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 814,
				BlockHeader = "201e7610201e76102093a6360bf2a0fd5dc1681fecd437d21d3130c3993d3c9badf63b8b1ced136a19a8b67614e994d04750b1d47429e41aac98106f71b4b6f79d7ee52c1bf7992e201e7610",
				MidState = "f068bce3cf5097dc874e18cdb546542ffb235673bc9237e4af680ed100d24aba",
				RestHeader = "2ce57e9d2e99f71b10761e20",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 815,
				BlockHeader = "bb47901fbb47901ff2d2b9ae010d823676861a8db3d39e75cd68bd7180b45513b25235823248dd2bacdba886e0ea2e100bd78e6c221b1dc4d5f87690ac601592477b3c7714c2b94ebb47901f",
				MidState = "b9989a95729a5f5a722d95dbf8b3e20feb7a43bf6b4fdf965939b1be39fa5d18",
				RestHeader = "773c7b474eb9c2141f9047bb",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 816,
				BlockHeader = "095c1da7095c1da75b71c2eafc1bf35282e8f3c4975351c7250384c7f370614fb401327ed32d88be931ad0ed6914aa592a124d4b1e4dc237bb9a0e3dd28da01fc7a0661f63540118095c1da7",
				MidState = "782b30c3cc7a941e7f4adb5a8a42bc6383c3cce8914804f07dc995c8c96b986e",
				RestHeader = "1f66a0c718015463a71d5c09",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 817,
				BlockHeader = "b8afb494b8afb4942cfcfa0b3805d7403c168c95084115483dadd5398438a21289582c820e7d0dbc210ac73566585be9cb8d724a46e95f3410e7b2ee17afc5104b4bf62112793258b8afb494",
				MidState = "f7c5f301a53a1e3c5c33cd9cd4cb7d1cab5488328d91567d1dafa4abda27c087",
				RestHeader = "21f64b4b5832791294b4afb8",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 818,
				BlockHeader = "06c4421b06c4421b959b04473312485d497965cbecc0c89a95489c90f8f4ae4e8b07297ead497cf8e7ff4cc8f9cc2afc8bbf5090c4d9af443f02015bf945c2878176ff24f3af120106c4421b",
				MidState = "425c3349bdcc59a567ef115f856c51ea550e9f2663c7684fc5cf1cf962ca52d4",
				RestHeader = "24ff76810112aff31b42c406",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 819,
				BlockHeader = "39403c9939403c9986fca19191fd5586b06433a18d612c20f8a45a1a550379d75ff66e2d60c0eefeec22f844d85a758c047575419e040ecd635756200dd9f4dddf197315c5f50e8539403c99",
				MidState = "3efd874cefbac8084e0641f838b54e995802ba6b71b0a87a62722523646fd4a6",
				RestHeader = "157319df850ef5c5993c4039",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 820,
				BlockHeader = "0db844c90db844c907255a21af85046512a171492ae5e3d1d6d20b7b56a032d1dbb5c36ac0d1842b7d4077154b8af15da1af69eed81e1f687a4ef88e7894c03d426eb86ccf52dba90db844c9",
				MidState = "7d1eb1508b4b1e92cd490aecba4a90391b7f751fb8ebf5f2f9f71abcf147550e",
				RestHeader = "6cb86e42a9db52cfc944b80d",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 821,
				BlockHeader = "b3de0ca5b3de0ca5b5922e09e65972f0e777cf109cde55465ea781c067ec9a03a24bb2bbaa1d4b1fcc13bbba42ae305a9ea0e869ed5faa641c65b567a4eea7f05e54975a43d1c17eb3de0ca5",
				MidState = "1f22dc834a7eaffc3e594ba0708889a9183fca785265882383bcf7214c33daf0",
				RestHeader = "5a97545e7ec1d143a50cdeb3",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 822,
				BlockHeader = "9c1cb43b9c1cb43bf0714abcd883c5450c9e5ab5475d6f3b6679d4c4c120bfb7a955a9ae101a207d54ca731bb1731b43307cbd9c37e3a1d8937c91c271e4a5575ec5d30adb1da6739c1cb43b",
				MidState = "58a844be34566592e3cfe89470bba53360f451f07092dca44410e544f046b79e",
				RestHeader = "0ad3c55e73a61ddb3bb41c9c",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 823,
				BlockHeader = "87e6a8a687e6a8a6c30b2057a48bdc8135804207f778c20e4996d825af25b8bb79be5037f7516f6a911495b32145b2d4ac2552767400fa0d9919bb3c175210e5c27606744da58c3187e6a8a6",
				MidState = "e93077fcc26651e6a180b47c0b6b0b40217af566bafb36f2027aec2e26c59ff7",
				RestHeader = "740676c2318ca54da6a8e687",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 824,
				BlockHeader = "ce8bdd23ce8bdd2354d089e404e266a13587967eb52557cf93290a4e417bd9455fd9e64947d4a2175bf9e3d1a4e00a2cb43a06d81d9ace67ac5057424f93d016e8867e3a72e51525ce8bdd23",
				MidState = "b7a66c3adb9ae0ad75bffbb5eb778f72aea6c085abe2d7e0783df6114dc7611b",
				RestHeader = "3a7e86e82515e57223dd8bce",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 825,
				BlockHeader = "73db964873db96483a46c793e473d3f074fb28065799fb027bddb08d05192c351efa8f51c9edef75116b776378a3a972ed8fd4e87404ef1db1fa9d020759eae35ead4a62e0af0a8873db9648",
				MidState = "01f77a5f5a3fce87fb1aeb1549f132c84b62e11e2ccd1fc67f5076bebba6a98a",
				RestHeader = "624aad5e880aafe04896db73",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 826,
				BlockHeader = "c1ef23cfc1ef23cfa3e6d0cedf81440c805e013d3b19af53d37877e478d5387120a88c4d455b67ff0588331386f74b659c47b39c43c40cb539ad566c41ddbc1bccb165418986aa57c1ef23cf",
				MidState = "5257ee57e56a35ce2a1820ae5bced2f84c9ed758bad0fc56c87a47cc26b7edb3",
				RestHeader = "4165b1cc57aa8689cf23efc1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 827,
				BlockHeader = "10fe489810fe489809346cb40c1f044201ec2654a7a27e6cd3b6ebfb4325b63c8379fd18cca5bdc3e00619a3f4fb38e3dc1501c39e0450368ad2243e48fac9e87593964daec29d9b10fe4898",
				MidState = "af0d6307c1a723acebf81819353710c3c930f4e35aaecfdc8e1bc19f94d66dff",
				RestHeader = "4d9693759b9dc2ae9848fe10",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 828,
				BlockHeader = "7de156137de156136cdd7a8d8578f2f56aed20ea8145a84001b90cb4919a04e5556e0e281f457cf362c1e486407e3cee5e16781115dd19722442762f6f1600ccfed5c3350e9124c47de15613",
				MidState = "4b20068b72c638afd2eb5fb3a243555c2762f0fe99c6cd17187a3b8c5ae259d3",
				RestHeader = "35c3d5fec424910e1356e17d",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 829,
				BlockHeader = "fdc16bfdfdc16bfd58ed6f18f6564876c4b0ab72968679aeff4f2d26ca2315254d08d95a4679551a17c688cf0e709a95704186ba48913c6c21ac0f83f3a500263167c53e6466bdecfdc16bfd",
				MidState = "77b77cdf91bf71878e6d402c1c325032f977f7d5ce1ac5ac698b98db68e2a9b7",
				RestHeader = "3ec56731ecbd6664fd6bc1fd",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 830,
				BlockHeader = "0bde79260bde7926f4ea7a7af20923c26d95bdff25f7966286b6ca1b00539fed90fe4b6c2f8ac3293140a41ad406429af875fc80ccd42f2ffc9bd30053a021c117b50535ae52156c0bde7926",
				MidState = "744d23edc710189f3db567cab713fa5e395fb90b3d1f26563c9cd55397e7908a",
				RestHeader = "3505b5176c1552ae2679de0b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 831,
				BlockHeader = "17c95b6517c95b6588d4bc44ae32cf2e9b5b4eb5d1dc082cad77b4643e43d2ab8013c422e1e0e8352d675e4e16f411334c1d483a7ba373f87d32bd8815c26658931e02739574a07817c95b65",
				MidState = "ea83f22a64b3a7e917290ddf9795c4359219569d7b4b5603a7b2867619bc899b",
				RestHeader = "73021e9378a07495655bc917",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 832,
				BlockHeader = "488248c6488248c69b61b75b3a2d1b3a207e9ae3e67d59e6687217d153a457c94082699129d6283e9da16930cde39a9d9126bc0a27ea70ca7ab17bde36d944c31967f96b0bf60478488248c6",
				MidState = "300ab49dd2edf2c508303f09889d6355e1ddacc84f966a4e17bf51920f912235",
				RestHeader = "6bf967197804f60bc6488248",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 833,
				BlockHeader = "cb1c51b2cb1c51b268717b953eb15c03e7803f6cc722b6d59f80f9012f2ceed0d6bb7f60a2e0ec7fe444f4aa5a3f784b297a80effa3a252b3bbff3e4ff71d317e398cc2afb80d618cb1c51b2",
				MidState = "3c6e57fee68c326dcc4fefd85b5a1a0c8804bce5c889233b703f2921ae825209",
				RestHeader = "2acc98e318d680fbb2511ccb",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 834,
				BlockHeader = "b45af849b45af849a35097492fdbaf580ca6ca1172a1d0c9a7524c0589601384ddc67653951ba5580b39148b2d8764c01af6bd03f708889577142be05b712d65343bd54fff51e055b45af849",
				MidState = "3fcae0e3563ef2cb11fc49967a79720047be930afed447e8fe33ac35c253ee5f",
				RestHeader = "4fd53b3455e051ff49f85ab4",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 835,
				BlockHeader = "31e0940831e09408875037c532383bf52a9a97bc0b127caf031b221d05f88a42ae1ab20efb6bfec9b3a1111834588db7a724a21feddeef9f81b618d28dc933633643352bc205743531e09408",
				MidState = "8c2ff2083a8114aac62da9ea4dece5e5da6b77c5a9e6cc72a98e0209f7439fbf",
				RestHeader = "2b354336357405c20894e031",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 836,
				BlockHeader = "7ef421907ef42190f0f041012d46ac1136fd70f3ee922f005bb6e97478b3967eb0c8af0912f798000703ce89504ee1a8b328282f5f9087b783c22847235e43a10b73407bbc1ed87a7ef42190",
				MidState = "230e9460a65789bd0093caffb0465628e25428c049c4505202c993d15607aae2",
				RestHeader = "7b40730b7ad81ebc9021f47e",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 837,
				BlockHeader = "9636f0ea9636f0eaef4ee2814f2cde186a1de8e029de4ee875ed93880b742f065ecb8b4f99937de3a36d655f5168386eec9dc5d026d1ba659424645280ccfefaf8d0ce63877204c59636f0ea",
				MidState = "4882d5a7789b88b083a6b7f73115c38d194c0beba91c6cc0542f4721dd4cd8cf",
				RestHeader = "63ced0f8c5047287eaf03696",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 838,
				BlockHeader = "edc7365fedc7365f71660557319267564679c7f1444b0b90be6c020eaad4c67068a556c26fba6da2e287c8457638d159e2778a2f39478f35b052139e79f8a81fdd81931c3a0123d6edc7365f",
				MidState = "e357770b197f9df8afd8262caf4038a2d34a4b792f69767b131676a137508b34",
				RestHeader = "1c9381ddd623013a5f36c7ed",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 839,
				BlockHeader = "d604def6d604def6ac44210b22bbbaaa6ba05296efca2584c63d56120308eb246fb04db53047155e11220d6a3e60348bd5598461ce8c6417482504aadfc556ebf4ea6a0cdf264220d604def6",
				MidState = "a65d737fb057482a63764d94d8421d3347da2980ddd3420bb77ba1a127e4c4cf",
				RestHeader = "0c6aeaf4204226dff6de04d6",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 840,
				BlockHeader = "cdd9ea12cdd9ea12e77192f530de14407bdf680b11695f8622f36b4e172601cf849582b62d54a1a55b2a6440bd9a8547820109bcd424c9efe1cd350d065263f120568046db3aa493cdd9ea12",
				MidState = "5bc3799008ec6b37aa7ae4b1db4a8dc737b269e3f18928ccfff35454e0925ba2",
				RestHeader = "4680562093a43adb12ead9cd",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 841,
				BlockHeader = "1bed779a1bed779a50109b312beb855c88414142f5e813d77a8e31a58ae20d0b86437fb216bd737492332d5438738f25c739325b17342c143e1d1138968c489facca39509d3d8f8f1bed779a",
				MidState = "43c9edbac8784836f41d8644c05fda209cda7b610540300db2296dbd644706a6",
				RestHeader = "5039caac8f8f3d9d9a77ed1b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 842,
				BlockHeader = "f9c834f7f9c834f7dd25cb223263d6dab84c8e6ef6326b73c91d798b2b3d237537409097e026d935aeacf562ad2befcb140a6192982a139123ee3c1e37260d1b72023433927fa2eef9c834f7",
				MidState = "45873d9febf740ae628112f2a3c99656e866e2782f519a1410578b03771047fa",
				RestHeader = "33340272eea27f92f734c8f9",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 843,
				BlockHeader = "47ddc17e47ddc17e46c4d55e2e7047f7c5ae67a5dab21ec421b83fe19ff92fb039ef8c93dfec223ecac1012a2ffee48da2fcdf102b541d19972af782d25182d021bdb405864b9da847ddc17e",
				MidState = "25676a3120659268a35ebc88b0ad8253946a012a055f2db5e3cbc648738a9684",
				RestHeader = "05b4bd21a89d4b867ec1dd47",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 844,
				BlockHeader = "d14b5c79d14b5c79772cff1da74ad7999631d56ac3d5af844d5c4f4e9928ef50d91914292372192ae9bc75de73cd342b7473bb5f352bba11a133f73a75e81cc5d7fb8a68536c4abdd14b5c79",
				MidState = "4d3cce0661d1e7169d27c8157be2ff459520c06028f506ad216f90ba9f2eb91a",
				RestHeader = "688afbd7bd4a6c53795c4bd1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 845,
				BlockHeader = "17443f9d17443f9daabea62d262bf3102070e7d9cf1f65dc007879a18adb4658efb7274b33b1a2a62e78ab2f803de934a089df9e52da6fa28669b78eaf5ba18afa2c1377d8a529ba17443f9d",
				MidState = "ea36a7759b1fac48f503f3597a7d54fdbd5622f622c787741776278a83182a7f",
				RestHeader = "77132cfaba29a5d89d3f4417",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 846,
				BlockHeader = "35a7ec0d35a7ec0d409ca3cf91e9c96c3b39aab914284588d1a89c5e30f66faa09d84f357e21d1d950289fa8e551663e11fa8ee4c468b70ad472e8213cb4f1792f92021f5f26ac6f35a7ec0d",
				MidState = "116de0a8a79dbd1c639d71d7d9ac886f24e86d9aacc0f44b68f581352943508d",
				RestHeader = "1f02922f6fac265f0deca735",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 847,
				BlockHeader = "d7c56ec4d7c56ec43375d1718396b6108aff50ab7795b6ab9ef7deaa688db236cefc4ee6f064a7b4d8b4a6fc3cee11e6dae3a30c3648bdb37964e931ed5f2920221d9562785507b0d7c56ec4",
				MidState = "378376e42cfb5d10b01d2063cc451e7c3b4ee02c9c1988c42fed2bedf6c82f5a",
				RestHeader = "62951d22b0075578c46ec5d7",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 848,
				BlockHeader = "00fa94b900fa94b93cf6ecff741a9e3513a3a1f97722673c77fd5c0fff4d5a5d92bfd0c2f9af0e9c8929cdd8ff122f9f1ee728325f0b49d9549988d081b465c45956662b347271ca00fa94b9",
				MidState = "e4bcde2dc7a5a9f4c286014259a2c9f53218cd5849492a55cc6d4bb46668a7a2",
				RestHeader = "2b665659ca717234b994fa00",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 849,
				BlockHeader = "4e0f21414e0f2141a595f53b6f280f511f057a305aa21a8ecf99226672096799956dccbec8231cb3b110fc1147a344a115d29a8255435511060bce6270d6f6b813728c4ab9b1c8964e0f2141",
				MidState = "ec63c544f4b94bd210ff1734153a3ae11a1ad44e548f4bfd72e5015965e448c1",
				RestHeader = "4a8c721396c8b1b941210f4e",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 850,
				BlockHeader = "d9f00202d9f002022a05dcc1765647de33c06451c952ae8212936efedd09871b32b77baa0f63665e84f874521e3e92f656ae527b2a0fc7b8652b2846daa3d0f6cfaaba77b847d10dd9f00202",
				MidState = "c17d3b95b7288f2dc75c6fad9333cd7a91342fedc4fea90e1a79a9965c98c6ad",
				RestHeader = "77baaacf0dd147b80202f0d9",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 851,
				BlockHeader = "75191c1175191c11fc44ee396d7229164b8416be90521525c2cafbabc481a092361375a119f1dc19fd179db743e49defd4ffea537a520ca41e60a901ba1a878da18aae39e6630bfd75191c11",
				MidState = "d785a9c2e4d93da888f0130b738539e90117d72e0ad32e6c14480e1423f7b8cf",
				RestHeader = "39ae8aa1fd0b63e6111c1975",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 852,
				BlockHeader = "90f1500d90f1500dc0b17e95133c1f1c8a788beea3de7027a49cc737c59a66f62e7281d7efeb130d116e8f7c944a990ba79df0cbfdd52a1655cc3bf2f24396aea98c984513cf9ace90f1500d",
				MidState = "d6516a2afac96306b2a04dc0cfa6e7ea60bc76baeea58bc0e2c2b58e89cefbe0",
				RestHeader = "45988ca9ce9acf130d50f190",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 853,
				BlockHeader = "662f784b662f784b7164e4d0c8bb24ab9040f4ec406c986d15bc54c2a79544ed2f39e597bdec7812961f62b75b821167e14e530af7c729e8b3ac5af8af7aa9a7005a8c0b62d07068662f784b",
				MidState = "e2b073650dfbb04ff99779d0c9db0275b8df199bd603d1b63fda8cfa745c2e1b",
				RestHeader = "0b8c5a006870d0624b782f66",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 854,
				BlockHeader = "4d063b334d063b334ba1951730a29bb4d0c9437ddf75b3b0a794fda1e6735ad13f5ba280df64b7fa8a5243b83939335b3e497d33eb8a72cd45e4dbe19b9a257fc7cc2e5e6cdced2b4d063b33",
				MidState = "c7a498c65b253721d712aa746d14002c1809969c67a7b5162642f7249829f7d8",
				RestHeader = "5e2eccc72beddc6c333b064d",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 855,
				BlockHeader = "00cbab8000cbab805bf701b063169260122331b36018b679c73677c6eeaf2fa7d9509ac56b3d17e63ad8785b8ed35801437948648e0e58bce9830a0cccf73e092cc26a73351f476900cbab80",
				MidState = "d45c8b08d03696c2bfe8acbeaa954bcab9c121304067087621c3c4e113c564a9",
				RestHeader = "736ac22c69471f3580abcb00",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 856,
				BlockHeader = "31807ad631807ad6b153cc3b98788ab29960caa1c6434f82166a99a5fb6df33fb64750d45d89186a2c81c3b2def47f31128d6b6b8cfa94fd8d6e83b26e2724a3023ea74afea35ec231807ad6",
				MidState = "8a6677d1224ded86be7e5edac84afffd7c5f6c0e92c4ac6097b9bad0220cac17",
				RestHeader = "4aa73e02c25ea3fed67a8031",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 857,
				BlockHeader = "e7d4db82e7d4db8296de73e3d40645ace62b973d4cb42207ed8930ba35cfaf2a519d3cde224849fea19e8e08744ebb29220699299090b29351b994439bcccbdb1bcd3e14db49ffb7e7d4db82",
				MidState = "58a5e774eb3b9d89732604d5bb7265f207eaaa458c8fc577d13545dcd13f8edf",
				RestHeader = "143ecd1bb7ff49db82dbd4e7",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 858,
				BlockHeader = "35e8680935e86809ff7e7c1ed014b6c8f28d70743034d5584524f611a88bbb66544b39d91657ad2e272d727bf495ad9c680fd695a07b09df4dbc2b874fec70c454c0645390bf77f335e86809",
				MidState = "358f369d68175fc479051143be3807273f7fd454b0b80b192378ccb000d557e8",
				RestHeader = "5364c054f377bf900968e835",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 859,
				BlockHeader = "5312a3ed5312a3eddb20adbd689476026ced85a2eb1eea86b8a509702962d4842bf6995bfdcba778eb2af18d1fa9bdf2cde3c89fee084e3081b45e5fbbf983f45ecae917f08a9b015312a3ed",
				MidState = "36f9e99c9bcf69f3d40f7139f2c5474509615e860414dd080e02c915e981cdfb",
				RestHeader = "17e9ca5e019b8af0eda31253",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 860,
				BlockHeader = "1ae1e2bb1ae1e2bb155418b318bff17f677d2c177ebe87b725c35a0865a0649661dfa3dbed0f22d967c3ea1a56170760241576edb17f39759229b4a60fc0dcef43c36c4cee2de5e81ae1e2bb",
				MidState = "1fb98272d695849e3ba4106b683f9f3d71b05c22dd9abe847e3bd111d62aaf8f",
				RestHeader = "4c6cc343e8e52deebbe2e11a",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 861,
				BlockHeader = "b50afccab50afccae7932a2b0edbd3b78042de8545bdee5ad5fae7b54c177d0e653c9dd22dda47927705bb5c95cad14832e52582be9a039fc887d989d97a3010b9a35a72cd75fb42b50afcca",
				MidState = "72c50cfcd9392e321bee4fb9bf622779497892ff3bfe2b8cc8c0a831b39fe1bd",
				RestHeader = "725aa3b942fb75cdcafc0ab5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 862,
				BlockHeader = "ca1b729aca1b729aa32a52b6061fb9705f29e496709c168d7a783948d46731ec69f9149ed44fac6ccf5db67d8918c47e57084033529139784245efbf33367c1269c6d87983070f55ca1b729a",
				MidState = "6f20ec50ba634e0c5352dae367f7dd2699e7914217c9f30e62a7f912269e2f46",
				RestHeader = "79d8c669550f07839a721bca",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 863,
				BlockHeader = "db926407db9264078cf49be771db210f907913e604bd9aa731730bcbf8bcbaed14d469f26337f308c3cff3c9cf4372fd999116970a4cb73e2cf3f0d52457e9a6ddbda141b3ca875ddb926407",
				MidState = "7290bac8166cabb22ea2ead9ad7072c09dc64e43533d8956b5daa884dce4438c",
				RestHeader = "41a1bddd5d87cab3076492db",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 864,
				BlockHeader = "529921c3529921c3014936684a5fceeac2f1c92c14306ae507b5ba96091039c2e14f76f2f80108e8cb500235eda7b0edbf549aaeb7a6267b2889a50a9fd426a92bc12c79fe29673a529921c3",
				MidState = "22de1455dbd7a5d74e6e1e902554a1397b77bc3208e060beb070cc9b0a1a94ad",
				RestHeader = "792cc12b3a6729fec3219952",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 865,
				BlockHeader = "a0adae4ba0adae4b6ae83fa4466c3f06ce54a263f8af1d365f5080ed7ccc45fee4fe72eed3cc4d60c1f4c6d2403a732a569c2b125869ddfecbc7c7d77e88f855d179962a56f83913a0adae4b",
				MidState = "f269a9875ecc3096ef7f60cba52d8ea40199fc22b75b77133ec3f8094c9b68a5",
				RestHeader = "2a9679d11339f8564baeada0",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 866,
				BlockHeader = "24ab145724ab1457149f349c5f4bb350f90612ad8b66530ada40efb915feef3418de50be26247f62609000c491cc914f12e15763b451cd01f85c390f0e5b55623821ee4c4266735024ab1457",
				MidState = "420ed9d4fbfb89123db03eb33c58dc004f7a7b455af4ddbfb4674d1617aade93",
				RestHeader = "4cee2138507366425714ab24",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 867,
				BlockHeader = "33bfdddd33bfddddfc743c8fca4ba95a7ba2f7607246502a649b2efd212bbfd56c3ff3c3161a5fac6c7c9b1e1b7d0f0f65bedfc7091bb3a689b64ae2c5888bb681a96d1ef2e2518e33bfdddd",
				MidState = "d0bb3d014f15032474939904597e4b2dd55238a5bcf01f8b1c44d170885a885b",
				RestHeader = "1e6da9818e51e2f2ddddbf33",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 868,
				BlockHeader = "81d36a6581d36a65651346cbc5591a778704d09756c5037bbc36f45394e7cb116eedf0bea0908fefd76c98cc33d366d4e379d7203d50c23871ce87e9c0f1e45c697e53286b94033c81d36a65",
				MidState = "8db04b980d743978159c04fbf69d4919f8d0a5e140ddb092336568c572f75c99",
				RestHeader = "28537e693c03946b656ad381",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 869,
				BlockHeader = "3316770c3316770ccf4e7912dddc33e2f4a110b2799086d6dbca8a62d1a89984641d4ca8c1ffc17233fb5dfed881f0c48da7bc7015e8a89c46614c0804b245473adef962399276ff3316770c",
				MidState = "31e61234ce558d980475751953ec8d5f2bc31fa52084da07f27c4d43da1b426b",
				RestHeader = "62f9de3aff7692390c771633",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 870,
				BlockHeader = "0732b67b0732b67bb2d7209a9b917b95581c28bc08da95a4e288a73ad0375c5d3a77d1321b22a1f10c97663fbe1d8453f0541d2b4f860f1741a5f3671e16b7298b61ea43df70c8900732b67b",
				MidState = "1e11f487ba3748afffbde39fe5aec56fde483aec274ecc49821b23bfc4788597",
				RestHeader = "43ea618b90c870df7bb63207",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 871,
				BlockHeader = "55464302554643021b762ad5969fecb1647e01f3eb5a49f53a246e9143f368993c26cd2eee86cbca9134a19f6dfa3139fe80b297ae012dfd78fc65ba31a457fca09a5418f9a2d92655464302",
				MidState = "7ba6019ee505c28d45b830a521232e77dd2a862717a125335796d35d2da4bc88",
				RestHeader = "18549aa026d9a2f902434655",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 872,
				BlockHeader = "f06f5d11f06f5d11edb53c4d8dbbcee97d42b361b259af98ea5afb3e296b81114182c7257c1dfa3969794f327036c9e2bf6e911dd141f7047d94d3ee27785ef530b9e1755d376606f06f5d11",
				MidState = "b813960c92e3425f2b7ceb67e696cdbefd20241000c5ae7c7b67bef166ac8d50",
				RestHeader = "75e1b9300666375d115d6ff0",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 873,
				BlockHeader = "cd789122cd7891224a38ef75d3255148c119b00e6a4f5847653f3632d65457a69e795acfddf1846b07b18f12014b9b1418ba587e832b0a276e92d7c610e8f37c247b0d270d0a60a7cd789122",
				MidState = "f74189f9277ad53ea4580bcc4dbb783786f8dc81b6e63b273f21589741d2dde2",
				RestHeader = "270d7b24a7600a0d229178cd",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 874,
				BlockHeader = "8ff1b0b38ff1b0b3c3a58cac9ea18e3e2bad1bf9afa2aaf04e97b223a21dbc38aef8dcc271e187d13c0a07b3b5b213b8ea169f132bc672a024fa3849cb621698414b9933930a4bf48ff1b0b3",
				MidState = "910366c9b82a02c8bff7f936716281eadda245b70a69f244b22b21c3c9bfd4e8",
				RestHeader = "33994b41f44b0a93b3b0f18f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 875,
				BlockHeader = "eb224b63eb224b63c742a1499663dba7e0f406be21937bf62d98156f4c09523df49c4bd04f3edc7632b664ec830ee624515dc456f7fe87cc85ad0529f0287fd43c54fc4d04af8f7ceb224b63",
				MidState = "356ade4b75dc5f765d0b0d1bc56a3c60ce1f28ea4b41f628ecf56948f4d7efc0",
				RestHeader = "4dfc543c7c8faf04634b22eb",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 876,
				BlockHeader = "4c5f13eb4c5f13ebd04675c792dccab986726496211f60816d6b4f64f50ab57b0776992e0e36cd470861dc4c9bd077b266d3143a1218adb1aaf8ba848695afebcc4b0738257224df4c5f13eb",
				MidState = "a589dcde2f93df382c917d81fb839c8998776b09d5e1fc426ef64f5d5990b2cb",
				RestHeader = "38074bccdf247225eb135f4c",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 877,
				BlockHeader = "94caa1cf94caa1cf05f19ef02031dd7fbec3db66103d0956136bfc954b0213f68a4c3d72293cf9d5b621109615c5579a0918670b601c09e0a97631512eb1312333e44f04c56d421594caa1cf",
				MidState = "6267621abad1e06c541b730310c02251b2f3c9cb56f8ecf478365bb4941420bb",
				RestHeader = "044fe43315426dc5cfa1ca94",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 878,
				BlockHeader = "fe145bd4fe145bd4a2ff1f5f8a5c3a45c0b8b64e6cfb5eeae9956f5610d62a7953a86c71ff0cf646834be2e74e0526c0535b1b6046d99405434cc9a2955c100d60c1a33fd7c90690fe145bd4",
				MidState = "e8a8ac0af59358022629774f9eafcf7e5c890a6e6a7b78000993867ed07761b8",
				RestHeader = "3fa3c1609006c9d7d45b14fe",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 879,
				BlockHeader = "284981c9284981c9ab8039ed7be02369485c089c6c880f7cc29becbca796d1a0186bee4def2a455715cd719d70fbf892f0b4e58a6e891ed41de10627dccb2d83f034e45d771ae866284981c9",
				MidState = "32650255e216f0bf979b6536a6302885a2e015517897f615e15e369a416420fe",
				RestHeader = "5de434f066e81a77c9814928",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 880,
				BlockHeader = "aa4de99baa4de99baa77d6ece210f835910943ff19ee3a3eb85a6915f4c062b47c10ec80f56490c6a481e791fcf9e51caaac3dcafe35ce93b90b902176973c83034a1c398190f573aa4de99b",
				MidState = "f71649c7b3d22db9f68c26bd35ed178e390545290ddbfa6f93ee64e2ce4ec492",
				RestHeader = "391c4a0373f590819be94daa",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 881,
				BlockHeader = "f8617622f86176221316df28dd1d69519d6b1c36fc6ded9010f52f6c677c6ff07ebee97b7ee1b545064a1deb0293cb97ee52eecd02b7aeef46aad0d412e0679600664c6312c4e8fef8617622",
				MidState = "9660922fab9d87ad4728749ff91c7a4b001f7d2a7a68820839e8d56098cea856",
				RestHeader = "634c6600fee8c412227661f8",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 882,
				BlockHeader = "217376d0217376d0456303a2c066ee990f3faeac91b7909805e6f9bffaca24e22fdca375941fa4c4a6e607e74cccfa3b7ab3128ddd69faeb9f266c63a42199ff5496c07d4a889683217376d0",
				MidState = "4d767d432cc86b3a14cd1b4f4a81aa2a68cd654c5cb28f86685d76ce08c0bf46",
				RestHeader = "7dc096548396884ad0767321",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 883,
				BlockHeader = "b4882b1eb4882b1e94bdb65ccc08616ecc2ac62c28121ad50ca8481a893d414d36bc819f191204e20aedb44d08b639017746bb13960a540fc1cc48a3c352b4a510386961a5e892c1b4882b1e",
				MidState = "896a031dd720c110b80b51ec7d76fd1300d4f2de9c5ecaaa18e48f713d4a70a9",
				RestHeader = "61693810c192e8a51e2b88b4",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 884,
				BlockHeader = "d56aad0dd56aad0dadbc7282b346c5c15fdf35b4989e4e02a99ee0dd07d4a932ac9929483996d0bc340ce95a4f69eea52b0129737e345b758fc041ce074fad8dc9357b06a12781e7d56aad0d",
				MidState = "807be4f309ef7885a6949e6eb9bfc74c0f23134fe8dfe8eddc5650490dc26ccb",
				RestHeader = "067b35c9e78127a10dad6ad5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 885,
				BlockHeader = "ee2318fdee2318fdb1be33a96ceab8069e48346c5a63f88c47f7994f1a205279871249d12cc05a32e55dc97ad283f1a21d8cf4d10f2797a5a718e62290c8b3b668835238aa48e14fee2318fd",
				MidState = "ebb3f8c56a62796bb8e81f44953274ca8fd9b4581e8d2bb5e6b359ad9923a963",
				RestHeader = "385283684fe148aafd1823ee",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 886,
				BlockHeader = "894c320c894c320c83fd462162069a3fb70de5da21635f2ff72e26fd01986bf18c6e43c83ddd842a62892b12d0d15e6eb66a819149ae6f4594d2496730e0bded44eea1454f314b25894c320c",
				MidState = "c17a4f313d6b59f5abcb24138c285f12fe7d879e45fcbefdd0b0066763f9c0ce",
				RestHeader = "45a1ee44254b314f0c324c89",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 887,
				BlockHeader = "d760bf93d760bf93ec9c4f5d5d140b5bc36fbe1105e212814fc9ec537454772d8e1d40c3fb99eeb6ebd803d146b91a686ad86f4b0dcd6878eac1a58c244b7e44387dfa057fa35ddfd760bf93",
				MidState = "685613cf18465197b1d0a7f41c6b8d535971b768ee06ca2f9744e1deb290d05d",
				RestHeader = "05fa7d38df5da37f93bf60d7",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 888,
				BlockHeader = "08927ab908927ab994ce5ad4285815283a01e1d1d7165ed5fbdccdcb38a3ac6c3d302c5f895222fd133ddd5633c1a9712be5f22bf1ecb8d84b30cb82839942f667df1686ebb8b4d708927ab9",
				MidState = "dae8907abc5a0865cdc43815e33a45743d797acd7ec24157703afaad6c079615",
				RestHeader = "8616df67d7b4b8ebb97a9208",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 889,
				BlockHeader = "01f99c1501f99c15499a19bcc8a35f32d936280ae6982226e5310112b1496551358431aac0d1de20d619c77340dc160c036825904d31f643f36e467b2af5c2eb9f5652ad5873e45f01f99c15",
				MidState = "0ed9a85e475f1422081c234def0a017cb18ae838935ac732dd8ecfdb293aa320",
				RestHeader = "ad52569f5fe47358159cf901",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 890,
				BlockHeader = "b8be66a3b8be66a3d156e814ce438dbacdc3f846c2fbbd832ef9c9e0dbe557667f42012c7ec4d111e9ed48a888d543c3c781b9f2902e3ce1a22de92cc04f58a17738f68dc587cdadb8be66a3",
				MidState = "46f3119b07506f004fa8032b8c85d42c2935617ad7c7202a8f1c55222dd3453f",
				RestHeader = "8df63877adcd87c5a366beb8",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 891,
				BlockHeader = "06d3f32a06d3f32a3af6f150c951fed6d926d17da57b70d586948f374ea163a281f1fe2719581bc870ac6b0367e33c35436e8e23eb8240ef5a9217c79716d19c08d480ee32ab0fd606d3f32a",
				MidState = "bc7b422b094d1da5a5acd752bae51e4c3a2d0be17642b61d5d6becc7d2fad3ac",
				RestHeader = "ee80d408d60fab322af3d306",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 892,
				BlockHeader = "a1fc0d39a1fc0d390c3503c7bf6de00ff2ea83eb6d7ad77836ca1ce435197c1a854df71e2aa8576374763053dd07ca84629af69352fc80d2287b785d215fbfc678d25da9bb4713a3a1fc0d39",
				MidState = "fe318995b0cb54a77884a98a1733f8cf4cc0fdf8ffa1916e3f557e1d23d4db1b",
				RestHeader = "a95dd278a31347bb390dfca1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 893,
				BlockHeader = "743ef8ab743ef8ab84bf56311420941e7dd01d5082cd4b1922168f1a212dd5bdeeeda1e0b8c4982dc8a8282dd5c97809eccb2fad1974378cbe3d3f10cd0274924ec9e0fe734e9323743ef8ab",
				MidState = "940d23e3bca0e46bd37c392cc683ea9655251fed820a58236b14acdde67cf787",
				RestHeader = "fee0c94e23934e73abf83e74",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 894,
				BlockHeader = "c2538532c2538532ed5f5f6d0f2e053a8932f687654dfe6a7ab1557194e9e1f9f09b9edc0e2d5dd2f0ca07c840db38c9d0c41c807645fac2bed5746d20818851f6e0a3d987ac7acbc2538532",
				MidState = "75820c66d5633bbb9b60801c56460eaf8dc7cc5b2f16225e26aeca4947cd4585",
				RestHeader = "d9a3e0f6cb7aac87328553c2",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 895,
				BlockHeader = "be24d89fbe24d89f4e3c299f2a76001a8ae345546ca2743713fc384930b6354f45c43cd7e0ea4328cf223ad83a547b85bcbe7392fd6f8cac315853ce4bf6a761e06efa8c3400a7b9be24d89f",
				MidState = "8e10fec249af4bb261836099fe0b779994daf31bdb7fabc312995757d656a909",
				RestHeader = "8cfa6ee0b9a700349fd824be",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 896,
				BlockHeader = "ebe4001eebe4001e2967e0ad9acae29dc1c0539ecc5a5d5f13414621de54ba3f4daa01587a1ce59cfa790ced8e3357ded9d6a8b5f62753101241a9646de733033949ecc81c0aa3c2ebe4001e",
				MidState = "69a4880375eb6bde0ec749bd703828949747291a96e8683711c95d9ac3994a0a",
				RestHeader = "c8ec4939c2a30a1c1e00e4eb",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 897,
				BlockHeader = "38f88da638f88da69207e9e896d853bacd222cd5afd910b16bdd0d775110c67b4f59fd53611d4f6be241e3355b13a116425d537ffc3fcc2885e1ad97134d714c00eb40ed3504640338f88da6",
				MidState = "3da74c5f2e2988342b48e864c4810b9b9f39465073771ca09225fa0c9183b6d9",
				RestHeader = "ed40eb0003640435a68df838",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 898,
				BlockHeader = "8b2932f78b2932f746efdea1a3deada917b4f3cbe06685e87756824a423153ba759a0e3d32cf8c08732dcfd745985d4f54e51615ac33ff5cc85e8cda2eb48a3a1b989fd85f33df6e8b2932f7",
				MidState = "2f74d2defeec4696844f3ba086f0bbf3f3ec50164f4cc8a3dd3ee42fcc521683",
				RestHeader = "d89f981b6edf335ff732298b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 899,
				BlockHeader = "27524c0627524c06182ef0189afa8fe12f78a539a866ec8b278d10f828a96b317af60835420d3f5e5f0bd352eb8b0e80d1ee690939fb71a1eea798f0a3869de7ef1ba6b34e0077cb27524c06",
				MidState = "a0d9224b2519a8361a974c43e8b73768ab43fd250c942d35757ef5f83dce77e6",
				RestHeader = "b3a61befcb77004e064c5227",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 900,
				BlockHeader = "4df0fc694df0fc690ae9125441931d75eb8651945aa803394c66ead9b75f07ea85d7ecde88df78d0102ecaedcc7b6d2ee7df78ea9f6346fc7f8d6e193ebf07c2905e4680c81115524df0fc69",
				MidState = "9fddbafc03aa8f3a4540a854cbeb0227b481f5d8a27dc0cb942c3300a0f30355",
				RestHeader = "80465e90521511c869fcf04d",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 901,
				BlockHeader = "291095d6291095d6aacb23a73609947d67c7caab76b6007accd2a2e8da63a2d648ec70bec4f3a1b9b9278fa4743a0952a58fce9d4be48daa9bfa34ad74f91b6322ad70bff9a7d582291095d6",
				MidState = "eff17aeea4416f28f71fe981a9a5e9f235171de476f6d65bd1648eda54606288",
				RestHeader = "bf70ad2282d5a7f9d6951029",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 902,
				BlockHeader = "c5440b75c5440b7593a5008ce172c2a2c37ff7dd6fa478c7741690a28ad0c993dd82892f48c59e2e7c7c125847184e5f48d881b2b63871dd5c538b9e489b646be9e54f92d8637dd4c5440b75",
				MidState = "41afa5a910008bab39f5bc98f1d273652fde95da1b37219ef18f5f2f36816d4c",
				RestHeader = "924fe5e9d47d63d8750b44c5",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 903,
				BlockHeader = "0ebe18e60ebe18e6f9f5caffe53b88638ebcc063a95e0a0f60c4012d63bb100a7e3338fca4bbf763a684ad8b7d57ca67b70864a0e03cb46551b463f059f9249488fc90d30e00010e0ebe18e6",
				MidState = "b50147eaff79b9571e2ee33798443effbfa8018afd0b7258e24b83e0cf07029c",
				RestHeader = "d390fc880e01000ee618be0e",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 904,
				BlockHeader = "a16d4b33a16d4b33c640390c52759c926571bdc5a49302d34d83c1d201215f20076073a0a4da9c57ca5430f098271bea47c298564c52c6a6b4e435e38b4394f90772b9ae55f2c64ea16d4b33",
				MidState = "39115c0b9c262af9729d551549d39f86a3b38a840da3fe8ec8442ba1c00317ec",
				RestHeader = "aeb972074ec6f255334b6da1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 905,
				BlockHeader = "3b7c978e3b7c978ec7286155911096c3507285e51debd84a013f792ca8dc1d1d5631e2057608c52738af7f249b6b67589fd5406068dd24bdb9f9cb6b1563d1a0bc76e68c8a0547b53b7c978e",
				MidState = "800aac73e7eeae79a20b6c5990e7ff3df6842bc78c58f5bd32774a4cd0ca2a46",
				RestHeader = "8ce676bcb547058a8e977c3b",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 906,
				BlockHeader = "4601ca2b4601ca2b9c4fae14c4bac61ac0e8097a00c96cd1f9eb2eeae48ff3e6712e4b1781bed885f4ad4a67ba404d36b8e4ce22a078df76b9bcfa3091a1f30c699575db179594e04601ca2b",
				MidState = "6d36d3fe4ad8ecf4398e0e4019f5de03d177aebf65062f21688985b3bd1314a3",
				RestHeader = "db759569e09495172bca0146",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 907,
				BlockHeader = "e12ae43ae12ae43a6e8ec08cbbd6a952d8adbbe7c8c9d374a921bb97ca070c5e758a450e7f1f10356181e3179e292b8e8f58f17b3b35dfdff6ca99e71d0f348c3eb20fa4f90347bce12ae43a",
				MidState = "9eb0617079d49ead82ab4b02485fa21a308093a262229b7ede50f0205e1bc0a4",
				RestHeader = "a40fb23ebc4703f93ae42ae1",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 908,
				BlockHeader = "2f3e72c22f3e72c2d72dc9c8b6e41a6ee50f941eab4986c501bd81ee3dc3199a77384209d0464f766412103c9a53f424a0aff3494d2d1cac273e311607673fb6cdb9cfaa467f30aa2f3e72c2",
				MidState = "3fe03ba5fa50c44cb02bc5374aa85777bdbc17c003b6571b522805014e562232",
				RestHeader = "aacfb9cdaa307f46c2723e2f",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 909,
				BlockHeader = "5239c1655239c165c183701c53a466caa588edf56a7c4b1f9a2dee04fc18dbf8a2a07445e0bc8121e7d622698b17bbc972c8a1fca8f874f53ca8628a94ea41f3fa280acfae8db7f75239c165",
				MidState = "2b90d8f693cd53a7679563dc1a9dc9b2af67c6bc0d55a967ea057469c94867dd",
				RestHeader = "cf0a28faf7b78dae65c13952",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 910,
				BlockHeader = "dc9cfb48dc9cfb480084487fc9692acd1a6f1b72db8ad75b25746c6b9fcb4a863b930f90b9ea9acf27062ef30be8bbb45e9764d331284c095e6d77a4bbf36ff2d7f64098fe06f0b4dc9cfb48",
				MidState = "e3574c6cc7341beed2b14ebac33d0680c7845eecb5ad43c24416328ebbd43ebd",
				RestHeader = "9840f6d7b4f006fe48fb9cdc",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 911,
				BlockHeader = "b8bd94b5b8bd94b5a06559d1bedfa1d596b19489f797d49ca5e0237ac3cfe671fea8947108f1cc6a67f51e07e4cd76b48984ad0b18c5d2d8d958e704e77f635bbdba44bc933227c1b8bd94b5",
				MidState = "5d05a87795745d942b82ecbcae58f222572c3b4bdc1df3249f29db4558c1fff5",
				RestHeader = "bc44babdc1273293b594bdb8",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 912,
				BlockHeader = "f6841b37f6841b370ad303757d7268fca55c094ead89e0bd5c5310ca1e34c5b48a4e4f64f7fa15ed0405641ff7b714811c4bb82a1a8af6f05f923c31f89bd9062c6c3ad96e14e079f6841b37",
				MidState = "951957fa021b9f895c6afe83004ddeecc178fa23ef00b234c7f8c3d657432dce",
				RestHeader = "d93a6c2c79e0146e371b84f6",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 913,
				BlockHeader = "3c758b173c758b174c8bf7b9808a84c22f39fedee5612ab7fd513696d4b5d5f459f3275ad2dd571014f5065307e9794391433b0df99da2b2d6a345d2561e1e8e98c9db83264f30583c758b17",
				MidState = "f23b398030de99308f9d449a18172890bb09ee8c3a6154edf025a9b1367b94fd",
				RestHeader = "83dbc99858304f26178b753c",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 914,
				BlockHeader = "8989189f8989189fb52a00f47c98f5df3b9cd715c8e1dd0955ecfded4771e2305ba124550198f51f7e959acea9596c0674dc0e2550ed5ee0f1dd950dcf131af32106249ed5c2a7618989189f",
				MidState = "afef34cfacb36c26b2400f9578783eb3238f11b5cbadf99f1da50269eb9d1d0e",
				RestHeader = "9e24062161a7c2d59f188989",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 915,
				BlockHeader = "d79ea626d79ea6261eca093077a666fb48feb04cac61915aad87c343bb2dee6c5d4f2151733b460a9f476369e90fab11edaef1542d4fecdf29626cd8e214c3a18d06f2b5af804a03d79ea626",
				MidState = "d4de1c7aa76e1b6c733c482ff2361e9b43e70320b520be476be94bba9f92114c",
				RestHeader = "b5f2068d034a80af26a69ed7",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 916,
				BlockHeader = "84d0fc1c84d0fc1c2f22a45680446132d40642f0fa345752b9993bdbd1038cef362356986ab0714f62ac55670917fbda32c5163e93a4a70dbf3702b34b9e95ee7020eab8052f728584d0fc1c",
				MidState = "d180a3d683ab64aa72bd85fa83f56d2729615daa2b2de6701f50b72b0e7cbd13",
				RestHeader = "b8ea207085722f051cfcd084",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 917,
				BlockHeader = "52eb08b252eb08b2baedf3983ecd106b5849c748ec1b1540731c27dbf6c36eea03e19452d479f080856724ab6e2940db2ff46ad6af7868ef1687f4460732c17bc7b6f0962d8e289352eb08b2",
				MidState = "a4eda4fc0289ead853dadde2147cef5f71ff0fd59acfd8fa44e02ef430c59ba9",
				RestHeader = "96f0b6c793288e2db208eb52",
				Nonce = "80000001"
			},
			new SelfTestData () {
				Index = 918,
				BlockHeader = "9af056969af056964c8f4245effa1410e7c90bde9c2d1ef0de3c47c94f9b09a042e8fced5812797f925ec9748ee2d1e1e57a9cdbb3bccf5b8c3cc4986c2ca7526ea9291070adfa8a9af05696",
				MidState = "9fd20bf2578eb8044cb26791aef47575b7fe3752a37288e3eadd95da1f02f59f",
				RestHeader = "1029a96e8afaad709656f09a",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 919,
				BlockHeader = "e805e31de805e31db52e4b81ea08852cf32ce41580acd14136d80d1fc25715dc4496f9e8cb286ad33101cea17a4555d5e0bebda727277752c976fcd4580cddc50c16f96d4e06b405e805e31d",
				MidState = "7f7a32d06106bffdd8372992480d47dc6ec6dc5d0fb7ac97c2c1c0e33d4a309f",
				RestHeader = "6df9160c05b4064e1de305e8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 920,
				BlockHeader = "351971a5351971a51ece54bde516f648008ebd4c642c85938e73d476361321184644f6e46b3925e7a900ba69159c7544af2d64548cdec19b2a2bf09f50ffbeb510da185db77df2a5351971a5",
				MidState = "0cf770844dbd6195c2abd978363984b272ba07b7a9c5dd001ca51543981840dd",
				RestHeader = "5d18da10a5f27db7a5711935",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 921,
				BlockHeader = "c3e325f0c3e325f040e031abcebc73747fc977c796709c27318f8f0b368194cb6f238b2105043c8a98d0c9b544cd169083261f395af39960edac38ca37db88117688f305dc4b1eafc3e325f0",
				MidState = "08c8bcb903e379935236e6fb8841b5b1d3ec36ab70b7ce51d707e3f18595c3d1",
				RestHeader = "05f38876af1e4bdcf025e3c3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 922,
				BlockHeader = "11f7b27811f7b278a9803ae7c9c9e4918c2b50fe7aef5078892a5562aa3da00771d2871dadb8fff899db4ad60e865073a917c5d5b88162c78512c9a81246a6da4c40c326aae8c67611f7b278",
				MidState = "c4631e21f7e55f07bb512028acb91590333a0b8a0a0a9a85f577e9f28e349ebb",
				RestHeader = "26c3404c76c6e8aa78b2f711",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 923,
				BlockHeader = "3319d4593319d4590ead1b77b9aedaf7b75d2855ba2990dc14b2b1e412418d9bb14d84aff3a5d26e6f55a06a1a39aa1f776556db71120423352cf0d8aa6570e1ccc183673e7c736b3319d459",
				MidState = "4791bc06e0c56845eec2414cd094ec995d9ee2ceb5b0666cfe59ab4733151fd2",
				RestHeader = "6783c1cc6b737c3e59d41933",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 924,
				BlockHeader = "60f3409c60f3409c7ed1ab4895eaaf68cdfc95d2579e9f82b8cde5509a518d7f7e7afdafb22ccabf0c3cd02db08970d046da91c413faee4e1ce16fd25315dbd6b1091a5bc0878aea60f3409c",
				MidState = "0b73372363398775fe9e9583d69afb43cf63db0e79e93bd4c726f5a5135a1918",
				RestHeader = "5b1a09b1ea8a87c09c40f360",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 925,
				BlockHeader = "ae08cd24ae08cd24e771b58490f82084da5e6e093a1e52d31068aca70d0d9abb8028faab93c495695bf36a8fdbc9832e0782cd1b7e47473f8747b1186c662621f3f84d7c8d02ae63ae08cd24",
				MidState = "5a6f9c9ea835fb4f771339d8a6cc8f44b92e576e9f4b829e045f406c9f7bf4cc",
				RestHeader = "7c4df8f363ae028d24cd08ae",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 926,
				BlockHeader = "d3945623d39456239b8f14bffc9c28f78e587ee7b79b652192ca2c55b053d49c3e47b953c4ea0d0aecd1a9807e50ac654a27f1c964543de06317b8391fbe51821eb43805fb7adb00d3945623",
				MidState = "0f5c58dce94590834e7e0f402bffc663cee77efd16475675eec16188b74d8c94",
				RestHeader = "0538b41e00db7afb235694d3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 927,
				BlockHeader = "21a8e3aa21a8e3aa042f1dfaf7aa99139aba571e9a1a1872ea65f2ac230fe1d840f6b64e7908c3ba63364a8e4fe3b361b5148311af5c3d56c18805a87e80ae959a64b05339c90fb021a8e3aa",
				MidState = "4879802953c3f970ea0d54eb98d955f8c0a17324ab4604573193f0dd954f06cc",
				RestHeader = "53b0649ab00fc939aae3a821",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 928,
				BlockHeader = "6fbd70326fbd70326dce2736f3b80a2fa61d30557e9accc34201b90397cbed1442a4b34a62753edc230aee54aad5f8367a463fc4904e0549caf0dc183a199fd56c70e34035b224666fbd7032",
				MidState = "4c098f7cdbf55cee07c267b1c9a5d66575f9f77d8564608745013b7267794272",
				RestHeader = "40e3706c6624b2353270bd6f",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 929,
				BlockHeader = "bcd1fdb9bcd1fdb9d66e3072eec67b4cb37f098c621a7f159a9c7f590a87f9504452b045c20a93e2eb63fd38410fad69439201265050995a8d61f642a60249e8cce3076c26250d40bcd1fdb9",
				MidState = "86ba31ca8337040e74592a8c5699d22c4d594eb1dafa004b35926919b678f10e",
				RestHeader = "6c07e3cc400d2526b9fdd1bc",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 930,
				BlockHeader = "77b1047977b10479ca1cf4bfd85f300ab63815dc47e8230e742c1575f91aa6c85ad0242213e9634c4cb0ef82562e9eb221fee69b2d529c9b6a1e46482cc3d465d1911a2bd2f1a30477b10479",
				MidState = "54e45a7259ca05218c44d801ba4d0d4ea15ab0b80718049d100f15a1492c503a",
				RestHeader = "2b1a91d104a3f1d27904b177",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 931,
				BlockHeader = "d9818093d98180936e2df17b7d2892ebea458480c30624b1e5a85107df596ac5d7e5ad4679de5f413a9c5406390e28a6ece44f98e6d79afeb973648f7fc44dc63488bf702b8c4ba6d9818093",
				MidState = "304f6fc5e65703cef79c02a38951b3b801c7c804a962eee6835cc934e5f9fae9",
				RestHeader = "70bf8834a64b8c2b938081d9",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 932,
				BlockHeader = "ff5f14c6ff5f14c60bff928871c83651471dfc6dd3d9733a795041cdcd865d796c359484fca4e5a6a7f9f18ef4c8b598af1d84e67e510c1fcc21bfe4984daecad913406d8154d742ff5f14c6",
				MidState = "f54e6cc1a58cbf5b63ce4c3f788ee5f4e036e8b98f9731f38d2a227dbdaf014d",
				RestHeader = "6d4013d942d75481c6145fff",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 933,
				BlockHeader = "2614885926148859e188587c21895b22bd5f2858784894efe1e33eba09329a56868552c4d0a5d993af292169ab3b7f602bf0907224c7b7d7aa98055f6ecaeea03cf7f1134ed1562926148859",
				MidState = "09e0af3be432f674a4b3584d0fd24a53d721a65c039af6aa6e760faa713a2957",
				RestHeader = "13f1f73c2956d14e59881426",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 934,
				BlockHeader = "7eeeed8f7eeeed8f0229f551da5001b2b17766d33bec3992cff5230fdfcfc22f3e55f6b80d266f6847e8dd2d47a16c4b0c5fcffebaa5e0a22ac44743df29ef0789da4c450611099a7eeeed8f",
				MidState = "0b1d814769ec4d67d67ac2ddeb828bb71b9e7ef928e34cbfc89f33ba366f6511",
				RestHeader = "454cda899a0911068fedee7e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 935,
				BlockHeader = "cc027b16cc027b166bc9fe8dd55e72cebdd93f0a1e6bede32790e965528bce6b4003f3b4fbc3b6bb0d3085d6a9e5e9d32d2d97bf5902037d97459c7d962626a041639859031e0f76cc027b16",
				MidState = "94a30f6ce7428a7a234ee8394cd532497abf71eca4e792efdafd63211b147789",
				RestHeader = "59986341760f1e03167b02cc",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 936,
				BlockHeader = "a8231384a82313840baa0fdfcad4ead6391ab7213a79ea24a8fba074768f6a56031878946166b0d3f714b0355530c101b6d65ab10ced788717fd91ceb87b258db936d6650e5c5487a8231384",
				MidState = "6a6e2d35d5b24dc2377130abc48eb404fa38a1446045e1e20dcf5df55f0f0422",
				RestHeader = "65d636b987545c0e841323a8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 937,
				BlockHeader = "f537a10cf537a10c744a191bc6e25bf2467d90581ef99d75009667cbe94b769105c675906796d15f51415c44be23c945b835aca446f30eb8376cbaff204811d9043ce3656224b65bf537a10c",
				MidState = "f070dd6173638416e1e337228b60351f96b5f5ca0817474fd70c9c68a3ed859d",
				RestHeader = "65e33c045bb624620ca137f5",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 938,
				BlockHeader = "49cc729049cc7290faa5eda16ae8fa35ab3368fbe8bf1cb7ba469a22bbbe21497a640c136909e318c56ca037320daa6c90940c044584a7f5ecb89e91594358af317db92fc86e458449cc7290",
				MidState = "cb487eb1c58bb93629ff98c6b01e73f479344394b8a513927d58f6258cee294d",
				RestHeader = "2fb97d3184456ec89072cc49",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 939,
				BlockHeader = "59b07faa59b07faa750239c5c2d6608d94191f9ba7d5e55c794c1eb354e4fbc79319d085a8c0d117025750119f52ca25f5fa79515dc1f41eb47ce8015845fcc6b2a48a46dfdd3d1a59b07faa",
				MidState = "9488c7ded8b8e38cf14376df028fc001f5d47918cd64726b58b7686a1c8a3e52",
				RestHeader = "468aa4b21a3ddddfaa7fb059",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 940,
				BlockHeader = "2f8902ca2f8902ca547bef6a9c303ad15020e537d9d8b785741628fac76b352065e4d49c6a9334c57903d9e8514c847db2ef14b735217680790c7d40e95828fe1f1b6e07992b750e2f8902ca",
				MidState = "81629b986b2006aed2ac031c9a324bb88e53950bd446f8edeb8f042014152912",
				RestHeader = "076e1b1f0e752b99ca02892f",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 941,
				BlockHeader = "a9d34f8ba9d34f8b60c6f0280e92759d220a36b4c04d27df3056950775e2735fcc0b2b37235034a5b61c136ee5246b7e92784d12ff9f7913d246e3f130dcd02fb657621a86a91151a9d34f8b",
				MidState = "a673fd010bdde0007711fb2877db402cedc893e8938fd80a9eb4b49ed761cedd",
				RestHeader = "1a6257b65111a9868b4fd3a9",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 942,
				BlockHeader = "ae2024c0ae2024c018879875349def22f8f1538735241c40b08bff36664ee9852c4a1eb87b24c0dc5f606633ae6c9db4bf848e17a46d8fe97afcce0e074a49aca7da052d4f658361ae2024c0",
				MidState = "e5cd6ec2f1f36a7b98ba7f0ff9dac1f0a1ee52b63541c0bc3d017cf2a0b4879a",
				RestHeader = "2d05daa76183654fc02420ae",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 943,
				BlockHeader = "49483ed049483ed0eac6aaec2ab9d15b11b505f4fd2383e360c18ce34cc602fd31a717af197a0eb1dddf7a3c7e5e5e69f010c6bead8be973e6064d29e0a2c38f81b42666834a608249483ed0",
				MidState = "3dc97e285ed12a95bf66f524067c4576dc51d5328bf6c13dafd8e81158ae319a",
				RestHeader = "6626b48182604a83d03e4849",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 944,
				BlockHeader = "975dcb57975dcb575366b32825c742771d18de2be0a33635b85c523abf820e38335514abcddb9b99b8aa1cb7b0fe091836e7c1cae3595f1ee231f55b01380dbfe836ed55a0e0140c975dcb57",
				MidState = "7921c9414412c02dd916e44fbda0d0f5e1d51446980d009aa879a6bd5d975716",
				RestHeader = "55ed36e80c14e0a057cb5d97",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 945,
				BlockHeader = "e6fcc8cee6fcc8cea3ebe3343770422092c09a549924e60a4695bd7a4578c40cafaa7d604676a4de6d3e2b15e673f99cac3c9997afda3e6951b3932c54507f322a81bf04bb04b4b7e6fcc8ce",
				MidState = "ec674a0b4743316cfe7d1a4b81f8f23ca1e0d9a478b8f650b51ac3583868687b",
				RestHeader = "04bf812ab7b404bbcec8fce6",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 946,
				BlockHeader = "d19634d9d19634d9aceb715ff22a29e14838838765e526d0a33f7cc3d89179c865356fbb22f0804df4c1b57bf1bb5383e05f183e7a9bb1a75d0352f54a05a4f534584a63842306b5d19634d9",
				MidState = "d693056ce28e84f0c6788745fe5a76a104c74d81d322fe243514cbf79c3c501c",
				RestHeader = "634a5834b5062384d93496d1",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 947,
				BlockHeader = "edf8d6c5edf8d6c5563f278baf8eae73ef72dba94d737e56bbafec39ebe5e9c3d739e8aacdc9d6bf1110dbca938005f6b33d60343e76af8dc4a174d760689f08e0b75255a7b99d0fedf8d6c5",
				MidState = "e47f64ab7248af073eb08b277a5909d420b62bcdfdbad385a2a7dadd3bb95e62",
				RestHeader = "5552b7e00f9db9a7c5d6f8ed",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 948,
				BlockHeader = "e93b3067e93b30677cbcf2aeaa0e9f3c977d0c20386a4ec26cace35b4ca330c96295d39aa492b10ac5c75917a523d70066ae25d1c79b0778e9bc644766886082895af2137b4c6616e93b3067",
				MidState = "fcd4153b7acadc0ac4b774cb269ae687df8456d95ce73b2f6ea3c82c81e13746",
				RestHeader = "13f25a8916664c7b67303be9",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 949,
				BlockHeader = "e6f1d0d0e6f1d0d0f6417a9933e438ef82c1b9f3a9ed2362330017a92032d8b1ea11e4df2b8eed3918fa3c6c8079ebc038d7e0993d97270e4b1094a2df2270d03939243608454e91e6f1d0d0",
				MidState = "94466cc86813150d962b622e59642da5337632d715c420499baf86b2079c9704",
				RestHeader = "36243939914e4508d0d0f1e6",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 950,
				BlockHeader = "dd32f24ddd32f24df3824de598f4bd80dd2490a90a8920df1529b4afdef8d9c175ecb4b5b6a579dc54e6cff431f3b45b58e6a9e0931c1ccd2d368828e1f23cafc1a6ff0d0aa41ea7dd32f24d",
				MidState = "d7c9c194b508135c424e54a89abd14ee0d71fec7eb06ae1bc079894615c8d39e",
				RestHeader = "0dffa6c1a71ea40a4df232dd",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 951,
				BlockHeader = "401287fa401287fa1a28269c341383f8384369fc73fb1aacf0c570e753b8e027a1c3dd95122fb161016563d33668750ae2b2159efe0f2465411be61e29395bdaa5075e27653aa2a2401287fa",
				MidState = "8da07e8757fc20e0545ccc934242e2cb57feaf57432e1a3cb3d7c32ea5c9de57",
				RestHeader = "275e07a5a2a23a65fa871240",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 952,
				BlockHeader = "8e2714818e27148183c730d82f21f41444a64233577bcefd4860363ec674ec63a371da913a06179737a081d594e6beba3cf65819cc142b947cc1d53b97892425c8b9426f5daa273e8e271481",
				MidState = "f15f3e54d8629abe659f79e0add410ade0e31b6cd1a05d3b339fefb561fb2386",
				RestHeader = "6f42b9c83e27aa5d8114278e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 953,
				BlockHeader = "294f2e90294f2e905506424f263dd64c5d6af4a11e7a35a0f896c3ebadeb05dba7cdd488ffd0f3eaf035db31fba0c8628086749a82e1c169d1cb04657b8c93b6cafe950793c731cd294f2e90",
				MidState = "9f4865d649fdb9989061f1a64411455ebfa78161a9dfd1dfc1299fbb12b7a57e",
				RestHeader = "0795fecacd31c793902e4f29",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 954,
				BlockHeader = "2efd52152efd52152e6436521d045fc9b401a865fcf0f8b08154df2bfe126fc107b309c569cf40116ad20b7d608c49e0ec41de51e02ec4830877aeeb557ab448b2c47c2191d2103e2efd5215",
				MidState = "a5abc2d8d288eeca32cfaaf123a5a057058c6741b40ed3e458b35f6ef75a4b9d",
				RestHeader = "217cc4b23e10d2911552fd2e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 955,
				BlockHeader = "ca266c24ca266c2400a348ca14204101cdc65ad3c3ef5f53318a6cd8e48a88390b0f03bcf799c112a97810c4ae203fd7657a924e54d3a83c1666e4670223dcd507667d196b07fc37ca266c24",
				MidState = "5608203b0ab84a5e350d113974d710a86712987368e63978fb176948fb7f2250",
				RestHeader = "197d660737fc076b246c26ca",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 956,
				BlockHeader = "173bf9ac173bf9ac694351060f2eb21ed928330aa76f12a48925322f584694750dbe00b70eb90c674805d6b77c08cfb8f8d9fad2972f7f25dedf71cc3698f0e006c36166e1c81175173bf9ac",
				MidState = "c747a7755979cc3d9d9a840c7e6383a5cf2f54046863252ec35343aa50cd5394",
				RestHeader = "6661c3067511c8e1acf93b17",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 957,
				BlockHeader = "b4d76578b4d765780e6f261a477e85b8998777d25931d0ef6b4761141910c892de56f726235d42a271278624384e458134ca029f0b3d91bb5652e58f7696b92d3a30f20192643f02b4d76578",
				MidState = "b8e4b5c2208032405756df1f6af41e2b7f5c25a3c498c56e59c58c45844c3856",
				RestHeader = "01f2303a023f64927865d7b4",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 958,
				BlockHeader = "01ecf2ff01ecf2ff770e2f56438cf6d4a5e950093cb18341c3e2286b8cccd4cee004f421bb2fd72026000d912c950f9834371cdee7b5e5d29b5d7136a268be62c81eff28ca6c013101ecf2ff",
				MidState = "0d6347631a6756e8c8c8eb15e4a55bf8c4b13838da82513a840c08f9ee2d1783",
				RestHeader = "28ff1ec831016ccafff2ec01",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 959,
				BlockHeader = "dd0d8b6d4f007f87e0ae39923e9a68f0b14c2940203137921b7defc2ff88e00ae2b3f11d4c3fa465b889ff6a93c269b533147fbdac48127a78e36cac1f56ec69c87df833fbc702154f007f87",
				MidState = "aea969dacb5e19bd95f68912fec3f3876e1933819b869c7747d447cbb8b951c6",
				RestHeader = "33f87dc81502c7fb877f004f",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 960,
				BlockHeader = "9c94874c9c94874c53089f93e2fb30278466cd17d573a6d017b8dc752960109a9153969ae5ae9accb6d9dec65bdeb317be80dcf2685d2fe4596c50c3b604fa5e06053a6ded16af1c9c94874c",
				MidState = "88a3986549dcb2601b2a928fdd7332c27d5f9fbf47dc4ba9b773d024bec18f8c",
				RestHeader = "6d3a05061caf16ed4c87949c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 961,
				BlockHeader = "46aa8d1f46aa8d1fa5bb3a4c42990e953f807af9a5d20763a67006b2df8f15262de0b84314dba74ece09d52e18c9be86ca725677ccdc22757545f8daa234b27daf3e8210c60831d446aa8d1f",
				MidState = "287e3a0e829f84b480c1f3bf7a8dd9394204c3ec829f774b3a0c9f6923d46248",
				RestHeader = "10823eafd43108c61f8daa46",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 962,
				BlockHeader = "e1d3a82ee1d3a82e78fa4cc439b5f0ce58442c676dd26e0656a79360c6072e9e313db23a93d09d18a84718d16d7299591204c71e8ddfec56257f4ba39303d1fb10c1577f73e5e945e1d3a82e",
				MidState = "999351a122f353e242320a3f9632c6e10ebe96e73d0aec4db3c04a118c5a6d7f",
				RestHeader = "7f57c11045e9e5732ea8d3e1",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 963,
				BlockHeader = "071b0560071b05608c6faf559288eaa483639813d244cc4037c60883570a8116d597d5a665e7adda91a91b9d98c2647d0be810e88c6ca5e608be385ddbaa8aa6a794f80cc3ff63cf071b0560",
				MidState = "7425b3bc31fea79bd37ec6ac9749b4d4e8fcd7ec81924b8a9c1cf358341e64df",
				RestHeader = "0cf894a7cf63ffc360051b07",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 964,
				BlockHeader = "f22e84eaf22e84ea9bd92dee2825799b73c098c7f8d1e3bbe0b39ae9f8d4cbb69637e5545c677fcadbcf2ec3dcb52fae142ec1e5b48f2ee1d2174940874b3ed88bfbc775ed94f124f22e84ea",
				MidState = "ba2cc28a0f3745ceac5154d4b42b99f08efcfe54c3c8b0e1702db7ba2dce411f",
				RestHeader = "75c7fb8b24f194edea842ef2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 965,
				BlockHeader = "bd1eff62bd1eff62c5c09650735609112e057f59c292fb65d8d29c16e5d998f2d1c0df48a3511684320d19c7863660ee76b22d43e08406c7a3cf15561844fc0b8be61b4c65cd3e24bd1eff62",
				MidState = "5dde7712a5021ad3653be6967254405b03e0b47d736415a275cdbd781f6e3996",
				RestHeader = "4c1be68b243ecd6562ff1ebd",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 966,
				BlockHeader = "59471a7159471a7197ffa8c86a72eb4947c931c78a916208880829c3cb51b06ad51cd83fd4c2a6a34f57f3c0a782ccee0621e2043bd692e2face8528e29a5c371440f865ee53565059471a71",
				MidState = "a21c31ccd970e3636b217f9db3ad748650164ae375368ba1bfe5e45b8df5c389",
				RestHeader = "65f84014505653ee711a4759",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 967,
				BlockHeader = "11556e1f11556e1f5c8b1d1e39a6918b02d21965dc66c10e989f8197a8b1f31b101210e69711d2040bb345d5381430aa410712276f3ab7cc464948a5c1f8417964060a2f357aae2e11556e1f",
				MidState = "60ec1bc2486fd6affadefff7bd39ba5f864a2375da84b5016e24b71606bc49fc",
				RestHeader = "2f0a06642eae7a351f6e5511",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 968,
				BlockHeader = "5f69fba65f69fba6c52b275a34b402a80e34f29cbfe6755ff03b48ed1b6d005712c00de17cb6dac2df5cbaad87874c63fb06d7ab31fa3b61d5b3045e635ae3bf6b2dc17a548cb1665f69fba6",
				MidState = "bfa45bf7506192c551fbb358faa093c2c0dfa0e22141215bc6e3c5618850dcb6",
				RestHeader = "7ac12d6b66b18c54a6fb695f",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 969,
				BlockHeader = "1b6d25611b6d2561c08d412b96454784ba4bf5b42200a3d12c4968d7454a39ec4500745635bf70d1e6415d90964334f6a4bd97e5bdbf18f63ae3dd23c2604f8cc8b01545fb510d8b1b6d2561",
				MidState = "4d1a96a44608fb39c6a35962964402af3f30911ae290a5c24f2bdf6fb513ad20",
				RestHeader = "4515b0c88b0d51fb61256d1b",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 970,
				BlockHeader = "44a24b5644a24b56c90e5bb986c930a843ef4702228d54630550e53ddb09e0130ac3f5325ebff1ac91021bb504a0b8d8049d04735a7c0c3e5c7eaffcb95a3ba2a7ee4072b303d90444a24b56",
				MidState = "237160ef826f2ec21a55b3ea0d06b2ef4fdedc76f23a98295e9795783a1ed61c",
				RestHeader = "7240eea704d903b3564ba244",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 971,
				BlockHeader = "04aaccf804aaccf8fb6c5ddf876f9ad8df728059cd7fbec5341bbbdb9f7d5ea04b0b6b48fca63088730ad1ee6364c2aa5b02648e93334ff2b38af7659e838bb66259a34ce1fbc34904aaccf8",
				MidState = "26bc0d7d86db8d87f3d74c3754675bafaf9911213f12a73a20c0e0619dd74a05",
				RestHeader = "4ca3596249c3fbe1f8ccaa04",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 972,
				BlockHeader = "17781f6717781f677e854bc1848fb9bd83b12127f8e305896a27cfa81d754f51fe6d041fff0c5a6cd5d81f3d9e4aa9efd6e996c9c603f2d3152acf24ac22fbe483324b037ce861e817781f67",
				MidState = "e20f433c1090db8a7f96e297acbce586af9bc0acbac8c5aa01dfaf88ea4cea79",
				RestHeader = "034b3283e861e87c671f7817",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 973,
				BlockHeader = "698a9bae698a9baed6ae697e82e491a0bbc82eda91b36d4e650c6ffd32998e6cc9d09fb8cce3cf7b66a1bc4f417aab19e0b633e7c7a1a4e148243331fd98bbd0371fca62c9a9ca59698a9bae",
				MidState = "972978d01c9553d71847c7a682badc8315dac4564be563d188320a992945624c",
				RestHeader = "62ca1f3759caa9c9ae9b8a69",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 974,
				BlockHeader = "046ac4a4046ac4a4bc0d0fa72f4eabbaa038e0e083f9200072e427f271daafb46cdcc6bd11100ccfdd2872a7adf94fb64ca5aab038f77bfd4d7576cef20672bb0c144e059d60db76046ac4a4",
				MidState = "af8a58d1b559b015cc1df198c994981a772053f4ab76dd2f1d7f5a97573c4b5e",
				RestHeader = "054e140c76db609da4c46a04",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 975,
				BlockHeader = "2d9fea992d9fea99c58e29361fd294de28dc322e8386d0914aeba558089956db319f479916a1fe10f5387ce4af6df62f637b5ec8c90ee77c60997e2b0c8d77bdbc8b377804128b072d9fea99",
				MidState = "7f8104d8b2bb89303bd02af73377a4da57edf68552a4594b7f5fc148d3301a9c",
				RestHeader = "78378bbc078b120499ea9f2d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 976,
				BlockHeader = "ed68654eed68654efdac41a197b41d98bf0919b000823e74af957184ee2ea8253930fdf8154a5cea01b56d89a80137ad6e995efc1fd79ff4ce897bff429a50967514bd67cd7a1c0fed68654e",
				MidState = "01101dde3fb20bea75c97bf76c6913a858712a821ef6558db94ba2c81327a499",
				RestHeader = "67bd14750f1c7acd4e6568ed",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 977,
				BlockHeader = "30934f2830934f2860cac3e984b694493782fae3d908288fe1137574cfdcedd613b55b9ec1d5f1fd89f43c67860bffdff9748f1f53bb4290ae1409683a6a29a9da1c8e7149e30a2730934f28",
				MidState = "10da49895326d2de6b80454002951c19a7094dd41fc9492496270fbf53926a6a",
				RestHeader = "718e1cda270ae349284f9330",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 978,
				BlockHeader = "0847a5940847a59402e4bf7a3928c5e99086c2b78938178bc6f7fde436070fbd086fd98cfcbc906690ab002d49602cf568c407bed21f488f5aa265390f4e646b88d92343f0f0ba170847a594",
				MidState = "4e988ea1ca4c3dd066178289c5113fc905214fda253bad64fbe626d34fa3fbb2",
				RestHeader = "4323d98817baf0f094a54708",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 979,
				BlockHeader = "565c321c565c321c6b83c8b6343636059ce89bee6db8cadc1e92c33aa9c31cf90a1dd688ad3741b34948df6d66426312f942c06e27f9dd3799117e34382110eeca85ff1932523df7565c321c",
				MidState = "3afb66864564b23a69b430769ac28d642530f43855420dda04c01b7bb2d606d5",
				RestHeader = "19ff85caf73d52321c325c56",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 980,
				BlockHeader = "2a57d1ff2a57d1ff34a62ef86e72235cfa30b7a3ad84226a44b2ce4c8ef490314a71f28543ff7b0fd3f19ebb055b64f96a7c7cad49ea370dce39bd04db4a2ec8b1e90c7f116b9a902a57d1ff",
				MidState = "cb873d6ea4adc1a7360f6b8978166df44e3988537df4d5c5af87d336c3347aee",
				RestHeader = "7f0ce9b1909a6b11ffd1572a",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 981,
				BlockHeader = "776b5e86776b5e869d46383469809478079290da9003d5bb9c4e95a301b09c6d4c1fee818475a4806c4e42d4b94321f6a98289e299595940c8fa330d39d5f9396234d85c0d31b253776b5e86",
				MidState = "faa116e982ffc6ca9f164d525e82d861e7c440a8e5a6afb1f6104cb4a76a037e",
				RestHeader = "5cd8346253b2310d865e6b77",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 982,
				BlockHeader = "13947895139478956f854aac5f9c76b02057424858033c5e4c842250e728b5e5507ce878e6abe6d0497ec7cdbc862528f2d49904abeadd357bb1dd5d77241d3af8843e547f12b8d813947895",
				MidState = "e25497721a13fdee584db013dcd5de49f3805afa8bb9d8b553ebdd40dac9feee",
				RestHeader = "543e84f8d8b8127f95789413",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 983,
				BlockHeader = "a39e3931a39e3931e1b4345bfef6b3133ab44177eb1f8504c148f41d1b7c4d0d89a8036bf443ac38da000fae5e4bc33eb612be6e404bdf4f512b950a9375f8ffc86b9a69d971d6e2a39e3931",
				MidState = "ad882af9daa47f0209785c121f134451c04c68b8c4e3a49b7b5d9d93037aaf13",
				RestHeader = "699a6bc8e2d671d931399ea3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 984,
				BlockHeader = "8cf61ce28cf61ce20226c38e3d6c839d47abdd605a1df512d3f230edb0ca980a7dc1ad045c02a7a5de1669730464ff460b0eaf4720c0ad01eb131effd102472948b0233833294ba68cf61ce2",
				MidState = "67ca8971056a16f1841f41d26e4658bb7b1908c72782e1d56ff43b89662ab0c9",
				RestHeader = "3823b048a64b2933e21cf68c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 985,
				BlockHeader = "1b28f39a1b28f39a4d1441df724beecbd410bed1fc22e568bb0a414c02d72db38ad141f9a1a5f29a3facf6bfdbe64db99139b1559918e629fd52b9ef0e2169d49142bf212cc77ddc1b28f39a",
				MidState = "d8f36bdb2ae5e5dc1c5172f655e56d8fbe8a7c3719fe8bfffeb4c15195b5e55b",
				RestHeader = "21bf4291dc7dc72c9af3281b",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 986,
				BlockHeader = "fec897abfec897ab9854098b7bb15f0316e63e51cd0e0a1ad60fc77da24f4e26a82361b278fa6cbf171a207c0f64404f61df7f8c9dac218f4abcb71e3b379cf7f731e02e5a035e90fec897ab",
				MidState = "7ec76474bc832743687f60e0a792742c14c55be2d72595a9992bcda6badfd159",
				RestHeader = "2ee031f7905e035aab97c8fe",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 987,
				BlockHeader = "e7053f42e7053f42d333253f6cdbb2583b0dc9f5788d250edee11a81fc8373daae2d58a51a81e12ec9d07608b548d50e6355c547346a4a44f5592a7394113294ed52f827dba849d4e7053f42",
				MidState = "84db3dea0dd4e8fb5a4abc185284f3cc2b6865bab1d1187f129e634ce46b18c3",
				RestHeader = "27f852edd449a8db423f05e7",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 988,
				BlockHeader = "d1cf65ded1cf65de0b899008aae950ba0d9fc46b88dce08de91d1dfd0f33bb0dd0817a70786b24e755d5ca709978a2783991033017a7bb3cdf60cb320789e9e34d1033072a1fcd57d1cf65de",
				MidState = "1a6670fd88eb7a896bc2466a2f9960b07ece2bf3ae4b9fa508dab9d6ea7111f0",
				RestHeader = "0733104d57cd1f2ade65cfd1",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 989,
				BlockHeader = "acf0fe4cacf0fe4caa6aa15a9f5fc8c289e13d82a4eaddce6988d40c323756f89396ff503180e2be447193e9d2661a72181108cf9cd2cb1d6df01f9e2d6f926035b069056af785b1acf0fe4c",
				MidState = "be4c50bf85d0e463b1aee4f9efe071cee0f70cfd503ca45a4a4cd90681288f6e",
				RestHeader = "0569b035b185f76a4cfef0ac",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 990,
				BlockHeader = "4819185b4819185b7ca9b3d2957baafba2a5eff06ce9447019bf61b918af6f6f97f3f947d326feb1ebeace25e872fc0a659ea4f228e4cf2281b3816050732f89ad36f04d6d2148564819185b",
				MidState = "fd4bf38c1b9a2ae7ece88616ae00a3879f5af9c8c7970f3bb08b6f653c4b3492",
				RestHeader = "4df036ad5648216d5b181948",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 991,
				BlockHeader = "ced24f4eced24f4eb7370c9006433de9d50af3f7fb9e432071d49cb6d576ca25ba2a6691bb7f6bd0371f72cec452319b716802c0ff4533adffdc05e30b0da73475812f61548c38a5ced24f4e",
				MidState = "8e587598f386fa9980ad4784186488a6d2777616bc862fe72354a9af738e1e4f",
				RestHeader = "612f8175a5388c544e4fd2ce",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 992,
				BlockHeader = "8edad0f08edad0f0ea940db607e9a819728d2c4ea68fac82a09f725599ea48b1fb71dba85565b5c7a2392ea9125997ded7c63fec9d656e80b5b7f13950d5652e73e6e359a63e08cd8edad0f0",
				MidState = "d7690585db33a6e739abd244b58e7f89877adc1abe3ba979ebc977ceded53989",
				RestHeader = "59e3e673cd083ea6f0d0da8e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 993,
				BlockHeader = "5639ad125639ad129ba2f7e205172d13cd7873d255c087f0d33e1c37ad8cf6d8332add62c49d73835127627d9284303ae188f826b0f45f1375ed91bf03f079871c2d3048312257c35639ad12",
				MidState = "872b87d673149858fee1e9204220b0f5f6b3756f739e583eb38ee8defea7dda2",
				RestHeader = "48302d1cc357223112ad3956",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 994,
				BlockHeader = "f262c722f262c7226de10a5afb320f4be63c25401cbfee938374a9e493040f503787d7597bfc250db61f89b24f8309e1843babc097ff086d046f4ff02383e9a34c95ec23b9b44d8ff262c722",
				MidState = "4e97629fb824c0275f36c144a618c92851b86cd16492874388d84fd837fb4de6",
				RestHeader = "23ec954c8f4db4b922c762f2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 995,
				BlockHeader = "7ce749687ce74968d43f44b8b9f002cfd67e9adac7754cfe4d25cfebba92455c9669f5a99a44c3c62d0271f1834ba2d1c5313a6ec925825f57175c63d2a441bd132a9c72dd0a65207ce74968",
				MidState = "d2d808cef0d24f321c6610def4a8e7c56817bca02ca573e0299738245582c79e",
				RestHeader = "729c2a1320650add6849e77c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 996,
				BlockHeader = "cafbd7f0cafbd7f03dde4df4b5fe73ebe2e17210abf5ff4fa5c095412d4e51989817f2a536c8b3b9d7fb7aa63fa896d79329e863272a8753fd8f56b204bcb8cc3933fa5e79b84bfacafbd7f0",
				MidState = "019c4d2771b8322d28c854e455677fb1d3199aca605b00365dfb2e1f78a4acad",
				RestHeader = "5efa3339fa4bb879f0d7fbca",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 997,
				BlockHeader = "a66e3cd3a66e3cd3fd3867236f777531de756954f90e5eba686e11c364f0922f4414da6d49fa24b746ee11090fa77ea95bb404dcd2a9a34d62b81e6432157c0c8d15c004631fd355a66e3cd3",
				MidState = "d012d89340c34e543f808674499a11daa98a543a8c90dc2efbd2a69fc4896091",
				RestHeader = "04c0158d55d31f63d33c6ea6",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 998,
				BlockHeader = "f482c95bf482c95b66d8715f6a85e64debd7418bdc8e120cc009d71ad7ac9e6b46c2d66947e2b3f0c9c64ef7513e5a8a5ac294be414e7bf04d8619d10f94410d71ee2f66c2ae4f6df482c95b",
				MidState = "acaaea71cc445a6ccad39e1607b592510d06b6d4ff0bcf2125903480ead6cdc9",
				RestHeader = "662fee716d4faec25bc982f4",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 999,
				BlockHeader = "24185f8724185f8796fe247269eae14f129f15ce6d1385620244b75c0e433d9da141738e4bd3adeea7972595516782683cbba6fa4f564c63dbb5b4098cb4716e84595b4f57f9740a24185f87",
				MidState = "7fc221207b37efdb8350235fc21c2487f56bb1c476401ca7784083af36cb0aa3",
				RestHeader = "4f5b59840a74f957875f1824",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1000,
				BlockHeader = "722cec0e722cec0eff9e2dae64f8526c1e02ee05519338b35adf7db381ff49d9a4f07089029df46ee50d67f80579e0f42a059bbf5cce289be0ebec17e2f229a4b7131d1c545ecb33722cec0e",
				MidState = "4f3171db93d2941606cc308061431fb943dd4671d0295daecbb5e0922d79c24d",
				RestHeader = "1c1d13b733cb5e540eec2c72",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1001,
				BlockHeader = "c0417a96c0417a96683d37ea5f06c3882b64c73c3513ec04b27a440af4bb5515a69e6d8573e3fdf76e55630ed0a52cde62bcb72f82fcc2ff5d896e1fe7165d49df33eb765fa91f25c0417a96",
				MidState = "d1a35533c9d3cf87e944ca978125696fce37db43958ac67113a66151737c2ae0",
				RestHeader = "76eb33df251fa95f967a41c0",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1002,
				BlockHeader = "daf37dc6daf37dc669ee8bbb4a3a06e7def912d680e1b157f876aa3c5e9c70ece13a02dd1d8571a6cdb34d77f015377c3719c5209bbc83180e03759bae457e29ad37d200ffb5cd6adaf37dc6",
				MidState = "6a467b94d824cd0b9042ffbb1cfffb0e3b21dfb0c8e078c08f19795074598f09",
				RestHeader = "00d237ad6acdb5ffc67df3da",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1003,
				BlockHeader = "27070a4e27070a4ed28d94f746487703eb5ceb0d636065a850117093d2587c28e4e8ffd87ac91fbce9970f90564498b15f4ce9f43633259819ef4693758e0ebcd3e07c7b7a8b430127070a4e",
				MidState = "19e3ea985f7b20972065ea9dd259191dd1b483775ed4e48f044fdaae86c449f6",
				RestHeader = "7b7ce0d301438b7a4e0a0727",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1004,
				BlockHeader = "513d3043513d3043db0eae8536cc602873ff3d5b63ee153a2818eef96817244fa8ab81b5c206b1522be781fe77f3db0c9a7e6cc3e1b4b2e8bb372277d52655b57abf642bda5c8f1c513d3043",
				MidState = "a6cde40e325945f07865725d3a78ac640faca145800d3e715df4be912b741eda",
				RestHeader = "2b64bf7a1c8f5cda43303d51",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1005,
				BlockHeader = "3675b2263675b22644ad7712f9e6154e506aa5e426c95eea664d8b644e5f533af199e547bedb6fa4eb961aae59a49dc833361481ea79d586f5ad621c73a883f414a919030b1e6f3e3675b226",
				MidState = "30faa3bab7068ff680e5e11869024d647ce0348567ffe8f8adc92dc2dba84006",
				RestHeader = "0319a9143e6f1e0b26b27536",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1006,
				BlockHeader = "d19ecc35d19ecc3516ec8a8aef02f786692f5752edc8c58c1683181134d66cb2f6f6df3e6409c3c664559853f6be5f6e1ab6dccbeb22eff6000b875fab3aaad6af9fa97e930ba609d19ecc35",
				MidState = "ac342eb8060cdf4cef1a0bad36690905252a469ad0fe79fe668be7e1af078165",
				RestHeader = "7ea99faf09a60b9335cc9ed1",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1007,
				BlockHeader = "2476947a2476947a9912e759a616422e3173a3348a28d8742a92e15900714df7658ae898b1026419965b9be4a59cf0484cc387a660235d5f81859419b0fd8046ff3efe45a5a6752d2476947a",
				MidState = "1fc4d920575783e71d53cb8d05bc2dc406ad53172c7a71b079a0353a61df6b4b",
				RestHeader = "45fe3eff2d75a6a57a947624",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1008,
				BlockHeader = "dc6e9de2dc6e9de27b9887bfb048362a61e39df7bd237a85c08fdde1bdff73ab71b01fb8b6ca5f5aae90329e89348bff0a753e6a36a8b1e9d76f9aea916c72b23acc1d2184ab1521dc6e9de2",
				MidState = "ed50a061e4d3b7bab8a6cd8b2a4a072e4c10a1e804bc4a17e7c62f33c4316422",
				RestHeader = "211dcc3a2115ab84e29d6edc",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1009,
				BlockHeader = "998500f2998500f2074d6d0812d497b9d7d0e53bcb0c09218ba7d80a6cfcebab623918717789c4bba2ec929149a54ad35615bdd58ea2b702f413c2138c6565bb0e21140be75acd85998500f2",
				MidState = "5a8f868f457114a24ab92ef0776b37f65e11592cf01ec8f3c29ded46854ae9b1",
				RestHeader = "0b14210e85cd5ae7f2008599",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1010,
				BlockHeader = "e7998d79e7998d7970ec77440de208d5e332be72ae8cbd73e3429f61dfb8f7e764e7156c0d51888d42e08a9b6febe0b79c88a3254a4c998348a35ff3439c8bef6286573020a1070ee7998d79",
				MidState = "0a48a38a67f8f9bfc41ab3babe06a9fecbcbfb8d534a440a89cc3cca40d0f743",
				RestHeader = "305786620e07a120798d99e7",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1011,
				BlockHeader = "bd47780ebd47780e885783095d387e8e8907a859a2da14b4e9d9f010129af743db121776f387c3ffc6134d0afab1bb1a3b638b0f0c4b34886d2641c8853aed06fe36494208c4fc9cbd47780e",
				MidState = "641702c39620ee094e1113686ddad43bcb70b0352ec4d0b9344664a7203918ec",
				RestHeader = "424936fe9cfcc4080e7847bd",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1012,
				BlockHeader = "9868117c9868117c2838945b52aef59605492170bee811f46a44a71f369e922e9e279b569806403d202053f57b6d4d21cf552a01417645a936bc89ec26674b965c6dcf21e4a9c6689868117c",
				MidState = "5ae379e6e195f492fb32fa1e7245f49da58ab8dc523874f5098b5a067cd0721a",
				RestHeader = "21cf6d5c68c6a9e47c116898",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1013,
				BlockHeader = "a68420a5a68420a5c3359fbd4f62d1e2ae2e33fd4c592ea8f1aa43146cce1cf7e21d0d6853de634a55bc76e305b8d770239756c4faefaf7e63d49549ed120e8034e8e4546ff549faa68420a5",
				MidState = "b1de0d3f650844752e375263a442ee8e2784a05f42c7e2346ebb6d1b6c3f93ed",
				RestHeader = "54e4e834fa49f56fa52084a6",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1014,
				BlockHeader = "e2362d41e2362d410de8c31160dcca545926c9e4b790310c19712d429731509de88f5cb6fa8382bef1d41347979c46ffb6e8cfc9f8eca6de4db4d3487145db34ce75240b8c2f82cee2362d41",
				MidState = "0465f3ac07beff17738d65dad2dbc00185958cef1a20accbebea9f3b41077bf4",
				RestHeader = "0b2475cece822f8c412d36e2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1015,
				BlockHeader = "304bbbc8304bbbc87688cc4d5bea3b706589a21b9a0fe45d710cf4990bed5dd9ea3d59b2176e8808b278197d0058424c25f16908a6921f727fa577aa68e679453407623000968b2c304bbbc8",
				MidState = "5c7386d958cdc8202ee30330f44801c290c880e5d6b93014358753b0f259b935",
				RestHeader = "306207342c8b9600c8bb4b30",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1016,
				BlockHeader = "7e5f48507e5f4850df27d58956f8ac8c72eb7b527e8f97aec9a7baf07ea96915eceb56ad42309b973784bb33e3f6d8c418b3157efff2ee7a2f6a6ed111d31aa1de75f50a3760a72ef0533c6a",
				MidState = "a85983ce7cd519c70fe34bff2678dd9a0d5786263762b89da40f7e3d4c9b01e2",
				RestHeader = "0af575de2ea760376a3c53f0",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1017,
				BlockHeader = "c0a6e62dc0a6e62d2a9e9918d56d856b01d1fe7fede623f6cd1d6d6b28955934931d400af9cae05a67736316abc13d58e8872ca57cec2a0229605f08f02e6ef3903a58452d7e4771c0a6e62d",
				MidState = "06221c65332d566e494ace0e6778d6d2d23cf4e74fd0abc21d6c5949549f77a8",
				RestHeader = "45583a9071477e2d2de6a6c0",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1018,
				BlockHeader = "aa7ac4c5aa7ac4c5edd9f3476264824f58b1956444c62e3a88773c11def21f248b1efbceed918c4270b39a6f2729eb4d134cf6397a8e88f03ff025988418d71a767a927dbe36f4c4aa7ac4c5",
				MidState = "629d819cc84bc6c0660c406b59b514ac835e8167249df9a2e8feb70240c7d7fe",
				RestHeader = "7d927a76c4f436bec5c47aaa",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1019,
				BlockHeader = "283dba7a283dba7a27753a55a99ca9342711f43cf46a691f9f5ee042922b8a2495baf4cb0f5a7de05fb00fbc91a8259bdc35e57f70bce8da6728e0046c4781343cbe590b321721fa283dba7a",
				MidState = "dfe1a07be452e1a8be88ce7e683625ac2d417172213a47fd78e1b7b7ecdf2c03",
				RestHeader = "0b59be3cfa2117327aba3d28",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1020,
				BlockHeader = "107532ab107532abd160fca3635d80985523c3c5595a425a174d7fe525363a5b149b5f3dc03a80defee4bb3031852f4dcacf0b45a88334eb776d3ff7e34a9f2ef5a6f57823adf504107532ab",
				MidState = "c6ac76943be29cd9aa729cd544608a718c575e6764123d18705b675fcb8669b5",
				RestHeader = "78f5a6f504f5ad23ab327510",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1021,
				BlockHeader = "4da6f31b4da6f31be06735f0bb21360b03512a4e276aa0e8125b9c518166dab7ed81984e3dadd98263bc5b090b555d7692df64be48cb4bba4467fcb31d38342a8122240220e803e24da6f31b",
				MidState = "e40ad6034343e7b9450a8c34755afd4211174cdc4569241a84f631e23f180d06",
				RestHeader = "02242281e203e8201bf3a64d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1022,
				BlockHeader = "75d6b9bd75d6b9bd4bc2a529f5e2b4124e55076dc79041e6c5d4963d76a418aadbd14d41b31a4df61def33d979ba32185cde83f59cde5681942f2b56eab3026d32c0f94b9bc0201275d6b9bd",
				MidState = "148c82c14747226fb04cca4cefb2fe3d8bba321c873587d2a32a39c276dfcf64",
				RestHeader = "4bf9c0321220c09bbdb9d675",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1023,
				BlockHeader = "c3ea4645c3ea4645b461ae65f0f0252e5ab7e0a4aa10f4371d6f5c94ea5f24e6dd7f4a3c904282b6cbc1bfaf602b636bec59d5aee18e0f325f1c7c993ce0fdd57d80531503b58ab8c3ea4645",
				MidState = "1a67a9104bb0e94044f8e1eb899efb319cf4319b156df7c1a6968c1ca6b156e4",
				RestHeader = "1553807db88ab5034546eac3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1024,
				BlockHeader = "55ecc23e55ecc23e736995a6fd977c4f4dccb3f8969a1e4a9628d0aff3b103e727179522b3b79efcfe6d9065c9051d8520b9bc82307812a25ef5b9c4efb2a42789ac8a63342d83de55ecc23e",
				MidState = "cbe2128cfcee0e34ee8eae079a3b69f0c487d0f4c4d8934135ed175272db276e",
				RestHeader = "638aac89de832d343ec2ec55",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1025,
				BlockHeader = "296c3bb2296c3bb2e36d020296e82bdeb06b0e7d07904159847586972f8003a7ead0258bd03a4db4ca7bc2c0783db1fc8028fe7c425fd1e1e11bed21759dc83d4ceac60c496d0fa0296c3bb2",
				MidState = "6aac419ce5fcbf4095d9a1d0c683bfacffca6379baa0860db6b01f80d0d1aebe",
				RestHeader = "0cc6ea4ca00f6d49b23b6c29",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1026,
				BlockHeader = "c49555c1c49555c1b5ac157a8c040d17c830c0ebce90a8fc34ab134416f81c1fee2c1f822635f83063b564870c0e56b15b897c73c9c1584895a51d4397df487e263e3068f69c11c4c49555c1",
				MidState = "95abd7cb2370dbf42b1dd7e6359ca9a2473f76d5c55f079ee68444b28014a0f3",
				RestHeader = "68303e26c4119cf6c15595c4",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1027,
				BlockHeader = "b15aecf1b15aecf1707fc43539f7c437dacd5fdc642f9ebd772b76aded584009c7d4ebd7d0fe7b2abe88ef44d230f3fa4875b7a785e883b5593f3d3377aa649303b78c551b52cc3bb15aecf1",
				MidState = "66aeae149130d4ed11a9226c41fd6e5629b5391f3b4813e077945529b1da3aa7",
				RestHeader = "558cb7033bcc521bf1ec5ab1",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1028,
				BlockHeader = "2fce89cd2fce89cdbbe9319e690d7318ea05092dc9233f799356f3e651ef256ad04485b13cb6e88e5509297b26d5d7ce9b16452ff922c5db47b5e0715da0f6ba859eea7d46677c7d2fce89cd",
				MidState = "1855dc428dc08b21aef7f8d5f31a88d4de42e54729667ad1f1673dddeaf447ae",
				RestHeader = "7dea9e857d7c6746cd89ce2f",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1029,
				BlockHeader = "233c1fa0233c1fa0a9b0c06d84b88790cfb40b85532340b6887170754513f71428c4a9be1c2c179c6b01e1698556c6eb5cbe05f86742232a901ef1ca6f3b50dcdfa6ae29a20e97d3233c1fa0",
				MidState = "d546ae264c2bd3aef558c014a3bc7a87f2932517e6db33f6c652c006dcccf088",
				RestHeader = "29aea6dfd3970ea2a01f3c23",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1030,
				BlockHeader = "fe5db70efe5db70e4992d1c0792ffe984bf5839c6f303df709dc2784681792ffebd82d9e9d31451138327dda51c72b77e2385039bd4b72cc5529df670d67dd62f6b4a93ee64cd9f1fe5db70e",
				MidState = "34d878b7d4876f4a1c1c1a33adcdc46f302c3aa4743e61b74212c79341e431a4",
				RestHeader = "3ea9b4f6f1d94ce60eb75dfe",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1031,
				BlockHeader = "4c7245954c724595b231dafc743c6fb458575cd353b0f0486177eedbdbd39f3bed872a9a23f84834ec42b5223e26b5c3083cd6ff05a5a08a73522cdbf231c25f280ad41e77b8689b4c724595",
				MidState = "a4130354e6b13e8a7dae302bef6fe9782df71fbfeacf7b696a36187db5d61a90",
				RestHeader = "1ed40a289b68b8779545724c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1032,
				BlockHeader = "0785c03d0785c03da286e5b7632b4b1dbd41c29dd485bc63ce212b4a7772322de73c307a9c23ce290d173f336ed6bd0dcdc6d26a0f4c9878a26249cfa097d070aab7023a7c3dc7b50785c03d",
				MidState = "8870ce02f695c8c7256dc732ae2169893d15a41f37b515682a68a68a4027afbb",
				RestHeader = "3a02b7aab5c73d7c3dc08507",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1033,
				BlockHeader = "a3aeda4ca3aeda4c74c5f82f59472d56d605740b9b8422067e57b9f85dea4ba5eb982a7171c5063c36b8ceb65b5301bed23f9100406049c34df20529d6277dbb79f7ad1a9c57f8cea3aeda4c",
				MidState = "dd89aa865ed6b671f1237f69b7f181426cdfc6e68d485002fbc9c5ad3f0dd6b6",
				RestHeader = "1aadf779cef8579c4cdaaea3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1034,
				BlockHeader = "5d0b1a6c5d0b1a6c1447bfd9a8d2b66f9729e93ac43256477a4c244b8f904927ee389725a348cad1c7431bdfa3904eabb46b84a8aae44d1fb9ca371000ce4fadd151a816a58e31605d0b1a6c",
				MidState = "0a89d5c4e934e94efd788eb5ebada8a615b3f08a523b07d7151c6b671c09ce01",
				RestHeader = "16a851d160318ea56c1a0b5d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1035,
				BlockHeader = "ab1fa7f4ab1fa7f47de7c815a4e0278ba38bc271a7b20a98d2e7eaa1024c5563f0e69420c00616776cf6dbf86d32c4547d1cc3b1cb3efe920a432b6ae0cc68ba9418374278cef99cab1fa7f4",
				MidState = "110aef98137a521df287137c1dabd67fda05ed4857af406c3f7639b008d81af7",
				RestHeader = "423718949cf9ce78f4a71fab",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1036,
				BlockHeader = "86404061864040611dc8d96899569e941fcc3b88c3c007d95352a1b02550f04eb3fb19016b314af020d24998adda9736412c4392835157b5325f110ce0e2134efc656f26209be67586404061",
				MidState = "95868d4523a75f6dc240661a3366d651829ccfd4212eafef6e83e9eda2369c59",
				RestHeader = "266f65fc75e69b2061404086",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1037,
				BlockHeader = "0c22555c0c22555c1841f517fc48c54f949e83de7850a289e9b6ae6369e08a0894053c77d088ba9ad21a701b6039f28f4b2b2b9bb5f712c68de133ef3460aa94a5833431aa1171770c22555c",
				MidState = "a8c8bcb39321f7f2af8df2b98f3c11b90e237e13cc27c0a671d9bf1aa2293eb9",
				RestHeader = "313483a5777111aa5c55220c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1038,
				BlockHeader = "ecb34e40ecb34e40f5286a19a323691e6a6fbfa6d2c192feaaf9c865743436e13b18e58828a183a52ef00d60d074bdaee04b177a69ddf7aaa90a0f3641ab654ab3239c707cb94f1decb34e40",
				MidState = "b3c15ddbf9eec992afd80e2a1927d02b673ffd22b549e90781431d2c83a74c8a",
				RestHeader = "709c23b31d4fb97c404eb3ec",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1039,
				BlockHeader = "39c7dbc839c7dbc85ec873559e31da3a76d198ddb641465002948ebce7f0421c3dc6e284ae026b2a19767594504244714f30deb70bb4d279f662ddcd5aade5bc1f21665c2957c5e139c7dbc8",
				MidState = "b3a564bc2e030065a7163a1c8eebe962757bf900475bb8d27dac008bd23d595c",
				RestHeader = "5c66211fe1c55729c8dbc739",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1040,
				BlockHeader = "44fec87d44fec87d9772efe780c0291fe8e21299c2799e78e9159c8f6db271d8c81c5bb644ff341541a08f288f7b3355ab546ea2e8843d770be18bdc556ae380de9f0637146328f044fec87d",
				MidState = "69f40c46ff30e99937f135535cb0fe46891d2d47f3cfcc6ca5fec7c0bbd666e8",
				RestHeader = "37069fdef02863147dc8fe44",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1041,
				BlockHeader = "2d3c70132d3c7013d2500a9a71ea7c740d089d3e6df8b86cf1e7f093c7e6968cce2751a98836596a34cee387a8cd9325c4f564e5447ed89b2ff3014f2f38ea735a5ecb06d58b1dbd2d3c7013",
				MidState = "482ed5bad798d3403719d3e963666827749c2514577eba0781a81acd5323999d",
				RestHeader = "06cb5e5abd1d8bd513703c2d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1042,
				BlockHeader = "7d8b14b27d8b14b233a45f016c710318c289a5e0dac8cf18836433dcfd39c5c04a4e5b3b3f5899ad1f6803d137f41eb72aa265672e3c665992db14ff91703b489d2853420a93dd047d8b14b2",
				MidState = "5af4d5eb9032ac6ce93499538737ac72d6468485d751ee507a3f2dc4fced04e0",
				RestHeader = "4253289d04dd930ab2148b7d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1043,
				BlockHeader = "caa0a13acaa0a13a9c43693d687f7435ceeb7e17be48826adb00fa3370f5d1fc4cfd58372e9d9635097345b9aeae6e7d0b8a8ffea0b4bb383d1d671ae13af3e59c34ff004a17f412caa0a13a",
				MidState = "7abbce2be440759a5bba061493e02110e67114b4d1f6a1e6b7638b43d3934b3b",
				RestHeader = "00ff349c12f4174a3aa1a0ca",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1044,
				BlockHeader = "3d2406093d240609a1282ffdd2045d57e64c8d42746b2aed649dbbf188e1d2c336015d6e90ad7938b81660b63bf94448aacb4c392b7be3ca3adeeb849381d6071bff9f4254514e033d240609",
				MidState = "d64aa35dc0b1a0d60823686eff50b4b2ddfc00a093ea5d59cdeb707d352f7445",
				RestHeader = "429fff1b034e51540906243d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1045,
				BlockHeader = "647a36d0647a36d0439defb15e63c3fa4e6e8cfa0cb604197b67b914f69ad535f8c788ee26e841f2f252117be832dfd68e344661d5c3343af4614844618efde8745d4e6c5b7d84dd647a36d0",
				MidState = "7cc66989b732a8f39f4065bed6ace25b3efc8391ccbad27361716216c8f990e1",
				RestHeader = "6c4e5d74dd847d5bd0367a64",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1046,
				BlockHeader = "7def75f27def75f2a7f6b42f0434a9a304fbe680b771f990787e66faf7a59614b97b0c3a015a8c7664b0a8577ac91aafe912ad2e215e520ba308e46d6d2a7ac9813d5f4d85836a8f7def75f2",
				MidState = "a9730d4565db418ac6bd4348c8945cb1ff2ac81512d2a983e678725c7652c113",
				RestHeader = "4d5f3d818f6a8385f275ef7d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1047,
				BlockHeader = "70b5a5f470b5a5f469978181317147a163a9002b48bc307ae4c840ee9b47426d09d8892b9a846cb8af2cba3afa391cee7f4a4aa74ab6ada00779ef783aebd36c6e7a996ccde29f0d70b5a5f4",
				MidState = "600c5d67fdf1323ac5926e5a9487c40363c8edc9d36e1ab73ac402fbd68444d7",
				RestHeader = "6c997a6e0d9fe2cdf4a5b570",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1048,
				BlockHeader = "59f24c8b59f24c8ba4769c34239b9af688d08bcff33b4a6eec9a93f3f57b672110e3801ec3e7d478b25357298bce4c2663ee970b826ad2374ef7dbc2710e9201b9305a4b4a560b3c59f24c8b",
				MidState = "34b2b526a377100d1c4376ca4a69af199f573f3e20e07117070c8aeac9d9a80c",
				RestHeader = "4b5a30b93c0b564a8b4cf259",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1049,
				BlockHeader = "fc51b211fc51b21141654aa7624f9d6d7861fd51b437f36d28b6ea238d8802a95e7683634a0431447e6abe341322f14145a062831c41326e1b58491ecfc63c2a9e5f6f224138463efc51b211",
				MidState = "29e4b2c0f020d78799b609e7469fee808376c0316fd8d997a6bfad66f16a2b5c",
				RestHeader = "226f5f9e3e46384111b251fc",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1050,
				BlockHeader = "e2cd296fe2cd296f4e46583e46cc660056357f09035136f12dd5ed7c21a50caa9eb71df33519cdd1bd98c1083468ef95596cc4048e9ecc0d79817cc69de3e12b7d8c1b01e0acd23be2cd296f",
				MidState = "a2822edd6951c7576e8af49d5ffdd792ad55b24172712717910c4535184e376f",
				RestHeader = "011b8c7d3bd2ace06f29cde2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1051,
				BlockHeader = "e0a0ec59e0a0ec594abe2e378f8c010e384d209bc569232c5c6fc74f43acbfe90b13d6b73fd2a4fe759b624d7cab3571636815873cf3592fbd9b16ddc939470cf291c807f144aa90e0a0ec59",
				MidState = "183a17ac2b623e5ff0866dc8826cb4fbfd9cb95906eea873e57260bbcb1d772d",
				RestHeader = "07c891f290aa44f159eca0e0",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1052,
				BlockHeader = "bbc085c7bbc085c7eaa03f8984027916b48e99b2e177206ddcda7f5e66b05ad4ce285b986212fa6d60daa81d7e9d8238354bef510f0792867a79d64361f55969900e6e735b698b18bbc085c7",
				MidState = "b27e708cca9e98e5984a2a1454ce52f5cf3bdd1c4cde9a7b81b64240e4133390",
				RestHeader = "736e0e90188b695bc785c0bb",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1053,
				BlockHeader = "0bdf196f0bdf196fb1ad120b8c3f610ca6e000833fd21d4b0e7fd6586c4a8fb353332b2d6b588212fc61fdd45dc3e10d4ef97e646eded9ffb0c1f0fdcca8661c169d46adedb943640bdf196f",
				MidState = "4846a6c97099fffacbbabf2f7b4df124d14897e5114aea19a36440094694ee4e",
				RestHeader = "ad469d166443b9ed6f19df0b",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1054,
				BlockHeader = "59f3a6f759f3a6f71a4d1c47874dd228b343d9ba2352d09d661a9cafdf069bef55e1282842243d3fe33c783c8ace4a860da92451e322e1a3398390bc39c63efa403bacc3c22a656d59f3a6f7",
				MidState = "2078e8c4f4d123d760151154276153c171bcf79cb4d7e0daf4b38f75500bb3ab",
				RestHeader = "c3ac3b406d652ac2f7a6f359",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1055,
				BlockHeader = "026679aa026679aaa04cec79c8288acc4ce1221a2c822ba57d7cfff7d60456bf685a048128c1c4e23882c253104c05938d4e781049e2e79f0b50c3d0a01f467c6b565098efe1f591026679aa",
				MidState = "f87d33b8fb53db669f6dccbcce7964a2453d30c44d609698c8feace07eb4e44b",
				RestHeader = "9850566b91f5e1efaa796602",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1056,
				BlockHeader = "507b0632507b063209ecf6b5c336fbe85943fb511002dff6d517c64d49c062fb6a08017d47b470ce66cd1830f26af7c04e4dcb95dbf6f7aad96da53970aba0879ccc4d86ea86eeff507b0632",
				MidState = "f1cbe03097fd7ca3b001b97a0fe0146ab8c8eddd0a9158988d9c8d2396528367",
				RestHeader = "864dcc9cffee86ea32067b50",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1057,
				BlockHeader = "9e8f94b99e8f94b9728bfff0be446c0465a6d488f48292482db28ca4bc7b6f376cb6fe7891ff879988dd66394142a6bb9ad1efa58dce324facfe25b90f878c013431c7ca68ddcf5d9e8f94b9",
				MidState = "e66317fa45a6b8303cf66c24bf179a4ab4dbc7a19a2f70362d465e3340e23094",
				RestHeader = "cac731345dcfdd68b9948f9e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1058,
				BlockHeader = "32d2ba2932d2ba29694b501282eaa50266c67b690d354fb74cbe77c642008c3f280a4e43f442f051701ecbb3e1d35acd78de2eff6c5d2c05df4393b27b4d029b307954f95aaac22132d2ba29",
				MidState = "b1613faae13230654a3541b958c27e18de7943cfb5702c7175853e6bffb84632",
				RestHeader = "f954793021c2aa5a29bad232",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1059,
				BlockHeader = "cefbd538cefbd5383b8a638a7805873b7e8a2dd7d434b65afcf404732978a4b72c66483aa44bf025fb36aa974ef870b976f182a4079b0b4daa95690f5e66727257ee62b3d85ab04bcefbd538",
				MidState = "fe37b2a9983eabc71bba6cb620811f04de3e3aaed82b3d79c5fd334969105226",
				RestHeader = "b362ee574bb05ad838d5fbce",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1060,
				BlockHeader = "e4ff7b8be4ff7b8bef7faf68d56a0d2747caccaa1025151ef1b8b0885ff03cb4f069a9a66cbad05f723565e061b9770807f4660cfe6b6a9987ffc73a2d384ee15edb3d97ad39b889e4ff7b8b",
				MidState = "71da55c6cf1ec7ce94e1e6895eabb7e9aa81badd325ed426aebe99c4756348b5",
				RestHeader = "973ddb5e89b839ad8b7bffe4",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1061,
				BlockHeader = "3214081332140813581eb8a4d0787e43532ca5e1f4a4c86f495377dfd2ac49f0f217a5a25a4a4661c84724802d49f95c929408eef7f2bd2fe60c37fa57d9ad16655c0ae1907b57c232140813",
				MidState = "625a8f8e9c0343216d6e8960b90177c8e9e8684eb475ddb625efa237c023a12a",
				RestHeader = "e10a5c65c2577b9013081432",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1062,
				BlockHeader = "7a73cfe57a73cfe5ec52ca20d99b7606c20af4630c647d7871684d57deb5b0c8fe9ef2cd152d89f6ef8adaf4844de92337937293ef06bc9203b8ef9fa6f1fca50873ee98a640e9c77a73cfe5",
				MidState = "61e033a4a11dcacf39202ee905759cdfac52782d9ccfa98f63b0dd5175d3dd31",
				RestHeader = "98ee7308c7e940a6e5cf737a",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1063,
				BlockHeader = "604676586046765887512c7ace2207b1cc413086d9e8b4cc5fefc1e1f11a2ec2f71d9178c2cc835331f8aae6017fce69f50e6c0afcf07a1319bc71d89cb3043e01cd7ad7b49d6eb460467658",
				MidState = "d4a428559c16c4f3aa080c5e21aa0a0bef8c72498076d08dde2a7de5a2fcefa7",
				RestHeader = "d77acd01b46e9db458764660",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1064,
				BlockHeader = "ae5b03e0ae5b03e0f0f035b6c93078ced8a309bdbc68681db78a883864d63afef9cc8e73ee68a1463080285d6678d9cbeecc177ebac1eeeb7fb48a17010125f2bef49ac5eed70ffbae5b03e0",
				MidState = "b15e8c79a4505dc4c3c1b6d4f7808392625a42b5c00a9691bae8ed4679039e71",
				RestHeader = "c59af4befb0fd7eee0035bae",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1065,
				BlockHeader = "92b1497192b149713a1407c3f2188dba2b1961df44c314f21fd2253962ba98737e4bb12527f803baa0ba32e679c587c2333b2fd6ebf62063d3a5e68867f2b40b990fd78b7480388b92b14971",
				MidState = "10c7c7628b10903ebc6c4658dafb291a3679da4a9bf6c2ce71f300705474aa43",
				RestHeader = "8bd70f998b3880747149b192",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1066,
				BlockHeader = "2eda64812eda64810c53193ae8346ff344dd134d0bc37b95cf09b2e64932b1eb82a7ab1cf9e26558a5614816dbdc89495c033ed53b5ce8731bd3614ab8059361a767cdc4b511cdca2eda6481",
				MidState = "5003f4716210828ea82fb82a6f7e99eb6cf589753acfcf01db29cfa0172ab2ba",
				RestHeader = "c4cd67a7cacd11b58164da2e",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1067,
				BlockHeader = "7ca1101a7ca1101a8786179911454d02bfb4c9b175e4e2d0f4071805fa6fbcac63c72206d3f368f1023b0723043dba16dd63f4fa88a48fa346c733c53e8f931bf0f914a0021969737ca1101a",
				MidState = "ac89d475a0cc4f3fd49527a6c08e0ee1525a33a3994c77c0138b27ca10a2129b",
				RestHeader = "a014f9f0736919021a10a17c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1068,
				BlockHeader = "d8550e71d8550e71313e648924f777ab4ab8639146ad3244d4175f676ed65a144c312d6ecb3ca7c5598d5d388d100bb2e45b50a77ac484582534696e290c8f954c0cfed4b4b84b7dd8550e71",
				MidState = "fa1df532eeb1053958ba80607d5dcfd14767cffabe84c1183b3c99b13590a050",
				RestHeader = "d4fe0c4c7d4bb8b4710e55d8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1069,
				BlockHeader = "b85f1bd4b85f1bd4148fc8f9a7b5c2ae5a6db7dafae9f96ee09c4e8688db9ad10158f3d217add0ccaaa2e19f4bb0767f271d5e0f9fd7fe22dd5d552713181a5d2035d8f955464981b85f1bd4",
				MidState = "53a25c4ab40b60d68b1cd0a0cafd8a29f08b2794e73be1f2016d47ba936f3f31",
				RestHeader = "f9d8352081494655d41b5fb8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1070,
				BlockHeader = "7cb799b57cb799b5eea9bf02cae2045639f454cadc79d790468e957e01c66928da6903655209f4a9076f11d16c756e775227ab2864e9b13866177987d5bf7769bcc0dde67ed408e07cb799b5",
				MidState = "df775f67a25b5db52ad9c52dc3db6675d0f423b52b9986ad0eb5594ed6541f60",
				RestHeader = "e6ddc0bce008d47eb599b77c",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1071,
				BlockHeader = "6346e46e6346e46eddccd3c32d3d993873c534a8781b8af5fd272f6428910b0269dbc66cc50a1c1d7ba16d5a700b23bd1f9149336f47f74791ba8c6909184bf6dab472e6d049cebe6346e46e",
				MidState = "bd13f2065c9bca4e959ead5797ee007f16463bc3e86c1a1ea2a36795a304829c",
				RestHeader = "e672b4dabece49d06ee44663",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1072,
				BlockHeader = "b05b71f6b05b71f6466bddff284a0a5480270dde5b9a3d4755c3f5bb9b4c173e6b89c367940106e6a720e1df8d06c9af8607d2e83895fbb057e01872a3381113e4d59cb898005f0cb05b71f6",
				MidState = "91055800f81f30f163d67bcc92468ab4c42f1715b7adad1eda87324bde783211",
				RestHeader = "b89cd5e40c5f0098f6715bb0",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1073,
				BlockHeader = "c2d15f43c2d15f43998802bf129dffe9d1b5c54ec4246fcd5b8423bd82e1e11b45de2ebed177fbff00c3e38fb1c785ee71bcbdfade199dbb20f49e3cec66a89901cd32a7fa938200c2d15f43",
				MidState = "5aa01024d8e9a55010849ccced3dccb7715bf1453a827f91d0c9448a472199ba",
				RestHeader = "a732cd01008293fa435fd1c2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1074,
				BlockHeader = "e8103841e81038416a6da80c2b9f627d3aae6a6de01a05df3ff4124d3e000e3932b8a8bc22ed0911703dccdd35bc8d71a8333273f27558725079cc6094f2b70cb4c7d4a5490fdf33e8103841",
				MidState = "df380edbb53fdffb25a7de81391ce2854e06e0d374066e4d7adafe59d4185b4e",
				RestHeader = "a5d4c7b433df0f49413810e8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1075,
				BlockHeader = "f7ca8ab5f7ca8ab5cf91de66bfcb99a4a8d7bd1391158ff8ae73e9b81919d9723e85aefeab7baa2e6e8c45e49bcbe9de7edb888f6f0ab83a561ba9c2437a9f73add322ca6e8a8b5af7ca8ab5",
				MidState = "09f8a34d8bb60c1bb1d14a2b924b5234c46ba69cc6d2fa06cf0ce624626b69b1",
				RestHeader = "ca22d3ad5a8b8a6eb58acaf7",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1076,
				BlockHeader = "8db9336b8db9336b30b643f4e2412e7866495e0f7503fbacfba1abc91b99b3b4189ec3221e993f9c70737bee51e5647c345cc725f9901377f9f6f53c7a400a5a0f55d492edf048f28db9336b",
				MidState = "2e6fa54690560163c7cdaddf37988e020c6db6ec813c56de0b57b35282c09381",
				RestHeader = "92d4550ff248f0ed6b33b98d",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1077,
				BlockHeader = "7865fce77865fce71a320881d8b2fc914a2313b3bcfa5f4043a739dfb23c7c243bbb04e45aa3c2202871093968d412bd19283f6da2d836dc4f832ecc84226eb0e918e7f8abb1d4fe7865fce7",
				MidState = "9959ca6d11ea79dd3a74f79341d2474c209ad3eb94f1ecaa1be7b36ccde42948",
				RestHeader = "f8e718e9fed4b1abe7fc6578",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1078,
				BlockHeader = "8b54fcb38b54fcb373b91caf9ee8995bd695e3a47f4f697c94aa3bfea681dbe522248d8d86762d6d489841a637a2bd9493a1d6cbb0e0e8ba83fb3e4850587a63ded6fee19aa0534f8b54fcb3",
				MidState = "7f1afcb349c9f514a0a41f64e2f19a82b84687d60ba205c2b73d43f2869ea841",
				RestHeader = "e1fed6de4f53a09ab3fc548b",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1079,
				BlockHeader = "8adefc258adefc25bf0436b9870c87b95d7374d19a8eee9034f9e7ee328224948696de7dc9244bd57cbf46cc9914c2f820c40c1b67a63c70b26d79cf20b907775fbae0b001a948bc8adefc25",
				MidState = "de3d10347b0d9bfae5440a80cdc0f38eddac251aa858791d78e6220a3cdc78e3",
				RestHeader = "b0e0ba5fbc48a90125fcde8a",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1080,
				BlockHeader = "c2a696cbc2a696cbc64707ef0a3eac3074f82f866c96260ad1a59a0c67e4b3679af6eb48d38e9d27f0611ea2513d24398c4b28a4b4ceeb1bd3143b66f935b4bfe57bed987ecf33e0c2a696cb",
				MidState = "8386c45af0378f8459cafccbdade1fa350c2f437464a62b61c0a0c5564472e9b",
				RestHeader = "98ed7be5e033cf7ecb96a6c2",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1081,
				BlockHeader = "d877fe43d877fe43217481bf62116ca90155a4d151f07d1a793313680a6917c69b91e5166727a4f7f85fceb9004f9e9e2ed4b1492da76dd1159896ee053b0bd7258dc8ed4d0dc283d877fe43",
				MidState = "dbb25ecc2285d3de51d1d28871a1affc97bc35816447ebdc5cc862cf54348e2d",
				RestHeader = "edc88d2583c20d4d43fe77d8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1082,
				BlockHeader = "268c8bcb268c8bcb8a138afb5d1fddc50db77d083470316cd1cedabe7d2523029d3fe2124ff16ba2dd63bdfeff1b1241cb80774e1a573b06d42951c494834460d522be99bd8bb476268c8bcb",
				MidState = "630f162c87fd401702f10d8b1144e0d8fcf044f96979deaba9cf503c50c7bba0",
				RestHeader = "99be22d576b48bbdcb8b8c26",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1083,
				BlockHeader = "aaa6155aaaa6155ab6bdce0037e753692f3ee11119ba80dab304f01165cfd890b60066615559d1b17ad50dcf27a4fb7e136ab54e5cb966218ea6980018fd09ecf992b5b45e1bbfb3aaa6155a",
				MidState = "5ebfbe604a826b4fe1d342beed4cfe8ef1d72910105b65421efb757420010622",
				RestHeader = "b4b592f9b3bf1b5e5a15a6aa",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1084,
				BlockHeader = "740b51cf740b51cf05f06282ebd74480c8c89dc427526bdc0b301d6c1af2efebc52c3504924be8ce96a345799dbcda6173cbfb557515c7e3924c7ee0b80844d801aa1f84078fce94740b51cf",
				MidState = "f9a8fcda5bcdd16db6238bf3dfac005eab5489afe7af374e2a17a48c433b4cc7",
				RestHeader = "841faa0194ce8f07cf510b74",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1085,
				BlockHeader = "d84a88d5d84a88d5aa8f42c715c9d2c14fae4e60e2a0c7d7e0514f5d492eb112f03a9e2ce2c41901b3aa6f0b222c6dce1b0288b76e23716b1a1a0bff64c45b98f2bf06ca9aa5ab76d84a88d5",
				MidState = "29f273a09474aa4c3a746eed624d7ebf588e64a252ebbb07dc0310de78fc36c3",
				RestHeader = "ca06bff276aba59ad5884ad8",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1086,
				BlockHeader = "b355d5fdb355d5fd66697e29453d98834056d89dded30723e623aa1f4d5f2ffe857f2285c70e7e2185b983a322425f06eae0ac1cab7efb44651feaa8a29f80add0b1c3808dc4ec6eb355d5fd",
				MidState = "d74d91fe741a6ed061e347b24484318c484abee1491f2f8794eb4ede76bb39ff",
				RestHeader = "80c3b1d06eecc48dfdd555b3",
				Nonce = "a0000001"
			},
			new SelfTestData () {
				Index = 1087,
				BlockHeader = "e339c325e339c32507e01baadaa3d54f5b5ee7888a3fd951820161a6527b8d165c41d7fe0a0f1c001b52a16fcd25ff52731368c026e5fda78384227d9533ad3f3851275eb3bdd32be339c325",
				MidState = "cfa6f15b58b130501f7d5fd8e5da4bd4f13aebdd00ab8745300b98ab4cb4286e",
				RestHeader = "5e2751382bd3bdb325c339e3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1088,
				BlockHeader = "314e51ad314e51ad707f25e6d5b1466b67c1c0bf6ebf8da2da9c28fdc63799525eefd4fa400a931a3dfa01268529ccc4b0f3fc37ca057fd3269a10f98d10c70cb976016def11364e314e51ad",
				MidState = "917c66dcb925ec801e9a718a003c5d878d832697e4d1ca191d190a36f98e9abb",
				RestHeader = "6d0176b94e3611efad514e31",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1089,
				BlockHeader = "0fde0f1f0fde0f1f56ab028250d8ac428177dab551dd26164b74bf9f646c981c0f9c8ca77ecdbda517e9199b78b37345cbf4d5bc06630b863a874b5098ee60a18c31f35aee0d47270fde0f1f",
				MidState = "d8df7ddb7b94558f6c861f1bd68070d088fadf79fdeb86ffbcd597c3d31353af",
				RestHeader = "5af3318c27470dee1f0fde0f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1090,
				BlockHeader = "460e95e0460e95e024bb002673f9f2cecbcd106e47ea32e590ebeca7ba0e5bfce24e92c3ba2afa2cf4e5f691bcd8030987213b01250141eb1aedbc47917fc87bc1877e04c8983554460e95e0",
				MidState = "0fd06e85a729a5423f6b0c6294c360c8d57dc5429eb8de10bc9e8641dc90c3c0",
				RestHeader = "047e87c1543598c8e0950e46",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1091,
				BlockHeader = "4c0e94ac4c0e94acda9ae30b7462291085b6732da0de62569543a7e57bc38afeca1cae2ed04d5bdc79ea882f2ea018be6e0e616043dde34bab6783d92554476168702943c215fe5a4c0e94ac",
				MidState = "5fe9dd9d5126b0b932ea36628d99456d57ef721c05310ee645b20219b2cb9efe",
				RestHeader = "432970685afe15c2ac940e4c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1092,
				BlockHeader = "354c3b42354c3b421579ffbf668b7c64aaddfed24b5d7c4a9d15fbe9d5f7afb2d027a52096969dd6ef03fa632da7c6c95ee7330d57b94501703e7989cd92a17f9639b26751693c22354c3b42",
				MidState = "eb4d9609c11a9ac2e12fe0c9904964b1a0903481121bac600ec9c0b934bc2961",
				RestHeader = "67b23996223c6951423b4c35",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1093,
				BlockHeader = "d13baeb8d13baeb88a9ed2d289a5e843fbecd39a44cff701a9b8029f7f11051b723eac49febd9c5bce5bcb37de148bc2bcb0ce98a479447d4684d20b6b956782938cfe35c2ae6078d13baeb8",
				MidState = "6b58b83b519330cc0f2f37870230122e30f3bc764eee5fe8ca4e033881e55ae8",
				RestHeader = "35fe8c937860aec2b8ae3bd1",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1094,
				BlockHeader = "0f8d128f0f8d128fef2e2b97f6a0d2232cea53474599c493a428a850e90e58c4b10efd0ad2d597c4ca4b32bbd67cb191b67fd54b774e6dc1ab5b41454ba25ccf70b42903ae7833920f8d128f",
				MidState = "ec6b0851e4eb43fa4a9d414d7eb3cb29f4baea3fee625a64725424e2963a7666",
				RestHeader = "0329b470923378ae8f128d0f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1095,
				BlockHeader = "5cf1f8ef5cf1f8ef48ff0f7808d0bb44f874e00574ca100df77f959eac6deb9fb3e95928ee2a01cf28619d3a0e79f4e71ac7f354c81e2682f424496c9e9bd12ab53bc82790b837715cf1f8ef",
				MidState = "3aa884fa4b04e349aad4d0e5a247b368e787e9c1e6a5d43059cf834f97ad39a0",
				RestHeader = "27c83bb57137b890eff8f15c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1096,
				BlockHeader = "f81a12fef81a12fe1a3e21f0feeb9d7c113992733bc977b0a7b5224c92e50417b746521f10493a2f68b3f070ee17ca42a0a2b173d3616bb458eee95ff0873ceb73eaf25b4e2343b9f81a12fe",
				MidState = "5531e57655c7071550905a5648f6c37036bad6d8ab84a2e0212426faceb4639a",
				RestHeader = "5bf2ea73b943234efe121af8",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1097,
				BlockHeader = "94098574940985748f63f4032206095b6248663b343af267b2582a013dff5a80595d5a48a0a3eac404d844e9b077b34ed0301b34103d11b3b4be093bc85bb702756d174300f6483c94098574",
				MidState = "805b31b66ad531863de69fbea70aa4ff37045210c12a830bf5ed2a2ee83a8372",
				RestHeader = "43176d753c48f60074850994",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1098,
				BlockHeader = "cab21cd9cab21cd926ba01e2924c1c0a69e497a5a1d81e28244db5a8aee34e986940ce9321511831306bc0910533a25b1041ca008086407ea1044e84a86e2c870a3cfc0144538728cab21cd9",
				MidState = "e9a791f0de8e886c1a5b8ac39dbf772436449b761f25b2dc953e8c596e3d18af",
				RestHeader = "01fc3c0a28875344d91cb2ca",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1099,
				BlockHeader = "3f9763923f97639238ff8a0838c180f647e917cb4ac1cf6da71414ff2b2c77511a68ec715f36fff9147130a58378db0a6dd93e8ea862623c4354bb22f9328e57ae13b773e85d52ff3f976392",
				MidState = "d735052486e1f254bb5c12e673018ce5cfb5afd3671231a5a4c135d479f7232e",
				RestHeader = "73b713aeff525de89263973f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1100,
				BlockHeader = "8dabf0198dabf019a19e934433cff112534bf0022d4182beffafda559ee8848c1c17e96c55583dcfdd70da9f44846d52b444076609b0a173355f347ad551b9a2227d727e056fee638dabf019",
				MidState = "c3bc20cfb24303abedb1bf44fca00a5541fb5c3c20552387c4edf50e4d52745e",
				RestHeader = "7e727d2263ee6f0519f0ab8d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1101,
				BlockHeader = "8e2226b48e2226b4f731ee9b4b6f63ebf2bb8ec858721e18a644a3002b01eeb9d974c2e1788ba7f2a76531980c367f458b615bf98be9421097f608e190fcd1ca41efba29c08371878e2226b4",
				MidState = "109ccc23fcad83c7906c74cf577333c2e8df66289ea88b6b9cab1d8fdbe1b135",
				RestHeader = "29baef41877183c0b426228e",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1102,
				BlockHeader = "6d7029d86d7029d8d74eda54e03c5b5465fe5750df487ae70c2927113d2e64048791f91c5b7bfd3c59281195329b9242739e920c8fbad440a9f3b71b0c1d0d18d7f09a14400322086d7029d8",
				MidState = "8fd84939ca250b64f75f063f2967edb702c55a57c5c4c1492cfb1b9aeb8800e9",
				RestHeader = "149af0d708220340d829706d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1103,
				BlockHeader = "bb84b65fbb84b65f40ede390db4acc7071603087c2c82d3964c4ee67b0ea7040893ff617bd1a5df1261efd16369032a7559d7a180b24be2a8e057ec75e76fc6c4dea5a6c90abbcf7bb84b65f",
				MidState = "b6b69b1cdf30625159b4bd759c2de772c1f0e345ef275d091f03cddb5213045a",
				RestHeader = "6c5aea4df7bcab905fb684bb",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1104,
				BlockHeader = "5cf5e6bc5cf5e6bcd49451b5ed814c6c4a5f947073d2c0f3999b28dc47dfea4a7de671d79cdb62a29abf32fe1a98c2af34daf1b69204fddc77e2a65a37fabcc8b4c4a5411ae9d5f35cf5e6bc",
				MidState = "5cdbba4b04650366e6eefab26a88b168adc79ee4b74518f1360ab020cca68656",
				RestHeader = "41a5c4b4f3d5e91abce6f55c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1105,
				BlockHeader = "aa0a7344aa0a73443d345af1e88fbd8856c16da757527344f136ef33bb9bf68680946ed313f79f5c73e0c177e19f42f6bb3cc85b012f8d25c535cd207249867fa101bb0f8e3ee414aa0a7344",
				MidState = "cc685128a18cd4a6efe18994c160fcc49e5e8e4390a9b8754e897623c144455b",
				RestHeader = "0fbb01a114e43e8e44730aaa",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1106,
				BlockHeader = "639a8f91639a8f9157eccdea52d28f705460ec6666474201df6970d055e8e72dcd68ada2222e513e65934c91edd74db4b643e96b44ba88f803a1c6b8f1518152e9247a18400719b8639a8f91",
				MidState = "f2fad347fd98cd06c417147209d622b3fc67774310f414f4ba57781326022997",
				RestHeader = "187a24e9b8190740918f9a63",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1107,
				BlockHeader = "5d6154465d61544689b0936080efa3421f5b0ae8c7a56780c2451ace890a5cd2c4f4f2344f5be4ffbc29006b9c032ed728da8d74c61559ba44b6ecdf64f478e9cf0f4069de142b855d615446",
				MidState = "10492cbcab68bca47b0b39d5b91200478a76bc0f84f1848102763e91c6ecae44",
				RestHeader = "69400fcf852b14de4654615d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1108,
				BlockHeader = "c40af168c40af1687c3ff35025a243902e0af30cbe3696648e4070eaf3876c446ca61e7075ace4471c718ef70bed651d60a93c857bc5aed11222cfc5adb3d6dd3969f75688d3a310c40af168",
				MidState = "46613f20ea8a09a78c0a43435e5d8bf12affd7e492d0b365c5df53a7ddf671f3",
				RestHeader = "56f7693910a3d38868f10ac4",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1109,
				BlockHeader = "111e7ef0111e7ef0e5dffd8b21b0b4ac3a6ccc43a1b64ab5e6db3741664378806e541b6c563428d47cac1cd5754d99aebbd034bc329b345d97bf3a70bde453895d4371379fe8f778111e7ef0",
				MidState = "f5fdef6cf899b6aff19bbca79bc2aa61cb127864e34a69d31ea8f6e2afb12eeb",
				RestHeader = "3771435d78f7e89ff07e1e11",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1110,
				BlockHeader = "6f23dfa36f23dfa36b526997f8dda624f92783681baabb777b6658e2bd1456dfff0734f20820fe4be3e1649dbb5a3432e5f7e4f9c278b8602465ca3e0337d77f47d99a6982d43ecf6f23dfa3",
				MidState = "81335c4af451096379ecd917ffc70db76412ae9bd1d15e16a2c17d1796ae1213",
				RestHeader = "699ad947cf3ed482a3df236f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1111,
				BlockHeader = "bd376c2bbd376c2bd4f273d3f3eb1740068a5c9fff2a6ec9d3021f3931d0621b02b531edd5de940e832c42dffbfc62ad85f090d9d1aeecf80bbbbe4e3d1e967520f41906e60d3681bd376c2b",
				MidState = "792d334cdd45e814aa801500216ff930b822424730d0b51ce1c120c126c8c9a8",
				RestHeader = "0619f42081360de62b6c37bd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1112,
				BlockHeader = "8e29f4408e29f44070c1a421832303815db8d8bf6b51c11b0bd1b72f3b797cc7c20ccc912eb22b0cba51661a3e2860cf26c6e4eb26e9bcff611a530e597c0705467c8b4873550f118e29f440",
				MidState = "d33f16d08371a4dfe574d826b1c6303a0bcae3528d5dd35c299138acc91f7c6a",
				RestHeader = "488b7c46110f557340f4298e",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1113,
				BlockHeader = "f7ca7ba5f7ca7ba54564f74172e471f4805ddf1c2e8db607814a8671d676bb28d531d0b985898096580209798518964ba49a7eb385a0994cce625514c4a3da13c9b954649903c0e9f7ca7ba5",
				MidState = "3d6e9007468e0e9c61e984ffc91c61aa0260f3f9ae3b4726984a44db586717cc",
				RestHeader = "6454b9c9e9c00399a57bcaf7",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1114,
				BlockHeader = "54e0a52f54e0a52fc18f23fe1b94e0f127868aa8988f10840f8f8b572c2e77090df861ba98d3727983964088f41cc799e503241fd1667a6c5a61ea01d016a9dbe8c2772c5f1e5b3754e0a52f",
				MidState = "33474cb8f8c4c84c44d26471150471c3f0703bdd9aa437244e677f722bb65ac9",
				RestHeader = "2c77c2e8375b1e5f2fa5e054",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1115,
				BlockHeader = "840a33cc840a33ccebab5ab607a9d381e62bd07a338acc9605aebd5b140d4a459282e8500bc84bfcd3cad518bae25a0c367e86683deca0a924482eaa063a502cd5203634a22942f6840a33cc",
				MidState = "55a55e1f0af3fe9cf972c398a5fc0c137c74ef6fb71b61ca72d1a439769c8501",
				RestHeader = "343620d5f64229a2cc330a84",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1116,
				BlockHeader = "d92f3dacd92f3dac3bcfe8227a942edc3e635f25bb9998cca850044acb706d7e9ad90d408e17601e2ab0868be2e231f212c37f2c9af626bf06e148c2b10b5a0bd9d3900ff879dc01d92f3dac",
				MidState = "15fc3fc6288f16336ef054cc31a9eecffd026891c017e9e20da0d997d69abf56",
				RestHeader = "0f90d3d901dc79f8ac3d2fd9",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1117,
				BlockHeader = "2743cb332743cb33a46ef15e75a29ff84bc5385c9e184c1d00ebcaa13f2c79ba9c870a3be92df84f63c180cba02f8fd2661eed97d510c3027315b019389a9705b0918c7deb604a7d2743cb33",
				MidState = "825351520f6daf0abf3f17fb36ba06f29f4c739f5ee60c91eea88351d5ebe732",
				RestHeader = "7d8c91b07d4a60eb33cb4327",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1118,
				BlockHeader = "745858bb745858bb0d0efa9a71b01014572811938298ff6f588691f7b2e885f69e3507374af8671d940421021cbaa323818ce1727ff0964e71ffc0a0c546c5321bae904760f12b8d745858bb",
				MidState = "cc4e03fd460e35e569c39995fd62a796207ae185cc6efc98b41ac171fe9f52b8",
				RestHeader = "4790ae1b8d2bf160bb585874",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1119,
				BlockHeader = "0110e0830110e0835ad7f2169df89416ef8db8b0537ec10fa7f14cd98925d8ada8cb8d769ee608ac5a85ffaf25d521bc596ec82dff05bd82c0192940c384cefd1ef08b1d8ea4ee170110e083",
				MidState = "854b2663515c8e5c072fac9755e8b6a9e3becb5bf82ca4f81f0ade4e33e17613",
				RestHeader = "1d8bf01e17eea48e83e01001",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1120,
				BlockHeader = "9d39fa929d39fa922c16048e9413764f08516a1e1b7d28b25728d986709df025ac28876d53dbd7b8d57435221bfeae3bdb4c9f7053eb0bb4923729d8f21864f5a0f620258ceb9f6c9d39fa92",
				MidState = "9ed1a222a9a91b3d07039a50779a2fe0c713a3442390d3a117dcfa1a0aebe5b9",
				RestHeader = "2520f6a06c9feb8c92fa399d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1121,
				BlockHeader = "4de219bc4de219bc7dc670ba1e874f3179bc73921eabc516fa70055017312d0509d66c14c1d41d96c93594f3606f2f853d56b05e1e447cc3f64f021dda159cea5408b748e4b90e0d4de219bc",
				MidState = "68e4ad114b7ad41447b629a6b8cb4749d75ccfdb668533c306fd3677e6d27ced",
				RestHeader = "48b708540d0eb9e4bc19e24d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1122,
				BlockHeader = "e275a389e275a3894f41e2f8a6a850feae9338b9e089a3d4cef7c9b17f208a036c73cafdcd5dd530b31a29735024a710b3d33bafe2c997afb287f1961e634c72c044bb7eb7baf645e275a389",
				MidState = "9be8bce28061efed31f5f06c6bd95c05a613e61bb5ca44d8704267fb93c2d874",
				RestHeader = "7ebb44c045f6bab789a375e2",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1123,
				BlockHeader = "be963cf7be963cf7ef23f34a9b1ec8062ad5b0d0fc97a0144e6380c0a22325ee2f884fddb41fd4e7988bc414ecc9bfba9abf0153655eab109e6c70266fa502a5c5e7514182303ea1be963cf7",
				MidState = "8b12ad20b8a1b85ea42f8049f507e3fd80d35708730d0e6070a15066444da95e",
				RestHeader = "4151e7c5a13e3082f73c96be",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1124,
				BlockHeader = "ccb34a20ccb34a208a1ffeab97d2a452d3bac35d8b08bdc8d6c91db5d953afb7737ec1efa364eb5c1ac8e29a57256c069c81537344bc2fecf899ed2a968b5199e92fa15c8695e06cccb34a20",
				MidState = "5e3b05f5d5b4fcaa1f640840a7c6330ed3804bbce352f3c7db6c7ac3d579a19e",
				RestHeader = "5ca12fe96ce09586204ab3cc",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1125,
				BlockHeader = "5918331259183312f2239dabe82db986c4e7dd5158c36908f604baf8ae22ea60eb915065dff71135bfaf4f3a7b00678ed4c365e82f0f0b85c7dfa14024a47fc10b6476293555998459183312",
				MidState = "19ebccb8957364e2824de3407428b3551caeb7d3023736679bd02b51334bce1d",
				RestHeader = "2976640b8499553512331859",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1126,
				BlockHeader = "03328fb303328fb3f6729c435233b147491195a42a0f5208f289b668730c9fc9b77590fb593d49483fa38c6c6ef41861a9c24cc8037455195001c1e9d676bbe6e72bfd4f0cea6c6803328fb3",
				MidState = "dd5d39fe0248e0f91dccda59c8c5e87286d70dcf7ec366398b462c72a7851bb8",
				RestHeader = "4ffd2be7686cea0cb38f3203",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1127,
				BlockHeader = "50471c3a50471c3a5f11a67f4d41226355736edb0e8e055a4a247cbfe7c8ab05b9238df6dc0945574612cd71386bc4a2725a426ba982bd9e2c4035686b3be3f5fc1438300c0f0bcc50471c3a",
				MidState = "ae5f3a3c9ab56604706ec529bf5d9e66b26659d8d827acc097b0aa1f948c838e",
				RestHeader = "303814fccc0b0f0c3a1c4750",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1128,
				BlockHeader = "c4734988c473498870ad557c8e0ffda3f33d84aa2fb7e36ab789fd8778811496237fcecbec855779b5d8da99b5e25803d429db9b3e9b7976b1a9811cd242cb469671912d1e4ca195c4734988",
				MidState = "a91f5cf603a61f92086816a3940f6608e2c4a0bb5f0682fcb157123518480ef6",
				RestHeader = "2d91719695a14c1e884973c4",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1129,
				BlockHeader = "95a333fe95a333fec943f05e7acfc9f5490cd0e6caa19a60dd47325bc1b647c1e689e4f88962d872724944931e23894a52fe7a278b71d0032d0d4da44ef4f666f304f341af51847195a333fe",
				MidState = "5b58e30e0a91a5f6cfb7fb75edb308c77a52e1a8b3837f96d102e1fb43408584",
				RestHeader = "41f304f3718451affe33a395",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1130,
				BlockHeader = "dec87dfddec87dfdffed93a30a488cbb4cb016f65c77f7845718b430ae23157c174cfb505ca334a16c3d14406ba6b155e69544ef11bf11c238088b757172909e0236b65afdec48aedec87dfd",
				MidState = "ca166652b6c94997dfaca6b82fc55d9759c93f3bce12093b5cd7ef0e4e5d80c4",
				RestHeader = "5ab63602ae48ecfdfd7dc8de",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1131,
				BlockHeader = "723c3ebd723c3ebd11fc36aca5448bc781fc64a6cd8e24cbe62765772946545e5e036e8be03b673a82a4f82fe88576297ec467d462eade7adca7a1206bb53fad5a756f7aa1edfe2e723c3ebd",
				MidState = "e0fdeecdcb4cdb40e9b2b1aef6e4a8162423a7221e744754c32df0b4dbccb5a9",
				RestHeader = "7a6f755a2efeeda1bd3e3c72",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1132,
				BlockHeader = "4d5dd72b4d5dd72bb1de47ff9aba02cffd3eddbdea9c210b67921c864c49ef492118f26bd9d94412f98893fcc503e9342d40dcb8498e20d9435f96cd432911888ce64666285117664d5dd72b",
				MidState = "ce37f13eb1a45b575ba97f8110a86be685e272d3e063c15fbcbb4000cfde5665",
				RestHeader = "6646e68c661751282bd75d4d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1133,
				BlockHeader = "0d6559cc0d6559cce33b49249b606dff9ac11613958e8a6e965df2240fbe6cd6626068826a0bd8801070441aa3de1a2a52d25df0e9532370a3c11d7b70d5873ef9f42d41ee6e6e900d6559cc",
				MidState = "caa9640ef9dfa29573609c8bbc68d6c785c9c0aadd08245d3ae2b89d11387111",
				RestHeader = "412df4f9906e6eeecc59650d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1134,
				BlockHeader = "e986f23ae986f23a831d5a7690d6e40716028f2bb19c88ae17c9a93332c108c12575ec622db54e18b39a0539780baa2c5ce0e9ba45068298198aa8cdb9e3837fff9e084c0b24d6c4e986f23a",
				MidState = "0f3e20133f9d17f5fa891ae3190184754951df51b4ff33f11a069b53a870c650",
				RestHeader = "4c089effc4d6240b3af286e9",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1135,
				BlockHeader = "7edc52ff7edc52ff0a2f7711537739b8e0f97b38751fa648c5dd6f953e6764349e11cf721fdd207882cf4be086476c3fd36fc8dcfcab15f340cc3b5794c6c4cd9d95af67eab4c6597edc52ff",
				MidState = "e09401fe632d02172b6c4ea4668ced04a8af4df6801ed7fad1e06a2e2461031e",
				RestHeader = "67af959d59c6b4eaff52dc7e",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1136,
				BlockHeader = "5ad53b4a5ad53b4a7f62b9113286d25a560cfb13df8dec22ae832bc63525a931fc5a451e4ba6786e61faf7b76441fc4c198cd7c5bc1b1e8954be7eeb088562e9d1350d47e213b30a5ad53b4a",
				MidState = "dab77edf9f1175d4edb640ee7943cc609746186394b50f2fe53695793dacb907",
				RestHeader = "470d35d10ab313e24a3bd55a",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1137,
				BlockHeader = "c3fb6d5bc3fb6d5b66ab1eada59a4f9e42caf62d60b7c6f446a0ce06cd3d8c0438dc17b89dfe37032d32c189f0d37491b6b6aea8064ecacb1b456d15bbe129fca90e185449b7cdf5c3fb6d5b",
				MidState = "e7c8d9a3153e13ede81ac0547dace88e5e6c9d9f87f942884fc6ed7109e1a881",
				RestHeader = "54180ea9f5cdb7495b6dfbc3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1138,
				BlockHeader = "0e49dad50e49dad59fc17b3d09b26b0aa9a2a40d432c09b00b49b8335f9a7443b583053195b8734f0b21a9004a24710e8c6f3d6ec2b9e11d64d7a72fec55bc9d0fd5c965bab731ed0e49dad5",
				MidState = "e8d98bfcbd4198893b90404dea8906368e18c032c272d862ae1c975b43256963",
				RestHeader = "65c9d50fed31b7bad5da490e",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1139,
				BlockHeader = "2cbbfb4d2cbbfb4d7abad3b33d8ba298e3c84fe475554ba96867ed03f5e0cc3a466189a9c9d2dbbc0429575510af7b3b926196f2d6ca8e0dbe7085e06837797fb021960d35d4563e2cbbfb4d",
				MidState = "18e481b8ea998b2ea69f447866d027ca3c46d79c43db6f63d51e065074ec5153",
				RestHeader = "0d9621b03e56d4354dfbbb2c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1140,
				BlockHeader = "c7e4155cc7e4155c4cf9e52b34a784d0fc8c01523c55b14b189e7ab1dc58e4b14abe82a01d995ec77422a7440771f31dc8e6daf61b1861e634c96510aaeba20674a672233d50cb69c7e4155c",
				MidState = "66326a7d49eed2182212372c9cbf993b29680ca756a484bad58a40bc2ec12912",
				RestHeader = "2372a67469cb503d5c15e4c7",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1141,
				BlockHeader = "c255e98dc255e98d800813b7fdd8818b3501ff1bc1338a8e13b5db5f3c13519052f79d78c79654fd73fb4351c819ea4aa7bc05f8008ad2c83d7d8d2aa5f9ce26814d785a2ff73da8c255e98d",
				MidState = "16dd1fd345df79cd365e8433577c23a30228c0bfc567a8f2edb45ef91fc779a0",
				RestHeader = "5a784d81a83df72f8de955c2",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1142,
				BlockHeader = "1069761410697614e9a71cf3f8e6f2a74263d852a5b33ee06b50a1b6afcf5dcb54a59a73ea9b621c647b44859a21a0e657598e8b51b9034ce836904aa68e0b460e9102213d2fd3fc10697614",
				MidState = "353e75355e8677432e8a4bc66f14b2bd5c7375d8cdc795591c35f40c90ca4327",
				RestHeader = "2102910efcd32f3d14766910",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1143,
				BlockHeader = "6804141d6804141dd7440705ebe2c97659b1afd99cacc1145f44283ddfd56c88c007445ee7e32f90d17fc77870ba74ffe3ee88f9a4b7fd3f0e530e2df87330bf18ee9356b02d9aa26804141d",
				MidState = "1c907ffbad6232df8cb6a6feb2ca20217da06bb96745d6678fce9029f99446c7",
				RestHeader = "5693ee18a29a2db01d140468",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1144,
				BlockHeader = "bd3b18c0bd3b18c02953a612b0ab8f83e0e2eafe2da046e0914b228a717961fbbebb21316270bbf2adb9218c5b1a0cbf3738b5a869fc119a557f6c65a97a24f3bb4a43097ba068fdbd3b18c0",
				MidState = "3522ea24332bfb06c6fa010a693d871cb00194b7450b391ef12041e2e98c025a",
				RestHeader = "09434abbfd68a07bc0183bbd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1145,
				BlockHeader = "68d79eae68d79eae605fc80c9e9a308ea70a58682d5c0bd0c66363321e9a06d315b0fe73ff78a898c88dcb4e1c8b8a3619b522d905a96be9726a8390c231ff53c9950c5e9e7e927068d79eae",
				MidState = "62f7772dbc606afcb4754ec64e24f3a84b05326925062c313d96ae37f0d58cab",
				RestHeader = "5e0c95c970927e9eae9ed768",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1146,
				BlockHeader = "8fa9d0c98fa9d0c90aca17bd8e260856a748900cc51855d7b9aec2f2b64ba875a3c9f865d2ecffec6a72e131c5321090ee899e76405862814ee4bacc406c6a93064485410bbc721d8fa9d0c9",
				MidState = "63c10263e18ab528e27421762625ebbea5c862018e169b721a3464f0de3a68a5",
				RestHeader = "418544061d72bc0bc9d0a98f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1147,
				BlockHeader = "5c0ee14e5c0ee14e5e8a18d8204fda58552f38e7d02a054b8ba1a284965842a394cb67ca4fb862d877cb2e6c0c2c0e339f9a3ab468d2b7859c40e545e04422a3e5e97e0ab338c3935c0ee14e",
				MidState = "ceff192cd0aadf6d6ed702aed21850d54ceaef395b675e34e5f91c53ff432035",
				RestHeader = "0a7ee9e593c338b34ee10e5c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1148,
				BlockHeader = "f737fb5df737fb5d30c92a50176bbc906ef3ea55982a6cee3bd82f317ccf5b1b982761c1612f69cedd376c642a489c6b0466219175b9da4ecf43f01d77299aac2848d5421c45091af737fb5d",
				MidState = "bafe8786d351e5eb453fbce25b2c12b962cb14d65e79b07e0e02a6b16f323956",
				RestHeader = "42d548281a09451c5dfb37f7",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1149,
				BlockHeader = "bb647471bb647471b386540f3d9ea6700215553fd972c8823ffecd4bf3396c417e7e4e0675a48c49dc5fc9ca38b0e2c980c8df4c3d32fdaec41ebe94590cd4c47b5ad612a2c2ecfabb647471",
				MidState = "7823829b9bb15fe53dada718280c78e3fbd3a734265f3fe03ea9843c4279ac70",
				RestHeader = "12d65a7bfaecc2a2717464bb",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1150,
				BlockHeader = "e1f83aa7e1f83aa7b4a1f3b724494d5c8f245307e6f026bf8b9adde6b9983b0d62c71e74626542c965a4d9b0d6f15f281cd748b991b5a60d37df218ba2987a514b39033a42010bfbe1f83aa7",
				MidState = "56593a3268c98e7802b3a3579d56b26a53be9127e9e547d4e90f646470bed695",
				RestHeader = "3a03394bfb0b0142a73af8e1",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1151,
				BlockHeader = "a089bbbaa089bbba324a0fe50e12a7ebb3851d8bab2115365ab55f74090d01490780e47b1b9fcff5a6a7ecc395233697a239d5d3c242618850c2c300b395011168b5cd7756be2cf3a089bbba",
				MidState = "9310ad055f44358243adadf9ba1d71b196b024f82cc9e60ab29e240c0038126a",
				RestHeader = "77cdb568f32cbe56babb89a0",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1152,
				BlockHeader = "ac2b11d9ac2b11d98a64aab101a7da9c1ad096dfe094c258baa6bab79338e369065ef40cfe51f4c2a6dd69203c06ce46cf7e7baf5c5dcd23942749154cf4a68ab756d407fbbf9864ac2b11d9",
				MidState = "1e9ff5398c900a5826fb947a49b0c4a42ed4e1b42a8340a1ac90c5e141452cbc",
				RestHeader = "07d456b76498bffbd9112bac",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1153,
				BlockHeader = "7511db7d7511db7d3607a46f23f1b8613bd5c6e234f8c611a103907c992afcab3202d97482035104c4796740d4ab03f306db2415040f9b35110cb02021b532436ca21100e2b07a757511db7d",
				MidState = "7fbb0cfeb0ab350f158f95ff13437746255eb3d6b97f1cc07218f6ea95140b8e",
				RestHeader = "0011a26c757ab0e27ddb1175",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1154,
				BlockHeader = "9b6d72039b6d7203dbcfd2797cbfceeb2fca77ba443b8475102be0dfb04e8d8e93a58f24eb78b4f87c0f42f930fd6d4c7e0f7644b399bac362a25aeeb0e7931133e8b66df55a96969b6d7203",
				MidState = "cc3d69c70eaa8a14be387fd76ad6d967e06c328732d10bf966a7cd8658068824",
				RestHeader = "6db6e83396965af503726d9b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1155,
				BlockHeader = "5b75f3a55b75f3a50d2cd49f7e65391bcc4db010ef2deed740f7b67d74c30b1bd5ec043a749a138e1ccafbd63fac2ca294e161012c633554319e9c276ad34d3631cba850b697e6d05b75f3a5",
				MidState = "6c767bf9cf487581a269f7988f89b87de93ae3d4273eda0bfb9b1d91adb49c1a",
				RestHeader = "50a8cb31d0e697b6a5f3755b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1156,
				BlockHeader = "249ca118249ca11863e64d2eedb72eb28e1eaba3a221296c9621a027d92b7b598affed36cacc4fabfc65ac6235115e3ff5421ccb84881890309838912de3783ee649773482c6a89c249ca118",
				MidState = "79efe9fe7054a217be1a63daa3a7e91dd4f042959d4c8a0facd5cb6ee8b211aa",
				RestHeader = "347749e69ca8c68218a19c24",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1157,
				BlockHeader = "56db0d9456db0d945bccd08507023e5c1e4fb859e71fdd85f77b1bfee67f9fc404d64c3fb2e4eea1dcb261da495b929a10db5c66c9168b86c152c01688546ed50efd5c1659caab8256db0d94",
				MidState = "103aa6ff01fee762b7844850dda1e41066d1a0174c73606c55ea40aabd26ab3a",
				RestHeader = "165cfd0e82abca59940ddb56",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1158,
				BlockHeader = "a4f09b1ca4f09b1cc46cd9c10210af782ab19190cb9f91d64f17e255593bab000684493ada86e73a3666a34f34e413ff7459aca224e2ffda922c36fe78c5fdd53260070a2f401189a4f09b1c",
				MidState = "2617fa1b7e88ecb4e472dea1728cde76d4ee42ec3b05bdda79b2e54ee45883ae",
				RestHeader = "0a0760328911402f1c9bf0a4",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1159,
				BlockHeader = "eb95d099eb95d0995631414e616639972bb8e507884c269799aa147eec90cc8beca0df4c9fdb93d97b121b74376a8cf7281e583540e22645638f53fb152a03440695d308fcf1fc3deb95d099",
				MidState = "739d47efc101cefd232d4cc37fb9556f4f46d2cf0a19b89951a8a94f02762000",
				RestHeader = "08d395063dfcf1fc99d095eb",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1160,
				BlockHeader = "e6e8162ee6e8162ec28f370c6b27718920bd902a33b6a4639eb4e373cc9ec85bce59ffdaed9057f055ab522887a4c5a244d3426e6661676d9d056c4f100bc2e121fcba656de3ab3ce6e8162e",
				MidState = "61ad88c8d3c711343b63b689ee4292d0683832e6aaf7a3f5ff7d5d19213ec7ba",
				RestHeader = "65bafc213cabe36d2e16e8e6",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1161,
				BlockHeader = "8111303d8111303d94ce4a84614253c139814297fbb50b064eea7020b216e0d3d2b5f8d14651935c4a83eb874101bad5a7d7c837f0ac79d28de31de9c9ac231058577f6bbb92495c8111303d",
				MidState = "84a7f39a5c9d81c4941129ca5277d4ac8839df7e46172c8a5204c4c94c42872d",
				RestHeader = "6b7f57585c4992bb3d301181",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1162,
				BlockHeader = "600d35fd600d35fd4840db71b2b2d2c0e4e7d5e037a836d0dce160a5d64bbeba48a18dbb005b9a360e283b0fce39bf16dc0d8769d2666bae175f21c1d9e90fc501fe3d1d5a1c7df1600d35fd",
				MidState = "09e7517f92f9296dde8b36f71b25f37acd1aa7ebccb40bd47065ead75034b4d5",
				RestHeader = "1d3dfe01f17d1c5afd350d60",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1163,
				BlockHeader = "3b2ece6b3b2ece6be822ecc4a7284ac860284ef753b633105d4c17b4fa4f59a50bb6119bb3d1ba3bd2fd1a24d892bf66a93f623f58b02783fd4bbe2076de40e221479d172c6e387c3b2ece6b",
				MidState = "b3710224671565729fcb9c326d74269705cc949bec83e1caf83705cbedf44616",
				RestHeader = "179d47217c386e2c6bce2e3b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1164,
				BlockHeader = "608e55c6608e55c68f74b95d89402b41b2552239079d8a9a5cb3f4962cbcf4d32c97796bf3a393fef288b6876e7f91f587474378365fbadefde78661943f7bbc249aa2420df1a92a608e55c6",
				MidState = "875b60c49adf02ae8e43af071d4f14c9e8d9ae1abb66aa89c06b66fb6c410951",
				RestHeader = "42a29a242aa9f10dc6558e60",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1165,
				BlockHeader = "f298145bf298145b8aae46ba83012dddc165b369f51deb00e355cc22e03a815b6f82faee3faa58ee1a7887b1cfa66a8267e7380dea0c1907bf8a3e9af9afa8a2a8aa266f92f55d90f298145b",
				MidState = "ad70cd5bc6b7221a8ca30000f8f0071ca98949384d363d51d1784267d8975278",
				RestHeader = "6f26aaa8905df5925b1498f2",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1166,
				BlockHeader = "b2a095fcb2a095fcbd0b48df84a6970e5de8ecbfa00f54631320a2c0a3aefee7b0ca7005abc6bcd8aad13be0fded409c76ccb8e2d77fb419c2dffc3df91030631860132d70cee471b2a095fc",
				MidState = "9d0c8db14e6ba14b90266a9c22e4b8b5f43918f5c24cd91dcc7a1ce67516b0d3",
				RestHeader = "2d13601871e4ce70fc95a0b2",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1167,
				BlockHeader = "73767c4e73767c4ed319f81daff63d30c53f7d47f39c4feed2e8b23a72855a9a66aec2ab7093b44f17b9b907ad39fa852bcd62b91064806c474ffc6a049c3ff484fda955cc196e0273767c4e",
				MidState = "e677e0f5e293a4d87592689ad5d15504ba90c3c9a5268e5624ebefafc68e633d",
				RestHeader = "55a9fd84026e19cc4e7c7673",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1168,
				BlockHeader = "59e419a959e419a9eccee591493a542d18dd2f02f74d40dc6e8a5ece22028e64c01897e0acf1ef0c00169ea9bc4850eb654f3f97d67ddf32040860348595fadb6ca07c24b38c426359e419a9",
				MidState = "5623991c9f2b33725e13c0c7faa938c9f49ce901338ffb8f8de24dc38581b5f7",
				RestHeader = "247ca06c63428cb3a919e459",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1169,
				BlockHeader = "090a3b27090a3b274774dfbb37db9f3320647262f9eb4d1bed53eb35f28e6973ea1a4af916a140d5c1928d7930187407c5d8e021199ba970f00bca6d05a7255792f4115099e36817090a3b27",
				MidState = "0ef766434300d764efd6e0b1cbaab1b7bba20dd5f1b3bf59cd27992f98a125ce",
				RestHeader = "5011f4921768e399273b0a09",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1170,
				BlockHeader = "561ec8af561ec8afb013e8f633e9104f2dc64b99dd6a016c45eeb28c654a76afecc847f5ae62a060c343057fa1f7c71f366fc0276e5adb369b57d230fd31c9f2dfd01d089839ef72561ec8af",
				MidState = "fc1b415100956c2d8a013ba8a5dc7a8bd70a27fb2bbe0d24839a8fd1772683b0",
				RestHeader = "081dd0df72ef3998afc81e56",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1171,
				BlockHeader = "9465c96e9465c96e233cef5f9bd02e91012a8bfe651ec97b9f02c63778cc78462460ab6afc3cb3e1304accc7442c28ec35f9a731b45dfaa7f1c01cc4e3a0631989d97876a67625c59465c96e",
				MidState = "58fba7404517076e7c820df96622a1815a39299adcd8d93aa6d49a5eb2c04e26",
				RestHeader = "7678d989c52576a66ec96594",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1172,
				BlockHeader = "6f4fd2536f4fd2534f65ed3603b538dc573b46ff5240882c17a31edc36e4eed9c63d95bc744abdaed18c8f00d259a4f924771a70b757818c3ba84312c7079c6c891a2f1d5dee946b6f4fd253",
				MidState = "adffe83cefd4fca696c2421de965b15b9f6dfee708b2bb688770bbf449311172",
				RestHeader = "1d2f1a896b94ee5d53d24f6f",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1173,
				BlockHeader = "e69985cfe69985cfc1861000ee47911dec417184354dec0f484562984060a23c8dae13946544602d6a401777bb652d9d81e9f022404a560c52c1436fd93cd012fef0b92e6adbf84de69985cf",
				MidState = "2b45d2a7fdf3b7af1df494c9b6a20a1eb3c363fa76e3afdc1bc31a58f16b54c5",
				RestHeader = "2eb9f0fe4df8db6acf8599e6",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1174,
				BlockHeader = "802d46c1802d46c1bb3e7a130ed8ff226ec1906c81879ebee9a080164b464630efcc8d969e6c477d6acdc9bf5917369fcb0344762fd8d9212be3491ba62d8adf3aa3e6694ee95628802d46c1",
				MidState = "46cbb4d9d8664d419a03c54e227bffa0cdebe3cb67ffa7c14929b3d2db27fca9",
				RestHeader = "69e6a33a2856e94ec1462d80",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1175,
				BlockHeader = "0aa4ee050aa4ee052d70934f835d4010d2cd3409e1d5ab934ad54e568d35a70467d8e58b2389718303c6b8856c429a8560fedaa4d67e00cc161169b954e3b418f46cad7093fde3be0aa4ee05",
				MidState = "ddadebfa9bc1c76ce36aeb35d9ecff687367810c9d6fedbdbd7a0f3ee4b672fa",
				RestHeader = "70ad6cf4bee3fd9305eea40a",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1176,
				BlockHeader = "66d277c566d277c5df579d74b617f7e5c651761fdfbc1fce3e4530959e705f763cf5e656b634bf145d32be33eda631e2669726e25fb8da74edb96cb263bfc5cc08cc8701fc99a69a66d277c5",
				MidState = "846715edaec63a06e3a20e4759e578aca99b0534f1294f5d765fb6899abcc8ba",
				RestHeader = "0187cc089aa699fcc577d266",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1177,
				BlockHeader = "0c41be860c41be868c7e13f1571a28fe48365a4c5ca41e781b07688ee2bb227321968aa87a9e42e325a5d6ac147d0c4a61e6cea7733aa18f6d268fb0e8dd9ecea30652633ad849820c41be86",
				MidState = "3ad3608a1c4e827e9e5688ed96cd928c122d6a75b7f62448411f72867da3a54e",
				RestHeader = "635206a38249d83a86be410c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1178,
				BlockHeader = "c8cba4d4c8cba4d47544ed645d6dd87873dd8dd04b1aa65a7629e442228fd36df8789262f730887558f93de35c9fe596b7469cac5f7c94bd17ed80f186c9f56bc20fef32a1c8d5a9c8cba4d4",
				MidState = "dfb6569b9b72eb10efbeab200a175338ca95db5687f22852d192d219a27f1b19",
				RestHeader = "32ef0fc2a9d5c8a1d4a4cbc8",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1179,
				BlockHeader = "f101cacaf101caca7ec507f24ef1c09cfc81df1e4ba757ec4f2f62a8b94e7a94bd3b143ee5de914eb2ba9c0b57769564d5534f8b2870b7b5673d422eb6dfe8a5c301596021d56e93f101caca",
				MidState = "9433716195a9c7995db851449c932b25baa0c9ad69ccbba58a24a30875778547",
				RestHeader = "605901c3936ed521caca01f1",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1180,
				BlockHeader = "92b2710392b2710339a22af51de9baebac1194e0ed2528e34749514f8a1681362ace90a5cdc0c4c8568637560775a6e53ec36c174849a8814aefc5f31e853a4613113311fb5e3e5e92b27103",
				MidState = "4ea83e5d9bbbda4f1b9542f89328af23d16c8ab52697b56b674b6e116cb009c2",
				RestHeader = "113311135e3e5efb0371b292",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1181,
				BlockHeader = "c6dfaeaec6dfaeae8dc0274be493feabe6fbc629e5ce03f8d25ba32ea4111f8d8e780c658066db2ef42e7c0eac54cf298b7e6f209d356ac0bbf03b45f05cb493bf54ec7d0036e4c8c6dfaeae",
				MidState = "6913b82a27ef6773352bf6daaddbc365bda9fd8f59dcf82ea18823a1ba7f08c1",
				RestHeader = "7dec54bfc8e43600aeaedfc6",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1182,
				BlockHeader = "6108c8bd6108c8bd5fff3ac2dbafe1e3ffc07897accd6a9b829130dc8a88380593d5055ce1fad36eaa2dddbb80b1684f734f142ff91a96fd881bb8a9373045553d637342687a7ebc6108c8bd",
				MidState = "d03671b8593376f523942544ff7f45caf54f86d28e9a967c44c753886fa210f0",
				RestHeader = "4273633dbc7e7a68bdc80861",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1183,
				BlockHeader = "a37e8901a37e8901c4ff8171ec559ad8943612dea13619a62eebe2bb9beec5d9e6cc3919b818bd40f6f424916ad2d3572b0e45cc644efd204bed3c092495353edc2e1f053d9c9deaa37e8901",
				MidState = "1828bb5bcf7bd3df2be461c7540cc8476a5b199db2e65997f02a69f4ce9722d7",
				RestHeader = "051f2edcea9d9c3d01897ea3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1184,
				BlockHeader = "f0931688f09316882d9f8aade7630bf4a198eb1485b6ccf88686a9120eaad215e87a36143d700aa0b4d4c3dc728a130e7dfb251058dd58ba8c8195867e3478201539015ce122c7b8f0931688",
				MidState = "6877141b38e68e697e852be1cf8651118e800c5d4297880f13045fa7ba2d78b6",
				RestHeader = "5c013915b8c722e1881693f0",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1185,
				BlockHeader = "2a279ff42a279ff416cd3a354f19856655546d452d084988eb10d04cc14abae991d6485f9eb3fca08694a123040f661cd34f9dafc731e236ebc727a983204d921266e333dfae06d12a279ff4",
				MidState = "99d778fbe3de6d1ab9d8ce2a841147b142c6fb4aaf671156426f648046b6af61",
				RestHeader = "33e36612d106aedff49f272a",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1186,
				BlockHeader = "b3797c67b3797c677452db36892762129bdbcf5223dfb0f68bfc7de6607415c3bc389444b18078b006398c49fee666a356d41a74b0a497ab897683483d9f89d4586aa637e2a1a5d7b3797c67",
				MidState = "e3c87ca3816e41b3ec00e524dfff6b8ea39e0f1ed6028717c985640cf704afea",
				RestHeader = "37a66a58d7a5a1e2677c79b3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1187,
				BlockHeader = "f00452caf00452ca951d00ffc75f62156e41fdb2aec84806e41c33f6edb479a03d7dad6e9ba13c2cf207e3bb44f1750054ed295bbee60a23217dc29b1f30e04933973d71c2124c83f00452ca",
				MidState = "fa371fe792d5bbfea2c696bb459299d2e6b959c243e427fa179ac3f4f267ba04",
				RestHeader = "713d9733834c12c2ca5204f0",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1188,
				BlockHeader = "c30e30a9c30e30a9007d4db4caf25066c598d16b9645e0724f46da80a9f769a2d4c0fa94e24e72cb45c3cb55210cff36440a731e464023f6cdadacec204235ee8bb8c34121a26f57c30e30a9",
				MidState = "ef229c1a6e952b6e0ad6ca165b5ff9714223e2307b842300d6010c47a522042c",
				RestHeader = "41c3b88b576fa221a9300ec3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1189,
				BlockHeader = "1122bd301122bd30691d56f0c500c182d1fbaaa17ac494c4a7e1a1d71db375ddd66ef6900d997e2bb69fab9f2efa969e876b106839cf2842ec9a16e7bb47a4a399adbd23a78388cb1122bd30",
				MidState = "7a0f68f3f68e9b4538f28b6f8198aa065db4e2c61d84dd496c50ee8857a8419a",
				RestHeader = "23bdad99cb8883a730bd2211",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1190,
				BlockHeader = "ea9bf838ea9bf838430467c6e4b508db136f7bfbb4ac9c54d14c8210f81e10b45acb929d6390538c3d4313a2afd4eb5ee269f0be4ffdd5ca1dbcbd57346c3e118aea5228de6e8a9dea9bf838",
				MidState = "d61043294e2757662329c8dc61941aec239cdd04c9a67fde01f5275f665cdf5b",
				RestHeader = "2852ea8a9d8a6ede38f89bea",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1191,
				BlockHeader = "86c4124886c4124815437a3edad0ea132c342d697bab03f7818210bede96282c5e278c943d920324a7af76cdc8b062096ca7d7976a7c3d3769ccb49a9fa3c54b88f05a6aeaccba4786c41248",
				MidState = "6f6d359b5adce7f8a908bb521db319533606353c79fed05e9eb1df7626deb6e1",
				RestHeader = "6a5af08847baccea4812c486",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1192,
				BlockHeader = "d3d99fcfd3d99fcf7ee28379d5de5b2f389606a05f2bb648d91dd6145152356860d5898ff7f02dc8a3d103e6c6fe348e830d18e044a8d017a93026e656ec0dcd1093866cfb87c688d3d99fcf",
				MidState = "3e862645a95124a0924b246e99a14cea20b499336dde6437fe36a28e9094d2d9",
				RestHeader = "6c86931088c687fbcf9fd9d3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1193,
				BlockHeader = "e3dff44de3dff44d209c309a76c4f0089cd5025c59ea521bd78b02456d53d6643d79d1063ae8239db03f5a89d6b4fea05ced64c081a9248c485529849c9a53ab53e8f4249facf0c3e3dff44d",
				MidState = "b66f71fe029339d076f0acda4f2453f43f9e66b12f319b8351aa141e0fe22009",
				RestHeader = "24f4e853c3f0ac9f4df4dfe3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1194,
				BlockHeader = "cd1c9ce4cd1c9ce45b7a4c4d68ee435dc1fc8d0104696c0fdf5d5649c787fb184484c7f81ccf6d54195997df5acc6aca1a2e1cdc19d9010a8f3e86d4c63e3d2341c380358a5c7847cd1c9ce4",
				MidState = "3bdb685fc8bdc3a76fb523a6a600418db4c15a3350f65c2928d3dbf2695c0685",
				RestHeader = "3580c34147785c8ae49c1ccd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1195,
				BlockHeader = "3222690632226906f444c7d3de3d32349e6bded9add8cdf0f1ccb5a472d702b504556a8f88c0c132ef8368bb2280cc5bd7d4ba4623f05d36c195b03bed2dd16e030f1f1e7d3f8f2732226906",
				MidState = "bd0109903a7c8cb24bd7f2c051cae53874d56aa05db626967a92ae0c5308bdc2",
				RestHeader = "1e1f0f03278f3f7d06692232",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1196,
				BlockHeader = "cd4b8315cd4b8315c683da4ad459146cb630904775d83492a1024251584f1b2d08b16486488bf33ea50a96aec0e302612aacb089aa779acb50f41dd2bfa369ebb44c7d2c1d5d0c43cd4b8315",
				MidState = "163494a58eac2d013d3172f686244abbea9798312551344083a5f9ce42d23d1f",
				RestHeader = "2c7d4cb4430c5d1d15834bcd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1197,
				BlockHeader = "a96c1c83a96c1c836664eb9dc9cf8c743271095e91e531d3216df9607b53b618cbc6e8677d21234ab16c04aea709068d340e0d7dfe09256ca2c0e1d29999cc61ce88347eeccb028ea96c1c83",
				MidState = "68f99e5fb3a4a57f974b3d9950944ff82d6129781e1dfa37d41551278d35ccfa",
				RestHeader = "7e3488ce8e02cbec831c6ca9",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1198,
				BlockHeader = "e9294b71e9294b71caf5895c32a8d7c65911a99a2b3bc097f04507f11304fea0f752670ae65267f8719f9587e5b2f53f41afc20cad17849b88d381174905bab727f1c06819c85db5e9294b71",
				MidState = "7933fcf7e971ccc792da69edc9acea8cb9eee4cf78186d25e1d745ec91033f70",
				RestHeader = "68c0f127b55dc819714b29e9",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1199,
				BlockHeader = "ac9b3230ac9b32304b440a2caf0e53037e5d19781270fce316e901bb11bac878b63d9f366633086cc6ca80fbd955a86211d37402f8530a4ccaf0b74d819ed7d393f32e029dc54e61ac9b3230",
				MidState = "aba0bbc9a12056edab28d24dba41958dd7dec1d68662451ada88840abf104759",
				RestHeader = "022ef393614ec59d30329bac",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1200,
				BlockHeader = "fe65f4d2fe65f4d29e8b957833d3d2a6e251a35a3787736aad69cc69e78546e8e7957616c046e20787efb161866f9588db7035986dc3c7d80c02e2640addf8b1799dc1048dbca646fe65f4d2",
				MidState = "6ef13d226119af984196836911533da2b765b5d504d1eab747182b7229360d1f",
				RestHeader = "04c19d7946a6bc8dd2f465fe",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1201,
				BlockHeader = "4c7a815a4c7a815a072a9eb42ee143c3eeb37c911a0726bb050592c05a415224e944731168b6ade55d800f4f655218d829cd11ed2c3275e416a3d4dfb921b3f5c353a06025fac99d4c7a815a",
				MidState = "6e4de84e57f80c162efcb794de182f69f7ca032ac43cce365b675028e617803c",
				RestHeader = "60a053c39dc9fa255a817a4c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1202,
				BlockHeader = "998e0ee1998e0ee170caa7ef29efb4dffb1555c8fe86da0c5da05917cefd5e5febf26f0dd7921104316be9774f260bcb3e72ad2cda7cde554de1fb874c90e9beca791913588c142b998e0ee1",
				MidState = "ce0c2a1b6c401008ffe72e6953828e5a456b0b0cbaf0c76e7e7d90b7ad6550ed",
				RestHeader = "131979ca2b148c58e10e8e99",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1203,
				BlockHeader = "58ca07b658ca07b66e4d8f13c62e1979673ab0a93277eea1a11c004c07a6297f08554b71145df77d6c22b1b7bf5f68e9f23847734ed71715c82012e1bc2f877154fe0c2e8eb202d158ca07b6",
				MidState = "cf9995d4e28228cb382ceb5ca9aaf656d27286b63f521345d572de17aad47173",
				RestHeader = "2e0cfe54d102b28eb607ca58",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1204,
				BlockHeader = "2978fe042978fe04bc62ff69c375a7b9c8b4a08f294c9d975b1b48050c9ab4303fcfe00bb4dca800b3d5fad87d14fb37447f79eacaa666a50a0638e4992da3e4a9150615f75c23342978fe04",
				MidState = "7e22a95f82ec0b77a680f923f124720eb64b37f737ecc51943cb331cb2db3028",
				RestHeader = "150615a934235cf704fe7829",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1205,
				BlockHeader = "e3520b6c5646ff858bb8d79a7d023bd28ac780d6d46cd756c3dcaea29ed70e572d8c6ae8f485a4a858eb0a6dfe0a280dcd0e22f8d5e1fee5d9a08cc4128462ce9790d811ff800a615646ff85",
				MidState = "e8c0e15ceca7e6fac9c90722c4fdaecc41fe6f969762409f5833f3039192620e",
				RestHeader = "11d89097610a80ff85ff4656",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1206,
				BlockHeader = "6496b3836496b3832e8e7a59fe662cf7dc68a4f018bdeb077406c3035c6c7351a302af6503403d7665fc61a117c504e0b903eb1ccaf6fe520152663b67a0d1aa24bd3b2aa5841bb96496b383",
				MidState = "f329a3769093d73d96b7b1e0d7344e9ab628d06841d8a3e65fcf2fb79154bf9b",
				RestHeader = "2a3bbd24b91b84a583b39664",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1207,
				BlockHeader = "7da5e88d7da5e88d119dc9f349f598f0db5c740efaa49a181e39a7f2fa8eeaffc6a1683bfcab4bf32b631e8c2efefec7015628f8c21a12ba35293f9a6b677074db218679e0e8fa067da5e88d",
				MidState = "21e6d2591536999e3c1a53d5260ef3ad8a26333b0b28688f6686b9eee7976205",
				RestHeader = "798621db06fae8e08de8a57d",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1208,
				BlockHeader = "386dc624386dc624726d2438aa49660db70718c88ffd45c1d982d55048b84ec15f9898b3a5463ae79a740bfee79c4484b9baf9a78f85461b0d921cb0165a1915b7a1ac73bed605a5386dc624",
				MidState = "5e7f6d10438ef95634b306b82d6fa2cc366a9756614129411d3599ff2429ba3e",
				RestHeader = "73aca1b7a505d6be24c66d38",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1209,
				BlockHeader = "8eaf5a808eaf5a80f295c6015c3c132674ed8338c5e81adac1f80777420722e5bbe8cef0132b9e513a111d86cc79cd829d9432828c1ccff2155dfb5e56030bc8ae9cd96d23e8d7668eaf5a80",
				MidState = "167ac0a7d7fcf9427b36d7840597a738e83644ad3bfd817f877006f87485f160",
				RestHeader = "6dd99cae66d7e823805aaf8e",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1210,
				BlockHeader = "9c34edc39c34edc33058dc2dd527855dd87ebac4757b335a2678de63272f2f8759d73415804e556ab3ad43db2ac8facfab14711f6d1b5da21a648f4a245b702513cf0a37d69d60679c34edc3",
				MidState = "7ca03f975341ae93f2cd9093ae1b19bf37c7310d534d07a27539043ad9bb8d01",
				RestHeader = "370acf1367609dd6c3ed349c",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1211,
				BlockHeader = "1082fd7a1082fd7aba4027d7e0cf3fb984c82eaa7204f34ff5b7c2174850b80318098d215e9adecef4c55864c6238730785c9ecb02b0831d8ffb4cecf1dc2ec7a387e676e48b844d1082fd7a",
				MidState = "357172109077ebe4a2df6b20e676110b8873b69878cd3678c81b034928121cce",
				RestHeader = "76e687a34d848be47afd8210",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1212,
				BlockHeader = "acab1789acab17898c7f394fd7eb21f29d8de01839045af2a5ee4fc42ec8d17b1c65871807d8ec590c42b3d7595858597c835674e146e59f5167a4594b9a2328576d726b0ccd48b3acab1789",
				MidState = "80425048d4026c4f4744345f478141c34941cfe5d3fcc720726ff48126e6da7a",
				RestHeader = "6b726d57b348cd0c8917abac",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1213,
				BlockHeader = "37d0ef0b37d0ef0b0eb9a81ef02805e413d53e787b4e828c4247314d93f0268ab3a5a8db5ec37c67193f385b42db2c08ff4a14aa7395be7b460658d7852d3bd61b5bf20d5f4442ee37d0ef0b",
				MidState = "10074137b9e24f4af7309586b3bd4c3e7c401a7181b23fbd06c783304d7d8acb",
				RestHeader = "0df25b1bee42445f0befd037",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1214,
				BlockHeader = "32155b9e32155b9e86ec7db9b1af8c407da3189edb50afc2ded5a97c8f5e4c23ae88010dce658c3935a37c95ee1333667c3e389672a4c5f0213a0144a7462a3eb4eca73f88f1d8cb32155b9e",
				MidState = "ab028f4480691b622bce0a21df8263a30eca156662f45aea44dd84c5abb20925",
				RestHeader = "3fa7ecb4cbd8f1889e5b1532",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1215,
				BlockHeader = "cd3e75adcd3e75ad582b9030a7cb6e789668ca0ca24f16658e0c362a75d6659bb2e4fa043bc580fb73658700079e96862a814bd67621118c2ebbf91d2977b75b5003de42cf89890acd3e75ad",
				MidState = "2b1c389d4e01773c93d284dc842d6e76c7080b28ef169b1ede767d8be36b8a44",
				RestHeader = "42de03500a8989cfad753ecd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1216,
				BlockHeader = "dc29848cdc29848c28c8835b4d0a35529d474cc57b14d4f8c9e1c747c4f626f7fa5edfd97c90b6e63ccf666cab427ceb31142615ec23801ddd14469704f7fb4d9189a7495d34bb63dc29848c",
				MidState = "c1ab649c944e24498ab8e2245e88f9abc9cec30e2677688de335ab5188096a67",
				RestHeader = "49a7899163bb345d8c8429dc",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1217,
				BlockHeader = "6b23c69c6b23c69c6952cbbf74efbfba25b9f93ec395352cf8f6d4bb7457d62414b0ef42f6f51ad966c1d1bf2e0962a156654da75dc1e511442fbc8a2079d484dba3c344a1479da86b23c69c",
				MidState = "6ad1c4453ed2b94ea85236a65cdaf0acc747e03acbd5f627be4b15a242b4d116",
				RestHeader = "44c3a3dba89d47a19cc6236b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1218,
				BlockHeader = "2b2b473d2b2b473d9bafcde4769529eac23c32956f869e8f28c2aa5937cb54b056f86459622162d31df702cea07a5cfbb26e02fe54bbbae5c069adcd72fedc05a6967d0ab1b7e48f2b2b473d",
				MidState = "edd88e695de0acfc495b0f66f3512c6735a065fd05e5ba44ea80b52e9ce276d8",
				RestHeader = "0a7d96a68fe4b7b13d472b2b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1219,
				BlockHeader = "48c307e848c307e84fa2ea35fccfaa4b40cd2df9de453a24311459fb5fa72302db1b87c3fcf869f77b24a49499caafb454770dedaa8881ab50f0045fdf050974f6fcd77abb8f9f5948c307e8",
				MidState = "19a258cb55b4b80839b56029f35f050087b10810af0e0a21617295271af2004c",
				RestHeader = "7ad7fcf6599f8fbbe807c348",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1220,
				BlockHeader = "e304e526e304e52606aea165fb1223c112c65ff79772aacd230408b4f315dd9eede75b5111c655d498a7a2ca7aa788ecf1529815485189d5a5c5a8859b1b0268c6862b13f5a808a1e304e526",
				MidState = "689389335d1d027c3c2b9a5acc2026c5e4d9cfd910271d7f42f59815eade7c5b",
				RestHeader = "132b86c6a108a8f526e504e3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1221,
				BlockHeader = "cd6d2447cd6d2447089b66fa7641dba24d1fc75181a63fa91f0a15508ced3d4e4f9154516b72e98aeb7e09177e5b038848ffb539c3e2b8d457df777a5b0d75c2a3c2a72b952cbf70cd6d2447",
				MidState = "03f2c7b588bf8ac9933f4db6a052671019b31d7d90adf5b9c5bf17840c4faf39",
				RestHeader = "2ba7c2a370bf2c9547246dcd",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1222,
				BlockHeader = "a88ebdb5a88ebdb5a87c774c6bb752aac96040689db43ceaa075cc5faff1d83912a5d9329b854eb59863dbb1b8d64b70604109f85db7367254e5b7bb4f56a2244551af4c67c58f3da88ebdb5",
				MidState = "f7b2da65142627b29f3a4a31d786116359826ad3972233a9ade77fe961f18910",
				RestHeader = "4caf51453d8fc567b5bd8ea8",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1223,
				BlockHeader = "567b6565567b6565df2cffc502f2441b5c8b0209d3fa97a0e01736b61c7a9769c6fbd7694547c3b8e4d58dd99872abec0a854511078381fc6e115c7c75bd97a288466ffb63a52244567b6565",
				MidState = "d6c3d4b5d058a8b0a7f79958ceff02736be0bdac082c2a0728f9e471751c372f",
				RestHeader = "fb6f46884422a56365657b56",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1224,
				BlockHeader = "0290fed20290fed21ca2112dcbc48c5b5d9788f52e82344660714c7bd84cd0262905d0fe3dc11a2d183aaa951565fba34189e5ef66273d577976a6595979c3f9f85bceefda963b4b0290fed2",
				MidState = "17742b5d61750cfe88b508fd70bf864291cdf5fa58d81becff1f59985a12bf32",
				RestHeader = "efce5bf84b3b96dad2fe9002",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1225,
				BlockHeader = "45db1f1e45db1f1e5468ebfa3db89cfb8fa80796cda693c6e5c678fda123ffc158f24366541608a33023693cf0e5411d9e75de1da8568b9e213518f1c84045ece86370f14346c00845db1f1e",
				MidState = "d3e4d32b8b4f1041b3b56d2b5f6c0900b4b9b903a3ea888dec2d6d126a1ccfc5",
				RestHeader = "f17063e808c046431e1fdb45",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1226,
				BlockHeader = "e104392de104392d26a7fd7233d47e33a86cb90395a6fa6995fd05aa889b17395c4f3d5d74f1ac945006fd2cbfffcfe79808c833bf123bbb4d19e93da2b393177972b4e496a53cf4e104392d",
				MidState = "bd851978a92d1904ad0625bcfa4cdebc7be407992cb71b6cdec36661310ba3ff",
				RestHeader = "e4b47279f43ca5962d3904e1",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1227,
				BlockHeader = "edb07a7aedb07a7a6f621bd2e657978c3db5e0ea70dd60fbce2c2b34db1dc64582c4612aacd7428e00653ddba0d3b5f618f4c25cb91a8a90eaf08cefd9873436833791c037d7b0daedb07a7a",
				MidState = "57ecdf38985b516561eb1d6a1385b0294332e1021848845bf8f7b70be21b4600",
				RestHeader = "c0913783dab0d7377a7ab0ed",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1228,
				BlockHeader = "471cffa1471cffa12fffca3bf47bdf0fc3010a163f3e4891d3fd649e3771f0a3ea7d71b0ab8b78d7a11766b6b7838d836279060f685df22e4e9dfdb52f8e632633dad8fe0bbb7ab6471cffa1",
				MidState = "3930d8299546ec1cf7cdffeac3f8195a289b7821d857965b1762467ef6439006",
				RestHeader = "fed8da33b67abb0ba1ff1c47",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1229,
				BlockHeader = "cf6ac09fcf6ac09f4e461c4ebb28d65ed4365ab66491ca10cf98dee4aa22b494fe3c9f56ecd3847d7506ee5523518261366e8d5409741c5661eabaff2dd803edb70eeaa6c26f217dcf6ac09f",
				MidState = "261d7327a1b960373dca2d893b566bb0c6f392b07cae7efbeea9418fa16defac",
				RestHeader = "a6ea0eb77d216fc29fc06acf",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1230,
				BlockHeader = "6b93daae6b93daae20852ec6b244b896ecfa0c232b9031b37fce6b91909acd0c0298994ee8b84b0cd5bba64c764e95b81838d11ce601d78e4f69956912b2bfac87f726f7fbb1956e6b93daae",
				MidState = "b7b5de4e9ff36466fcf5c6dfd79f39c610d793634e7df7a443ea35fd0034e606",
				RestHeader = "f726f7876e95b1fbaeda936b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1231,
				BlockHeader = "db2b813edb2b813e1de683ff337102341a669f6ba3120e04c945d6543591327a7251ef0f2c4480b99d494e5bb3297f89b7ec15352bba86b91e072be242fb4bc5e89774ca06d93551db2b813e",
				MidState = "74054fafe4e197d53ffb56630153681f8a9163648509bcd8746310ed73da1d0b",
				RestHeader = "ca7497e85135d9063e812bdb",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1232,
				BlockHeader = "29400ec529400ec586868d3b2e7f735027c878a18792c25621e09caaa84d3fb674ffec0b650247362f17cda5ee338c3ee595143628f7eeedbbc5670797eacf54d05d81abf506004329400ec5",
				MidState = "67f1c5dc39a5d57413f8879f81a3edd8274da9efb66642d8788b4888c4aa1d07",
				RestHeader = "ab815dd0430006f5c50e4029",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1233,
				BlockHeader = "18db17ef18db17ef66d4d88c00c616af97c2a49c3bf7c55529ecfac2d2d0e2fa9923654ea57ae5534d61c181570a0a9a4619bcf29d289f14e5ca49a459114926329406a897632fdb18db17ef",
				MidState = "d6b0d948a01404a63ef46e9008322945416e1e095654458f35801e20912e13df",
				RestHeader = "a8069432db2f6397ef17db18",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1234,
				BlockHeader = "a47d845ea47d845edbc84c08e866d798eb1a15f206da98dc9267287065940b6ac4332b6ef0c8e2ece3f12799ba32744283e5adb0d7ea1a4dee836e33e36c823ad5e886eaa4577a34a47d845e",
				MidState = "d39de8448471a9d063efd2fd708c452936a78725af372c0405218bb1de6bddf5",
				RestHeader = "ea86e8d5347a57a45e847da4",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1235,
				BlockHeader = "6271050462710504ef3097a58018590b5286001f38634593fba5c1ad852a627f5fe50cf085557c124fcd9b6c8ae7cd79754fbe66a6a61ea87146560feebc6843d765f581f34df8e262710504",
				MidState = "e637c0718f8a97cc08e3a4239770c97f81fcbfdae7393b1643a0b9fb0d819680",
				RestHeader = "81f565d7e2f84df304057162",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1236,
				BlockHeader = "dc1a877adc1a877a1bfc735de2939782ec6cf3f19a2d566e3ae022f65a525f417df3362c0f2e60cb7891338d1413959e7166bb05e1ef139086a5cdce363b5cd7ed634080f608c02cdc1a877a",
				MidState = "d14bb76019479183ce59c36cf3ff074bd6528ef16f8d5d7103e6b848ee4b4e78",
				RestHeader = "804063ed2cc008f67a871adc",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1237,
				BlockHeader = "b83b20e7b83b20e7bbde84b0d7090f8b68ae6c08b63a53aebb4bd9057d55fb2c3f08ba0c9edb2254615736080d243ea0dfb4798777b23298a6ae150f9fa8475fff93bcc2eb8cc2a0b83b20e7",
				MidState = "60d3f62bdb0b0cb5c0c72fe75b51d668d94f0abdb76601e9b253f6bc4190f391",
				RestHeader = "c2bc93ffa0c28cebe7203bb8",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1238,
				BlockHeader = "a59d8ae4a59d8ae4a43e539dedfd81585d26f94cb3a03f6650947fea7365009dc952ffc82cb09f95704b4de4f526993b7708cf2275cf0d9993f5fbe1deaaa94a4a0387d49cf4ac23a59d8ae4",
				MidState = "aea89a5ede0e77bdcf9a023504313e8efa9844f34e5909a3e4644c30aae8d8eb",
				RestHeader = "d487034a23acf49ce48a9da5",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1239,
				BlockHeader = "e43454fae43454faa1334071997db7ccd31a40fe41c8170c3bb2b2875bc3e9bd194cdc290e853223aaf15d7cdfbde04c3de778f37a47d0e641047513fd98871c75760da6ec3d856ee43454fa",
				MidState = "31fa2c4091f51288ccb4ece2f8dee5a3427451f8055b6f321dd45b191dfcd015",
				RestHeader = "a60d76756e853decfa5434e4",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1240,
				BlockHeader = "d3e2317ed3e2317e63c291eceba2267a9f11c91e43b58cecf539a62c17f524da8ba17de214c94410b5e86ee480eedf0c66f72bb75966cd043d43f548610ff8ae1fc4eda5e70ac867d3e2317e",
				MidState = "9900232dfbe36b7021f5a89573e6145996ce549604c1d1745e953a1c0509b36e",
				RestHeader = "a5edc41f67c80ae77e31e2d3",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1241,
				BlockHeader = "afcfc348afcfc348974b80384eb76912ef553e0b234e67b24f22a62d51ca5c80029a9101dd89278e0055d2122ce6b3f32072f998312c955c01b58c12f1dd81f5fb577a90069ad4fdafcfc348",
				MidState = "7c67ea3b8b4440d5498d58a96745082b4b85430abb49a916fd8f1cee7dff2e95",
				RestHeader = "907a57fbfdd49a0648c3cfaf",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1242,
				BlockHeader = "84778077847780771dc1339767a563da9d166bd6d00c7d3ac6d6427bbe83e660f29b07895e41ab14eec00fce54dc99fee02084b4f44a88e796d8dcbce96cce6c30fca4b1effebdd484778077",
				MidState = "842432445227ca0fd83a856b03b6c0a73171e4ecde909b4092a1d275382622f5",
				RestHeader = "b1a4fc30d4bdfeef77807784",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1243,
				BlockHeader = "23d0f31123d0f31132c7bf058762916209148f1fa779db91ebc529a9af6be382a13e66faacfc44ad5a5158511ed7128aa7bb4a1b81f47b1ab18aa8e555c53b5f34ceb9e9b7aa8d6723d0f311",
				MidState = "1ac28b48c3565186a91bfe50eb1ff3f5698a9f905a6d28aeebafd62170caf64f",
				RestHeader = "e9b9ce34678daab711f3d023",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1244,
				BlockHeader = "00ea16ef00ea16ef124e7cfa6bcb34fb18812348031de963da7ab2eecd13d18e2575db65272e87e96a7aef36f49b55e33abee175b17749d48d976fe832cfb495b53018ebe271b62700ea16ef",
				MidState = "f02487b793698549f81c78549cef6767e4f458f130bb8cfed498e643ca47670f",
				RestHeader = "eb1830b527b671e2ef16ea00",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1245,
				BlockHeader = "9b1231fe9b1231fee48d8f7261e716343146d5b6ca1d50068ab03f9bb48bea0629d2d45cae8804356fdc4ec2278d37bd78e5574a89544520882b8a8fa8f6959db36975b1bffabca89b1231fe",
				MidState = "d19c2a761672384dbb5fdb00f4fe26af7e6b7960b500dc3d2b9ded276f2e595c",
				RestHeader = "b17569b3a8bcfabffe31129b",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1246,
				BlockHeader = "eb1d01f8eb1d01f8993fc779861cbc17e8c4f05e34b997b11d21e1806f20b8f45c4d293bd8fa36bbdf6b687633c8d4a42befe4e6113ec042a7a22c5b680e8662661dd3c91481e646eb1d01f8",
				MidState = "4ab74fb4ced5260b54eaee84fa2e8ff018f3a1c5aca36a0b89f57c6f78def35c",
				RestHeader = "c9d31d6646e68114f8011deb",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1247,
				BlockHeader = "ce6f2a15ce6f2a15a42449973ba4eba605e73512ea8f5f96e1184b2d408c7f80ca292faf508900e58fe73b9637825441716daa9d0771b1284557a71c7beb3e8587c8e5e5e1b37eb5ce6f2a15",
				MidState = "cac8dcbaab62ec8ef61fbfb29d609b017f48c11e7ac39f08fb705e9edb1ed3d2",
				RestHeader = "e5e5c887b57eb3e1152a6fce",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1248,
				BlockHeader = "58b1b69b58b1b69b0db7faade85331c5929c356cc2ab285b5069dc426ef280fd2e15db286259b603612ba4e390c28178576a0280c745c87a3e3ffa36b9ddcd0086a78cb40f17eea658b1b69b",
				MidState = "c5aca3b42179f1e01123c0e7b1332ff63e42e02e120e87a6a29445d409c84505",
				RestHeader = "b48ca786a6ee170f9bb6b158",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1249,
				BlockHeader = "02969a0802969a0845e01347b6c02f1c4725cb350df86629373e061abda7e8d41eddb4712b9b43fb374e06603761ab89d76ed0177d3ab2fcd82d05c3a9bfd35d57b91eb15ffb04cb02969a08",
				MidState = "3e9ba9fff2adfaca9e1a6eaa019db5b94d4c1220eeb6f02c238c7dd1125ba2af",
				RestHeader = "b11eb957cb04fb5f089a9602",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1250,
				BlockHeader = "50ab279050ab2790ae7f1c83b1cea0395387a46cf078197b8fd9cc713063f410218cb16df3dd1d7ec06e757d6d14bb66b19dc3d3b559d85a6c2780da7070ca2584d546e126e0e3fc50ab2790",
				MidState = "cc3882a1387c87051459d17da40709316b18fb9d0d9db1cf20c2323a5a881715",
				RestHeader = "e146d584fce3e0269027ab50",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1251,
				BlockHeader = "273a4c30273a4c308cdcdc89aaab1e32212d43a9d8ea72c6d8e7ee8801566b6b5a83b87db968ab959ae6d88fa5ac49e5e5abda120cd29b186d896aa75ff409252325c0f779311a84273a4c30",
				MidState = "5498362d49b7985664f23e088340464fb12cb0bbe2194d2d1c4f7bf9bd19f5c4",
				RestHeader = "f7c02523841a3179304c3a27",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1252,
				BlockHeader = "27196de627196de6aa3c644d623ab120f36e5c742720e1742746301db0b009a3d634894346dc9bbae62ebb2d1f04b6c13ec4b8daa55172f472c4c97e91dcc984d5ebdcacbb7f218c27196de6",
				MidState = "19caeeaa4f3119288ca44db07ded01ae1891158453e97a96180e68033fbc277e",
				RestHeader = "acdcebd58c217fbbe66d1927",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1253,
				BlockHeader = "973bafee973bafee44d80d404d6d9bb83bde98e25b995392357d021ced35112e3a45c527fd17bc6659639211f5e49472a2313c8261342711142ec1073c2d0b5f8b6a1abaa49c5e63973bafee",
				MidState = "cb94e572bf8b8882c4b4cd90e35ee635a022e0ef435c01d258628e565c020f1e",
				RestHeader = "ba1a6a8b635e9ca4eeaf3b97",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1254,
				BlockHeader = "e54f3c75e54f3c75ad78177c487b0cd4484071193f1907e38d18c97360f11d6a3cf3c223be4dfae49e9b9d968fd644ccc3890ffb35673a56e5a2ad07cc13cfe1fe7095df7b0485dbe54f3c75",
				MidState = "fbc74b77ac51a3acfa5d384b345407de1c581057bf574fb1edb8e036533044bb",
				RestHeader = "df9570fedb85047b753c4fe5",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1255,
				BlockHeader = "3264c9fd3264c9fd161720b743897df054a34a502399ba34e4b38fc9d4ad2aa63ea2bf1ee4a5a678b3c389fc70255564624fe97779fc400aec6cbe0fc5a8d0982911f582a509ad203264c9fd",
				MidState = "d964f0bc4613b42a2375cf1303909157ebfb31091a874e775af10fc2d3222685",
				RestHeader = "82f5112920ad09a5fdc96432",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1256,
				BlockHeader = "b5e2872bb5e2872b7a5f078bcbfe7eb0df82fef5b8ace851a7f03065a49222e96e2b894eebc7d8a8abfe1ebca3d86a23998451f00082e23e8853e46ca656b59fc5e4e6e760d16536b5e2872b",
				MidState = "cd7535a097789da4574592f41c57b5160a21a63e128b56eda3b19d8856eb2f93",
				RestHeader = "e7e6e4c53665d1602b87e2b5",
				Nonce = "c0000001"
			},
			new SelfTestData () {
				Index = 1257,
				BlockHeader = "3a1c42313a1c423166238e6e272de5fd0ff510ac0634223f89ac7915b5853a8385e3f29458236183754f26f3eaf3d32a720515db7f59ba8cb46ecaa51513d1e9ffa4d72f5c03d1233a1c4231",
				MidState = "efcf9ebf7060bd2bc146fbbea930fac2983cb737b3f3d3801d1d15d54e853e81",
				RestHeader = "2fd7a4ff23d1035c31421c3a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1258,
				BlockHeader = "e63e1a99e63e1a9946d7874ab9562e3908114f9b0a6cffc44f102c534868f00a3fdc1c52ada3338295c1e0ba0f6dcaae67b538767b0d087ca82aae6414dd5749bea75851a97bdb47e63e1a99",
				MidState = "f0233456ee9498fd046da96b30ad50f50f634e040975100c50d6204c7b368e5a",
				RestHeader = "5158a7be47db7ba9991a3ee6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1259,
				BlockHeader = "b6827271b6827271d39d6205a5d578754d0410f6949637524abab20130d9157be1ffef28ee7cfc70ff13594d9da3bea98a81c933d0c09ff41f33e9511ce815e474d2fa34c4ecdd77b6827271",
				MidState = "c29453f4548fa5187cbb7908bb64711ba97c11d15d6e1174bd2798103b9c362c",
				RestHeader = "34fad27477ddecc4717282b6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1260,
				BlockHeader = "034f87c9034f87c95122ceba9b56066b54ae7a7a76af83c5c7176a5f8a82aead62fdf24109dc5a344662cb8459c78672ebb8c233c62532dca5c9029f4d559513c859aa59c3350c1d034f87c9",
				MidState = "4f826a46e79ebc7831ffeaecd94c7b9ed8ca444a838182bcba8fc2909689d31e",
				RestHeader = "59aa59c81d0c35c3c9874f03",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1261,
				BlockHeader = "5064145150641451bac2d7f696647887601053b1592e36171fb231b6fe3ebae964abef3dd6e3866abf702224d743fa09105652d0a726521c6427150c7e05e74739148904f95902bc50641451",
				MidState = "2d0348bb08113aadd0ac921c6a58821255d44e922f8f4c6a4d5181a250b58200",
				RestHeader = "04891439bc0259f951146450",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1262,
				BlockHeader = "a739f95da739f95d479da3cb4e58804e1421f0c93460a2521f5acc06fe43ffbe966245ffe851d45f01bf044ea26faaa1c7b0ddc248d6de3a9fec14e22cba78781cd7207d5bae7d5da739f95d",
				MidState = "eba8d8d9958dcf426ca6caadfbf2f6884eecc8d667cd63e0c392cd3deb497c20",
				RestHeader = "7d20d71c5d7dae5b5df939a7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1263,
				BlockHeader = "026a940e026a940e4b39b7684619cdb6ca68db8ea6517257ff5b2f52a82f95c3dc06b40cd2f56c7f9447d8f1f73c71fd9c872e3c7108fe45210c5d95d197592849bac118852e04f9026a940e",
				MidState = "a9b0ba6526112f75ab76b9b1fe0a2bfcb665844e55f94d95ff74d2cd04b0864f",
				RestHeader = "18c1ba49f9042e850e946a02",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1264,
				BlockHeader = "628e8de1628e8de184e6a1b88c12d4c1412350175735c6b643b594f411739c863a5579d824eab7ee0e41b5d2cdcb834b5c0e08507f68d3a78f0d0d5fc548acaa2396d2221cf9ba0d628e8de1",
				MidState = "0636037e5ac56bcef15d9627459a86bdd416510eaef2c52666c3754a080e9c5f",
				RestHeader = "22d296230dbaf91ce18d8e62",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1265,
				BlockHeader = "3eaf264f3eaf264f24c8b20b81884bc9bd64c92f7343c3f7c3204b0334773871fd6afdb9cd9b8027124659361d1d14bfeddd8b180517074ee771c5c55cfbbc2cd5fb3f62eaf9ed223eaf264f",
				MidState = "93a0a5e634d20102b6f3da985ae5dcf1da6f4f03a3bc8406391ab830d862a18b",
				RestHeader = "623ffbd522edf9ea4f26af3e",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1266,
				BlockHeader = "7f5c04517f5c0451936760df5b03008e2a3206b8f0ddb512605c3c0f5a65259c6380ddf1ea7a58cc25c62e886baf9065d494089163232be89937e29e4337c7871b67c33e80c34f6a7f5c0451",
				MidState = "0858fc8568df5950902d5e0060e30ccdb565138b22ed31a712654395b5a7fa44",
				RestHeader = "3ec3671b6a4fc38051045c7f",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1267,
				BlockHeader = "95cbb68095cbb6801ee6c9a861fc60abc906fb0c0f504f2b23d3bea6a9beeea7f55d293658aa1b4c80af37eb9ad868f8798974ea3ae54725b0b86a6303a3e1ce10a01546acef2bbb95cbb680",
				MidState = "80619c5bc1a9ac26991716120e7ffcba4094d95efdb0b63a6b48a91eb1f0823f",
				RestHeader = "4615a010bb2befac80b6cb95",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1268,
				BlockHeader = "bdd0b08bbdd0b08b20531ba011f518eed78acc812c5154d29a358a60463d40c2673eb1b68cc500ad68376f17b06c71f5c5e88d2f870b94c907baed153716bf5e719e485504072e9cbdd0b08b",
				MidState = "b272eb3066c3c1496a3cf28b36d9fc1e7fbd8f5c95d6f971513551133b0ad516",
				RestHeader = "55489e719c2e07048bb0d0bd",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1269,
				BlockHeader = "5a0636365a063636d083c5ebba7e1690c07828f6c9e2471ea4885a5a16f763a31aef48f4b1672f21b8678a329f5b86f8c49f5ab8097aedbfbb8afea018237d013ea26821e08bb7655a063636",
				MidState = "b9e65aeead67b99c706d3d70b9e2f15319395b6c652beda1cc0f7df3b1e104f6",
				RestHeader = "2168a23e65b78be03636065a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1270,
				BlockHeader = "4f1344614f1344612b50f5136c66e61a91b79a59badcd62fc89f88fac71f29995b152a967dc549bd885df46dcf8623f4a0b977aac3bed8d0e686e07d04f2cf55826c0372232668af4f134461",
				MidState = "4c59191c8e9a62e5dd6d8caaab374adf6c682b1c5352feba45dda9db47a51416",
				RestHeader = "72036c82af6826236144134f",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1271,
				BlockHeader = "9d27d1e89d27d1e894efff4f677457369e1a73909e5b8980203a4e513adb35d55dc327911302c05bd7c843de6a4f7447cb0fcdc78291156ada09df0d5a2d3d03a9bef355696a7be29d27d1e8",
				MidState = "67564db287622edca82b9086f41ea8747d4248d49da3ff704fdb6cc38ee7fa63",
				RestHeader = "55f3bea9e27b6a69e8d1279d",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1272,
				BlockHeader = "ea3c5e70ea3c5e70fd8f088a6282c852aa7c4cc781db3cd278d515a7ad9741115f71248dcdd9dce3bb1a15f33bacbad0171a0cc17dfff05aed6891a97c8b344fb870f25866bf4152ea3c5e70",
				MidState = "a23434c33edd8fc67358750f9c4d7b9b063b115261eb5891442214579d66fb04",
				RestHeader = "58f270b85241bf66705e3cea",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1273,
				BlockHeader = "a3e751dea3e751de4ec928b39af4c80a2312f3c5f607715f93533a5d8f8bb36e2a3aab350908052e104dac70c863699fc9642d4aee5aa73727860ba632234433546da62b8eb16733a3e751de",
				MidState = "f85829f38d53d80633da7d7c506fa0ff54ffb82584d8cb04971f9dd7811dd5a8",
				RestHeader = "2ba66d543367b18ede51e7a3",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1274,
				BlockHeader = "a80d53d5a80d53d58bb397c46ddb98848e3f8f17ead8ebb6216fb420d648ff5e7665d35f0d8c5bdf5d97a1f7e42535f1214f70881d45ae627b236f5b1d086aa8ed592e0ed21d2eb1a80d53d5",
				MidState = "c15865d67ed2d13fcc21a0ab8b98d313c1fb2282cfa0db9d8c0a96782829d100",
				RestHeader = "0e2e59edb12e1dd2d5530da8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1275,
				BlockHeader = "44366ee444366ee45df2a93c63f67abda7044184b1d85259d1a541cdbcc018d67ac2cc56f6babe09b8780eaa2d78bfb5717e4ed750ca2c21aa138dcf85ea1758285f047c8decac2d44366ee4",
				MidState = "dcb181987a063c9e2f128e7150edfe378546ab4a6736b893a5efcec4fcaaf400",
				RestHeader = "7c045f282dacec8de46e3644",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1276,
				BlockHeader = "1614834b1614834bf611fb215d2b1555a09e807542c7a102b2f060a65e06da23661cd607bac4fc91cc28cf97865546d75d55d97c254cd56fcad0901dda8bed6798f232654f981e821614834b",
				MidState = "1154bb75d02d906aefbdc89ccd45f6795bd21351d9ee5d18529b5ea663d80ed0",
				RestHeader = "6532f298821e984f4b831416",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1277,
				BlockHeader = "642810d2642810d25fb1045d58398671ad0059ac254754540a8c27fdd2c2e65f68cbd302f65ec3bfbfd2d3d08872f268f0bdef6bffda473e670ebb02e38c5b55e8de4435193dec85642810d2",
				MidState = "16d6d082e283d94f4884fc88cff0088e72b75bccf51d5ef970554a2f5fe42626",
				RestHeader = "3544dee885ec3d19d2102864",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1278,
				BlockHeader = "00512be100512be131ef17d44e5568a9c5c40b19ed46bbf6bac2b4aab83affd76c27ccf9ccdc8eb014360a3a42c92a85b98341c5cfe06dd9220f49a611b3502fa7478e3ddd5d1d0500512be1",
				MidState = "6146849842d4f2ce57ebc6d6bd32511bc45b403d3e1b74524996db6b5d8c618c",
				RestHeader = "3d8e47a7051d5ddde12b5100",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1279,
				BlockHeader = "44c9bb7544c9bb75d3f9b0d7e88e6460be6dfb4c2a8797e7399d057585b5c9637fa303dffb81b12633f93b56cefdc0aed52df6ad4bad0b6137f2cb095e1fa545eb8ea51494e492f044c9bb75",
				MidState = "e2eff27e4cbf05ec45712aa6e7ab505fcff156bf3c6f7de150f4359a9eba6a1c",
				RestHeader = "14a58eebf092e49475bbc944",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1280,
				BlockHeader = "2867b2902867b2907994345cab0b8f6aaa5abb24e5b14f18449f4f4c7feeb3d59d862022723d3fb99f5f9809d2be8ec309ec985c3354429460eadf9477674fd099caf75ef82871bc2867b290",
				MidState = "d6979bd3b5ac71680ef9a3bcc562e7d7d40cc2cb72a254e3b97422059f57abaf",
				RestHeader = "5ef7ca99bc7128f890b26728",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1281,
				BlockHeader = "c751fdd8c751fdd879d154f0afd3fc640b71767e08168e2bf7892ca32b7ee60066ac767ca27eda55503371dd11ae35444bce30530cf7eec48b91c8b4618d8df35406de588e273225c751fdd8",
				MidState = "99912e4a79f17699981079a93b58b5e2160e3d4cd9dcc472db5e502fb2bb34fb",
				RestHeader = "58de06542532278ed8fd51c7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1282,
				BlockHeader = "856ec03a856ec03aa3ace48fc721c810befe33f175682d5ce82eac477cfdc8342ea73c1e7bffa692e89092ab4665519d32f71217531b421d55196faa76d92cd10319593f7719061e856ec03a",
				MidState = "224d94d7afd0d1dc638570bd3d396bad6368c3738eeea811f6b4fd2ab9c269d6",
				RestHeader = "3f5919031e0619773ac06e85",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1283,
				BlockHeader = "1a04f7fe1a04f7fe18cc9af4952c100b47bfb9c14e08789bcbb8ab018a2a5c3392b29e7c053ec9630329bab1eb9399da4ca095aad520ffb8123bc32a083ef122b3fcba2ce160b7c71a04f7fe",
				MidState = "86a236845bf5de50fdc880913fb53ecf66c03b6d8b5a11ecbd973a16ea069e2d",
				RestHeader = "2cbafcb3c7b760e1fef7041a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1284,
				BlockHeader = "6818848568188485816ba330903a8127542192f731882bec23537258fde6686f94619b77623845fc02086fd2a5fba1bbf5f57085d2cfc9feb014d25c6853fceee77d756b6cab1d9868188485",
				MidState = "df2c22e70cc0242a5c941149bc1cce353662195c3b8e57a02d7846d31c634182",
				RestHeader = "6b757de7981dab6c85841868",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1285,
				BlockHeader = "02e4742602e47426d7777de03da87f8e6fbcfdd179fd7d74a0234f0eb7064a4c79242e38e9c41cdaafd115f8c2f1c603a3315522d68d3821992935bada15493772eb040971246ae202e47426",
				MidState = "734e3bbe367d5f6d4e13254fcd37d96f5c1257a6aad3157050c18d876bb8c623",
				RestHeader = "0904eb72e26a24712674e402",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1286,
				BlockHeader = "0559f97e0559f97eb9e7f47f791bd87a4bd013dfa075293cb71db31ad4b9175f3f2f2fee032263af1e1e207b73a3500817e7093c63b199199e33825227673c1166047b7dabace0610559f97e",
				MidState = "b58ab2bc0a6e106b37f84f8b4e9cf2d57c135a28b63ab7d4a3f6286c2461a7f2",
				RestHeader = "7d7b046661e0acab7ef95905",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1287,
				BlockHeader = "bf55b962bf55b9622b9a1f847790626f45b8370a47acc0f415de9c43c94c2d6af3e0197ecfc03da58e0f6748dca31d03b1a6d316437c78229e03be50938177f1a720c276ae6280dbbf55b962",
				MidState = "9fea32408f8b2cfa0d4f0c75ed3afab0e3847ce1de6a6f5de7cb4f445f2c6534",
				RestHeader = "76c220a7db8062ae62b955bf",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1288,
				BlockHeader = "5a7ed3715a7ed371fdd931fb6dac44a75d7de9770eab2697c51429f1b0c446e2f83d1275164c316920c04b0d81cc575203b21366d9ba376a84a73e060fb573d73bd2c6364cbbbb9b5a7ed371",
				MidState = "8bc42fa8ab183b2dd22b6fe79e6f7705e951c70280b076a00c25c6261de8e4f2",
				RestHeader = "36c6d23b9bbbbb4c71d37e5a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1289,
				BlockHeader = "ca9e0288ca9e028845c1d17793d898ace42a7d37ce1a3cebe7ba7be3539870d9ecc8e017feb36f4e0d3886f393223411047c625007087ccc7362f97b09e74ff2343fd1031d54367cca9e0288",
				MidState = "6da68b8e1b04bbcb46b86950bcc8aca0710aeea198c8cff38c25b765aa218ce2",
				RestHeader = "03d13f347c36541d88029eca",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1290,
				BlockHeader = "c284041bc284041bb258b3735c948a35b6f177170da6ac72503e1a3977cdf2a50026f9b29bd65b06fc624b5e814db200ec3f69b7429b089beae436141aeacb624266611a5ac27a8fc284041b",
				MidState = "27e814cc2f1cbec4ecc9b736a1e5490e503875794122957b3ea1a3343cfa5eee",
				RestHeader = "1a6166428f7ac25a1b0484c2",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1291,
				BlockHeader = "109991a2109991a21bf8bcaf57a2fb51c354504ef0265fc3a8d9e190eb89ffe102d4f6ae424a4397f7ec466ea5e7e5e11f47dcbfee5bd458772ad04f30911edf3df442677322b97f109991a2",
				MidState = "22f13187e2192569d2a34b585342d137cdc33641e2cdd85bcca1fd6d206f3316",
				RestHeader = "6742f43d7fb92273a2919910",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1292,
				BlockHeader = "abc2abb2abc2abb2ed37ce264ebddd8adb1802bbb825c666580f6e3dd10117590631f0a5df4fe4cd8ff9b4859586a3269f92f466180792b3bc27814dc574bcc4b0201569877a0dcaabc2abb2",
				MidState = "da48edc770edeb0dbb1eb825f80510d30027b472fd481ee1d4d86c164729a566",
				RestHeader = "691520b0ca0d7a87b2abc2ab",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1293,
				BlockHeader = "3f6c54233f6c5423660664f5b1cb752ec26ec4ba6cfe154e921ee715489203b73f38e3f67dcaebc77c910df24edd9474a7119412ca79f11aa9f6517c83a3eb6075ba2a065b5b0e133f6c5423",
				MidState = "f4993d15494597dc2e056a3cd34572c5bd6e707f2d8aa61250d2318685ee6f93",
				RestHeader = "062aba75130e5b5b23546c3f",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1294,
				BlockHeader = "2bd2ead82bd2ead8dc0929358442ea8ec00c427a8d17bc7a69e54f9f9cc3c0eda726f15cb8aaaf5641db07787beeff18281c3adcd5d7270245921fa8a07c0ec7f2abcc36d1dd0e642bd2ead8",
				MidState = "e170d21689ff518fd1ecdbfaf920bceaef816cdf625a42b0eab724db88a2698f",
				RestHeader = "36ccabf2640eddd1d8ead22b",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1295,
				BlockHeader = "be80831ebe80831e57645d92719a4d21049a27e3e695a387323255f72e9cb696a06b97ceb4bcbd552382f9b8a2b581983496657652db057cad90368166ebc5dd655edf2f796f3b7fbe80831e",
				MidState = "9dd5abd36c109db8d1d1c65c6fe277778095a9c0aeadc4287914c99f82e8efc7",
				RestHeader = "2fdf5e657f3b6f791e8380be",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1296,
				BlockHeader = "e8bff93be8bff93b0097c005a12b6c51271610caea2ffcd3c004e3568125d03bfcdf98b20b8592a8097034fc8fa4b0813e344a62a106905a93244d75aa0bd4bd0783a040aeec5e86e8bff93b",
				MidState = "699b3dfff9076ce1d647012430f543f699d4b0b36ec5f3b38732f6a9382bba20",
				RestHeader = "40a08307865eecae3bf9bfe8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1297,
				BlockHeader = "35d486c235d486c26936c9419c39dd6d3478e901cdaeaf24189fa9adf4e1dd77fe8d95ae23f84f55c71b385589eb55ae6f4bf23e7dee112b4e82ddf8a5b12d4cf9167a2bb11da51535d486c2",
				MidState = "b20e6940e813e2dc42f9d83cacd775d5d7ce91e2731f7d1058162a81884a9200",
				RestHeader = "2b7a16f915a51db1c286d435",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1298,
				BlockHeader = "f34219e7f34219e78309034c0fce0615d5c9ebb059033490cc1e16072228c971a6549382a5543e6f62018f317442c25076c88c7e6590589af7d3d826f76724cec00d577af701ec40f34219e7",
				MidState = "3693cba04f38d67af8cc195931d95a1baea66518da9381e0fed07896e29a867e",
				RestHeader = "7a570dc040ec01f7e71942f3",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1299,
				BlockHeader = "987a74d6987a74d633c3639366562c8bc7e2e5ae6892be054ea8dfed2d4320cfc71fccc17055a1a6dd4dd3b6c0dd0e9e9d418f84629bf2a89cd3285a8b1f867e672cb62ac445a8b5987a74d6",
				MidState = "ff54a4d8d5318925d79a8e85acf76a198119175c38354c21bbb006cb855826b3",
				RestHeader = "2ab62c67b5a845c4d6747a98",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1300,
				BlockHeader = "1a13723e1a13723e134ae057bc811c8b1a542179ecbc94ceccf50fd57604fe7fb63c3294f98974c141148e34dda26713421991d31ce6d074016d4067681bf836e3e704140193ede61a13723e",
				MidState = "4843ed82648c97304f228aca3fdddb909cf6a8cffa708c803e782fe0bf45e93a",
				RestHeader = "1404e7e3e6ed93013e72131a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1301,
				BlockHeader = "6728ffc56728ffc57ce9e992b78f8da726b6fab0cf3c47202490d52ce9bf0bbbb8ea2f8fa11ac98cf1c0dcc7ab47fdaf37185aa6be2c16efe75c35b84549f361dd144555c0eda8306728ffc5",
				MidState = "10838ffe5738717b7597eb1511385781e02148659951ada2b2dc6a625e391935",
				RestHeader = "554514dd30a8edc0c5ff2867",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1302,
				BlockHeader = "b53c8c4db53c8c4de589f3ceb29dfec33219d3e7b3bcfb717c2c9c835d7b17f7ba992c8b6b0fe745fccac46065bb5ebd5b29cafafe38628dfaa3b24c7cb12f170e9fac4d0962285bb53c8c4d",
				MidState = "035fb7c9764dfb69879b6184b03254f7cedfe514c4a17c69b89419d9f2711fe7",
				RestHeader = "4dac9f0e5b2862094d8c3cb5",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1303,
				BlockHeader = "035119d5035119d54e28fc0aadab6fdf3f7bac1e973baec2d4c762dad0372433bc472986a4410094cc45b25b60e97280eb74cc05d21d5f52206ffab3249fe3346aa3e8550fd5e3b6035119d5",
				MidState = "8d694d7fd8ec3f3b5f4589b1222107b8a9cedeea242a966baf235e5f5cbe0e74",
				RestHeader = "55e8a36ab6e3d50fd5195103",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1304,
				BlockHeader = "e8995158e8995158024138948c9672ffe06c044f8ebee9938cb0337ad162f9953a205c787acf379dce878bf7b9afa202de2608ea6c7f388fe0ee1a96ef520dacd7cb8233dd5fbc3fe8995158",
				MidState = "659d656ec2cfc69dafc6d98d8e443111d87c4863556e733093026b1fe4b716bc",
				RestHeader = "3382cbd73fbc5fdd585199e8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1305,
				BlockHeader = "98e0862c98e0862ce1651e59c36c8a4f3d005dd88795a89103fdf3e60baee94708406931ba905fda0c5cdfc3856be92d4a01d20f5d47baecb35fdcdf9ab52ba073c744654bf894e698e0862c",
				MidState = "005d7c1a9abe30e94dea6b2c53160c0cba2d7c3b83b5a1e724c6f16662273e3c",
				RestHeader = "6544c773e694f84b2c86e098",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1306,
				BlockHeader = "3895475038954750c3c74e776793664c0cb44efaadf1dfa8b6a57e9aa858f14c86c4eafb9af4acf9ad512f44593d06eb75854daa1123a23abd28b3fca609cd353732b61f765e150b38954750",
				MidState = "3507299d257d624b057cc058d94e366281610f613e2168bce91f8e634ff02608",
				RestHeader = "1fb632370b155e7650479538",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1307,
				BlockHeader = "d4be615fd4be615f950661ef5eaf48842578006874f1464b66db0b478ed00ac48a21e4f241d45cf9e0dc81f1f08947f5a7549d7bebe78910645ee7a565d342ef16363a5b4e39aeffd4be615f",
				MidState = "91dc0583fa4cb5d6330eb69063597ecc7abacfbd28542d4d338b843126ac4f93",
				RestHeader = "5b3a3616ffae394e5f61bed4",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1308,
				BlockHeader = "3cefc4323cefc4321d63627a87209593ed61341338c8b47f1294e5fe5247c749d91a7d9a828838bd3d9816203f49e9d83f16d2a0acbad9d7d3bb55733766fc43ebc3ab6156b8c9683cefc432",
				MidState = "46249f8eca364994f4194008ced539155d3afb49c40e17f81c7a2f0efc5dd4d9",
				RestHeader = "61abc3eb68c9b85632c4ef3c",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1309,
				BlockHeader = "d818de41d818de41efa275f27e3c77cb0626e680ffc81b22c2cb72ab38bfe0c1de77779193b99301804fb9e2d86d21eb55b46803a73dcd5152f9a2d959978f904160442c17b42b5ed818de41",
				MidState = "a4bae4aafa96e4eac4b81b40921ad1bbfcab23476be15164589b899496a61e10",
				RestHeader = "2c4460415e2bb41741de18d8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1310,
				BlockHeader = "262c6bc9262c6bc958417e2d794ae8e71288bfb7e347ce731a663802ac7becfde025748db81ae67d4d8874316defef8544ce726230c246dfc92c1369ea095b3a509c437808a5db97262c6bc9",
				MidState = "26d7fffc94585276ee6006cd4a800bf023b7a8b32a2d979105dc448c901c48b1",
				RestHeader = "78439c5097dba508c96b2c26",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1311,
				BlockHeader = "9d6cff0c9d6cff0c36e2aa03a429fb4952420bb6dc2159782ae63cded48521985f2fb4c49d3ac4e3a92c998a45420bb13e6110ca04127c72d526a17569a05c017a4eaf772f3ab8a79d6cff0c",
				MidState = "dd3a659e63b2185ad27bd0887f8d162487e2fa20c37ee312ad51ce0059098bad",
				RestHeader = "77af4e7aa7b83a2f0cff6c9d",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1312,
				BlockHeader = "096736b0096736b02e6a22e4a5c7cf9ee2506de205397c9b7731c555c363b8f7afab0a38b14c7c3294226dca50632ec8dfca11d7e89b375aebd65e7edfed88307b64a367fda4a591096736b0",
				MidState = "682b198401259d62bc7e630f3bebdf8f1bc379fa7cc4341bc3cdca52f6d0b311",
				RestHeader = "67a3647b91a5a4fdb0366709",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1313,
				BlockHeader = "ccdbfb53ccdbfb535e8f38cbe7718037a211577c1c08f5424283ca9243b0ae88be4d854175132a4825dc7c1b7b484d1009ea24653187926a55e4dc5551aa1791e5a9c57644392ac3ccdbfb53",
				MidState = "31e2671b2d656f7322a93cb3eb82b54c87c6edb7b42fae791f9fa11cccb0a896",
				RestHeader = "76c5a9e5c32a394453fbdbcc",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1314,
				BlockHeader = "093ac141093ac141b785e3ec587f36b631a9177196e9c657de50014a032852430454c44d07f6faafec0f23782d35aff79015ece79ae37a5fc67c24d2d6c7036f93d379240d160e99093ac141",
				MidState = "26c1f9ea133c331aa0125969061884c7cef534fe781e3b659cf52457dcc2c148",
				RestHeader = "2479d393990e160d41c13a09",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1315,
				BlockHeader = "4228c1c04228c1c02364650d970a6f0af9d92bb0741fb5ef5eb68f5ce6dc407ef545725b9b9ef619effcebcb4598757fe951fb3778d2183cae03272db1de4f741e1a3a0857d2f2174228c1c0",
				MidState = "cd205eb1f9ef0a3f8468b4a699630e5014307b92c46898f54e4f6acbb4454c3e",
				RestHeader = "083a1a1e17f2d257c0c12842",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1316,
				BlockHeader = "4d5f78694d5f7869b9306c3e78d65db9449c74793ce8eca9ff5428955384bc5e5eccc128e8392cc85ddad6ac693b704a8536532caa8418174bc3ae865afcbcb137f3a820b5cece194d5f7869",
				MidState = "679e788a604c27e9e6bf49848384a90cce76c30f1d45679a26127819fc85bc7d",
				RestHeader = "20a8f33719ceceb569785f4d",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1317,
				BlockHeader = "0052f82b0052f82b915b517f78749de8cbc4d119cf92b56cdb4821112888505734fe948a834b2ebf411889fce426b81ab9c44c5be4fe6e8157874da96da88b940385955937d879ed0052f82b",
				MidState = "0b2f7c1670665d0b8a2eed0713540f0ed7109768b04d381ccf3f118bff42c25d",
				RestHeader = "59958503ed79d8372bf85200",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1318,
				BlockHeader = "9b7b123a9b7b123a639a63f76e8f7f21e388838796911c0f8b7faebf0e0069cf385b8e81843bb5ae0372cd86e8a393464e6ad64108e11414e825e149e9a1ff86d31a900b9c6b79b39b7b123a",
				MidState = "7d2e45554460359289a9787ecac4497cf9da2a217498605d5dd4719f632ca679",
				RestHeader = "0b901ad3b3796b9c3a127b9b",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1319,
				BlockHeader = "be84a2d9be84a2d9ef2cf1bfd5a6cc769c109461fe7622486af9b84ea467a8f7b9b1f0e646d619b949ae4242fd28d6b277675666205355d5a13ec5fbcabbf1ff554a9f5215156b2bbe84a2d9",
				MidState = "70ec63ad076edca046b8464e6ff382ed5750222ed0c20893edb69aefccaf2c3d",
				RestHeader = "529f4a552b6b1515d9a284be",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1320,
				BlockHeader = "abf26eedabf26eedcbf31208efad0ae3ef2361ed74f21382a09fef39f0f1fe7a4a3222ed0327db21033e7963df94b8b029fb64210ba3555d50e0b0e96244cf799d8d0d1becd8213cabf26eed",
				MidState = "223a20f18e65f1d6e5fa56aabbcd77acc0b4829c4c26e9c4c59cee3b55df44c3",
				RestHeader = "1b0d8d9d3c21d8eced6ef2ab",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1321,
				BlockHeader = "744fd326744fd3267c3a2c6da8b6aae51c7117bc51d0a1dd0b1ed25b776f287ca34c4b9763d358e8e3bd6de43a62b782ddc264139b3683526671bfc1fc0d151155c86830eb085638744fd326",
				MidState = "0a8b2d3d23c8daaffa7af686979c84ba70c38bc8036a04ab0ea53d9035ea3d6f",
				RestHeader = "3068c855385608eb26d34f74",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1322,
				BlockHeader = "c26460adc26460ade5da36a9a3c41b0128d4f0f33450552f63b999b2ea2b34b8a5fa48920005f2c5fe273f7585432322bac1a5db50c99ea7d1de156f4da27db55d25b759d65620b3c26460ad",
				MidState = "83d5b9b2250f2c0eb23635171ad4c9ceb83aa642670d3f63c97ea55bc5c079a1",
				RestHeader = "59b7255db32056d6ad6064c2",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1323,
				BlockHeader = "5e8d7abc5e8d7abcb71948219ae0fd394198a261fb4fbbd213f02660d1a34d30a9574289b810d965057cf76f46f62f8348e9e865b4237562c4da72b47bf621280f8f0a6e9207f61a5e8d7abc",
				MidState = "39c506c790907e415c3bc6e0e3ee225ead6a459f0f8381b7fbf2d8c319d0d8ab",
				RestHeader = "6e0a8f0f1af60792bc7a8d5e",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1324,
				BlockHeader = "cb8f1c0bcb8f1c0b98d5785830e45904a2406f5f60fafb02ac502a939b717f8d4e98876d66cd294d496dae2adf4a66ca169c778d65a726b25b3a32ab2d7cb60451306b5d13954403cb8f1c0b",
				MidState = "069037105d3e8847ce2db8bbfaf1ca989cefcf9bce7d7cb64577117d00c144d2",
				RestHeader = "5d6b3051034495130b1c8fcb",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1325,
				BlockHeader = "a6b0b578a6b0b57838b789aa265ad00c1e82e8767c07f9422cbbe1a2be751a7811ac0c4d146998870f671421112cf481811a0e5b5b5e58882c30c46a272c0efa4b1c2155766e38b7a6b0b578",
				MidState = "b39f7b73c7148c7e4b71e88b19190e604eda6d2c00e168158ace23065262b95b",
				RestHeader = "55211c4bb7386e7678b5b0a6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1326,
				BlockHeader = "67b8361a67b8361a6a148bcf27003b3cbb0521cd27f962a55c87b74181e9980552f48164d408bc2fff99c59754c06cdee954cb786c68aeb95de127363c168096530d7524a791a42a67b8361a",
				MidState = "416e75e31dd55f4e3731b09d926d407fbf2a6d96f239260efb9b433975e050b2",
				RestHeader = "24750d532aa491a71a36b867",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1327,
				BlockHeader = "b4cdc3a1b4cdc3a1d3b4940b220eac58c867fa040b7916f6b4227e98f5a5a44155a27e5ff10521146eec6deb80ae33ed2703bbdcccd0f789506aac4b80a917c00aba764a3c6a5bc5b4cdc3a1",
				MidState = "f8581aea2c7f95c0739e8008d1ccd4eeeb7280de5072e87610a5ca62362dd73a",
				RestHeader = "4a76ba0ac55b6a3ca1c3cdb4",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1328,
				BlockHeader = "b70e6f18b70e6f180b6cffecd90d8e35d573cd9138a1aa535b75f49e71d9e349c65e8244e4136b93b7a67a541a7b38a531ee47eca77c79bc8408ec1ac95f7b89319f173ea686de5db70e6f18",
				MidState = "6deb6df0b2cea2b1ed1745e71802df366b1bbffb1b47cc7749de0d91d68a4118",
				RestHeader = "3e179f315dde86a6186f0eb7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1329,
				BlockHeader = "b6080fc5b6080fc56e465497224b231897d35962d93a9abf35e77024d0577915efebb55c8e0f5164f4d99de6b5062f7a9fded41e97cb956ead3e18ffc4bf812b88890509c8e3d061b6080fc5",
				MidState = "0d7837c02652eeec754b15e8d530568ce1ad2aac4e970ebbdccc9f68741dd8ed",
				RestHeader = "0905898861d0e3c8c50f08b6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1330,
				BlockHeader = "031d9c4d031d9c4dd7e65ed31e599434a3353299bcb94d108d83377b43138650f19ab2570c1a068bccd35e86057d0ba21ae1c6f4d68d18935d7df23aec7ea77d2a8945054d3eeaeb031d9c4d",
				MidState = "c6a931f9a7682e014744015865748918970397f2562220acfc3531beebe543c9",
				RestHeader = "0545892aebea3e4d4d9c1d03",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1331,
				BlockHeader = "513129d4513129d44085670e19670550af980bd0a0390162e51efdd2b6cf928cf448af531f7ed7eabbfc5d6a946c00577bab3018ae219939ffd341fdba660bf194cd8b55626abb87513129d4",
				MidState = "d97813a3e43f8b3108379a10b8cfe2d5ed34d0a8f847ccdd9c38d9f27ff94578",
				RestHeader = "558bcd9487bb6a62d4293151",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1332,
				BlockHeader = "d052b310d052b3103b11cfa4a9d4d275c9be0e235847923dbc333c2ec8959ade5e1c625045ba30ebb2095ec498e3a0c0f84e283baa3476e56d1ab194e2146b770d9231233b472321d052b310",
				MidState = "701f0aa488be99ed1fadac1754254d335f5181fe19eb1476c6f12fda74516f7b",
				RestHeader = "2331920d2123473b10b352d0",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1333,
				BlockHeader = "6c7bcd1f6c7bcd1f0d50e21ca0f0b4ade182c0911f47f9e06c69c9dbae0db35662795c4707d53de6d6fe4d902dddcda7dd263621494411be0e046d9b1c4244b85b1ed0611a7fea046c7bcd1f",
				MidState = "11f418657c4dd6c7eafb389b808b0476275c7516a2368939f6b098aac9c056d2",
				RestHeader = "61d01e5b04ea7f1a1fcd7b6c",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1334,
				BlockHeader = "cceafe09cceafe092771b792f46679a175a10e245e5e7e688695f9dd715343b6b26e8cadd40270dbea59c104f34d490e8c7a14cddee3532623b810b15bedfe2c2ffbbf503584ff9acceafe09",
				MidState = "ad27142791aaa1e2aaf43d1126bb7d4ba75a459f399a494d172f7e79464de7b2",
				RestHeader = "50bffb2f9aff843509feeacc",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1335,
				BlockHeader = "a80b9777a80b9777c752c8e5e9dcf1a9f1e3873b7a6c7ba90600b0ec9457dfa17482118efa078558f4cbcad38c50a6d680f5fcda44ddd960b4cb7a10212bb00e8d55f4307dedcd36a80b9777",
				MidState = "4db4cf439df3127d25378d549200e0d98d140a9911e0066253d294f00506afe7",
				RestHeader = "30f4558d36cded7d77970ba8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1336,
				BlockHeader = "6813181868131818f9b0c90aea815bd98e66c092255de50b36cc868a58cb5c2eb6ca86a4d9dd09cb773ce95580964b112c0c8f6daeeba334f9d310209b5925eaf2f57f06d622748b68131818",
				MidState = "10be671c8ad37c9113ec23d0267391bb3630dd5fa604fafe11c7f37848b92794",
				RestHeader = "067ff5f28b7422d618181368",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1337,
				BlockHeader = "2803d58a2803d58a46402777e2ff2ecc71b47a59df22456723dd391d6e493898e9a32124485fbfba589001d30fe952e4303d6045557399a5df9c0aea400260fca3ba3a440bb7724c2803d58a",
				MidState = "4107e891718d0c89ecb478c153c60124f34208b257a5128941d3ec31a645ec39",
				RestHeader = "443abaa34c72b70b8ad50328",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1338,
				BlockHeader = "087af84e087af84eeca5ee4abcaca5c9cb8d8ee2d25bcf0bb5d7b00632f663b913c0825ca7b357d05ea2554043cc5399788ccb27b901f32918c1d7286744b3daad59040dfd18dcb1087af84e",
				MidState = "d7ec08ffe8ae40adbaf9201682ad9e221bdc1b765066f47c9499c4f07a762aee",
				RestHeader = "0d0459adb1dc18fd4ef87a08",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1339,
				BlockHeader = "568f85d6568f85d65545f786b7ba16e5d7ef6719b6db825d0d72765da5b26ff5156e7f575203bc3c64b94b78eedaeeff45fcf4d59bdd990ce9b5ef48610d96d4d7893f13322c7c10568f85d6",
				MidState = "936a5a08c1c1e556db5b05626d7e1a931768b97519a45e27a6aff592762d853b",
				RestHeader = "133f89d7107c2c32d6858f56",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1340,
				BlockHeader = "f8f94d74f8f94d74e59bf755fb81e5fb251b18e2f0ee30dfebeb5e93933b5b8f6a386fe8f630c756d1ed8571b9e2babf4c9327867b273d3aec064dfb6e14db0e810038707a81cb7cf8f94d74",
				MidState = "7ba5b240f6502743dbb048cb6a4f64cc4908eefe60ae0f319e0bc951d7b11dfd",
				RestHeader = "703800817ccb817a744df9f8",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1341,
				BlockHeader = "460edafb460edafb4e3a0091f68f5618317df119d36ee430438725ea06f667cb6ce66be384ca92004d9368135e694b6cff3f6fa06d34be4d8647f8ef298517aeecba59263c6af493460edafb",
				MidState = "4dab31fe98713de7dfa6c4093038cb7cffeb46cae81e9c66497b51323a8a8d2f",
				RestHeader = "2659baec93f46a3cfbda0e46",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1342,
				BlockHeader = "40d051b840d051b8b89b468c4eba2704b808a4486c7e832124f1bd40571d273231b281ad5fb636ecd84999126f230ac70bab10af2afdcc385d0e36793f1c13e9bb7984614034c3fc40d051b8",
				MidState = "ebb5f58d712236cd899ab634a860c49a567cdcb89b55a19c246fe51546c5fe82",
				RestHeader = "618479bbfcc33440b851d040",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1343,
				BlockHeader = "8ee5de408ee5de40213b4fc849c89820c46a7d7f50fe36737c8c8497cad9346e33607ea9abd36d9c013014651862b9cfe6b34166d55823b2e1d2c9e2ebbc7b7fadbee638163d07298ee5de40",
				MidState = "1a376f11d6d2842d79db3311a160ceb68350acc45852948f9c77e9d22837c559",
				RestHeader = "38e6bead29073d1640dee58e",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1344,
				BlockHeader = "a62e20dea62e20de0f73a416e777cad8f7ed12495ebbc1b9a8e1333298cc14bd295d951a40f560ad636f16651748a57b72c50de0ec7f9f1fd8db631322371c579e72973ab5b5917ea62e20de",
				MidState = "8199520847e530374e8a02a37b07030397e48b370e4d8fa5a2b14bfce525cf3b",
				RestHeader = "3a97729e7e91b5b5de202ea6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1345,
				BlockHeader = "906cc774906cc7744a52c0cad8a01d2c1c149dee093adbadb0b28736f20039712f688c0c453332a35ccbb94444be0a001f039f64232eef35951f0286c2ef314cd4d63b1ac20e634a906cc774",
				MidState = "c437bea8484e007fa24b99da163b2b53c2437253014e161694c1aa49fdbb7796",
				RestHeader = "1a3bd6d44a630ec274c76c90",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1346,
				BlockHeader = "ec29aff8ec29aff8e6aa8f4eab412d7dd517e56081c7e49269ff9ade30bda3c63f6aabb04ed7cdb49db84cd6e54f6fd68ab3cd8e3b7a56736a6da72753fbb4a0febbc135eb6b02b6ec29aff8",
				MidState = "8a28abe416ef51e409b21e6c881f91942e2d410a5b46edc4537008508d4ad304",
				RestHeader = "35c1bbfeb6026bebf8af29ec",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1347,
				BlockHeader = "3a3d3c803a3d3c804f4a988aa64e9e99e279be97644798e3c19a6035a379af024118a8acb08648db9908ed1d5ca21a797836e7383f1614583b0cb852f8ecdd1df21106677261f4493a3d3c80",
				MidState = "1eb0ab8396033ed0b8a9a9690a386d7e6ee7de5e7a205aa15211312944acc4d5",
				RestHeader = "670611f249f46172803c3d3a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1348,
				BlockHeader = "5ce829c65ce829c65de41ec37f9222266f3bf9287b4f3ceea68ef30d7f64336ac536cbca6c6504a60a3497e48356ee400448f4d890746648e21a3d1413efa3caa0034a28e9d43f675ce829c6",
				MidState = "ed6e90361bd0a4c5b3a50b5085b8a1227e10d490912f9e6105853c6c40904ad3",
				RestHeader = "284a03a0673fd4e9c629e85c",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1349,
				BlockHeader = "f71144d5f71144d52f23303b75ae045f87ffab96424fa39156c580ba66dc4be2c992c5c12f72aea6f4bd5098a10b2c62cac66566859eed22c826c5fe39edcac6bd83962f863fa92cf71144d5",
				MidState = "2027b8f82a3cd78717db9094d85caf42a20819287372bc5b27e062f540fbf217",
				RestHeader = "2f9683bd2ca93f86d54411f7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1350,
				BlockHeader = "fb12b007fb12b007fa3f9c710de2d0cefbf6354b101a98ef6a7c4f7122ed45b3e9e30b40bd44d1aacac1d4a33e2fdef156ba54492c278fd00dd4f646c49de2968f598b53c4e8f274fb12b007",
				MidState = "e1a5c31bc1c6f38c6ebad5817907618975009f248b093fe2b7e0a4caca903667",
				RestHeader = "538b598f74f2e8c407b012fb",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1351,
				BlockHeader = "49273d8e49273d8e63dea5ad08f041ea07580e82f49a4b40c21716c795a952eeeb91083b5cd7f86902e9b672781e6b046184388b4fc49c077f6f8ef5cf5e1376624be00ad28f86ef49273d8e",
				MidState = "e9bf0813fae62617df9d2368f3e1a38ed77c3890eaa29b0b58d063c25b8ec19a",
				RestHeader = "0ae04b62ef868fd28e3d2749",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1352,
				BlockHeader = "973bca16973bca16cc7dafe903feb20714bae7b9d719fe921ab2dc1e08655e2aed3f05379820827612fda53b55824f412ccd356eaa8b72c82b2edef13d30e169ffad7c0449df7759973bca16",
				MidState = "6531bcac6b2fd7b9c8f5702b2d476070a27ffb41ee6a20c54a31d780af4d00b2",
				RestHeader = "047cadff5977df4916ca3b97",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1353,
				BlockHeader = "e4ec2b6ae4ec2b6a64aad857680cdfd004f8af432260a8d9c417e6f19ad04ceea1907b99bf334c06d7cdd35763a5d7d39eaa7b4266ccdd79f072ec4105147bda075615473b614efee4ec2b6a",
				MidState = "fae28f660f0cbd0974e00d59be25874780d0fff8ea432bec2419233c0f26064e",
				RestHeader = "47155607fe4e613b6a2bece4",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1354,
				BlockHeader = "1cc9996b1cc9996b4c2faeb66b1cd0967779c71d41f15134133e2f9b61e0739a0320b0db076b71ddab65fb5908d5ed3c67db270805f177210f48e5f1e1fc37ea56e8b54e0bdd9e571cc9996b",
				MidState = "830743141828007441201e5739df15f8addb3b718c0d98cfec7e9f94b83d51f6",
				RestHeader = "4eb5e856579edd0b6b99c91c",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1355,
				BlockHeader = "6add26f36add26f3b5ceb7f2662a41b284dba054257005866bd9f5f2d49c7fd605ceadd77e834d05fb69dcddc9f793ef6dc7aab9d5a9ba3b2b44393012adbbdb92202e7c502f01ec6add26f3",
				MidState = "311ccc63f5bd823b8be15b209204bf348e2d1634af8f1143c3440838b707ac9b",
				RestHeader = "7c2e2092ec012f50f326dd6a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1356,
				BlockHeader = "056fd5f4056fd5f45dd2188ec0b418234499178efca05a6b21a4926245d14537f866b9979a12f3b75027facf741ed09bf684a9d4c89a51bd392714271d25412b8e47ae088b20fe5d056fd5f4",
				MidState = "32c76e76172c37a49d9981ed65d9cd450dd5820cd67d2d5e8245be96a18b7185",
				RestHeader = "08ae478e5dfe208bf4d56f05",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1357,
				BlockHeader = "952cb5a2952cb5a2e095f65f8d11c279cd6af3ea165e57fb62cb02f8b7a7dbe510044e787f288530f03934c84cf60613a920b2abe29d4509da83cc5cee332541defaee795269acfc952cb5a2",
				MidState = "dfce20bfef6e636abea19769eb97b6d3e8fe7356cd5f4e1d3f7df70588708a7f",
				RestHeader = "79eefadefcac6952a2b52c95",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1358,
				BlockHeader = "4704eec64704eec6e7a321aa920835aaf731d587870e437f7b276062bec9903e500dbe38bf638f4428df7af3a86aaef54c2574ea09d16fb39eba3c2f3b7a2bd11501a91a1af5f86c4704eec6",
				MidState = "076412900fbc2ca8b5e9b6c0fe2538117e9341200eb3a951dd34c225cf4023a4",
				RestHeader = "1aa901156cf8f51ac6ee0447",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1359,
				BlockHeader = "ef4004bbef4004bb3f464e637904f5c7caf1d0fb5881416baf37af3162816e7a1f14c2c362ea22192101603a8c279dd2d589ba956ec7b0489db29c49ab13e6076407456827cd4fbcef4004bb",
				MidState = "cc1111c0de07a006a5f6d3fda4af4f2461dd0614910674b4a4da94e2d21b8a18",
				RestHeader = "68450764bc4fcd27bb0440ef",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1360,
				BlockHeader = "18762ab018762ab048c668f26988deec52952249570ef1fd873e2d96f94116a1e4d7449f91b07771c961b6a471d3995cf19469c47d30e70976d80a8c49db1b98a35d6e4ef3e0102418762ab0",
				MidState = "6c1a529075d7b85c7d4cade06d06fffe62e7c0b6fb39837ef14f54b792079390",
				RestHeader = "4e6e5da32410e0f3b02a7618",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1361,
				BlockHeader = "e6db9e4ae6db9e4abf37f349b4e83a3b39c839bef001b0efad3db40f525b74f2f2842e5b352b352d9c5c1b4d28d72e6d88f9203beb4c19cd67349abfc8134f552aba007e1bc01107e6db9e4a",
				MidState = "3c4fe0133fbae8e6f54ec266c407b5517dba07bbabef0f35dbf8b912b63a334a",
				RestHeader = "7e00ba2a0711c01b4a9edbe6",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1362,
				BlockHeader = "645d1428645d1428fecd81d62d379ab1d437b40b9f5ea24132e5880d9a912f8ae3ca8f9134530b052c3f2c1bad0424c7a5a6f4eba6b8038a7031cfa9d7a757618b348f7ec49eeefd645d1428",
				MidState = "00c889fa58f8b2dab841a96dc8dc885950391f4c153b7b6b1f8e95f706e2b190",
				RestHeader = "7e8f348bfdee9ec428145d64",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1363,
				BlockHeader = "b272a1b0b272a1b0676d8a1228450bcee0998d4283de56928a804f640e4d3bc6e5788c8df8d4a03d0248f1c5e9e25a097e3fa9d7d4349d1d0c7e0920cf795bdd7508cf5b860a8046b272a1b0",
				MidState = "e4f54ee40bda94f64204b6374d4a83df5de1a8988eaee7c117befca7819885e9",
				RestHeader = "5bcf087546800a86b0a172b2",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1364,
				BlockHeader = "ebe3d3e8ebe3d3e86d35f3960366333637fa9789a614f6be7549f273e1233dad2ae6958e0d3138a791eeeddfb6b02f721414bcc581239d63384f5a79b97607f38fd19b39f5727664ebe3d3e8",
				MidState = "8b069a35fdd9a610b7dcb1486588ffb991727bc9cdf2aafa1fa52726aa3ef52e",
				RestHeader = "399bd18f647672f5e8d3e3eb",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1365,
				BlockHeader = "abeb5589abeb55899f93f4bb040c9e66d47dd0df51055f20a414c811a497bb3a6c2d0aa4ec457690497bbec4ef16d1bd1bab6a4e8369de597e81be9050116b27dbca5c122e170545abeb5589",
				MidState = "664ca73b09f3df6acd227d17e6560b916036d7edf707e07bd8d2da6c782b6520",
				RestHeader = "125ccadb4505172e8955ebab",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1366,
				BlockHeader = "f42e025ef42e025e9d5dfda1fb9ee498bf15946275ac83b41ea3aa4bf4f0d0103bef5f4b36641a2b97bf9fe56189ad4f00ac4386f38b5cc925b55c4b0ff694b4163be71a2761b220f42e025e",
				MidState = "a83c751dd56f5a13150cb50ff9cdc3111ca644fbada2a7aabc8f2bae461f56a8",
				RestHeader = "1ae73b1620b261275e022ef4",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1367,
				BlockHeader = "dd6ca9f5dd6ca9f5d83c1955edc837ece43c1f06202b9da82675fd4f4e24f5c441fa563e9a0bdc69b9d8e35d0469482d7f3ac331550763ec107caee5ba00337bf3f54c147340505add6ca9f5",
				MidState = "e45d54121d98a50ec74fc0c2d7ecdf8ef7561f864b79e2275762113948b44f48",
				RestHeader = "144cf5f35a504073f5a96cdd",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1368,
				BlockHeader = "629152be629152be9865544f8642f56f5b0f6195f0aac6f528cb5382163f281aa223192e5f365b83a09f41934865304b408f6ad43ac7d42f33d2f6943b7e920ebec22f3203f7e051629152be",
				MidState = "f7866120872d82ee77ae23628c4bb84de86b7ba33924b738519de83aaee4d6f2",
				RestHeader = "322fc2be51e0f703be529162",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1369,
				BlockHeader = "b0a5df45b0a5df4501045d8a8250668c67713accd329794780671ad88afb3555a4d1162991d9f6c2505e5bcafacc5bdc77ab9b5047099ed0e66768a82a30783f1f315d6af1401ed0b0a5df45",
				MidState = "322b217c2ac27b98db17e07776a1f9e4e4ac6b11fbfd88ade74166c0f67122ab",
				RestHeader = "6a5d311fd01e40f145dfa5b0",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1370,
				BlockHeader = "feba6ccdfeba6ccd6aa466c67d5ed7a874d31303b7a92c98d802e02ffdb74191a67f1325f0ab3d6b481b1f0ae7cbb32670d8c673306c2c2590f56e4dbe3ced3506c56d4ad28ae5aafeba6ccd",
				MidState = "bb7b6a33c6945e415afe526904da9d9d24fd2718b15bcd49a7c5c3b50644fcb8",
				RestHeader = "4a6dc506aae58ad2cd6cbafe",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1371,
				BlockHeader = "3114b9ed3114b9ed8696dcc4123265ecf38bb3b6c059cea72067b167e3799e0a46676a02f183dcee0fac11fcdb76f3c47e6f7c93c93e99f2f4fd8abdeead30a9a16c20537db25cd73114b9ed",
				MidState = "44b882a5a7a1cc950c471ecfc07875c4dfd2ad053da8c5e9186708228f398738",
				RestHeader = "53206ca1d75cb27dedb91431",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1372,
				BlockHeader = "82d6084c82d6084ce902b3431b2de3df571d20ba10fed8d0a5c8778e7e11d4b32fc706b783a1b5d9785ce839559f9a8d779cd6be2d113222b5cd11ffc8dea2568db1bd28ef9b0dff82d6084c",
				MidState = "ce0b86982553dc79295a41f0aab16b94c6d34bcfa0ad8ace4890ad9a31f44c01",
				RestHeader = "28bdb18dff0d9bef4c08d682",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1373,
				BlockHeader = "6465b6e66465b6e601d70ade68fabd24d76bbcd0c4fdc8fdb20072a50d7289054f9f42d6d2e83c772e7efbb0dd4b04ea28cffcffb8e50d8fffacd443bb0d9fcea4976725ba4aec726465b6e6",
				MidState = "e2bf1cfaa453934658df1e897dfa195915832feff45094bd77fdefc3f8433801",
				RestHeader = "256797a472ec4abae6b66564",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1374,
				BlockHeader = "20355574203555742c07791b41fcd8ded061a2659a67c34f82a67efae256fd70b0d21fccd3c9474c73e6422ff74a95bf8bc719763386625a0dec7eac806eef6c7c76c993200989fb20355574",
				MidState = "17abcf18737bffd3b7aa53d87c5b1cc8eee5326c5deeb5295f549b1f996ec3f5",
				RestHeader = "93c9767cfb89092074553520",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1375,
				BlockHeader = "6e49e2fb6e49e2fb95a682573d094afaddc37b9c7de777a1da414551551209acb2811cc7f4ebc663c428b2b029742364851721efb5477a8da6ec42409d507017eb5d3efff72545456e49e2fb",
				MidState = "b87d62e5dc5993805bbd801b0924cd0865c9a267754f470cb865a6c29e567c22",
				RestHeader = "ff3e5deb454525f7fbe2496e",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1376,
				BlockHeader = "964e750d964e750de8030eff6de5b3da7e6e630c3ca08dff7414cb57ff1fb435b44a397a053661c594878fea3ea8b1d9e8e320190bd5945047934bfa7b0bde313910e1cc2bd5acc8964e750d",
				MidState = "08825391f74793fa2b9cee2a7da1255ed3a83b9842e517c2ef579d60a496c960",
				RestHeader = "cce11039c8acd52b0d754e96",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1377,
				BlockHeader = "3176901c3176901cba422076630095129733157a049ff4a2244b5804e596cdacb9a733712436984aac051cbe1e502726efa3a561cf77302c32789652c28b694ae6a334d5d9f12ba13176901c",
				MidState = "13dfd2929d49518861235cb28d95fd1181a065bf88fbe74f110396a5f78e138b",
				RestHeader = "d534a3e6a12bf1d91c907631",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1378,
				BlockHeader = "7f8b1da37f8b1da323e229b25e0e062ea395eeb1e71fa7f37ce61e5b5952d9e8bb55306dc2c8db71c004effddfa748fafd79d3904ee4a2bbaca35a7fd19a2a6b609823a72788e77e7f8b1da3",
				MidState = "df1a9e2d6ac86879c7e50c9b640ff2a527da239501d16131fdd472f50c4589bc",
				RestHeader = "a72398607ee78827a31d8b7f",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1379,
				BlockHeader = "f31def0ef31def0e07b688457c000bb98b20a7e273d3db205fa023665981e51753add1ea570e4c0f05020704ad55dc5bc189a97a888af6048823f53607d35e21a92a8bf5bfa8b990f31def0e",
				MidState = "f88da3259dc6c76079c8a29fa99b467ad4276cca948b58c5f4427e707e602c67",
				RestHeader = "f58b2aa990b9a8bf0eef1df3",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1380,
				BlockHeader = "40317c9640317c9670569181770e7cd59882801957528f72b73be9bdcd3cf153555bcee666334225d0766f9c7d93f4eee1cc929c0b89dcf658dc0c6fc310be252adb7cc73a32d35140317c96",
				MidState = "7bf9279e14edfcf6d5ab22ee956689c3f235b08ff5faff6f70127f2ba477a4f8",
				RestHeader = "c77cdb2a51d3323a967c3140",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1381,
				BlockHeader = "5810eab15810eab1fb62ded3013107538830b765b4f583e2dd5861b36591b8889531fa2d6aee851dc6ae834e1fb0f1779e0c5eca4f8e0e301ee44e54aeabdee0a6b6218ecde4fa1a5810eab1",
				MidState = "9a217ff9b1643464106e926e15e92b8a12499bf676864ad9d137cff3e7ad4567",
				RestHeader = "8e21b6a61afae4cdb1ea1058",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1382,
				BlockHeader = "25948a3925948a395bf07b392329acc5b46b0189b50c2f129d6e5d76150e476d502a49b13740729180f06347cee70390ee5db81c00e8d61d781780d5944b21b4dcd75ccae87c0dfb25948a39",
				MidState = "c4c50f87e07abfb227f53065267187b953e113605a62da587fdead26bc3c2ee9",
				RestHeader = "ca5cd7dcfb0d7ce8398a9425",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1383,
				BlockHeader = "73a817c173a817c1c49084751e361de1c1cddac0988ce364f50a24cd88ca53a953d846ac21d0603367cf6b44a9d3533042bbb08bc13fa5df7b57df15811362db0f4e9f86c04d43c673a817c1",
				MidState = "1233ca8ac0bded9596673e76b889c3985d075e5fd6aa8c39965c41f53bc0b620",
				RestHeader = "869f4e0fc6434dc0c117a873",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1384,
				BlockHeader = "232ddb97232ddb979c2e852d9aaa31b1f51bf468ec0651d06e5827eb13d2ea787b113a0c41dc2beca625a32257bc31b293c4b6fd38a292d24df5036cf62ef07bdcf5dde287b3cc0a232ddb97",
				MidState = "cacf03fcc6a65ecb9e93723f3cf0bd8cffcd1f0aebf3b6b80dee045f03b4f971",
				RestHeader = "e2ddf5dc0accb38797db2d23",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1385,
				BlockHeader = "5bd4c8e05bd4c8e0cd032f1754c6d843256c81fa261dc8d2ef0eedb7d1e70b3b5a695905a292014fbb37dbbf17362e52435d6222505cc9b3af2e62cb0c15f53a9d3ef5c4459d10105bd4c8e0",
				MidState = "4b54c3d74da594d80f74c49b831b2fbc4a538e4dd2998c9ca0166c6863241429",
				RestHeader = "c4f53e9d10109d45e0c8d45b",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1386,
				BlockHeader = "a9e85568a9e8556836a338534fd4495f31cf5a310a9d7b2447a9b30e44a317775d1856017ce0e0df5fadc599c30f83752bfeb5173a10c5c477c5422b4b12ea08a049e5ff99f28b4aa9e85568",
				MidState = "10b5b44c036700055d3b02f5076bd9e0fb36b2e0a28387b052ab1d22ff77a958",
				RestHeader = "ffe549a04a8bf2996855e8a9",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1387,
				BlockHeader = "f7fde2eff7fde2ef9f42418f4ae2ba7b3d313368ed1c2e759f447a65b85f24b35fc653fc44bb4befd23ebfe3c18865f147a308e7dbb6cc7be4cdee5304309f61dd47a1c223d27946f7fde2ef",
				MidState = "c4af2027a13bb85af65c9862900f80740cca23f27b17cecc3d6256201992171f",
				RestHeader = "c2a147dd4679d223efe2fdf7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1388,
				BlockHeader = "f78cdbbaf78cdbbadaa3409e6baac49296d651be097bd4d58892651bf26f2f0329920608ac3ee6510dee0801679148414399e0dcd858b1badb43fdf42da8ba65a35d4d9377cf455ff78cdbba",
				MidState = "4a38f69135c556a65bfc0c4c88da81bdf2d6fe8c3d505880bb44ada6553137aa",
				RestHeader = "934d5da35f45cf77badb8cf7",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1389,
				BlockHeader = "530abd52530abd527bbc2568b486f36c4982475b5312e13a78be6e52514c6e15ffdd65f5955564bc806f62b275ca4ffa9537af8e5c3578e263992e802ec47eeeef3077c92bd7b386530abd52",
				MidState = "7a94ffd5ca16413e4ecbd270c056c3b7ec45c1632ecc3d5e898a521a3078cfc5",
				RestHeader = "c97730ef86b3d72b52bd0a53",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1390,
				BlockHeader = "655477a7655477a770bb021ac695c631b7d3bc78bf9ee6dc6ec49ef79deb21df4933543780a2eaef422c6a2d9ed557376bf92a8fb6ee2fc8a1fc3b7f980a1a738e0ec7a47c6838eb655477a7",
				MidState = "fa6290ac68c32ef1b15dd89920cecc94c30117b0cae1b97d39618cca098ba4be",
				RestHeader = "a4c70e8eeb38687ca7775465",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1391,
				BlockHeader = "b268042eb268042ed95b0b56c2a2374dc43595afa21d992dc65f644d10a72d1b4be25133a50c44009b080027bb9d3e03f6b34bc8a0aa794b55f9efa8fa5c25981541c6d983625980b268042e",
				MidState = "0618d38df1ad5fd35ae65549177575d9653c94fabf8f093db89e07cfd67f1ed9",
				RestHeader = "d9c64115805962832e0468b2",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1392,
				BlockHeader = "14df3d4214df3d429e9aabd9905a3b820b1ba1392cc7493763e2aea1ddeca08ca13168faa745778d939ce62598cf07f924ffdd302ad820a0f2abe73c246481b13f575aa65fee00e814df3d42",
				MidState = "e10a259b317dd06fa1c2877794de5197a032842ab7f90952a92fee8dcda7fb09",
				RestHeader = "a65a573fe800ee5f423ddf14",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1393,
				BlockHeader = "98ae5ecd98ae5ecddd9db3c8dc4b0411cab273256a0598a2d99a297b8f3bd7c679034047022a9217664418abe0968becc0a5c4b7e57c5641d5dee5bb3cf2f5162e8d9deaaa10292498ae5ecd",
				MidState = "6ff2824a5a9cebf0741cb7a1e64915be40b39af64d74d3b1aa1843bb09dea1dc",
				RestHeader = "ea9d8d2e242910aacd5eae98",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1394,
				BlockHeader = "7045f5b17045f5b1abd5269c51f183ba98ba2f4026e95d4b33c750199c6195e8f9f582833887db81b4d2a1d1e6f3788de657ee250b01306b9ecb4f6ad9f6a8d23f6e6baf1d8634a47045f5b1",
				MidState = "053bcdab744e4f4dca639bb3c373f39f1408266169ae8e2b642d0539cc97c023",
				RestHeader = "af6b6e3fa434861db1f54570",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1395,
				BlockHeader = "c5c5207bc5c5207b8ba2a7585f30711c798e9bc1d007b7764f0f94b8ed56366578c75dadbead1b59458971153102d8904396e03b555ccc01961ca6b01ecb6e5f3747aec6c1c7e137c5c5207b",
				MidState = "4d7b87e6cb9d244c34800a35d76e624c6859322ff84329bbe7223746bd1a8515",
				RestHeader = "c6ae473737e1c7c17b20c5c5",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1396,
				BlockHeader = "eefb4670eefb46709423c1e64fb459400232ed0fcf9568082815111d8415de8c3d8adf897f54f09c72683763677d92f1fdaaef263607b7b369811a7da03c7887d53c3be480ae0467eefb4670",
				MidState = "13340a13615c7e2fdea533a4e0aa7cda44271a0672e05ea5d438764f323e5732",
				RestHeader = "e43b3cd56704ae807046fbee",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1397,
				BlockHeader = "170e2302170e230276477978f64013e40f7bf8409579466bc1b3e7e497fbbe353d5fdd60ff5590cde4b405dc9d5ff229e1b0a0f55973b047ac11be871705b2b3c9ba7ef5f1088acb170e2302",
				MidState = "c341b6514e0b3a395f99e4810335c2f310c3b03a25b94169beb2dfd339250c59",
				RestHeader = "f57ebac9cb8a08f102230e17",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1398,
				BlockHeader = "3a89eee33a89eee36a587d913d52b75a96ad1cb72570c7b2fec37b570c6ca4e915b8b71e11c6b560222e36fec61b331e01bbdd65751818e3f4f2fc42aa70ca4ecd80ca8106280d693a89eee3",
				MidState = "d4562e1ad00a454e705f4853f5d98bc9f99cb1f73431d53cdcc10667345e6290",
				RestHeader = "81ca80cd690d2806e3ee893a",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1399,
				BlockHeader = "09bf401809bf4018c73f85c9f6823492764f1b12375163e07bde026afe0d668477a0587b5c6318b8173c98b6e498f3e2a33b5c1fa58648f3be8db7c3b5466900dd7de1f8d15b26c309bf4018",
				MidState = "5556690d75082f786e66be38f27e71df43b32a68177f9b15202ba567a148b92a",
				RestHeader = "f8e17dddc3265bd11840bf09",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1400,
				BlockHeader = "56d3cd9f56d3cd9f30de8f05f190a5af83b1f4491bd01631d37ac9c172c972c0794e5576b1d98ee778b3f38158f1a7f8c01988df65656a15b7e244c27070936665222ede7f41a75556d3cd9f",
				MidState = "656ea10fbed043162c516eb8ce4ec4eadf1d297309fd85ed71efd54612169514",
				RestHeader = "de2e226555a7417f9fcdd356",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1401,
				BlockHeader = "a4e85a27a4e85a27997e9841ec9d16cb8f14cd80ff50c9832b158f17e5857efc7cfc527260464bc4e071c7a3e15b34f8bad741af312fcd02c7822a228f317ab9dcfe7c954ab9eed9a4e85a27",
				MidState = "2b0063dbb3cd5fefa6affa679c7f7027a8a66578cc2aade244350070044809e0",
				RestHeader = "957cfedcd9eeb94a275ae8a4",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1402,
				BlockHeader = "94ca108c94ca108c05c5f67ac236fc872b149821f98c56ce554370c24135482541a1effc5723251bb94244755f25efa1b1fc8fd3eb137afed402bd77596c75966c94a986ccff341794ca108c",
				MidState = "f2e4f0a8f16a7b068803854a2371c35cd103af946d50be0e71fc9bcb5a22dc1f",
				RestHeader = "86a9946c1734ffcc8c10ca94",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1403,
				BlockHeader = "bd003681bd0036810e461008b3bae4acb4b8ea6ff91906602e4aee28d7f5ef4c066471d8cca95d16cedf0ba8d3f34d8907382c7cd60606ac6e3b8dd147608be04fed49e27a60e3eebd003681",
				MidState = "05bf328d0c8cb025eb9f96d252a00b878f79f27856c1ea33e8a9406cebf12805",
				RestHeader = "e249ed4feee3607a813600bd",
				Nonce = "e0000001"
			},
			new SelfTestData () {
				Index = 1404,
				BlockHeader = "aa3880de1c2c74f8fb1afdc68a65d0f852bb8cd3518e7a536eaeb17c6b3cb137544704b1934f7b3b88d9ffc4f7eb1d5b368eb1c8db52b73c58faf4d25ed0b4d4131c5dc1f21f28f71c2c74f8",
				MidState = "b892a411cb98804d40e113a61802250150782ced2f2aa4c8d45c91095165125c",
				RestHeader = "c15d1c13f7281ff2f8742c1c",
				Nonce = "e0000001"
			},
		};
	}
}

