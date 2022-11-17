using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[Serializable]
	[ExcludeFromNodeBuild]
	public class ContextTreeNode<T> where T : ScriptableObject
	{
		[Serializable]
		public enum TreeNodeType
		{
			Folder,

			Method,

			Field,

			Class,

			Unknown
		}

		public string baseName;
		
		public T data;
		
		public GraphContextMenuTree treeMenu;

		public TreeNodeType nodeType;

		public UnityEvent onClick;

		public bool isFolded = true;

		public ContextTreeNode(string _name, T _data, TreeNodeType _type)
		{
			baseName = _name;
			data = _data;
			nodeType = _type;
		}

		public void ToggleFoldState()
		{
			isFolded = !isFolded;
		}

		public void OnClick()
		{

		}

		public override string ToString()
		{
			return baseName + " " + data?.ToString();
		}
	}
}