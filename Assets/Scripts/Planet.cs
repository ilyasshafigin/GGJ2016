using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour {

	// Количество растений/секторов
	public int count = 30;
	// Радиус планеты для определения положения растений
	public float radius = 1.85f;
	// Минимальное время появления растений
	public float minSpawnTime = 3;
	// Максимальное время появления растений
	public float maxSpawnTime = 5;
	// Значение повышения температуры за один кадр
	public float tempSpeed = 0.1f;
	// Диспетчер эффектов
	public EffectsManager effects;

	// Массив растений
	private Plant[] plants;
	// Массив температур, от -1 до 1
	private float[] temps;
	// Угол одного сетора
	private float sectorAngle;

	// Инициализация объекта
	private void Start () {
		this.plants = new Plant[this.count];
		this.temps = new float[this.count];

		this.sectorAngle = 360f / this.count;

		for (int i = 0; i < this.count; i++) {
			float angle = i * this.sectorAngle;
			float x = this.radius * Mathf.Cos (angle * Mathf.Deg2Rad);
			float y = this.radius * Mathf.Sin (angle * Mathf.Deg2Rad);

			Vector3 translation = new Vector3 (x, y, 0);
			Vector3 rotation = new Vector3 (0, 0, angle - 90);

			Plant plant = Plant.CreatePlant (this, i);
			plant.transform.SetParent (this.transform);
			plant.transform.Translate (translation);
			plant.transform.Rotate (rotation);
			this.plants[i] = plant;

			this.temps [i] = 0.0f;
		}
	}
		
	// Спаунит растение на свободном сеторе
	public Plant Spawn() {
		List<int> sectors = this.GetAvaliableSectors ();
		if (sectors.Count > 0) {
			// Находим рандомный сектор
			int index = sectors [(int)Random.Range (0, sectors.Count - 1)];
			// Получаем растение под индексом сектора
			Plant plant = this.plants [index];
			// Устанавливаем тип
			plant.SetType (Plant.Type.Tree);
			// Устанавливаем состояние роста
			plant.SetState (Plant.State.Growth);
			// Устанавливаем прогресс роста в 30%
			plant.SetGrowth (0.3f);
			return plant;
		} else {
			return null;
		}
	}

	// Возвращает список свободных секторов
	private List<int> GetAvaliableSectors() {
		List<int> sectors = new List<int> ();
		for (int i = 0; i < this.count; i++) {
			Plant plant = this.plants [i];
			if (plant.IsNone ()) {
				sectors.Add (i);
			}
		}
		return sectors;
	}
	
	// Обновление объекта на каждом кадре
	private void Update () {
		if (GameManager.instance.IsGameOver ())
			return;

		int near = this.GetNearSector ();
		int far = near + this.count / 2;
		if (far >= this.count)
			far -= this.count;
		int area = this.count / 4;
		bool[] sun = new bool[this.count];

		this.temps [near] += this.tempSpeed * Time.deltaTime;
		sun [near] = true;
		for (int i = 1; i <= area; i++) {
			int l = near - i;
			int r = near + i;

			float c = Mathf.Sqrt(1.0f - (float) i / area);

			if (l < 0)
				l += this.count;
			if (r >= this.count)
				r -= this.count;

			this.temps [l] += this.tempSpeed * c * Time.deltaTime;
			this.temps [r] += this.tempSpeed * c * Time.deltaTime;
			sun [l] = true;
			sun [r] = true;
		}

		this.temps [far] -= this.tempSpeed * Time.deltaTime;
		sun [far] = false;
		for (int i = 1; i <= area; i++) {
			int l = far - i;
			int r = far + i;

			float c = Mathf.Sqrt(1.0f - (float) i / area);

			if (l < 0)
				l += this.count;
			if (r >= this.count)
				r -= this.count;

			this.temps [l] -= this.tempSpeed * c * Time.deltaTime;
			this.temps [r] -= this.tempSpeed * c * Time.deltaTime;
			sun [l] = false;
			sun [r] = false;
		}

		for (int i = 0; i < this.count; i++) {
			this.temps [i] = Mathf.Clamp (this.temps [i], -1.0f, 1.0f);

			Plant plant = this.plants[i];
			plant.SetTemp (this.temps [i]);
			plant.SetSun (sun [i]);
		}

		// Получаем количество активных деревьев
		int treeCount = this.GetLiveTreeCount ();
		// Если нет активных деревьев, то "конец игры"
		if (treeCount == 0) {
			GameManager.instance.GameOver ();
			return;
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

	// Получает количество живых деревьев
	private int GetLiveTreeCount() {
		int count = 0;
		for (int i = 0; i < this.count; i++) {
			Plant plant = this.plants [i];
			if (plant.IsTree () && plant.state != Plant.State.DryingUpDone
				&& plant.state != Plant.State.WithreingDone) {
				count++;
			}
		}
		return count;
	}

	//
	public float GetTemperature(int sector) {
		return this.temps [sector];
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
		this.effects.OnComet (cometAngle, size);
	}

	// Событие попадания кометы на растение
	public void OnComet(int index) {
		this.plants [index].OnComet ();
	}

	//
	public void OnGrow(int index) {
		this.effects.OnGrow (index);
	}

	// Событие запуска игры
	public void OnStartGame() {
		foreach (Plant plant in this.plants) {
			plant.OnStartGame ();
		}

		// Запускаем спаун первых растений
		int count = 2;
		while(count-- > 0) {
			Plant plant = this.Spawn ();
			if (plant != null) {
				// Изначально, дерево выросло на половину
				plant.SetGrowth (0.5f);
			}
		}
	}

	// Событие окончания игры
	public void OnGameOver() {
		for (int i = 0; i < this.count; i++) {
			this.plants [i].OnGameOver ();
			this.temps [i] = 0.0f;
		}
	}

}
