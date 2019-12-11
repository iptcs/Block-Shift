using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    GameController myGameController;
    public int myX, myY;
    public int pusher;
    public int pushZone;

    // Start is called before the first frame update
    void Start()
    {
        myGameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    void MovePusher1()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && myGameController.pushingEnabled && pusher == 1)
        {
            Debug.Log("Push code ran");
            if (pushZone >= 25)
            {
                gameObject.transform.position = myGameController.pusherZones[0].transform.position;
                pushZone = 0;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            else if (pushZone < 25)
            {
                gameObject.transform.position = myGameController.pusherZones[pushZone + 1].transform.position;
                pushZone++;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            // skips the first place on repeat; don't know why
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && myGameController.pushingEnabled && pusher == 1)
        {
            Debug.Log("Push code ran");
            if (pushZone == 0)
            {
                gameObject.transform.position = myGameController.pusherZones[25].transform.position;
                pushZone = 25;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            else if (pushZone > 0)
            {
                gameObject.transform.position = myGameController.pusherZones[pushZone - 1].transform.position;
                pushZone--;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            // skips the first place on repeat; don't know why
        }
    }

    void MovePusher2()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && myGameController.pushingEnabled && pusher == 2)
        {
            Debug.Log("Push code ran");
            if (pushZone >= 25)
            {
                gameObject.transform.position = myGameController.pusherZones[0].transform.position;
                pushZone = 0;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            else if (pushZone < 25)
            {
                gameObject.transform.position = myGameController.pusherZones[pushZone + 1].transform.position;
                pushZone++;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            // skips the first place on repeat; don't know why
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && myGameController.pushingEnabled && pusher == 2)
        {
            Debug.Log("Push code ran");
            if (pushZone == 0)
            {
                gameObject.transform.position = myGameController.pusherZones[25].transform.position;
                pushZone = 25;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            else if (pushZone > 0)
            {
                gameObject.transform.position = myGameController.pusherZones[pushZone - 1].transform.position;
                pushZone--;
                float x = gameObject.transform.position.x;
                myX = (int)x;
                float y = gameObject.transform.position.y;
                myY = (int)y;
            }
            
        }
    }

    void Falling()
    {
        if (myGameController.grid[myX, myY - 1] == null)
        {
            myGameController.MoveBlock(gameObject, myX, myY - 1, myX, myY);
        }
    }

    private void OnMouseDown()
    {
        if (myGameController.destructionEnabled == true)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myGameController.fallingEnabled && gameObject.transform.position.y >= 0)
        {
            Falling();
        }

        MovePusher1();
        MovePusher2();
    }
}
