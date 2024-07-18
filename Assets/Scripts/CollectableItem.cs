using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Item item;
    public int count;

    private void Awake()
    {
        
    }

    public void SetItem(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.icon;
    }

}
