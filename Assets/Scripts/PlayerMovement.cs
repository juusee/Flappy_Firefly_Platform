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
	float velocity = 30f;
	float JumpVelocity = 80f;
	float PrevJumpTime = 0f;
	float MaxJumpPower = 100f;
	float CurrentJumpPower = 100f;
	float JumpPower = 22f;

	float fallAcceleration = 3f;
	float fallTerminalVelocity = 100f;

	GameObject lastPlatform;

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
	}

	void FixedUpdate ()
	{
		if (-PlayerRB.velocity.y < fallTerminalVelocity) {
			PlayerRB.velocity = new Vector3(velocity, PlayerRB.velocity.y - fallAcceleration, PlayerRB.velocity.z);
		}

		if (CurrentJumpPower < MaxJumpPower) {
			CurrentJumpPower += 0.4f;
		}
	}

	void Jump(float x) {
		if ((Time.time - PrevJumpTime) < 0.2f || CurrentJumpPower < JumpPower) {
			return;
		}

		CurrentJumpPower = CurrentJumpPower - JumpPower < 0 ? 0 : CurrentJumpPower - JumpPower;
				
		PlayerRB.velocity = new Vector3 (PlayerRB.velocity.x, 0, 0);

		float max = Screen.width / 2f;

		if (x > max) {
			x -= max;
		} else {
			x = x - max;
		}

		float maxAngle = 25f;
		float angle = x / max * maxAngle;

		Vector3 angleVelocity = Quaternion.AngleAxis (angle, Vector3.right) * Vector3.up;

		PlayerRB.AddForce (angleVelocity * JumpVelocity, ForceMode.VelocityChange);
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
		if (col.gameObject.tag == "Platform" && lastPlatform != col.gameObject) {
			int points = int.Parse (PointsDisplay.text) + 1;
			PointsDisplay.text = points.ToString ();
			lastPlatform = col.gameObject;
		}
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
