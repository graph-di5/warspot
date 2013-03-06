using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarSpot.MetroClient.ViewModel
{
    public class ServiceLocator
    {
        static UnityContainer _container = new UnityContainer();

        static ServiceLocator()
        {
            _container.RegisterType(typeof(ReplayViewModel),typeof(ReplayViewModel),"ReplayVM",new ContainerControlledLifetimeManager(),
                new InjectionMember[0]);
        }

        public ServiceLocator()
        {
        }

        public ReplayViewModel ReplayViewModel
        {
            get
            {
                return _container.Resolve<ReplayViewModel>("ReplayVM");
            }
        }
    }
}
