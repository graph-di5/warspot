using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WarSpot.Cloud.UserService
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WarSpot.Cloud.UserService entry point called", "Information");

            var address = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ServiceEndpoint"];
            var prefix = address.Protocol == "tcp" ? "net.tcp://" : "http://";
            ServiceHost host = new ServiceHost(typeof (WarSpotMainUserService),
                                               new Uri(prefix + address.IPEndpoint.ToString()));

            host.Open();

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }

            host.Close();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            CloudStorageAccount.SetConfigurationSettingPublisher(
                                        (a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));

            return base.OnStart();
        }
    }
}
