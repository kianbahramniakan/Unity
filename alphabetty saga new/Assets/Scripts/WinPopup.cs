using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour
{
    public GameObject winPopup;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += ShowWinPopup;
        AdManager.OnInterstitialAdsClosed += InterstitialAdCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= ShowWinPopup;
        AdManager.OnInterstitialAdsClosed -= InterstitialAdCompleted;
    }

    private void InterstitialAdCompleted()
    {

    }

    private void ShowWinPopup()
    {
        
        winPopup.SetActive(true);
    }

    public void LoadNextLevel()
    {
        //AdManager.Instance.ShowInterstitialAd();

        GameEvents.LoadNextLevelMethod();
    }
}
