using BetNameSpace;
using Com.BigWin.Frontend.Data;
using Newtonsoft.Json;
using Socket;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Wheel;

namespace Screens
{
    public class DoubleChanceScreen : ScreenBlueprint
    {
        public override ScreenName ScreenID => ScreenName.DOUBLE_CHANCE;

        [SerializeField] Toggle chipNo10Btn;
        [SerializeField] Toggle chipNo50Btn;
        [SerializeField] Toggle chipNo100Btn;
        [SerializeField] Toggle chipNo200Btn;
        [SerializeField] Toggle chipNo500Btn;
        [SerializeField] Toggle chipNo1000Btn;

        #region BETTING RETATED GRID
        //---------All Type Cards------------
        [SerializeField] Button[] doubleGridBtns;
        [SerializeField] Button[] singleGridBtns;
        [SerializeField] Button[] randomPickButtonsBtns;

        #endregion
        [SerializeField] Button exitBtn;
        [SerializeField] Button betOkBtn;
        [SerializeField] Button clearBtn;
        [SerializeField] Button doubleBtn;
        [SerializeField] Button repeatBtn;

        [SerializeField] TextMeshProUGUI timerTxt;
        [SerializeField] TextMeshProUGUI balanceTxt;
        [SerializeField] TextMeshProUGUI totalBetsTxt;
        [SerializeField] TextMeshProUGUI winTxt;
        [SerializeField] TextMeshProUGUI commentTxt;
        [SerializeField] TextMeshProUGUI previousSingleWinsTxt;
        [SerializeField] TextMeshProUGUI previousDoubleWinsTxt;


        const int SINGLE_BETS_LIMIT = 5000;
        const int OVERALL_BETS_LIMIT = 50000;

        private bool isUserPlacedBets;
        private bool isBetConfirmed;
        private bool canPlaceBet;
        private bool isLastGameWinAmountReceived;
        private bool canPlacedBet;
        private bool isthisisAFirstRound;
        private bool isPreviousBetPlaced;
        private bool isdataLoaded;
        private bool isTimeUp;


        int[] randomPick = new int[] { 5, 10, 15, 20, 25, 50, 75,3 };
        private int lastWinNumber;
        int[] Double_Bets_Container = new int[12];
        int[] CopyArray_Double_Bets_Container = new int[12];
        int[] Single_Bets_Container = new int[10];
        int[] CopyArray_Single_Bets_Container = new int[10];
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
        public GameObject PrintingManagerObj;

