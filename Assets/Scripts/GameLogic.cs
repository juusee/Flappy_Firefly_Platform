using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

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
	float platformWidth;
	float platformGap = 60f;
	int platformBufferBack = 2;
	int platformCount = 0;
	Vector3 prevPlatformPos;

	void Start ()
	{
		platformLength = platformBig.transform.FindChild("Platform").GetComponent<Renderer> ().bounds.size.x;
		platformWidth = platformBig.transform.FindChild("Platform").GetComponent<Renderer> ().bounds.size.z;
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
				prevPlatformPos.y + weightedRandom(new Dictionary<float, float> () {{0, 1}, {5, 2}, {10, 3}, {15, 3}, {20, 6}, {25, 10}}),
				// if changed, set player jump max angle in PlayerMovement
				prevPlatformPos.z + weightedRandom(new Dictionary<float, float> () {{15, 1}, {20, 1}, {25, 1}, {30, 3}, {35, 3}, {45, 3}}, true) // Random.Range(-45f, 45f)
			);

			platform.SetActive (true);
			prevPlatformPos = platform.transform.localPosition;
			++platformCount;

			SpawnTrees (4, new Vector3(
				platform.transform.localPosition.x,
				prevPlatformPos.y,
				platform.transform.localPosition.z
			));
		}
	}

	float weightedRandom(Dictionary<float, float> valuesAndWeights, bool randomMinus = false) {
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

	void SpawnTrees(float count, Vector3 platformPosition) {
		float areaLength = platformLength + 40f;
		float areaWidth = 40f;
		float areaHeight = 30f;
		float treeStartXPos = platformPosition.x - areaLength / 2f;
		float treeStartYPos = platformPosition.y - 20f;
		float treeEndYPos = platformPosition.y + 20f;
		float treeStartZPos = platformWidth / 2 + 20f;
		float treeEndZPos = treeStartZPos + areaWidth;

		float slotLength = (areaLength) / count;
		for (int i = 0; i < count; ++i) {
			GameObject tree = getTree ();
			tree.transform.parent = mover;
			float side = i % 2 == 0 ? -1 : 1;
			tree.transform.localPosition = new Vector3 (
				treeStartXPos + Random.Range(slotLength * i, slotLength * (i + 1)),
				Random.Range(treeStartYPos, treeEndYPos),
				platformPosition.z + Random.Range(treeStartZPos, treeEndZPos) * side
			);
			tree.SetActive (true);
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
