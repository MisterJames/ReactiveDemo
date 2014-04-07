using System;
using System.Configuration;

namespace ReactiveApp.Handler
{
	public static class Config
	{
	
		public static String StorageConnectionString
		{
			get
			{
				return ConfigurationManager.AppSettings["StorageConnectionString"];
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

