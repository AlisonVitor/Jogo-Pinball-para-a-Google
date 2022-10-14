// Spring_Launcher : Description : Manage the spring Launcher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_Launcher : MonoBehaviour {


	//private Rigidbody rb;
	private bool _GetButton = false;							// true if you want input manage by Edit -> Project Settings -> Input
	[Header ("Manual or Auto launcher")]
	public bool Auto_Mode = false;								// true : Auto / false : Manual
	private Rigidbody rb_Ball;
	private bool Activate = false;
	private string name_F;

	[Header ("Force apply to the ball")]
	public float _Spring_Force  = 7;							// force apply to the ball
	private float tmp_Spring_Force;

	[Header ("Sound fx")]
	public AudioClip Sfx_Pull ;									// sound played when you pull the plunger
	private bool Play_Once	= false;
	public AudioClip Sfx_Kick ;									// sound played when the ball is kicked
	private AudioSource  sound_;								// Audiosource component

	private float Spring_Max_Position = 0;						// spring maximum position
	private float Spring_Min_Position = -.6f;					// spring minimum position

	private GameObject obj_Game_Manager;						// Game_Manager
	private Manager_Input_Setting gameManager_Input;			// access Manager_Input_Setting from Game_Manager on the hierarchy

	[Header ("Spring force to change cam view")]
	public float Cam_Change_Min = .4f;							// if the force is greater than this value , the camera returns to the previous camera
	private GameObject Camera_Board;							// Access camera
	private Camera_Movement camera_Movement;					// Access Camera component

	[Header ("Time To wait before player could launch the ball")]	
	public float Timer = .5f;									// Time to wait before player could launch the ball
	private float tmp_Timer = 0;
	private bool b_Timer = false;

	private bool Ball_ExitThePlunger = false;					

	private bool b_Debug = false;								// use when you want to make test. Call by Manager_Input_Setting.js		
	private bool b_touch =false;
	private bool b_Tilt = false;
	private GameObject obj_Mission_SkillShot;


    public bool springIsPulled = false;

	public BoxCollider _BoxCollider;

    public GameObject Mobile_Collider_zl;

	void Start() {														// --> Init
		Camera_Board = GameObject.Find("Main Camera");						// init Camera
		if(!b_Debug && Camera_Board && Camera_Board.GetComponent<Camera_Movement>())
			camera_Movement = Camera_Board.GetComponent<Camera_Movement>();

		//rb = GetComponent<Rigidbody>();
		sound_ = GetComponent<AudioSource>();								// init audio

		obj_Game_Manager = GameObject.Find("Manager_Game");					// init game_Manager
		if(obj_Game_Manager != null){
			gameManager_Input = obj_Game_Manager.GetComponent<Manager_Input_Setting>();	

			name_F = gameManager_Input.F_Plunger();
		}
	}

	void Update(){														// --> Update
        if (rb_Ball)
        {
            if (Mobile_Collider_zl && !Mobile_Collider_zl.activeInHierarchy)
                Mobile_Collider_zl.SetActive(true);
            
            for (var i = 0; i < Input.touchCount; ++i)
            {                   // This part is used for mobile control
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (hit.transform.name == "Mobile_Collider" || hit.transform.name == "Mobile_Collider_zl")
                        {
                            b_touch = true;
                        }
                    }

                }
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    b_touch = false;
                }
            }


            if (Activate && Auto_Mode)
            {                                           // Case : AutoMode : true
                if (!_GetButton)
                {
                    if (Input.GetKeyDown(name_F) || b_touch)
                    {
                        Ball_AddForceExplosion();                                   //	Add Force
                        if (!b_Debug && camera_Movement) camera_Movement.PlayIdle();    // Change Cam
                        F_Desactivate();                                            // Desactivate spring
                    }
                }
                else
                {
                    if (Input.GetButtonDown(name_F) || b_touch)
                    {
                        Ball_AddForceExplosion();                                   //	Add Force
                        if (!b_Debug && camera_Movement) camera_Movement.PlayIdle();    // Change Cam
                        F_Desactivate();                                            // Desactivate spring
                    }
                }
            }

            if (Activate && !Auto_Mode)
            {                                       // -> Case : Manual Mode : Auto_mode = false

                if (!_GetButton)
                {
                    if (Input.GetKey(name_F) || b_touch)
                    {
                        if (transform.localPosition.z >= Spring_Min_Position)
                        {
                            // Move the spring launcher
                            transform.localPosition = new Vector3(
                                transform.localPosition.x,
                                transform.localPosition.y,
                                Mathf.MoveTowards(transform.localPosition.z, Spring_Min_Position, 1 * Time.deltaTime)
                            );

                            if (!sound_.isPlaying && Sfx_Pull && !Play_Once)
                            {       // play a sound
                                sound_.clip = Sfx_Pull;
                                sound_.volume = .7f;
                                sound_.Play();
                                Play_Once = true;
                            }
                            springIsPulled = true;
                        }
                        tmp_Spring_Force = _Spring_Force * .5f * transform.localPosition.z * transform.localPosition.z; // save the force
                    }
                    else
                    {
                        if (Activate)
                        {
                            if (transform.localPosition.z < Spring_Max_Position)
                            {
                                // Move the spring launcher
                                transform.localPosition = new Vector3(
                                    transform.localPosition.x,
                                    transform.localPosition.y,
                                    Mathf.MoveTowards(transform.localPosition.z, Spring_Max_Position, 15 * Time.deltaTime)
                                );


                                if (transform.localPosition.z == 0 && springIsPulled)
                                {
                                    if (Play_Once)
                                    {
                                        sound_.Stop();
                                        Play_Once = false;
                                    }
                                    springIsPulled = false;
                                    Ball_AddForceExplosion();                           // add force
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetButton(name_F) || b_touch)
                    {
                        if (transform.localPosition.z >= Spring_Min_Position)
                        {

                            // Move the spring launcher
                            transform.localPosition = new Vector3(
                                transform.localPosition.x,
                                transform.localPosition.y,
                                Mathf.MoveTowards(transform.localPosition.z, Spring_Min_Position, 1 * Time.deltaTime)
                            );


                            if (!sound_.isPlaying && Sfx_Pull && !Play_Once)
                            {       // play a sound
                                sound_.clip = Sfx_Pull;
                                sound_.volume = .7f;
                                sound_.Play();
                                Play_Once = true;
                            }

                            springIsPulled = true;
                        }
                        tmp_Spring_Force = _Spring_Force * .5f * transform.localPosition.z * transform.localPosition.z; // save the force
                    }
                    else
                    {
                        if (Activate)
                        {
                            if (transform.localPosition.z < Spring_Max_Position)
                            {

                                // Move the spring launcher
                                transform.localPosition = new Vector3(
                                    transform.localPosition.x,
                                    transform.localPosition.y,
                                    Mathf.MoveTowards(transform.localPosition.z, Spring_Max_Position, 15 * Time.deltaTime)
                                );

                                if (transform.localPosition.z == 0 && springIsPulled)
                                {
                                    if (Play_Once)
                                    {
                                        sound_.Stop();
                                        Play_Once = false;
                                    }
                                    springIsPulled = false;
                                    Ball_AddForceExplosion();                           // add force
                                }
                            }
                        }
                    }
                }


                if (Ball_ExitThePlunger && transform.localPosition.z == 0)
                {       // Prevent error with th camera movement
                    F_Desactivate();
                }
            }
            else if (!Activate && !Auto_Mode)
            {                                   // Move the spring launcher to init his position
                if (transform.localPosition.z < Spring_Max_Position)
                {

                    // Move the spring launcher
                    transform.localPosition = new Vector3(
                        transform.localPosition.x,
                        transform.localPosition.y,
                        Mathf.MoveTowards(transform.localPosition.z, Spring_Max_Position, 15 * Time.deltaTime)
                    );

                    if (transform.localPosition.z == 0)
                    {
                        Play_Once = false;
                    }
                }
            }

            if (b_Timer)
            {                                                   // Time to wait before adding force to the ball after the respawn
                tmp_Timer = Mathf.MoveTowards(tmp_Timer, Timer,             // Prevent error with th camera movement
                    Time.deltaTime);
                if (Timer == tmp_Timer)
                {                                       // Shoot ball enable.  		
                    tmp_Timer = 0;
                    b_Timer = false;
                    Activate = true;
                }
            }


            if (b_Tilt && rb_Ball)
            {                                           // Prevent Bug : Kick the ball if the game is on tilt mode and a ball on the plunger
                rb_Ball.AddForce(transform.forward * _Spring_Force, ForceMode.VelocityChange);
                if (!b_Debug && camera_Movement) camera_Movement.PlayIdle();
                if (!sound_.isPlaying && Sfx_Kick) sound_.PlayOneShot(Sfx_Kick);
                rb_Ball = null;
            }
        }
        else{
            if (Mobile_Collider_zl && Mobile_Collider_zl.activeInHierarchy)
                Mobile_Collider_zl.SetActive(false);
        }
	}

	public void F_Activate(){ 															// Activate the plunger	. Call by Manager_Game.js on game object Manager_Game on the hierarchy
		b_Timer = true;	Ball_ExitThePlunger = false;
		if(_BoxCollider)_BoxCollider.enabled = true;	
	}			
	public void F_Desactivate(){														// Desactivate the plunger. Call by Manager_Game.js on game object Manager_Game on the hierarchy
		Activate = false;
		if(_BoxCollider)_BoxCollider.enabled = false;
	}										
	public void F_Activate_After_Tilt(){ 												// Activate the plunger if the table is tilted..Call by Manager_Game.js on game object Manager_Game on the hierarchy
		b_Timer = true;								
		Ball_ExitThePlunger = false;
		b_Tilt = false;
		if(_BoxCollider)_BoxCollider.enabled = true;
	} 									

	public void Tilt_Mode(){b_Tilt = true;}											//  Desactivate the plunger if the table is tilted. Call by Manager_Game.js on game object Manager_Game on the hierarchy


	public void Ball_AddForceExplosion(){												// --> Add force to the ball
		if(rb_Ball != null){
			if(!Auto_Mode){
				rb_Ball.AddForce(transform.forward*_Spring_Force*tmp_Spring_Force*tmp_Spring_Force, ForceMode.VelocityChange);	
				if(Cam_Change_Min < tmp_Spring_Force){
					if(camera_Movement)camera_Movement.PlayIdle();	
					Ball_ExitThePlunger = true;
					if(obj_Mission_SkillShot)										// if obj_Mission_SkillShot != null send a message to start the skillshot timer on the skillshot mission 
						obj_Mission_SkillShot.SendMessage("Skillshot_Mission");		
				}
			}
			else{
				if(!b_Debug && camera_Movement)camera_Movement.PlayIdle();	

				rb_Ball.AddForce(transform.forward*_Spring_Force, ForceMode.VelocityChange);	
			}
			if(!sound_.isPlaying && Sfx_Kick)sound_.PlayOneShot(Sfx_Kick);

            //Debug.Log("Here");

			tmp_Spring_Force = 0;
			rb_Ball = null;
		}
	}


	public void BallOnPlunger(Rigidbody rb_obj){										// Know if the ball is on the plunger
		if(!b_Debug && camera_Movement && rb_obj)camera_Movement.PlayPlunger();

		if(rb_obj){F_Activate();}
		else{F_Desactivate();}

		rb_Ball = rb_obj;				
	}

	public void F_Debug(){
		b_Debug = true;
	}

	public void Connect_Plunger_To_Skillshot_Mission(GameObject obj){				// --> Call by the mission that use the skillshot  
		obj_Mission_SkillShot = obj;
	}


	public void F_InputGetButton(){														// use Edit -> Project Settings -> Input for Flippers
		_GetButton = true;
	}

	public void ActivatePlunger(){									// Use This function is you want to pull plunger outside this script.Call SendMessage("ActivatePlunger");
		if(!sound_.isPlaying && Sfx_Pull && !Play_Once){		
			sound_.clip = Sfx_Pull;
			sound_.volume = .7f; 
			sound_.Play();
			Play_Once = true;
		}
		b_touch = true;	

	}

	public void DeactivatePlunger(){								// Use This function is you want to push plunger outside this script.Call SendMessage("DeactivatePlunger");
		b_touch = false;
	}

}
