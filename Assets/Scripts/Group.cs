using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour
{
    private bool _isDownPressedAfterSpawn;
    private float _leftAtTime;
    private float _rightAtTime;
    // Time since last gravity tick
    float lastFall = 0;

    public float HoldButtonDelay;

    // Use this for initialization
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            _leftAtTime = -1f;

        if (Input.GetKeyUp(KeyCode.RightArrow))
            _rightAtTime = -1f;

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryToMoveGroup(-1, 0, 0);
            _leftAtTime = Time.time;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryToMoveGroup(1, 0, 0);
            _rightAtTime = Time.time;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            TryToRotateGroup(-90);

        else if (Time.time - lastFall >= 0.7)
            TryToMoveGroupDown();

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            _isDownPressedAfterSpawn = true;

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (_isDownPressedAfterSpawn)
                TryToMoveGroupDown();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_leftAtTime > 0 && _leftAtTime + HoldButtonDelay < Time.time)
            {
                TryToMoveGroup(-1, 0, 0);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_rightAtTime > 0 && _rightAtTime + HoldButtonDelay < Time.time)
            {
                TryToMoveGroup(1, 0, 0);
            }
        }

    }

    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);

            // Not inside Border?
            if (!Grid.IsInsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void UpdateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.Height; ++y)
            for (int x = 0; x < Grid.Width; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    public void TryToMoveGroup(int delX, int delY, int delZ)
    {
        var oldPosition = transform.position;        
        transform.position += new Vector3(delX, delY, delZ);

        // See if valid
        if (IsValidGridPos())
        {
            // Its valid. Update grid.
            UpdateGrid();
            GameHelper.PlayMoveSound();
        }
        else
        // Its not valid. revert.
            transform.position = oldPosition;
    }

    public void TryToRotateGroup(int delRound)
    {
        transform.Rotate(0, 0, -90);

        // See if valid
        if (IsValidGridPos())
        {
            // It's valid. Update grid.
            UpdateGrid();
            GameHelper.PlayFlipSound();
        }
        else
        // It's not valid. revert.
            transform.Rotate(0, 0, 90);
    }

    public bool TryToMoveGroupDown()
    {
        transform.position += new Vector3(0, -1, 0);

        if (IsValidGridPos())
        {
            UpdateGrid();
            lastFall = Time.time;
            return true;
        }

        else
        {
            GameHelper.PlayLandSound();
            transform.position += new Vector3(0, 1, 0);
            Grid.DeleteFullRows();
            FindObjectOfType<Spawner>().SpawnNext();
            enabled = false;
            lastFall = Time.time;

            _isDownPressedAfterSpawn = false;
            _leftAtTime = -1f;
            _rightAtTime = -1f;
            return false;
        }        
    }
}
