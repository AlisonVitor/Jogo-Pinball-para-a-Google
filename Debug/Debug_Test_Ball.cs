// Debug_Test_Ball.js : Description : Use on Prefab Debug_Test_Ball. Allow you to put the ball somewhere on playfield and add force to it.
// When you press a key the ball on playfield go to respawn position. When press key up a force is added to the ball. If there is no ball on playfield a ball is created
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Test_Ball : MonoBehaviour {



	public GameObject obj_Ball;					// Connect Ball : Pinball_ball prefab
	private Rigidbody tmp_rb;					// access rgidbody component
	public GameObject obj_Spawn;				// Connect ball respawn position

	private AudioSource sound_;					// audiosource component
	public AudioClip a_Loading;					// sound
	public AudioClip a_Shoot;					// sound

	public float TimeBeforeShootTheBall = 1;	// choose time before you add force to the ball
	public float Force = 2;

	public bool ReuseBall = true;				// if true : Check if there is a ball on playfield. If there is a ball on playfield the ball goes to respawn position.	If there is no ball on playfield a ball is created			
	public string Input_Key = "";				// Choose the input key that respawn the ball on playfield

	void Start () {																	// --> Init
		sound_ = GetComponent<AudioSource>();											// access audiosource component
	}

	void Update () {																// --> Update
		if(Input.GetKeyDown(Input_Key)){												// GetKeyDown : ball go to the respwn position
			GameObject[] gos;
			gos = GameObject.FindGameObjectsWithTag("Ball"); 						// Find ball oon playfield
			GameObject tmp_Ball;

			if(ReuseBall){																// ReuseBall = true
				if (gos.Length != 0) {													// there is a ball on playfield
					tmp_Ball = gos[0];
					tmp_Ball.transform.localPosition = obj_Spawn.transform.position;
					tmp_Ball.transform.rotation = obj_Spawn.transform.rotation;
					tmp_Ball.GetComponent<Rigidbody>().isKinematic = true;
					tmp_Ball.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
				}
				else{																	// there is no ball on playfield
					tmp_Ball = Instantiate(obj_Ball, 
						obj_Spawn.transform.position, 
						obj_Spawn.transform.rotation);
				}
			}
			else{																		// ReuseBall = false	
				tmp_Ball = Instantiate(obj_Ball, 
					obj_Spawn.transform.position, 
					obj_Spawn.transform.rotation);
			}
			tmp_rb = tmp_Ball.GetComponent<Rigidbody>();
			tmp_rb.isKinematic = true;
		}

		if(Input.GetKeyUp(Input_Key)){													// GetKeyUp : add force to the ball
			sound_.clip = a_Loading;
			sound_.Play();
			StartCoroutine("Ball_AddForceExplosion");
		}
	}

	IEnumerator Ball_AddForceExplosion(){													// --> Add force
		yield return new WaitForSeconds(TimeBeforeShootTheBall);
		tmp_rb.isKinematic = false;
		tmp_rb.AddForce(transform.forward*Force, ForceMode.VelocityChange);	
		sound_.clip = a_Shoot;
		sound_.Play();
	}
}
