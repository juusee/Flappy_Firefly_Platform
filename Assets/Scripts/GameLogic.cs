using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public Transform mover;
	public GameObject platformSmall;
	public GameObject platformMedium;
	public GameObject platformBig;
	public GameObject treeRock1;
	public GameObject treeRock2;
	public Transform player;
	public Transform platformSpawnPoint;
	public float platformBufferFront;
	public Text pointsDisplay;

	List<GameObject> platforms = new List<GameObject> ();
	List<GameObject> trees = new List<GameObject> ();
	float platformLength;
	float platformGap = 60f;
	int platformBufferBack = 2;
	int platformCount = 0;
	Vector3 prevPlatformPos;

	void Start ()
	{
		platformLength = platformBig.transform.FindChild("Platform").GetComponent<Renderer> ().bounds.size.x;
	}

	void Update ()
	{
		pointsDisplay.text = "" + Mathf.Floor (mover.transform.localPosition.x / -10);
		// todo set inactive when falling too much
		if (player.transform.position.y < -300f) {
			player.gameObject.SetActive (false);
		}

		float distanceToSpawnPlatform = platformBufferFront * (platformLength + platformGap);

		if (platformSpawnPoint.position.x < distanceToSpawnPlatform) {
			platformSpawnPoint.localPosition = new Vector3 (
				platformSpawnPoint.localPosition.x + platformLength + platformGap,
				platformSpawnPoint.localPosition.y,
				platformSpawnPoint.localPosition.z
			);

			GameObject platform = getPlatform ();
			platform.transform.parent = mover;
			platform.transform.localPosition = new Vector3 (
				platformSpawnPoint.localPosition.x,
				prevPlatformPos.y + Random.Range(-25f, 40f),
				prevPlatformPos.z + Random.Range(-45f, 45f)
			);

			platform.SetActive (true);
			prevPlatformPos = platform.transform.localPosition;
			++platformCount;

			GameObject tree1 = getTree ();
			tree1.transform.parent = mover;
			// Todo count platform width so tree wont get trough platform
			float random = Random.value < 0.5 ? Random.Range (-50f, -35f) : Random.Range (30f, 45f);
			tree1.transform.localPosition = new Vector3 (
				platformSpawnPoint.localPosition.x + Random.Range(-25f, 25f),
				platform.transform.localPosition.y + Random.Range(-25f, 25f),
				platform.transform.localPosition.z + random
			);
			tree1.SetActive (true);

			GameObject tree2 = getTree ();
			tree2.transform.parent = mover;
			tree2.transform.localPosition = new Vector3 (
				platformSpawnPoint.localPosition.x + Random.Range(-25f, 25f),
				platform.transform.localPosition.y + Random.Range(-25f, 25f),
				platform.transform.localPosition.z + random * -1 + Random.Range(0, 10f)
			);
			tree2.SetActive (true);

			GameObject tree3 = getTree ();
			tree3.transform.parent = mover;
			tree3.transform.localPosition = new Vector3 (
				platformSpawnPoint.localPosition.x + Random.Range(-25f, 25f),
				platform.transform.localPosition.y + Random.Range(-25f, 25f),
				platform.transform.localPosition.z + random * -1 + Random.Range(0, 10f)
			);
			tree3.SetActive (true);
		}
	}

	GameObject getTree() {
		GameObject newTree = null;
		for (int i = 0; i < trees.Count; ++i) {
			if (!trees[i].activeSelf || trees[i].transform.position.x < (player.transform.position.x - platformBufferBack * platformLength)) {
				newTree = trees [i];
				break;
			}
		}
		if (newTree == null) {
			float randomValue = Random.value;
			if (randomValue < 0.5f) {
				newTree = (GameObject) Instantiate (treeRock1);
			} else {
				newTree = (GameObject) Instantiate (treeRock2);
			}
			trees.Add (newTree);
		}
		newTree.SetActive (false);
		return newTree;
	}

	GameObject getPlatform() {
		GameObject newPlatform = null;
		for (int i = 0; i < platforms.Count; ++i) {
			if (!platforms[i].activeSelf || platforms[i].transform.position.x < (player.transform.position.x - platformBufferBack * platformLength)) {
				newPlatform = platforms [i];
				break;
			}
		}
		if (newPlatform == null) {
			float randomValue = Random.value;
			if (randomValue < 0.33f) {
				newPlatform = (GameObject) Instantiate (platformSmall);
			} else if (randomValue < 0.66f) {
				newPlatform = (GameObject) Instantiate (platformMedium);
			} else {
				newPlatform = (GameObject) Instantiate (platformBig);
			}
			platforms.Add (newPlatform);
		}
		newPlatform.transform.name = newPlatform.transform.name + platformCount;
		newPlatform.SetActive (false);
		return newPlatform;
	}

	void OnDisable() {
		pointsDisplay.text = "" + 0;
	}

	public void Reset ()
	{
		foreach (GameObject platform in platforms) {
			Destroy (platform);
		}
		platforms = new List<GameObject> ();
		foreach (GameObject tree in trees) {
			Destroy (tree);
		}
		trees = new List<GameObject> ();
		platformSpawnPoint.transform.position = new Vector3 (80f, 0, 0);
		platformCount = 0;
		// todo
		prevPlatformPos = new Vector3 (55f, -9f, 0f);
	}
}
