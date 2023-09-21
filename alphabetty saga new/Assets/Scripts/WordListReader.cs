using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordListReader : MonoBehaviour
{
    public static List<string> validWords = new List<string>();

    void Awake()
    {
        LoadWordList();
    }

    void LoadWordList()
    {
        string filePath = Application.dataPath + "/wordlist.txt"; // Path to your wordlist.txt file

        if (File.Exists(filePath))
        {
            string[] words = File.ReadAllLines(filePath);
            validWords.AddRange(words);
        }
        else
        {
            Debug.LogError("Wordlist file not found!");
        }
    }
}