using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public class NodeContextMenu : MonoBehaviour
	{
		private GraphContextMenu contextMenu;
	
		public bool shouldDisable;
	
		private void Start()
		{
			GraphController.Instance.nodePanel = gameObject;
		}
	
		private void OnEnable()
		{
			contextMenu.searchBar = string.Empty;
		}
	
		private void OnDisable()
		{
	
		}
	
	}
}
