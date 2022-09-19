using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    Dictionary<ScreenName, ScreenBlueprint> screenCollection;
    private Stack<ScreenName> screensStack;
    [SerializeField] private ScreenName currentScreenId, lastScreenId;
    private Dictionary<ScreenName, ScreenBlueprint> screensCollection;
    public static ScreenController intance;

    private void Awake()
    {
        intance = this;
    }
    public ScreenName StartingScreen;
    public void Start()
    {

        screensStack = new Stack<ScreenName>();
        screensCollection = new Dictionary<ScreenName, ScreenBlueprint>();
        LoadScreens();
        Show(StartingScreen);
    }
    private void LoadScreens()
    {
        currentScreenId = ScreenName.NONE;
        ScreenBlueprint[] s = this.GetComponentsInChildren<ScreenBlueprint>(true);
        foreach (ScreenBlueprint screen in s)
        {
            screen.Initialize(this);
            screensCollection.Add(screen.ScreenID, screen);
            screen.gameObject.SetActive(false);
        }

    }

    public void Show(ScreenName id, object data = null, bool iscomingfromBack = false)
    {
        try
        {


            if (currentScreenId == id) return;
            lastScreenId = currentScreenId;
            Debug.Log("Show Screen: " + id);
            SoundManager.instance.PlayClip("click");
            screensCollection[id].Show(data);
            if (!iscomingfromBack)
                screensStack.Push(lastScreenId);
            if (id == ScreenName.FUN_TARGET_TIMER_GAME_SCREEN)
            {

            }
            currentScreenId = id;
            Hide();
        }
        catch (Exception e)
        {

                screensStack.Push(lastScreenId);
            currentScreenId = id;
            Debug.Log("Show Screen: " + e.Message);
            Debug.Log("Show Screen: " + e.StackTrace);
            throw;
        }
    }
    public void ShowHomeScreen() => Show(ScreenName.HOME_SCREEN);
    void Hide()
    {
        if (lastScreenId == ScreenName.NONE) return;
        screensCollection[lastScreenId].Hide();

    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                Back();
            }
        }
    }
    protected virtual IEnumerator CallFuntionWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
    public void Back(object data = null)
    {
        if (currentScreenId == ScreenName.HOME_SCREEN) return;
        if (screensStack.Count == 0) return;
        ScreenName screen = screensStack.Pop();
        Show(screen, data, true);
    }
}
public enum ScreenName
{
    SPLASH_SCREEN,
    LOGIN_SCREEN,
    HOME_SCREEN,
    FUN_TARGET_TIMER_GAME_SCREEN,
    JEETO_JOKER_TIMER_GAME_SCREEN,
    MY_ACCOUNT_SCREEN,
    POINT_TRANSFER_SCREEN,
    RECEIVABLES_SCREEN,
    TRANSFERABLES_SCREEN,
    CHANGE_PASSWORD_SCREEN,
    CHANGE_PIN_SCREEN,
    DOUBLE_CHANCE,
    CARD_16,
    REPRINTSCREEN,
    NONE
}