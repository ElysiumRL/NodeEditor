using UnityEngine;

namespace ElysiumGraphs
{
	public class ElysiumDebugInternal
	{
		public void Log(string format)
		{
			Debug.Log(format);
		}
		public void LogWarning(string format)
		{
			Debug.LogWarning(format);
		}	
		public void LogError(string format)
		{
			Debug.LogError(format);
		}	
		public void LogAssert(string format)
		{
			Debug.LogAssertion(format);
		}

		public string Test()
		{
			return "test";
		}
		public string Test4()
		{
			return "test88";
		}
	}
}
