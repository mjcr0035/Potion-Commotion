using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIngredient2D : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging = false;
    public bool hasBeenAdded = false;

    //audioclips
    public AudioClip[] ingredientDragSounds;

    void Start()
    {
        // Store the original position of the ingredient
        originalPosition = transform.position;
        
    }

    void OnMouseDown()
    {
        // Begin dragging
        isDragging = true;
        AudioManager.Instance.PlayRandomSoundFXClip(ingredientDragSounds, transform, 0.6f, Random.Range(0.9f, 1.3f));

    }

    void OnMouseDrag()
    {
        // Move the ingredient with the mouse (2D coordinates)
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; // Set the distance from the camera (this can be adjusted)
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector2(worldPosition.x, worldPosition.y);

        }
    }

    void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;
        transform.position = originalPosition;
    }

    // Method to return the ingredient to its original position
    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        //hasBeenAdded = false;
    }
}

