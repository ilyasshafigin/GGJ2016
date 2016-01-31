using UnityEngine;
using System.Collections;

public class GrowManager : MonoBehaviour {

	// Префаб эффекта полного роста
	public static Object prefab = Resources.Load ("Prefabs/Grow");

	// Прогресс от 0 до 1
	private float grow = 0.0f;
	// Скорость
	public float growSpeed = 1f;

	// 
	private PlanetManager planet;
	// Аниматор
	private Animator animator;

	// Количество каждов анимации
	public int animFrames = 7; 

	// Инициализация
	private void Awake () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
	}

	// Инициализация при создании префаба
	private void Initialize(PlanetManager planet) {
		this.planet = planet;
	}
		
	// Устанавливает прогресс анимации
	public void SetProgress(float progress) {
		this.animator.Play ("Grow", 0, progress);
	}

	//
	public void Grow() {
		this.grow = 0.0f;
		this.gameObject.SetActive (true);
	}

	private void Update() {
		// Увеличиваем прогресс
		this.grow += this.growSpeed * Time.deltaTime;
		// Если взрыв закончился
		if (this.grow >= 1.0f) {
			this.grow = 1.0f;
			//
			this.gameObject.SetActive(false);
		}
		// Управляем анимацией
		this.SetProgress (this.grow);
	}


	// Создает объект эффекта
	public static GrowManager CreateGrow(PlanetManager planet) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		GrowManager grow = gameObject.GetComponent<GrowManager> ();
		grow.Initialize (planet);
		return grow;
	}

}
