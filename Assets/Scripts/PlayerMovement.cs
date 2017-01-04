﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public Slider jumpPowerSlider;

	public Slider jumpVelocitySlider;
	public Text jumpVelocityText;
	public Slider uiJumpPowerSlider;
	public Text jumpPowerText;

	Rigidbody playerRB;
	float jumpVelocity = 80f;
	float prevJumpTime = 0f;
	float maxJumpPower = 100f;
	float currentJumpPower = 100f;
	float jumpPower = 22f;

	bool run = false;

	void Awake ()
	{
		playerRB = GetComponent<Rigidbody>();
		playerRB.velocity = new Vector3 (10f, 0f, 0f);

		jumpVelocitySlider.onValueChanged.AddListener(changeJumpVelocity);
		jumpVelocitySlider.value = jumpVelocity;
		jumpVelocityText.text = jumpVelocity.ToString ();
		uiJumpPowerSlider.onValueChanged.AddListener(changeJumpPower);
		uiJumpPowerSlider.value = jumpPower;
		jumpPowerText.text = jumpPower.ToString ();
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Jump (Input.mousePosition.x);
		} else {
			for (int i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).phase == TouchPhase.Began)
					Jump (Input.GetTouch (i).position.x);
			}
		}
		jumpPowerSlider.value = currentJumpPower;

		/* GOD MODE

		float jumpLength = 36f;
		GameObject reallyClosest = null;
		GameObject closestPlatform = null;
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Platform")) {
			if (p.transform.position.x > transform.position.x && (closestPlatform == null ||
				(p.transform.position.x - transform.position.x) < (closestPlatform.transform.position.x - transform.position.x))) {
				closestPlatform = p;
			}
			if (reallyClosest == null || Mathf.Abs (p.transform.position.x - transform.position.x) < Mathf.Abs (reallyClosest.transform.position.x - transform.position.x)) {
				reallyClosest = p;
			}
		}

		float angle = Vector3.Angle (
			new Vector3 (transform.position.x, transform.position.y, transform.position.z),
			new Vector3 (closestPlatform.transform.position.x, closestPlatform.transform.position.y, closestPlatform.transform.position.z)
		);

		float xDiff = closestPlatform.transform.position.x - transform.position.x;
		float yDiff = Mathf.Abs(closestPlatform.transform.position.x - transform.position.z);

		angle = Mathf.Atan ((closestPlatform.transform.position.x - transform.position.x) / Mathf.Abs(closestPlatform.transform.position.z - transform.position.z)) * Mathf.Rad2Deg;

		if (closestPlatform.transform.position.z < transform.position.z) {
			angle = angle * -1f + 180f;
		}

		transform.rotation = Quaternion.AngleAxis ((angle - 90f), Vector3.up);

		bool onPlatform = false;
		if ((Mathf.Abs(reallyClosest.transform.position.y) - Mathf.Abs(transform.position.y)) < 6 && Vector3.Distance (transform.position, reallyClosest.transform.position) < 21) {
			onPlatform = true;
		}

		if (!onPlatform && (closestPlatform.transform.position.x - transform.position.x) > jumpLength / 2f) {
			print ("JUMP");
		}
		*/
	}

	void FixedUpdate ()
	{
		playerRB.AddForce (Vector3.down * 150f);

		if (currentJumpPower < maxJumpPower) {
			currentJumpPower += 0.4f;
		}
	}

	void Jump(float x) {
		if ((Time.time - prevJumpTime) < 0.2f || currentJumpPower < jumpPower) {
			return;
		}

		currentJumpPower = currentJumpPower - jumpPower < 0 ? 0 : currentJumpPower - jumpPower;
				
		playerRB.velocity = Vector3.zero;
		//playerRB.angularVelocity = Vector3.zero;

		float max = Screen.width / 2f;

		if (x > max) {
			x -= max;
		} else {
			x = x - max;
		}

		float maxAngle = 25f;
		float angle = x / max * maxAngle;

		Vector3 angleVelocity = Quaternion.AngleAxis (angle, Vector3.right) * Vector3.up;

		playerRB.AddForce (angleVelocity * jumpVelocity, ForceMode.Impulse);
		prevJumpTime = Time.time;
	}

	void OnEnable ()
	{		
		playerRB.isKinematic = false;
		run = true;
	}

	void OnDisable ()
	{
		currentJumpPower = maxJumpPower;
		playerRB.isKinematic = true;
		run = false;
	}

	public void changeJumpVelocity(float jumpVelocity) {
		this.jumpVelocity = jumpVelocity;
		jumpVelocityText.text = jumpVelocity.ToString ();
	}

	public void changeJumpPower(float jumpPower) {
		this.jumpPower = jumpPower;
		jumpPowerText.text = jumpPower.ToString ();
	}
}
