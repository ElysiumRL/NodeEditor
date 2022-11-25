using System;

namespace ElysiumAttributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class MethodPure : Attribute {}
	
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class MethodCallable : Attribute { }
	
}