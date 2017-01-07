using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverManager : MonoBehaviour {

	public GameObject MoverInstance;

	Vector3 MoverSpawnPoint;
	Mover Mover;

	void OnEnable() {
		MoverSpawnPoint = new Vector3(0f, 0f, 0f);
		Mover = MoverInstance.GetComponent<Mover> ();

	}

	public void Reset()
	{
		MoverInstance.transform.position = MoverSpawnPoint;
		MoverInstance.SetActive (false);
		MoverInstance.SetActive (true);
	}

	public void DisableControl()
	{
		Mover.enabled = false;
	}

	public void EnableControl()
	{
		Mover.enabled = true;
	}
}