using ElysiumAttributes;

namespace ElysiumUtilities
{
	[System.Serializable]
	[ExcludeFromNodeBuild]
	public class ParameterInfos
	{
		public enum ParameterType
		{
			Default,
			ReturnValue,
			OutValue
		}
		
		public string name;
		public string type;
		public int order;
		public ParameterType parameterType;
		
		public ParameterInfos(string _name,string _type,int _order,ParameterType _parameterType)
		{
			name = _name;
			type = _type;
			order = _order;
			parameterType = _parameterType;
		}
	}
}