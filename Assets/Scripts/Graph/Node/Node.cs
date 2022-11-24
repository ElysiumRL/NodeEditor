using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using ElysiumUtilities;
using ElysiumAttributes;

namespace ElysiumGraphs
{
	[Serializable]
	[ExcludeFromNodeBuild]
	public class Node : MonoBehaviour
	{
		public enum NodeType
		{
			Unknown,
			Native,
			Branch,
			Entry,
			Exit,
			Pure,
			Callable
		}

		
		public int uniqueID;

		public string defaultName = "Default Node";
		
		public List<InputPort> inputs = new List<InputPort>();

		public OutputPort returnValue;
		
		public List<OutputPort> outputs = new List<OutputPort>();

		public FakeImGuiWindow window;

		public NodeClass nodeClass;
        
		public MethodInfos methodToRun;

		public NodeType nodeType;

		public MethodInfo method;
		
		public bool canRunNode = true;
		
		private GameObject inputPortRootHierarchy;

		private GameObject outputPortRootHierarchy;

		public void InitializeMethodNode(NodeClass _nodeClass, MethodInfos _method)
		{
			nodeClass = _nodeClass;
			methodToRun = _method;
			
			CreateNode();
		}
		
		
		public override string ToString()
		{
			return $"Node {defaultName} / ID :{uniqueID}";
		}

		//This is executed BEFORE BeginPlay to ensure if node is valid or not
		public virtual void PreProcessNode()
		{
			if (nodeClass == null)
			{
				Debug.LogError("Node Class not found !");
				canRunNode = false;
			}

			if (methodToRun == null)
			{
				Debug.LogError("Node Method not found !");
				canRunNode = false;
			}
			
		}

		public void ResolveMethod()
		{
			Debug.Log(methodToRun.name);
			method = Type.GetType(nodeClass.typeName)?.GetMethod(methodToRun.name);
		}

		public void ResolveFields()
		{
			
		}
		
		public void CreateNode()
		{
			ResolveMethod();
			if (nodeType != NodeType.Callable)
			{
				//Create 2 Exec ports
				GameObject execIn =	PortBuilder.CreateInput("Exec","Exec In", inputPortRootHierarchy.transform, false, ObjectCollectionType.Default);
				GameObject execOut = PortBuilder.CreateOutput("Exec","Exec Out", outputPortRootHierarchy.transform, false, ObjectCollectionType.Default);
			}

			if (method == null)
			{
				Debug.LogError("Method could not be retrieved");
				return;
			}

			if (methodToRun.defaultReturnValue.name != string.Empty)
			{
				returnValue = PortBuilder.CreateOutput(methodToRun.defaultReturnValue.type,
					                         methodToRun.name,
					                         outputPortRootHierarchy.transform, false,
					                         ObjectCollectionType.Any)
				                         .GetComponent<OutputPort>();
			}

			foreach (ParameterInfos methodParam in methodToRun.parameters)
			{
				GameObject unusedParam = null;
				switch (methodParam.parameterType)
				{
					case ParameterInfos.ParameterType.Default:
						
						unusedParam = PortBuilder.CreateOutput(methodParam.type,
							methodParam.name,
							outputPortRootHierarchy.transform, false,
							ObjectCollectionType.Any);
						break;
					case ParameterInfos.ParameterType.OutValue:
						unusedParam = PortBuilder.CreateInput(methodParam.type,
							methodParam.name,
							inputPortRootHierarchy.transform, false,
							ObjectCollectionType.Any);
						break;
					case ParameterInfos.ParameterType.ReturnValue:
						unusedParam = PortBuilder.CreateInput(methodParam.type,
							methodParam.name,
							inputPortRootHierarchy.transform, false,
							ObjectCollectionType.Any);
						break;
					default:
						Debug.LogWarning("Parameter Type not found");
						break;
				}
				Debug.Log(unusedParam.name + " Parameter Created");
			}
			UpdatePortsInNode();
		}
		
