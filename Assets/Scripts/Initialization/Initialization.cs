using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElysiumUtilities;
using ElysiumAttributes;
using ElysiumGraphs;

//Initialization class : This is the place where all the "initializable" systems should register
public class Initialization
{
	[RuntimeInitializeOnLoadMethod]
	public static void InitializeSystems()
	{
		PortBuilder.Initialize();
		PortBuilder.FindAllPorts();
	}

}
