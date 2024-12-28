using System;

namespace Assets.Scripts.Entities.Func.Utilits
{
	[Serializable]
	public enum SwitcherTriggerContext
	{
		Toggle,
		None,
		On,
		Off,
		Block,
		Unblock,
		Wait,
		Stay,
	}

	public enum EqualsMode
	{
		More,
		Less,
		Equels
	}
}
