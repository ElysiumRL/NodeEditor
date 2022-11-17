using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensions
{
	public static List<GameObject> FindChildenObjectsWithTag(this Transform parent, string tag)
	{
		List<GameObject> taggedGameObjects = new List<GameObject>();
 
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (child.tag == tag)
			{
				taggedGameObjects.Add(child.gameObject);
			}
			if (child.childCount > 0)
			{
				taggedGameObjects.AddRange(FindChildenObjectsWithTag(child, tag));
			}
		}
		return taggedGameObjects;
	}

}