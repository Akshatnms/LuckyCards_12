using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Wheel
{
    class DoubleChanceWheel : MonoBehaviour
    {
        #region OuterWheel

        [SerializeField]float outerwheelangle;
        [SerializeField] int currentOuterWheelNumber;
        [SerializeField] int nextOuterWheelNumber;
        #endregion

        #region InnerWheel
        [SerializeField] float innerWheelAngle;
        [SerializeField] int currentInnerWheelNumber;
        [SerializeField] int nextInnerWheelNumber;
        #endregion


        #region Common Arguments
        private int wheelTime = 7;
        private int noOfRounds = 3;
        public bool isStarted;
        [SerializeField] GameObject outterWheel, innerWheel;
        #endregion
        public Action OnSpinComplete;

        int[] angles = { 0, 36, 72, 108, -216, -180, -144, -108, -72, -36 };

        public Button spinBtn;
        public int testNum1;
        public int testNum2;
        private void Start()
        {
            //spinBtn.onClick.AddListener(() =>
            //{
            //    Spin(testNum1,testNum2);
            //});
        }
        public iTween.EaseType easetype;


        public void Spin(int innerNum, int outerNum)
        {
            Debug.Log("Spin");
            int[] outerWheelNumbers = new int[] { 0, 6, 4, 7, 3, 8, 2, 9, 1, 5 };
            int[] innerWheelNumbers = new int[] { 3, 8, 2, 9, 1, 5, 0, 6, 4, 7 };
            int outerIndex = 0;
            int innerIndex = 0;
            for (int i = 0; i < outerWheelNumbers.Length; i++)
            {
                if (outerWheelNumbers[i] == outerNum)
                {
                    outerIndex = i;
                    break;
                }
            } 
            for (int i = 0; i < innerWheelNumbers.Length; i++)
            {
                if (innerWheelNumbers[i] == innerNum)
                {
                    innerIndex = innerWheelNumbers.Length-i;
                    Debug.Log("index is " + innerIndex);
                    break;
                }
            }
            
            OuterWheel(outerIndex);
            InnerWheel(innerIndex);
        }

        void InnerWheel(int number)
        {
            nextOuterWheelNumber = number;
            if (currentOuterWheelNumber == nextOuterWheelNumber)
            {
                innerWheelAngle = 0;
            }
            else if (currentOuterWheelNumber > nextOuterWheelNumber)
            {
                innerWheelAngle = Mathf.Abs(currentOuterWheelNumber - nextOuterWheelNumber) / 10f;
            }
            else
            {
                innerWheelAngle = Mathf.Abs(10 - (nextOuterWheelNumber - currentOuterWheelNumber)) / 10f;

            }
            innerWheelAngle += noOfRounds;
            SoundManager.instance.PlayClip("spinwheel");
            iTween.RotateBy(innerWheel, iTween.Hash("z", innerWheelAngle, "time", wheelTime,
                 "easetype", easetype, "oncompletetarget", this.gameObject));
            StartCoroutine(Playsound(5));

        }
        IEnumerator Playsound(float sec)
        {
            yield return new WaitForSeconds(sec);
            SoundManager.instance.PlayClip("spinwheel");
        }
        void OuterWheel(int number)
        {
            nextInnerWheelNumber = number;
            if (currentInnerWheelNumber == nextInnerWheelNumber)
            {
                outerwheelangle = 0;
            }
            else if (currentInnerWheelNumber > nextInnerWheelNumber)
            {
                outerwheelangle = Mathf.Abs(currentInnerWheelNumber - nextInnerWheelNumber) / 10f;
            }
            else
            {
                outerwheelangle = Mathf.Abs(10 - (nextInnerWheelNumber - currentInnerWheelNumber)) / 10f;

            }
            outerwheelangle += noOfRounds;
            StartCoroutine(Playsound(5));
            iTween.RotateBy(outterWheel, iTween.Hash("z", -outerwheelangle, "time", wheelTime + 1, "easetype", easetype,
                "oncomplete", "OnAnimationComplete", "oncompletetarget", this.gameObject));

        }
        void OnAnimationComplete()
        {
            OnSpinComplete?.Invoke();
            innerWheelAngle = noOfRounds - innerWheelAngle;
            outerwheelangle = noOfRounds - outerwheelangle;
            currentOuterWheelNumber = nextOuterWheelNumber;
            currentInnerWheelNumber = nextInnerWheelNumber;
        }
    }

   
}
