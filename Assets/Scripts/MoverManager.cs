using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverManager : MonoBehaviour {

	public GameObject moverInstance;

	Vector3 moverSpawnPoint;
	Mover mover;

	void OnEnable() {
		moverSpawnPoint = new Vector3(0f, 0f, 0f);
		mover = moverInstance.GetComponent<Mover> ();

	}

	public void Reset()
	{
		moverInstance.transform.position = moverSpawnPoint;
		moverInstance.SetActive (false);
		moverInstance.SetActive (true);
	}

	public void DisableControl()
	{
		mover.enabled = false;
	}

	public void EnableControl()
	{
		mover.enabled = true;
	}
}