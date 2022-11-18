using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using ElysiumGraphs;
using ElysiumUtilities;

namespace ElysiumEditor
{
	public class GenerateNodeData : EditorWindow
	{
		public static List<string> namespaceWhitelist = new List<string>() { "ElysiumGraphs", "ElysiumUtilities" };
		public static List<string> namespaceBlacklist = new List<string>() { "UnityEditor", "UnityEngine" };
	
		//[MenuItem("Graph View/Generate Node Data")]
		private static void GenerateData()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> unityTypes = new List<Type>();
	
			foreach(var assembly in assemblies)
			{
				unityTypes.AddRange(assembly.GetTypes().Where(_type => _type.IsSubclassOf(typeof(UnityEngine.Object))));
			}
	
			foreach(var type in unityTypes)
			{
				Debug.Log(type.FullName);
			}
	
		}
	
		private static List<Type> GetAllTypesWithWhitelist()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> unityTypes = new List<Type>();
	
			//Get All types in specific namespace
			foreach (var assembly in assemblies)
			{
				unityTypes.AddRange
				(assembly.GetTypes().Where
				(
					_type => namespaceWhitelist.Exists(_x => _x == _type.Namespace)
					&& _type.Namespace != "Null"
				//||  _type.IsSubclassOf(typeof(UnityEngine.Object)))
				//&&	!namespaceBlacklist.Contains(_type.Namespace.Split('.')[0])
				)
				);
			}
	
			//Remove weird "type+<>c__DisplayClass" thing
			unityTypes.RemoveAll(_x => _x.DeclaringType != null);
	
			return unityTypes;
		}
	
		[MenuItem("Graph View/Generate Node Data (No Editor Types)")]
		//TODO: Rework asset path
		private static void GenerateDataNoEditor()
		{
			if(AssetDatabase.IsValidFolder("Assets/Resources/NodeData"))
			{
				AssetDatabase.DeleteAsset("Assets/Resources/NodeData");
			}
			AssetDatabase.CreateFolder("Assets/Resources", "NodeData");
			AssetDatabase.CreateFolder("Assets/Resources/NodeData", "Methods");
	
			List<Type> unityTypes = GetAllTypesWithWhitelist();
			
			List<NodeClassData> classes = new List<NodeClassData>();
			foreach (var type in unityTypes)
			{
				NodeClassData classToAdd = CreateInstance<NodeClassData>();
				classToAdd.name = type.Name;
				classToAdd.baseClass = type;
				classToAdd.fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).ToList();
				MethodInfo[] methods = type.GetMethods();
				AssetDatabase.CreateFolder("Assets/Resources/NodeData/Methods", classToAdd.name);
	
				foreach (var method in methods)
				{
					if(!unityTypes.Exists(_x => _x == method.GetBaseDefinition().DeclaringType))
					{
						continue;
					}
					NodeData dataToAdd = CreateInstance<NodeData>();
					dataToAdd.baseClass = type;
					dataToAdd.methodName = method.Name;
					dataToAdd.name = method.Name;
					ParameterInfo[] parameters = method.GetParameters();
					if (method.ReturnParameter != null)
					{
						dataToAdd.methodReturnValue = method.ReturnParameter;
					}
					foreach (var methodParam in parameters)
					{
						if(methodParam.IsOut)
						{
							dataToAdd.methodOutReturnValues.Add(methodParam);
						}
						else
						{
							dataToAdd.methodParameters.Add(methodParam);
						}
					}
					classToAdd.methods.Add(dataToAdd);
					AssetDatabase.CreateAsset(dataToAdd, "Assets/Resources/NodeData/Methods/" + classToAdd.name + "/" + dataToAdd.name + ".asset");
				}
				AssetDatabase.CreateAsset(classToAdd, "Assets/Resources/NodeData/" + classToAdd.name + ".asset");
			}
			EditorUtility.FocusProjectWindow();
			AssetDatabase.SaveAssets();
			
	
		}
	
		[MenuItem("Graph View/Generate Serialized Node Data (No Editor Types)")]
		private static void GenerateDataNoEditorSerialized()
		{
			NodeRuntimeGeneration.GenerateNodesRuntime(out _);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	
			Debug.Log("Successfully generated Nodes !");
	
			//#if UNITY_EDITOR
			//		if (AssetDatabase.IsValidFolder("Assets/Resources/NodeDataSerialized"))
			//		{
			//			AssetDatabase.DeleteAsset("Assets/Resources/NodeDataSerialized");
			//		}
			//		AssetDatabase.CreateFolder("Assets/Resources", "NodeDataSerialized");
			//#endif
			//		List<Type> unityTypes = GetAllTypesWithWhitelist();
			//		foreach (var type in unityTypes)
			//		{
			//			NodeClass classBase = CreateInstance<NodeClass>();
			//			classBase.name = type.Name;
			//			classBase.baseClass = type.Name;
			//
			//			List<FieldInfo> fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).ToList();
			//
			//			foreach(var field in fields)
			//			{
			//				classBase.fields.Add(new VariableInfos(field.Name, field.FieldType.Name, field.FieldType.AssemblyQualifiedName));
			//			}
			//			MethodInfo[] methods = type.GetMethods();
			//
			//			foreach (var method in methods)
			//			{
			//				if (!unityTypes.Exists(_x => _x == method.GetBaseDefinition().DeclaringType))
			//				{
			//					continue;
			//				}
			//				MethodInfos newMethod = new MethodInfos(method.Name, type.Name, type.AssemblyQualifiedName);
			//
			//				ParameterInfo[] parameters = method.GetParameters();
			//				if (method.ReturnParameter != null)
			//				{
			//					newMethod.defaultReturnValue = new ParameterInfos(
			//						method.ReturnParameter.Name,
			//						method.ReturnParameter.ParameterType.Name,
			//						-1,
			//						ParameterInfos.ParameterType.ReturnValue);
			//				}
			//				int i = 0;
			//				foreach (var methodParam in parameters)
			//				{
			//					if (methodParam.IsOut)
			//					{
			//						newMethod.parameters.Add(new ParameterInfos
			//						(methodParam.Name, methodParam.ParameterType.Name, i,
			//							ParameterInfos.ParameterType.OutValue));
			//					}
			//					else
			//					{
			//						newMethod.parameters.Add(new ParameterInfos
			//						(methodParam.Name, methodParam.ParameterType.Name, i,
			//							ParameterInfos.ParameterType.Default));						
			//					}
			//					i++;
			//				}
			//				classBase.methods.Add(newMethod);
			//			}
			//#if UNITY_EDITOR
			//			AssetDatabase.CreateAsset(classBase, "Assets/Resources/NodeDataSerialized/" + classBase.name + ".asset");
			//#endif
			//		}
			//#if UNITY_EDITOR
			//		EditorUtility.FocusProjectWindow();
			//		AssetDatabase.SaveAssets();
			//#endif
		}
	
		[MenuItem("Graph View/Generate Port Types (No Editor Types)")]
		private static void GeneratePortTypes()
		{
			PortBuilder.Initialize();
			PortBuilder.FindAllPorts();
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			Debug.Log("Successfully generated Port Types !");
	
		}
	}
}
