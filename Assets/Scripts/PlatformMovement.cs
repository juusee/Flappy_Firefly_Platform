using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	Rigidbody platformRB;

	// Use this for initialization
	void Start () {
		platformRB = GetComponent<Rigidbody> ();
	}

	void Update() {
	}
}
