public class CraftPanel : ToolBarPanel
{
    public override void OnClick(int id, string name)
    {
        ItemContainer inventory = GameManager.instance.currentCauldron.cauldronInventory;
        GameManager.instance.itemController.OnClick(inventory.slots[id], name);
        //update ui
        GameManager.instance.toolbarPanel.Show();
        GameManager.instance.inventoryPanel.Show();

       

        Show();
    }
    private void OnEnable()
    {
        Show();
        if(GameManager.instance.cauldronPanelObject.activeInHierarchy)
        GameManager.instance.cauldronManager.CheckCurrentRecipe(GameManager.instance.currentCauldron);
    }
    public override void Show()
    {
        //set button if it has item, else clean it
        for (int i = 0; i < GameManager.instance.currentCauldron.cauldronInventory.slots.Count && i < buttons.Count; i++)
        {
            if (GameManager.instance.currentCauldron.cauldronInventory.slots[i].item == null)
            {
                buttons[i].Clean();
            }
            else
            {
                buttons[i].Set(GameManager.instance.currentCauldron.cauldronInventory.slots[i]);
            }
        }
    }
}

