using Microsoft.WindowsAzure.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.HealthMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var monitor = new DbMonitor();
            monitor.Monitor();
        }

    }
}
