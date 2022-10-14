// Camera_Movement : Description : Use on the GameObject Main_Camera on the hierarchy. 
// Manage Animator attach to the game Object
// Manage the UI button named  Text_Camera on the hierachy 
// This script send message to the script CameraSmoothFollow that you could find on game Object Pivot_Cam on the hierarchy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Movement : MonoBehaviour {
	private Animator anim;														// Animator Component
	private int MoveHash = Animator.StringToHash("b_Move");						// Refers to Animator Parameters b_Move.
	private int MoveStateHash = Animator.StringToHash("Base.Movement");			// Refers to Animator State Movement

	private int IdleHash = Animator.StringToHash("b_Idle_View");				// Refers to Animator Parameters b_Idle_View.
	private int IdleStateHash_1 = Animator.StringToHash("Base.Idle_View_1");	// Refers to Animator State Idle_View_1


	private int IdleHash_2 = Animator.StringToHash("b_Plunger");				// Refers to Animator Parameters b_Plunger.

	//private int PlungerHash = Animator.StringToHash("b_Plunger_View");			// Refers to Animator Parameters b_Plunger_View.
	//private int PlungerStateHash_1 = Animator.StringToHash("Base.Plunger_View");// Refers to Animator State Plunger_View

	private int ShakerHash = Animator.StringToHash("b_Shake");					// Refers to Animator Parameters b_Shake.
	private int ShakeStateHash_Right = Animator.StringToHash("Base.Shake_Right");// Refers to Animator State Shake_Right
	private int ShakeStateHash_Left = Animator.StringToHash("Base.Shake_Left");	// Refers to Animator State Shake_Left
	private int ShakeStateHash_Up = Animator.StringToHash("Base.Shake_Up");		// Refers to Animator State Shake_Up
	private int ShakeStateHash_Right_Plunger = Animator.StringToHash("Base.Shake_Left_Plunger");	// Refers to Animator State Shake_Up_Plunger
	private int ShakeStateHash_Left_Plunger = Animator.StringToHash("Base.Shake_Right_Plunger");	// Refers to Animator State Shake_Up_Plunger
	private int ShakeStateHash_Up_Plunger = Animator.StringToHash("Base.Shake_Up_Plunger");			// Refers to Animator State Shake_Up_Plunger


	public int 					CamView = 1;									// the camera currently used

	public Text 				Txt;											// Object to write the camera currently used
	private bool 				b_ChangeViewEnable = true;						// The camera could change only if b_ChangeViewEnable = true
	private float 				Timer_ChangeView = 0;							// Timer to calculate the minimum time before you could change the camera

	private GameObject 			obj_Pivot_Cam;									// the game object parent with the script CameraSmoothFollow.js								
	private CameraSmoothFollow 	cameraSmoothFollow;								// use to access CameraSmoothFollow component

	// Multi Ball variable
	private bool 				CameraMultiBall = false;						// Use when the game is on multiBall mode
	private int 				LastView  = 0;									// Use when the game is on multiBall mode 

	private bool 				CamStyle2D = false;												// Use to know if a 2D camera is used

	void Start () {
		anim = GetComponent<Animator>();													// Access to the Animator Component
		obj_Pivot_Cam = GameObject.Find("Pivot_Cam");										// Find object named Pivot_Cam on the hierarchy
		cameraSmoothFollow = obj_Pivot_Cam.GetComponent<CameraSmoothFollow>();				// Access to the CameraSmoothFollow Component from obj_Pivot_Cam

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);					// know what animation is active
		if(stateInfo.fullPathHash == MoveStateHash)			
			cameraSmoothFollow.Player_Change_Camera(8);										// if MoveStateHash is set at default layer state
		else
			cameraSmoothFollow.Player_Change_Camera(1);										// if IdleStateHash_1 is set at default layer state

		LastView = CamView;

		GameObject tmp  = GameObject.Find("PauseAndView");									// Find Gameobject Text_Camera

		if(tmp!=null){
			Transform[] children = tmp.GetComponentsInChildren<Transform>(true);

			foreach (Transform child in children){
				if(child.name == "Text_Camera"){
					Txt = child.GetComponent<Text>();								// Access the component UI.Text
					tmp = GameObject.Find("PauseAndView");
				}
			}

			if(tmp!=null){
				Transform[] children2 = tmp.GetComponentsInChildren<Transform>(true);

			foreach (Transform child in children2){
				if(child.name == "btn_Mobile_Pause"){
					Txt.transform.SetParent(child);						// Make the gameObject child of btn_Mobile_Pause gameObject
					Txt.gameObject.SetActive(true);								// Set active this gamObject
				}
			}
			}
		}



		if(cameraSmoothFollow.Return_CamStyle())CamStyle2D = true;							// Cam 2D is used
	}


	void Update () {
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);			// know what animation is active
		if(stateInfo.fullPathHash == MoveStateHash)											// If the active state is MoveStateHash ("Base.Movement")
			anim.SetBool(MoveHash, false);													// Animator Parameters MoveHash ("b_Move") = false

		if(!b_ChangeViewEnable){															// --> Minimium time before two animations. .5 seconds
			Timer_ChangeView = Mathf.MoveTowards(Timer_ChangeView,.5f,Time.deltaTime);
			if(Timer_ChangeView == .5f){
				b_ChangeViewEnable = true;
				Timer_ChangeView = 0;
			}
		}

		if(stateInfo.fullPathHash == ShakeStateHash_Right
			|| stateInfo.fullPathHash == ShakeStateHash_Left
			|| stateInfo.fullPathHash == ShakeStateHash_Up
			|| stateInfo.fullPathHash == ShakeStateHash_Up_Plunger
			|| stateInfo.fullPathHash == ShakeStateHash_Right_Plunger
			|| stateInfo.fullPathHash == ShakeStateHash_Left_Plunger
		)		
			anim.SetInteger(ShakerHash, 0);	
	}

	public void Shake_Cam(int shake){														// Play a specific animation when player shake the pinball. It's call by Manager_Game.js from the gameObject Manager_Game on the hierarchy
		anim.SetInteger(ShakerHash, shake);

	}

	public void StartPauseMode(){anim.speed = 0;}
	public void StopPauseMode(){anim.speed = 1;}

	public void PlayAnimation(){																// Play an animation ("Base.Movement").
		anim.SetBool(MoveHash, true);														// Animator Parameters MoveHash ("b_Move") = true
	}


	public void PlayIdle(){																	// This function is call by the script Spring_Launcher.js from the gameObject Spring on the hierachy
		if(!CamStyle2D){
			anim.SetInteger(ShakerHash, 0);
			anim.SetBool(IdleHash_2, false);													// The ball exit the plunger										
			cameraSmoothFollow.ExitPlunger(CamView);	
		}										
	}


	public void PlayPlunger(){																	// This function is call by the script Spring_Launcher.js from the gameObject Spring on the hierachy 
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);			// The ball enter the plunger
		//if(!CamStyle2D){
		if(stateInfo.fullPathHash == IdleStateHash_1 									
			|| stateInfo.fullPathHash == MoveStateHash){			
			anim.SetInteger(ShakerHash, 0);										
			anim.SetBool(IdleHash_2, true);													// Play the plunger animation
			cameraSmoothFollow.Plunger(5);													// Change the position of the camera
		}
		//}

	}


	public void Selected_Cam(){																// This function is used by the gameObject btn_Cam on the hierarchy to change the camera with a button on screen
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if(!CamStyle2D){
			if(b_ChangeViewEnable && !CameraMultiBall && stateInfo.fullPathHash != MoveStateHash){
				b_ChangeViewEnable = false;
				CamView ++;																		// Choose the next camera
				if(CamView == 5)
					CamView = 1;
				cameraSmoothFollow.Player_Change_Camera(CamView);								// Change the position of the camera
				if(Txt)Txt.text = CamView.ToString();										// Change Text on screen
			}
		}
	}


	public void Camera_MultiBall_Start(){									// --> Call by Manager_Game.js to use a specific camera when MultiBall Mode Start. Camera 4 is used
		if(!CamStyle2D){
			LastView = CamView;												// Save the camera number
			if(CamView == 1 || CamView == 2){								// The camera change only if it's cam 1 and cam 2 because cam 3 and 4 already sees the whole playfield 
				if(b_ChangeViewEnable){
					b_ChangeViewEnable = false;
					CamView = 6;											// Transition between cam 1 and 4. Change animation
					cameraSmoothFollow.Player_Change_Camera(CamView);		// Change the target that the camera look at

				}

			}
			if(Txt)Txt.text = "";													// Change Text on screen
			CameraMultiBall = true;	
		}
	}
	public void Camera_MultiBall_Stop(){									// --> Call by Manager_Game.js to use a specific camera when MultiBall Mode Stop
		if(!CamStyle2D){												
			CameraMultiBall = false;
			if(b_ChangeViewEnable){
				b_ChangeViewEnable = false;	
				CamView = LastView;											// Use to choose the good camera after MultiBall

				int tmp_Cam;
				if(CamView < 5)		
					tmp_Cam = 1;
				else
					tmp_Cam = CamView;
				anim.SetInteger(IdleHash, tmp_Cam);							// Play animation

				cameraSmoothFollow.Player_Change_Camera(CamView);			// Change the target that the camera look at
				if(Txt)Txt.text = LastView.ToString();								// Change Text on screen
			}
		}
	}

}
