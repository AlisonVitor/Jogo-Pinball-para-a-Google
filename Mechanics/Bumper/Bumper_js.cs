// Bumper : Description : Bumper Manager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper_js : MonoBehaviour {

	[Header ("Infos to missions")]	
	public int index;							// choose a number. Used to create script mission.
	public GameObject[] Parent_Manager;					// Connect on the inspector the missions that use this object
	public string functionToCall = "Counter";			// Call a function when OnCollisionEnter -> true;

	[Header ("Force applied to the ball")]	
	public float bumperForce = .6f;					// modify the force applied to the ball

	[Header ("Bumper sound")]
	public AudioClip Sfx_Hit;					// Sound when ball hit bumper
	private AudioSource  sound_;					// AudioSource Component

	[Header ("Points when the bumper is hit")]
	public int Points = 1000;					// Points you win when the object is hitting 
	private GameObject obj_Game_Manager;					// Use to connect the gameObject Manager_Game
	private Manager_Game gameManager;					// Manager_Game Component from obj_Game_Manager

	[Header ("LED connected to the bumper")]
	public GameObject obj_Led;					// Usefull if you want a led blinked when the slingshot is hitting
	private ChangeSpriteRenderer Led_Renderer;			// ChangeSpriteRenderer Component from obj_Led

	[Header ("Toy connected to the bumper")	]					// Connect a GameObject or paticule system with the script Toys.js attached
	public GameObject Toy;
	private Toys toy;
	public int AnimNum = 0;

	[Header ("Connected More than One Animation to the bumper")	]					// Connect a GameObject or paticule system with the script Toys.js attached
	public Toys[] Toys;
	public int[] AnimNums;



	void Start(){																			// --> function Start
		obj_Game_Manager = GameObject.Find("Manager_Game");										// Find the gameObject Manager_Game
		if(obj_Game_Manager!=null)
			gameManager = obj_Game_Manager.GetComponent<Manager_Game>();							// Access Manager_Game from obj_Game_Manager
		sound_ = GetComponent<AudioSource>();													// Access AudioSource Component

		if(obj_Led)Led_Renderer = obj_Led.GetComponent<ChangeSpriteRenderer>();				// If obj_Led = true; Access ChangeSpriteRenderer Component
		if(Toy)toy = Toy.GetComponent<Toys>();													// access Toys component if needed
	}

	void OnCollisionEnter(Collision collision) {											// --> Detect collision when bumper enter on collision with other objects
		ContactPoint[] tmpContact = collision.contacts;
		foreach (ContactPoint contact in tmpContact) {								// if there is a collision : 
			Rigidbody rb  = contact.otherCollider.GetComponent<Rigidbody>();				// Access rigidbody Component
			float t = collision.relativeVelocity.magnitude;								// save the collision.relativeVelocity.magnitude value
			rb.velocity = new Vector3(rb.velocity.x*.25f,rb.velocity.y*.25f,rb.velocity.z*.25f);		// reduce the velocity at the impact. Better feeling with the slingshot
			rb.AddForce( -1 * contact.normal * bumperForce,  ForceMode.VelocityChange);   	  	// Add Force
		}

		if(Sfx_Hit)sound_.PlayOneShot(Sfx_Hit);						// Play a sound

		for(var j = 0;j<Parent_Manager.Length;j++){
			Parent_Manager[j].SendMessage(functionToCall,index);								// Call Parents Mission script
		}

		if(obj_Game_Manager!=null){
			if(gameManager)gameManager.F_Mode_BONUS_Counter();													// Send Message to the gameManager(Manager_Game.js) Add 1 to BONUS_Global_Hit_Counter
			if(gameManager)gameManager.Add_Score(Points);														// Send Message to the gameManager(Manager_Game.js) Add Points to Add_Score
		}
		if(obj_Led)Led_Renderer.Led_On_With_Timer(.2f);											// If Obj_Led. Switch On the Led during .2 seconds
		if(Toy)toy.PlayAnimationNumber(AnimNum);												// Play toy animation if needed


		if(Toys.Length > 0){																	// Play more than One animation
			for(var i = 0;i<Toys.Length;i++){
				Toys[i].PlayAnimationNumber(AnimNums[i]);								
			}
		}

	}

	public int index_info(){																		// return the index of the object. Use by missions
		return index;
	}

}
