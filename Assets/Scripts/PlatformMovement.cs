using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	Rigidbody platformRB;
	Vector3 initialPosition;
	bool directionUp = Random.value > 0.5f ? true : false;
	bool forceAdded = false;
	bool jou = false;
	float velocity = 10f;

	// Use this for initialization
	void Start () {
		platformRB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (directionUp && !forceAdded) {
			platformRB.AddForce (Vector3.up * velocity, ForceMode.Impulse);
			if (jou) {
				platformRB.AddForce (Vector3.up * velocity, ForceMode.Impulse);
			}
			forceAdded = true;
			jou = true;
			print ("force added up");
		} else if (!directionUp && !forceAdded) {
			platformRB.AddForce (Vector3.down * velocity, ForceMode.Impulse);
			if (jou) {
				platformRB.AddForce (Vector3.down * velocity, ForceMode.Impulse);
			}	
			forceAdded = true;
			jou = true;
			print ("force added downw");
		}
		if (transform.localPosition.y - initialPosition.y > 10f && directionUp) {
			directionUp = false;
			forceAdded = false;
		} else if (transform.localPosition.y - initialPosition.y < -10f && !directionUp) {
			directionUp = true;
			forceAdded = false;
		}
	}

	void OnEnable() {
		initialPosition = transform.localPosition;
	}
}
