using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ElysiumInterfaces;
namespace ElysiumGraphs
{
	public class StringField : MonoBehaviour, IFieldObject
	{
		public string fieldValue = "";

		public void OnValueModified(string newValue)
		{
			fieldValue = newValue;
		}

		public object TransmitToNode()
		{
			return fieldValue;
		}

		private void Start()
		{
			GetComponent<TMP_InputField>().text = fieldValue;
		}
	}
}
