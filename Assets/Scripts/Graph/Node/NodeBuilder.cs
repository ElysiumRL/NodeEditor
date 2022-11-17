using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public class NodeBuilder
	{
		public static GameObject CreateNode(NodeClass _class, MethodInfos _methodName)
		{
			GameObject node = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/NodeTemplate"),
				GraphController.Instance.graphRoot.transform);
			
			if (node == null)
			{
				Debug.LogError("NodeTemplate not found in Resources folder");
				return null;
			}

			node.name = _class.baseClass + "/" + _methodName.name;
			Node nodeComponent = node.GetComponentInChildren<Node>();

			if (nodeComponent == null)
			{
				Debug.LogError("Node Component not found");
				return null;
			}
			
			nodeComponent.InitializeMethodNode(_class,_methodName);
			
			
			
			
			return null;
		}

	}
}