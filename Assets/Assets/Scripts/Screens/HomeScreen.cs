

using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Com.BigWin.Frontend.Data;
using System;
using Newtonsoft.Json;
using Socket;

namespace Screens
{
    public class HomeScreen : ScreenBlueprint
    {
        [SerializeField] TextMeshProUGUI chipTxt;
        [SerializeField] TextMeshProUGUI userIdTxt;
        [SerializeField] Button myAccountBtn;
        [SerializeField] Button logoutBtn;
        [SerializeField] Button funTargetTimerGameBtn;
        [SerializeField] Button jeetoJokerTimerGameBtn;
        [SerializeField] Button DoubleChanceGameBtn;
        [SerializeField] Button card16Btn;
        [SerializeField] Button changePwdBtn;

        public override void Initialize(ScreenController screenController)
        {
            base.Initialize(screenController);
            AddListners();
        }



        private void AddListners()
        {
            changePwdBtn.onClick.AddListener(() =>
            {
                sc.Show(ScreenName.MY_ACCOUNT_SCREEN);
            });
            try
            {


                DoubleChanceGameBtn.onClick.AddListener(() =>
                {

                    RegistertionInfo reg = new RegistertionInfo() { gameId = Games.DoubleChance, playerId = LocalDatabase.data.email };
                    Debug.Log(reg);
                    Action<string> OnCurrentRoundInfo = null;
                    SocketHandler.intance.SendEvent(Constant.RegisterPlayer, reg, (json) =>
                {
                            isRequestCompleted = false;
                            sc.Show(ScreenName.DOUBLE_CHANCE, data: (object)json);
                        }, Constant.OnCurrentTimer);
                    isRequestCompleted = true;
                });
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
                throw;
            }
            logoutBtn.onClick.AddListener(
                () =>
                {
                    SocketHandler.intance.SendEvent(Constant.OnLogout,
                        (res) =>
                        {
                            Debug.Log(res);
                        });
                    sc.Show(ScreenName.LOGIN_SCREEN);
                }
                );
            funTargetTimerGameBtn.onClick.AddListener(
                () =>
                {

                    RegistertionInfo reg = new RegistertionInfo() { gameId = Games.funWheel, playerId = LocalDatabase.data.email };
                    Debug.Log(reg);
                    Action<string> OnCurrentRoundInfo = null;
                    SocketHandler.intance.SendEvent(Constant.RegisterPlayer, reg, (json) =>
                    {
                        isRequestCompleted = false;
                        sc.Show(ScreenName.FUN_TARGET_TIMER_GAME_SCREEN, data: (object)json);
                    }, Constant.OnCurrentTimer);
                    isRequestCompleted = true;
                }
                );

            jeetoJokerTimerGameBtn.onClick.AddListener(
               () =>
               {
                   RegistertionInfo reg = new RegistertionInfo() { gameId = Games.JeetoJoker, playerId = LocalDatabase.data.email };
                   Debug.Log(reg);
                   Action<string> OnCurrentRoundInfo = null;
                   SocketHandler.intance.SendEvent(Constant.RegisterPlayer, reg, (json) =>
                   {
                       isRequestCompleted = false;
                       sc.Show(ScreenName.JEETO_JOKER_TIMER_GAME_SCREEN, data: (object)json);
                   }, Constant.OnCurrentTimer);
                   isRequestCompleted = true;
                   //sc.Show(ScreenName.JEETO_JOKER_TIMER_GAME_SCREEN);
               }
               );
            card16Btn.onClick.AddListener(
               () =>
               {
                   RegistertionInfo reg = new RegistertionInfo() { gameId = Games.card16, playerId = LocalDatabase.data.email };
                   Debug.Log(reg);
                   Action<string> OnCurrentRoundInfo = null;
                   SocketHandler.intance.SendEvent(Constant.RegisterPlayer, reg, (json) =>
                   {
                       isRequestCompleted = false;
                       sc.Show(ScreenName.CARD_16, data: (object)json);
                   }, Constant.OnCurrentTimer);
                   isRequestCompleted = true;
                   //sc.Show(ScreenName.JEETO_JOKER_TIMER_GAME_SCREEN);
               }
               );

        }

        string balance;
        string userId;
        void UpdateUi()
        {
            chipTxt.text = balance;
            userIdTxt.text = userId;
        }
        bool isRequestCompleted = false;
        public override void Show(object data = null)
        {
            base.Show();
            SoundManager.instance.PlayBackgroundMusic();
            isRequestCompleted = false;
            object user = new { user_id = LocalDatabase.data.email };
            SocketHandler.intance.SendEvent(Constant.OnUserProfile, user, (json) =>
            {
                BackEndData<PlayerProfile> profile = JsonUtility.FromJson<BackEndData<PlayerProfile>>(json);
                balance = profile.data.coins.ToString();
                userId = profile.data.user_id.ToUpperInvariant().ToString();
                UpdateUi();
            });


        }
        public override void Hide()
        {
            base.Hide();
            SoundManager.instance.StopBackgroundMusic();

        }

        public override ScreenName ScreenID => ScreenName.HOME_SCREEN;
    }
}

public class RegistertionInfo
{
    public Games gameId;
    public string playerId;
}

public class LoginData
{
    public int id;
    public string distributor_id;
    public string user_id;
    public string username;
    public object IMEI_no;
    public string device;
    public string last_logged_in;
    public string last_logged_out;
    public int IsBlocked;
    public string password;
    public string created_at;
    public string updated_at;
    public int active;
    public int coins;
}

[Serializable]
public class PlayerProfile
{
    public int id;
    public string distributor_id;
    public string user_id;
    public string username;
    public object IMEI_no;
    public string device;
    public DateTime last_logged_in;
    public DateTime last_logged_out;
    public int IsBlocked;
    public string password;
    public DateTime created_at;
    public DateTime updated_at;
    public int active;
    public int coins;
}
