
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SiteManager : MonoBehaviour
{
    public static SiteManager Instance;
    public Transform site;
    public GridLayoutGroup grid;
    public Cell prefabCell;

    [Range(0, 20)]
    public int numCol = 10;
    [Range(0, 20)]
    public int numRow = 8;

    const float scale = 0.0053f;
    public int Count => numCol * numRow;

    public List<Cell> cells = new();

    protected void Awake()
    {
        Instance = this;
        InitSite();
    }

    public Transform GetAtIndex(int row, int col)
    {
        int index = row * col + col;
        return index < Count && row >= 0 && col >= 0 ? site.GetChild(row * col + col) : null;
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
        if (col - 1 >= 0)
        {
            left = true;
            result.Add(index - 1);
        }
        // right
        if (col + 1 < numCol)
        {
            right = true;
            result.Add(index + 1);
        }
        // up
        if (row - 1 >= 0)
        {
            up = true;
            result.Add(index - numCol);
        }
        // down
        if (row + 1 < Count / numCol)
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

    void InitSite()
    {
        if (prefabCell == null)
            return;

        grid.constraintCount = numCol;

        if (grid == null)
            return;

        for (int i = 0; i < Count; i++)
        {
            Cell newCell = Instantiate(prefabCell, site);
            cells.Add(newCell);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 size = grid.cellSize;
        Vector2 space = grid.spacing;
        float newX = size.x * numCol + (numCol - 1) * space.x;
        float newY = size.y * numRow + (numRow - 1) * space.y;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, scale * new Vector3(newX, newY, 1));
    }
#endif
}
