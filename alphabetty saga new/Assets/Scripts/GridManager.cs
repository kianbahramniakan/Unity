using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public GameObject[] letterPrefabs; // Array to hold your 26 letter prefabs
    public int gridSize = 5;
    public float spacing = 10f;
    private LetterController _letterController;
    private GridSquare _gridSquare;
    private BoardData _boardRow;
    public GameObject colider;
    
    

    private void Start()
    {
        
        GenerateGrid();
        
    }

    private void Update()
    {

        
        
    }

    private void GenerateGrid()
    {
        if (letterPrefabs.Length != 26)
        {
            Debug.LogError("You should provide 26 letter prefabs.");
            return;
        }

        
        // Calculate total width and height of the grid
        float totalWidth = gridSize * (letterPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x + spacing);
        float totalHeight = gridSize * (letterPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.y + spacing);
        Instantiate(colider, new Vector3(0, -totalHeight/2, 0), Quaternion.identity);
        // Calculate starting position to center the grid
        Vector3 startPosition = new Vector3(-totalWidth / 2f, totalHeight / 2f, 0f);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Calculate the random index to select a letter prefab
                int randomIndex = Random.Range(0, 26);
                GameObject letterPrefab = letterPrefabs[randomIndex];

                // Calculate the position for this prefab
                Vector3 position = startPosition + new Vector3(x * (letterPrefab.GetComponent<SpriteRenderer>().bounds.size.x + spacing), -y * (letterPrefab.GetComponent<SpriteRenderer>().bounds.size.y + spacing), 0);

                // Instantiate the letter prefab
                GameObject letter = Instantiate(letterPrefab, position, Quaternion.identity);

                // Set the parent of the instantiated letter
                letter.transform.parent = transform;
            }
        }
    }
}