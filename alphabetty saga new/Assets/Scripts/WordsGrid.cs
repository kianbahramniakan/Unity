using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

    void Start()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    public void SetSquaresPosition()
    {
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();

        var Offset = new Vector2
        {
            x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f
        };

        var startPosition = GetFirsSquarePosition();
        int columnNumber = 0;
        int rowNumber = 0;

        foreach (var square in _squareList)
        {
            if (rowNumber +1 > currentGameData.selectedBoardData.Rows)
            {
                columnNumber++;
                rowNumber = 0;
            }

            var positionX = startPosition.x + Offset.x * columnNumber;
            var positionY = startPosition.y - Offset.y * rowNumber;

            square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
            rowNumber++;
        }
        Debug.Log(_squareList.Count);
    }

    private Vector2 GetFirsSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = _squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squareTransform = _squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);

        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midWidthPosition = (((currentGameData.selectedBoardData.Columns - 1) * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = (((currentGameData.selectedBoardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y += midWidthHeight;

        return startPosition;
    }

    private void SpawnGridSquares()
    {
        if (currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach (var squares in currentGameData.selectedBoardData.Board)
            {
                foreach (var squareLetter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);

                    if (normalLetterData.image == null || selectedLetterData == null)
                    {
                        Debug.LogError("All files in your  array should have some letters. Press Fill up with random button in your board data to add random letter. Letter:" + squareLetter);
                        
                        #if UNITY_EDITOR

                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
                        #endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridSquarePrefab));
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctLetterData, selectedLetterData);
                        _squareList[_squareList.Count - 1].transform.SetParent(this.transform);
                        _squareList[_squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        _squareList[_squareList.Count - 1].transform.localScale = squareScale;
                        _squareList[_squareList.Count - 1].GetComponent<GridSquare>().SetIndex(_squareList.Count - 1);
                    }

                }
            }
        }
    }

    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.Columns * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = ((currentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = midWidthHeight;

        return startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition;
    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
    
// Generate random letters on empty grid squares
    public void GenerateRandomLettersOnEmptyGrids()
    {
        // Iterate through each row
        for (int r = 0; r < currentGameData.selectedBoardData.Rows; r++)
        {
            // Iterate through each column
            for (int c = 0; c < currentGameData.selectedBoardData.Columns; c++)
            {
                GameObject square = _squareList[c * currentGameData.selectedBoardData.Rows + r];

                if (square != null)
                {
                    GridSquare gridSquare = square.GetComponent<GridSquare>();

                    // Check if the square is part of a correct word
                    
                    
                        // Check if the square has an empty letter
                     
                        
                            // Generate a random letter
                            int randomIndex = Random.Range(0, alphabetData.AlphabetNormal.Count);
                            var randomLetterData = alphabetData.AlphabetNormal[randomIndex];
                            // Update the square with the random letter
                            gridSquare.SetSprite(randomLetterData, randomLetterData, randomLetterData);
                    
                }
            }
        }
    }
    public void ApplyGravity()
    {
        GameObject temp;
        // Loop through each row (from bottom to top)
        for (int r = 0; r < currentGameData.selectedBoardData.Rows - 1; r++)
        {
            for (int c = 0; c < currentGameData.selectedBoardData.Columns; c++)
            {
                GameObject currentSquare = _squareList[c * currentGameData.selectedBoardData.Rows + r];
                GameObject squareBelow = _squareList[c * currentGameData.selectedBoardData.Rows + r + 1];
                if (currentSquare != null && squareBelow == null)
                {
                    // If the current square exists but the one below is empty, move the current square down
                    _squareList[c * currentGameData.selectedBoardData.Rows + r + 1] = currentSquare;
                    currentSquare.tag = "UpTrue";
                    _squareList[c * currentGameData.selectedBoardData.Rows + r] = null;
                }
            }
        }
    }
}
