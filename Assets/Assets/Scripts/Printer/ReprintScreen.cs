using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ApiPrototype;
using System.Linq;
public class ReprintScreen : ScreenBlueprint
{
    public override ScreenName ScreenID => ScreenName.REPRINTSCREEN;
    [SerializeField] public GameObject content;
    [SerializeField] public GameObject printPrefab;

    [SerializeField] public Button back;

    int gameId;
    public override void Initialize(ScreenController screenController)
    {
        base.Initialize(screenController);
        back.onClick.AddListener(() =>
        {
            if (gameId == 1)
            {
                // screenController.Show(ScreenName.criketScreen);
                return;
            }
            // screenController.ShowBettingScreen();
        });
    }
    void PopulateContent(ThisRoundBets obj)
    {
        if (obj.status != 200 || obj == null)
        {
            // commonPopup.Show();
            return;
        }
        if (obj.data.Count == 0)
            // commonPopup.Show("No Tickets Found");
            Debug.Log("No Tickets Found");
        for (int i = 0; i < obj.data.Count; i++)
        {

            int totalSpots = obj.data[i].betInfo.Count;
            Dictionary<string, BetInfo> bets = new Dictionary<string, BetInfo>();
            for (int j = 0; j < obj.data[i].betInfo.Count; j++)
            {
                string spotName = obj.data[i].betInfo[j].series + obj.data[i].betInfo[j].bet_no;
                BetInfo betinfo = new BetInfo()
                {
                    amount = int.Parse(obj.data[i].betInfo[j].points),
                    serise = char.Parse(obj.data[i].betInfo[j].series),
                    spotNo = int.Parse(obj.data[i].betInfo[j].bet_no),
                    ticketValue = "2",
                    qty = obj.data[i].betInfo[j].qty,
                    singleTicketPrize = int.Parse(obj.data[i].betInfo[j].points),

                };
                if (!bets.ContainsKey(spotName))
                    bets.Add(spotName, betinfo);

            }
            float serviseChrge = bets.Sum(x => x.Value.amount) * poolPrizePc / 100;
            float poolPriz = bets.Sum(x => x.Value.amount) - serviseChrge;

            //Indtantiate prefab
            var prefab = Instantiate(printPrefab);
            prefab.transform.parent = content.transform;
            ReprintPrefab reprint = prefab.GetComponent<ReprintPrefab>();
            string barcodeNo = obj.data[i].barcode_no;
            string drawtime = obj.data[i].draw_time;
            // Printer p = new Printer(obj.data[i].playing_time, obj.data[i].playing_date, totalSpots.ToString(),
            //     drawtime, barcodeNo,
            //     bets, poolPriz, serviseChrge, (GameIds)int.Parse(obj.data[i].game_id), 40);
            // reprint.Initialize(i + 1, obj.data[i].playing_time, barcodeNo, p);
            // prefab.transform.localScale = new Vector2(1, 1);
           
        }
    }
    public override void Show(object data = null)
    {
        // base.Show(data);
        // if (data != null) gameId = (int)data;
        // object user = new
        // {
        //     retailer_id = screenController.dataStorageController.LoginResponseData.data.retailer_id
        //   ,
        //     game_id = gameId
        // };
        // var poolPrizeObj = new GenricWebHandler<PoolPrize>();
        // poolPrizeObj.SendGetRequestToServer(Constant.POOL_PRIZE_URL, OnPoolPrize);

        // var obj = new GenricWebHandler<ThisRoundBets>(user);
        // obj.SendRequestToServer(Constant.REPRINT_URL, PopulateContent);
    }

    float poolPrizePc;
    void OnPoolPrize(PoolPrize p)
    {
        if (p == null) return;
        if (p.status != 200)
        {
            return;
        }

        poolPrizePc = p.data.pool_prize_percentage;
    }
    public override void Hide()
    {
        base.Hide();


        foreach (Transform chile in content.transform)
        {
            Destroy(chile.gameObject);

        }
    }
}



public class BetsData
{
    public string retailer;
    public string game_id;
    public string barcode_no;
    public string amount;
    public string draw_time;
    public string playing_date;
    public string playing_time;
    public List<Bet> betInfo;
}

public class ThisRoundBets
{
    public int status;
    public string message;
    public List<BetsData> data;
}

public class BetInfo
{
    public int amount;
    public string ticketValue;
    public char serise;
    public int spotNo;
    public int qty;
    public int singleTicketPrize;
}

