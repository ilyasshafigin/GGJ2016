using UnityEngine;
using System.Collections;

public class Comet : MonoBehaviour {

	// Минимальное время респауна
	public float minTimeSpawn = 3;
	// Максимально время респауна
	public float maxTimeSpawn = 5;
	// Сила, с которой летит камета
	public float force = 200;
	// Радиус, на котором появляются кометы
	public float radius = 10;
	// Размер кометы в секторах
	public int size = 1;

	private float angle;
	private float elapsedTime;
	private float deltaTime;

	// Инициализация
	void Start () {
		this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
		this.deltaTime = 0.0f;
	}

	// Обновление на каждом кадру из менеджера эффектов
	public void UpdateComet (float deltaTime) {
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime >= this.elapsedTime) {
			this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
			this.deltaTime = 0.0f;

			// Спауним комету
			this.Spawn ();
		}
	}

	// Респаун кометы
	private void Spawn() {
		// Активируем комету
		this.gameObject.SetActive (true);

		// Устанавливаем угол прилета кометы
		this.angle = Random.Range (0, 360);
		this.transform.position =  new Vector3 (this.radius*Mathf.Cos(angle * Mathf.Deg2Rad), this.radius*Mathf.Sin(angle * Mathf.Deg2Rad), 0);
		this.transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);

		Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
		body.velocity = Vector2.zero;
		body.AddForce (new Vector2(-Mathf.Cos(angle * Mathf.Deg2Rad), -Mathf.Sin(angle * Mathf.Deg2Rad)) * this.force);
	}

	// Событие столкновения кометы с объектами
	private void OnTriggerEnter2D(Collider2D collider) {
		// Если комента столкнулась с планетой
		Planet planet = collider.GetComponent<Planet>();
		if (planet != null) {
			// Запускаем событие
			planet.OnComet(this.angle, this.size);
			// Запускаем звук
			this.GetComponent<AudioSource> ().PlayOneShot (this.GetComponent<AudioSource>().clip, 1.0f);
			// Деактивируем комету
			Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
			body.velocity = Vector2.zero;
			//this.transform.position =  new Vector3 (-10, 0, 0);
			this.gameObject.SetActive (false);
		}
	}
}
