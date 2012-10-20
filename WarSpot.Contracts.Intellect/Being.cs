namespace WarSpot.Contracts.Intellect
{
	public class Being : IBeingInterface
	{
		public BeingCharacteristics Characteristics { private set; get; }

		public Being(BeingCharacteristics characteristics)
		{
			Characteristics = characteristics;
		}

		// todo return desired action
		public Action Think()
		{
			// todo do something
			return new Action(); 
		}
	}
}
