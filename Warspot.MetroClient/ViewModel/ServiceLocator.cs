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

        static Replay _rep;
        static ReplayDescription _repD;

        static WarSpotServiceClient _client = new WarSpotServiceClient();

        static string _uname;

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

        public Replay Rep
        {
            get { return _rep; }
            set { _rep = value; }
        }

        public ReplayDescription RepDesc
        {
            get { return _repD; }
            set { _repD = value; }
        }

        public string Username
        {
            get { return _uname; }
            set { _uname = value; }
        }
    }
}
