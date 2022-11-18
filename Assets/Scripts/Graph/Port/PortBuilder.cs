using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumGraphs;
using ElysiumInterfaces;
using ElysiumAttributes;
using System;

[ExcludeFromNodeBuild]
public static class PortBuilder
{
	private static MultiMap<ObjectCollectionType, Map<string, PortType>> portTypeList = new();

	private static MultiMap<ObjectCollectionType, Map<string, IFieldObject>> fieldList = new();

	public static void Initialize()
	{
		//Possible mem leak here (or somewhat close to it)

		//Map regeneration
		portTypeList = null;
		fieldList = null;
		
		portTypeList = new();
		fieldList = new();

		foreach (ObjectCollectionType collectionType in (ObjectCollectionType[])Enum.GetValues(typeof(ObjectCollectionType)))
		{
			portTypeList.Add(collectionType, new());
			fieldList.Add(collectionType, new());
		}
	}


	public static void LoadAllPorts()
	{
		GetAllNativePorts();
		GetAllCustomPorts();
	}



	//Finds and generates port types for all custom classes and also for all collection types
	public static void FindAllPorts()
	{
		//portTypeList.Clear();
		//portTypeList.TrimExcess();
		NodeRuntimeGeneration.GenerateNodesRuntime(out List<NodeClass> allCustomClasses);

		GetAllNativePorts();
		GetAllCustomPorts();
		JSONTools.ExportJsonFromClassFile(Application.streamingAssetsPath + "/PortTypes/PortTypesList.json",portTypeList);
	}

	private static void GetAllNativePorts()
	{
		//C# Usual types

		GetNativePort("Bool", false, System.Drawing.Color.Tomato);

		GetNativePort("Int", false, System.Drawing.Color.DeepSkyBlue);
		GetNativePort("Int16", false, System.Drawing.Color.DodgerBlue);
		GetNativePort("Int32", false, System.Drawing.Color.CornflowerBlue);
		GetNativePort("Int64", false, System.Drawing.Color.MidnightBlue);

		GetNativePort("Uint", false, System.Drawing.Color.RoyalBlue);
		GetNativePort("Uint16", false, System.Drawing.Color.SteelBlue);
		GetNativePort("Uint32", false, System.Drawing.Color.LightSteelBlue);
		GetNativePort("Uint64", false, System.Drawing.Color.LightSlateGray);
		
		GetNativePort("Float", false, System.Drawing.Color.GreenYellow);
		GetNativePort("Double", false, System.Drawing.Color.ForestGreen);

		GetNativePort("Char", false, System.Drawing.Color.SandyBrown);
		GetNativePort("String", false, System.Drawing.Color.Violet);

		GetNativePort("Byte", false, System.Drawing.Color.DarkTurquoise);

		//Exec port : used to execute and move to other functions
		//Similar to the "Exec" pin in Unreal Engine
		GetNativePort("Exec", true, System.Drawing.Color.White);

	}

	private static void GetNativePort(string portType, bool defaultCollectionTypeOnly,System.Drawing.Color color)
	{
		if (!defaultCollectionTypeOnly)
		{
			foreach (ObjectCollectionType type in System.Enum.GetValues(typeof(ObjectCollectionType)).Cast<ObjectCollectionType>())
			{
				CreatePort(portType, type, color);
				//Map<string, PortType> nodePortType = new Map<string, PortType>();
				//PortType newPortType = ScriptableObject.CreateInstance<PortType>();
				//newPortType.color = System.Drawing.Color.PowderBlue;
				//newPortType.typeName = portType;
				//newPortType.portCollectionType = type;
				//newPortType.name = portType;
				//nodePortType.Add(portType, newPortType);
				//portTypeList.Add(type, nodePortType);
			}
		}
		else
		{
			CreatePort(portType, ObjectCollectionType.Default, color);
			//Map<string, PortType> nodePortType = new Map<string, PortType>();
			//PortType newPortType = ScriptableObject.CreateInstance<PortType>();
			//newPortType.color = System.Drawing.Color.PowderBlue;
			//newPortType.typeName = portType;
			//newPortType.portCollectionType = ObjectCollectionType.Default;
			//newPortType.name = portType;
			//nodePortType.Add(portType, newPortType);
			//portTypeList.Add(ObjectCollectionType.Default, nodePortType);
		}
	}

