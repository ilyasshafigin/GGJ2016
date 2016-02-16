﻿using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Префаб эффекта взрыва
	public static Object prefab = Resources.Load ("Prefabs/Explosion");

	// Прогресс взрыва от 0 до 1
	private float exp = 0.0f;
	// Скорость взрыва
	public float expSpeed = 1f;

	//
	private Planet planet;
	// Аниматор
	private Animator animator;

	// Количество каждов анимации
	public int animFrames = 16; 

	// Инициализация
	private void Awake () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
	}

	// Инициализация при создании префаба
	private void Initialize(Planet planet) {
		this.planet = planet;
	}

	// Устанавливает прогресс анимации
	private void SetProgress(float progress) {
		this.animator.Play ("Explosion", 0, progress);
	}

	//
	public void OnExplosion() {
		this.exp = 0.0f;
		this.gameObject.SetActive (true);
	}

	private void Update() {
		// Увеличиваем прогресс
		this.exp += this.expSpeed * Time.deltaTime;
		// Если взрыв закончился
		if (this.exp >= 1.0f) {
			this.exp = 1.0f;
			//
			this.gameObject.SetActive(false);
		}
		// Управляем анимацией
		this.SetProgress (this.exp);
	}

	// Создает объект эффекта
	public static Explosion CreateExplosion(Planet planet) {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		Explosion explosion = gameObject.GetComponent<Explosion> ();
		explosion.Initialize (planet);
		return explosion;
	}
}