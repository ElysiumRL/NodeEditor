using System;

namespace ElysiumAttributes
{
	[AttributeUsage(AttributeTargets.Class
	                | AttributeTargets.Interface
	                | AttributeTargets.Enum
	                | AttributeTargets.Struct
	                | AttributeTargets.Method
	                | AttributeTargets.Field
	                | AttributeTargets.Property)]
	public class ExcludeFromNodeBuild : Attribute
	{
	}
}