// LCD_Screen : Description : LCD : Choose the LCD Screen position relative to object cam. Usefull when screen resolution change.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_Screen : MonoBehaviour {

	public float Screen_Position_X = 0.15f; 						// LCD Screen Position. 0 = left Corner. 	1 = Right Corner  
	public float Screen_Position_Y = .9f; 							// LCD Screen Position. 0 = bottom Corner. 	1 = Up Corner  
	public float Screen_Position_Z = 8.5f; 							// LCD Screen Position. 0 = bottom Corner. 	1 = Up Corner  

	public Camera cam;												// The reference Camera
	public CameraSmoothFollow MainCam;
	public GameObject lCD_Screen;

	private bool OneTime = true;

	void Start() {
		if(Screen.width < Screen.height)
			OneTime = false;
		else
			OneTime = true;

		lCD_Screen.transform.position = cam.ViewportToWorldPoint(				// --> Choose the LCD Screen position relative to object cam
			new Vector3(Screen_Position_X,Screen_Position_Y,Screen_Position_Z)); 		// Transforms position from viewport space into world space.

		GameObject tmp = GameObject.Find("Pivot_Cam");

		MainCam = tmp.GetComponent<CameraSmoothFollow>();
	}


	void Update(){
		if(Screen.width < Screen.height && (MainCam.F_ReturnLastCam() == 3 || MainCam.F_ReturnLastCam() == 4)){
			lCD_Screen.transform.position = cam.ViewportToWorldPoint(				// --> Choose the LCD Screen position relative to object cam
				new Vector3(.5f,.94f,Screen_Position_Z)); 		// Transforms position from viewport space into world space.

			if(OneTime){
				lCD_Screen.SetActive(true);
				OneTime = false;
			}
		}
		else if(Screen.width < Screen.height && MainCam.F_ReturnLastCam() != 3 && MainCam.F_ReturnLastCam() != 4){
			lCD_Screen.transform.position = cam.ViewportToWorldPoint(				// --> Choose the LCD Screen position relative to object cam
				new Vector3(.5f,Screen_Position_Y,Screen_Position_Z)); 		// Transforms position from viewport space into world space.

			if(!OneTime){
				lCD_Screen.SetActive(false);
				OneTime = true;
			}
		}
		else if(Screen.width > Screen.height){
			lCD_Screen.transform.position = cam.ViewportToWorldPoint(				// --> Choose the LCD Screen position relative to object cam
				new Vector3(Screen_Position_X,Screen_Position_Y,Screen_Position_Z)); 		// Transforms position from viewport space into world space.
			if(OneTime){
				lCD_Screen.SetActive(true);
				OneTime = false;
			}
		}
	}
}
