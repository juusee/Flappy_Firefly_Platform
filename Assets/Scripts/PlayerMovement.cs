using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public Slider JumpPowerSlider;
	public Text PointsDisplay;

	public Slider SetupJumpVelocitySlider;
	public Text SetupJumpVelocityText;
	public Slider SetupJumpPowerSlider;
	public Text SetupJumpPowerText;

	public static int Score = 0;

	Rigidbody PlayerRB;
	float JumpVelocity = 80f;
	float PrevJumpTime = 0f;
	float MaxJumpPower = 100f;
	float CurrentJumpPower = 100f;
	float JumpPower = 22f;

	void Awake ()
	{
		PlayerRB = GetComponent<Rigidbody>();
		PlayerRB.velocity = new Vector3 (10f, 0f, 0f);

		SetupJumpVelocitySlider.onValueChanged.AddListener(ChangeJumpVelocity);
		SetupJumpVelocitySlider.value = JumpVelocity;
		SetupJumpVelocityText.text = JumpVelocity.ToString ();
		SetupJumpPowerSlider.onValueChanged.AddListener(ChangeJumpPower);
		SetupJumpPowerSlider.value = JumpPower;
		SetupJumpPowerText.text = JumpPower.ToString ();
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
		JumpPowerSlider.value = CurrentJumpPower;

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
		PlayerRB.AddForce (Vector3.down * 150f);

		if (CurrentJumpPower < MaxJumpPower) {
			CurrentJumpPower += 0.4f;
		}
	}

	void Jump(float x) {
		if ((Time.time - PrevJumpTime) < 0.2f || CurrentJumpPower < JumpPower) {
			return;
		}

		CurrentJumpPower = CurrentJumpPower - JumpPower < 0 ? 0 : CurrentJumpPower - JumpPower;
				
		PlayerRB.velocity = Vector3.zero;
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

		PlayerRB.AddForce (angleVelocity * JumpVelocity, ForceMode.Impulse);
		PrevJumpTime = Time.time;
	}

	void OnEnable ()
	{		
		PlayerRB.isKinematic = false;
	}

	void OnDisable ()
	{
		CurrentJumpPower = MaxJumpPower;
		PlayerRB.isKinematic = true;
		Score = int.Parse(PointsDisplay.text);
		PointsDisplay.text = "0";
	}

	void OnCollisionEnter (Collision col)
	{
		// Todo improve
		int points = int.Parse (PointsDisplay.text) + 1;
		PointsDisplay.text = points.ToString ();
	}

	public void ChangeJumpVelocity(float jumpVelocity) {
		this.JumpVelocity = jumpVelocity;
		SetupJumpVelocityText.text = jumpVelocity.ToString ();
	}

	public void ChangeJumpPower(float jumpPower) {
		this.JumpPower = jumpPower;
		SetupJumpPowerText.text = jumpPower.ToString ();
	}
}
