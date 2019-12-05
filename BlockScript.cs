using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    GameController myGameController;
    public int myX, myY;

    // Start is called before the first frame update
    void Start()
    {
        myGameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    void Falling()
    {
        myGameController.MoveBlock(gameObject, myX, myY--);
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
        if (myGameController.fallingEnabled && myY > 0)
        {
            Falling();
            Debug.Log(myX + myY + " is falling");
        }
    }
}
