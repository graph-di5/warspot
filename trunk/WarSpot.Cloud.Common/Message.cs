using System;
using System.Collections.Generic;

namespace WarSpot.Cloud.Common
{
	public class Message
	{
		public Guid ID;
		public List<Guid> ListOfDlls;

        public Message(Guid _ID, List<Guid> _ListOfDlls)
        {
            this.ID = _ID;
            this.ListOfDlls = _ListOfDlls;
        }

        public Message()
        {
        }

	}
}
