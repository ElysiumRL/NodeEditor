using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public class GraphContextMenuTree : MonoBehaviour
	{
		private void Start()
		{
			GenerateTreeView();
			UpdateMenu();
		}


		public string searchBar;
		
		public TreeNode<ContextTreeNode<NodeClass>> tree;

		public void GenerateTreeView()
		{
			tree = new TreeNode<ContextTreeNode<NodeClass>>(
				new ContextTreeNode<NodeClass>("Search", null, ContextTreeNode<NodeClass>.TreeNodeType.Unknown));
			
			List<NodeClass> classes = NodeRuntimeGeneration.GetAllNodeClasses();

			HashSet<string> allNamespaces = new HashSet<string>();

			List<Type> types = new List<Type>();

			foreach (NodeClass _class in classes)
			{
				Type type = Type.GetType(_class.typeName);
				types.Add(type);
				allNamespaces.Add(type?.Namespace);
			}
			foreach(string _namespace in allNamespaces)
			{
				TreeNode<ContextTreeNode<NodeClass>> namespaceTree = tree.AddChild(new ContextTreeNode<NodeClass>(_namespace, null,
					ContextTreeNode<NodeClass>.TreeNodeType.Folder));

				foreach (NodeClass _class in classes)
				{
					if (!_class.typeName.Contains(_namespace))
					{
						continue;
					}
					
					TreeNode<ContextTreeNode<NodeClass>> classTree = namespaceTree.AddChild(new ContextTreeNode<NodeClass>(
						_class.baseClass, _class,
						ContextTreeNode<NodeClass>.TreeNodeType.Class));

					TreeNode<ContextTreeNode<NodeClass>> methodsTree = classTree.AddChild(new ContextTreeNode<NodeClass>(
						"Methods", _class,
						ContextTreeNode<NodeClass>.TreeNodeType.Folder));
					
					TreeNode<ContextTreeNode<NodeClass>> fieldsTree = classTree.AddChild(new ContextTreeNode<NodeClass>(
						"Fields", _class,
						ContextTreeNode<NodeClass>.TreeNodeType.Folder));

					foreach (MethodInfos method in _class.methods)
					{
						TreeNode<ContextTreeNode<NodeClass>> methodInTree = methodsTree.AddChild(new ContextTreeNode<NodeClass>(
							method.name, _class,
							ContextTreeNode<NodeClass>.TreeNodeType.Method));
					}
					
					foreach (VariableInfos field in _class.fields)
					{
						TreeNode<ContextTreeNode<NodeClass>> fieldInTree = fieldsTree.AddChild(new ContextTreeNode<NodeClass>(
							field.name, _class,
							ContextTreeNode<NodeClass>.TreeNodeType.Field));
					}

				}
			}
		}

		public void UpdateMenu()
		{
			IEnumerator<TreeNode<ContextTreeNode<NodeClass>>> enumerator = tree.GetEnumerator();

			while (enumerator.MoveNext())
			{
				Debug.Log(enumerator.Current?.ToString());
			}
		}
	}
}
