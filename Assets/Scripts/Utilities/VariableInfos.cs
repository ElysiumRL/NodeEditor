using ElysiumAttributes;

namespace ElysiumUtilities
{
	[System.Serializable]
	[ExcludeFromNodeBuild]
	public class VariableInfos
	{
		public string name;
		public string type;
		public string fullName;
		public int order;
		
		public VariableInfos(string _name,string _type)
		{
			name = _name;
			type = _type;
		}

		public VariableInfos(string _name, string _type, string _fullName)
		{
			name = _name;
			type = _type;
			fullName = _fullName;
		}

		public override string ToString()
		{
			return "Field : " + name;
		}
	}
}
