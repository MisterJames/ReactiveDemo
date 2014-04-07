using System;
using System.Configuration;

namespace ReactiveApp.Web
{
	public static class Config
	{
	
		public static String webpagesVersion
		{
			get
			{
				return ConfigurationManager.AppSettings["webpages:Version"];
			}
		}
	
		public static String webpagesEnabled
		{
			get
			{
				return ConfigurationManager.AppSettings["webpages:Enabled"];
			}
		}
	
		public static String ClientValidationEnabled
		{
			get
			{
				return ConfigurationManager.AppSettings["ClientValidationEnabled"];
			}
		}
	
		public static String UnobtrusiveJavaScriptEnabled
		{
			get
			{
				return ConfigurationManager.AppSettings["UnobtrusiveJavaScriptEnabled"];
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

