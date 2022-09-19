using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Dialog:MonoBehaviour
{
    public GameObject dialog;
    public Action OnDialogShow = delegate { };
    public Action OnDialogHide = delegate { };
    public Button hideDialog;
   
    public Dialog(Transform dialogContainer)
    {
        dialog = dialogContainer.FindRecursive(DialogName).gameObject;
        Transform hideDialogButtonObj = dialogContainer.FindRecursive("DialogHidder");
        hideDialog = hideDialogButtonObj.GetComponent<Button>();
    }

    protected abstract string DialogName { get; }

    public bool IsVisible { get; set; }

    protected virtual void Show(bool showOK)
    {
        IsVisible = true;
        hideDialog.gameObject.SetActive(true);
        dialog.SetActive(true);
        if (showOK) hideDialog.onClick.AddListener(Hide);
        OnDialogShow?.Invoke();
        OnDialogShow = () => { };
    }

    protected virtual void Hide()
    {
        IsVisible = false;
        dialog.SetActive(false);
        hideDialog.gameObject.SetActive(false);
        hideDialog.onClick.RemoveListener(Hide);
        OnDialogHide?.Invoke();
        OnDialogHide = () => { };
        
    }
}