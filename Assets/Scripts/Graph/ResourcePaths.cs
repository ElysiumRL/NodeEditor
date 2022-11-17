using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Paths of All resources (Node data, Ports etc...)
/// </summary>
public static class ResourcePaths
{
	public static readonly string NodeDataPath = Application.streamingAssetsPath + "/NodeData";

	public static readonly string NodeDataMethodsPath = Application.streamingAssetsPath + "/NodeData/Methods";

	public static readonly string NodeDataSerializedPath = Application.streamingAssetsPath + "/NodeDataSerialized";

	public static readonly string NodeDataMethodsSerializedPath =
		Application.streamingAssetsPath + "/NodeDataSerialized/Methods";
}
