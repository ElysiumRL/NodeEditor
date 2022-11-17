using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[Serializable]
	[CreateAssetMenu(fileName = "NewNodeClass", menuName = "Graph View/Serializable Node Class")]
	[ExcludeFromNodeBuild]
	public class NodeClass : ScriptableObject
	{
		public string baseClass;
		public string typeName;

		public List<VariableInfos> fields = new List<VariableInfos>();

		public List<MethodInfos> methods = new List<MethodInfos>();

		public override string ToString()
		{
			string str = baseClass + " ";

			foreach (VariableInfos field in fields)
			{
				str += field.ToString();
			}
			foreach (MethodInfos method in methods)
			{
				str += method.ToString();
			}
			return str;
		}
	}
}
