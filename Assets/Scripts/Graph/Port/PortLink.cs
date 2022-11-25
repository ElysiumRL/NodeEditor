using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using ElysiumInterfaces;
using UnityEngine.InputSystem;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public enum PortLinkState
	{
		Dragged,
		Linked
	}
	[ExcludeFromNodeBuild]
	public class PortLink : MonoBehaviour,
		IPointerEnterHandler,
		IPointerExitHandler,
		ISelectable
	{
		public int uniqueID;

		public InputPort input;

		public OutputPort output;
		
		public PortType portType;
		
		private bool isDestroyed;

		[SerializeField]
		private PortLinkState linkStatus = PortLinkState.Linked;

		[SerializeField]
		private ObjectState linkState = ObjectState.Idle;

		private SpriteShapeRenderer spriteShapeRenderer;

		private SpriteShapeController spriteShapeController;

		private const int inputControllerID = 0;

		private const int outputControllerID = 1;

		public ObjectState CurrentLinkState
		{
			get
			{
				Debug.Log(linkState);
				return linkState;
			}
			set
			{
				linkState = value;
			}
		}

		public PortLinkState PortLinkStatus
		{
			get 
			{
				return linkStatus;
			}
			set
			{
				linkStatus = value;
				Debug.Log(linkStatus);
				UpdateLinkState();
			}
		}

		private void Awake()
		{
			uniqueID = GraphController.GenerateID(this);

			spriteShapeController = GetComponent<SpriteShapeController>();
			spriteShapeRenderer = GetComponent<SpriteShapeRenderer>();

			if (spriteShapeController == null)
			{
				Debug.LogError("SpriteShapeController not found !");
			}
			if (spriteShapeRenderer == null)
			{
				Debug.LogError("SpriteShapeRenderer not found !");
			}

			GenerateLinkVisual();
		}



		public void UpdateLinkState()
		{

		}

		private void CheckForLinks()
		{
			if (PortLinkStatus == PortLinkState.Linked)
			{
				if (GraphController.Instance.hoveredPort != null)
				{
					if (input == null)
					{
						if(output.CanConnectToPort(GraphController.Instance.hoveredPort))
						{
							output.ConnectTo(GraphController.Instance.hoveredPort);
							input = (InputPort)GraphController.Instance.hoveredPort;

							//We found a single port trying to connect to a multi port, removing it
							if(output.portConnectionType == PortConnectionType.Single && output.portLinks.Count != 0)
							{
								//Removing each links for output to avoid getting double outputs
								while (output.portLinks.Count != 0)
								{
									output.portLinks[0].RemoveLink();
								}
							}
							if (input.portConnectionType == PortConnectionType.Single && input.portLinks.Count != 0)
							{
								while (input.portLinks.Count != 0)
								{
									input.portLinks[0].RemoveLink();
								}
							}

							input.ConnectTo(output);
							input.portLinks.Add(this);
							output.portLinks.Add(this);
							DrawLink();
						}
						else
						{
							Debug.LogWarning("Input port doesn't match with output port !");
							RemoveLink();
						}
					}
					//Input here is trying to connect to output
					if (output == null)
					{
						if (input.CanConnectToPort(GraphController.Instance.hoveredPort))
						{
							input.ConnectTo(GraphController.Instance.hoveredPort);
							output = (OutputPort)GraphController.Instance.hoveredPort;
							
							//Okay, this one i am not so sure if i'll ever use it but whatever
							if (input.portConnectionType == PortConnectionType.Single && input.portLinks.Count != 0)
							{
								while (input.portLinks.Count != 0)
								{
									input.portLinks[0].RemoveLink();
								}
							}

							output.ConnectTo(input);
							input.portLinks.Add(this);
							output.portLinks.Add(this);
						}
						else
						{
							Debug.LogWarning("Output port doesn't match with Input port !");
							RemoveLink();
						}
					}
				}
				else
				{
					//Show Node Tab
					RemoveLink();
				}
				GraphController.Instance.draggedLink = null;
			}
		}

		public void RemoveLink()
		{
			if(!isDestroyed)
			{
				if(input != null)
				{
					input.portLinks.Remove(input.portLinks.Find(x => x == this));
					input.portsConnected.Remove(input.portsConnected.Find(x => x == output));
				}
				if(output != null)
				{
					output.portLinks.Remove(output.portLinks.Find(x => x == this));
					output.inputPortConnected = null;
				}
				Debug.Log("Link Removed");
				isDestroyed = true;
				Destroy(gameObject);
			}
		}

		public void UpdateInputLinkPosition()
		{
			Vector3 position;
			if(input == null)
			{
				position = GraphController.Instance.mouseWorldPos;
			}
			else
			{
				position = input.WorldPosition;
			}

			spriteShapeController.spline.SetPosition(inputControllerID, position);
		}
		public void UpdateOutputLinkPosition()
		{
			Vector3 position;
			if (output == null)
			{
				position = GraphController.Instance.mouseWorldPos;
			}
			else
			{
				position = output.WorldPosition;
			}
			spriteShapeController.spline.SetPosition(outputControllerID, position);
		}

		public void DrawLink()
		{
			Color portTypeColor = new Color(
				portType.color.R / 255f,
				portType.color.G / 255f,
				portType.color.B / 255f,
				portType.color.A / 255f);
			
			if(PortLinkStatus == PortLinkState.Dragged)
			{
				portTypeColor.a /= 2f;
			}

			spriteShapeRenderer.color = portTypeColor;

			UpdateInputLinkPosition();
			UpdateOutputLinkPosition();
			UpdateSplineTangents();
		}

		public void UpdateSplineTangents()
		{
			Vector3 distance; //input.WorldPosition - output.WorldPosition;

			if(input == null)
			{
				distance = GraphController.Instance.mouseWorldPos - output.WorldPosition;
			}
			else if(output == null)
			{
				distance = input.WorldPosition - GraphController.Instance.mouseWorldPos;
			}
			else
			{
				distance = input.WorldPosition - output.WorldPosition;
			}


			Vector3 leftTangent = new Vector3(distance.x / 2f, 0, 0);
			Vector3 rightTangent = new Vector3(-distance.x / 2f, 0, 0);

			spriteShapeController.spline.SetRightTangent(inputControllerID, rightTangent);
			spriteShapeController.spline.SetLeftTangent(inputControllerID, leftTangent);

			spriteShapeController.spline.SetRightTangent(outputControllerID, rightTangent);
			spriteShapeController.spline.SetLeftTangent(outputControllerID, leftTangent);

		}

		public void GenerateLinkVisual()
		{
			spriteShapeController.spline.Clear();

			spriteShapeController.spline.InsertPointAt(inputControllerID, new Vector3(-1, 0, 0));
			spriteShapeController.spline.InsertPointAt(outputControllerID, new Vector3(1, 0, 0));

			spriteShapeController.spline.SetTangentMode(inputControllerID, ShapeTangentMode.Continuous);
			spriteShapeController.spline.SetTangentMode(outputControllerID, ShapeTangentMode.Continuous);
		}

		private void Update()
		{
			if (PortLinkStatus == PortLinkState.Dragged)
			{
				if (!(GraphController.Instance.draggedLink == this && Mouse.current.leftButton.isPressed))
				{
					PortLinkStatus = PortLinkState.Linked;            
					CheckForLinks();
				}
				DrawLink();
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if(CurrentLinkState != ObjectState.Selected)
			{
				CurrentLinkState = ObjectState.Hovered;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (CurrentLinkState != ObjectState.Selected)
			{
				CurrentLinkState = ObjectState.Idle;
			}
		}

		public void Select()
		{
			CurrentLinkState = CurrentLinkState ==
			ObjectState.Selected ? ObjectState.Hovered : ObjectState.Selected;

			if (CurrentLinkState == ObjectState.Selected)
			{
				GraphController.Instance.selectedObjects.Add(this);
			}
			else
			{
				GraphController.Instance.selectedObjects.Remove(this);
			}
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
				CurrentLinkState = ObjectState.Idle;
			}
		}

		private void OnDestroy()
		{
			GraphController.RemoveID(this);
			RemoveLink();

		}

	}
}