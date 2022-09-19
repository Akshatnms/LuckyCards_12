using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlertPopupManager : MonoBehaviour {
	public static AlertPopupManager Inst;

	public AlertPopup[] alertPopups;

	private AlertPopup alert;

    public RectTransform ParentRect;
	void Awake(){
		Inst = this;
       
	}

    internal void ShowUpgrade(string title, string msg, string[] options, UnityAction[] actions)
    {
        if (alert != null) return;


        alert = (AlertPopup)Instantiate(alertPopups[options.Length], ParentRect, false);

       
        alert.title.text = title;
        alert.msg.text = msg;
        alert.transform.SetAsLastSibling();
        for (int i = 0; i < options.Length; i++)
        {
            alert.buttonsLbls[i].text = options[i];
            alert.buttons[i].onClick.RemoveAllListeners();
            alert.buttons[i].onClick.AddListener(actions[i]);
            alert.buttons[i].onClick.AddListener(delegate { HideUpgradePopup(); });
        }
    }


	internal void Show(string title,string msg,string[] options,UnityAction[] actions){

        if (alert != null) return;

        Debug.Log("Alert Upgradation");
		alert = (AlertPopup)Instantiate (alertPopups[options.Length], ParentRect, false);

//        Debug.Log("UIManager.Inst.ParentRect "+UIManager.Inst.ParentRect.name);
		alert.title.text = title;
		alert.msg.text = msg;
		alert.transform.SetAsLastSibling ();
		for (int i = 0; i < options.Length; i++) {
			alert.buttonsLbls [i].text = options [i];
			alert.buttons [i].onClick.RemoveAllListeners ();
			alert.buttons [i].onClick.AddListener (actions [i]);
			alert.buttons [i].onClick.AddListener (HidePopUp);
		}
	}

	internal void CustomPopup(string title,string msg,string[] options,UnityAction[] actions,int Index){
        if (alert != null) return;
		alert = (AlertPopup)Instantiate (alertPopups[Index], ParentRect, false);
		alert.title.text = title;
		alert.msg.text = msg;
		alert.transform.SetAsLastSibling ();
		for (int i = 0; i < options.Length; i++) {
			alert.buttonsLbls [i].text = options [i];
			alert.buttons [i].onClick.RemoveAllListeners ();
			alert.buttons [i].onClick.AddListener (actions [i]);
			alert.buttons [i].onClick.AddListener (HidePopUp);
		}
	}

	internal void Show(string title,string msg){
        if (alert != null) return;
		alert = (AlertPopup)Instantiate (alertPopups[0],ParentRect, false);
		alert.title.text = title;
		alert.msg.text = msg;
		alert.transform.SetAsLastSibling ();
	}

    void HideUpgradePopup(){
       /* val = true;
        if(val){
            return;
        }
        if (alert != null)
        {
            Destroy(alert.gameObject);
            alert = null;
            //          iTween.ScaleTo (alert.gameObject,iTween.Hash ("scale",Vector3.zero,"time",0.3f,"easetype",iTween.EaseType.easeOutExpo,"oncomplete","DestroyME","oncompletetarget",this.gameObject));
        }*/
    }


    void HidePopUp()
    {
		if (alert != null) {
			Destroy (alert.gameObject);
			alert = null;
//			iTween.ScaleTo (alert.gameObject,iTween.Hash ("scale",Vector3.zero,"time",0.3f,"easetype",iTween.EaseType.easeOutExpo,"oncomplete","DestroyME","oncompletetarget",this.gameObject));
		}
    }




//	void DestoryME(){
//		Destroy (this.gameObject);
//	}

//    public void OnButtonClick(string arg)
//    {
//
//        switch(arg)
//        {
//            case "Ok":
//                break;
//            case "Cancel":
//                break;
//        }
//
//		HidePopUp ();
//    }
}
