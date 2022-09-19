using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JeetoJoker
{
    class JeetoJokerWheel : MonoBehaviour
    {

        #region FaceCard
        public float faceCardcustomAngle;
        private int currentFaceCardNo;
        private int nextFaceCardNumber;
        [SerializeField] private FaceCard currenFaceCard;
        #endregion

        #region Card
        public float cardcustomAngle;
        private int currentCardNo;
        private int nextCardNumber;
        [SerializeField] private Card currenCard;
        #endregion


        #region Common Arguments
        [SerializeField] private int wheelTime = 7;
        [SerializeField] private int noOfRounds = 3;
        [HideInInspector] public bool isStarted;
        [SerializeField] GameObject outterWheel, innerWheel;
        #endregion
        public Action OnSpinComplete;

        int[] angles = { 0, 36, 72, 108, -216, -180, -144, -108, -72, -36 };

        public Button spinBtn;
        public int wheelNo;
        public float innerWheelNo;
        public float outerWheelNo;
        public FaceCard nextFaceCard;
        public Card nextCard;
        private void Start()
        {
            //spinBtn.onClick.AddListener(() =>
            //{
            //    Spin(wheelNo);
            //});
        }
        public iTween.EaseType easetype;

        public void Spin(FaceCard faceCard, Card card)
        {
            Debug.Log("Spin");
            SpinFaceCardWheel(faceCard);
            SpinCardWheel(card);
        }  public void Spin(int faceCard, int card)
        {
            Debug.Log("Spin");
            SpinFaceCardWheel((FaceCard)faceCard);
            SpinCardWheel((Card)card);
        } 
       public void Spin(int numbers)
        {
            FaceCard facecard=FaceCard.NONE;
            Card card=Card.NONE;
            switch ((CardsNumbers)numbers)
            {
                case CardsNumbers.JACK_OF_HEARTS:facecard =
                        FaceCard.Jack;card = Card.Hearts;
                    break;
                case CardsNumbers.JACK_OF_SPADES:
                    facecard = FaceCard.Jack; card = Card.Spade;
                    break;
                case CardsNumbers.JACK_OF_DIAMONDS:
                    facecard = FaceCard.Jack; card = Card.Diamond;
                    break;
                case CardsNumbers.JACK_OF_CLUBS:
                    facecard = FaceCard.Jack; card = Card.Clubs;
                    break;
                case CardsNumbers.QUEEN_OF_HEARTS:
                    facecard = FaceCard.Queen; card = Card.Hearts;
                    break;
                case CardsNumbers.QUEEN_OF_SPADES:
                    facecard = FaceCard.Queen; card = Card.Spade;
                    break;
                case CardsNumbers.QUEEN_OF_DIAMONDS:
                    facecard = FaceCard.Queen; card = Card.Diamond;
                    break;
                case CardsNumbers.QUEEN_OF_CLUBS:
                    facecard = FaceCard.Queen; card = Card.Clubs;
                    break;
                case CardsNumbers.KING_OF_HEARTS:
                    facecard = FaceCard.King; card = Card.Hearts;
                    break;
                case CardsNumbers.KING_OF_SPADES:
                    facecard = FaceCard.King; card = Card.Spade;
                    break;
                case CardsNumbers.KING_OF_DIAMONDS:
                    facecard = FaceCard.King; card = Card.Diamond;
                    break;
                case CardsNumbers.KING_OF_CLUBS:
                    facecard = FaceCard.King; card = Card.Clubs;
                    break;
                default:
                    break;
            }
            if (card == Card.NONE || facecard == FaceCard.NONE)
            {
                AndroidToastMsg.ShowAndroidDefaltMessage();
                return;
            }
            SpinFaceCardWheel(facecard);
            SpinCardWheel(card);
        }
        void SpinFaceCardWheel(FaceCard faceCard)
        {
            nextFaceCardNumber = (int)faceCard;
            currenFaceCard = faceCard;
            if (currentFaceCardNo == nextFaceCardNumber)
            {
                faceCardcustomAngle = 0;
            }
            else if (currentFaceCardNo > nextFaceCardNumber)
            {
                faceCardcustomAngle = Mathf.Abs(currentFaceCardNo - nextFaceCardNumber) / 12f;
            }
            else
            {
                faceCardcustomAngle = Mathf.Abs(12 - (nextFaceCardNumber - currentFaceCardNo)) / 12f;

            }
            faceCardcustomAngle += noOfRounds;
            currenFaceCard = faceCard;
            iTween.RotateBy(outterWheel, iTween.Hash("z", faceCardcustomAngle, "time", wheelTime,
                 "easetype", easetype, "oncompletetarget", this.gameObject));
          
        }
        void SpinCardWheel(Card card)
        {
            currenCard = card;
            nextCardNumber = (int)card;
            if (currentCardNo == nextCardNumber)
            {
                cardcustomAngle = 0;
            }
            else if (currentCardNo > nextCardNumber)
            {
                cardcustomAngle = Mathf.Abs(currentCardNo - nextCardNumber) / 12f;
            }
            else
            {
                cardcustomAngle = Mathf.Abs(12 - (nextCardNumber - currentCardNo)) / 12f;

            }
            cardcustomAngle += noOfRounds;
            currenCard = card;
            iTween.RotateBy(innerWheel, iTween.Hash("z", -cardcustomAngle, "time", wheelTime+1, "easetype", easetype,
                "oncomplete", "OnAnimationComplete", "oncompletetarget", this.gameObject));
            
        }
         void OnAnimationComplete()
        {
            OnSpinComplete?.Invoke();
            faceCardcustomAngle = noOfRounds - faceCardcustomAngle;
            currentFaceCardNo = nextFaceCardNumber;
            cardcustomAngle = noOfRounds - cardcustomAngle;
            currentCardNo = nextCardNumber;
        }
     
    }

    public enum FaceCard
    {

        Jack = 0,
        Queen = 1,
        King = 2,
        NONE=-1,
    }
    public enum Card
    {
        Diamond = 0,
        Spade = 1,
        Hearts = 2,
        Clubs = 3,
        NONE=-1
    }
    public enum CardsNumbers
    {
        JACK_OF_HEARTS = 0,
        JACK_OF_SPADES = 1,
        JACK_OF_DIAMONDS = 2,
        JACK_OF_CLUBS = 3,
        QUEEN_OF_HEARTS = 4,
        QUEEN_OF_SPADES = 5,
        QUEEN_OF_DIAMONDS = 6,
        QUEEN_OF_CLUBS = 7,
        KING_OF_HEARTS = 8,
        KING_OF_SPADES = 9,
        KING_OF_DIAMONDS = 10,
        KING_OF_CLUBS = 11,
    }
}
