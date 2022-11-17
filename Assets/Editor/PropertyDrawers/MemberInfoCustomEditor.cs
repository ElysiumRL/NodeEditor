using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace ElysiumEditor
{
	//[CustomEditor(typeof(ScriptableObject),true)]
	//[CanEditMultipleObjects]
	public class MemberInfoCustomEditor : Editor
	{
	
		ScriptableObject obj;
	
		System.Type type;
	
		public void OnEnable()
		{
			obj = (ScriptableObject)target;
			type = target.GetType();
		}
	
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Separator();
			foreach(var field in type.GetFields())
			{
				EditorGUILayout.BeginHorizontal();
	
				EditorGUILayout.LabelField(field.Name);

				if (field.FieldType == typeof(System.Type))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(field.DeclaringType.Name);
					EditorGUI.EndDisabledGroup();
				}
				if (field.FieldType == typeof(string))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField((string)field.GetValue(obj));
					EditorGUI.EndDisabledGroup();
				}
				if (field.FieldType == typeof(MemberInfo))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(((MemberInfo)field.GetValue(obj)).Name);
					EditorGUI.EndDisabledGroup();
				}
				if (field.FieldType == typeof(MethodInfo))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(((MethodInfo)field.GetValue(obj)).Name);
					EditorGUI.EndDisabledGroup();
				}
				if (field.FieldType == typeof(ParameterInfo))
				{
					EditorGUI.BeginDisabledGroup(true);
					string returnType = "void";

					if((ParameterInfo)field.GetValue(obj) != null)
					{
						returnType = ((ParameterInfo)field.GetValue(obj)).ParameterType.Name;
					}

					EditorGUILayout.TextField(returnType);
					EditorGUI.EndDisabledGroup();
				}
				if(field.FieldType == typeof(FieldInfo))
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.TextField(field.FieldType.Name + " / " + field.Name);
					EditorGUI.EndDisabledGroup();
				}
				if (field.FieldType == typeof(List<FieldInfo>))
				{
					EditorGUILayout.EndHorizontal();
					List<FieldInfo> fields = (List<FieldInfo>)field.GetValue(obj);

					if(fields != null)
					{
						EditorGUI.indentLevel++;
						foreach (var fieldObject in fields)
						{
							EditorGUILayout.BeginHorizontal();
							
							EditorGUI.BeginDisabledGroup(true);
							
							EditorGUILayout.TextField(fieldObject.FieldType.Name,fieldObject.Name);
							
							EditorGUI.EndDisabledGroup();
							
							EditorGUILayout.EndHorizontal();

						}
						EditorGUI.indentLevel--;

					}
					EditorGUILayout.BeginHorizontal();
				}
				if (field.FieldType == typeof(List<ParameterInfo>))
				{
					EditorGUILayout.EndHorizontal();
					List<ParameterInfo> parameters = (List<ParameterInfo>)field.GetValue(obj);

					if (parameters != null)
					{
						EditorGUI.indentLevel++;
						foreach (var paramObject in parameters)
						{
							EditorGUILayout.BeginHorizontal();

							EditorGUI.BeginDisabledGroup(true);

							EditorGUILayout.TextField(paramObject.ParameterType.Name, paramObject.Name);

							EditorGUI.EndDisabledGroup();

							EditorGUILayout.EndHorizontal();

						}
						EditorGUI.indentLevel--;

					}
					EditorGUILayout.BeginHorizontal();
				}
				EditorGUILayout.EndHorizontal();
			}
		}
	
		public void HandleFieldType(FieldInfo field)
		{
			EditorGUI.BeginDisabledGroup(true);
			
			EditorGUI.EndDisabledGroup();
		}
	
	
	}
}