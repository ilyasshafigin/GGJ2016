using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour {
	
	public Button resumeBut;
	public Button restartBut;
	public Button pauseBut;
	public GameObject panelWithPause;
	public GameObject panelWithResume;
	private bool paused;

	// Use this for initialization
	void Start () {
		//panelWithPause.SetActive (false);
		//panelWithResume.SetActive (false);
		paused = false;
	}

	public void PausePress() {
		panelWithPause.SetActive (false);
		panelWithResume.SetActive (true);
		paused = true;
		Time.timeScale = 0;
	}

	public void ResumePress() {
		panelWithPause.SetActive (true);
		panelWithResume.SetActive (false);
		paused = false;
		Time.timeScale = 1;
	}

	public void RestartPress() {
		//Application.LoadLevel (0);
		GameManager.instance.Restart();
		panelWithPause.SetActive (false);
		panelWithResume.SetActive (false);
		paused = false;
		Time.timeScale = 1;
	}

	// Событие запуска игры
	public void OnStartGame() {
		panelWithPause.SetActive (true);
		panelWithResume.SetActive (false);
	}

	// Событие окончания игры
	public void OnGameOver() {
		panelWithPause.SetActive (false);
		panelWithResume.SetActive (false);
	}

}
