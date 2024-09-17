using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DraggableIngredient : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Collider2D ingredientCollider;

    void Start()
    {
        // Store the initial position of the ingredient
        originalPosition = transform.position;

        // Get the ingredient's collider for use in enabling/disabling it
        ingredientCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Handle dragging
        if (isDragging)
        {
            // Move the ingredient to the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);  // Z = 0 for 2D
        }
    }

    private void OnMouseDown()
    {
        // Start dragging when mouse button is pressed
        isDragging = true;

        // Optionally disable the collider while dragging so it doesn't interfere with trigger detection
        ingredientCollider.enabled = false;
    }

    private void OnMouseUp()
    {
        // Stop dragging when mouse button is released
        isDragging = false;

        // Re-enable the collider to allow collision detection with the pot
        ingredientCollider.enabled = true;

        // Optionally, snap back to original position if not dropped in the pot
        if (!IsOverPot())
        {
            transform.position = originalPosition;
        }
    }

    // Check if the ingredient is over the pot by checking collision overlap (optional feature)
    private bool IsOverPot()
    {
        // Check for collisions with the pot (you can adjust this method based on your setup)
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Pot"))  // Assuming the pot is tagged as "Pot"
            {
                Debug.Log("OVER POT!");
                return true;
            }
        }
        return false;
    }
}
