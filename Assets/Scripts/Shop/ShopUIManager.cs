using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public Button closeButton;
    public List<Item> shopItems;
    public Transform parentPanel;
    public GameObject shopItemPrefab;

    public GameObject shopPanel;
    private void Start()
    {
        Close();
        closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        shopPanel.SetActive(false);
    }
    void PopulateItems()
    {
        if (shopItems.Count > parentPanel.childCount)
        {
            foreach (Item item in shopItems)
            {
                ShopItemUI currentItem = Instantiate(shopItemPrefab, parentPanel).GetComponent<ShopItemUI>();
                currentItem.SetShopItem(item);
            }
        }
    }
    public void Open()
    {
        shopPanel.SetActive(true);
        PopulateItems();
    }
}