		/// <summary>
		/// Executes Node instruction
		/// </summary>
		public virtual void ProcessNode()
		{
			//Parameters for the Method
			List<object> inputParams = new List<object>();

			int i = 0;
			//Feeds the input parameters to the method
			foreach (ParameterInfos param in methodToRun.parameters)
			{
				switch (param.parameterType)
				{
					case ParameterInfos.ParameterType.Default:
						inputParams.Add(inputs.Find(_port => _port.name == param.name)?.GetValue());
						break;
					case ParameterInfos.ParameterType.ReturnValue:
						inputParams.Add(inputs.Find(_port => _port.name == param.name)?.GetValue());
						break;
					case ParameterInfos.ParameterType.OutValue:
						inputParams.Add(null);
						break;
				}
				i++;
			}
		

			if(method.IsStatic)
			{
				returnValue.SetValue(method.Invoke(null, inputParams.ToArray()));
			}
			else
			{
				if(returnValue != null)
				{
					returnValue.SetValue(method.Invoke(new ElysiumDebugInternal(), inputParams.ToArray()));
				}
				else
				{
					method.Invoke(new ElysiumDebugInternal(), inputParams.ToArray());
				}
			}

			i = 0;
			foreach (ParameterInfos param in methodToRun.parameters)
			{
				if (param.parameterType == ParameterInfos.ParameterType.OutValue)
				{
					outputs.Find(_port => _port.name == param.name)?.SetValue(inputParams[i]);
				}
				
				i++;
			}
			
			
			//foreach (OutputPort param in outputs)
			//{
			//	if (methodToRun.parameters[i].parameterType == ParameterInfos.ParameterType.OutValue)
			//	{
			//		param.SetValue(inputParams[i]);
			//	}
			//	i++;
			//}
			
			//Find Next Exec ports
			List<InputPort> ports = inputs.FindAll(port => port.portType.typeName == "Exec");

			//No ports found, we just leave
			if (!ports.Any())
			{
				return;
			}

			//Go for each possible exec port (in order) and execute next node instructions
			foreach (InputPort nextInstruction in ports)
			{
				Node nextNode = nextInstruction.portsConnected.Any() ? nextInstruction.portsConnected.First().node : null;

				if (nextNode != null && nextNode.canRunNode)
				{
					nextNode.ProcessNode();
				}
			}
		}

		public List<OutputPort> GetOutputPorts(string portType)
		{
			return outputs.FindAll(x => x.portType.typeName == portType);
		}

		public List<InputPort> GetInputPorts(string portType)
		{
			return inputs.FindAll(x => x.portType.typeName == portType);
		}

		private void OnDestroy()
		{
			GraphController.RemoveID(this);
			window.onWindowPositionChanged.RemoveListener(OnWindowMoved);
		}

		public void UpdatePortsInNode()
		{
			inputs = GetComponentsInChildren<InputPort>().ToList();
			outputs = GetComponentsInChildren<OutputPort>().ToList();
		}

		protected virtual void Awake()
		{
			uniqueID = GraphController.GenerateID(this);
			
			window = GetComponentInParent<FakeImGuiWindow>();

			inputPortRootHierarchy = transform.FindChildenObjectsWithTag("Input Port Spawner").First();
			outputPortRootHierarchy = transform.FindChildenObjectsWithTag("Output Port Spawner").First();

			UpdatePortsInNode();

			if (window == null)
			{
				Debug.LogError("Window Not Found");
			}
			if (inputPortRootHierarchy == null)
			{
				Debug.LogError("Input Port Root Hierarchy Not Found !");
			}

			if (outputPortRootHierarchy == null)
			{
				Debug.LogError("Output Port Root Hierarchy Not Found !");
			}

			window.onWindowPositionChanged.AddListener(OnWindowMoved);
		}

		public void OnWindowMoved()
		{
			foreach (InputPort inputPort in inputs)
			{
				foreach (PortLink link in inputPort.portLinks)
				{
					link.UpdateInputLinkPosition();
				}
			}
			foreach (OutputPort outputPort in outputs)
			{
				foreach(PortLink link in outputPort.portLinks)
				{
					link.UpdateOutputLinkPosition();
				}
			}
		}


	}
}