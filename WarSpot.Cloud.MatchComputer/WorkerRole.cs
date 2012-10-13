using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.MatchComputer
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // Это образец реализации рабочего процесса. Замените его собственной логикой.
            Trace.WriteLine("WarSpot.Cloud.MatchComputer entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Задайте максимальное число одновременных подключений 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Дополнительные сведения по управлению изменениями конфигурации
            // см. раздел MSDN по ссылке http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
