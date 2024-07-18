using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Recipe")]
public class CauldronRecipe : ScriptableObject
{
    public List<Item> requiredMaterials;
    public Item output;
    public float completeCount;//how many times it has been crafted
    public float processTime;

}


