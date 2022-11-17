using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ElysiumInterfaces;

namespace ElysiumGraphs
{
	public class FloatField : MonoBehaviour, IFieldObject
	{
		public float fieldValue = 0f;

		public void OnValueModified(string newValue)
		{
			fieldValue = float.Parse(newValue);
		}

		public object TransmitToNode()
		{
			return fieldValue;
		}

		private void Start()
		{
			GetComponent<TMP_InputField>().text = fieldValue.ToString();
		}
	}
}
