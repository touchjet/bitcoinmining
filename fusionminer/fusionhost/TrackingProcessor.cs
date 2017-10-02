using System;
using System.Net;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace fusionhost
{
	public class TrackingProcessor : TProcessor
	{
		private TProcessor processor;
		public TrackingProcessor(TProcessor processor)
		{
			this.processor = processor;
		}

		public Boolean Process(TProtocol inProt, TProtocol outProt)
		{
			TTransport t = inProt.Transport;
			IPAddress ip = ((IPEndPoint)((TTLSSocket)t).TcpClient.Client.RemoteEndPoint).Address;
			Console.WriteLine (ip.ToString ());
			bool result = processor.Process(inProt, outProt);
			Console.WriteLine (result.ToString ());
			return result;
		}
	}
}

