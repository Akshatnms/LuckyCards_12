using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : MonoBehaviour
{

    float customAngle;
    int currentNumber;
    int nextnumber;
    int wheelTime = 7;
    int noOfRounds = 3;

    [HideInInspector]public bool isStarted;
    [SerializeField] GameObject wheel;
    public Action OnSpinComplete;

    int[] angles = { 0, 36, 72, 108, -216, -180, -144, -108, -72, -36 };

    public Button spinBtn;
    int wheelNo;
    private void Start()
    {
        //spinBtn.onClick.AddListener(() =>
        //{
        //    Spin(wheelNo);
        //});
    }
    public iTween.EaseType easetype;



    public void Spin(int number)
    {
        nextnumber = number;
        if (currentNumber == nextnumber)
        {
            customAngle = 0;
        }
        else if (currentNumber > nextnumber)
        {
            customAngle = Mathf.Abs(currentNumber - nextnumber) / 10f;
        }
        else
        {
            customAngle = Mathf.Abs(10 - (nextnumber - currentNumber)) / 10f;

        }
        customAngle += noOfRounds;
        iTween.RotateBy(wheel, iTween.Hash("z", -customAngle, "time", wheelTime,
              "oncomplete", "OnAnimationComplete", "easetype", easetype, "oncompletetarget", this.gameObject));
    }

    void OnAnimationComplete()
    {
        print("completed");

        OnSpinComplete?.Invoke();
        customAngle = noOfRounds - customAngle;
        wheel.transform.eulerAngles = new Vector3(0, 0, angles[nextnumber]);
        currentNumber = nextnumber;
    }

    public void SetWheelInitialAngle(int number)
    {
        wheel.transform.eulerAngles = new Vector3(0, 0, angles[number]);
        currentNumber = number;
    }

    public void ForceFullyStopWheel()
    {
        iTween.Stop(wheel);
    }
}
