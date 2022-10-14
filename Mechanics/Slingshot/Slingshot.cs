
// Slingshot : Description : Mange slingshot mechanics.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	[Header ("Infos to missions")]
	public int index;							// choose a number. Used to create script mission.
	public GameObject[] Parent_Manager;					// Connect on the inspector the missions that use this object
	public string functionToCall = "Counter";			// Call a function when OnCollisionEnter -> true;

	[Header ("Force parameters")]	
	public float Slingshot_force = 10;					// change the slingshot force added to a ball
	public float ForceMinimum = 1;  				// Minimum contact velocity between ball and slingshot to apply force
	public float relativeVelocityMax = 1;					// The maximum force apply to the ball

	[Header ("Sound fx")]	
	public AudioClip Sfx_Hit;					// Sound when ball hit the slingshot		
	private AudioSource  sound_;					// Audio Component

	[Header ("Points when the slingshot is hit")]
	public int Points = 1000;					// Points you win when the object is hitting 
	private GameObject obj_Game_Manager;
	private Manager_Game gameManager;

	[Header ("Connect a led")]
	public GameObject obj_Led;					// Usefull if you want a led blinked when the slingshot is hitting
	private ChangeSpriteRenderer Led_Renderer;

	[Header ("Toy connected to the Slingshot")	]				// Connect a GameObject or paticule system with the script Toys.js attached
	public GameObject obj_Toy;					// Usefull if you want a led blinked when the slingshot is hitting
	private Toys toy;
	public int animNumber = 0;


	void Start(){																	//	--> Init
		obj_Game_Manager = GameObject.Find("Manager_Game");								// Find the gameObject Manager_Game
		if(obj_Game_Manager!=null)
			gameManager = obj_Game_Manager.GetComponent<Manager_Game>();					// Access Manager_Game from obj_Game_Manager
		sound_ = GetComponent<AudioSource>();											// Access AudioSource Component

		if(obj_Led)Led_Renderer = obj_Led.GetComponent<ChangeSpriteRenderer>();		// Access led component if needed

		if(obj_Toy)toy = obj_Toy.GetComponent<Toys>();									// Access led component if needed
	}

	void OnCollisionEnter(Collision collision) {									// --> OnCollisionEnter with the ball
		Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

		if (rb != null && collision.relativeVelocity.magnitude > ForceMinimum){
			if(collision.relativeVelocity.magnitude < relativeVelocityMax){
				//Debug.Log("Yipo");
				float t = collision.relativeVelocity.magnitude;
				rb.velocity = new Vector3(rb.velocity.x*.5f,rb.velocity.y*.5f,rb.velocity.z*.5f);			// reduce the velocity at the impact. Better feeling with the slingshot
				rb.AddForce(transform.forward*Slingshot_force*t,ForceMode.VelocityChange);			// add force
			}
			else
				rb.AddForce(transform.forward*Slingshot_force*relativeVelocityMax,ForceMode.VelocityChange);


			if(Sfx_Hit)sound_.PlayOneShot(Sfx_Hit);										// Play a sound if needed

			for(var j = 0;j<Parent_Manager.Length;j++){
				Parent_Manager[j].SendMessage(functionToCall,index);					// Call Parents Mission script
			}

			if(gameManager)gameManager.F_Mode_BONUS_Counter();											// add one to the BONUS_Counter
			if(gameManager)gameManager.Add_Score(Points);												// add points

			if(obj_Led)Led_Renderer.Led_On_With_Timer(.2f);								// blinking

			if(obj_Toy)toy.PlayAnimationNumber(animNumber);								// play animation
		}
	}

}
