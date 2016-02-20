using UnityEngine;
using System.Collections;

public class Grow : MonoBehaviour {

	// Префаб эффекта полного роста
	public static Object prefab = Resources.Load ("Prefabs/Grow");

	// Прогресс от 0 до 1
	private float grow = 0.0f;
	// Скорость
	public float growSpeed = 1f;

	// Планета
	private Planet planet;
	// Аниматор
	private Animator animator;

	// Инициализация
	private void Awake () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
	}

	// Инициализация при создании префаба
	private void Initialize(Planet planet) {
		this.planet = planet;
	}

	private void Update() {
		// Увеличиваем прогресс
		this.grow += this.growSpeed * Time.deltaTime;
		// Если взрыв закончился
		if (this.grow >= 1.0f) {
			this.grow = 1.0f;
			// Делаем объект неактивным
			this.gameObject.SetActive(false);
		}
		// Управляем анимацией
		this.SetProgress (this.grow);
	}

	// Устанавливает прогресс анимации
	public void SetProgress(float progress) {
		this.animator.Play ("Grow", 0, progress);
	}

	//
	public void OnGrow() {
		this.grow = 0.0f;
		this.gameObject.SetActive (true);
	}

	// Событие запуска игры
	public void OnStartGame() {

	}

	// Событие окончания игры
	public void OnGameOver() {

	}

	// Создает объект эффекта
	public static Grow CreateGrow(Planet planet) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		Grow grow = gameObject.GetComponent<Grow> ();
		grow.Initialize (planet);
		return grow;
	}

}
