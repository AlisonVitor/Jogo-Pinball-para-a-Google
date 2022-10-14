// Spinner_LapCounter : Description : Count the spinner rotation. This object is used by mission 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner_LapCounter : MonoBehaviour {


	[Header ("Infos to missions")]	
	public int index;										// choose a number. Used to create script mission.
	public GameObject[] Parent_Manager;								// Connect on the inspector the missions that use this object
	public string functionToCall = "Counter";						// Call a function when OnCollisionEnter -> true;

	static int Lap = 0;									
	//private int tmp_CheckLap;

	[Header ("Spinner rotation sound")]
	public AudioClip Sfx_Rotation;								// Sound when ball hit Spinner
	private AudioSource  sound_ ;								// AudioSource component

	[Header ("Points when the spinner rotate")]
	public int Points = 1000;								// Points you win when the object is hitting 
	private GameObject obj_Game_Manager;								// Use to connect the gameObject Manager_Game
	private Manager_Game gameManager;								// Manager_Game Component from obj_Game_Manager


	void Start(){														// --> init
		obj_Game_Manager = GameObject.Find("Manager_Game");					// Find the gameObject Manager_Game
		gameManager = obj_Game_Manager.GetComponent<Manager_Game>();		// Access Manager_Game from obj_Game_Manager
		sound_ = GetComponent<AudioSource>();								// Access AudioSource Component
	}

	void OnTriggerExit (Collider other) {								// --> When ball enter on the trigger
		Lap++;
		//tmp_CheckLap = Lap;
		for(var j = 0;j<Parent_Manager.Length;j++){
			Parent_Manager[j].SendMessage(functionToCall,index);			// Call Parents Mission script
		}
		if(Sfx_Rotation)sound_.PlayOneShot(Sfx_Rotation);					// Play soiund if needed
		if(gameManager) gameManager.F_Mode_BONUS_Counter();									// Send Message to the gameManager(Manager_Game.js) Add 1 to BONUS_Global_Hit_Counter
		if(gameManager)gameManager.Add_Score(Points);										// Send Message to the gameManager(Manager_Game.js) Add Points to Add_Score
	}


	public int index_info(){													// return the index of the object. Use by missions
		return index;
	}

}
