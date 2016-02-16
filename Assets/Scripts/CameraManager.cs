using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {
    public GameObject StartMonitor;
    public GameObject GameMonitor;
    public GameObject FinalMonitor;
    public Canvas CanvasMenu;

	public AudioClip backgroundSound;
	public AudioClip startSound;
	public AudioClip stopSound;

    private byte Monitor = 0;
	private AudioSource audio;

    void Start() {
		Vector3 sm = GameObject.Find ("StartMonitor").transform.position;
		transform.position = new Vector3 (sm.x,sm.y,-10);
        StartMonitor.SetActive(true);
        GameMonitor.SetActive(false);
        FinalMonitor.SetActive(false);
        CanvasMenu.enabled = false;
		this.audio = this.GetComponent<AudioSource> ();
    }
    
    void Update() {
		// Событие перехода на экран игры
        if (Input.GetKeyDown(KeyCode.Mouse0) && Monitor==0){
            Monitor = 1;
            StartMonitor.SetActive(false);
            GameMonitor.SetActive(true);
			// Запускаем звук
			this.audio.loop = false;
			this.audio.clip = this.startSound;
			this.audio.Play ();
        }
		// Переход на экран игры
        if (Monitor == 1){
            CanvasMenu.enabled = true;
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), 0.05f);
        }
		// Переход на экран проигрыша
        if (Monitor == 2){
            CanvasMenu.enabled = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(22, 0, -10), 0.05f);
        }
    }

	// Событие окончания игры
    public void GameOverInCamera() {
        Monitor = 2;
        GameMonitor.SetActive(false);
        FinalMonitor.SetActive(true);
		// Запускаем звук
		this.audio.loop = false;
		this.audio.clip = this.stopSound;
		this.audio.Play ();
    }
}