	private static void GetAllCustomPorts()
	{
		NodeRuntimeGeneration.GenerateNodesRuntime(out List<NodeClass> allCustomClasses);

		foreach (ObjectCollectionType type in System.Enum.GetValues(typeof(ObjectCollectionType)).Cast<ObjectCollectionType>())
		{
			foreach (NodeClass nodeClass in allCustomClasses)
			{
				CreatePort(nodeClass.name, type, System.Drawing.Color.Indigo);
				//Map<string, PortType> nodePortType = new Map<string, PortType>();
				//PortType newPortType = ScriptableObject.CreateInstance<PortType>();
				//newPortType.color = System.Drawing.Color.Tomato;
				//newPortType.typeName = nodeClass.name;
				//newPortType.portCollectionType = type;
				//newPortType.name = nodeClass.name;
				//nodePortType.Add(nodeClass.baseClass, newPortType);
				//portTypeList.Add(type, nodePortType);
			}
		}
	}

	private static void CreatePort(string portType, ObjectCollectionType collectionType, System.Drawing.Color color)
	{
		//Should be a tuple but whatever
		//TODO(?) replace
		Map<string, PortType> nodePortType = new Map<string, PortType>();
		PortType newPortType = ScriptableObject.CreateInstance<PortType>();
		newPortType.color = color;
		newPortType.typeName = portType;
		newPortType.portCollectionType = collectionType;
		newPortType.name = portType;
		nodePortType.Add(portType, newPortType);
		portTypeList.AddInExistingKey(collectionType, nodePortType);

	}


	public static PortType FindType(System.Type type,ObjectCollectionType collectionType)
	{
		return portTypeList[collectionType].Find(x => x[type.Name] != null)[type.Name];
	}
	
	public static PortType FindType(string type,ObjectCollectionType collectionType)
	{
		Debug.Log("1Port Found : " + portTypeList[collectionType].Find((x) => x.dictionary.Find(y => y.Value.typeName == type) != null)[type]);

		//Debug.Log("Port Found : " + portTypeList[collectionType].Find(map => map[type]));

		return portTypeList[collectionType].Find((x) => x.dictionary.Find(y => y.Value.typeName == type) != null)[type];
	}
	
	public static GameObject CreateInput(System.Type type, string name, Transform parent,bool addPortField,ObjectCollectionType collectionType)
	{
		GameObject port = GameObject.Instantiate(Resources.Load<GameObject>("Port/Prefabs/InputPort"), parent);
		InputPort portComponent = port.GetComponent<InputPort>();
		portComponent.portType = FindType(type, collectionType);
		portComponent.defaultName = name;
		portComponent.name = name;

		if (addPortField)
		{
			GameObject field = CreateField(type, "Test", port.transform, true);
		}

		portComponent.UpdatePortState();

		return port;
	}
	public static GameObject CreateInput(string type, string name, Transform parent,bool addPortField,ObjectCollectionType collectionType)
	{
		GameObject port = GameObject.Instantiate(Resources.Load<GameObject>("Port/Prefabs/InputPort"), parent);
		InputPort portComponent = port.GetComponent<InputPort>();
		portComponent.portType = FindType(type, collectionType);
		portComponent.defaultName = name;
		portComponent.name = name;

		if (addPortField)
		{
			//GameObject field = CreateField(type, "Test", port.transform, true);
		}
		portComponent.UpdatePortState();

		return port;
	}
	
	
	public static GameObject CreateOutput(System.Type type, string name, Transform parent,bool addPortField,ObjectCollectionType collectionType)
	{
		GameObject port = GameObject.Instantiate(Resources.Load<GameObject>("Port/Prefabs/OutputPort"), parent);
		OutputPort portComponent = port.GetComponent<OutputPort>();
		portComponent.portType = FindType(type,collectionType);
		portComponent.defaultName = name;
		portComponent.name = name;

		if (addPortField)
		{
			//GameObject field = CreateField(type, "Test", port.transform, true);
		}
		
		return port;
	}
	public static GameObject CreateOutput(string type, string name, Transform parent,bool addPortField,ObjectCollectionType collectionType)
	{
		GameObject port = GameObject.Instantiate(Resources.Load<GameObject>("Port/Prefabs/OutputPort"), parent);
		OutputPort portComponent = port.GetComponent<OutputPort>();
		portComponent.portType = FindType(type,collectionType);
		portComponent.defaultName = name;
		portComponent.name = name;

		if (addPortField)
		{
			//GameObject field = CreateField(type, "Test", port.transform, true);
		}
		
		return port;
	}
	public static GameObject CreateField(System.Type type, string name, Transform parent,bool addPortField)
	{
		//GameObject port = Object.Instantiate(Resources.Load<GameObject>("Port/Prefabs/OutputPort"), parent);
		//OutputPort portComponent = port.GetComponent<OutputPort>();
		//portComponent.portType = type;
		//portComponent.defaultName = name;
		//portComponent.name = name;
		//return port;
		return null;
	}
	
	
	
	public static IFieldObject AddFieldToType(System.Type type,ObjectCollectionType collectionType)
	{
		return null;
		//return fieldList[collectionType].Find((x) => x.dictionary.Find(y => y.Value == type) != null)[type];
	}
	
	
}
