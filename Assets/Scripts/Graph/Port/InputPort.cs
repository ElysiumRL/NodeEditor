using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElysiumGraphs
{
	public class InputPort : Port
	{	
		public List<Port> portsConnected = new List<Port>();
		
		public override object GetValue()
		{
			switch (portState)
			{
				case PortState.Empty when field != null:
					return field.TransmitToNode();
				case PortState.Busy when portConnectionType == PortConnectionType.Single && portLinks.Count > 0:
					//Accessing [0] instead of using First() is "better"
					return portsConnected[0].GetValue();
			}

			Debug.LogError("Port Value could not be found");
			return portValue;
		}
		
		public override bool CanConnectToPort(Port _port)
		{
			if (_port.GetType() == GetType() || _port.CurrentPortState != CurrentPortState && _port.portType != portType)
			{
				Debug.LogWarning("Can't Connect ports !");
				return false;
			}
			return true;
		}

		public override bool ConnectTo(Port _port)
		{
			if(CanConnectToPort(_port))
			{
				portsConnected.Add(_port);
				Debug.Log("Port Connected !");
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			return "Input Port " + defaultName + " / ID :" + uniqueID;
		}

		public override bool Detach(Port _port)
		{
			int portIndex = portsConnected.FindIndex(x => x == _port);
			int linkIndex = portLinks.FindIndex(x => x.output == _port);

			if (linkIndex != -1 && portIndex != -1)
			{
				portLinks[linkIndex].RemoveLink();
				//portLinks.RemoveAt(linkIndex);
				//portsConnected.RemoveAt(portIndex);
				return true;
			}
			Debug.LogWarning("Port to detach couldn't been found");
			return false;
		}

		public override void DetachAll()
		{
			for (int i = 0; i < portLinks.Count; i++)
			{
				portLinks[i].RemoveLink();
			}
			portLinks.Clear();
			portLinks.TrimExcess();

			portsConnected.Clear();

		}

		public override PortLink CreateLink()
		{
			GameObject portLink = Instantiate(Resources.Load<GameObject>("Prefabs/PortLink"),
				GraphController.Instance.linksRoot.transform);
			PortLink link = portLink.GetComponent<PortLink>();
			link.input = this;
			link.portType = portType;
			link.PortLinkStatus = PortLinkState.Dragged;

			//PortLink link = new PortLink(this, portType);
			GraphController.Instance.draggedLink = link;
			Debug.Log("Link Created");
			return link;
		}
	}
}