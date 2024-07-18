using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarController : MonoBehaviour
{
    [SerializeField] int toolBarSize = 8;
    int selectedToolID;

    public Action<int> onChange;

    internal void Set(int id)
    {
        selectedToolID = id;
    }

    public Item GetItem { get { return GameManager.instance.playerInventory.slots[selectedToolID].item; } }
    // Update is called once per frame
    void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            //get the mouse scroll to select tool being used
            //by using mouse scroll data (whether is more than 0)
            //-1 towards left on toolbar vice versa
            if (delta > 0)
            {
                selectedToolID += 1;
                //if exceeds bar size, stays
                selectedToolID = (selectedToolID >= toolBarSize ? 0 : selectedToolID);
            }
            else
            {
                selectedToolID -= 1;
                //if go beyond 0, move to end of toolbar
                selectedToolID = (selectedToolID < 0 ? toolBarSize - 1 : selectedToolID);
            }
            onChange?.Invoke(selectedToolID);
        }
    }
}
