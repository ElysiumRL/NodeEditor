using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using ElysiumInterfaces;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public enum PortState
	{
		Empty,

		Busy,

		Hovered
	}
	
	[ExcludeFromNodeBuild]
	public enum PortConnectionType
	{
		Single,

		Multi
	}

	[Serializable]
	[ExcludeFromNodeBuild]
	public abstract class Port : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public int uniqueID;

		public string defaultName = "Default Port";

		public List<PortLink> portLinks;

		public PortType portType;

		public PortConnectionType portConnectionType = PortConnectionType.Multi;

		[SerializeField]
		public Map<PortState, Sprite> portIcons = new Map<PortState, Sprite>();

		[SerializeField]
		protected Image portImage;

		[SerializeField]
		protected PortState portState;

		public RectTransform rectTransform;
		

		public TextMeshProUGUI text;

		public Node node;

		public IFieldObject field;

		protected object portValue;

		public abstract object GetValue();

		public void SetValue(object value)
		{
			portValue = value;
		}
		
		public Vector3 WorldPosition
		{
			get
			{
				return rectTransform.position;
			}
			set
			{
				rectTransform.position = value;
			}
		}

		public abstract bool CanConnectToPort(Port _port);

		public abstract bool ConnectTo(Port _port);

		public abstract bool Detach(Port _port);

		public abstract void DetachAll();

		public abstract PortLink CreateLink();
		
		public PortState CurrentPortState
		{
			get
			{
				return portState;
			}
			set
			{
				portState = value;
				UpdatePortState();
			}
		}

		public void UpdatePortState()
		{
			if (portImage != null)
			{
				portImage.sprite = portIcons[portState];
				portImage.color = new Color(portType.color.R,portType.color.G,portType.color.B,portType.color.A);
			}
			text.text = defaultName;
		}

		private void Update()
		{
			if (Mouse.current.leftButton.wasPressedThisFrame && GraphController.Instance.hoveredPort == this)
			{
				if (GraphController.Instance.draggedLink == null)
				{
					CreateLink();
				}
			}
			text.text = defaultName;

		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			GraphController.Instance.hoveredPort = this;
			Debug.Log("Port Hovered");
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			GraphController.Instance.hoveredPort = null;
			Debug.Log("Port Left");
		}

		private void OnDestroy()
		{
			GraphController.RemoveID(this);
		}

		private void Awake()
		{
			uniqueID = GraphController.GenerateID(this);
			rectTransform = GetComponent<RectTransform>();
			text = GetComponentInChildren<TextMeshProUGUI>();
			node = GetComponentInParent<Node>();
			field = GetField();

			if (rectTransform == null)
			{
				Debug.LogError("Rect Transform Not Found !");
			}

			if (text == null)
			{
				Debug.LogError("Port Text Not Found !");
			}

			if (node == null)
			{
				Debug.LogError("Parent Node Not Found !");
			}

			text.text = defaultName;
		}

		public IFieldObject GetField()
		{
			return transform.parent.gameObject.GetComponentInChildren<IFieldObject>();
		}
	}
}