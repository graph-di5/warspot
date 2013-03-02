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
            this.ListOfDlls = new List<Guid>();
        }

        public override string ToString()
        {
            string result = this.ID.ToString() + ' ';

            foreach (Guid intellect in this.ListOfDlls)
            {
                result = result + intellect.ToString() + ' ';
            }

            return result;
        }

	}
}
