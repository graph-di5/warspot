using System;
using System.IO;
using System.Reflection;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Security
{
    public class IntellectDomainProxy:IBeingInterface
    {
        private AppDomain _domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
        private IBeingInterface _reference;

        private IntellectDomainProxy()
        {
        }

        public IntellectDomainProxy(byte[] assembly)
        {
            _domain.SetData("Intellect", assembly);
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var s1 = AppDomain.CurrentDomain.BaseDirectory;
            var s2 = _domain.BaseDirectory;
            try
            {
                foreach (var asm in asms)
                {
                    if (asm.FullName.Contains("WarSpot.Contracts.Intellect"))
                        _domain.CreateInstanceFrom(asm.Location, typeof(BeingCharacteristics).FullName);
                }

                _reference = (IBeingInterface)_domain.CreateInstanceFromAndUnwrap(Assembly.GetExecutingAssembly().Location, typeof(IntellectSecurityProxy).FullName);
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
