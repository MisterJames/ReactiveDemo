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
            System.Diagnostics.Trace.TraceInformation("Checking database");
            try
            {
                using (var connection = new SqlConnection(Config.DefaultConnection))
                {
                    connection.Open();
                    connection.Close();
                }
                System.Diagnostics.Trace.TraceInformation("Database looks good");
                UpgradeDatabase();
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.TraceInformation("Database failure");
                DegradeDatabase();
                System.Diagnostics.Trace.TraceInformation("Database degregation complete");
            }
        }
        private void DegradeDatabase()
        {
            try
            {
                var webClient = new System.Net.WebClient();
                webClient.DownloadString(Config.DegradeURL);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Unable to degrade " + ex);
            }
        }
        private void UpgradeDatabase()
        {
            try
            {
                var webClient = new System.Net.WebClient();
                webClient.DownloadString(Config.UpgradeURL);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Unable to upgrade " + ex);   
            }
            
        }


    }
}
