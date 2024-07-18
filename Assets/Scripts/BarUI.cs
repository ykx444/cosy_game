using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    [SerializeField] private float maxValue;//equals total process time/or max satis affinity level
    [SerializeField] private float currentValue;

    public GameObject barObject;

    public Image progressForeBar;
    public float MaxValue
    {
        set
        {
            maxValue = value;
        }
        get
        {
            return maxValue;
        }

    }
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            currentValue = value;
        }
    }

    public virtual void UpdateBar()
    {
        float fillAmount = currentValue / maxValue;
        progressForeBar.fillAmount = fillAmount;
    }

   public void Show(bool show)
    {
        barObject.SetActive(show);
    }
}

