using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float endDelay = 2.5f;
	public PlayerManager playerManager;
	public MoverManager moverManager;
	public GameLogic gameLogic;
	public GameObject panel;
	public GameObject startCounter;

	WaitForSeconds endWait;
	bool start = false;

	public void startGame()
	{
		start = true;
	}

	void Start ()
	{
		endWait = new WaitForSeconds (endDelay);

		StartCoroutine (GameLoop());
	}

	IEnumerator GameLoop ()
	{
		playerManager.DisableControl ();
		playerManager.Reset ();
		moverManager.DisableControl ();
		moverManager.Reset ();

		panel.SetActive (true);
		yield return StartCoroutine (RoundStarting());
		panel.SetActive (false);

		startCounter.SetActive (true);
		yield return StartCoroutine (StartCounter (3));
		startCounter.SetActive (false);
		yield return StartCoroutine (RoundPlaying());
		yield return StartCoroutine (RoundEnding());
		StartCoroutine (GameLoop ());
	}

	IEnumerator RoundStarting ()
	{
		while (!start) {
			yield return null;
		}

		gameLogic.Reset ();
		gameLogic.GetComponent<GameLogic> ().enabled = true;

		start = false;
	}

	IEnumerator StartCounter (int count)
	{
		WaitForSeconds countWait = new WaitForSeconds (0.8f);
		startCounter.GetComponentInChildren<Text> ().text = count.ToString();
		yield return countWait;
		--count;
		if (count > 0) {
			yield return StartCounter (count);
		}
	}

	IEnumerator RoundPlaying ()
	{
		playerManager.EnableControl ();
		moverManager.EnableControl ();

		while (playerManager.playerInstance.activeSelf) {
			yield return null;
		}
	}

	IEnumerator RoundEnding ()
	{
		playerManager.DisableControl ();
		moverManager.DisableControl ();
		yield return endWait;
	}
}