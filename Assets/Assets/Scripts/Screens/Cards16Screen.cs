using Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Com.BigWin.Frontend.Data;
using Socket;
using BetNameSpace;
using Newtonsoft.Json;
using System;
using System.Linq;
using Wheel;

public class Cards16Screen : ScreenBlueprint
{
    [SerializeField] Toggle chipNo10Btn;
    [SerializeField] Toggle chipNo50Btn;
    [SerializeField] Toggle chipNo100Btn;
    [SerializeField] Toggle chipNo200Btn;
    [SerializeField] Toggle chipNo500Btn;
    [SerializeField] Toggle chipNo1000Btn;

    #region BETTING RETATED GRID
    //---------All Type Cards------------
    [SerializeField] Button[] cardsBtn;
    [SerializeField] Button[] ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn;
    [SerializeField] Button[] ALL_JACKS_QUEENS_KINGS_Btn;

    #endregion
    [SerializeField] Button exitBtn;
    [SerializeField] Button betOkBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] Button doubleBtn;
    [SerializeField] Button repeatBtn;

    [SerializeField] Sprite[] cardsType;//Heart Spead Daimond Clubs
    [SerializeField] Image[] cards;//Heart Spead Daimond Clubs


    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] TextMeshProUGUI balanceTxt;
    [SerializeField] TextMeshProUGUI totalBetsTxt;
    [SerializeField] TextMeshProUGUI commentTxt;
    [SerializeField] TextMeshProUGUI winTxt;


    const int SINGLE_BETS_LIMIT = 5000;
    const int OVERALL_BETS_LIMIT = 50000;
    const Games CURRENT_GAME_ID = Games.JeetoJoker;
    private bool isUserPlacedBets;
    private bool isBetConfirmed;
    private bool canPlaceBet;
    private bool isLastGameWinAmountReceived;
    private bool canPlacedBet;
    private bool isthisisAFirstRound;
    private bool isPreviousBetPlaced;
    private bool isdataLoaded;
    private bool isTimeUp;


    private int lastWinNumber;
    int[] ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER = new int[4];//All
    int[] CopyOflastround_ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER = new int[4];//All
    int[] CardsBetContiner = new int[16];
    int[] Copyoflastround_CardsBetContiner = new int[16];
    int[] ALL_JACKS_QUEENS_KINGS_BET_CONTINER = new int[4];
    int[] Copyoflastround_ALL_JACKS_QUEENS_KINGS_BET_CONTINER = new int[4];
    int currentlySelectedChip = 10;

    [SerializeField] float balance;
    int totalBets;
    string roundcount;
    string lastroundcount;
    string lastWinRoundcount;
    string isPreviousWinsRecivied;
    string winningAmount;
    string currentComment;
    string userId;
    string[] PrizeName;
    string[] commenstArray = { "Bets are Empty!!", "For Amusement Only", "Bet Accepted!! your bet amount is :", "Please click on Take", "Bets Confirmed" };
    RegistertionInfo user;
    public GameObject PrintingManagerObj;

    public override ScreenName ScreenID => global::ScreenName.CARD_16;
    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        AddListners();
    }
    private void AddListners()
    {
        exitBtn.onClick.AddListener(() =>
        {
            SocketHandler.intance.SendEvent(Constant.onleaveRoom);
            sc.Back();
        });

        betOkBtn.onClick.AddListener(() =>
        {
            OnBetsOk();
        });

        clearBtn.onClick.AddListener(() =>
        {
            if (!canPlaceBet) return;
            ResetAllBets();
        });

        doubleBtn.onClick.AddListener(() =>
        {
            Debug.Log("isdataLoaded " + isdataLoaded);
            int doubleBets = totalBets * 2;
            if (!isdataLoaded)
            {
                AndroidToastMsg.ShowAndroidToastMessage("please wait");
                return;
            }
            if (!canPlaceBet || isTimeUp) return;
            if (currentlySelectedChip == 0)
            {
                AndroidToastMsg.ShowAndroidToastMessage("please select a chip first");
                return;
            }

            if (balance < doubleBets || balance < doubleBets + totalBets)
            {
                AndroidToastMsg.ShowAndroidToastMessage("not enough balance");
                return;
            }
            for (int i = 0; i < CardsBetContiner.Length; i++)
            {
                CardsBetContiner[i] *= 2;
            }
            for (int i = 0; i < ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER.Length; i++)
            {
                ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[i] *= 2;
            }
            for (int i = 0; i < ALL_JACKS_QUEENS_KINGS_BET_CONTINER.Length; i++)
            {
                ALL_JACKS_QUEENS_KINGS_BET_CONTINER[i] *= 2;
            }
            UpdateUi();

            SoundManager.instance.PlayClip("addbet");
        });
        AddCardBetListners();
        repeatBtn.onClick.AddListener(() => { RepeatBets(); });
        chipNo10Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 10; });
        chipNo50Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 50; });
        chipNo100Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 100; });
        chipNo200Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 200; });
        chipNo500Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 500; });
        chipNo1000Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 1000; });
    }
    private void AddCardBetListners()
    {
        for (int i = 0; i < cardsBtn.Length; i++)
        {
            int index = i;
            cardsBtn[i].onClick.AddListener(() =>
            {
                AddBets(ref CardsBetContiner, index);
            });
        }


        for (int i = 0; i < ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn.Length; i++)
        {
            int index = i;
            ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[i].onClick.AddListener(() =>
            {
                AddBets(ref ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER, index);
            });
        }

        for (int i = 0; i < ALL_JACKS_QUEENS_KINGS_Btn.Length; i++)
        {
            int index = i;
            ALL_JACKS_QUEENS_KINGS_Btn[i].onClick.AddListener(() =>
            {
                AddBets(ref ALL_JACKS_QUEENS_KINGS_BET_CONTINER, index);

            });
        }
    }
    private void AddBets(ref int[] continer, int containerIndex)
    {
        Debug.Log("isdataLoaded " + isdataLoaded);
        if (!isdataLoaded)
        {
            AndroidToastMsg.ShowAndroidToastMessage("please wait");
            return;
        }
        if (!canPlaceBet || isTimeUp) return;
        if (currentlySelectedChip == 0)
        {
            AndroidToastMsg.ShowAndroidToastMessage("please select a chip first");
            return;
        }

        if (balance < currentlySelectedChip || balance < currentlySelectedChip + totalBets)
        {
            AndroidToastMsg.ShowAndroidToastMessage("not enough balance");
            return;
        }
        if (continer[containerIndex] + currentlySelectedChip > SINGLE_BETS_LIMIT)
        {
            AndroidToastMsg.ShowAndroidToastMessage("reached the limit");
            return;
        }
        continer[containerIndex] += currentlySelectedChip;
        UpdateUi();

        SoundManager.instance.PlayClip("addbet");
    }
    void RepeatBets()
    {
        int __totalBets = Copyoflastround_CardsBetContiner.Sum() +
            Copyoflastround_ALL_JACKS_QUEENS_KINGS_BET_CONTINER.Sum()+
            CopyOflastround_ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER.Sum();
        if (!isdataLoaded)
        {
            AndroidToastMsg.ShowAndroidToastMessage("please wait");
            return;
        }
        if (!canPlaceBet || isTimeUp) return;
        if (currentlySelectedChip == 0)
        {
            AndroidToastMsg.ShowAndroidToastMessage("please select a chip first");
            return;
        }

        if (balance < __totalBets)
        {
            AndroidToastMsg.ShowAndroidToastMessage("not enough balance");
            return;
        }
        CardsBetContiner = Copyoflastround_CardsBetContiner;
        ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER= CopyOflastround_ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER  ;
        ALL_JACKS_QUEENS_KINGS_BET_CONTINER = Copyoflastround_ALL_JACKS_QUEENS_KINGS_BET_CONTINER;
        UpdateUi();
        SoundManager.instance.PlayClip("addbet");
    }
    void OnBetsOk()
    {
        if (totalBets == 0)
        {
            commentTxt.text = "Bets Are Empty";
            return;
        }
        if (isTimeUp)
        {
            AndroidToastMsg.ShowAndroidToastMessage("Time UP");
            return;
        }
        BettingButtonInteractablity(false);
        commentTxt.text = "Bets Confirmed";
        clearBtn.interactable = false;
        betOkBtn.interactable = false;
        doubleBtn.interactable = false;
        repeatBtn.interactable = false;
        balance -= totalBets;
        isBetConfirmed = true;
        CopyOflastround_ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER;
        Copyoflastround_ALL_JACKS_QUEENS_KINGS_BET_CONTINER = ALL_JACKS_QUEENS_KINGS_BET_CONTINER;
        Copyoflastround_CardsBetContiner = CardsBetContiner;
        SendBets();
        UpdateUi();
    }
    private void SendBets()
    {
        if (totalBets == 0)
        {
            currentComment = commenstArray[0];
            UpdateUi();
            return;
        }

        var c = new Allcard
        {

            no_0 = CardsBetContiner[0],
            no_1 = CardsBetContiner[1],
            no_2 = CardsBetContiner[2],
            no_3 = CardsBetContiner[3],
            no_4 = CardsBetContiner[4],
            no_5 = CardsBetContiner[5],
            no_6 = CardsBetContiner[6],
            no_7 = CardsBetContiner[7],
            no_8 = CardsBetContiner[8],
            no_9 = CardsBetContiner[9],
            no10 = CardsBetContiner[10],
            no11 = CardsBetContiner[11],

        };
        var hsdc = new AllFaceCard
        {
            no_0 = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[0],
            no_1 = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[1],
            no_2 = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[2],
            no_3 = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[3],
        };

        var jqk = new FaceCard()
        {
            no_0 = ALL_JACKS_QUEENS_KINGS_BET_CONTINER[0],
            no_1 = ALL_JACKS_QUEENS_KINGS_BET_CONTINER[1],
            no_2 = ALL_JACKS_QUEENS_KINGS_BET_CONTINER[2],
        };


        Bets data = new Bets
        {

            gameId = (int)Games.JeetoJoker,
            playerId = LocalDatabase.data.email,
            allcards = c,
            allFaceCards = hsdc,
            faceCards = jqk,
            points = totalBets
        };
        PostBet(data);
        canPlaceBet = false;
    }
    private void PostBet(Bets data)
    {
        SocketHandler.intance.SendEvent(Constant.OnPlaceBet, data, (res) =>
        {
            var response = JsonConvert.DeserializeObject<BetConfirmation>(res);
            if (response == null)
            {
                return;
            }
            if (Constant.IS_INVALID_USER == response.status)
            {
                return;
            }
            if (response.status == "200") 
            {

                CopyOflastround_ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER;
                Copyoflastround_ALL_JACKS_QUEENS_KINGS_BET_CONTINER = ALL_JACKS_QUEENS_KINGS_BET_CONTINER;
                Copyoflastround_CardsBetContiner = CardsBetContiner; 
                balance -= totalBets; 
                isBetConfirmed = true; 
            }
            currentComment = response.message;
            UpdateUi();
            Debug.Log("is bet placed starus with statu - " + JsonUtility.FromJson<BetConfirmation>(res).status);

        });

    }
    public override void Show(object data = null)
    {
        base.Show(data);
        if (data == null)
        {
            Debug.Log("someting went wrong");
            return;
        }
        user = new RegistertionInfo() { gameId = Games.funWheel, playerId = LocalDatabase.data.email };
        var b = JsonConvert.DeserializeObject<currentRoundInfo>(data.ToString());

        isdataLoaded = true;
        for (int i = 0; i < b.previousWinData.Count; i++)
        {
            int cardNumber = b.previousWinData[i].winNo % 4;
            char[] jkq = new char[] { 'A','J', 'Q', 'K' };
            int faceNumber = b.previousWinData[i].winNo / 4;
            cards[i].transform.GetChild(0).GetComponent<Image>().sprite = cardsType[cardNumber];
            cards[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = jkq[faceNumber].ToString();
        }
        StartCoroutine(Timer(b.gametimer));
        balance = b.balance;
        UpdateUi();
        AddSocketListners();
    }
    void AddSocketListners()
    {
        Action onBadResponse = () => { sc.Back(); };

        SocketHandler.intance.ListenEvent<WeelData>(Constant.OnWinNo, (json) =>
        {
            if (!isActive) return;
            Debug.Log("round ended " + json);
            StartCoroutine(OnRoundEnd(json));
        }, onBadResponse);
        SocketHandler.intance.ListenEvent(Constant.OnTimerStart, (json) =>
        {
            if (!isActive) return;
            Debug.Log("Timer started");
            OnTimerStart();
        });
        SocketHandler.intance.ListenEvent(Constant.OnDissconnect, (json) =>
        {
            if (!isActive) return;
            print("dissconnected");
            GenricDialogue.intance.Show("Something Went Wrong\nPlease Login Again");
            GenricDialogue.intance.OnDialogHide = () => { ScreenController.intance.Show(ScreenName.LOGIN_SCREEN); };

        });
        SocketHandler.intance.ListenEvent<winAmount>(Constant.OnWinAmount, (json) =>
        {
            if (!isActive) return;
            Debug.Log("Recieved win amount " + json);
            StartCoroutine(ShowWinAmount(json.win_points));

        }, onBadResponse);


        SocketHandler.intance.ListenEvent(Constant.OnTimeUp, (json) =>
        {
            if (!isActive) return;
            Debug.Log("TimeUp");
            BettingButtonInteractablity(false);
            isTimeUp = true;
        });

    }
    #region GAME_FLOW
    private void OnTimerStart()
    {
        object o = new { user_id = LocalDatabase.data.email };
        SocketHandler.intance.SendEvent(Constant.OnTimer, o, (json) =>
        {
            var o = JsonConvert.DeserializeObject<roundInfo>(json);
            balance = o.balance;
            isdataLoaded = true;
            StopCoroutine(Timer());
            StartCoroutine(Timer(o.gametimer));
        });
        // StartCoroutine(GetCurrentTimer());
    }

    bool canShowWinAmount;
    IEnumerator ShowWinAmount(int winAmount)
    {
        Debug.Log("win amou nt "+winAmount);
        yield return new WaitUntil(() => canShowWinAmount);
        if (winAmount == 0)
        {
            commentTxt.text = "No Wins";
            yield break;
        }
        commentTxt.text = "You Won:" + winAmount;
        winTxt.text = "You Won:" + winAmount;

    }

    int currentTime = 0;
    bool canStopTimer;
    /// <summary>
    /// This is the 60 sec timer 
    /// </summary>
    /// <param name="counter"></param>
    /// <returns></returns>
    private IEnumerator Timer(int counter = 60) //60
    {
        isTimeUp = false;
        canPlaceBet = true;
        repeatBtn.interactable = betOkBtn.interactable = clearBtn.interactable = doubleBtn.interactable = true;
        isUserPlacedBets = false;
        isBetConfirmed = false;
        canPlaceBet = true;
        commentTxt.text = commenstArray[1];
        canStopTimer = false;
        canShowWinAmount = false;
        Debug.Log("timer started");
        while (counter > 0)
        {
            if (canStopTimer) yield break;
            timerTxt.text = counter.ToString();
            currentTime = counter;
            if (counter < 6)
            {
                BettingButtonInteractablity(false);

                repeatBtn.interactable = betOkBtn.interactable = clearBtn.interactable = doubleBtn.interactable = false;
                canPlaceBet = false;
            }
            yield return new WaitForSeconds(1f);
            counter--;
        }
        currentTime = 0;
        timerTxt.text = 0.ToString();
    }

    IEnumerator OnRoundEnd(WeelData o)
    {
        yield return new WaitUntil(() => currentTime == 0);
        int no = o.win_no;
        var sixteenCardsWheel = FindObjectOfType<SpinWheel16Cards>();
        sixteenCardsWheel.Spin(no);
        sixteenCardsWheel.OnSpinComplete = () =>
        {
            StartCoroutine(WinAnimation(no));
        };
    }

    IEnumerator WinAnimation(int winNumber)
    {
        int A_J_Q_K_WinIndex = 0;
        if (winNumber < 4) A_J_Q_K_WinIndex = 0;
        else
        if (winNumber < 8) A_J_Q_K_WinIndex = 1;
        else
        if (winNumber < 12) A_J_Q_K_WinIndex = 2;
       else if (winNumber < 15) A_J_Q_K_WinIndex = 3;

        int H_S_D_c_WinIndex = 0;
        if (winNumber == 0 || winNumber == 4|| winNumber == 12 || winNumber == 8) H_S_D_c_WinIndex = 0;
        else
        if (winNumber == 1 || winNumber == 5|| winNumber == 13 || winNumber == 9) H_S_D_c_WinIndex = 1;
        else
        if (winNumber == 2 || winNumber == 6|| winNumber == 14 || winNumber == 10) H_S_D_c_WinIndex = 2;
        else
        if (winNumber == 3 || winNumber == 7|| winNumber == 15 || winNumber == 11) H_S_D_c_WinIndex = 3;
        Debug.Log("Win Animation2");
        BettingButtonInteractablity(false);
        for (int i = 0; i < 5; i++)
        {
            cardsBtn[winNumber].interactable = true;
            ALL_JACKS_QUEENS_KINGS_Btn[A_J_Q_K_WinIndex].interactable = true;
            ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[H_S_D_c_WinIndex].interactable = true;
            yield return new WaitForSeconds(0.5f);
            cardsBtn[winNumber].interactable = false;
            ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[H_S_D_c_WinIndex].interactable = false;
            ALL_JACKS_QUEENS_KINGS_Btn[A_J_Q_K_WinIndex].interactable = false;
            yield return new WaitForSeconds(0.5f);
        }
        ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[H_S_D_c_WinIndex].interactable = true;
        ALL_JACKS_QUEENS_KINGS_Btn[A_J_Q_K_WinIndex].interactable = true;
        cardsBtn[winNumber].interactable = false;
        canShowWinAmount = true;
        ResetAllBets();
        UpdateUi();
    }
    void GetCurrentTimer()
    {
        Debug.Log("timer started");
        //yield return new WaitUntil(() => currentTime <= 0);
        object o = new { user_id = LocalDatabase.data.email };
        //update data
        SocketHandler.intance.SendEvent(Constant.OnTimer, o, (json) =>
        {
            var b = JsonConvert.DeserializeObject<currentRoundInfo>(json.ToString());

            isdataLoaded = true;
            for (int i = 0; i < b.previousWinData.Count; i++)
            {
                int cardNumber = b.previousWinData[i].winNo % 4;
                char[] jkq = new char[] {'A', 'J', 'Q', 'K' };
                int faceNumber = b.previousWinData[i].winNo / 4;
                cards[i].transform.GetChild(0).GetComponent<Image>().sprite = cardsType[cardNumber];

                cards[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = jkq[faceNumber].ToString();
            }
            balance = b.balance;
            if (b.gametimer < 10)
            {
                isTimeUp = true;
                BettingButtonInteractablity(false);
            };
            isdataLoaded = true;
            StopCoroutine(Timer());
            StartCoroutine(Timer(b.gametimer));
        });
    }
    #endregion
    void BettingButtonInteractablity(bool status)
    {
        foreach (var card in cardsBtn)
        {
            card.interactable = status;
        }
        foreach (var card in ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn)
        {
            card.interactable = status;
        }
        foreach (var card in ALL_JACKS_QUEENS_KINGS_Btn)
        {
            card.interactable = status;
        }
    }
    private void UpdateUi()
    {
        #region BETS
        PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Clear();
        for (int i = 0; i < cardsBtn.Length; i++)
        {
            cardsBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = CardsBetContiner[i].ToString();
            if(CardsBetContiner[i] > 0)
            {
                PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Add(cardsBtn[i].name, CardsBetContiner[i].ToString());
            }
        }
        for (int i = 0; i < ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn.Length; i++)
        {
            ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[i].GetComponentInChildren<TextMeshProUGUI>().text = ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[i].ToString();
            if(ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[i] > 0)
            {
                PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Add(ALL_HEARTS_SPADES_DIAMONDS_CLUBS_Btn[i].name, ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER[i].ToString());
            }
        }
        for (int i = 0; i < ALL_JACKS_QUEENS_KINGS_Btn.Length; i++)
        {
            ALL_JACKS_QUEENS_KINGS_Btn[i].GetComponentInChildren<TextMeshProUGUI>().text = ALL_JACKS_QUEENS_KINGS_BET_CONTINER[i].ToString();
            if(ALL_JACKS_QUEENS_KINGS_BET_CONTINER[i] > 0)
            {
                PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Add(ALL_JACKS_QUEENS_KINGS_Btn[i].name, ALL_JACKS_QUEENS_KINGS_BET_CONTINER[i].ToString());
            }
        }
        totalBets = CardsBetContiner.Sum() + ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER.Sum() + ALL_JACKS_QUEENS_KINGS_BET_CONTINER.Sum();
        #endregion
        balanceTxt.text = balance.ToString();
        totalBetsTxt.text =  totalBets.ToString();
        LocalDatabase.data.balance = balance;
        LocalDatabase.SaveGame();
    }
    private void ResetAllBets()
    {

        CardsBetContiner = new int[16];
        ALL_JACKS_QUEENS_KINGS_BET_CONTINER = new int[4];
        ALL_HEARTS_SPADES_DIAMONDS_CLUBS_BET_CONTINER = new int[4];
        totalBets = 0;
        isUserPlacedBets = false;
        canPlaceBet = true;
        isTimeUp = false;

        //UI
        BettingButtonInteractablity(true);
        clearBtn.interactable = true;
        betOkBtn.interactable = true;
        doubleBtn.interactable = true;
        repeatBtn.interactable = true;
        UpdateUi();

    }
    public override void Hide()
    {
        base.Hide();
        RemoveSocketListners();
        ResetAllBets();

    }
    void RemoveSocketListners()
    {
        SocketHandler.intance.RemoveListners(Constant.OnWinNo);
        SocketHandler.intance.RemoveListners(Constant.OnTimerStart);
        SocketHandler.intance.RemoveListners(Constant.OnDissconnect);
        SocketHandler.intance.RemoveListners(Constant.OnWinAmount);
        SocketHandler.intance.RemoveListners(Constant.OnTimeUp);
    }

    class winAmount
    {

        public int win_no;
        public int win_points;
        public string player_id;
        public int balance;
    }

    public class previousWinData
    {
        public int winNo;
    }

    public class currentRoundInfo
    {
        public int balance;
        public int gametimer;
        public List<PreviousWinData> previousWinData;
    }

    class roundInfo
    {
        public int balance;
        public int gametimer;
        //public List<weelNumbers> previousWinData;
    }
}