using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private enum Screen {
		Menu, 		// экран меню
		Game,		// экран игры
		GameOver	// экран проигрыша
	}

	public static GameManager instance = null;
    public CameraManager mainCamera;

	public GameObject menuScreen;
	public GameObject gameScreen;
	public GameObject gameOverScreen;
	public PauseMenuManager pauseMenu;

	// Фоновый звук
	public AudioClip backgroundSound;
	// Звук начала игры
	public AudioClip startSound;
	// Звук окончания игры
	public AudioClip gameOverSound;
	// Планета
	public Planet planet;
	// Диспетчер эффектов
	public EffectsManager effects;
	// Текст с очками
	public Text scoreText;
	// Коэффициент сглаживания перехода камеры
	public float easing = 0.05f;

	// Очки за игру
	public int score = 0;
	// Пауза или нет
	private bool pause = false;
	// Количество очков за секунду на ожно дерево
	private int scoresPerSecond = 2;
	// Конец игры
	private bool gameOver = true;
	// Текущий экран
	private Screen screen;
	// Перемещается ли камера на данный момент
	private bool animated = false;

	private AudioSource audioSource;

	// Инициализация
	private void Start () {
		instance = this;

		this.screen = Screen.Menu;
		this.mainCamera.transform.position = new Vector3 (this.menuScreen.transform.position.x, this.menuScreen.transform.position.y, this.mainCamera.transform.position.z);
		//this.menuScreen.SetActive(true);
		//this.gameScreen.SetActive(false);
		//this.gameOverScreen.SetActive(false);
		this.pauseMenu.gameObject.SetActive(false);
		this.audioSource = this.GetComponent<AudioSource> ();
	}
	
	// Обновление на каждом кадре
	private void Update () {
		this.scoreText.text = score.ToString ();

		if (this.screen == Screen.Menu) {
			// Событие перехода на экран игры
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				this.GotoGameScreen ();
			} else
			// Переход на экран игры
			if (this.animated) {
				Vector3 pos = new Vector3 (this.menuScreen.transform.position.x, this.menuScreen.transform.position.y, this.mainCamera.transform.position.z);
				this.mainCamera.transform.position = Vector3.Lerp(this.mainCamera.transform.position, pos, this.easing);
				if (this.mainCamera.transform.position.Equals (pos)) {
					this.animated = false;
				}
			}
		}
			
		if (this.screen == Screen.Game) {
			// Переход на экран игры
			if (this.animated) {
				Vector3 pos = new Vector3 (this.gameScreen.transform.position.x, this.gameScreen.transform.position.y, this.mainCamera.transform.position.z);
				this.mainCamera.transform.position = Vector3.Lerp(this.mainCamera.transform.position, pos, this.easing);
				if (this.mainCamera.transform.position.Equals (pos)) {
					this.animated = false;
				}
			}
		}
			
		if (this.screen == Screen.GameOver) {
			// Событие перехода на экран игры
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				this.Restart ();
			} else
			// Переход на экран проигрыша
			if (this.animated) {
				Vector3 pos = new Vector3 (this.gameOverScreen.transform.position.x, this.gameOverScreen.transform.position.y, this.mainCamera.transform.position.z);
				this.mainCamera.transform.position = Vector3.Lerp (this.mainCamera.transform.position, pos, this.easing);
				if (this.mainCamera.transform.position.Equals (pos)) {
					this.animated = false;
				}
			}
		}
	}

	// Событие перехода на экран игры
	private void GotoGameScreen() {
		this.screen = Screen.Game;
		this.animated = true;
		this.menuScreen.SetActive(false);
		this.gameOverScreen.SetActive (false);
		this.gameScreen.SetActive(true);
		this.pauseMenu.gameObject.SetActive (true);

		// Запускаем звук
		this.audioSource.loop = false;
		this.audioSource.clip = this.startSound;
		this.audioSource.Play ();

		// Запускаем событие
		this.OnStartGame();
	}

	// Событие перехода на экран окончания игры
	private void GotoGameOverScreen() {
		this.screen = Screen.GameOver;
		this.animated = true;
		this.menuScreen.SetActive(false);
		this.gameOverScreen.SetActive (true);
		this.gameScreen.SetActive(false);
		this.pauseMenu.gameObject.SetActive (false);

		// Запускаем звук
		this.audioSource.loop = false;
		this.audioSource.clip = this.gameOverSound;
		this.audioSource.Play ();
	}

	// Событие переходна на экран меню
	private void GotoMenuScreen() {
		this.screen = Screen.Menu;
		this.animated = true;
		this.menuScreen.SetActive(true);
		this.gameOverScreen.SetActive (false);
		this.gameScreen.SetActive(false);
		this.pauseMenu.gameObject.SetActive (false);

		// Запускаем звук
		this.audioSource.loop = false;
		this.audioSource.clip = this.startSound;
		this.audioSource.Play ();
	}

	// Добавляет очки
	public void AddScore(int count) {
		this.score += count * this.scoresPerSecond;
	}

	//
	public void SetPause(bool pause) {
		
	}

	// Событие начала игры
	private void OnStartGame() {
		this.gameOver = false;
		this.pauseMenu.OnStartGame ();
		this.planet.OnStartGame ();
		this.effects.OnStartGame ();
		this.score = 0;
	}
		
	// Конец игры
	public void GameOver() {
		this.GotoGameOverScreen();

		this.gameOver = true;
		this.pauseMenu.OnGameOver ();
		this.planet.OnGameOver ();
		this.effects.OnGameOver ();
	}

	//
	public bool IsGameOver() {
		return this.gameOver;
	}

	// Перезапуск игры
	public void Restart() {
		this.gameOver = false;
		this.pauseMenu.OnGameOver ();
		this.planet.OnGameOver ();
		this.effects.OnGameOver ();

		this.GotoMenuScreen ();
	}

}
