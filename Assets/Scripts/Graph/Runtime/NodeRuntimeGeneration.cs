using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public static class NodeRuntimeGeneration
	{
		public static void GenerateNodesRuntime(out List<NodeClass> classesGenerated)
		{
			List<Type> unityTypes = ReflectionUtility.GetAllTypesWithWhitelist(ReflectionUtility.namespaceWhitelist);
			List<NodeClass> classes = new List<NodeClass>();
			foreach (Type type in unityTypes)
			{
				if (type.GetCustomAttribute(typeof(ExcludeFromNodeBuild)) != null || type.GetInterfaces().ToList()
				                                                                         .Exists(match =>
					                                                                         match.GetCustomAttribute(
						                                                                         typeof(
							                                                                         ExcludeFromNodeBuild)) !=
					                                                                         null))
				{
					continue;
				}

				NodeClass classBase = ScriptableObject.CreateInstance<NodeClass>();
				classBase.name = type.Name;
				classBase.baseClass = type.Name;
				classBase.typeName = type.AssemblyQualifiedName;

				List<FieldInfo> fields = type
				                         .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
				                                    BindingFlags.Instance).ToList();

				foreach (FieldInfo field in fields)
				{
					if (field.GetCustomAttribute(typeof(ExcludeFromNodeBuild)) != null)
					{
						classBase.fields.Add(new VariableInfos(field.Name, field.FieldType.Name,
							field.FieldType.AssemblyQualifiedName));
					}
				}

				MethodInfo[] methods = type.GetMethods();

				foreach (MethodInfo method in methods)
				{
					if(method.GetCustomAttribute(typeof(ExcludeFromNodeBuild)) != null ||
						!unityTypes.Exists(_x => _x == method.GetBaseDefinition().DeclaringType))
					{
						continue;
					}

					MethodInfos newMethod = new MethodInfos(method.Name, type.Name, type.AssemblyQualifiedName);

					ParameterInfo[] parameters = method.GetParameters();
					if (method.ReturnParameter != null)
					{
						newMethod.defaultReturnValue = new ParameterInfos(
							method.ReturnParameter.Name,
							method.ReturnParameter.ParameterType.Name,
							-1,
							ParameterInfos.ParameterType.ReturnValue);
					}

					int i = 0;
					foreach (ParameterInfo methodParam in parameters)
					{
						if (methodParam.IsOut)
						{
							newMethod.parameters.Add(new ParameterInfos
							(methodParam.Name, methodParam.ParameterType.Name, i,
								ParameterInfos.ParameterType.OutValue));
						}
						else
						{
							newMethod.parameters.Add(new ParameterInfos
							(methodParam.Name, methodParam.ParameterType.Name, i,
								ParameterInfos.ParameterType.Default));
						}
						i++;
					}

					classBase.methods.Add(newMethod);
				}

				classes.Add(classBase);
			}

			foreach (NodeClass nodeClass in classes)
			{
				RuntimeAssetSave.SaveAsset(nodeClass, nodeClass.baseClass);
			}

			classesGenerated = classes;
		}

		
		
		public static List<NodeClass> GetAllNodeClasses()
		{
			return RuntimeAssetSave.LoadAllAssetsFromFolder<NodeClass>(RuntimeAssetSave.streamingAssetNodePath,
				".json");
		}
		
	}
}