using System.Collections.Generic;
using ElysiumAttributes;
namespace ElysiumUtilities
{
	[System.Serializable]
	[ExcludeFromNodeBuild]
	public class MethodInfos
	{
		public string name;
		public string methodClass;
		public string fullName;
		public string typeName;
		public ParameterInfos defaultReturnValue;

		public List<ParameterInfos> parameters = new List<ParameterInfos>();

		public MethodInfos(string _name, string _class)
		{
			name = _name;
			methodClass = _class;
		}

		public MethodInfos(string _name, string _class,string _fullName)
		{
			name = _name;
			methodClass = _class;
			fullName = _fullName;
		}

		public override string ToString()
		{
			return "Method : " + name;
		}
	}
}
