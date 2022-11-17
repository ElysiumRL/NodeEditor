using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;
using ElysiumGraphs;
public class Initialization
{
	[RuntimeInitializeOnLoadMethod]
	public static void InitializeSystems()
	{
		PortBuilder.FindAllPorts();
	}

}
