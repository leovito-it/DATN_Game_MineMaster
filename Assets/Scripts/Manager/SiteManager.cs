using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool Checked = false;
}

public class SiteManager : MonoBehaviour
{
    public static SiteManager Instance;
    public Transform site;
    public int numCol = 10;
    public int Count => site.childCount;

    public List<Cell> cells = new List<Cell>();

    protected void Awake()
    {
        Instance = this;

        for (int i = 0; i < Count; i++)
        {
            Cell cell = GetAtIndex(i).gameObject.AddComponent<Cell>();
            cells.Add(cell);
        }
    }

    public Transform GetAtIndex(int row, int col)
    {
        int index = row * col + col;
        return index < Count && row >=0 && col >= 0 ? site.GetChild(row * col + col) : null ;
    }

    public Transform GetAtIndex(int index)
    {
        return index < Count && index >= 0 ? site.GetChild(index) : null;
    }

    public int GetRow(int index)
    {
        return index / numCol;
    }

    public int GetCol(int index)
    {
        return index % numCol;
    }

    public List<int> GetUDLR(int index)
    {
        List<int> result = new List<int>();

        int row = GetRow(index);
        int col = GetCol(index);

        // left
        if (col - 1 >= 0)
            result.Add(index - 1);
        // right
        if (col + 1 < numCol)
            result.Add(index + 1);
        // up
        if (row - 1 >= 0)
            result.Add(index - numCol);

        // down
        if (row + 1 < Count / numCol)
            result.Add(index + numCol);

        return result;
    }

    public List<int> GetAround(int index)
    {
        List<int> result = new List<int>();

        int row = GetRow(index);
        int col = GetCol(index);

        bool up, down, left, right;
        up = down = left = right = false;

        // left
        if (col-1>=0)
        {
            left = true;
            result.Add(index - 1);
        }
        // right
        if (col+1 <numCol)
        {
            right = true;
            result.Add(index + 1);
        }
        // up
        if (row -1 >= 0)
        {
            up = true;
            result.Add(index - numCol);
        }
        // down
        if (row + 1 <Count/numCol)
        {
            down = true;
            result.Add(index + numCol);
        }
        // up-left
        if (up && left)
        {
            result.Add(index - numCol - 1);
        }
        // up-right
        if (up && right)
        {
            result.Add(index - numCol + 1);
        }
        // up-left
        if (down && left)
        {
            result.Add(index + numCol - 1);
        }
        // up-rigt
        if (down && right)
        {
            result.Add(index + numCol + 1);
        }
        return result;
    }
}
