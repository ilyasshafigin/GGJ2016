using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour {

	// Количество растений/секторов
	public int plantCount = 36;
	// Радиус планеты для определения положения растений
	public float radius;
	// Минимальное время появления растений
	public float minSpawnTime = 3;
	// Максимальное время появления растений
	public float maxSpawnTime = 5;

	// Массив растений
	private PlantManager[] plants;
	// Массив температур, максимум - 1
	private float[] temps;
	// Значение повышения температуры за один кадр
	private float tempSpeed = 0.01f;
	// Угол одного сетора
	private float sectorAngle;

	// Инициализация объекта
	private void Start () {
		this.plants = new PlantManager[this.plantCount];
		this.temps = new float[this.plantCount];

		this.sectorAngle = 360f / this.plantCount;

		for (int i = 0; i < this.plantCount; i++) {
			float angle = i * this.sectorAngle;
			float x = this.radius * Mathf.Cos (angle * Mathf.Deg2Rad);
			float y = this.radius * Mathf.Sin (angle * Mathf.Deg2Rad);

			PlantManager plant = PlantManager.CreatePlant ();
			plant.transform.SetParent (this.transform);
			plant.transform.Translate (new Vector3(x, y, 0));
			plant.transform.Rotate (new Vector3(0, 0, angle-90));
			this.plants[i] = plant;
		}

		// Запускаем спаун первых растений
		this.Spawn();
	}
	
	// Обновление объекта на каждом кадре
	private void Update () {
		int near = this.GetNearSector ();
		int far = near + this.plantCount / 2;
		if (far >= this.plantCount)
			far -= this.plantCount;
		int area = this.plantCount / 4;
		bool[] sun = new bool[this.plantCount];

		this.temps [near] += this.tempSpeed;
		sun [near] = true;
		for (int i = 1; i <= area; i++) {
			int l = near - i;
			int r = near + i;

			float c = Mathf.Sqrt(1.0f - (float) i / area);

			if (l < 0)
				l += this.plantCount;
			if (r >= this.plantCount)
				r -= this.plantCount;

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
				l += this.plantCount;
			if (r >= this.plantCount)
				r -= this.plantCount;

			this.temps [l] -= this.tempSpeed * c;
			this.temps [r] -= this.tempSpeed * c;
			sun [l] = false;
			sun [r] = false;
		}

		for (int i = 0; i < this.plantCount; i++) {
			PlantManager plant = this.plants[i];
	
			plant.SetTemp (this.temps[i]);
			plant.setSun (sun[i]);
		}
	}

	private void Spawn() {
		int count = 2;
		while(count-- > 0) {
			List<int> sectors = this.GetAvaliableSectors ();
			if (sectors.Count > 0) {
				int index = sectors [(int)Random.Range (0, sectors.Count - 1)];
				PlantManager plant = this.plants [index];
				plant.SetType (PlantManager.Type.Tree);
			}
		}
	}

	private List<int> GetAvaliableSectors() {
		List<int> sectors = new List<int> ();
		for (int i = 0; i < this.plantCount; i++) {
			PlantManager plant = this.plants [i];
			if (plant.IsNone ()) {
				sectors.Add (i);
			}
		}
		return sectors;
	}

	public int GetNearSector() {
		float angle = (90 - this.transform.eulerAngles.z % 360);
		if (angle < 0)
			angle += 360;
		int sector = (int) (angle / this.sectorAngle);
		return sector;
	}

}
