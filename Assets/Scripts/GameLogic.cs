using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class GameLogic : MonoBehaviour {
	
	public GameObject PlatformSmall;
	public GameObject PlatformMedium;
	public GameObject PlatformBig;
	public GameObject TreeRock1;
	public GameObject TreeRock2;
	public GameObject TreeRock3;
	public GameObject TreeRock4;
	public GameObject TreeRock5;
	public Transform Player;
	public Transform PlatformSpawnPoint;
	public float PlatformBufferFront;

	List<GameObject> Platforms = new List<GameObject> ();
	List<GameObject> Trees = new List<GameObject> ();
	float GeneralPlatformLength;
	float PlatformGap = 60f;
	int PlatformBufferBack = 2;
	int PlatformCount = 0;
	Vector3 PrevPlatformPos;

	void Start ()
	{
		GeneralPlatformLength = PlatformMedium.transform.FindChild ("Platform").GetComponent<Renderer> ().bounds.size.x;
	}

	void Update ()
	{
		// todo set inactive when falling too much
		if (Player.transform.position.y < -300f) {
			Player.gameObject.SetActive (false);
		}

		float distanceToSpawnPlatform = Player.localPosition.x + PlatformBufferFront * (GeneralPlatformLength + PlatformGap);

		if (PlatformSpawnPoint.position.x < distanceToSpawnPlatform) {
			GameObject platform = GetPlatform ();
			float platformLength = platform.transform.FindChild ("Platform").GetComponent<Renderer> ().bounds.size.x;

			PlatformSpawnPoint.localPosition = new Vector3 (
				PlatformSpawnPoint.localPosition.x + platformLength + PlatformGap,
				PlatformSpawnPoint.localPosition.y,
				PlatformSpawnPoint.localPosition.z
			);

			platform.transform.localPosition = new Vector3 (
				PlatformSpawnPoint.localPosition.x,
				PrevPlatformPos.y + WeightedRandom(new Dictionary<float, float> () {{0, 1}, {5, 2}, {10, 3}, {15, 3}, {20, 6}, {25, 10}}),
				// if changed, set player jump max angle in PlayerMovement
				PrevPlatformPos.z + WeightedRandom(new Dictionary<float, float> () {{15, 1}, {20, 1}, {25, 1}, {30, 3}, {35, 3}, {45, 3}}, true) // Random.Range(-45f, 45f)
			);

			platform.SetActive (true);
			PrevPlatformPos = platform.transform.localPosition;
			++PlatformCount;

			SpawnTrees (6, new Vector3(
				platform.transform.localPosition.x,
				PrevPlatformPos.y,
				platform.transform.localPosition.z
			), platform.transform.FindChild ("Platform"));
		}
	}

	float WeightedRandom(Dictionary<float, float> valuesAndWeights, bool randomMinus = false) {
		float sumOfWeights = valuesAndWeights.Sum (pair => pair.Value);
		float random = Random.Range (0, sumOfWeights);
		float currentMin;
		float currentMax = 0;
		Dictionary<float, float> valuesAndWeightsOrdered = valuesAndWeights.OrderBy (pair => pair.Value).ToDictionary(p => p.Key, p => p.Value);
		foreach (KeyValuePair<float, float> pair in valuesAndWeightsOrdered) {
			currentMin = currentMax;
			currentMax += pair.Value;
			if (random > currentMin && random <= currentMax) {
				if (randomMinus) {
					return Random.value > 0.5f ? pair.Key : -pair.Key;
				}
				return pair.Key;
			}

		}
		return valuesAndWeightsOrdered.Last ().Key;
	}

	void SpawnTrees(float count, Vector3 platformPosition, Transform platform) {
		float platformLength = platform.GetComponent<Renderer> ().bounds.size.x;
		float platformWidth = platform.GetComponent<Renderer> ().bounds.size.z;
		float areaLength = platformLength + 40f;
		float areaWidth = 80f;
		float areaHeight = 40f;
		float treeStartXPos = platformPosition.x - areaLength / 2f;
		float treeStartYPos = platformPosition.y - 20f;
		float treeEndYPos = treeStartYPos + areaHeight;
		float treeStartZPos = platformWidth / 2 + 20f;
		float treeEndZPos = treeStartZPos + areaWidth;

		float slotLength = (areaLength) / count;
		for (int i = 0; i < count; ++i) {
			GameObject tree = GetTree ();
			float side = i % 2 == 0 ? -1 : 1;
			tree.transform.localPosition = new Vector3 (
				treeStartXPos + Random.Range(slotLength * i, slotLength * (i + 1)),
				Random.Range(treeStartYPos, treeEndYPos),
				platformPosition.z + Random.Range(treeStartZPos, treeEndZPos) * side
			);
			tree.SetActive (true);
		}
	}

	GameObject GetTree() {
		GameObject newTree = null;
		for (int i = 0; i < Trees.Count; ++i) {
			if (!Trees[i].activeSelf || Trees[i].transform.position.x < (Player.transform.position.x - PlatformBufferBack * GeneralPlatformLength)) {
				newTree = Trees [i];
				break;
			}
		}
		if (newTree == null) {
			float randomValue = Random.value;
			if (randomValue < 0.20f) {
				newTree = (GameObject) Instantiate (TreeRock1);
			} else if (randomValue < 0.40f) {
				newTree = (GameObject) Instantiate (TreeRock2);
			} else if (randomValue < 0.60f) {
				newTree = (GameObject) Instantiate (TreeRock3);
			} else if (randomValue < 0.80f) {
				newTree = (GameObject) Instantiate (TreeRock4);
			} else {
				newTree = (GameObject) Instantiate (TreeRock5);
			}
			Trees.Add (newTree);
		}
		newTree.SetActive (false);
		return newTree;
	}

	GameObject GetPlatform() {
		GameObject newPlatform = null;
		for (int i = 0; i < Platforms.Count; ++i) {
			if (!Platforms[i].activeSelf || Platforms[i].transform.position.x < (Player.transform.position.x - PlatformBufferBack * GeneralPlatformLength)) {
				newPlatform = Platforms [i];
				break;
			}
		}
		if (newPlatform == null) {
			float randomValue = Random.value;
			if (randomValue < 0.33f) {
				newPlatform = (GameObject) Instantiate (PlatformSmall);
			} else if (randomValue < 0.66f) {
				newPlatform = (GameObject) Instantiate (PlatformMedium);
			} else {
				newPlatform = (GameObject) Instantiate (PlatformBig);
			}
			Platforms.Add (newPlatform);
		}
		newPlatform.transform.name = newPlatform.transform.name + PlatformCount;
		newPlatform.SetActive (false);
		return newPlatform;
	}

	public void Reset ()
	{
		foreach (GameObject platform in Platforms) {
			Destroy (platform);
		}
		Platforms = new List<GameObject> ();
		foreach (GameObject tree in Trees) {
			Destroy (tree);
		}
		Trees = new List<GameObject> ();
		PlatformSpawnPoint.transform.position = new Vector3 (80f, 0, 0);
		PlatformCount = 0;
		// todo
		PrevPlatformPos = new Vector3 (55f, -9f, 0f);
		SpawnTrees (6, new Vector3(
			PlatformSpawnPoint.transform.localPosition.x,
			0,
			PlatformSpawnPoint.transform.localPosition.z
		), GameObject.Find("StartPlatform").transform.FindChild ("Platform"));
	}
}
