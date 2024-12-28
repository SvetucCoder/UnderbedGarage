using UnityEngine;

namespace Assets.Scripts.Entities.Func.Utilits
{
	public interface IInterpolateAlgs<T>
	{
		T Interp(T current, T end, float t);
	}

	public class VectorInterpolateLinear : IInterpolateAlgs<Vector3>
	{
		public Vector3 Interp(Vector3 current, Vector3 end, float t)
		{
			var dir = end - current;

			return dir.magnitude < t ? end : current + dir.normalized * t;
		}
	}

	public class VectorInterpolateLerp : IInterpolateAlgs<Vector3>
	{
		public Vector3 Interp(Vector3 current, Vector3 end, float t) 
		{
			var dir = end - current;

			if(dir.magnitude < t)
			{
				return end;
			}

			return Vector3.LerpUnclamped(current, end, t);
		}
	}

	public class VectorInterpolateSLerp : IInterpolateAlgs<Vector3>
	{
		public Vector3 Interp(Vector3 current, Vector3 end, float t)
		{
			var dir = end - current;

			if(dir.magnitude < t)
			{
				return end;
			}

			return Vector3.SlerpUnclamped(current, end, t);
		}
	}


	public static class VectorInterpolate
	{
		public static IInterpolateAlgs<Vector3> CreateAlgs(MethodsAction methodsAction)
		{
			switch(methodsAction)
			{
				case MethodsAction.Linear:
					return new VectorInterpolateLinear();
				case MethodsAction.Lerp:
					return new VectorInterpolateLerp();
				case MethodsAction.Slerp:
					return new VectorInterpolateSLerp();
				default: return null;
			}
		}
	}
}
