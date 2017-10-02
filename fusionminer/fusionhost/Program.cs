using System;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using FusionMiner.Thrift;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.MySqlClient;
using System.Text;
using System.Collections.Generic;
using System.IO;
using FusionMiner;
using System.Threading;
using System.Net.Sockets;

namespace fusionhost
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Utility.DebugMode = true;
			Utility.PurgeLog ();
			if ((args.Length > 0) && (args [0].Equals ("scattergen"))) {
				ScatterCreator.GenerateScatterData ();
			}
			new Thread (new ThreadStart (() => {
				try {
					X509Certificate2 cert = new X509Certificate2 ("server.p12", "");
					HostServiceHandler hostServiceHandler = new HostServiceHandler ();
					TProcessor processor = new TrackingProcessor( new HostService.Processor (hostServiceHandler));
					TServerTransport serverTransport = new TTLSServerSocket (7802, 10000, cert);
					TServer server = new TThreadPoolServer (processor, serverTransport);
					Utility.Log (LogLevel.Warning, "FusionMiner SSL Host Started");
					server.Serve ();
				} catch (Exception e) {
					Utility.Log (LogLevel.Error, "Main-{0}", e.ToString ());
				}
			})).Start ();
			try {
				// create protocol factory, default to BinaryProtocol
				TProtocolFactory ProtocolFactory = new TBinaryProtocol.Factory(true,true);
				TServerTransport servertrans = new TServerSocket (new TcpListener(System.Net.IPAddress.Any, 7709),10000);
				TTransportFactory TransportFactory = new TFramedTransport.Factory();
				HostServiceHandler hostServiceHandler = new HostServiceHandler();
				HostService.Processor processor = new HostService.Processor (hostServiceHandler);

				TServer ServerEngine = new TThreadPoolServer (processor, servertrans, TransportFactory, ProtocolFactory);

				Utility.Log (LogLevel.Warning, "FusionMiner TCP Host Started");
				ServerEngine.Serve();

			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
			while (true)
				;
		}

	}
}
