// CameraSmoothFollow.cs : Description : script on object named Pivot_Cam on the hierarchy. Manage Camera movement in association with Camera_Movement.js on Main Camera on the hierarchy
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {
	public bool DebugCamView = false;		// Use to test new camera position

	public bool b_PortraitMode = false;		// true if Portrait mode



	private Vector2 tmp_offsetPortraitMode;


	private float smoothTime= 10;			// value to make the camera movement smoothly
	//private float distance= 4; 				// distance between transform.position, tmp_Target.position
	private float ChangeCamSpeed = 1;			// Speed to switch between two cam
	// --> Cam 1
	[System.Serializable]
	public class Cam1
	{
		public Transform Target_Fixed_Cam_1;			// the camera target when ball < Max_Pos_Y_Cam_1 value
		public Transform Target_Vertical_Cam_1;			// the camera target when ball > Max_Pos_Y_Cam_1 value
		public float Max_Pos_Y_Cam_1 = 0.25f;			// Max Movement for the camera. value between 0 to 1
		public float LimitCam1 = 0.07f;					// Camera follow (Vertical) ball if the ball position Z >  LimitCam1
		public float SpeedCam_1  = 100;					// smooth mcamera movement  ref 50
		public float Cam1_PosY = -6.2f;					// Modify the position Y of Cam 1
		public float Cam1_PosZ	 = -14;					// Modify the position Z of Cam 1
		public Vector2 offsetPortaitViewPositionCam1;	// Special distance when it is portrait mode
	}
	public Cam1 cam1 = new Cam1();
	// --> Cam 1
	[System.Serializable]
	public class Cam2
	{
		public Transform Target_Fixed_Cam_2;			// the camera target when ball < Max_Pos_Z_Cam_2 value
		public Transform Target_Horizontal_Cam_2;			// the camera target when ball > Max_Pos_Z_Cam_2 value
		public float Max_Pos_Z_Cam_2  = 0.75f;			//  Max Movement for the camera. value between 0 to 1
		public float LimitCam2	 = 0.45f;			// Camera follow (horizontal) ball if the ball position Z >  LimitCam2
		public float SpeedCam_2 = 100;			// smooth mcamera movement ref 2
		public float Cam2_PosY = -6.16f;		// Modify the position Y of Cam 2
		public float Cam2_PosZ=  -14;			// Modify the position Z of Cam 2
		public Vector2 offsetPortaitViewPositionCam2;				// Special distance when it is portrait mode
	}
	public Cam2 cam2 							  = new Cam2();
	// --> Cam 2
	[System.Serializable]
	public class Cam3
	{
		public Transform Target_Fixed_Cam_3;			// Cam 3 : camera target 
		public float Cam3_PosY = -6.2f;			// Modify the position Y of Cam 3
		public float Cam3_PosZ = -13.3f;		// Modify the position Z of Cam 3
		public Vector2 offsetPortaitViewPositionCam3;				// Special distance when it is portrait mode
	}
	public Cam3 cam3 = new Cam3();

	// --> Cam 4
	[System.Serializable]
	public class Cam4
	{
		public Transform Target_Fixed_Cam_4;			// Cam 4 : camera target
		public float Cam4_PosY = -6.2f;			// Modify the position Y of Cam 4
		public float Cam4_PosZ	= -13.3f;		// Modify the position Z of Cam 4
		public Vector2 offsetPortaitViewPositionCam4;				// Special distance when it is portrait mode
	}
	public Cam4 cam4 = new Cam4();

	[System.Serializable]
	public class CamPlunger
	{
		public Transform Target_Plunger;			// Plunger Cam target ref 100
		public float CamPlunger_PosY = -6.2f;			// Modify the position Y of Cam Plunger
		public float CamPlunger_PosZ = -13.3f;		// Modify the position Z of Cam Plunger
		public Vector2 offsetPortaitViewPositionPlunger;				// Special distance when it is portrait mode
	}
	public CamPlunger camPlunger= new CamPlunger();

	// --> Focus Cams									
	private Transform Target_Fixed_Cam_Focus1;			
	private float CamMini_PosY = -6.2f;			
	private float CamMini_PosZ = -13.3f;	
	private Vector2 CamMini_Portrait  = new Vector2(0,0);	
	private float ChangeCamSpeedMini= 3;
	[System.Serializable]
	public class FocusCams
	{
		public Transform Target_Fixed;
		public float Focus_PosY 	= -6.2f;
		public float Focus_PosZ 	= -13.3f;
		public float ChangeCamSpeed = 10;
		public Vector2 offsetPortaitViewPositionFocus;				// Special distance when it is portrait mode
	}
	public FocusCams[] focusCams 	 = new FocusCams[1];
	private int LastCam	 = 1;					// Know the last cam number. Use when Focus Cam Ended

	private Transform tmp_Target;			// temporary target that camera must reach
	//private bool Move_Camera = false;		// know if the camera could move

	[Header ("Object that follow the ball")]
	public GameObject obj_Follow_Ball;			// Object that follow the ball on scene
	private Transform Ball ;			// Ball GameObject on scene

	private bool b_follow = true;		// if obj_Follow_Ball follow a ball on playfield
	private Vector3 InitPos	;		// Position when b_follow = false
	//private Rigidbody rb_obj_Follow_Ball;			// rigidbody for the obj_Follow_Ball
	private float lastPosZ;				// save obj_Follow_Ball.transform.position.z 
	private bool Up	= false;		// The ball goes to the back of the playfield


	private float limit = 10;			// obj_Follow_Ball.transform.localPosition.z > limit the camera follow the ball. use for Cam 1 and Cam 2
	private float speed = 1;			// movement speed for the camera

	private bool Plunger_Activate  = false;		// know if the ball is in the plunger		
	private float Cam_Style 	= 0;			// 0 : Cam1 	1 : Cam2	4 : Cam3 and Cam4	5 : Cam Plunger		8 : Animation Cam when Game Start 

	private bool b_ChangeCamera_Position = false;		// Know if we are switching between two camera	

	// The next 3 vairables are used to move the camera to a new Camera position.
	private float t;	
	private Transform startPosition;			
	private Transform target;			// Use to change the camera angle

	private bool PlayerChangeTheCamera = false;		//	Know if the player change the camera view

	//private bool T_Position = false;

	private bool b_PauseMode = false;		// Use to pause camera movement

	private float offsetY 	= 0;			// Use to choose a position for camera
	private float offsetZ 	= 0;			// Use to choose a position for camera

	public bool b_Portrait	= false;		// Use to know orientation device
	public bool b_Landscape = true;		// Use to know orientation device

	//private bool OneTimePortrait 	= true;	// Use to know orientation device
	//private bool OneTimePortraitUpsideDown = true;	// Use to know orientation device
	//private bool OneTimeLandscape = true;	// Use to know orientation device


	[Header ("2D Cam Style")]
	public bool Mode2DCam = false; 	// Use Orthographic cam (2D style)
	public Camera MainCamera2D;			// Need to be connected
	public float PortraitOrthoSize = .5f;		// CHoose the size of camera when mobile device is in portrait mode
	public float LandscapeOrthoSize = .3f;		// CHoose the size of camera when mobile device is in landscape mode

	public float MinCam2DPositionZ = 10;		// Correspond to obj_Follow_Ball.transform.position.z
	public float MaxCam2DPositionZ = 42;		// Correspond to obj_Follow_Ball.transform.position.z
	public float Ball2DOffset 	= 0.08f;		// Offset between 2D camera and ball
	public int Camera2DSpeedFollow = 50;			// Speed to follow the ball with the 2D camera

	private DeviceOrientation LastOrientation;


	void Start(){

		//rb_obj_Follow_Ball = GetComponent<Rigidbody>();									// access rigidbody component	
		startPosition 		= transform;	
		if(!Mode2DCam){
			InitPos.y 			= 	obj_Follow_Ball.transform.position.y;
			InitPos.z 			= 	obj_Follow_Ball.transform.position.z;
		}
		else{

			obj_Follow_Ball.transform.position = new Vector3 (
				obj_Follow_Ball.transform.position.x,
				obj_Follow_Ball.transform.position.y,
				MinCam2DPositionZ
			);

			StartCoroutine ("WaitToInit");
		}

	}

	IEnumerator WaitToInit(){
		yield return new WaitForEndOfFrame();
		InitPos.y 			= 	obj_Follow_Ball.transform.position.y;
		InitPos.z 			= 	MinCam2DPositionZ;
	}

	public bool Return_CamStyle(){														// --> Call by Camera_Movement.js on Main Camera GameObject on the hierarchy. Know If game use 2D Cam Style
		return Mode2DCam;
	}

	public void F_OrientationChange(){											
		GameObject[] gos;			
		gos = GameObject.FindGameObjectsWithTag("Flipper"); 						// Find all game objects with tag Flipper
		/*for (var go : GameObject in gos)  { 
			go.GetComponent.<Flippers>().PreventBugWhenOrientationChange();			// Release Flippers to prevent bug with flippers
		} */
		foreach (GameObject go in gos){
			go.GetComponent<Flippers>().PreventBugWhenOrientationChange();			// Release Flippers to prevent bug with flippers
		}
	}



	public void Update(){																		// --> UPDATE Function

		if(Input.deviceOrientation == DeviceOrientation.Portrait && Input.deviceOrientation != LastOrientation){
			b_Portrait = true;
			b_Landscape = false;
			//OneTimePortrait = false;
			//OneTimeLandscape = true;
			//OneTimePortraitUpsideDown = true;
			F_OrientationChange();
		}
		else if(Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown && Input.deviceOrientation != LastOrientation){
			b_Portrait = true;
			b_Landscape = false;
			//OneTimePortrait = true;
			//OneTimeLandscape = true;
			//OneTimePortraitUpsideDown = false;
			F_OrientationChange();
		}
		else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft && Input.deviceOrientation != LastOrientation
			|| Input.deviceOrientation == DeviceOrientation.LandscapeRight && Input.deviceOrientation != LastOrientation){
			b_Portrait = false;
			b_Landscape = true;
			//OneTimePortrait = true;
			//OneTimeLandscape = false;
			//OneTimePortraitUpsideDown = true;
			F_OrientationChange();
		}
		LastOrientation = Input.deviceOrientation;

		if(b_Portrait){
			b_Landscape = false;
			b_PortraitMode = true;
			Player_Change_Camera(LastCam);

			if(Mode2DCam && MainCamera2D)MainCamera2D.orthographicSize = PortraitOrthoSize;

			b_Portrait = false;


		}
		if(b_Landscape){
			b_Landscape = false;
			b_PortraitMode = false;
			Player_Change_Camera(LastCam);

			if(Mode2DCam && MainCamera2D)MainCamera2D.orthographicSize = LandscapeOrthoSize;

			b_Portrait = false;
		}

		if(!b_PauseMode){																	// if The game is not on pause
			if(Mode2DCam){																		// True if you want to sue the 2D camera Style
				if(b_follow){																	// --> obj_Follow_Ball follow the ball on playfield
					if(Ball != null){															// IF there is a ball on playfield

						obj_Follow_Ball.transform.position = new Vector3 (					// obj_Follow_Ball follow the ball
							Ball.position.x,
							Ball.position.y,
							Ball.position.z
						);

					

						if (obj_Follow_Ball.transform.localPosition.z > MaxCam2DPositionZ) {

							obj_Follow_Ball.transform.localPosition = new Vector3(
								obj_Follow_Ball.transform.localPosition.x,
								obj_Follow_Ball.transform.localPosition.y,
								MaxCam2DPositionZ
							);
						}

						if (obj_Follow_Ball.transform.localPosition.z < MinCam2DPositionZ) {
							obj_Follow_Ball.transform.localPosition = new Vector3(
								obj_Follow_Ball.transform.localPosition.x,
								obj_Follow_Ball.transform.localPosition.y,
								MinCam2DPositionZ
							);
						}
					}
					else{																		// ELSE. Check if there is ball on playfield
						var targetObj2 = GameObject.FindGameObjectWithTag("Ball");
						if(targetObj2 != null){													// if true
							Ball = targetObj2.transform;
							//if(Ball != null)													
							//	rb_obj_Follow_Ball = Ball.GetComponent<Rigidbody>();			
						}


						obj_Follow_Ball.transform.localPosition = new Vector3(
							obj_Follow_Ball.transform.localPosition.x,
							obj_Follow_Ball.transform.localPosition.y,
							MinCam2DPositionZ
						);
					}
				}
				else{																			// IF false. 

					obj_Follow_Ball.transform.position = new Vector3(							// obj_Follow_Ball go to his init position
						obj_Follow_Ball.transform.position.x,
						InitPos.y,
						InitPos.z
					);
				}
			}
			else{
				if(b_follow){																	// --> obj_Follow_Ball follow the ball on playfield
					if(Ball != null){															// IF there is a ball on playfield
						obj_Follow_Ball.transform.position = new Vector3(						// obj_Follow_Ball follow the ball
							obj_Follow_Ball.transform.position.x,
							Ball.position.y,
							Ball.position.z
						);

					}
					else{																		// ELSE. Check if there is ball on playfield
						var targetObj = GameObject.FindGameObjectWithTag("Ball");
						if(targetObj != null){													// if true
							Ball = targetObj.transform;
							//if(Ball != null)													
							//	rb_obj_Follow_Ball = Ball.GetComponent<Rigidbody>();			
						}
					}
				}
				else{																			// IF false. 

					obj_Follow_Ball.transform.position = new Vector3(						// obj_Follow_Ball follow the ball
						obj_Follow_Ball.transform.position.x,
						InitPos.y,
						MinCam2DPositionZ
					);
				}

				if(Ball != null){
					if(lastPosZ > obj_Follow_Ball.transform.position.z){						// --> know if the ball is going to go down
						if(lastPosZ >= obj_Follow_Ball.transform.position.z + .1){				// ball is going to go down
							Up = false;
							lastPosZ = obj_Follow_Ball.transform.position.z;
						}
					}
					else{																		 // ball is going to go up
						Up = true;
						lastPosZ = obj_Follow_Ball.transform.position.z;
					}
				}


				cam1.Target_Vertical_Cam_1.transform.localPosition = new Vector3 (
					cam1.Target_Vertical_Cam_1.transform.localPosition.x,
					cam1.Max_Pos_Y_Cam_1*(obj_Follow_Ball.transform.localPosition.y - 0) /(2-0),// Normalize Target_Vertical_Cam_1 Movement. Target_Vertical_Cam_1 follow obj_Follow_Ball
					cam1.Target_Vertical_Cam_1.transform.localPosition.z
				);


				cam2.Target_Horizontal_Cam_2.transform.localPosition = new Vector3 (
					cam2.Target_Horizontal_Cam_2.transform.localPosition.x,
					cam2.Target_Horizontal_Cam_2.transform.localPosition.y,
							cam2.Max_Pos_Z_Cam_2*(obj_Follow_Ball.transform.localPosition.y - 0) /(2-0)// Normalize Target_Horizontal_Cam_2 Movement. Target_Horizontal_Cam_2 follow obj_Follow_Ball
				);

			}

		}
	}

	public void LateUpdate () {   																// --> LateUpdate is called after all Update functions have been called. A follow camera should always be implemented in LateUpdate because it tracks objects that might have moved inside Update.
		//Debug.Log(b_ChangeCamera_Position);
		if(!b_PauseMode){																	// if The game is not on pause
			if(Mode2DCam){																	// True if you want to sue the 2D camera Style
				transform.position = Vector3.Slerp( transform.position, 
					new Vector3(
						transform.position.x,
						transform.position.y,
						obj_Follow_Ball.transform.position.z - Ball2DOffset
					),
					Time.fixedDeltaTime * Camera2DSpeedFollow
				);
			}
			else{
				//if(!startWithMovementAnimation){
				if(!b_ChangeCamera_Position){

					if(Cam_Style != 5){															// The camera change occurs only when the ball is not in the launcher
						if (Cam_Style == 0){													// --> Cam 1
							if(obj_Follow_Ball.transform.localPosition.z > limit){				// Camera Move Up and Down
								//Move_Camera = true;
								speed = 0;
								tmp_Target = cam1.Target_Vertical_Cam_1;
							}
							else{
								//Move_Camera = false;
								speed = 0;
							}
						}
						else if (Cam_Style == 1){												// --> Cam 2 Camera Move Forward Backward
							if(obj_Follow_Ball.transform.localPosition.z > limit && Up){
								//Move_Camera = true;
								tmp_Target = cam2.Target_Horizontal_Cam_2;
								smoothTime = cam2.SpeedCam_2;
								speed = 0;
							}
							else {
								//Move_Camera = false;
								tmp_Target = cam2.Target_Fixed_Cam_2;
								smoothTime = cam2.SpeedCam_2*.5f;
								speed = Mathf.MoveTowards(speed,2,Time.deltaTime*3);
							}
						}
					}

					if(	tmp_Target != null)															
						//distance = Vector3.Distance( transform.localPosition, tmp_Target.localPosition) ;	// Calculate the distance between transform.position and tmp_Target.position

					if(Cam_Style == 1){															// --> Cam 2 Movement
						transform.localPosition = Vector3.Slerp( transform.localPosition, 
							new Vector3(
								transform.localPosition.x,
								tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x,
								tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y
							),
							(2+speed*speed*20)*(Time.fixedDeltaTime * smoothTime)
						);
					}
					else if(Cam_Style != 8 && Cam_Style != 5 ){									// --> Cam 1 Movement	
						transform.localPosition = Vector3.Slerp( transform.localPosition, 
							new Vector3(
								transform.localPosition.x,
								tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x,
								tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y
							),
							(Time.fixedDeltaTime * smoothTime)
						);
					}
				}
				else if(Cam_Style != 8){																	// --> Move the camera when the player change the camera view
					t += Time.deltaTime/ChangeCamSpeed;


					float localXPos = 0;
					float localYPos =  Mathf.Lerp(startPosition.localPosition.y, tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x, t);				// Change the camera position
					float localZPos =  Mathf.Lerp(startPosition.localPosition.z, tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y, t);



					if(Cam_Style ==9)
						localXPos = Mathf.Lerp(startPosition.localPosition.x, tmp_Target.localPosition.x, t);
					else
						localXPos = Mathf.Lerp(startPosition.localPosition.x, .228f, t);

					transform.localPosition = new Vector3(
						localXPos,
						localYPos,
						localZPos
					);

					transform.rotation = Quaternion.Lerp(startPosition.rotation, target.rotation, t);		// Change camera rotation

					if(Mathf.Round(transform.localPosition.y*10) == Mathf.Round((tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x)*10) 		
						&& Mathf.Round(transform.localPosition.z*10) == Mathf.Round((tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y)*10) ){ 	// Use when it is the end of Focus 1
						if(Cam_Style !=9)ChangeCamSpeed 	= 2;
					}
					if(Mathf.Round(transform.localPosition.y*1000000) == Mathf.Round((tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x)*1000000) 		
						&& Mathf.Round(transform.localPosition.z*1000000) == Mathf.Round((tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y)*1000000) ){ 	// Wait until the camera reach his new position
						b_ChangeCamera_Position = false;													// the camera reach his new position
						PlayerChangeTheCamera = false;
						//Debug.Log("ici");
					}
					if(obj_Follow_Ball.transform.localPosition.z > limit && Cam_Style != 4 
						&& !PlayerChangeTheCamera){	 														// Use to prevent bug when the player change camera view and the ball > limit
						b_ChangeCamera_Position = false;
					}

				}

				if(DebugCamView){																			// true if you want to customize your camera view
					transform.rotation = Quaternion.Lerp(startPosition.rotation, target.rotation, t);		// Change camera rotation
					if(Cam_Style == 0){offsetY = cam1.Cam1_PosY;offsetZ = cam1.Cam1_PosZ;}
					if(Cam_Style == 1){offsetY = cam2.Cam2_PosY;offsetZ = cam2.Cam2_PosZ;}
					if(LastCam == 3){offsetY = cam3.Cam3_PosY;offsetZ = cam3.Cam3_PosZ;}
					if(LastCam == 4){offsetY = cam4.Cam4_PosY;offsetZ = cam4.Cam4_PosZ;}
					if(Cam_Style == 5){offsetY = camPlunger.CamPlunger_PosY;offsetZ = camPlunger.CamPlunger_PosZ;}
					if(Cam_Style == 9){offsetY = CamMini_PosY;offsetZ = CamMini_PosZ;}
					if(LastCam == 3 || LastCam ==  4 || Cam_Style == 5 || Cam_Style == 9){									
						transform.localPosition = Vector3.Slerp( transform.localPosition, 
							new Vector3(
								transform.localPosition.x,
								tmp_Target.localPosition.y+offsetY+tmp_offsetPortraitMode.x,
								tmp_Target.localPosition.z+offsetZ+tmp_offsetPortraitMode.y
							),
							(2*20)*(Time.fixedDeltaTime * 100)
						);
					}
					//}
				}	
			}
		}
	}

	public void Plunger(int CamView){									// --> Call by Camera_Movement.js on Main Camera GameObject on the hierarchy. Ball Enter the plunger
		Plunger_Activate = true;
		newTarget(CamView);												// Change the camera view
	}
	private int cmp;
	public void ExitPlunger(int CamView){								// --> Call by Camera_Movement.js on Main Camera GameObject on the hierarchy. Ball Exit the plunger
		cmp++;
		//Debug.Log(cmp);
		Plunger_Activate = false;
		newTarget(CamView);												// Change the camera view
	}

	public int F_ReturnLastCam(){
		return LastCam;
	}

	public void FocusCam(int value){										// Use a Focus camera
		if(LastCam != 6){												// if Multiball mode is not activate
			if(value != -1){
				CamMini_PosY = focusCams[value].Focus_PosY;				
				CamMini_PosZ = focusCams[value].Focus_PosZ;	
				CamMini_Portrait = focusCams[value].offsetPortaitViewPositionFocus;				
				Target_Fixed_Cam_Focus1 = focusCams[value].Target_Fixed;
				ChangeCamSpeedMini = focusCams[value].ChangeCamSpeed;
				Player_Change_Camera(9);
			}
			else 
				Player_Change_Camera(LastCam);
		}
	}
	public void Player_Change_Camera(int CamView){						// --> Call by Camera_Movement.js on Main Camera GameObject on the hierarchy. Ball Enter the plunger
		PlayerChangeTheCamera = true;
		newTarget(CamView );											// Change the camera view
	}	


	public void newTarget(int CamView){									// --> Call by Camera_Movement.js on Main Camera GameObject on the hierarchy.
		//Debug.Log("Plunger_Activate " + Plunger_Activate);
		//Debug.Log("ici");
		if(!Mode2DCam){													// True if you want to sue the 2D camera Style
			switch (CamView) {
			case 1:
				if(!Plunger_Activate){									// -> Cam 1 : Camera Move Up and Down
					Cam_Style 				= 0;								// Use to know the camera type
					offsetY 				= cam1.Cam1_PosY;					// Y : Camera_Pivot Target position we want to target
					offsetZ					= cam1.Cam1_PosZ;					// Z : Camera_Pivot Target position we want to target

					if(b_PortraitMode)tmp_offsetPortraitMode = cam1.offsetPortaitViewPositionCam1;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime 				= cam1.SpeedCam_1;					// Camera Vertical speed;
					limit 					= cam1.LimitCam1;					// obj_Follow_Ball.transform.localPosition.z > limit the camera follow the ball. use for Cam 1 and Cam 2
					tmp_Target 				= cam1.Target_Fixed_Cam_1;			// Camera try to reach this position

					b_ChangeCamera_Position = true;								// 
					t 						= 0;						// 	The next 3 variables are used to move the camera to the next Camera view	
					target 					= cam1.Target_Fixed_Cam_1;			// Camera look at this object transform		
					LastCam 				= 1;


				}
				break;
			case 2:
				if(!Plunger_Activate){									// -> Cam 2 : Camera Move Forward Backward
					Cam_Style 				= 1;								// Use to know the camera type
					offsetY 				= cam2.Cam2_PosY;					// Y : Camera_Pivot Target position we want to target
					offsetZ 				= cam2.Cam2_PosZ;					// Z : Camera_Pivot Target position we want to target

					if(b_PortraitMode)tmp_offsetPortraitMode = cam2.offsetPortaitViewPositionCam2;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime 				= cam2.SpeedCam_2;					// Camera Horizontal speed 50;
					limit 					= cam2.LimitCam2;					// obj_Follow_Ball.transform.localPosition.z > limit the camera follow the ball. use for Cam 1 and Cam 2
					tmp_Target 				= cam2.Target_Fixed_Cam_2;			// Camera try to reach this position
					b_ChangeCamera_Position = true;
					t  						= 0;
					target 					= cam2.Target_Fixed_Cam_2;			// Camera look at this object transform
					LastCam 				= 2;
				}
				break;
			case 3:
				if(!Plunger_Activate){									// -> Cam 3 : Camera Fixe
					Cam_Style 				= 4;
					offsetY 				= cam3.Cam3_PosY;					// Y : Camera_Pivot Target position we want to target
					offsetZ				 	= cam3.Cam3_PosZ;					// Z : Camera_Pivot Target position we want to target
					if(b_PortraitMode)tmp_offsetPortraitMode = cam3.offsetPortaitViewPositionCam3;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime				= 0;//100;
					tmp_Target 				= cam3.Target_Fixed_Cam_3;			// Camera try to reach this position
					b_ChangeCamera_Position = true;
					t 						= 0;
					target 					= cam3.Target_Fixed_Cam_3;			// Camera look at this object transform
					LastCam 				= 3;
				}
				break;
			case 4:
				if(!Plunger_Activate){									// -> Cam 4 : Camera Fixe
					Cam_Style 				= 4;
					offsetY 				= cam4.Cam4_PosY;					// Y : Camera_Pivot Target position we want to target
					offsetZ 				= cam4.Cam4_PosZ;					// Z : Camera_Pivot Target position we want to target
					if(b_PortraitMode)tmp_offsetPortraitMode = cam4.offsetPortaitViewPositionCam4;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime				= 0;				//100;
					tmp_Target 				= cam4.Target_Fixed_Cam_4;			// Camera try to reach this position
					b_ChangeCamera_Position = true;			
					t 						= 0;	
					target 					= cam4.Target_Fixed_Cam_4;			// Camera look at this object transform
					LastCam 				= 4;
				}
				break;


			case 5:														// -> Cam Plunger
				Cam_Style 				= 5;
				smoothTime 				= 0;								//
				tmp_Target 				= camPlunger.Target_Plunger;
				offsetY 				= camPlunger.CamPlunger_PosY;		// Y : Camera_Pivot Target position we want to target
				offsetZ 				= camPlunger.CamPlunger_PosZ;		// Z : Camera_Pivot Target position we want to target

				if(b_PortraitMode)tmp_offsetPortraitMode = camPlunger.offsetPortaitViewPositionPlunger;	// Offset Portrait View
				else	tmp_offsetPortraitMode = new Vector2(0,0);


				b_ChangeCamera_Position = true;					
				t = 0;
				target 					= camPlunger.Target_Plunger;		// Camera look at this object transform
				LastCam 				= 5;
				break;	



			case 6:
				if(!Plunger_Activate){									// Use For MultiBall Mode.  Same Param as Case 4
					Cam_Style 				= 4;
					offsetY				 	= cam4.Cam4_PosY;				// Y : Camera_Pivot Target position we want to target
					offsetZ 				= cam4.Cam4_PosZ;				// Z : Camera_Pivot Target position we want to target

					if(b_PortraitMode)tmp_offsetPortraitMode = cam4.offsetPortaitViewPositionCam4;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime				= 0;							//
					tmp_Target 				= cam4.Target_Fixed_Cam_4;		// Camera try to reach this position
					b_ChangeCamera_Position = true;			
					t 						= 0;
					target 					= cam4.Target_Fixed_Cam_4;		// Camera look at this object transform
					LastCam 				= 6;
				}
				break;  

			case 8:
				Cam_Style 					= 8;						// -> Cam Animation when game start
				break; 

			case 9 :
				if(!Plunger_Activate){									// -> Focus Cams
					Cam_Style 				= 9;
					offsetY 				= CamMini_PosY;				// Y : Camera_Pivot Target position we want to target
					offsetZ 				= CamMini_PosZ;				// Z : Camera_Pivot Target position we want to target

					if(b_PortraitMode)tmp_offsetPortraitMode = CamMini_Portrait;	// Offset Portrait View
					else	tmp_offsetPortraitMode = new Vector2(0,0);

					smoothTime				= 0;						//
					tmp_Target 				= Target_Fixed_Cam_Focus1;	// Camera try to reach this position
					b_ChangeCamera_Position = true;			
					t 						= 0;	
					target 					= Target_Fixed_Cam_Focus1;	// Camera look at this object transform
					ChangeCamSpeed 			= ChangeCamSpeedMini;		// Transition speed between two camera
				}
				break;

			}	
		}
	}




	public void ChangeSmoothTimeWhenBallIsLost(){smoothTime = 10;}			// Use By manger_Game.js. Globaly Modify the smoothTime when the ball is lost. It avoids that the camera move too harshly when the ball respawn on the plunger  
	public void ChangeSmoothTimeInit(){										// Use By MultiBall.js. Globaly Modify the smoothTime when the ball is lost. It avoids that the camera move too harshly when the ball respawn on the plunger  
		if(Cam_Style == 0)smoothTime = cam1.SpeedCam_1;
		if(Cam_Style == 1)smoothTime = cam2.SpeedCam_2;
		if(Cam_Style == 4)smoothTime = 0;
		if(Cam_Style == 5)smoothTime = 0;
	}					

	//public void F_Camera_Move_Enable(){Move_Camera = true;}
	//public void F_Camera_Move_Disable(){Move_Camera = false;}

	public void StartPauseMode(){b_PauseMode=true;}
	public void StopPauseMode(){b_PauseMode=false;}


}
