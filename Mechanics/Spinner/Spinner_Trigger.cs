// Spinner_Trigger : Description : Use to know the direction of the ball
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner_Trigger : MonoBehaviour {

	public GameObject obj_Spinner;						// Connect to the object spinner in the hierachy
	private Spinner_Rotation spinner;					// Access component
	private int dir = 0;								// Know the direction of the ball



	void Start () {
		spinner = obj_Spinner.GetComponent<Spinner_Rotation>();			// access component
	}

	void OnTriggerEnter (Collider other) {							// When the ball enter the trigger
		if(other.transform.tag == "Ball"){
			Rigidbody rb = other.GetComponent<Rigidbody>();		
			spinner.Spin(rb.velocity.magnitude*dir);						// Send the velocity and the direction of the ball
		}
	}


	void FixedUpdate(){													// Save the direction of the ball
		var fwd = transform.InverseTransformDirection (Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, fwd ,out hit, 10) &&  hit.transform.tag == "Ball") {
			dir = 1;
		}
		else if (Physics.Raycast (transform.position, -fwd ,out hit, 10) &&  hit.transform.tag == "Ball") {
			dir = -1;
		}
	}
}
