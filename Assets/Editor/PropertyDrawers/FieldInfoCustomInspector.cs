using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using ElysiumUtilities;
using System;

namespace ElysiumEditor
{

	/// <summary>
	/// NOT WORKING YET PLS DONT USE IT AAAAAAA
	/// @TODO: finish this
	/// </summary>
	public class FieldInfoCustomInspector : Editor
	{
		public delegate void InspectorFieldMethod(FieldInfo fieldType, object target);

		private static Map<FieldInfo, InspectorFieldMethod> fieldToDisplay = new Map<FieldInfo, InspectorFieldMethod>();

		public void Awake()
		{
			fieldToDisplay.Clear();
			fieldToDisplay.TrimExcess();

			foreach(var method in typeof(FieldInfoCustomInspector).GetMethods(BindingFlags.NonPublic))
			{
				//fieldToDisplay.Add(System.Type.GetType(method.Name), (InspectorFieldMethod)Delegate.CreateDelegate(typeof(InspectorFieldMethod), method));
			}

		}

		private static void DefaultInspectorField(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField(fieldType.DeclaringType.Name);
			EditorGUI.EndDisabledGroup();
		}

		//Generates field property for non list proprties
		public static void GenerateFieldProperty(FieldInfo fieldType,object target)
		{
			InspectorFieldMethod fieldMethod;
			//EditorGUILayout.BeginHorizontal();

			//EditorGUILayout.LabelField(fieldType.Name);

			if (fieldToDisplay.TryGetValue(fieldType, out fieldMethod))
			{
				fieldMethod(fieldType, target);
			}
			else
			{
				DefaultInspectorField(fieldType, target);
			}
			//EditorGUILayout.EndHorizontal();

		}
		public static void GenerateListFieldProperty(FieldInfo fieldType, object target)
		{
			InspectorFieldMethod fieldMethod;
			//EditorGUILayout.BeginHorizontal();

			//EditorGUILayout.LabelField(fieldType.Name);

			if (fieldToDisplay.TryGetValue(fieldType, out fieldMethod))
			{
				fieldMethod(fieldType, target);
			}
			else
			{
				DefaultInspectorField(fieldType, target);
			}
			//EditorGUILayout.EndHorizontal();

		}
		private void String(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField((string)fieldType.GetValue(target));
			EditorGUI.EndDisabledGroup();
		}
		
		private void Int(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField(fieldType.DeclaringType.Name);
			EditorGUI.EndDisabledGroup();
		}

		private void MemberInfo(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField(fieldType.DeclaringType.Name);
			EditorGUI.EndDisabledGroup();
		}

		private void ParameterInfo(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField(fieldType.DeclaringType.Name);
			EditorGUI.EndDisabledGroup();

		}

		private void Type(FieldInfo fieldType, object target)
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.TextField(fieldType.DeclaringType.Name);
			EditorGUI.EndDisabledGroup();
		}

	}

}
