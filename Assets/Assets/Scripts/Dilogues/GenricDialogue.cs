using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenricDialogue : Dialog
{
   [SerializeField] private Button okButton;
   [SerializeField] private TextMeshProUGUI okButtonText;
    [SerializeField] private TextMeshProUGUI messageTextUI;
    public static GenricDialogue intance;
    private void Awake()
    {
        intance = this;
        okButton.onClick.AddListener(Hide);
        //gameObject.SetActive(false);

    }
    public GenricDialogue(Transform dialogContainer) : base(dialogContainer)
    {
        messageTextUI = dialog.transform.FindRecursive("Message").GetComponent<TextMeshProUGUI>();
        okButton = dialog.transform.FindRecursive("OKButton").GetComponent<Button>();
        okButtonText = okButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Show(string msg="Something Went Wrong", bool showOK = true, string okButtonMsg = "OK")
    {
        base.Show(showOK);
        messageTextUI.text = msg;
        this.okButtonText.text = okButtonMsg;
        okButton.gameObject.SetActive(showOK);
    }

    public new void Hide()
    {
        base.Hide();
        //okButton.onClick.RemoveListener(Hide);
    }


    protected override string DialogName => "GenricDialogue";
}