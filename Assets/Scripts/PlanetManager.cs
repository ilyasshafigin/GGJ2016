﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour {

	// Количество растений/секторов
	public int count = 36;
	// Радиус планеты для определения положения растений
	public float radius;
	// Минимальное время появления растений
	public float minSpawnTime = 3;
	// Максимальное время появления растений
	public float maxSpawnTime = 5;

	// Массив растений
	private PlantManager[] plants;
	// Массив эффектор привлечения внимания
	private GrowManager[] grows;
	// Массив эффектов взрыва
	private ExplosionManager[] explosions;
	// Массив температур, максимум - 1
	private float[] temps;
	// Значение повышения температуры за один кадр
	private float tempSpeed = 0.01f;
	// Угол одного сетора
	private float sectorAngle;

	// Инициализация объекта
	private void Start () {
		this.plants = new PlantManager[this.count];
		this.explosions = new ExplosionManager[this.count];
		this.grows = new GrowManager[this.count];
		this.temps = new float[this.count];

		this.sectorAngle = 360f / this.count;

		for (int i = 0; i < this.count; i++) {
			float angle = i * this.sectorAngle;
			float x = this.radius * Mathf.Cos (angle * Mathf.Deg2Rad);
			float y = this.radius * Mathf.Sin (angle * Mathf.Deg2Rad);

			Vector3 translation = new Vector3 (x, y, 0);
			Vector3 rotation = new Vector3 (0, 0, angle - 90);

			PlantManager plant = PlantManager.CreatePlant (this, i);
			plant.transform.SetParent (this.transform);
			plant.transform.Translate (translation);
			plant.transform.Rotate (rotation);
			this.plants[i] = plant;

			ExplosionManager explosion = ExplosionManager.CreateExplosion (this);
			explosion.gameObject.SetActive(false);
			explosion.transform.SetParent (this.transform);
			explosion.transform.Translate (translation);
			explosion.transform.Rotate (rotation);
			this.explosions[i] = explosion;

			GrowManager grow = GrowManager.CreateGrow (this);
			grow.gameObject.SetActive(false);
			grow.transform.SetParent (this.transform);
			grow.transform.Translate (translation);
			grow.transform.Rotate (rotation);
			this.grows[i] = grow;
		}

		// Запускаем спаун первых растений
		int count = 2;
		while(count-- > 0) {
			PlantManager plant = this.Spawn ();
			if (plant != null) {
				// Изначально, дерево выросло на половину
				plant.SetGrowth (0.5f);
			}
		}
	}

	// Спаунит растение на свободном сеторе
	public PlantManager Spawn() {
		List<int> sectors = this.GetAvaliableSectors ();
		if (sectors.Count > 0) {
			int index = sectors [(int)Random.Range (0, sectors.Count - 1)];
			PlantManager plant = this.plants [index];
			plant.SetType (PlantManager.Type.Tree);
			plant.SetState (PlantManager.State.Growth);
			return plant;
		} else {
			return null;
		}
	}

	// Возвращает список свободных секторов
	private List<int> GetAvaliableSectors() {
		List<int> sectors = new List<int> ();
		for (int i = 0; i < this.count; i++) {
			PlantManager plant = this.plants [i];
			if (plant.IsNone ()) {
				sectors.Add (i);
			}
		}
		return sectors;
	}
	
	// Обновление объекта на каждом кадре
	private void Update () {
		int near = this.GetNearSector ();
		int far = near + this.count / 2;
		if (far >= this.count)
			far -= this.count;
		int area = this.count / 4;
		bool[] sun = new bool[this.count];

		this.temps [near] += this.tempSpeed;
		sun [near] = true;
		for (int i = 1; i <= area; i++) {
			int l = near - i;
			int r = near + i;

			float c = Mathf.Sqrt(1.0f - (float) i / area);

			if (l < 0)
				l += this.count;
			if (r >= this.count)
				r -= this.count;

			this.temps [l] += this.tempSpeed * c;
			this.temps [r] += this.tempSpeed * c;
			sun [l] = true;
			sun [r] = true;
		}

		this.temps [far] -= this.tempSpeed;
		sun [far] = false;
		for (int i = 1; i <= area; i++) {
			int l = far - i;
			int r = far + i;

			float c = Mathf.Sqrt(1.0f - (float) i / area);

			if (l < 0)
				l += this.count;
			if (r >= this.count)
				r -= this.count;

			this.temps [l] -= this.tempSpeed * c;
			this.temps [r] -= this.tempSpeed * c;
			sun [l] = false;
			sun [r] = false;
		}

		for (int i = 0; i < this.count; i++) {
			PlantManager plant = this.plants[i];
			plant.SetTemp (Mathf.Clamp(this.temps[i], 0.0f, 1.0f));
			plant.SetSun (sun[i]);
		}
	}

	// Возвращает индекс ближайшего сектора к солнцу
	private int GetNearSector() {
		float angle = (90 - this.transform.eulerAngles.z % 360);
		if (angle < 0)
			angle += 360;
		int sector = (int) (angle / this.sectorAngle);
		return sector;
	}

	// Событие распространения пожара от одного дерева
	public void OnFire(int index) {
		int l = index - 1;
		int r = index + 1;

		if (l < 0)
			l += this.count;
		if (r >= this.count)
			r -= this.count;

		this.plants [l].OnBurn ();
		this.plants [r].OnBurn ();
	}

	// Событие падения кометы
	public void OnComet(float cometAngle, int size) {
		float angle = (cometAngle - this.transform.eulerAngles.z % 360);
		if (angle < 0)
			angle += 360;
		int sector = (int) (angle / this.sectorAngle);
		int area = size / 2;

		this.explosions [sector].Explosion ();
		this.plants [sector].OnComet();
		for (int i = 1; i < area; i++) {
			int l = sector - i;
			int r = sector + i;

			if (l < 0)
				l += this.count;
			if (r >= this.count)
				r -= this.count;
			
			this.explosions [l].Explosion ();
			this.explosions [r].Explosion ();
			this.plants [l].OnComet ();
			this.plants [r].OnComet ();
		}
	}

	//
	public void OnGrow(int index) {
		this.grows [index].Grow ();
	}

}
