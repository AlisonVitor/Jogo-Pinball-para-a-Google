// Anim_On_LCD.js : Description : Manage Animation you could put on LCD Screen. Play Pause and destroy gameObject. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_On_LCD : MonoBehaviour {
	private GameObject obj_Manager;		
	private	Animator anim;


	void Start(){
		anim = GetComponent<Animator>();
	}

	public void DestoyAnimGameobject () {				// Destroy animation
		Destroy (gameObject);
	}

	public void Pause_Anim(){							// Pause animation
		if(anim.speed==0)Stop_Pause_Anim();
		else Start_Pause_Anim();  
	}

	public void Stop_Pause_Anim(){						// Stop
		anim.speed = 1;
	}
	public void Start_Pause_Anim(){					// Start
		anim.speed = 0;
	}

	public void PlayAnimation () {}

	public void StopAnimation(){}

	public void Mission_End_Fail(){
		obj_Manager.SendMessage("Mission_Fail");
	}

	public void MissionStartLCDMobile(){
		GameObject obj_LCDMobile = GameObject.Find("LCD_Content");
		if(obj_LCDMobile)this.transform.SetParent(obj_LCDMobile.transform);
	}
}
