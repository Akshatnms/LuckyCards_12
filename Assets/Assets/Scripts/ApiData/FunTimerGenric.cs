using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Games.FunTarget.ApiData
{
    class FunTimerGenric<T> where T : class
    {
        private string userId;
        private int gameId;
        private object obj;
        Action<T> onRequetcomplete;
        public FunTimerGenric(object obj)
        {
            this.obj = obj;
        }
        public FunTimerGenric()
        {

        }
        public void SendRequestToServer(string url, Action<T> OnRequestComplete)
        {
            WebRequestHandler.instance.Post(url, JsonUtility.ToJson(obj), OnServerResponse);
            onRequetcomplete = OnRequestComplete;
        }

        public void SendGetRequestToServer(string url, Action<T> OnRequestComplete)
        {
            WebRequestHandler.instance.Get(url, OnServerResponse);
            onRequetcomplete = OnRequestComplete;
        }

        private void OnServerResponse(string json, bool status)
        {
            try
            {
                T currentRound = JsonUtility.FromJson<T>(json);
                if (!status)
                {
                    MonoBehaviour.print("something went wrong");
                    AndroidToastMsg.ShowAndroidDefaltMessage();
                    if (onRequetcomplete != null)
                        onRequetcomplete(null);
                }
                if (onRequetcomplete != null)
                    onRequetcomplete(currentRound);
            }
            catch
            {
                T obj = null;
                onRequetcomplete(obj);
            }
        }
    }




    public class User
    {
        public string user_id;
        public int game;//it the game id
    }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

namespace CurrentRoundNameSpace
{

    [Serializable]
    public class RoundWinData
    {
        public int ar_id;
        public string win_X;
        public int win_no;
        public int game;
    }

    [Serializable]
    public class CurrentRound
    {
        public string message;
        public string status;
        public string coins;
        public int round_count;
        public bool is_current_round_bet_place;
        public string sec;
        public string no_0;
        public string no_1;
        public string no_2;
        public string no_3;
        public string no_4;
        public string no_5;
        public string no_6;
        public string no_7;
        public string no_8;
        public string no_9;
        public string pre_round_win_amount;
        public List<RoundWinData> round_win_data;
    }
}

namespace LastBetNameSpace
{
    [Serializable]
    public class LastBet
    {
        public string round_count;
        public int winning_amount;
        public int win_no;
        public int no_0;
        public int no_1;
        public int no_2;
        public int no_3;
        public int no_4;
        public int no_5;
        public int no_6;
        public int no_7;
        public int no_8;
        public int no_9;
        public int is_winning_amount_add;
    }

    [Serializable]
    public class LastBetData
    {
        public string status;
        public string message;
        public LastBet last_bet;
    }
}

namespace BetNameSpace
{
    [Serializable]
    public class Bet
    {
        public long round_count;
        public string playerId;
        public string device;
        public string points;
        public int no_0;
        public int no_1;
        public int no_2;
        public int no_3;
        public int no_4;
        public int no_5;
        public int no_6;
        public int no_7;
        public int no_8;
        public int no_9;
        public int gameId;
    }

    public class BetConfirmation
    {
        public string status;
        public string message;
    }


}

namespace WinNampSpcae
{
    [Serializable]
    public class Win
    {

        public string status      ;
        public string message     ;
        public string round_count ;
        public int win_no         ;
        public string win_x       ;
        public string sec;
    };

}