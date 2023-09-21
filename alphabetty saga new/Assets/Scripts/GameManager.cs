using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager; // Reference to your GridManager script
    private List<LetterController> selectedLetters = new List<LetterController>();

    private void Start()
    {
        // Initialize your game here, if needed
    }

    private void Update()
    {
        // Check if all letters have been selected
        if (AllLettersSelected())
        {
            // Handle the game win condition
            Debug.Log("Congratulations! You've selected all letters.");
            // You can add more logic here for what happens when the player wins.
        }
    }

    private bool AllLettersSelected()
    {
        // Get all LetterController components in the scene
        LetterController[] allLetters = FindObjectsOfType<LetterController>();

        // Filter selected letters from all letters
        selectedLetters = allLetters.Where(letter => letter.IsSelected()).ToList();

        // Check if the count of selected letters matches the total number of letters
        return selectedLetters.Count == 26;
    }
}