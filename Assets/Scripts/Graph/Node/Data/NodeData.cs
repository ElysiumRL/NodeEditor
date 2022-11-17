using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	//Node Data used to generate all editor/user public methods inside all classes.
	[Serializable]
	[ExcludeFromNodeBuild]
	[CreateAssetMenu(fileName = "NewNodeData", menuName = "Graph View/Node Data")]
	public class NodeData : ScriptableObject
	{
		public System.Type baseClass;
		public string methodName;
		public Delegate method;
		public List<ParameterInfo> methodParameters;
		public List<ParameterInfo> methodOutReturnValues;
		public ParameterInfo methodReturnValue;

		private void Awake()
		{
			methodParameters = new List<ParameterInfo>();
			methodOutReturnValues = new List<ParameterInfo>();
		}
	}
}
