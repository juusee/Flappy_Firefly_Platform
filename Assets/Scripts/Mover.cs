using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed;

	Rigidbody moverRB;

	// Use this for initialization
	void Awake ()
	{
		moverRB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		moverRB.velocity = Vector3.left * speed;
	}

	void OnEnable ()
	{
		moverRB.isKinematic = false;
	}

	void OnDisable ()
	{
		moverRB.isKinematic = true;
	}
}
