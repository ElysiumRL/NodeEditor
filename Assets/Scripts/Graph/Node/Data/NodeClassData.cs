using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[Serializable]
	[CreateAssetMenu(fileName = "NewNodeClass", menuName = "Graph View/Node Class")]
	[ExcludeFromNodeBuild]
	public class NodeClassData : ScriptableObject
	{
		public System.Type baseClass;
		public List<FieldInfo> fields;
		public List<NodeData> methods;

		private void Awake()
		{
			fields = new List<FieldInfo>();
			methods = new List<NodeData>();
		}

	}
}