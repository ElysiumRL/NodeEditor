using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumAttributes;

namespace ElysiumUtilities
{
	[ExcludeFromNodeBuild]
	public static class RuntimeAssetSave
	{
		public static string streamingAssetNodePath = Application.streamingAssetsPath + "/RuntimeNodes/";

		public static void SaveAsset(ScriptableObject _object, string filenameNoExtension)
		{
			string jsonObj = JsonUtility.ToJson(_object);
			if (!(File.Exists(streamingAssetNodePath + filenameNoExtension + ".json")))
			{
				StreamWriter writer = File.CreateText(streamingAssetNodePath + filenameNoExtension + ".json");
				writer.Write(jsonObj);
				writer.Flush();
				writer.Close();
			}
			else
			{
				using (StreamWriter writer = new StreamWriter(streamingAssetNodePath + filenameNoExtension + ".json"))
				{
					writer.Write(jsonObj);
					writer.Flush();
				}
			}
		}

		public static T LoadAsset<T>(string filenameNoExtension)
		{
			if (!(File.Exists(streamingAssetNodePath + filenameNoExtension + ".json")))
			{
				Debug.LogError("File Not Found");
			}
			using (StreamReader reader = new StreamReader(streamingAssetNodePath + filenameNoExtension + ".json"))
			{
				string assetString = reader.ReadToEnd();
				T obj = JsonUtility.FromJson<T>(assetString);
				return obj;
			}
		}
		public static List<T> LoadAllAssetsFromFolder<T>(string folderPath,string fileExtension) where T : ScriptableObject,new()
		{
			List<T> assets = new List<T>();

			foreach (string file in Directory.EnumerateFiles(Application.streamingAssetsPath + "/RuntimeNodes", "*" + fileExtension))
			{
				string assetString = File.ReadAllText(file);
				T obj = ScriptableObject.CreateInstance<T>();

				JsonUtility.FromJsonOverwrite(assetString, obj);
				assets.Add(obj);
			}

			return assets;
		}
		
	}
}
