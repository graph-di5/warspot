using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using WarSpot.Cloud.Tournament;

namespace WarSpot.Cloud.UserService
{
	public class WorkerRole : RoleEntryPoint
	{
		private AutoResetEvent _disposeEvent = new AutoResetEvent(false);
		//private Autoscaler autoscaler;
	    private TournamentManager _tournamentManager = TournamentManager.GetInstance();
		public override void Run()
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			// This is a sample worker implementation. Replace with your logic.
			Trace.WriteLine("WarSpot.Cloud.UserService entry point called", "Information");
			ServiceHost host = null;
			try
			{
				var address = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["ServiceEndpoint"];
				var prefix = address.Protocol == "tcp" ? "net.tcp://" : "http://";
				host = new ServiceHost(typeof(WarSpotMainUserService),
								new Uri(prefix + address.IPEndpoint.ToString()));
				var address2 = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["MetadataEndpoint"];
				prefix = address2.Protocol == "tcp" ? "net.tcp://" : "http://";
				var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
				behaviour.InstanceContextMode = InstanceContextMode.PerSession;
				host.Description.Endpoints[1].Address = new EndpointAddress(new Uri(prefix + address2.IPEndpoint));
				host.Open();

                _tournamentManager.Start();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Exception occured: {0}", ex.Message);
				if (ex.InnerException != null)
					Trace.TraceError("Exception details: {0}", ex.InnerException.Message);
			}

			//this.autoscaler = EnterpriseLibraryContainer.Current.GetInstance<Autoscaler>();
			//this.autoscaler.Start();

			while (!_disposeEvent.WaitOne(10000))
			{
				Trace.WriteLine("Working", "Information");
			}
            _tournamentManager.Stop();
			if (host != null)
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

			TimeSpan tsMin = TimeSpan.FromMinutes(1);

			DiagnosticMonitorConfiguration dmc = DiagnosticMonitor.GetDefaultInitialConfiguration();

			// Transfer logs to storage every minute

			dmc.Logs.ScheduledTransferPeriod = tsMin;

			// Transfer verbose, critical, etc. logs

			dmc.Logs.ScheduledTransferLogLevelFilter = LogLevel.Undefined;
			DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", dmc);

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			CloudStorageAccount.SetConfigurationSettingPublisher(
																	(a, b) => b(RoleEnvironment.GetConfigurationSettingValue(a)));

			return base.OnStart();
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.TraceError("Unhandled Exception: {0}", e.ExceptionObject);
			Trace.Flush();
		}
	}
}
