using System;
using System.IO;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Security
{
    public class IntellectDomainProxy:IBeingInterface
    {
        private AppDomain _domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence,
                                                           new AppDomainSetup()
                                                               {
                                                                   ApplicationBase =
                                                                       AppDomain.CurrentDomain.BaseDirectory,
                                                                   PrivateBinPath =
                                                                       AppDomain.CurrentDomain.BaseDirectory,
                                                               });
        private IBeingInterface _reference;

        private IntellectDomainProxy()
        {
        }

        public IntellectDomainProxy(byte[] assembly)
        {
            _domain.SetData("Intellect", assembly);

            try
            {
                _reference = (IBeingInterface)_domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(IntellectSecurityProxy).FullName);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            return _reference.Construct(step, ci);
        }

        public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            return _reference.Think(step, characteristics, area);
        }

        public IntellectDomainProxy Copy()
        {
            return new IntellectDomainProxy
                       {
                           _domain = _domain,
                           _reference =
                               (IBeingInterface)
                               _domain.CreateInstanceAndUnwrap(typeof (IntellectSecurityProxy).Assembly.FullName,
                                                               typeof (IntellectSecurityProxy).Name)
                       };
        }

        public void Unload()
        {
            _reference = null;
            AppDomain.Unload(_domain);
        }
    }
}
