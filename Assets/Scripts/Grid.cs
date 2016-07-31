using UnityEngine;
using System.Collections;

public class Grid : Singleton<Grid>
{
    public const int Width = 10;
    public const int Height = 20;
    
    public Transform[,] grid = new Transform[Width, Height];

    public Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    public bool IsInsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < Width &&
                (int)pos.y >= 0);
    }

    public void DeleteRow(int y)
    {
        for (int x = 0; x < Width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void DecreaseRow(int y)
    {
        for (int x = 0; x < Width; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < Height; ++i)
            DecreaseRow(i);
    }

    public bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    public void DeleteFullRows()
    {
        for (int y = 0; y < Height; ++y)
        {
            if (IsRowFull(y))
            {
                GameHelper.PlayFullRawSound();
                GameHelper.IncreaseScore();
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }
}
