using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerController : MonoBehaviour
{
    public string team;
    private Color originalColor;
    private SpriteRenderer theSprite;
    public Grid theGrid;
    private LevelController theLevel;
    public enum CheckerType
    {
        Piece, King
    }
    public CheckerType checkerType = CheckerType.Piece;
    public Sprite kingSprite;
    void Start()
    {
        theLevel = FindObjectOfType<LevelController>();
        theSprite = GetComponent<SpriteRenderer>();
        originalColor = theSprite.color;
    }

    void OnMouseDown()
    {
        if ((theLevel.currentTurn == LevelController.Turn.Red && team.Equals("Red")) || (theLevel.currentTurn == LevelController.Turn.Blue && team.Equals("Blue")))
        {
            theLevel.SelectChecker(theGrid.x, theGrid.y, this);
            SwitchToSelectedColor();
        }
    }

    public void SwitchToSelectedColor()
    {
        theSprite.color = new Color(originalColor.r * 0.6f, originalColor.g * 0.6f, originalColor.b * 0.6f);
    }

    public void SwitchToUnSelectedColor()
    {
        theSprite.color = originalColor;
    }

    public void UpgradeChecker()
    {
        checkerType = CheckerType.King;
        theSprite.sprite = kingSprite;
    }
}
