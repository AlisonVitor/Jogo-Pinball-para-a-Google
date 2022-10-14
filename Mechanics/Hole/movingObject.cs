using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObject : MonoBehaviour {

    public bool             Mode_Pause = false;
    public bool             b_openDoor = false;

    public float            endPosition = 1;

    private float           currentPos = 0;

    public float            speed_Open = 1;
    public float            speed_Close = 1;

    public void openDoor(){
        b_openDoor = false;
    }

    public void closeDoor()
    {
        b_openDoor = true;
    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Y)){
            b_openDoor = true; 
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            b_openDoor = false;
        }
        */


        if(!Mode_Pause){
            if (transform.localPosition.x != endPosition && !b_openDoor)
            {                  // Close the door
                currentPos = Mathf.MoveTowards(currentPos, endPosition, Time.deltaTime * speed_Close);
                transform.localPosition = new Vector3(currentPos, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x != 0 && b_openDoor)     // Open the door
            {
                currentPos = Mathf.MoveTowards(currentPos, 0, Time.deltaTime * speed_Open);
                transform.localPosition = new Vector3(currentPos, transform.localPosition.y, transform.localPosition.z);

            } 
        }

    }


    public void Pause_Anim()
    {                                       // --> Pause Animation
        if (!Mode_Pause){
            Mode_Pause = true;
        }
        else{
            Mode_Pause = false; 
        }

    }

}
