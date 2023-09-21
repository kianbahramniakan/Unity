using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectPuzzleButton : MonoBehaviour
{
    public GameData gameData;
    public GameLevelData levelData;
    public Text categoryText;
    public Image progressBarFilling;

    private string gameSceneName = "GameScene";

    private bool _levelLocked;

    void Start()
    {
        _levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInformation();

        if (_levelLocked)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

    }

    private void OnEnable()
    {
        AdManager.OnInterstitialAdsClosed += InterstitialAdsClosed;
    }

    private void OnDisable()
    {
        AdManager.OnInterstitialAdsClosed -= InterstitialAdsClosed;
    }

    private void InterstitialAdsClosed()
    {

    }

    private void UpdateButtonInformation()
    {
        var currentIndex = -1;
        var totalBoards = 0;

        foreach (var data in levelData.data)
        {
            if (data.catagoryName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCatagoryCurrentIndexValues(gameObject.name);
                totalBoards = data.boardData.Count;

                if (levelData.data[0].catagoryName == gameObject.name && currentIndex < 0)
                {
                    DataSaver.SaveCatagoryData(levelData.data[0].catagoryName, 0);
                    currentIndex = DataSaver.ReadCatagoryCurrentIndexValues(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }

        if (currentIndex == -1)
            _levelLocked = true;

        categoryText.text = _levelLocked ? string.Empty : (currentIndex.ToString() + "/" + totalBoards.ToString());
        progressBarFilling.fillAmount =
            (currentIndex > 0 && totalBoards > 0) ? ((float)currentIndex / (float)totalBoards) : 0f;
    }

    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;

      //  AdManager.Instance.ShowInterstitialAd();

        SceneManager.LoadScene(gameSceneName);
    }
}
