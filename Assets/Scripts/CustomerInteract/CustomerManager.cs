using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    public List<CustomerData> customerData = new();
    public List<CauldronRecipe> recipeList = new();
    public List<Customer> customers = new();

    private Queue<Vector3> availableTargetPosition;

    //they will spawn and then pick 1 of 3 slots to move towards
    public Vector3[] targetPoints;
    public Vector3[] spawnPoint;

    private int maxCustomers = 4;
    private float minSpawnTime = 5f;
    private float maxSpawnTime = 7f;

    public bool isOperating = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        //starts queue of available spots (so no overlapping)
        availableTargetPosition = new Queue<Vector3>(targetPoints);
        if (LevelManager.instance.LevelNum == 0)
        {
            maxCustomers = 2;
           
        }
    }

    public void StartOperation()
    {
        if (LevelManager.instance.LevelNum == 1)
            GameManager.instance.audioManager.PlayBackgroundMusic(GameManager.instance.audioManager.backgroundMusicClips[3]);
        //reset score
        GameManager.instance.scoreManager.Reset();
        isOperating = true;

        // Start spawning customers
        StartCoroutine(SpawnCustomerCoroutine());
    }

    private void Update()
    {
        //if exceeds operation time; close the shop automatically
        if (GameManager.instance.timeController.Hours > 17f)
        {
            CloseOperation();
        }
        //it has to be operation time to reopen
    }

    public void CloseOperation()
    {
        isOperating = false;
        GameManager.instance.scoreManager.ShowResult();
        if (customers.Count > 0)
        {
            foreach (Customer customer in customers)
            {
                CustomerLeave(customer);
            }
        }
    }

    private IEnumerator SpawnCustomerCoroutine()
    {
        Debug.Log(maxCustomers);
        Debug.Log(customers.Count);
        while (isOperating)
        {
            if (customers.Count < maxCustomers)
            {
                SpawnCustomer();
            }

            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void SpawnCustomer()
    {

        // Random customer, random order
        int customerIndex;
        bool customerExists;

        do
        {
            customerIndex = Random.Range(0, customerData.Count);
            customerExists = customers.Exists(c => c.CustomerData.customerPrefab == customerData[customerIndex].customerPrefab);
        }
        while (customerExists);

        int recipeIndex = Random.Range(0, recipeList.Count);

        Vector3 targetPosition = availableTargetPosition.Dequeue(); // take a free spot

        GameObject customerObject = Instantiate(customerData[customerIndex].customerPrefab,
                                                 spawnPoint[0], Quaternion.identity);

        Customer customer = customerObject.GetComponent<Customer>();
        customer.Initialise(recipeList[recipeIndex].output, targetPosition);
        customer.CustomerData.customerPrefab = customerData[customerIndex].customerPrefab; // Assuming you have a field to store prefab

        customers.Add(customer);
    }

    public void CustomerLeave(Customer customer)
    {
        customer.isOrdering = false;
        customer.ShowMood();
        customers.Remove(customer);

        customer.isLeaving = true;
        customer.leavePosition = spawnPoint[1];

        availableTargetPosition.Enqueue(customer.targetPosition);
        //if still has time left before day ends?
        if (isOperating)
            StartCoroutine(SpawnCustomerCoroutine());
    }

    public void HandItemToCustomer(Item item, Customer customer)
    {
        if (item == customer.OrderItem)
        {
            //REMOVE ITEM FROM inventory
            GameManager.instance.playerInventory.RemoveItem(item);
            //add points based on mood level
            GameManager.instance.coinManager.AddCoins(CalculateEarn(item, customer));
            //record points earned
            GameManager.instance.scoreManager.coinEarnedToday += item.price;
            GameManager.instance.scoreManager.customerServedToday += 1;
            //customer leaf
            customer.ShowMood();
            CustomerLeave(customer);
        }
    }

    private float CalculateEarn(Item item, Customer customer)
    {
        if (customer.GetMoodBar.CurrentValue < 0.5)
        {
            return (float)(item.price - item.price * 0.25f);
        }
        if (customer.GetMoodBar.CurrentValue < 0.1)
        {
            return item.price / 2;
        }
        else return item.price;
    }
}
