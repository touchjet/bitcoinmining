using System;
using FusionMiner.Thrift;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using FusionMiner;

namespace fusionhost
{
	public static class UpgradePacker
	{
		public static string AssemblyDirectory {
			get {
				string codeBase = Assembly.GetExecutingAssembly ().CodeBase;
				UriBuilder uri = new UriBuilder (codeBase);
				string path = Uri.UnescapeDataString (uri.Path);
				return Path.GetDirectoryName (path);
			}
		}

		public static bool RemoteIsOldVersion (string remoteVer, string localVer)
		{
			int rver, lver;
			try {
				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}
				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}

				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}

				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer);
				lver = Convert.ToInt32 (localVer);
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}
				return false;
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return false;
			}
		}

		private static void AddStep ( List<MaintStep> steps, MaintStepType type, string path, string command, string localfile)
		{
			var step = new MaintStep ();
			step.Type = type;
			step.Path = path;
			step.Command = command;
			if (localfile != "") {
				step.FileData = File.ReadAllBytes (localfile);
				step.Md5 = Utility.GetMd5Hash (step.FileData);
			}
			steps.Add (step);
		}
//		private static DateTime _nestUpgrade = DateTime.UtcNow;

		public static Maintenance GetMaintenancePack (string version, string mac)
		{

			Maintenance maint = new Maintenance ();
			string updir = AssemblyDirectory + @"/upgrade/";
			if (File.Exists (updir + @"fusionminer.exe")) {
				string ver = FileVersionInfo.GetVersionInfo (updir + @"/fusionminer.exe").FileVersion;
				//Utility.Log (LogLevel.Info, "Package Version: {0}", ver);
//				if (((RemoteIsOldVersion (version, ver))&&(DateTime.UtcNow>_nestUpgrade))||(RemoteIsOldVersion(version,"0.6.5660.17740"))){
//					_nestUpgrade = DateTime.UtcNow.AddSeconds (30);
				if (RemoteIsOldVersion (version, ver)){
					maint.SoftwareVersion = ver;
					maint.Steps = new List<MaintStep> ();

					AddStep (maint.Steps, MaintStepType.SystemCommand, "/usr/bin/systemctl", " stop httpd", "");

					AddStep (maint.Steps, MaintStepType.AddFile, @"/usr/local/bin/upgrade/", "fusionminer.exe", updir + @"/fusionminer.exe");
					AddStep (maint.Steps, MaintStepType.AddFile, @"/usr/local/bin/", "System.Net.Http.Formatting.dll", updir + @"/System.Net.Http.Formatting.dll");
					AddStep (maint.Steps, MaintStepType.AddFile, @"/usr/local/bin/", "Newtonsoft.Json.dll", updir + @"/Newtonsoft.Json.dll");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "Default.aspx", updir + @"dash/Default.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "Details.aspx", updir + @"dash/Details.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "Logon.aspx", updir + @"dash/Logon.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "Restarting.aspx", updir + @"dash/Restarting.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "ShowAll.aspx", updir + @"dash/ShowAll.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/", "BatchChange.aspx", updir + @"dash/BatchChange.aspx");
					//AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/bin/", "fusiondash.dll", updir + @"dash/bin/fusiondash.dll");
					//if (RemoteIsOldVersion (version, "0.6.5292.20887")) {
					//	AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/bin/", "Thrift.dll", updir + @"dash/bin/Thrift.dll");
					//	AddStep (maint.Steps, MaintStepType.AddFile, @"/srv/http/img/nav/", "batch-active.png", updir + @"dash/img/nav/batch-active.png");
					//	AddStep (maint.Steps, MaintStepType.AddFile, @"/etc/systemd/", "journald.conf", updir + @"journald.conf");
					//}

					AddStep (maint.Steps, MaintStepType.MinerCommand, "", "reboot", "");

					maint.Checksum = Aes.EncryptString (mac);
					Utility.Log (LogLevel.Info, "Sending Update Pack "+ version + "->" + ver);
				} 
			}
			return maint;
		}
	}
}

