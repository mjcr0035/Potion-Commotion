using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Pot2D : MonoBehaviour
{
    // List to track added ingredients
    private List<string> addedIngredients = new List<string>();

    // Define your recipes: key is a combination of ingredients, value is the resulting item name
    private Dictionary<HashSet<string>, string> recipes = new Dictionary<HashSet<string>, string>()
    {
       { new HashSet<string> { "Ingredient1", "Ingredient2" }, "Potion1" },   // Example recipe: Salt + Pepper = Flour
        { new HashSet<string> { "Ingredient3", "Ingredient2" }, "Potion2" },
    };

    // Define exceptions: key is a combination of ingredients that should "fail"
    private HashSet<HashSet<string>> recipeExceptions = new HashSet<HashSet<string>>()
    {
        new HashSet<string> { "Ingredient1", "Ingredient3" }     // Example exception: Ingredient1 + Ingredient3 = fail
    };

    // Dictionary to store prefabs associated with each recipe result
    public Dictionary<string, GameObject> recipePrefabs = new Dictionary<string, GameObject>();

    // Prefabs for different recipe results (assign these in the Inspector)
    public GameObject Potion1prefab;
    public GameObject Potion2prefab;
    public GameObject PepperSunprefab;

    // Flag to ensure only one recipe is created at a time
    private bool isRecipeBeingProcessed = false;

    void Start()
    {
        // Assign prefab references to each recipe result
        recipePrefabs["Potion1"] = Potion1prefab;  // Ensure flourPrefab is assigned
        recipePrefabs["Potion2"] = Potion2prefab;  // Ensure breadPrefab is assigned
        recipePrefabs["PepperSun"] = PepperSunprefab;

    }

    // Called when another collider enters the pot's trigger collider (2D)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient"))
        {
            DragIngredient2D ingredientScript = other.GetComponent<DragIngredient2D>();

            // Check if the ingredient has already been added
            if (ingredientScript != null && !ingredientScript.hasBeenAdded)
            {
                Debug.Log("Ingredient " + other.gameObject.name + " added to the pot!");
                ingredientScript.hasBeenAdded = true; // Mark the ingredient as added
                addedIngredients.Add(other.gameObject.name); // Add the ingredient's name to the list
                other.GetComponent<DragIngredient2D>().ReturnToOriginalPosition(); // Return the ingredient to its original position

                Debug.Log("Current ingredients in pot: " + string.Join(", ", addedIngredients));
                if (!isRecipeBeingProcessed)
                {
                    CheckForRecipe(); // Check if a valid recipe has been formed
                }
            }
            else
            {
                Debug.Log(other.gameObject.name + " has already been added.");
            }
        }
    }

    // Check if the added ingredients match a recipe
    void CheckForRecipe()
    {
        if (isRecipeBeingProcessed)
        {
            Debug.Log("A recipe is already being processed.");
            return;
        }

        // Check for exceptions first
        if (CheckForExceptions())
        {
            Debug.LogWarning("Invalid ingredient combination. No recipe will be created.");
            ResetIngredients(); // Clear the ingredients if the exception is triggered
            return; // Do nothing if an exception occurs
        }

        // Check for valid recipes
        foreach (var recipe in recipes)
        {
            // If the ingredients in the pot match a recipe
            if (recipe.Key.SetEquals(addedIngredients))
            {
                Debug.Log("Recipe match found: " + string.Join(", ", recipe.Key));
                isRecipeBeingProcessed = true; // Set flag to indicate a recipe is being processed
                CreateNewItem(recipe.Value, recipe.Key); // Create the resulting item and clear the used ingredients
                return;
            }
        }
        Debug.Log("No matching recipe found.");
    }

    // Check if the added ingredients match an exception (invalid combination)
    bool CheckForExceptions()
    {
        foreach (var exception in recipeExceptions)
        {
            if (exception.SetEquals(addedIngredients))
            {
                return true; // If the ingredients match an exception, return true
            }
        }
        return false; // No exceptions found
    }

    // Create the new item and clear used ingredients
    void CreateNewItem(string newItem, HashSet<string> usedIngredients)
    {
        Debug.Log("Recipe completed! Creating " + newItem);

        // Remove used ingredients from the pot
        foreach (string ingredient in usedIngredients)
        {
            if (addedIngredients.Contains(ingredient))
            {
                addedIngredients.Remove(ingredient);

                // Reset `hasBeenAdded` flag for the ingredient
                GameObject ingredientObject = GameObject.Find(ingredient);
                if (ingredientObject != null)
                {
                    DragIngredient2D ingredientScript = ingredientObject.GetComponent<DragIngredient2D>();
                    if (ingredientScript != null)
                    {
                        ingredientScript.hasBeenAdded = false;
                    }
                }
            }
        }

        // Log remaining ingredients
        Debug.Log("Ingredients remaining in pot after creating new item: " + string.Join(", ", addedIngredients));

        // Instantiate the new item (if the prefab exists for the recipe result)
        if (recipePrefabs.ContainsKey(newItem))
        {
            InstantiateNewItem(recipePrefabs[newItem]); // Instantiate the corresponding prefab
        }
        else
        {
            Debug.LogWarning("No prefab found for the item: " + newItem);
        }

        // Reset the recipe flag
        isRecipeBeingProcessed = false;
    }

    // Method to instantiate the new item prefab
    void InstantiateNewItem(GameObject itemPrefab)
    {
        // Instantiate the prefab at the pot's position or any desired location
        GameObject newItemObject = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Debug.Log("New item instantiated: " + newItemObject.name);
    }

    // Reset ingredients (optional, to reset the pot after a failure or successful recipe)
    void ResetIngredients()
    {
        Debug.Log("Resetting ingredients...");

        // Clear the list of added ingredients
        addedIngredients.Clear();

        // Find all ingredients in the scene and reset their 'hasBeenAdded' flag
        DragIngredient2D[] allIngredients = FindObjectsOfType<DragIngredient2D>();
        foreach (DragIngredient2D ingredient in allIngredients)
        {
            ingredient.hasBeenAdded = false;
        }

        Debug.Log("Ingredients have been reset in the pot.");
        isRecipeBeingProcessed = false; // Ensure the flag is reset so new combinations can be processed
    }

    // Optional: Reset method to clear the pot (if needed)
    public void ResetPot()
    {
        addedIngredients.Clear();
        Debug.Log("Pot has been reset.");
        isRecipeBeingProcessed = false;
    }
}

