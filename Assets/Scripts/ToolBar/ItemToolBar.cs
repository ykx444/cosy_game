using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemToolBar : ToolBarPanel
{
    [SerializeField] ToolBarController controller;
    private void Start()
    {
        Init();
        controller.onChange += Highlight;
        Highlight(0);
    }
    public override void OnClick(int id)
    {
        //when click on item on toolbar,
        //highlight it and select it
        controller.Set(id);
        Highlight(id);
    }


    int currentSelectedTool;

    public void Highlight(int id)
    {
        //first de-highlighted the previous tool
        buttons[currentSelectedTool].Highlight(false);
        //highlight new tool
        currentSelectedTool = id;
        buttons[currentSelectedTool].Highlight(true);
    }
}
