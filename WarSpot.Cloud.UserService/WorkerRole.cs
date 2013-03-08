using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WarSpot.Cloud.UserService
{
	public class WorkerRole : RoleEntryPoint
	{
	    private AutoResetEvent _disposeEvent = new AutoResetEvent(false);
		//private Autoscaler autoscaler;

		public override void Run()
		{
			// This is a sample worker implementation. Replace with your logic.
			Trace.WriteLine("WarSpot.Cloud.UserService entry point called", "Information");


			//this.autoscaler = EnterpriseLibraryContainer.Current.GetInstance<Autoscaler>();
			//this.autoscaler.Start();


			var address = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ServiceEndpoint"];
			var prefix = address.Protocol == "tcp" ? "net.tcp://" : "http://";
			ServiceHost host = new ServiceHost(typeof(WarSpotMainUserService),
																				 new Uri(prefix + address.IPEndpoint.ToString()));
			var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
			behaviour.InstanceContextMode = InstanceContextMode.PerSession;

            ServiceMetadataBehavior smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            smb.HttpGetEnabled = true;
            var metadataAddress = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["MetadataEndpoint"];
            smb.HttpGetUrl = new Uri("http://" + metadataAddress.IPEndpoint + "/WSDL");

			host.Open();

            while (!_disposeEvent.WaitOne(10000))
			{
				Trace.WriteLine("Working", "Information");
			}
            
			host.Close();
            Trace.WriteLine("Stopped", "Information");
		}

        public override void OnStop()
        {
            _disposeEvent.Set();
            base.OnStop();
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
