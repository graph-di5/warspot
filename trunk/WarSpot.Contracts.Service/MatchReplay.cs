using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WarSpot.Contracts.Service
{
    [DataContract]
    public class MatchReplay//Используем этот класс для хранения и сериализации истории событий.
    {
        [DataMember]
        public Version AssemblyVersion { get; set; }

        [DataMember]
        public List<WarSpotEvent> Events;

        public MatchReplay()
        {
            AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}
