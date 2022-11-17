using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ElysiumUtilities;
using ElysiumInterfaces;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public class GraphController : MonoBehaviour
	{
		public List<ISelectable> selectedObjects = null;

		public PortLink draggedLink = null;

		public Port hoveredPort = null;

		private Map<MonoBehaviour, int> idTable = new Map<MonoBehaviour, int>();

		public Vector3 mouseWorldPosLastFrame = new Vector3(0, 0, 0);
		public Vector3 mouseWorldPos = new Vector3(0, 0, 0);

		public Stack<FakeImGuiWindow> windowOrder = new Stack<FakeImGuiWindow>();

		public GameObject linksRoot;

		public GameObject graphRoot;

		public GameObject nodePanel;

		public GameObject windowsRoot;

		public Camera mainCamera;
		
		public Entry entryNode;

		private static readonly object _lock = new object();

		private static bool isApplicationQuitting;

		private static GraphController instance;

		public bool isInPause;

		public static GraphController Instance
		{
			get
			{
				if (isApplicationQuitting)
				{
					return null;
				}
				lock(_lock)
				{
					if (instance == null)
					{
						instance = FindObjectOfType<GraphController>();
						if (instance == null)
						{
							GameObject manager = new GameObject("Graph Controller");
							instance = manager.AddComponent<GraphController>();
							Debug.Log("Graph Controller Instance Created");
						}
					}
					return instance;
				}
			}
		}

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}

			if (instance.selectedObjects == null)
			{
				instance.selectedObjects = new List<ISelectable>();
			}
			mainCamera = Camera.main;
		}

		private void Update()
		{
			if (Mouse.current.leftButton.wasPressedThisFrame)
			{
				while (selectedObjects.Count != 0)
				{
					selectedObjects[0].Deselect();
				}
			}

			if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
			{
				nodePanel.SetActive(true);
			}
			
			Vector3 mousePos = Mouse.current.position.ReadValue();

			mouseWorldPosLastFrame = mouseWorldPos;

			mouseWorldPos = mainCamera.ScreenToWorldPoint(
				new Vector3(mousePos.x, mousePos.y, transform.position.z));
			mouseWorldPos.z = 0f;

			// Process Graph Tick

			if(entryNode == null)
			{
				Debug.LogError("Entry Node Not found in graph !");
			}
			else
			{
				if (!isInPause)
				{
					entryNode.ProcessNode();
				}
			}

		}

		private void OnDestroy()
		{
			isApplicationQuitting = true;
		}

		public static int GenerateID(MonoBehaviour _object)
		{
			System.Random randomGenerator = new System.Random();
			int id;
			do 
			{
				id = randomGenerator.Next(int.MinValue, int.MaxValue);
			} while (Instance.idTable.dictionary.FindIndex(x => x.Value == id) != -1);

			Instance.idTable.Add(_object, id);
			return id;
		}
		public static void ReplaceID(MonoBehaviour _object,int newID)
		{
			if (Instance.idTable.TryGetValue(_object, out _))
			{
				Instance.idTable.dictionary.Find(_x => _x.Key == _object).Value = newID;
			}
		}

		public static bool RemoveID(MonoBehaviour _object)
		{
			return Instance != null && Instance.idTable.Remove(_object);
		}

		public static void ShowNodePanel()
		{

		}

		public static void HideNodePanel()
		{

		}

	}
}