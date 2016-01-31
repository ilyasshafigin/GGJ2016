using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour {
	public Button resumeBut;
	public Button restartBut;
	public Button pauseBut;
	public GameObject panelWithPause;
	public GameObject pausePan2;
	private bool paused;

	// Use this for initialization
	void Start () {
		resumeBut = resumeBut.GetComponent<Button>();
		restartBut = restartBut.GetComponent<Button> ();
		pauseBut = pauseBut.GetComponent<Button> ();
		panelWithPause.SetActive (true);
		pausePan2.SetActive (false);
		paused = false;
	}

	public void PausePress(){
		panelWithPause.SetActive (false);
		pausePan2.SetActive (true);
		paused = true;
		Time.timeScale = 0;
	}

	public void ResumePress(){
		panelWithPause.SetActive (true);
		pausePan2.SetActive (false);
		paused = false;
		Time.timeScale = 1;
	}

	public void RestartPress(){
		Application.LoadLevel (0);
		paused = false;
		Time.timeScale = 1;
	}
}
