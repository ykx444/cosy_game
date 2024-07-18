using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    public Button closeButton;
    public Button openButton;

    public List<CauldronRecipe> recipes;
    public Transform parentPanel;
    public GameObject recipeItemPrefab;
    void Start()
    {
        Close();
        closeButton.onClick.AddListener(Close);
        openButton.onClick.AddListener(Open);
    }

    // Update is called once per frame
    void PopulateRecipes()
    {
        if (recipes.Count > parentPanel.childCount)
        {
            foreach (CauldronRecipe recipe in recipes)
            {
                GameObject recipeUIItem = Instantiate(recipeItemPrefab, parentPanel);
                RecipeItemUI recipeUIItemUI = recipeUIItem.GetComponent<RecipeItemUI>();

                recipeUIItemUI.SetRecipe(recipe);
            }
        }

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Open()
    {
        gameObject.SetActive(true);
        PopulateRecipes();
    }
}
