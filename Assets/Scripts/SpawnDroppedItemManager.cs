using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDroppedItemManager : MonoBehaviour
{
    public static SpawnDroppedItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] GameObject pickUpItemPrefab;
    public void SpawnItem(Vector3 position, Item item, int count, float probability)
    {
        if (Random.value < probability)
        {
            GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
            o.SetActive(true);
            o.GetComponent<CollectableItem>().SetItem(item, count);
        }
    }

    public void SpawnItem(Vector3 position, Item item, int count)
    {
        GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        o.SetActive(true);
        o.GetComponent<CollectableItem>().SetItem(item, count);
    }
}
