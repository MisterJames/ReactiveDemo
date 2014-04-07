using System;
using System.Configuration;

namespace ReactiveApp.HealthMonitor
{
	public static class Config
	{
	
		public static String DegradeURL
		{
			get
			{
				return ConfigurationManager.AppSettings["DegradeURL"];
			}
		}
	
		public static String UpgradeURL
		{
			get
			{
				return ConfigurationManager.AppSettings["UpgradeURL"];
			}
		}
		public static String DefaultConnection
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			}
		}
	}
}

