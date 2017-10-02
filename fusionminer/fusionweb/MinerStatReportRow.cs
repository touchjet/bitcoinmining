using System;

namespace fusionweb
{
	public class MinerStatReportRow
	{
		public long UniqueId { get; set; }

		public string SN { get; set; }

		public string NickName { get; set; }

		public string Version { get; set; }

		public DateTime LastStatusTime { get; set; }

		public DateTime LastMaintTime { get; set; }

		public string LocalIp{ get; set; }

		public string InternetIp{ get; set; }

		public TimeSpan UpTime{ get; set; }

		public double CurrentSpeed { get; set; }

		public double AverageSpeed{ get; set; }

		public long Accepted{ get; set; }

		public long HardwareError{ get; set; }

		public double BoardTemp{ get; set; }

		public double ChipTemp{ get; set; }

		public double ErrorRate { get { return HardwareError / (Accepted + HardwareError); } }
	}
}

