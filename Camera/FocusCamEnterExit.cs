// FocuCamEnterExit.js : Description : script use when you want the camera focus on a specific element
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamEnterExit : MonoBehaviour {
	private CameraSmoothFollow cam;								// Access Component CameraSmoothFollow from the main camera
	[Header ("Choose the focus view you want to use")]
	public int FocusCam  = 0;									// Select in the inspector The view you want to use. use -1 for the Exit trigger

	void Start(){
		GameObject tmp   = GameObject.Find("Pivot_Cam");
		if(tmp)cam = tmp.GetComponent<CameraSmoothFollow>();	// Access Component CameraSmoothFollow from the main camera
	}

	void OnTriggerEnter(Collider other) {						// --> When the ball enter the trigger, the camera view change
		if(other.tag == "Ball")
			cam.FocusCam(FocusCam);
	}
}
