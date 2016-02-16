using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
    public CameraManager camera;

	// Очки за игру
	public int score = 0;
	// Пауза или нет
	public bool pause = false;
	// Количество очков за секунду на ожно дерево
	public int scoresPerSecond = 2;
	//
	public bool gameOver = false;

	public Planet planet;
	public Text text;

	// 
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = score.ToString ();
	}

	// Добавляет очки
	public void AddScore(int count) {
		this.score += count * this.scoresPerSecond;
	}

	//
	public void SetPause(bool pause) {
		
	}
		
	//
	public void GameOver() {
		this.camera.GameOverInCamera();
	}

}
