using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

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
			ServiceHost host = new ServiceHost(typeof(WarSpotMainUserService),
																				 new Uri(prefix + address.IPEndpoint.ToString()));
            var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;
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
