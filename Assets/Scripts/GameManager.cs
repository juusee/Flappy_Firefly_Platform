using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public float endDelay = 2.5f;
	public PlayerManager playerManager;
	public MoverManager moverManager;
	public GameLogic gameLogic;
	public GameObject menuPanel;
	public GameObject playingPanel;
	public GameObject setupPanel;
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
		gameLogic.GetComponent<GameLogic> ().enabled = true;
		gameLogic.Reset ();

		yield return StartCoroutine (RoundStarting());
		yield return StartCoroutine (StartCounter (3));
		yield return StartCoroutine (RoundPlaying());
		yield return StartCoroutine (RoundEnding());
		StartCoroutine (GameLoop ());
	}

	IEnumerator RoundStarting ()
	{		
		playingPanel.SetActive (false);
		setupPanel.SetActive (false);
		menuPanel.SetActive (true);

		while (!start) {
			yield return null;
		}

		menuPanel.SetActive (false);
		playingPanel.SetActive (true);
		startCounter.SetActive (true);

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
		startCounter.SetActive (false);
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

	public void ShowSetup(bool show) {
		menuPanel.SetActive (!show);
		setupPanel.SetActive (show);
	}
}
