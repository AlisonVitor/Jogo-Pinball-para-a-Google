using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger_Ball_Detector : MonoBehaviour {

	// Plunger_Ball_Detector.js : Description : Use to know if a ball is on the launcher
	public GameObject obj_Spring;
	public Spring_Launcher spring_Launcher;
	public Rigidbody rb_Ball;

	public bool Ball_Collision = false;

	void Start(){
		spring_Launcher = obj_Spring.GetComponent<Spring_Launcher>();
	}

	void OnCollisionStay(Collision collision) {					// Ball is on the launcher
		if(collision.transform.tag == "Ball"){
			rb_Ball = collision.transform.GetComponent<Rigidbody>();
			spring_Launcher.BallOnPlunger(rb_Ball);
			Ball_Collision = true;
		}
	}


	void OnCollisionExit(Collision collision){						// Ball exit the launcher
		if(collision.transform.tag == "Ball"){
			//Debug.Log(collision.transform.name);
			rb_Ball = null;
			spring_Launcher.BallOnPlunger(rb_Ball);
			Ball_Collision = false;
		}
	}


	public bool ReturnCollision(){
		return Ball_Collision;
	}
}
