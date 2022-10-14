// Rollovers.js : Description : Manage the rollover mechanics
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollovers : MonoBehaviour {
	[Header ("Infos to missions")	]
	public int index;					// choose a number. Used to create script mission.
	public GameObject[] Parent_Manager;			// Connect on the inspector the missions that use this object
	public string functionToCall = "Counter";	// Call a function when OnCollisionEnter -> true;

	[Header ("Sound fx")]	
	public AudioClip Sfx_Hit;			// Sound when ball hit Rollover
	private AudioSource  sound_;

	[Header ("Points when ball go through rollover")]
	public int Points = 1000;			// Points you win when the object is hitting 
	private GameObject obj_Game_Manager;			// Use to connect the gameObject Manager_Game
	private Manager_Game gameManager;			// Manager_Game Component from obj_Game_Manager

	[Header ("Toy connected to the Rollover")]			// Connect a GameObject or paticule system with the script Toys.js attached
	public GameObject Toy;
	private Toys toy;
	public int AnimNum = 0;

	void Start(){															// --> Init
		obj_Game_Manager = GameObject.Find("Manager_Game");						// Find the gameObject Manager_Game
		if(obj_Game_Manager!=null)
			gameManager = obj_Game_Manager.GetComponent<Manager_Game>();			// Access Manager_Game from obj_Game_Manager
		sound_ = GetComponent<AudioSource>();									// Access AudioSource Component
		if(Toy)toy = Toy.GetComponent<Toys>();									// access Toys component if needed
	}

	void OnTriggerEnter (Collider other) {								// --> When the ball enter the trigger
		if(other.tag == "Ball"){
			for(var j = 0;j<Parent_Manager.Length;j++){
				Parent_Manager[j].SendMessage(functionToCall,index);			// Call Parents Mission script
			}

			if(!sound_.isPlaying && Sfx_Hit)sound_.PlayOneShot(Sfx_Hit);		// Play a sound if needed
			if(obj_Game_Manager!=null){
				if(gameManager)gameManager.F_Mode_BONUS_Counter();									// Send Message to the gameManager(Manager_Game.js) Add 1 to BONUS_Global_Hit_Counter
				if(gameManager)gameManager.Add_Score(Points);										// Send Message to the gameManager(Manager_Game.js) Add Points to Add_Score
			}
			if(Toy)toy.PlayAnimationNumber(AnimNum);							// Play toy animation if needed
		}
	}

	public int index_info(){														// return the index of the object. Use by missions
		return index;
	}
}
