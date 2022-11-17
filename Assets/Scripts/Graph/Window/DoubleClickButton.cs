using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using ElysiumInterfaces;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public enum ButtonState
	{
		Disabled,

		Idle,

		Hovered,

		SingleClick, //Single Click

		DoubleClick  //Double Click
	}

	[RequireComponent(typeof(Image))]
	[ExcludeFromNodeBuild]
	public class DoubleClickButton : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,ISelectable
	{
		private ButtonState currentState = ButtonState.Idle;

		[HideInInspector]
		public Image image;

		private bool ignoreDeselection;

		public ButtonState CurrentState
		{
			get
			{
				return currentState;
			}
			set
			{
				if (currentState == ButtonState.Disabled)
				{
					return;
				}
				currentState = value;
				events[currentState]?.Invoke();
				image.color = buttonState[currentState];
			}
			
		}

		public Map<ButtonState,UnityEvent> events = new Map<ButtonState, UnityEvent>();

		public Map<ButtonState, Color> buttonState = new Map<ButtonState, Color>();

		private void Awake()
		{
			image = GetComponent<Image>();
		}


		public void OnPointerClick(PointerEventData eventData)
		{
			if (CurrentState == ButtonState.Disabled)
			{
				return;
			}

			switch (eventData.clickCount)
			{
				case 1:
					CurrentState = ButtonState.SingleClick;
					break;
				case 2:
					CurrentState = ButtonState.DoubleClick;
					break;
				default:
					CurrentState = ButtonState.SingleClick;
					break;
			}
			Select();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (CurrentState != ButtonState.SingleClick && CurrentState != ButtonState.DoubleClick)
			{
				CurrentState = ButtonState.Hovered;
			}

		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (CurrentState != ButtonState.SingleClick && CurrentState != ButtonState.DoubleClick)
			{
				CurrentState = ButtonState.Idle;
			}
		}

		public void DisableButton()
		{
			currentState = ButtonState.Disabled;
		}

		public void EnableButton()
		{
			currentState = ButtonState.Idle;
		}

		public void Select()
		{
			if (CurrentState is ButtonState.SingleClick or ButtonState.DoubleClick)
			{
				if (GraphController.Instance.selectedObjects.FindIndex(x => x as DoubleClickButton == this) == -1)
				{
					GraphController.Instance.selectedObjects.Add(this);
					ignoreDeselection = true;
				}
			}
			else
			{
				if (GraphController.Instance.selectedObjects.FindIndex(x => x as DoubleClickButton == this) != -1)
				{
					GraphController.Instance.selectedObjects.Remove(this);
				}
			}
		}

		public void Deselect()
		{
			if (ignoreDeselection)
			{
				ignoreDeselection = false;
				return;
			}

			if (GraphController.Instance != null)
			{
				int windowIndex = GraphController.Instance.selectedObjects.FindIndex(x => x as DoubleClickButton == this);
				if (windowIndex != -1)
				{
					GraphController.Instance.selectedObjects.Remove(this);
				}
				CurrentState = ButtonState.Idle;
			}
		}
	}
}
