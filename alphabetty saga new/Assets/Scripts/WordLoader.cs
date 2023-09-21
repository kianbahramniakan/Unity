using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WordLoader : MonoBehaviour
{
    public List<string> loadedWords = new List<string>(); // This list will store your words.

    void Start()
    {
        LoadWordsFromFile("E://Atomic Heart//Alphabetty-Saga-PlayableScene//alphabetty saga new//Assets//wordlist.txt"); // Load words from the text file.
    }

    // Function to load words from a text file.
    private void LoadWordsFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                string[] words = File.ReadAllLines(filePath);
                loadedWords.AddRange(words);
                Debug.Log("Loaded " + words.Length + " words from the file.");
            }
            catch (IOException e)
            {
                Debug.LogError("Error reading the file: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}