using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlertPopup : MonoBehaviour {

	public List<Button> buttons;
	public List<Text> buttonsLbls;
	public Text title;
	public Text msg;

	[SerializeField]RectTransform popupRect;

	void Awake(){
		popupRect.localScale = Vector3.zero;
	}

	void Start(){
		iTween.ScaleTo (popupRect.gameObject,iTween.Hash ("scale",Vector3.one,"time",0.7f,"easetype",iTween.EaseType.easeOutExpo));

		if (buttons.Count < 1) {
			Destroy (this.gameObject,3);
		}
	}
}
