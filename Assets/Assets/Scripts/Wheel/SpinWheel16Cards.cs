using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Wheel
{
    class SpinWheel16Cards : MonoBehaviour
    {
        #region FaceCard
        public float outerWheelangle;
        private int currentFaceCardNo;
        private int nextFaceCardNumber;
        [SerializeField] private FaceCard currenFaceCard;
        #endregion

        #region Card
        public float innerWheelAngle;
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


        public void Spin(int numbers)
        {
            FaceCard facecard = FaceCard.NONE;
            Card card = Card.NONE;


            switch ((CardsNumbers)numbers)
            {
                case CardsNumbers.ACES_OF_HEARTS:
                    facecard = FaceCard.ACES; card = Card.Hearts;
                    break;
                case CardsNumbers.ACES_OF_SPADES:
                    facecard = FaceCard.ACES; card = Card.Spade;
                    break;
                case CardsNumbers.ACES_OF_DIAMONDS:
                    facecard = FaceCard.ACES; card = Card.Diamond;
                    break;
                case CardsNumbers.ACES_OF_CLUBS:
                    facecard = FaceCard.ACES; card = Card.Clubs;
                    break;
                case CardsNumbers.JACK_OF_HEARTS:
                    facecard = FaceCard.Jack; card = Card.Hearts;
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
                Debug.Log("invalid wheel number from server");
                return;
            }
           
           
            outerWheel(facecard);
            innerWheels(card);
        }
        void outerWheel(FaceCard faceCard)
        {
            nextFaceCardNumber = (int)faceCard;
            currenFaceCard = faceCard;
            if (currentFaceCardNo == nextFaceCardNumber)
            {
                outerWheelangle = 0;
            }
            else if (currentFaceCardNo > nextFaceCardNumber)
            {
                outerWheelangle = Mathf.Abs(currentFaceCardNo - nextFaceCardNumber) / 16f;
            }
            else
            {
                outerWheelangle = Mathf.Abs(16 - (nextFaceCardNumber - currentFaceCardNo)) / 16f;

            }
            outerWheelangle += noOfRounds;
            currenFaceCard = faceCard;
            iTween.RotateBy(outterWheel, iTween.Hash("z", outerWheelangle, "time", wheelTime,
                 "easetype", easetype, "oncompletetarget", this.gameObject));

        }
        void innerWheels(Card card)
        {
            currenCard = card;
            nextCardNumber = (int)card;
            if (currentCardNo == nextCardNumber)
            {
                innerWheelAngle = 0;
            }
            else if (currentCardNo > nextCardNumber)
            {
                innerWheelAngle = Mathf.Abs(currentCardNo - nextCardNumber) / 16f;
            }
            else
            {
                innerWheelAngle = Mathf.Abs(16 - (nextCardNumber - currentCardNo)) / 16f;

            }
            innerWheelAngle += noOfRounds;
            currenCard = card;
            iTween.RotateBy(innerWheel, iTween.Hash("z", -innerWheelAngle, "time", wheelTime + 1, "easetype", easetype,
                "oncomplete", "OnAnimationComplete", "oncompletetarget", this.gameObject));

        }
        void OnAnimationComplete()
        {
            OnSpinComplete?.Invoke();
            outerWheelangle = noOfRounds - outerWheelangle;
            currentFaceCardNo = nextFaceCardNumber;
            innerWheelAngle = noOfRounds - innerWheelAngle;
            currentCardNo = nextCardNumber;
        }
        public enum FaceCard
        {
            King = 0,
            Queen = 3,
            Jack = 2,
            ACES = 1,
            NONE = -1,
        }
        public enum Card
        {
            Clubs = 0,
            Hearts = 1,
            Spade = 2,
            Diamond = 3,
            NONE = -1
        }
        public enum CardsNumbers
        {
            ACES_OF_HEARTS = 0,
            ACES_OF_SPADES = 1,
            ACES_OF_DIAMONDS = 2,
            ACES_OF_CLUBS = 3,
            JACK_OF_HEARTS = 4,
            JACK_OF_SPADES = 5,
            JACK_OF_DIAMONDS = 6,
            JACK_OF_CLUBS = 7,
            QUEEN_OF_HEARTS = 8,
            QUEEN_OF_SPADES = 9,
            QUEEN_OF_DIAMONDS = 10,
            QUEEN_OF_CLUBS = 11,
            KING_OF_HEARTS = 12,
            KING_OF_SPADES = 13,
            KING_OF_DIAMONDS = 14,
            KING_OF_CLUBS = 15,
        }

    }

}