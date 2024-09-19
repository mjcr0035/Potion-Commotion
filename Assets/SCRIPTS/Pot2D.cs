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
        { new HashSet<string> { "Salt", "Pepper" }, "Flour" },   // Example recipe: Salt + Pepper = Flour
        { new HashSet<string> { "Salt", "Sun" }, "SaltSun" },    // Example recipe: Water + Flour = Bread
        { new HashSet<string> { "Pepper", "Sun" }, "PepperSun" }
    };

    // Dictionary to store prefabs associated with each recipe result
    public Dictionary<string, GameObject> recipePrefabs = new Dictionary<string, GameObject>();

    // Flag to ensure only one recipe is created at a time
    private bool isRecipeBeingProcessed = false;

    void Start()
    {
        // Manually add the prefabs to the dictionary or set them up in the Unity Inspector
        // You could also expose the prefab assignment in the Inspector as public fields

        // Assign prefab references to each recipe result
        // Example: Replace these with the correct prefabs you've created
        recipePrefabs["Flour"] = flourPrefab;  // Ensure flourPrefab is assigned
        recipePrefabs["SaltSun"] = SaltSun;  // Ensure breadPrefab is assigned
        recipePrefabs["PepperSun"] = PepperSun;
    }

    // Prefabs for different recipe results (assign these in the Inspector)
    public GameObject flourPrefab;
    public GameObject SaltSun;
    public GameObject PepperSun;

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

    // Optional: Reset method to clear the pot (if needed)
    public void ResetPot()
    {
        addedIngredients.Clear();
        Debug.Log("Pot has been reset.");
    }
}
