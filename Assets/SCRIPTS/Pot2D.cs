using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot2D : MonoBehaviour
{
    private List<string> addedIngredients = new List<string>();

    private Dictionary<HashSet<string>, string> recipes = new Dictionary<HashSet<string>, string>()
    {
        { new HashSet<string> { "Ingredient1", "Ingredient2" }, "Potion1" },
        { new HashSet<string> { "Ingredient1", "Ingredient3" }, "Potion2" },
        { new HashSet<string> { "Ingredient1", "Ingredient4" }, "Potion3" },
        { new HashSet<string> { "Ingredient1", "Ingredient5" }, "Potion4" },
        { new HashSet<string> { "Ingredient2", "Ingredient3" }, "Potion5" },
        { new HashSet<string> { "Ingredient2", "Ingredient4" }, "Potion6" },
        { new HashSet<string> { "Ingredient2", "Ingredient5" }, "Potion7" },
        { new HashSet<string> { "Ingredient3", "Ingredient4" }, "Potion8" },
        { new HashSet<string> { "Ingredient3", "Ingredient5" }, "Potion9" },
        { new HashSet<string> { "Ingredient4", "Ingredient5" }, "Potion10" },
    };

    private HashSet<HashSet<string>> recipeExceptions = new HashSet<HashSet<string>>()
    {
      //new HashSet<string> { "Ingredient1", "Ingredient4" },
      //new HashSet<string> { "Ingredient2", "Ingredient4" },

    };

    public Dictionary<string, GameObject> recipePrefabs = new Dictionary<string, GameObject>();
    public GameObject Potion1;
    public GameObject Potion2;
    public GameObject Potion3;
    public GameObject Potion4;
    public GameObject Potion5;
    public GameObject Potion6;
    public GameObject Potion7;
    public GameObject Potion8;
    public GameObject Potion9;
    public GameObject Potion10;

    public GameObject SwirlingReady;

    private bool isRecipeBeingProcessed = false;

    private bool isSwirling = false;
    private int swirlCount = 0;
    private Vector2 lastMousePosition;
    private bool inFirstQuadrant = false;


    void Start()
    {
        recipePrefabs["Potion1"] = Potion1;
        recipePrefabs["Potion2"] = Potion2;
        recipePrefabs["Potion3"] = Potion3;
        recipePrefabs["Potion4"] = Potion4;
        recipePrefabs["Potion5"] = Potion5;
        recipePrefabs["Potion6"] = Potion6;
        recipePrefabs["Potion7"] = Potion7;
        recipePrefabs["Potion8"] = Potion8;
        recipePrefabs["Potion9"] = Potion9;
        recipePrefabs["Potion10"] = Potion10;

        
    }

    void Update()
    {
        DetectSwirl();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ingredient"))
        {
            DragIngredient2D ingredientScript = other.GetComponent<DragIngredient2D>();

            if (ingredientScript != null && !ingredientScript.hasBeenAdded)
            {
                Debug.Log("Ingredient " + other.gameObject.name + " added to the pot!");
                

                ingredientScript.hasBeenAdded = true;
                addedIngredients.Add(other.gameObject.name);
                other.GetComponent<DragIngredient2D>().ReturnToOriginalPosition();

                Debug.Log("Current ingredients in pot: " + string.Join(", ", addedIngredients));

                if (!isRecipeBeingProcessed)
                {
                    CheckForRecipe();
                }

            }
            else
            {
                Debug.Log(other.gameObject.name + " has already been added.");
            }
        }
    }

    void CheckForRecipe()
    {
        if (isRecipeBeingProcessed)
        {
            Debug.Log("A recipe is already being processed.");
            return;
        }

        if (CheckForExceptions())
        {
            Debug.LogWarning("Invalid ingredient combination. No recipe will be created.");
            ResetIngredients();
            return;
        }

        foreach (var recipe in recipes)
        {
            if (recipe.Key.SetEquals(addedIngredients))
            {
                Debug.Log("Recipe match found: " + string.Join(", ", recipe.Key));
                SwirlingReady.SetActive(true);

                isSwirling = true;  // Now you need to swirl to create the potion
                isRecipeBeingProcessed = true;
                //CreateNewItem(recipe.Value);
                return;
            }
        }
        
        Debug.Log("No matching recipe found.");
        
    }

    bool CheckForExceptions()
    {
        foreach (var exception in recipeExceptions)
        {
            if (exception.SetEquals(addedIngredients))
            {
                return true;
            }
        }
        return false;
    }

    void CreateNewItem(string newItem)
    {
        Debug.Log("Creating " + newItem);

        addedIngredients.Clear();
        DragIngredient2D[] allIngredients = FindObjectsOfType<DragIngredient2D>();

        foreach (DragIngredient2D ingredient in allIngredients)
        {
            ingredient.hasBeenAdded = false;
        }

        if (recipePrefabs.ContainsKey(newItem))
        {
            Instantiate(recipePrefabs[newItem], transform.position, Quaternion.identity);
            
        }
        else
        {
            Debug.LogWarning("No prefab found for the item: " + newItem);
        }

        isRecipeBeingProcessed = false;
        isSwirling = false;
        swirlCount = 0;
        Debug.Log("Ingredients have been reset after creating new item.");
    }

    void DetectSwirl()
    {
        if (Input.GetMouseButton(0) && isSwirling)
        {
            // Get the current mouse position in world coordinates
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Get the collider bounds of the pot and log them
            Collider2D potCollider = GetComponent<Collider2D>();
            Bounds potBounds = potCollider.bounds;

            Debug.Log($"Mouse Position: {currentMousePosition}, Pot Bounds: {potBounds}");

            // Check if the mouse position is within the pot's collider bounds
            if (potBounds.Contains(currentMousePosition))
            {
                Debug.Log("Mouse is inside the pot collider.");

                // Calculate the swirl angle using the pot's position as reference
                Vector2 relativePosition = currentMousePosition - (Vector2)transform.position;
                float angle = Vector2.SignedAngle(lastMousePosition - (Vector2)transform.position, relativePosition);

                if (angle > 20 && angle < 160 && !inFirstQuadrant)
                {
                    inFirstQuadrant = true;
                }
                else if (angle > -160 && angle < -20 && inFirstQuadrant)
                {
                    swirlCount++;
                    inFirstQuadrant = false;
                    Debug.Log("Swirl detected! Total swirls: " + swirlCount);

                    if (swirlCount >= 2)
                    {
                        foreach (var recipe in recipes)
                        {
                            if (recipe.Key.SetEquals(addedIngredients))
                            {
                                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Potion"))
                                {
                                    Destroy(obj); // Destroy any existing potions in the scene
                                }
                                
                                CreateNewItem(recipe.Value);
                                SwirlingReady.SetActive(false);
                                return;
                            }
                        }
                    }
                }
                // Update last position
                lastMousePosition = currentMousePosition;
            }
            else
            {
                Debug.Log("Mouse is outside of the pot collider.");
            }
        }
    }


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


    public void ResetPot()
    {
        addedIngredients.Clear();
        Debug.Log("Pot has been reset.");
        isRecipeBeingProcessed = false;
        isSwirling = false;
        swirlCount = 0;
    }

    
    
}

