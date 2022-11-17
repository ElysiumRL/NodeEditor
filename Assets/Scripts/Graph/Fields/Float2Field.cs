using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ElysiumUtilities;
using ElysiumInterfaces;
namespace ElysiumGraphs
{
	public class Float2Field : MonoBehaviour,IFieldObject
	{
		public enum FloatFieldType
		{
			X,
			Y,
			Z,
			W
		}

		public FloatFieldType fieldType = FloatFieldType.X;

		public Map<FloatFieldType, float> fieldVaues = new Map<FloatFieldType, float>();


		public void SelectFieldType(FloatFieldType _fieldType)
		{
			fieldType = _fieldType;
		}

		public void OnValueModified(string newValue)
		{
			fieldVaues[fieldType] = float.Parse(newValue);
		}

		public object TransmitToNode()
		{
			return null;
		}

		private void Start()
		{
			//GetComponent<TextMeshProUGUI>().text = fieldValue.ToString();
		}
	}
}
