using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ADScript : MonoBehaviour
{
    //public Slider sliderHome;
    //public Slider sliderFuelCar;
    //public float rewardBonusSliderHome;
    //public float rewardBonusSliderFuel;
    //public float lowBalanceFuel;
    public string nameScene;
    public GameObject PanelDeath;
    public GameObject inMainMenuButton;
    public TMP_Text adsText;

    private int i;
    public void ShareFriend(){
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLPluginJS.ShareFunction();
#endif
    }

    public void ShowAdInterstitial(){
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.InterstitialFunction();
#endif
    }

    public void ShowAdReward(){
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.RewardFunction();
#endif
       // sliderHome.value += rewardBonusSliderHome;
    	//if(sliderFuelCar.value<=lowBalanceFuel) sliderFuelCar.value += rewardBonusSliderFuel;
    }

    //Change language

    public void SetEnglish(string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    	PlayerPrefs.SetString("lang", message);
#endif

    }
    private void Start()
    {
        i = 0;
        if (nameScene == "Menu") ShowAdInterstitial();
        
    }
    private void Update()
    {
        CheckAds();
        //if (sliderHome.value <= sliderHome.minValue) ShowAdInterstitial();
        
        if (PanelDeath.activeSelf && i == 0) 
        {
            i++;
            StartCoroutine("Pause");
           // ShowAdInterstitial();
        } 
        
    }

    IEnumerator Pause()
    {
        inMainMenuButton.SetActive(false);
        adsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        adsText.text = "До начала рекламы: 4";
        yield return new WaitForSeconds(1f);
        adsText.text = "До начала рекламы: 3";
        yield return new WaitForSeconds(1f);
        adsText.text = "До начала рекламы: 2";
        yield return new WaitForSeconds(1f);
        adsText.text = "До начала рекламы: 1";
        yield return new WaitForSeconds(1f);
        Debug.Log("Ads");
#if UNITY_WEBGL && !UNITY_EDITOR
    	WebGLPluginJS.InterstitialFunction();
#endif
        adsText.gameObject.SetActive(false);
        inMainMenuButton.SetActive(true);
        
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            AudioListener.pause = false;

        }
        else
        {
            AudioListener.pause = true;
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            AudioListener.pause = false;
        }
        else
        {
            AudioListener.pause = true;
        }
    }

    public void CheckAds()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if(WebGLPluginJS.GetAdsOpen() == "yes")
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
#endif
    }
}
