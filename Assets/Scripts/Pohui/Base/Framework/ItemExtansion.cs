using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ActionAndActivator
{
	public static class ItemExtansion
	{
		public static void Foreach<T>(this IEnumerable<T> values, Action<T> action)
		{
			foreach(var item in values)
			{ 
				action.Invoke(item);
			}
		}
	}

	public static class GameObjectExtansion
	{
		public static float GetDistance2D(this Transform transform, Transform target)
		{
			if(transform != null && target != null) 
			{
				var vec1 = target.position - transform.position;
				var vec2 = new Vector2(vec1.x, vec1.y);
				return vec2.magnitude;
			}
			return float.MaxValue;
		}
	}
}
