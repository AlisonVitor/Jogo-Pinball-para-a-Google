// ConnectCameraToUI.js : Description : This script is used to connect the gameObject UI_Cam to the UI_Cam Object on the hierarchy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectCameraToUI : MonoBehaviour {
	void Start () {
		GameObject tmp  = GameObject.Find("Cam_UI");
		if(tmp != null){
			Camera tmp_Cam = GameObject.Find("Cam_UI").GetComponent<Camera>();		// Find the camera

			GetComponent<Canvas>().worldCamera = tmp_Cam;							// Connect the gameObject UI_Cam to the UI_Cam Object
		}
	}

}
