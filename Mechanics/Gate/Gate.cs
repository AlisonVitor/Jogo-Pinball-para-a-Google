// Gate : Description : Manage Gate mechanic : 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	public bool b_Trigger_Open = true;	// True if it is the trigger that open the gate. False if it is the trigger that close the door

	public GameObject obj_Gate;		// COnnect the gate
	private Target target;			// 


	void Start () {								
		target = obj_Gate.GetComponent<Target>();	// Access Target compenent
	}

	void OnTriggerEnter (Collider other) {	// -> On trigger Enter
		if(other.transform.tag == "Ball"){			// If it is a ball
			if(b_Trigger_Open){						// If it is the trigger taht open the gate
				target.Desactivate_Object();
			}
			else{									// if it the trigger that close the gate
				target.Activate_Object();
			}
		}
	}

}
