using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public PlayerManager PlayerManager;
	public CanvasManager CanvasManager;
	public GameLogic GameLogic;

	public Text highScoreText;

	float endDelay = 2.5f;
	WaitForSeconds endWait;
	Data data = new Data();
	static bool start = false;


	void Start ()
	{
		endWait = new WaitForSeconds (endDelay);

		Data d = DataStorage.LoadFromFile<Data> ("data");
		int highScoreVal = 0;
		if (d != null) {
			data = d;
			highScoreVal = d.highScore;
		}
		highScoreText.text = highScoreVal.ToString ();
		StartCoroutine (GameLoop());
	}

	IEnumerator GameLoop ()
	{
		CanvasManager.Reset ();
		PlayerManager.DisableControl ();
		PlayerManager.Reset ();
		GameLogic.GetComponent<GameLogic> ().enabled = true;
		GameLogic.Reset ();

		yield return StartCoroutine (RoundStarting());
		yield return StartCoroutine (RoundPlaying());
		yield return StartCoroutine (RoundEnding());
		StartCoroutine (GameLoop ());
	}

	IEnumerator RoundStarting ()
	{
		while (!start) {
			yield return null;
		}

		start = false;
	}

	IEnumerator RoundPlaying ()
	{
		PlayerManager.EnableControl ();

		while (PlayerManager.PlayerInstance.activeSelf) {
			yield return null;
		}
	}

	IEnumerator RoundEnding ()
	{
		PlayerManager.DisableControl ();
		if (PlayerMovement.Score > data.highScore) {
			highScoreText.text = PlayerMovement.Score.ToString ();
			data.highScore = PlayerMovement.Score;
			DataStorage.SaveToFile ("data", data);
		}
		yield return endWait;
	}

	public static void StartGame() {
		start = true;	
	}
}

class Data {
	public int highScore = 0;
}
