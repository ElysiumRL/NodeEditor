using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumGraphs;

public class Print : Node
{
	public override void ProcessNode()
	{
		List<OutputPort> ports = GetOutputPorts("String");

		foreach(var port in ports)
		{
			Debug.Log((string)port.field.TransmitToNode());
		}

		base.ProcessNode();
	}
}
