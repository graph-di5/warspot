using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WarSpot.Contracts.Service
{
    [DataContract]
    public class ReplayDescription
    {
        [DataMember]
        public Guid id { get; set; }

        [DataMember]
        public string name { get; set; } 

        [DataMember]
        public List<string> intellects;

        /*
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> Intellects
        {
            get { return intellects; }
            set { intellects = value; }
        }

        */
 
        
    }
}
