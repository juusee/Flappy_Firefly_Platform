using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CameraMovement : MonoBehaviour {

	public float smooth = 2.0F;
	public float tiltAngle = 30.0F;
	public Transform player;

	Vector3 playerOffset;

	void Start ()
	{
		playerOffset = new Vector3 (transform.position.x - player.position.x, transform.position.y - player.position.y, transform.position.z - player.position.z);
	}

	void Update()
	{
		float tiltAroundZ = CrossPlatformInputManager.GetAxis("Horizontal") * tiltAngle;
		Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -tiltAroundZ);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

		if (player != null) {
			transform.position = new Vector3(player.position.x, player.position.y, player.position.z) + playerOffset;
		}
	}
}
