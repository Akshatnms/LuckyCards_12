using System;
using System.Collections;
using Com.BigWin.Frontend.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class SplashScreen : ScreenBlueprint 
    {
        private Transform loadingScreen;
        private Slider loadingSlider;
        private TextMeshProUGUI loadingText;

        public override void Initialize(ScreenController screenController)
        {
            base.Initialize(screenController);

            loadingScreen = transform.FindRecursive("LoadingScreen");
            loadingSlider = transform.FindRecursive("LoadingSlider").GetComponent<Slider>();
            loadingText = transform.FindRecursive("LoadingText").GetComponent<TextMeshProUGUI>();

        }

        public override void Show(object data = null)
        {
            base.Show(data);
            Debug.Log("here");
            OnSplashTimeComplete();
            //StartCoroutine(CallFuntionWithDelay(2));
        }
        public override void Hide()
        {
            base.Hide();
        }
        public override ScreenName ScreenID => ScreenName.SPLASH_SCREEN;


        private void OnSplashTimeComplete()
        {
            loadingScreen.gameObject.SetActive(true);
            StartCoroutine(CallFuntionWithDelay(3));
        }


#if UNITY_EDITOR
        private IEnumerator QuickLoad(Action action)
        {
            yield return null;
            action();
        }
#endif

          IEnumerator CallFuntionWithDelay(float delay=3)
        {
            float t = 0;
            while (t < delay)
            {
                t += Time.deltaTime;
                if (loadingText != null) loadingText.text = "Loading " + (int)(t / delay * 100) + "%";
                loadingSlider.value = (int)(t / delay * 100);
                yield return null;
            }
            if (loadingText != null) loadingText.text = "Loading 100%";
            yield return new WaitForSeconds(1);
            loadingScreen.gameObject.SetActive(false);
            sc.Show(ScreenName.LOGIN_SCREEN);
        }
    }
}