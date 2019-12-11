using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[,] grid;
    public List<Vector3> pusherPos = new List<Vector3>();
    public GameObject[] pusherZones;
    public GameObject cubePrefab, pusherSpace;
    public GameObject pusher1, pusher2;
    int gridX = 8, gridY = 5;
    Vector3 blockPosition;
    Color[] validColors = { Color.red, Color.blue, Color.yellow, Color.green, Color.white, Color.black };
    public bool destructionEnabled = false;
    public bool fallingEnabled = false;
    public bool matching = false;
    public bool filling = false;
    public bool pushingEnabled = false;
    public List<GameObject> matches = new List<GameObject>();
    bool foundMatch = true;
    public int count = 0;
    public float turnTimer = 2.5f;
    public float turnLength = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        pusherPos.Add(new Vector3(-2, 0, 0));
        pusherPos.Add(new Vector3(-2, 1, 0));
        pusherPos.Add(new Vector3(-2, 2, 0));
        pusherPos.Add(new Vector3(-2, 3, 0));
        pusherPos.Add(new Vector3(-2, 4, 0));

        pusherPos.Add(new Vector3(0, 6, 0));
        pusherPos.Add(new Vector3(1, 6, 0));
        pusherPos.Add(new Vector3(2, 6, 0));
        pusherPos.Add(new Vector3(3, 6, 0));
        pusherPos.Add(new Vector3(4, 6, 0));
        pusherPos.Add(new Vector3(5, 6, 0));
        pusherPos.Add(new Vector3(6, 6, 0));
        pusherPos.Add(new Vector3(7, 6, 0));

        pusherPos.Add(new Vector3(9, 4, 0));
        pusherPos.Add(new Vector3(9, 3, 0));
        pusherPos.Add(new Vector3(9, 2, 0));
        pusherPos.Add(new Vector3(9, 1, 0));
        pusherPos.Add(new Vector3(9, 0, 0));

        pusherPos.Add(new Vector3(7, -2, 0));
        pusherPos.Add(new Vector3(6, -2, 0));
        pusherPos.Add(new Vector3(5, -2, 0));
        pusherPos.Add(new Vector3(4, -2, 0));
        pusherPos.Add(new Vector3(3, -2, 0));
        pusherPos.Add(new Vector3(2, -2, 0));
        pusherPos.Add(new Vector3(1, -2, 0));
        pusherPos.Add(new Vector3(0, -2, 0));

        

        grid = new GameObject[gridX, gridY];
        pusherZones = new GameObject[26];
        MakePusherZones();
        FillGrid();
        SpawnPusher(pusher1, 1, 1);
        SpawnPusher(pusher2, 5, 2);
    }

    void MakePusherZones()
    {
        for(int x = 0; x < 26; x++)
        {
            pusherZones[x] = Instantiate(pusherSpace, pusherPos[x], Quaternion.identity);
        }
    }

    void SpawnPusher(GameObject pusherBlock, int zoneNumber, int pusherNumber)
    {
        pusherBlock = Instantiate(cubePrefab, pusherZones[zoneNumber].transform.position, Quaternion.identity);
        pusherBlock.GetComponent<Renderer>().material.color = validColors[Random.Range(0, validColors.Length)];
        pusherBlock.GetComponent<BlockScript>().pusher = pusherNumber;
        pusherBlock.GetComponent<BlockScript>().pushZone = zoneNumber;
        float x = pusherBlock.transform.position.x;
        pusherBlock.GetComponent<BlockScript>().myX = (int)x;
        float y = pusherBlock.transform.position.y;
        pusherBlock.GetComponent<BlockScript>().myY = (int)y;
    }

    // fills the grid, but ignores occupied spaces
    void FillGrid()
    {
        if (filling)
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
    }

    public void MoveBlock(GameObject block, int destX, int destY, int oldX, int oldY)
    {
        if (destX >= 0 && destY >= 0)
        {
            grid[destX, destY] = block;
            block.transform.position = new Vector3(destX, destY, 0);
            grid[destX, destY].GetComponent<BlockScript>().myX = destX;
            grid[destX, destY].GetComponent<BlockScript>().myY = destY;
            grid[oldX, oldY] = null;
        }
    }

    public void PushRowRight(GameObject pushedBlock)
    {
        int y = pushedBlock.GetComponent<BlockScript>().myY;
        int x = 0;
        bool foundEmpty = false;
        while (x <= gridX && foundEmpty == false)
        {
            if (x < gridX)
            {
                MoveBlock(grid[x, y], grid[x, y].GetComponent<BlockScript>().myX + 1, grid[x, y].GetComponent<BlockScript>().myY,
                    grid[x, y].GetComponent<BlockScript>().myX, grid[x, y].GetComponent<BlockScript>().myY);
            }
            else if (x == gridX)
            {
                Destroy(grid[x, y]);
            }
            if (grid[x+1, y] != null)
            {
                foundEmpty = true;
            }
            x++;
        }
        grid[0, y] = pushedBlock;
        pushedBlock.transform.position = new Vector3(0, y, 0);
        grid[0, y].GetComponent<BlockScript>().myX = 0;
        grid[0, y].GetComponent<BlockScript>().myY = y;
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
        if (matching)
        {
            foundMatch = false;
            for (int y = 0; y < gridY; y++)
            {
                for (int x = 0; x < gridX; x++)
                {
                    if (grid[x, y] != null)
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
            }

            for (int i = 0; i < matches.Count; i++)
            {
                Destroy(matches[i]);
                matches[i] = null;
            }

            matches.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > turnTimer)
        {
            FillGrid();
            CheckMatches();
            // I switched the order, and the two methods are now running properly, so it seems
            // But I'm not sure why this fixed the problem
            turnTimer += turnLength;
        }
        // testing pushing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PushRowRight(pusher1);
        }
    }
}
