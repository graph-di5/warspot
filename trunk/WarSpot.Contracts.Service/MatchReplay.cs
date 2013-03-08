using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using WarSpot.MatchComputer;

namespace WarSpot.MatchComputer
{
    [DataContract]
    public class MatchReplay//Используем этот класс для хранения и сериализации истории событий.
    {
        [DataMember]
        Version AssemblyVersion { get; set; }

        [CollectionDataContractAttribute]
        public List<WarSpotEvent> Events;

        public MatchReplay()
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
