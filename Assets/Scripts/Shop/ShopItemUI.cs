using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private Item currentItem;

    private void Start()
    {
        buyButton.onClick.AddListener(BuyItem);
    }
    public void SetShopItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item.icon;
        nameText.text = item.itemName;
        priceText.text = item.price.ToString();
    }

    public void BuyItem()
    {
        GameManager.instance.coinManager.UseCoins(currentItem.price);
        SpawnDroppedItemManager.instance.SpawnItem(GameManager.instance.player.transform.position, currentItem, 1);

    }
}
