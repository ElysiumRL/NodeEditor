using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ElysiumGraphs;

[CanEditMultipleObjects]
[CustomEditor(typeof(Port), true)]
public class PortCustomInspector : Editor
{
	private Port port;

	private PortType portTypeToAdd;

	private void OnEnable()
	{
		//EditorGUILayout.HelpBox("Some warning text", MessageType.Warning);
		port = (Port)target;
	}
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Update Port"))
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
