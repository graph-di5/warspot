using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarSpot.MetroClient.ServiceClient;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WarSpot.MetroClient.ViewModel
{
    public class ServiceLocator
    {
        static UnityContainer _container = new UnityContainer();

        static ReplayViewModel _replay = new ReplayViewModel();

        static WarSpotServiceClient _client = new WarSpotServiceClient();

        static ServiceLocator()
        {

            //_container.RegisterInstance(typeof(Frame), "MainFrame", Window.Current.Content);
        }

        public ServiceLocator()
        {
        }

        public ServiceClient.WarSpotServiceClient ServiceClient
        {
            get
            {
                return _client;
            }
        }

        public ReplayViewModel ReplayViewModel
        {
            get
            {
                return _replay;
            }
        }
    }
}
