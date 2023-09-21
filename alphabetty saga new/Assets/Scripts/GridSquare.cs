using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    private int _index = -1;
    private bool _selected;
    private bool _clicked;
    private bool _correct;
    private AlphabetData.LetterData _normalLetterData;
    private AlphabetData.LetterData _selectedLetterData;
    private AlphabetData.LetterData _correctLetterData;
    private SpriteRenderer _displayedImage;
    private string currentLetter;
    
    public int SquareIndex { get; set; }
    public Image background;
    public Image letterImage;
    public Text letterText;

    private AudioSource _source;

    public void SetIndex(int index)
    {
        _index = index;
    }

    public int GetIndex()
    {
        return _index;
    }

    public string GetLetter()
    {
        return currentLetter;
    }

    public void SetLetter(string letter)
    {
        currentLetter = letter;
        letterText.text = letter;
    }

    void Start()
    {
        _selected = false;
        _clicked = false;
        _correct = false;
        _displayedImage = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var trueWords = GameObject.FindGameObjectsWithTag("Square");
    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += SelectSquare;
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= SelectSquare;
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    private void CorrectWord(string word, List<int> squareIndexes)
    {
        if (_selected && squareIndexes.Contains(_index))
        {
            _correct = true;
            this.gameObject.tag = "True";
            _displayedImage.sprite = _correctLetterData.image;
        }

        _selected = false;
        _clicked = false;
    }

    public void OnEnableSquareSelection()
    {
        _clicked = true;
        _selected = false;
    }

    public void OnDisableSquareSelection()
    {
        _selected = false;
        _clicked = false;

        if (_correct == true)
        {
            _displayedImage.sprite = _correctLetterData.image;
            this.gameObject.tag = "True";
        }
        else
        {
            _displayedImage.sprite = _normalLetterData.image;
            this.gameObject.tag = "Square";
        }
    }

    public void SelectSquare(Vector3 position)
    {
        if (this.gameObject.transform.position == position)
            _displayedImage.sprite = _selectedLetterData.image;
    }

    public void SetSprite(AlphabetData.LetterData normalLetterData, AlphabetData.LetterData selectedLetterData,
        AlphabetData.LetterData correctLetterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = _normalLetterData.image;
    }

    private void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();
        CheckSquare();
        _displayedImage.sprite = _selectedLetterData.image;
    }

    private void OnMouseEnter()
    {
        CheckSquare();
    }

    private void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    public void CheckSquare()
    {
        if (_selected == false && _clicked == true)
        {
            _selected = true;
            GameEvents.CheckSquareMethod(_normalLetterData.letter, gameObject.transform.position, _index);
        }
    }

    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

        public void ApplyGravity()
    {
        // Loop through each row (from bottom to top)
        for (int r = 0; r < currentGameData.selectedBoardData.Rows - 1; r++)
        {
            for (int c = 0; c < currentGameData.selectedBoardData.Columns; c++)
            {
                GameObject currentSquare = _squareList[c * currentGameData.selectedBoardData.Rows + r];
                GameObject squareBelow = _squareList[c * currentGameData.selectedBoardData.Rows + r + 1];
                currentSquare.tag = "UpTrue";
                if (currentSquare != null && squareBelow == null)
                {
                    // If the current square exists but the one below is empty, move the current square down
                    _squareList[c * currentGameData.selectedBoardData.Rows + r + 1] = currentSquare;
                    _squareList[c * currentGameData.selectedBoardData.Rows + r] = null;
                    
                    currentSquare.GetComponent<Transform>().position += new Vector3(0f, -squareOffset, 0f);
                }
            }
        }

        // After applying gravity, generate random letters in the top row
        for (int c = 0; c < currentGameData.selectedBoardData.Columns; c++)
        {
            GameObject topSquare = _squareList[c * currentGameData.selectedBoardData.Rows];

            if (topSquare == null)
            {
                // If the top square is empty, generate a random letter
                int randomIndex = Random.Range(0, alphabetData.AlphabetNormal.Count);
                var randomLetterData = alphabetData.AlphabetNormal[randomIndex];

                GameObject newSquare = Instantiate(gridSquarePrefab);
                newSquare.transform.SetParent(this.transform);
                newSquare.transform.position = new Vector3(c * squareOffset, topPosition, 0f);

                GridSquare gridSquare = newSquare.GetComponent<GridSquare>();
                gridSquare.SetSprite(randomLetterData, randomLetterData, randomLetterData);
                _squareList[c * currentGameData.selectedBoardData.Rows] = newSquare;
            }
        }
    }
    /*private BoardData _boardData;
    private BoardData.BoardRow _boardRow;
    private WordsGrid _wordsGrid;
    private string[] row;
    private IEnumerator AddGravity()
    {

        row = _boardRow.Row;
        int col = _boardData.Columns;
        int nullCounter = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                
            }
        }
    }*/
}
