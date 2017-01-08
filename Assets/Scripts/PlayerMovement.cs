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

	float acceleration = 3f;
	float terminalVelocity = 100f;

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
		if (-PlayerRB.velocity.y < terminalVelocity) {
			PlayerRB.AddForce (Vector3.down * acceleration, ForceMode.VelocityChange);
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
				
		PlayerRB.velocity = Vector3.zero;
		// playerRB.angularVelocity = Vector3.zero;

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
