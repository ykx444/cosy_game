using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionnaireItem : MonoBehaviour
{
    public TextMeshProUGUI label;  // Reference to the Text component for the label
    public Toggle[] toggles;  // Array of Toggle components

    // Set the label text
    public void SetLabel(string labelText)
    {
        label.text = labelText;
    }
    private void Start()
    {
        foreach (var item in toggles)
        {
            item.isOn = false;
            item.onValueChanged.AddListener(delegate { OnToggleChanged(item); });
        }
 
    }

    // Handle toggle change
    private void OnToggleChanged(Toggle changedToggle)
    {
        //If a toggle other than the one that was changed is found to be on, it is turned off.
        if (changedToggle.isOn)
        {
            foreach (Toggle toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false;
                }
            }
        }
        Debug.Log(GetSelectedValue());
    }

    // Get the selected value from the toggles position
    public int GetSelectedValue()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                return i;
            }
        }
        return 0;  // Default to 0 if no toggle is selected
    }

    // Check if any toggle is selected
    public bool IsAnyToggleOn()
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                return true;
            }
        }
        return false;
    }
}

