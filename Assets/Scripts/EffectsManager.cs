using System;
using UnityEngine;


public class EffectsManager  : MonoBehaviour {

	// Планета
	public Planet planet;
	// Радиус для определения положения эффектов
	public float radius = 2.5f;

	// Массив эффектор привлечения внимания
	private Grow[] grows;
	// Массив эффектов взрыва
	private Explosion[] explosions;
	// Массив комет
	private Comet[] comets;
	// 
	private TemperatureLigth[] lights;

	// Инициализация
	private void Start() {
		int count = this.planet.count;
		float sectorAngle = 360f / count;

		this.explosions = new Explosion[count];
		this.grows = new Grow[count];
		this.comets = (Comet[]) this.GetComponentsInChildren<Comet>(true);
		this.lights = new TemperatureLigth[count];

		for (int i = 0; i < count; i++) {
			float angle = i * sectorAngle;
			float x = Mathf.Cos (angle * Mathf.Deg2Rad);
			float y = Mathf.Sin (angle * Mathf.Deg2Rad);

			Vector3 translation = new Vector3 (x, y, 0);
			Vector3 rotation = new Vector3 (0, 0, angle - 90);

			Explosion explosion = Explosion.CreateExplosion (this.planet);
			explosion.gameObject.SetActive(false);
			explosion.transform.SetParent (this.planet.transform);
			explosion.transform.Translate (this.radius * translation);
			explosion.transform.Rotate (rotation);
			this.explosions[i] = explosion;

			Grow grow = Grow.CreateGrow (this.planet);
			grow.gameObject.SetActive(false);
			grow.transform.SetParent (this.planet.transform);
			grow.transform.Translate (this.radius * translation);
			grow.transform.Rotate (rotation);
			this.grows[i] = grow;

			TemperatureLigth light = TemperatureLigth.CreateLight ();
			light.gameObject.SetActive(true);
			light.transform.SetParent (this.planet.transform);
			light.transform.Translate (light.radius * translation);
			light.transform.Rotate (rotation);
			this.lights[i] = light;
		}
	}

	// Обновение на каждом каждре
	private void Update() {
		foreach (Comet comet in this.comets) {
			comet.UpdateComet (Time.deltaTime);
		}

		for (int i = 0; i < this.planet.count; i++) {
			this.lights [i].SetTemperature (this.planet.GetTemperature(i));
		}
	}

	// Событие падения кометы
	public void OnComet(float cometAngle, int size) {
		int count = this.planet.count;
		float sectorAngle = 360f / count;
		float angle = (cometAngle - this.planet.transform.eulerAngles.z % 360);
		if (angle < 0)
			angle += 360;
		int sector = (int) (angle / sectorAngle);
		int area = size / 2;

		this.explosions [sector].OnExplosion ();
		this.planet.OnComet (sector);
		for (int i = 1; i < area; i++) {
			int l = sector - i;
			int r = sector + i;

			if (l < 0)
				l += count;
			if (r >= count)
				r -= count;

			this.explosions [l].OnExplosion ();
			this.explosions [r].OnExplosion ();
			this.planet.OnComet (l);
			this.planet.OnComet (r);
		}
	}

	//
	public void OnGrow(int sector) {
		this.grows [sector].OnGrow ();
	}

}


