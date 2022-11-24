using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumAttributes;
using System;
// ReSharper disable RedundantOverriddenMember


namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public enum ObjectCollectionType
	{
		Default,
		Array,
		List,
		Any,
		None
	}

	[CreateAssetMenu(fileName = "NewPortType", menuName = "Graph View/Port Type")]
	[ExcludeFromNodeBuild]
	public class PortType : ScriptableObject
	{
	
		public ObjectCollectionType portCollectionType = ObjectCollectionType.Default;
	
		public string typeName = "Default Type";
	
		//note: Using UnityEngine.Color creates self reference loop when serializing with newtonsoft json
		
		public System.Drawing.Color color = System.Drawing.Color.Azure;

		public override string ToString()
		{
			return "Port Type : " + typeName + " / Collection Type : " + portCollectionType;
		}
	
		public static bool operator == (PortType a, PortType b)
		{
			return 
				a.portCollectionType == b.portCollectionType 
				&& 
				a.typeName == b.typeName;
		}
	
		public static bool operator != (PortType a, PortType b)
		{
			return
				!(a.portCollectionType == b.portCollectionType
				&&
				a.typeName == b.typeName);
		}
		
		public override bool Equals(object other)
		{
			return base.Equals(other);
		}
        	
        public override int GetHashCode()
        {
        	return base.GetHashCode();
        }
		
	}
}