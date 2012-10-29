using System;
namespace WarSpot.Client.XnaClient.AIManager
{
    class Intellect
	{
		public string Name { get; set; }
		public string Path { get; set; }
        public byte[] byteDll;
        public int SizeInBits { get; set; }

        public Intellect(string path)
        {
            this.byteDll = System.IO.File.ReadAllBytes(path);
            this.Path = path;
			Name = getName(byteDll);
        }

		private string getName(byte[] byteDll)
		{
			//Not implemented  yet.
			return "Intellect" + Path.GetHashCode() % 1000;
		}


    }
}
