using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    const int GRID_WIDTH = 4;
    const int GRID_HEIGHT = 8;
    const float START_X_ODD = -3.8f;
    const float START_X_EVEN = -5.3f;
    const float START_Y = -3.5f;
    const float X_LENGHT = 3f;
    const float Y_LENGHT = 1f;
    Grid[,] allGrid;
    public GameObject CheckerRedPrefabs;
    public GameObject CheckerBluePrefabs;
    public List<TableController> allTable;
    public enum MatchState
    {
        UnSelected, Selected
    }
    public MatchState matchState = MatchState.UnSelected;
    public enum Turn
    {
        Red, Blue
    }
    public Turn currentTurn = Turn.Red;
    public CheckerController currentChecker;
    public Text turnText;
    public List<CheckerController> allChecker;

    void Start()
    {
        initChecker();
    }

    void initChecker()
    {
        allGrid = new Grid[GRID_WIDTH, GRID_HEIGHT];
        for (int y = 0, t = 0; y < GRID_HEIGHT; y++)
            for (int x = 0; x < GRID_WIDTH; x++, t++)
            {
                if (y == 0 || y == 1)
                {
                    CheckerController theChecker;
                    if (y % 2 == 0)
                        theChecker = Instantiate(CheckerRedPrefabs, new Vector3(START_X_EVEN + X_LENGHT * x, START_Y + Y_LENGHT * y, -2), Quaternion.identity).GetComponent<CheckerController>();
                    else
                        theChecker = Instantiate(CheckerRedPrefabs, new Vector3(START_X_ODD + X_LENGHT * x, START_Y + Y_LENGHT * y, -2), Quaternion.identity).GetComponent<CheckerController>();
                    allGrid[x, y] = new Grid(x, y, theChecker);
                    theChecker.theGrid = allGrid[x, y];
                    allChecker.Add(theChecker);
                }
                else if (y == GRID_HEIGHT - 1 || y == GRID_HEIGHT - 2)
                {
                    CheckerController theChecker;
                    if (y % 2 == 1)
                        theChecker = Instantiate(CheckerBluePrefabs, new Vector3(START_X_ODD + X_LENGHT * x, START_Y + Y_LENGHT * y, -2), Quaternion.identity).GetComponent<CheckerController>();
                    else
                        theChecker = Instantiate(CheckerBluePrefabs, new Vector3(START_X_EVEN + X_LENGHT * x, START_Y + Y_LENGHT * y, -2), Quaternion.identity).GetComponent<CheckerController>();
                    allGrid[x, y] = new Grid(x, y, theChecker);
                    theChecker.theGrid = allGrid[x, y];
                    allChecker.Add(theChecker);
                }
                else
                {
                    allGrid[x, y] = new Grid(x, y, null);
                }
                allTable[t].theGrid = allGrid[x, y];
            }
    }

    void ShowGrid()
    {
        for (int y = 0; y < GRID_HEIGHT; y++)
            for (int x = 0; x < GRID_WIDTH; x++)
                Debug.Log(x + "," + y + ":" + allGrid[x, y].theChecker.team);
    }

    public void SelectChecker(int x, int y, CheckerController theChecker)
    {
        if (matchState == MatchState.UnSelected)
        {
            currentChecker = theChecker;
            matchState = MatchState.Selected;
        }
        else
        {
            UnSelectChecker();
            currentChecker = theChecker;
            matchState = MatchState.Selected;
        }
    }

    public void UnSelectChecker()
    {
        if (currentChecker)
            currentChecker.SwitchToUnSelectedColor();
        currentChecker = null;
    }

    public void SelectTable(int x, int y)
    {
        if (matchState == MatchState.Selected)
        {
            CheckMove(x, y);
            UnSelectChecker();
            matchState = MatchState.UnSelected;
        }
    }

    void CheckMove(int moveToX, int moveToY)
    {
        int currentX = currentChecker.theGrid.x;
        int currentY = currentChecker.theGrid.y;

        Debug.Log(currentX + ":" + moveToX);
        if (!allGrid[moveToX, moveToY].theChecker)
        {
            if (currentChecker.checkerType == CheckerController.CheckerType.Piece)
            {
                if (Mathf.Abs(currentX - moveToX) <= 1 && Mathf.Abs(currentY - moveToY) <= 1)
                {
                    if (currentChecker.team.Equals("Red") && moveToY - currentY > 0)
                        MoveChecker(moveToX, moveToY);
                    else if (currentChecker.team.Equals("Blue") && moveToY - currentY < 0)
                        MoveChecker(moveToX, moveToY);
                }
                else if (Mathf.Abs(currentX - moveToX) == 1 && Mathf.Abs(currentY - moveToY) == 2)
                {
                    if (currentChecker.team.Equals("Red"))
                    {
                        if (currentY % 2 == 0)
                        {
                            if (moveToX > currentX)
                            {
                                if (currentChecker.team.Equals("Red") && allGrid[currentX, moveToY - 1].theChecker.team.Equals("Blue"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(currentX, moveToY - 1);
                                }
                            }
                            else
                            {
                                if (currentChecker.team.Equals("Red") && allGrid[moveToX, moveToY - 1].theChecker.team.Equals("Blue"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX, moveToY - 1);
                                }
                            }
                        }
                        else
                        {
                            if (moveToX > currentX)
                            {
                                if (currentChecker.team.Equals("Red") && allGrid[moveToX, moveToY - 1].theChecker.team.Equals("Blue"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX, moveToY - 1);
                                }
                            }
                            else
                            {
                                if (currentChecker.team.Equals("Red") && allGrid[moveToX + 1, moveToY - 1].theChecker.team.Equals("Blue"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX + 1, moveToY - 1);
                                }
                            }
                        }
                    }
                    else if (currentChecker.team.Equals("Blue"))
                    {
                        if (currentY % 2 == 0)
                        {
                            if (moveToX > currentX)
                            {
                                if (currentChecker.team.Equals("Blue") && allGrid[currentX, moveToY + 1].theChecker.team.Equals("Red"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(currentX, moveToY + 1);
                                }
                            }
                            else
                            {
                                if (currentChecker.team.Equals("Blue") && allGrid[moveToX, moveToY + 1].theChecker.team.Equals("Red"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX, moveToY + 1);
                                }
                            }
                        }
                        else
                        {
                            if (moveToX > currentX)
                            {
                                if (currentChecker.team.Equals("Blue") && allGrid[moveToX, moveToY + 1].theChecker.team.Equals("Red"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX, moveToY + 1);
                                }
                            }
                            else
                            {
                                if (currentChecker.team.Equals("Blue") && allGrid[moveToX + 1, moveToY + 1].theChecker.team.Equals("Red"))
                                {
                                    MoveChecker(moveToX, moveToY);
                                    EatChecker(moveToX + 1, moveToY + 1);
                                }
                            }
                        }
                    }
                }
            }
            else if (currentChecker.checkerType == CheckerController.CheckerType.King)
            {
                MoveChecker(moveToX, moveToY);
            }
        }
    }

    void MoveChecker(int x, int y)
    {
        if (y % 2 == 0)
            currentChecker.transform.position = new Vector3(START_X_EVEN + X_LENGHT * x, START_Y + Y_LENGHT * y, -2);
        else
            currentChecker.transform.position = new Vector3(START_X_ODD + X_LENGHT * x, START_Y + Y_LENGHT * y, -2);

        currentChecker.theGrid.theChecker = null;
        allGrid[x, y].theChecker = currentChecker;
        currentChecker.theGrid = allGrid[x, y];

        if ((currentChecker.team.Equals("Red") && y == GRID_HEIGHT - 1) || (currentChecker.team.Equals("Blue") && y == 0))
            currentChecker.UpgradeChecker();

        SwitchTurn();
    }

    void SwitchTurn()
    {
        if (currentTurn == Turn.Red)
        {
            currentTurn = Turn.Blue;
            UpdateText("Blue");
        }
        else
        {
            currentTurn = Turn.Red;
            UpdateText("Red");
        }
    }

    void UpdateText(string team)
    {
        switch (team)
        {
            case "Red":
                turnText.text = "Red";
                turnText.color = new Color(255f / 255f, 83f / 255f, 83f / 255f);
                break;
            case "Blue":
                turnText.text = "Blue";
                turnText.color = new Color(59f / 255f, 116f / 255f, 1);
                break;
        }
    }

    void EatChecker(int x, int y)
    {
        allChecker.Remove(allGrid[x, y].theChecker);
        Destroy(allGrid[x, y].theChecker.gameObject);
        allGrid[x, y].theChecker = null;
    }

    public void Reset()
    {
        for (int i = 0; i < allChecker.Count; i++)
            Destroy(allChecker[i].gameObject);
        allChecker = new List<CheckerController>();
        initChecker();
        currentTurn = Turn.Red;
        UpdateText("Red");
    }
}
