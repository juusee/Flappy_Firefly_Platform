using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public GameObject MenuPanel;
	public GameObject PlayPanel;
	public GameObject SetupPanel;

	public Button PlayButton;
	public Button SetupButton;
	public Button HomeButton;

	public GameObject StartCounter;

	// Use this for initialization
	void Awake () {
		Reset ();
		SetupButton.onClick.AddListener (() => ShowSetup(true));
		HomeButton.onClick.AddListener (() => ShowSetup(false));
		PlayButton.onClick.AddListener (OnPlayButtonClick);
	}

	void ShowSetup (bool show) {
		MenuPanel.SetActive (!show);
		SetupPanel.SetActive (show);
	}

	void OnPlayButtonClick () {
		MenuPanel.SetActive (false);
		PlayPanel.SetActive (true);
		StartCounter.SetActive (true);
		StartCoroutine (CountStart (3));
	}

	IEnumerator CountStart (int count)
	{
		WaitForSeconds countWait = new WaitForSeconds (0.8f);
		StartCounter.GetComponentInChildren<Text> ().text = count.ToString();
		yield return countWait;
		if (--count > 0) {
			yield return CountStart (count);
		}
		StartCounter.SetActive (false);
		GameManager.StartGame ();
	}

	public void Reset() {
		MenuPanel.SetActive (true);
		PlayPanel.SetActive (false);
		SetupPanel.SetActive (false);
	}
}
