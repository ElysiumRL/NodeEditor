using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ElysiumGraphs;

public class Entry : Node
{
	protected override void Awake()
	{
		base.Awake();
		GraphController.Instance.entryNode = this;
	}

	public override void ProcessNode()
	{
		InputPort port = inputs.Find(port => port.portType.typeName == "Exec");
		
		Node nextNode = port.portsConnected.Any() ? port.portsConnected.First().node : null;

		if (nextNode != null && nextNode.canRunNode)
		{
			nextNode.ProcessNode();
		}
	}
}
