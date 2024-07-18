using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //dyanmic customer data
    private Item orderItem;
    private float currentPatience;

    [SerializeField] CustomerData data;

    public CustomerData CustomerData
    {
        get
        {
            return data;
        }
    }
    private Sprite mood;
    public Item OrderItem
    {
        get
        {
            return orderItem;
        }
    }
    public MoodBar GetMoodBar
    {
        get
        {
            return bar;
        }
    }
    public Vector3 targetPosition;
    public Vector3 leavePosition;

    //update condition control
    public bool isOrdering = false;
    public bool isLeaving = false;

    //paitience UI controlled by barUI class
    MoodBar bar;


    private void Start()
    {
        bar = GetComponent<MoodBar>();
        bar.Show(false);

    }
    public void Initialise(Item orderItem, Vector3 targetPosition)
    {
        this.orderItem = orderItem;
        currentPatience = data.maxPatience;
        //bar.MaxValue = maxPatience;
        this.targetPosition = targetPosition;
    }

    public void WalkToPoint(Vector3 target)
    {
        gameObject.GetComponent<Animator>().SetBool("iswalk", true);
        if (CheckDistance(target))
            transform.position = Vector3.MoveTowards(transform.position, target, 3 * Time.deltaTime);
        else
        {
            isOrdering = true;
            bar.MaxValue = data.maxPatience;
            bar.Show(true);
            gameObject.GetComponent<Animator>().SetBool("iswalk", false);
        }  //only start ordering when reached
    }
    public void LeaveToPoint(Vector3 target)
    {
        if (CheckDistance(target))
        {
            gameObject.GetComponent<Animator>().SetBool("iswalk", true);
            transform.position = Vector3.MoveTowards(transform.position, target, 3 * Time.deltaTime);
        }
        else Destroy(gameObject);
    }
    private bool CheckDistance(Vector3 target)
    {
        return (Vector3.Distance(transform.position, target) > 0.1f);
    }

    private void Update()
    {
        if (!isLeaving)
        {
            WalkToPoint(targetPosition);
         
        }
        if (isOrdering)
        {
            bar.itemImage.sprite = orderItem.icon;
            currentPatience -= Time.deltaTime;
            bar.CurrentValue = currentPatience;
            if (LevelManager.instance.LevelNum == 1) //dont update the bar
            {
                bar.UpdateBar();
                if (bar.CurrentValue <= 0 && LevelManager.instance.LevelNum == 1)
                {
                    GameManager.instance.customerManager.CustomerLeave(gameObject.GetComponent<Customer>());
                }
            }
        }

        else if (isLeaving)
        {
            LeaveToPoint(leavePosition);
          
        }
    }

    public void ShowMood()
    {
        // Hide the item to show mood
        bar.Show(false);

        // Define the thresholds for the moods
        float[] thresholds = { 0.1f, 0.5f, 0.6f, 0.7f, 0.8f };

        // Loop through the thresholds to find the correct mood
        for (int i = thresholds.Length - 1; i >= 0; i--)
        {
            if (bar.CurrentValue < thresholds[i])
            {
                mood = data.moods[i];
            }
        }

        // Display the mood
        bar.ShowMood(mood);
    }

}