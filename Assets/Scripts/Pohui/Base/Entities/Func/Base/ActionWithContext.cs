using Assets.Scripts.Entities.Func.Utilits;
using Assets.Scripts.Entities.Visualizer;

using System;

namespace Assets.Scripts.Entities
{
	[Serializable]
	public class ActionWithContext
	{
		[ReadOnlyInspector] public string name;
		public SwitcherTriggerContext Context;
		public ActionBase3D Action;
	}
}
