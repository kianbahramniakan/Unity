using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class WordChecker : MonoBehaviour
{
    public GameData currentGameData;
    public GameLevelData gameLevelData;
    private BoardData _boardRow;
    private string _word;

    private int _assignedPoints = 0;
    private int _completedWords = 0;
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();
    private bool _shouldCheckWord = false; // Flag to indicate whether to check the word

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
        GameEvents.OnLoadNextLevel -= LoadNextGameLevel;
        
    }

    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }

    void Start()
    {
        currentGameData.selectedBoardData.ClearData();
        _assignedPoints = 0;
        _completedWords = 0;
    }

    private void OnMouseUp()
    {
        checkWord();
    }

    void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);
            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);
        }

        // Check the word after a delay when _shouldCheckWord is true
        if (_shouldCheckWord)
        {
            /*StartCoroutine(CheckWordWithDelay());
            _shouldCheckWord = false; // Reset the flag*/
        }
    }

    private IEnumerator CheckWordWithDelay()
    {
        // Wait for a short delay before checking the word
        yield return new WaitForSeconds(0.02f); // Adjust the delay duration as needed

        // Perform word checking
        //checkWord();
    }

    private void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = squarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;

            _rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1));
            _rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1));
            _rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 0f));
            _rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 0f));
            _rayDiagonalLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 1));
            _rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, -1));
            _rayDiagonalRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 1));
            _rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, -1));
        }
        else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = SelectRay(_rayStartPosition, squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            _shouldCheckWord = true; // Set the flag to check the word
        }
        else
        {
            if (IsPointOnTheRay(_currentRay, squarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                _word += letter;
                _shouldCheckWord = true; // Set the flag to check the word
            }
        }

        Debug.Log(_assignedPoints);
        _assignedPoints++;
    }

    public bool IsTrueForDestroying = false;
    private WordsGrid _wordsGrid;
    private void checkWord()
    {
        
        // Load words from wordlist.txt
        string[] wordList = LoadWordListFromFile("E://Atomic Heart//Alphabetty-Saga-PlayableScene//alphabetty saga new//Assets//wordlist.txt");

        if (wordList != null)
        {
            foreach (string word in wordList)
            {
                if (_word.ToUpper() == word.ToUpper())
                {
                    IsTrueForDestroying = true;
                    GameEvents.CorrectWordMethod(_word, _correctSquareList);
                    _completedWords++;
                    _word = string.Empty;
                    _correctSquareList.Clear();
                    // Destroy correct words with the "True" tag
                    
                    GameObject[] trueWords = GameObject.FindGameObjectsWithTag("True");
                    
                    foreach (GameObject trueWord in trueWords)
                    {
                        Destroy(trueWord, 0.5f);
                    }

                    Debug.Log("gravity applied");
                    
                    gameObject.GetComponent<WordsGrid>().ApplyGravity();
                                        
                    GameObject[] UpertrueWords = GameObject.FindGameObjectsWithTag("UpTrue");
                    
                    foreach (GameObject letters in UpertrueWords)
                    {
                        letters.transform.Translate(0,-1.29f,0);
                    }

                    //FillUpWithRandom();
                    //_wordsGrid.SetSquaresPosition();
                    return;
                }
            }
        }
    }
    private string[] LoadWordListFromFile(string filePath)
    {
        try
        {
            // Read all lines from the file
            return File.ReadAllLines(filePath);
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading wordlist file: {e}");
            return null;
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);
        return true;
    }

    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1f)) < tolerance)
        {
            return _rayDown;
        }

        if (Mathf.Abs(direction.x - (-1f)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayLeft;
        }

        if (Mathf.Abs(direction.x - 1f) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayRight;
        }

        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }

        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }

        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }

        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }

        return _rayDown;
    }
    
    private void ClearSelection()
    {
        checkWord();
        //gameObject.GetComponent<WordsGrid>().ApplyGravity();

        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
        _shouldCheckWord = false; // Reset the flag when deselecting
        

    }

    private BoardData GameDataInstance;
    public void FillUpWithRandom()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], pattern: @"[a-zA-Z]").Count;
                string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int index = UnityEngine.Random.Range(0, letters.Length);

                if (errorCounter == 0)
                {
                    GameDataInstance.Board[i].Row[j] = letters[index].ToString();
                }
            }
        }
    }
    private void CheckBoardCompleted()
    {
        bool loadNextCategory = false;

        if (currentGameData.selectedBoardData.SearchWords.Count == _completedWords)
        {
            // Save current level progress
            var categoryName = currentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCatagoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;
            for (int index = 0; index < gameLevelData.data.Count; index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCatagoryCurrentIndexValues(gameLevelData.data[index].catagoryName);
                    readNextLevelName = false;
                }

                if (gameLevelData.data[index].catagoryName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;
                }
            }

            var currentLevelSize = gameLevelData.data[currentCategoryIndex].boardData.Count;
            if (currentBoardIndex < currentLevelSize)
                currentBoardIndex += 1;

            DataSaver.SaveCatagoryData(categoryName, currentBoardIndex);

            // Unlock Next Category
            if (currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;
                if (currentCategoryIndex < gameLevelData.data.Count) // If this is not the last category
                {
                    categoryName = gameLevelData.data[currentCategoryIndex].catagoryName;
                    currentBoardIndex = 0;
                    loadNextCategory = true;

                    if (nextBoardIndex <= 0)
                    {
                        DataSaver.SaveCatagoryData(categoryName, currentBoardIndex);
                    }
                }
                else
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod();
            }

            if (loadNextCategory)
                GameEvents.UnlockNextCategoryMethod();
        }
    }


    private void DestroyTrueWords()
    {
        GameObject[] squareGameObjects = GameObject.FindGameObjectsWithTag("Square");
        foreach (int squareIndex in _correctSquareList)
        {
            if (squareIndex >= 0 && squareIndex < squareGameObjects.Length)
            {
                Destroy(squareGameObjects[squareIndex]);
            }
        }
    }
}