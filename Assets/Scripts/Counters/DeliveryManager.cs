using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSucces;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;


    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0 )
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (GameHandler.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = GetWeightedRandomRecipe();
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            { // Has the same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                { // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    { // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        { // Ingredient matches
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe
                    successfulRecipesAmount++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSucces?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // No matches found!
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    

    public RecipeSO GetWeightedRandomRecipe()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        RecipeSO randomRecipe = recipeListSO.recipeSOList[0];
        
        Dictionary<RecipeSO, int> adjustedWeights = new Dictionary<RecipeSO, int>();
        foreach (RecipeSO recipe in recipeListSO.recipeSOList)
        {
            int adjustedWeight = recipe.baseWeight;
            
            if (sceneName == "GameScene1")
            {
                if (recipe.recipeName == "Salad")
                {
                    adjustedWeight *= 3;
                }else if (recipe.recipeName == "Burger")
                {
                    adjustedWeight *= 2;
                }
            }
            
            if (sceneName == "GameScene2")
            {
                if (recipe.recipeName == "Burger")
                {
                    adjustedWeight *= 2;
                }else if (recipe.recipeName == "CheeseBurger")
                {
                    adjustedWeight *= 2;
                }
            }
            
            if (sceneName == "GameScene3")
            {
                if (recipe.recipeName == "CheeseBurger")
                {
                    adjustedWeight *= 2;
                }else if (recipe.recipeName == "MEGABurger")
                {
                    adjustedWeight *= 3;
                }
            }
            
            adjustedWeights[recipe] = adjustedWeight;
        }
        int totalWeight = 0;
        foreach (int w in adjustedWeights.Values)
            totalWeight += w;
            
        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;
        foreach (var recipeName in adjustedWeights)
        {
            cumulative += recipeName.Value;
            if (randomValue < cumulative)
                return recipeName.Key;
        }
        return randomRecipe;
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
