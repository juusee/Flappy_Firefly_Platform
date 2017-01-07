using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject PlayerInstance;

	Vector3 PlayerSpawnPoint;
	PlayerMovement PlayerMovement;

	void OnEnable() {
		PlayerSpawnPoint = new Vector3(0f, 0f, 0f);
		PlayerMovement = PlayerInstance.GetComponent<PlayerMovement> ();
	
	}

	public void Reset()
	{
		PlayerInstance.transform.position = PlayerSpawnPoint;
		PlayerInstance.SetActive(false);
		PlayerInstance.SetActive(true);
	}

	public void DisableControl()
	{
		PlayerMovement.enabled = false;
	}

	public void EnableControl()
	{
		PlayerMovement.enabled = true;
	}
}