        public override void Initialize(ScreenController screenController)
        {
            base.Initialize(screenController);
            AddListners();
            Double_Bets_Container = new int[doubleGridBtns.Length];
            CopyArray_Double_Bets_Container = new int[doubleGridBtns.Length];
            Single_Bets_Container = new int[singleGridBtns.Length];
            CopyArray_Single_Bets_Container = new int[singleGridBtns.Length];
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
            for (int i = 0; i < randomPickButtonsBtns.Length; i++)
            {
                int _index = i;
                randomPickButtonsBtns[_index].onClick.AddListener(() =>
                {
                    int index = _index;
                    OnRandomPick(randomPick[index]);
                });
            }
            AddCardBetListners();
            repeatBtn.onClick.AddListener(() => RepeatBets());
            doubleBtn.onClick.AddListener(() => OnDoubleBets());
            chipNo10Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 10; });
            chipNo50Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 50; });
            chipNo100Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 100; });
            chipNo200Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 200; });
            chipNo500Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 500; });
            chipNo1000Btn.onValueChanged.AddListener((i) => { if (!i) return; currentlySelectedChip = 1000; });

        }
        void OnRandomPick(int max)
        {
            int maxValue = max * currentlySelectedChip;
            if ((maxValue + totalBets) > balance)
            {
                Debug.Log("Not Enough Balance");
                AndroidToastMsg.ShowAndroidToastMessage("Not Enough Balance");
                return;
            }
            for (int i = 0; i < max; i++)
            {
                int _index = UnityEngine.Random.Range(0, 99);
                AddBets(ref Double_Bets_Container, _index);
            }
        }
        private void AddCardBetListners()
        {
            for (int i = 0; i < doubleGridBtns.Length; i++)
            {
                int index = i;
                doubleGridBtns[i].onClick.AddListener(() =>
                {
                    AddBets(ref Double_Bets_Container, index);
                });
                doubleGridBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 33f;
            }


            for (int i = 0; i < singleGridBtns.Length; i++)
            {
                int index = i;
                singleGridBtns[i].onClick.AddListener(() =>
                {
                    AddBets(ref Single_Bets_Container, index);
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
        void OnDoubleBets()
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
            for (int i = 0; i < Double_Bets_Container.Length; i++)
            {
                Double_Bets_Container[i] *= 2;
            }

            for (int i = 0; i < Single_Bets_Container.Length; i++)
            {
                Single_Bets_Container[i] *= 2;
            }
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


            bets data = new bets
            {

                gameId = (int)Games.DoubleChance,
                playerId = LocalDatabase.data.email,
                @double = Double_Bets_Container,
                single = Single_Bets_Container,
                points = Single_Bets_Container.Sum() + Double_Bets_Container.Sum()

            };
            PostBet(data);
            canPlaceBet = false;
        }
        void RepeatBets()
        {
            int __totalBets =
                CopyArray_Single_Bets_Container.Sum() +
                CopyArray_Double_Bets_Container.Sum();
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
            Single_Bets_Container = CopyArray_Single_Bets_Container;
            Double_Bets_Container = CopyArray_Double_Bets_Container;
            UpdateUi();
            SoundManager.instance.PlayClip("addbet");
        }
        private void PostBet(bets data)
        {
            Debug.LogError("data   " + data);
            SocketHandler.intance.SendEvent(Constant.OnPlaceBet, data, (res) =>
            {
                Debug.LogError("bets res   " + res );
                var response = JsonConvert.DeserializeObject<BetConfirmation>(res);
                if (response == null)
                {
                    return;
                }
                if (Constant.IS_INVALID_USER == response.status)
                {
                    return;
                }
                if (response.status == "200") { balance -= totalBets; isBetConfirmed = true; }
                currentComment = response.message;
                CopyArray_Single_Bets_Container = Single_Bets_Container;
                CopyArray_Double_Bets_Container = Double_Bets_Container;
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
            var o = JsonConvert.DeserializeObject<roundInfo>(data.ToString());
            balance = o.balance;
            isdataLoaded = true;
            string Singlewins = "Single: ";
            string Doublewins = "Double: ";
            foreach (var w in o.previousWinData)
            {
                Singlewins += (w.outer_win_no + " ").ToString();
                Doublewins += (w.outer_win_no + w.inner_win_no + " ").ToString();
            }
            previousSingleWinsTxt.text = Singlewins;
            previousDoubleWinsTxt.text = Doublewins;
            StartCoroutine(Timer(o.gametimer));
            try
            {

                UpdateUi();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
                throw;
            }
            AddSocketListners();
        }
        void AddSocketListners()
        {
            Action onBadResponse = () => { sc.Back(); };

            SocketHandler.intance.ListenEvent<weelNumbers>(Constant.OnWinNo, (json) =>
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
        IEnumerator ShowWinAmount(int winAmount)
        {
            Debug.Log("timer started");
            yield return new WaitUntil(() => currentTime < 56 && currentTime > 50);
            if (winAmount == 0)
            {
                commentTxt.text = "No Wins";
                yield break;
            }
            commentTxt.text = "You Won:" + winAmount;
            winTxt.text = "Wins:" + winAmount;

        }
        private void OnTimerStart()
        {
            object o = new { user_id = LocalDatabase.data.email };
            SocketHandler.intance.SendEvent(Constant.OnTimer, o, (json) =>
             {
                 var o = JsonConvert.DeserializeObject<roundInfo>(json);
                 balance = o.balance;
                 isdataLoaded = true;
                 string Singlewins = "Single: ";
                 string Doublewins = "Double: ";
                 foreach (var w in o.previousWinData)
                 {
                     Singlewins += (w.outer_win_no + " ").ToString();
                     Doublewins += (w.outer_win_no + w.inner_win_no + " ").ToString();
                 }
                 previousSingleWinsTxt.text = Singlewins;
                 previousDoubleWinsTxt.text = Doublewins;
                 StopCoroutine(Timer());
                 Debug.LogError("timer  " + o.gametimer);
                 StartCoroutine(Timer(o.gametimer));
             });
            // StartCoroutine(GetCurrentTimer());
        }
        IEnumerator GetCurrentTimer()
        {
            Debug.Log("timer started");
            //yield return new WaitUntil(() => currentTime <= 0);
            SocketHandler.intance.SendEvent(Constant.OnTimer, (json) =>
            {
                var o = JsonConvert.DeserializeObject<roundInfo>(json);
                balance = o.balance;
                isdataLoaded = true;
                string Singlewins = "Single: ";
                string Doublewins = "Double: ";
                foreach (var w in o.previousWinData)
                {
                    Singlewins += (w.outer_win_no + " ").ToString();
                    Doublewins += (w.outer_win_no + w.inner_win_no + " ").ToString();
                }
                previousSingleWinsTxt.text = Singlewins;
                previousDoubleWinsTxt.text = Doublewins;
                StopCoroutine(Timer());
                StartCoroutine(Timer(o.gametimer));
            });
            yield break;
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
            Debug.Log("timer started");
            while (counter > 0)
            {
                if (canStopTimer) yield break;
                timerTxt.text = counter.ToString();
                currentTime = counter;
                if (counter < 6)
                {
                    repeatBtn.interactable = betOkBtn.interactable = clearBtn.interactable = doubleBtn.interactable = false;
                    canPlaceBet = false;
                    if (!isBetConfirmed)
                    {
                        OnBetsOk();
                    }
                }
                yield return new WaitForSeconds(1f);
                counter--;
            }
            currentTime = 0;
            timerTxt.text = 0.ToString();
        }

        IEnumerator OnRoundEnd(weelNumbers o)
        {
            yield return new WaitUntil(() => currentTime == 0);
            var DCW = FindObjectOfType<DoubleChanceWheel>();
            DCW.Spin(o.inner_win_no, o.outer_win_no);
            DCW.OnSpinComplete = () =>
            {
                int doubleWin = int.Parse(o.outer_win_no.ToString() + o.inner_win_no.ToString());
                StartCoroutine(WinAnimation(doubleWin, o.outer_win_no));
            };
        }
        #endregion

        int index;
        IEnumerator WinAnimation(int doubleWinNumber, int singleWinNumber)
        {

            Debug.Log("Win Animation " + index++);
            BettingButtonInteractablity(false);
            for (int i = 0; i < 5; i++)
            {
                doubleGridBtns[doubleWinNumber].interactable = true;
                singleGridBtns[singleWinNumber].interactable = true;
                yield return new WaitForSeconds(0.5f);
                doubleGridBtns[doubleWinNumber].interactable = false;
                singleGridBtns[singleWinNumber].interactable = false;
                yield return new WaitForSeconds(0.5f);
            }
            singleGridBtns[singleWinNumber].interactable = true;
            doubleGridBtns[doubleWinNumber].interactable = false;
            ResetAllBets();
            UpdateUi();
        }
        void BettingButtonInteractablity(bool status)
        {
            foreach (var card in doubleGridBtns)
            {
                card.interactable = status;
            }
            foreach (var card in singleGridBtns)
            {
                card.interactable = status;
            }

        }
        private void UpdateUi()
        {
            #region BETS
            PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Clear();
            for (int i = 0; i < doubleGridBtns.Length; i++)
            {
                string v = Double_Bets_Container[i] == 0 ? $"{i}" : Double_Bets_Container[i].ToString();
                if (Double_Bets_Container[i] != 0)
                {
                    doubleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
                else
                {
                    doubleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
                }
                doubleGridBtns[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = i.ToString();
                doubleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = v;
                if(Double_Bets_Container[i] > 0)
                {
                    PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Add(doubleGridBtns[i].name, Double_Bets_Container[i].ToString() );
                }
            }
            for (int i = 0; i < singleGridBtns.Length; i++)
            {
                string v = Single_Bets_Container[i] == 0 ? $"{i}" : Single_Bets_Container[i].ToString();
                if (Single_Bets_Container[i] != 0)
                {
                    singleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

                }
                else
                {
                    singleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;


                }
                singleGridBtns[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = i.ToString();
                singleGridBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = v;
                if(Single_Bets_Container[i] > 0)
                {
                    PrintingManagerObj.GetComponent<PrintingManager>()._ticketContent.Add(singleGridBtns[i].name, Single_Bets_Container[i].ToString() );
                }
            }


            totalBets = Double_Bets_Container.Sum() + Single_Bets_Container.Sum();
            #endregion              
            balanceTxt.text = balance.ToString();
            totalBetsTxt.text = "TOTAL:" + totalBets.ToString();
            LocalDatabase.data.balance = balance;
            LocalDatabase.SaveGame();
        }
        private void ResetAllBets()
        {

            Double_Bets_Container = new int[100];
            Single_Bets_Container = new int[10];
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

        class roundInfo
        {
            public int balance;
            public int gametimer;
            public List<weelNumbers> previousWinData;
        }
        class weelNumbers

        {
            public int outer_win_no;
            public int inner_win_no;
        }
        public class winAmount
        {
            public int outer_win;
            public int inner_win;
            public int win_points;
            public string player_id;
            public int balance;
        }
        public class bets
        {
            public string playerId;
            public int gameId;
            public int points;
            public int[] single, @double;
        }
    }

}

