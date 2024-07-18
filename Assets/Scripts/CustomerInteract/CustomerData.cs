using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Data/CustomerData")]
public class CustomerData : ScriptableObject
{
    //static customer data
    //public Item orderItem;
    public float maxPatience = 100f;
    //public float currentPatience;
    public GameObject customerPrefab;
    public List<Sprite> moods;
}
