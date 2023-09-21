using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    [System.Serializable]
    public class SearchingWord
    {
        [HideInInspector]
        public bool Found = false;
        public string Word;
    }

    [System.Serializable]
    public class BoardRow
    {
        private GridSquare _gridSquare;
        public int Size;
        public string[] Row;

        public BoardRow() { }

        public BoardRow(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            Size = size;
            Row = new string[Size];
            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < Size; i++)
            {
                Row[i] = " ";
            }
        }
        
    }

    public float timeInSeconds;
    public int Columns = 0;
    public int Rows = 0;

    public BoardRow[] Board;
    public List<SearchingWord> SearchWords = new List<SearchingWord>();

    public void ClearData()
    {
        foreach (var word in SearchWords)
        {
            word.Found = false;
        }
    }

    public void ClearWithEmptyString()
    {
        for (int i = 0; i < Columns; i++)
        {
            Board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        Board = new BoardRow[Columns];
        for (int i = 0; i < Columns; i++)
        {
            Board[i] = new BoardRow(Rows);
        }
    }

    public void LoadWordsFromFile(string filePath)
    {
        // Clear existing words
        SearchWords.Clear();

        // Read words from the file and add them to SearchWords
        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string word = line.Trim().ToUpper(); // Convert to uppercase
                if (!string.IsNullOrEmpty(word))
                {
                    SearchingWord searchingWord = new SearchingWord
                    {
                        Word = word,
                        Found = false
                    };
                    SearchWords.Add(searchingWord);
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading wordlist file: " + e.Message);
        }
    }

}
