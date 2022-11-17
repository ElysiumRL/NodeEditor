using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ElysiumGraphs;
using ElysiumUtilities;


public class GraphContextMenu : MonoBehaviour
{
	private void Start()
	{
		GenerateContextView();
		UpdateMenu();
	}

	private List<ContextMenuButton> buttons = new List<ContextMenuButton>();

	public string searchBar;

	public GameObject buttonParent;

	public void GenerateContextView()
	{
		if (buttonParent == null)
		{
			Debug.LogError("Button Parent not found !");
		}


		List<NodeClass> classes = NodeRuntimeGeneration.GetAllNodeClasses();

		HashSet<string> allNamespaces = new HashSet<string>();

		//List<Type> types = new List<Type>();

		foreach (NodeClass _class in classes)
		{
			Type type = Type.GetType(_class.typeName);
			//types.Add(type);
			if (type?.Namespace != null)
			{
				allNamespaces.Add(type.Namespace);
			}
		}

		foreach (string _namespace in allNamespaces)
		{
			foreach (NodeClass _class in classes)
			{
				if (!_class.typeName.Contains(_namespace))
				{
					continue;
				}

				foreach (MethodInfos method in _class.methods)
				{
					GenerateContextMenuButton(method, _class,
						ContextMenuButton.ContextButtonType.Method);
				}

				foreach (VariableInfos field in _class.fields)
				{
					GenerateContextMenuButton(field, _class,
						ContextMenuButton.ContextButtonType.Field);
				}
			}
		}
	}

	public void GenerateContextMenuButton(MethodInfos _nodeData, NodeClass _class, ContextMenuButton.ContextButtonType type)
	{
		GameObject button = Instantiate(Resources.Load<GameObject>("UI/ContextMenu/ContextMenuButton"),
			buttonParent.transform);
		ContextMenuButton buttonComponent = button.GetComponent<ContextMenuButton>();
		buttonComponent.name = _nodeData.name;
		buttonComponent.buttonClass = _class.baseClass;
		buttonComponent.displayedClass = _class.baseClass;
		
		buttonComponent.buttonName = _nodeData.name;
		buttonComponent.displayedName = _nodeData.name;

		buttonComponent.type = type;
		buttonComponent.onClick = () => NodeBuilder.CreateNode(_class, _nodeData);
		buttons.Add(buttonComponent);
		buttonComponent.UpdateButtonVisual(searchBar);
	}
	public void GenerateContextMenuButton(VariableInfos _nodeData, NodeClass _class, ContextMenuButton.ContextButtonType type)
	{
		GameObject button = Instantiate(Resources.Load<GameObject>("UI/ContextMenu/ContextMenuButton"),
			buttonParent.transform);
		ContextMenuButton buttonComponent = button.GetComponent<ContextMenuButton>();
		buttonComponent.name = _nodeData.name;
		buttonComponent.buttonClass = _class.baseClass;
		buttonComponent.displayedClass = _class.baseClass;

		buttonComponent.buttonName = _nodeData.name;
		buttonComponent.displayedName = _nodeData.name;
		buttonComponent.type = type;
		//buttonComponent.onClick = () => NodeBuilder.CreateNode(_class, _name);
		buttons.Add(buttonComponent);
		buttonComponent.UpdateButtonVisual(searchBar);
	}
	public void UpdateMenu()
	{
		if (searchBar == string.Empty)
		{
			ShowAllButtons();
			return;
		}

		foreach (ContextMenuButton button in buttons)
		{
			if (ContainsStringMatch(button.buttonName.ToLower().Split().ToList()))
			{
				button.EnableButton();
				button.HighlightString(searchBar.ToLower().Split().ToList());
			}
			else
			{
				HideButton(button);
			}
		}
	}

	public void ShowAllButtons()
	{
		foreach (ContextMenuButton button in buttons)
		{
			button.EnableButton();
		}
	}

	public void HideButton(ContextMenuButton _button)
	{
		buttons.Find(button => button == _button)?.DisableButton();
	}

	
	public bool ContainsStringMatch(List<string> searchSplit)
	{
		//This is probably the worst unoptimized way to do it but it somehow works

		foreach (char letter in searchBar.ToLower())
		{
			int index = searchSplit.FindIndex(_char => _char == letter.ToString());
			if (index != -1)
			{
				searchSplit.RemoveAt(index);
			}
		}
		return searchSplit.Count == 0;
	}
}