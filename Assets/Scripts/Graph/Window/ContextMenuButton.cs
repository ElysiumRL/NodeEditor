using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using ElysiumAttributes;

public delegate GameObject OnButtonClick();

namespace ElysiumGraphs
{
	[ExcludeFromNodeBuild]
	public class ContextMenuButton : MonoBehaviour
	{
		[Serializable]
		public enum ContextButtonType
		{
			Method,
	
			Field,
	
			Class,
	
			Unknown
		}
	
		public ContextButtonType type;
	
		public Image buttonImage;
	
		[SerializeField]
		private TextMeshProUGUI buttonText;
		
		public string buttonName;
	
		public string buttonClass;
		
		public string displayedName;
	
		public string displayedClass;
	
		public OnButtonClick onClick;
		
		public void UpdateButtonVisual(string searchFilter)
		{
			displayedClass = buttonClass;
			displayedName = buttonName;
			buttonText.text = displayedName + " : " + displayedClass;
		}
	
		public void HighlightString(List<string> searchSplit)
		{
			//TODO: this
		}
		
		public void DisableButton()
		{
			gameObject.SetActive(false);
		}
	
		public void EnableButton()
		{
			gameObject.SetActive(true);
		}
	
		public virtual void Click()
		{
			onClick?.Invoke();
		}
	}
}
