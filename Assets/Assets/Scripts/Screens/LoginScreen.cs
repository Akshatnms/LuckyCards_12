using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Com.BigWin.Frontend.Data;
using System.Text;
using System.CodeDom;
using System.Collections;
using System;
using System.Linq;
using UnityEditor;
using System.Collections.Generic;
using Socket;

namespace Screens
{
    public class LoginScreen : ScreenBlueprint 
    {
        private TMP_InputField userNameInput;
        private TMP_InputField passwordInput;
        private Toggle rememberPasswordToggle;
        private Button loginBtn;
        private Button exitBtn;
        public override void Initialize( ScreenController screenController)
        {
            base.Initialize(screenController);

            FindAllReferences();
            AddListeners();
        }

        private void FindAllReferences()
        {
            userNameInput =transform.FindRecursive("UserName").GetComponentInChildren<TMP_InputField>();
            passwordInput = transform.FindRecursive("Password").GetComponentInChildren<TMP_InputField>();
            rememberPasswordToggle = transform.FindRecursive("RememberPassword").GetComponentInChildren<Toggle>();
            loginBtn = transform.FindRecursive("LoginButton").GetComponent<Button>();
            exitBtn = transform.FindRecursive("ExitButton").GetComponent<Button>();
           
        }

        private void AddListeners()
        {
            loginBtn.onClick.AddListener(OnClickLoginButton);
            exitBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayClip("quit");
                Application.Quit();
            });
        }

        bool isServerVersionGreater = false;
        string newApkLink = string.Empty;
        bool isDataLoaded = false;
        public override void Show(object data = null)
        {
            base.Show();
            isServerVersionGreater = false;
            isLoginRequestSent = false;

            //newApkLink = "http://13.233.60.158:5000";
            // newApkLink = "http://65.0.12.36:4000";
            newApkLink = "http://65.1.1.23:5000";
            isDataLoaded = false;


            userNameInput.text = LocalDatabase.data.email?.Replace("FUN", string.Empty);
            passwordInput.text = LocalDatabase.data.password;

        }
        public override void Hide()
        {
            base.Hide();
            isLoginRequestSent = false;
        }
        void CheckAndroidVrsion()
        {

            webRequestHandler.Get(Constant.GET_APK_VERSION_URL, (json, status) =>
             {
                 ApkVersion apkVersion = JsonConvert.DeserializeObject<ApkVersion>(json);
                
                 isDataLoaded = true;
                 print($"server version {apkVersion.data.version_code}  and app version {Application.version}");
                 isServerVersionGreater = compareVersionCode(Application.version, apkVersion.data.version_code) == -1;
                 if (!isServerVersionGreater) return;
                  GenricDialogue.intance.Show("Please download the lastest version\npress ok to download");
                  GenricDialogue.intance.OnDialogHide = () => Application.OpenURL(apkVersion.data.ApkUrl);
             });
        }

        //return 0 if same, return 1 if version1 is greater and return -1 if version1 is lower
        public int compareVersionCode(string version1, string version2, char spliteType = '.')
        {

            string[] version1A = version1.Split(spliteType);
            string[] version2A = version2.Split(spliteType);



            // To avoid IndexOutOfBounds
            int maxIndex = Math.Min(version1A.Length, version2A.Length);

            for (int i = 0; i < maxIndex; i++)
            {
                int v1 = int.Parse(version1A[i]);
                int v2 = int.Parse(version2A[i]);

                if (v1 < v2)
                {
                    return -1;
                }
                else if (v1 > v2)
                {
                    return 1;
                }

            }
            return 0;
        }
        public override ScreenName ScreenID => global::ScreenName.LOGIN_SCREEN;


        bool isLoginRequestSent;
        private WebRequestHandler webRequestHandler;

        private void OnClickLoginButton()
        {
            SoundManager.instance.PlayClip("login");

            if (Application.platform == RuntimePlatform.Android)
                if (isLoginRequestSent)
                {
                     GenricDialogue.intance.Show("Please wait");
                    return;
                }

            if (Application.platform == RuntimePlatform.Android)
                if (isServerVersionGreater)
                {
                     GenricDialogue.intance.Show("Please download the lastest version\npress ok to download");
                     GenricDialogue.intance.OnDialogHide = () => Application.OpenURL(newApkLink);
                    return;
                }


            if (userNameInput.text == "" && passwordInput.text == "")
            {
                AndroidToastMsg.ShowAndroidToastMessage("Emplty Field");
                print("Emplty Field");
                return;
            }
            object form = new  { user_id = "FUN" + userNameInput.text, password = passwordInput.text, imei = "1321365464987", device = SystemInfo.deviceUniqueIdentifier };


            if (!SocketHandler.intance.isConnected)
            {
                 GenricDialogue.intance.Show("Please check your internet connection");
                return;
            }
            if (!isLoginRequestSent)
                SocketHandler.intance.SendEvent(Constant.OnLogin, form, (res) =>
            {
                BackEndData<LoginData> back = Constant.GetObjectOfType<BackEndData<LoginData>>(res);
                print("Login resposne " + back.status);
                isLoginRequestSent = false;
                OnLoginRequestProcessed(back.status, back.message,res);
            });
            isLoginRequestSent = true;
            //webRequestHandler.Post(Constant.LOGIN_URL, JsonUtility.ToJson(form), OnLoginRequestProcessed);
        }

        private void OnLoginRequestProcessed(int status, string message,object data=null)
        {
            if (status == 401)
            {
                 GenricDialogue.intance.Show(message);
                return;
            } 
            if (status == 404)
            {
                Debug.Log("incorrect password "+message);
                 GenricDialogue.intance.Show(message, okButtonMsg: "Try again");
                return;
            }
            if (status == 200)
            {
                SaveData();
                sc.Show(ScreenName.HOME_SCREEN,data);
                return;
            }
            else
            {
                if (status == 202)
                {
                     GenricDialogue.intance.Show(message, okButtonMsg: "Force Login");
                    ForceLogin form = new ForceLogin { user_id = "FUN" + userNameInput.text, password = passwordInput.text, device = SystemInfo.deviceUniqueIdentifier };
                     GenricDialogue.intance.OnDialogHide = () =>
                    {
                        SocketHandler.intance.SendEvent(Constant.OnForceLogin, form, (res) =>
                        {
                            BackEndData<LoginResponseData> back = Constant.GetObjectOfType<BackEndData<LoginResponseData>>(res);
                            print("Login resposne " + res);
                            OnLoginRequestProcessed(back.status, back.message);
                        },Constant.OnLogin);
                        SaveData();
                    };
                    return;
                }
            }

             GenricDialogue.intance.Show(message);
        }

        private void SaveData()
        {
            if (rememberPasswordToggle.isOn)
            {
                LocalDatabase.data.email="FUN" + userNameInput.text;
                LocalDatabase.data.password = passwordInput.text;
                LocalDatabase.SaveGame();
            }
            else
            {
                LocalDatabase.DeleteData();
            }
        }

      
    }
}

[Serializable]
public class LoginForm
{
    public string user_id;
    public string password;
    public string device;
    public string imei;
}

[Serializable]
public class LoginResponseData
{
    public LoginResponce user_data;
}

[Serializable]
public class LoginResponce
{
    public string message;
    public string status;
    public string coins;
    public long round_count;
    public string device;
    public string user_id;
}

[Serializable]
public class LogoutForm
{
    public string user_id;
    public LogoutForm(string user_id)
    {
        this.user_id = user_id;
    }
}
public class ApkData
{
    public string status;
    public string message;
    public string version_code;
    public string ApkUrl;
}

[Serializable]
public class ApkVersion
{
    public ApkData data;
}
[Serializable]
public class ForceLogin
{
    public string user_id;
    public string password;
    public string device;
}
public class UserData
{
    public string message;
    public string status;
    public string coins;
    public string user_id;
    public string device;
}
[Serializable]
public class ForceLoginResponse
{
    public UserData user_data;
}

public class e
{
    public string message { get; set; }
    public string status { get; set; }
}
[Serializable]
public class Invalide
{
    public e user_data { get; set; }
}
