using System;
using Thrift.Transport;
using Thrift.Protocol;
using FusionMiner.Thrift;

namespace fusionutil
{
	public class SimpleMinerIntf
	{
		private TSocket _transport;
		private TBinaryProtocol _protocol;
		private MinerService.Client _client;

		public SimpleMinerIntf (string ip)
		{
			_transport = new TSocket (ip, 2087); 
			_protocol = new TBinaryProtocol (_transport);
			_client = new MinerService.Client (_protocol);
		}

		public bool Ping ()
		{
			bool result = false;
			try {
				_transport.Open ();
				result = _client.Ping ().Length > 0;
			} catch {
			} finally {
				_transport.Close ();
			}
			return result;
		}

		public MinerStatus GetStatus ()
		{
			_transport.Open ();
			var result = _client.GetStatus ();
			_transport.Close ();
			return result;
		}
	}
}

