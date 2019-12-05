using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[,] grid;
    public Vector3[] pusherZone;
    public GameObject cubePrefab, tester;
    public int gridX = 9, gridY = 5;
    Vector3 blockPosition;
    Color[] validColors = { Color.red, Color.blue, Color.yellow, Color.green, Color.white, Color.black };
    public bool destructionEnabled = false;
    public bool fallingEnabled = false;
    public List<GameObject> matches = new List<GameObject>();
    bool foundMatch = true;
    public int count = 0;
    public float turnTimer = 1.5f;
    public float turnLength = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[gridX, gridY];
        FillGrid();
    }

    // fills the grid, but ignores occupied spaces
    void FillGrid()
    {
        Debug.Log("Filling grid...");
        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                if (grid[x, y] == null)
                {
                    //print("NULL! at x:" + x + " y:" + y);
                    blockPosition = new Vector3(x, y, 0);
                    grid[x, y] = Instantiate(cubePrefab, blockPosition, Quaternion.identity);
                    grid[x, y].GetComponent<Renderer>().material.color = validColors[Random.Range(0, validColors.Length)];
                    grid[x, y].GetComponent<BlockScript>().myX = x;
                    grid[x, y].GetComponent<BlockScript>().myY = y;
                    // create an X and Y variable on a cube when it spawns
                    // cubes have code that makes them constantly move one space down when a certain bool is active
                    // unless they are already positioned at the bottom of the grid

                }
            }
        }
    }

    public void MoveBlock(GameObject block, int destX, int destY)
    {
        if (grid[destX, destY] == null && destX >= 0 && destY >= 0)
        {
            grid[destX, destY] = block;
            blockPosition = new Vector3(destX, destY, 0);
            block.transform.position = blockPosition;
            grid[destX, destY].GetComponent<BlockScript>().myX = destX;
            grid[destX, destY].GetComponent<BlockScript>().myY = destY;
        }
    }

    bool CheckHorizontalMatch(int x, int y)
    {
        if (x <= gridX - 3)  //Index out of range safety check
        {
            if (grid[x, y].GetComponent<Renderer>().material.color == grid[x + 1, y].GetComponent<Renderer>().material.color &&
             grid[x, y].GetComponent<Renderer>().material.color == grid[x + 2, y].GetComponent<Renderer>().material.color)
            {
                //print("MATCH at x:" + x + " y:" + y);
                return true;
            }
        }

        return false;
    }

    bool CheckVerticalMatch(int x, int y)
    {
        if (y <= gridY - 3) //Index out of range safety check
        {
            if (grid[x, y].GetComponent<Renderer>().material.color == grid[x, y + 1].GetComponent<Renderer>().material.color &&
              grid[x, y].GetComponent<Renderer>().material.color == grid[x, y + 2].GetComponent<Renderer>().material.color)
            {
                //print("MATCH at x:" + x + " y:" + y);
                return true;
            }
        }

        return false;
    }

    void AddMatch(int x, int y)
    {
        if (!matches.Contains(grid[x, y]))
        {
            matches.Add(grid[x, y]);
        }

    }

    //GameObject[] CheckMatches()
    void CheckMatches()
    {
        foundMatch = false;
        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                // if block is the same color as two other blocks next to it, add it to the list
                if (CheckHorizontalMatch(x, y))
                {
                    AddMatch(x, y);
                    AddMatch(x + 1, y);
                    AddMatch(x + 2, y);
                    foundMatch = true;
                    //Debug.Log("Added horizontal match blocks to list starting with x:" + x + " y:" + y);
                }

                if (CheckVerticalMatch(x, y))
                {
                    AddMatch(x, y);
                    AddMatch(x, y + 1);
                    AddMatch(x, y + 2);
                    foundMatch = true;
                    //Debug.Log("Added vertical match blocks to list starting with x:" + x + " y:" + y);
                }

            }
        }

        for (int i = 0; i < matches.Count; i++)
        {
            Destroy(matches[i]);
            matches[i] = null;
        }

        matches.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > turnTimer)
        {
            //FillGrid();
            //CheckMatches();
            // I switched the order, and the two methods are now running properly, so it seems
            // But I'm not sure why this fixed the problem
            turnTimer += turnLength;
        }
    }
}