using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using ElysiumGraphs;
using UnityEngine;

namespace ElysiumEditor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(FakeImGuiWindow))]
	public class FakeImGuiWindowInspector : Editor
	{
		private FakeImGuiWindow window;
	
		private void OnEnable()
		{
			//EditorGUILayout.HelpBox("Some warning text", MessageType.Warning);
			window = (FakeImGuiWindow)target;
		}
		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal();
	
			if(GUILayout.Button("Update Window"))
			{
				window.UpdateEditorWindow();
			}
	
			GUILayout.EndHorizontal();
	
			if (GUILayout.Button("Select Node"))
			{
				GameObject nodeObject = window.gameObject.GetComponentInChildren<Node>().gameObject;
				Selection.objects = new Object[] { nodeObject };
			}
	
			GUILayout.Space(10);
	
			base.OnInspectorGUI();
		}
	}
}