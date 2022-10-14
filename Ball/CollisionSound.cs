//CollisionSound.js : Description : Play a sound when something enter on collision whese this object
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour {
	[Header ("-> Hit Sound")]	
	public AudioClip 		a_hit;							// sound to play
	private AudioSource 	Hit_audio;						// roll AudioSource Component
	public float 			volumMax = .1f;					// volume to play
	[Header ("-> Flipper Case")	]
	public bool 			b_Flipper = false;				// true if this object is a flipper
	public GameObject 		Flipper;						// Connect the flipper

	void Start () {
		if(!b_Flipper)
			Hit_audio = GetComponent<AudioSource>();					// Access <AudioSource>() Component; if Hit sound is selected on the inspector
		else
			Hit_audio = Flipper.GetComponent<AudioSource>();		
	}


	void OnCollisionEnter(Collision collision) {						// -> On Collision Enter 
		if (collision.relativeVelocity.magnitude > 1){
			Hit_audio.volume = collision.relativeVelocity.magnitude*.25f;
			if(Hit_audio.volume > volumMax)
				Hit_audio.volume = volumMax;
			if(a_hit)Hit_audio.clip = a_hit;
			Hit_audio.Play();
		}
	}
}
