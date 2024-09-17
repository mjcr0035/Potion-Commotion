using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    // List to store the ingredients currently in the pot
    public List<GameObject> ingredientsInPot = new List<GameObject>();

    // Called when a GameObject enters the pot's trigger zone
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is tagged as Salt or Pepper
        if (other.gameObject.CompareTag("Salt") || other.gameObject.CompareTag("Pepper"))
        {
            // Add the ingredient to the pot
            AddIngredient(other.gameObject);

            // Optionally destroy or disable the ingredient GameObject after adding it to the pot
            Destroy(other.gameObject);  // or other.gameObject.SetActive(false);

            // Provide feedback that the ingredient has been added
            Debug.Log($"{other.gameObject.tag} has been added to the pot.");
        }
    }

    // Method to add the ingredient to the list of ingredients in the pot
    public void AddIngredient(GameObject ingredient)
    {
        // Check to avoid adding the same ingredient multiple times
        if (!ingredientsInPot.Contains(ingredient))
        {
            Debug.Log("Ingred Added!");
            ingredientsInPot.Add(ingredient);
        }

        // After adding an ingredient, check if a food item can be created
        CheckForFoodCreation();
    }

    // Method to check if a valid combination of ingredients is in the pot
    private void CheckForFoodCreation()
    {
        // Flags to track if Salt and Pepper are present
        bool hasSalt = false;
        bool hasPepper = false;

        // Iterate over the ingredients in the pot
        foreach (var ingredient in ingredientsInPot)
        {
            // Check if the ingredient is Salt or Pepper by tag
            if (ingredient.CompareTag("Salt"))
                hasSalt = true;
            if (ingredient.CompareTag("Pepper"))
                hasPepper = true;
        }

        // If both Salt and Pepper are in the pot, create a food item
        if (hasSalt && hasPepper)
        {
            CreateFoodItem("Salt and Pepper Dish");
        }
    }

    // Method to create the food item and remove the ingredients from the pot
    private void CreateFoodItem(string foodItemName)
    {
        Debug.Log($"Created food item: {foodItemName}");

        // Clear the pot by destroying the ingredients (or disabling them if you prefer)
        foreach (var ingredient in ingredientsInPot)
        {
            Destroy(ingredient);  // Destroy the ingredient GameObject
        }

        // Clear the list of ingredients after creating the food item
        ingredientsInPot.Clear();

        // Optionally, you could instantiate a new GameObject to represent the food item in the scene
        // For now, we just log the creation of the food item
    }

    // For visualization, you can list all ingredients currently in the pot
    public void ShowIngredients()
    {
        Debug.Log("Current ingredients in the pot:");
        foreach (var ingredient in ingredientsInPot)
        {
            Debug.Log($"- {ingredient.tag}");
        }
    }
}

