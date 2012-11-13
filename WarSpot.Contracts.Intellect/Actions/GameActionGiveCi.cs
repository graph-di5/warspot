using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionGiveCi : GameAction
	{
		public GameActionGiveCi(Guid senderId, Guid targetId, float ci) : base(senderId)
		{
			_transmissibleCi = ci;
			ActionType = ActionTypes.GameActionGiveCi;
			TargetId = targetId;
		}

		private float _transmissibleCi;

		public Guid TargetId { get; private set; }

		public override float Cost()
		{
			return (_transmissibleCi);//сколько передаём--столько и стоит
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}