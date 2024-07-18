using UnityEngine;
using UnityEngine.UI;

public class RecipeItemUI : MonoBehaviour
{
    public Image targetItemImage;
    public Transform ingredientGrid;

    public GameObject ingredientPrefab;
    public void SetRecipe(CauldronRecipe recipe)
    {
        targetItemImage.sprite = recipe.output.icon;
   
        foreach (Item item in recipe.requiredMaterials)
        {
            GameObject GO = Instantiate(ingredientPrefab, ingredientGrid);
            GO.GetComponent<Image>().sprite = item.icon;
        }
    }
}
