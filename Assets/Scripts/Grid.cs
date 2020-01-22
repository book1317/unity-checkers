using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int x, y;
    public CheckerController theChecker = null;
    public Grid(int x, int y, CheckerController theChecker)
    {
        this.x = x;
        this.y = y;
        this.theChecker = theChecker;
    }
}
