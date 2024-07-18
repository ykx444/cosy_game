using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] float totalCoins = 0;
    public TextMeshProUGUI coinText;

    private void Start()
    {
        UpdateCoinText();
    }
    public void AddCoins(float amount)
    {
        totalCoins += amount;
        UpdateCoinText();
    }

    public void UseCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            UpdateCoinText();
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = totalCoins.ToString();
        }
    }
}
