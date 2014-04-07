using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveApp.HealthMonitor
{
    class DbMonitor
    {
        public void Monitor()
        {
            while (true)
            {
                EngageMonitoring();
                Thread.Sleep(1000);
            }

        }
        public void EngageMonitoring()
        {
            System.Diagnostics.Trace.Write("Checking database");
            try
            {
                using (var connection = new SqlConnection(Config.DefaultConnection))
                {
                    connection.Open();
                    connection.Close();
                }
                System.Diagnostics.Trace.Write("Database looks good");
                UpgradeDatabase();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write("Database failure");
                DegradeDatabase();
                System.Diagnostics.Trace.Write("Database degregation complete");
            }
        }
        private void DegradeDatabase()
        {
            var webClient = new System.Net.WebClient();
            webClient.DownloadString(Config.DegradeURL);
        }
        private void UpgradeDatabase()
        {
            var webClient = new System.Net.WebClient();
            webClient.DownloadString(Config.UpgradeURL);
        }


    }
}
