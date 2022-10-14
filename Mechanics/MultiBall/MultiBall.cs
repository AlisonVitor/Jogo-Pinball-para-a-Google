// MutilBall.js : Description : This script manage how to eject ball when Mutli ball is activated. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBall : MonoBehaviour {

	public int index;
	public GameObject obj_Game_Manager;
	private Manager_Game gameManager;
	public GameObject[] Gestionnaire_Parent;
	public string functionToCall = "Counter";			// Call a function when OnCollisionEnter -> true;

	public Transform Spawn;
	public Transform Spawn_tmp;
	public GameObject tmp_Ball;

	public AudioClip s_Load_Ball;
	public AudioClip s_Shoot_Ball;
	private AudioSource source;
	public Rigidbody rb;

	public float Time_Part_1= 2;					// Respawn Timer 
	private float tmp_Time = 0;
	private bool b_Part_1 = true;

	public float Slingshot_force = 4;

	public float Time_Part_2 = 0;					// Time to wait before adding force to the ball after the respawn			
	private float tmp_Time_2 = 0;
	private bool b_Part_2 = true;

	public bool Kickback = false;

	public GameObject obj_Door;

	public float Time_Part_3 = .5f;					// Time to wait before adding force to the ball after the respawn			
	//private float tmp_Time_3 = 0;
	//private bool b_Part_3 = true;

	public GameObject obj_Led;

	public int ball_Number = 3;
	public int counter = 0;
	private BoxCollider Box;

	private bool Pause = false;
	private CameraSmoothFollow pivotCam;			// access component CameraSmoothFollow. Use to avoid that the camera move too harshly when the ball respawn on the plunger  



	void Start(){
		Box = GetComponent<BoxCollider>();
		source = GetComponent<AudioSource>();
		if (obj_Game_Manager == null)														// Connect the Mission to the gameObject : "Manager_Game"
			obj_Game_Manager = GameObject.Find("Manager_Game");
		//if (obj_Game_Manager != null)
		//	gameManager = obj_Game_Manager.GetComponent<Manager_Game>();	

		GameObject tmp  = GameObject.Find("Pivot_Cam");
		if(tmp)pivotCam = tmp.GetComponent<CameraSmoothFollow>();	// Access Component CameraSmoothFollow from the main camera
	}


	void OnCollisionEnter(Collision collision ) {	
		if(collision.transform.tag == "Ball"){
			tmp_Ball = collision.gameObject;
			rb = tmp_Ball.GetComponent<Rigidbody>();
			if(!Kickback){
				rb.isKinematic = true;
				tmp_Ball.transform.position = Spawn.position;
			}
			b_Part_1 = false;
			if(obj_Led)obj_Led.GetComponent<ChangeSpriteRenderer>().F_ChangeSprite_Off();

			if(Gestionnaire_Parent.Length > 0){
				for(var j = 0;j<Gestionnaire_Parent.Length;j++){
					Gestionnaire_Parent[j].SendMessage(functionToCall,index);			// Call Parents Mission script
				}
			}
		}
	}


	void Update(){
		if(!Pause){
			if(!b_Part_1){													// Respawn Timer 
				tmp_Time = Mathf.MoveTowards(tmp_Time,Time_Part_1,
					Time.deltaTime);
				if(tmp_Time == Time_Part_1){								
					tmp_Time = 0;
					b_Part_1 = true;
					b_Part_2 = false;
					if(!Kickback)
						Ball_Respawn();
				}
			}

			if(!b_Part_2){													// Time to wait before adding force to the ball after the respawn
				tmp_Time_2 = Mathf.MoveTowards(tmp_Time_2,Time_Part_2,
					Time.deltaTime);
				if(tmp_Time_2 == Time_Part_2){						
					tmp_Time_2 = 0;
					b_Part_2 = true;
					Ball_AddForceExplosion();
				}
			}
		}
	}

	public void Ball_Respawn(){

		rb.velocity  = new Vector3(0,0,0);	
		tmp_Ball.transform.position = Spawn.position;
		rb.isKinematic = false;
	}

	public void Ball_AddForceExplosion(){
		rb.AddForce(Spawn.transform.forward*Slingshot_force, ForceMode.VelocityChange);	
		if(Slingshot_force>0){
			source.clip = s_Shoot_Ball;
			source.Play();
		}
		if(pivotCam)pivotCam.ChangeSmoothTimeInit();			// Call CameraSmoothFollow.js
	}

	public void KickBack_MultiOnOff(){
		if(Box.isTrigger) Box.isTrigger= false;
		else Box.isTrigger = true;
	}

	public void F_Pause(){
		if(!Pause)Pause = true;
		else Pause = false;
	}


	public void initHole(){		// Use by Game_Manager function InitGame_GoToMainMenu()
		b_Part_1 = true;
		b_Part_2 = true;
		Box.isTrigger = true;
		rb = null;
	}
}
