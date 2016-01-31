﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	// Очки за игру
	public int score = 0;
	// Пауза или нет
	public bool pause = false;
	// Количество очков за секунду на ожно дерево
	public int scoresPerSecond = 2;

	public PlanetManager planet;

	// 
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Добавляет очки
	public void AddScore(int count) {
		this.score += count * this.scoresPerSecond;
	}

	//
	public void SetPause(bool pause) {
		this.planet.SetPause (pause);
	}

}