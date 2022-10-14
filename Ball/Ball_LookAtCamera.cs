// Ball_LookAtCamera : Description : Use to create a fake light effect on the ball for mobile version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_LookAtCamera : MonoBehaviour {
	public GameObject target;

	void Start () {
		target = GameObject.FindWithTag("MainCamera");		// Find Main camera on the hierarchy
	}

	void Update () {
		if(target)transform.LookAt(target.transform);				// This gameObject look at the camera
	}
}
