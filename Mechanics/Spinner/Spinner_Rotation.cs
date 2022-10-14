// Spinner : Description : Manage the rotation of the spinner
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner_Rotation : MonoBehaviour {

	private HingeJoint hinge;
	private bool b_Timer = false;
	public AudioClip Sfx_Hit;
	private AudioSource  sound_;
	//private GameObject obj_Game_Manager;
	//private Manager_Game gameManager;
	private bool b_Pause = false;

	void Start(){
		Physics.IgnoreLayerCollision(0,10, true);										// Default = 0 L_Spinner = 10
		hinge = GetComponent<HingeJoint>();
		var motor = hinge.motor;
		hinge.motor = motor;
		hinge.useMotor = true;

		//obj_Game_Manager = GameObject.Find("Manager_Game");
		//gameManager = obj_Game_Manager.GetComponent<Manager_Game>();	
		sound_ = GetComponent<AudioSource>();
	}


	void Update(){																	// Decrease the spinner speed
		if(b_Timer && !b_Pause){																	
			var motor = hinge.motor;				
			motor.targetVelocity = Mathf.MoveTowards(motor.targetVelocity,0,700*
				Time.deltaTime);
			hinge.motor = motor;
			if(motor.targetVelocity == 0){
				b_Timer = false;
			}
		}
	}


	public void Spin(float value){														// Call by the script Spinner_Trigger.js on gameObject Trigger_Spinner on the hierarchy
		if(Sfx_Hit)sound_.PlayOneShot(Sfx_Hit);	
		var motor = hinge.motor;
		motor.targetVelocity = 1000*value;

		hinge.motor = motor;
		b_Timer = true;
	}

	public void F_Pause_Start(){hinge.useLimits = true;b_Pause = true;}					// Use when Pause mode enable
	public void F_Pause_Stop(){hinge.useLimits = false;b_Pause = false;}	
}
