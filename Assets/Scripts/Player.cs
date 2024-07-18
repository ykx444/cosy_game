using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // user actions
    //**Action1: pick up item when near
    //to interact with objects with colliders, we use trigger collider
    //**Action2: to plow and seed a tile
    //to modify tiles, we use UseToolGrid() method
    CollectableItem pickItem;

    [SerializeField] MarkerManager markerManager;
    [SerializeField] TilemapReadManager tileManager;

    [SerializeField] float maxDistance = 1.5f;


    Vector3Int selectedTile;
    //distance between character and mouse position must within this range to perform action
    bool selectable;
    //interact with object or people
    public GameObject interactObject;

    //get current selected tool
    ToolBarController toolbar;
    //Animator anim;

    [SerializeField] ToolAction onCropTileHarvest;

    Customer customer;

    private void Awake()
    {
        toolbar = GetComponent<ToolBarController>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Mark();
        CheckDistance();
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
                UseToolGrid();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // interact with NEARBY object, npc, or animal
            if (interactObject != null)
            {
                GameManager.instance.currentCauldron = interactObject.GetComponent<CauldronInteract>();
                //show ui
                interactObject.GetComponent<Interactable>().Interact();
                //if (interactObject.name == "pot")
                //{
                //    //clear ui
                //    GameManager.instance.cauldronManager.ClearCauldron(interactObject.GetComponent<CauldronInteract>());
                //}
               
            }
        }
    }

    private void CheckDistance()
    {
        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
        selectedTile = markerManager.markedCellPos;
    }
    private void Mark()
    {
        Vector3Int gridPos = tileManager.GetGridPosition(Input.mousePosition, true);
        markerManager.markedCellPos = gridPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PickUp":
                pickItem = collision.gameObject.GetComponent<CollectableItem>();
                if (GameManager.instance.playerInventory != null)
                {
                    GameManager.instance.playerInventory.itemProcessing = pickItem;
                    bool temp = GameManager.instance.playerInventory.Add(pickItem.item, pickItem.count);
                    //update inventory
                    GameManager.instance.toolbarPanel.Show();
                    GameManager.instance.inventoryPanel.Show();
                    //destroy object if moved into inventory
                    GameManager.instance.audioManager.PlaySoundEffect("collect");
                    if (temp) Destroy(collision.gameObject);
                }
                else
                {
                    pickItem.count = GameManager.instance.playerInventory.itemProcessing.count;
                }
                break;
            case "Teleport":
                collision.gameObject.transform.parent.GetComponent<Transition>().InitialiseTransition(transform);
                break;
            case "Interactable":
                interactObject = collision.gameObject;
                break;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactObject = null;
    }


    private void UseToolGrid()
    {
        Item item = toolbar.GetItem;

        // Check if an item is equipped and it has an on-tilemap action
        if (item != null)
        {
            if (item.onTilemapAction != null && selectable)
            {
                bool completeAction = item.onTilemapAction.OnApplyToTileMap(Input.mousePosition, selectedTile, tileManager, item);
                if (completeAction)
                {
                    item.onTilemapAction.OnItemUsed(item, GameManager.instance.playerInventory);
                    //update UI
                    GameManager.instance.toolbarPanel.Show();
                    GameManager.instance.inventoryPanel.Show();
                }
            }
            // Check if the item is an ingredient
            else if (item.itemType == ItemType.ingredient)
            {
                // Check if interacting with a customer
                if (IsTappingCustomer())
                {
                    GameManager.instance.customerManager.HandItemToCustomer(item, customer);
                }
            }
        }

        // No item is equipped, check if harvesting
        PickUpTile();

    }


    bool IsTappingCustomer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Customer"))
            {
                customer = hit.collider.gameObject.GetComponent<Customer>();
                return true;
            }
        }

        return false;
    }


    public void PickUpTile()
    {
        if (onCropTileHarvest == null)
        {
            return;
        }
        onCropTileHarvest.OnApplyToTileMap(Input.mousePosition, selectedTile, tileManager, null);
    }

    // Helper method to check if the pointer is over a UI element
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
