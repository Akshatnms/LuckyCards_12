using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Com.BigWin.Frontend.Data
{
    public class Constant
    {
        // URLs and APIs
        public static readonly string BASE_URL = "https://www.fungameasiaa.com/fungames_asiaa/api";
        //public static readonly string BASE_URL = "https://www.fungameasiaa.com/api";
        public static readonly string LOGIN_URL = BASE_URL + "/login";
        public static readonly string LOGOUT_URL = BASE_URL + "/logout";
        public static readonly string PROFILE_URL = BASE_URL + "/profile";
        public static readonly string POINT_TRANSFER_URL = BASE_URL + "/send_point_to_user";
        public static readonly string FORCE_LOGIN_TRANSFER_URL = BASE_URL + "/update_device_id";
        public static readonly string RECEIVABLES_URL = BASE_URL + "/notification ";
        public static readonly string TRANSFERABLES_URL = BASE_URL + "/sender_notification";
        public static readonly string ACCEPT_POINTS_URL = BASE_URL + "/accept_points";
        public static readonly string SEND_POINTS_URL = BASE_URL + "/send_point_to_user";
        public static readonly string REJECT_POINTS_URL = BASE_URL + "/reject_points";
        public static readonly string CHANGE_PASSWORD_URL = BASE_URL + "/change_password";
        public static readonly string CHANGE_PIN_URL = BASE_URL + "/logout";
        public static readonly string GET_WIN_URL = BASE_URL + "/GameWinningNo";
        public static readonly string CURRENT_ROUND_URL = BASE_URL + "/current_round ";
        public static readonly string ADD_SHOWCURRENT_BET_URL = BASE_URL + "/add_showcurrent_bet";
        public static readonly string LAST_BET_DATA_URL = BASE_URL + "/lastBetUser";
        public static readonly string ADD_WIN_AMOUNT_URL = BASE_URL + "/addWinAmount";
        public static readonly string GET_APK_VERSION_URL = BASE_URL + "/getApkVersion";


        // Data Keys
        public const string EMAIL_DATA_KEY = "EMAIL";
        public const string PASSWORD_DATA_KEY = "PASSWORD";
        public static readonly string IS_INVALID_USER = "401";

        public const string PASSWORD_NOT_MATCHED = "Password not matched!\nPlease enter same Password and retry.";
        public const string PASSWORD_UPDATE_COMPLETE = "Password Changed Successfully.";


        public const string RegisterPlayer = "RegisterPlayer";
        public const string OnLogin = "OnLogin";
        public const string OnPlaceBet = "OnPlaceBet";
        public const string OnWinNo = "OnWinNo";
        public const string OnWinAmount = "OnWinAmount";
        public const string OnTakeAmount = "OnTakeAmount";
        public const string OnChipMove = "OnChipMove";
        public const string OnPlayerExit = "OnPlayerExit";
        public const string OnJoinRoom = "OnJoinRoom";
        public const string OnTimeUp = "OnTimeUp";
        public const string OnTimerStart = "OnTimerStart";
        public const string OnDrawCompleted = "OnDrawCompleted";
        public const string OnWait = "OnWait";
        public const string OnGameStart = "OnGameStart";
        public const string OnAddNewPlayer = "OnAddNewPlayer";
        public const string OnCurrentTimer = "OnCurrentTimer";
        public const string OnBetsPlaced = "OnBetsPlaced";
        public const string OnBotsData = "OnBotsData";
        public const string OnPlayerWin = "OnPlayerWin";
        public const string onEnterLobby = "onEnterLobby";
        public const string onleaveRoom = "onleaveRoom";
        public const string OnForceLogin = "OnForceLogin";
        public const string OnTimer = "OnTimer";
        public const string OnDissconnect = "disconnect";
        public const string OnUserInfo = "OnUserInfo";
        public const string OnChangePassword = "OnChangePassword";
        public const string OnNotification = "OnNotification";
        public const string OnsenderNotification = "OnsenderNotification";
        public const string OnSendPoints = "OnSendPoints";
        public const string OnAcceptPoints = "OnAcceptPoints";
        public const string OnRejectPoints = "OnRejectPoints";
        public const string OnReturnPoints = "OnReturnPoints";
        public const string OnUserProfile = "OnUserProfile";
        public const string OnLogout = "OnLogout";
        public const string OnPreBet = "OnPreBet";
        public const string OnDisconnect = "OnDisconnect";
        public static T GetObjectOfType<T>(object json) where T : class
        {
            T t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(json.ToString());
                return t;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return t;
        }
    }
    public enum Games
    {
        DoubleChance=1,
        funWheel = 4,
        JeetoJoker = 3,
        card16 = 2,
    }
    public class BackEndData3<T> where T : class
    {
        public string status;
        public string message;
        public T data;
    }
    public class BackEndData2<T> where T : class
    {
        public bool status;
        public string message;
        public T data;
    }  
    public class BackEndData<T> where T : class
    {
        public int status;
        public string message;
        public T data;
        public string eventName;
        public bool ValidateData(Action onSomethingWentWrong)
        {
            
            if (status != 200)
            {
                Debug.Log(" event :" + eventName);
                Debug.Log("Status :" + status);
                Debug.Log("msg: " + message);
                GenricDialogue.intance.Show();
                GenricDialogue.intance.OnDialogHide = onSomethingWentWrong;
                return false;
            }
            return true;
        }
    }
}