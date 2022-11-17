using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ElysiumInterfaces;
namespace ElysiumGraphs
{
	public class IntField : MonoBehaviour, IFieldObject
	{
		public int fieldValue = 0;

		public void OnValueModified(string newValue)
		{
			fieldValue = int.Parse(newValue);
		}

		public object TransmitToNode()
		{
			return fieldValue;
		}

		private void Start()
		{
			GetComponent<TextMeshProUGUI>().text = fieldValue.ToString();
		}





	}
}
