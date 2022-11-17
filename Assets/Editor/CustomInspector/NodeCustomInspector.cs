using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ElysiumGraphs;

namespace ElysiumEditor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Node), true)]
	public class NodeCustomInspector : Editor
	{
		private Node node;
	
		private PortType portTypeToAdd;
	
		private void OnEnable()
		{
			//EditorGUILayout.HelpBox("Some warning text", MessageType.Warning);
			node = (Node)target;
		}
		public override void OnInspectorGUI()
		{
			portTypeToAdd = (PortType)EditorGUILayout.ObjectField("Port Type To Add",portTypeToAdd, typeof(PortType), false);
	
			GUILayout.BeginHorizontal();
	
			if (GUILayout.Button("+ Output Port"))
			{
				//TODO: Finds in Output Port folder if port type is equal to portTypeToAdd
				//TODO: then, add it to specific content layout and update window size if possible
	
			}
	
			if (GUILayout.Button("+ Input Port"))
			{
				//TODO: Finds in Input Port folder if port type is equal to portTypeToAdd
				//TODO: then, add it to specific content layout and update window size if possible
			}
	
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Update All Ports"))
			{
	
			}
	
			//if (GUILayout.Button("Select Node"))
			//{
			//	GameObject nodeObject = window.gameObject.GetComponentInChildren<Node>().gameObject;
			//	Selection.objects = new Object[] { nodeObject };
			//}
	
				GUILayout.Space(10);
	
			base.OnInspectorGUI();
		}
	}
}