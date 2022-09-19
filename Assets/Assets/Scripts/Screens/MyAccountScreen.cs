using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Com.BigWin.Frontend.Data;

namespace Screens
{
    public class MyAccountScreen : ScreenBlueprint 
    {
        public Toggle pointTransferBtn;
        public Toggle receivablesBtn;
        public Toggle transferablesBtn;
        public Toggle changePasswordBtn;
        public Toggle changePinBtn;
        public Button closeBtn,okBtn;
        public TMP_InputField passwordField, newPwdField, confirmPwdField;
        public  TextMeshProUGUI mainBalance;

        string pwd, newPwd, confPwd;
        public override void Initialize( ScreenController screenController)
        {
            base.Initialize(screenController);

            FindUIReferences();
            AddListners();
        }

        private void FindUIReferences()
        {
            //pointTransferBtn = screenObj.transform.FindRecursive("PointTransferButton").GetComponent<Toggle>();
            //receivablesBtn = screenObj.transform.FindRecursive("ReceivablesButton").GetComponent<Toggle>();
            //transferablesBtn = screenObj.transform.FindRecursive("TransferablesButton").GetComponent<Toggle>();
            //changePasswordBtn = screenObj.transform.FindRecursive("ChangePasswordButton").GetComponent<Toggle>();
            //closeBtn = screenObj.transform.FindRecursive("CloseButton").GetComponent<Button>();
            //mainBalance = screenObj.transform.FindRecursive("Chip").GetComponentInChildren<TextMeshProUGUI>();

           
        }

        private void AddListners()
        {
            passwordField.onValueChanged.AddListener((v) => {
                pwd = v;
            });
            confirmPwdField.onValueChanged.AddListener((v) => {
                confPwd = v;
            });
            newPwdField.onValueChanged.AddListener((v) => {
                newPwd = v;
            });
            okBtn.onClick.AddListener(() =>
            {
                ChangePwd();
            });
            closeBtn.onClick.AddListener(() => { sc.Back(); });
        }

        void ChangePwd()
        {
            if (string.IsNullOrEmpty(pwd)|| string.IsNullOrEmpty(confPwd)|| string.IsNullOrEmpty(newPwd))
            {
                AndroidToastMsg.ShowAndroidToastMessage("Invalid Password");
                return;
            }

            if (confPwd != newPwd)
            {
                AndroidToastMsg.ShowAndroidToastMessage("Password did not matched") ;
                return;
            }
            if (confPwd.Length<4&& newPwd.Length<4)
            {
                AndroidToastMsg.ShowAndroidToastMessage("Password is too short") ;
                return;
            }
            var o = Newtonsoft.Json.JsonConvert.SerializeObject(new { current_password = pwd, password = newPwd, confirm_password = confPwd });
            WebRequestHandler.instance.Post(Constant.CHANGE_PASSWORD_URL,o,(json,status)=> {
                Debug.Log(json);
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(json);
                AndroidToastMsg.ShowAndroidToastMessage(user.user_data.message);
            });
        }

        public override void Show(object data=null)
        {
            base.Show();
        }

        public override ScreenName ScreenID => global::ScreenName.MY_ACCOUNT_SCREEN;
         class UserData
        {
            public string message ;
            public string status ;
        }

         class User
        {
            public UserData user_data ;
        }


    }
}