using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using ElysiumInterfaces;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[Serializable]
	[ExcludeFromNodeBuild]
	public enum ObjectState
	{
		Idle,
		Hovered,
		Selected
	}

	[SelectionBase]
	[ExcludeFromNodeBuild]
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	[RequireComponent(typeof(GraphicRaycaster))]
	public class FakeImGuiWindow : MonoBehaviour,
		IPointerClickHandler,
		IDragHandler,
		IEndDragHandler,
		IPointerEnterHandler,
		IPointerExitHandler,
		ISelectable
	{

		public string windowName = "Default Window";
		
		public int uniqueID = 0;
		
		[SerializeField]
		private ObjectState windowState;
		
		public ObjectState CurrentWindowState
		{
			get
			{
				return windowState;
			}
			set
			{
				windowState = value;
				UpdateWindowStateColor();
			}
		}

		public Map<ObjectState, Color> colorPalette = new Map<ObjectState, Color>();

		[SerializeField]
		private Image background;

		[SerializeField]
		private TextMeshProUGUI windowText;

		private new RectTransform transform;

		public bool ShowBackground = true;

		public UnityEvent onWindowPositionChanged = new UnityEvent();

		public int windowLayerOrder = 0;

		public void PushToFront()
		{

		}

		public void SetWindowPosition(Vector3 position)
		{
			transform.position = position;
			onWindowPositionChanged?.Invoke();
		}

		public void SetWindowScale(Vector2 scale)
		{
			transform.sizeDelta = scale;
		}

		private void Start()
		{
			transform = GetComponent<RectTransform>();
			CurrentWindowState = ObjectState.Idle;
			GraphController.Instance.windowOrder.Push(this);
			windowText.text = windowName;
		}

		[ContextMenu("Update Window")]
		public void UpdateEditorWindow()
		{
			if (windowText != null)
			{
				windowText.text = windowName;
			}

			UpdateWindowStateColor();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Select();
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (GraphController.Instance.hoveredPort == null && GraphController.Instance.draggedLink == null)
			{
				Vector2 mousePos = Mouse.current.position.ReadValue();
				Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
					new Vector3(mousePos.x, mousePos.y, transform.position.z));
				mouseWorldPos.z = 0f;

				transform.position = mouseWorldPos;

				onWindowPositionChanged?.Invoke();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//Debug.Log("Window Hovered");

			if (CurrentWindowState != ObjectState.Selected)
			{
				CurrentWindowState = ObjectState.Hovered;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			//Debug.Log("Window Not Hovered");

			if (CurrentWindowState != ObjectState.Selected)
			{
				CurrentWindowState = ObjectState.Idle;
			}
		}

		public void Select()
		{
			CurrentWindowState = CurrentWindowState ==
				ObjectState.Selected ? ObjectState.Hovered : ObjectState.Selected;

			if (CurrentWindowState == ObjectState.Selected)
			{
				GraphController.Instance.selectedObjects.Add(this);
			}
			else
			{
				GraphController.Instance.selectedObjects.Remove(this);
			}
		}

		public void OnDestroy()
		{
			Deselect();
			GraphController.RemoveID(this);
		}

		private void Awake()
		{
			uniqueID = GraphController.GenerateID(this);
		}

		public void Deselect()
		{
			if (GraphController.Instance != null)
			{
				int windowIndex = GraphController.Instance.selectedObjects.FindIndex(x => (UnityEngine.Object)x == this);
				if (windowIndex != -1)
				{
					GraphController.Instance.selectedObjects.RemoveAt(windowIndex);
				}
				CurrentWindowState = ObjectState.Idle;
			}
		}

		public void UpdateWindowStateColor()
		{
			if(background != null)
			{
				background.color = colorPalette[windowState];
				background.gameObject.SetActive(ShowBackground);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if(CurrentWindowState != ObjectState.Selected)
			{
				Select();
			}
		}
	}
}
