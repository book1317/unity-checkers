using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public Grid theGrid;
    private LevelController theLevel;
    public enum TableType
    {
        PlayAble, UnPlayAble
    }
    public TableType tableType = TableType.UnPlayAble;
    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
    }


    void OnMouseDown()
    {
        if (tableType == TableType.PlayAble)
            theLevel.SelectTable(theGrid.x, theGrid.y);
        else
            theLevel.UnSelectChecker();
    }

    void PrintDetail()
    {
        if (theGrid.theChecker)
            Debug.Log("Table:" + theGrid.x + ":" + theGrid.y + ":" + theGrid.theChecker.team);
        else
            Debug.Log("Table:" + theGrid.x + ":" + theGrid.y + ":" + "Null");
    }
}
