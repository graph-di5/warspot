namespace WarSpot.Client.XnaClient.AIManager
{
    class Intellect
    {
        public string Name { get; set; }
        public byte[] byteDll;
        public int SizeInBits { get; set; }

        public Intellect(string path, string name)
        {
            this.byteDll = System.IO.File.ReadAllBytes(path);
            this.Name = name;
        }
    }
}
