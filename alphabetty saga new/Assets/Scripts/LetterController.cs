using UnityEngine;

public class LetterController : MonoBehaviour
{
    public Sprite normalSprite; // Assign the normal sprite in the Inspector
    public Sprite selectedSprite; // Assign the selected sprite in the Inspector
    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    private bool isMouseHeldDown = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Set the initial sprite to the normal sprite
    }

    private void OnMouseEnter()
    {
        if (isMouseHeldDown && !isSelected)
        {
            isSelected = true;
            spriteRenderer.sprite = selectedSprite; // Show the selected sprite when the mouse enters
        }
    }

    private void OnMouseExit()
    {
        if (isMouseHeldDown && isSelected)
        {
            isSelected = false;
            spriteRenderer.sprite = normalSprite; // Revert to the normal sprite when the mouse exits
        }
    }

    private void OnMouseDown()
    {
        // Toggle the selection state
        isSelected = !isSelected;
        isMouseHeldDown = true;

        // Show/hide the selected sprite based on the selection state
        spriteRenderer.sprite = isSelected ? selectedSprite : normalSprite;
    }

    private void OnMouseUp()
    {
        isMouseHeldDown = false;
    }

    // Add a method to check if the letter is selected
    public bool IsSelected()
    {
        return isSelected;
    }
}