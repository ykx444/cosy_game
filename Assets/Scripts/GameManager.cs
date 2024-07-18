using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
  
    }

    public TilemapReadManager tilemapReadManager;
    public ItemManageController itemController;
    public ToolBarPanel toolbarPanel;
    public MarkerManager markerManager;
    public DayTimeController timeController;
    public RecipeManager recipeManager;
    public CustomerManager customerManager;
    public InteractionActionManager interactionActionManager;
    public CoinManager coinManager;
    public ScoreManager scoreManager;
    public DialogueManager dialogueManager;
    public TutorialManager tutorialManager;
    public AudioManager audioManager;
    public ShopUIManager shopManager;
    public UIController uIController;

    public RecipeUIManager recipeUIPanel;

    public GameObject cauldronPanelObject;

    public CauldronCraftManager cauldronManager;

    public Image resultSlot;
    public CauldronInteract currentCauldron;

    public InventoryPanel inventoryPanel;
    public Animator playerAnim;
    public GameObject player;
    public ItemContainer playerInventory;
    public GameObject dialoguePanel;
    public GameObject resultPanel;

    public ItemContainer craftPanelInvetory;

    public bool Movable = true;
}
