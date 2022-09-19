
using System;
using System.Collections;
using UnityEngine;

public abstract class ScreenBlueprint : MonoBehaviour
{
    protected GameObject screenObj;
    protected ScreenController sc;
    protected bool isActive;//check the screen if currently active

    public virtual void Initialize(ScreenController screenController)
    {
        this.sc = screenController;
        screenObj = this.gameObject;
    }

    public abstract ScreenName ScreenID { get; }

    public virtual void Show(object data = null)
    {
        ActivateScreen(true);
        this.transform.SetAsLastSibling();
        //if (ScreenID == ScreenName.HOME) return;
    }

    public virtual void Back()
    {
        sc.Back();
        SoundManager.instance.playClick();

    }

    public virtual void Hide()
    {
        ActivateScreen(false);
    }

    public virtual void ActivateScreen(bool state)
    {
        screenObj.SetActive(state);
        isActive = state;

    }


}


