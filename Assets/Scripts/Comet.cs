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

	// Угол прилета кометы
	private float angle;

	//
	private IEnumerator SpawnLoop() {
		// Задержка между респаунами кометы
		yield return new WaitForSeconds (Random.Range (this.minTimeSpawn, this.maxTimeSpawn));

		if (!GameManager.instance.IsGameOver ()) {
			Spawn ();
		
			StartCoroutine(SpawnLoop ());
		}
	}

	// Респаун кометы
	private void Spawn() {
		// Активируем комету
		this.Show ();

		// Устанавливаем угол прилета кометы
		this.angle = Random.Range (0, 360);
		this.transform.position = new Vector3 (this.radius * Mathf.Cos (angle * Mathf.Deg2Rad), this.radius * Mathf.Sin (angle * Mathf.Deg2Rad), 0);
		this.transform.rotation = Quaternion.AngleAxis (angle + 90, Vector3.forward);

		Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
		body.velocity = Vector2.zero;
		body.AddForce (new Vector2 (-Mathf.Cos (angle * Mathf.Deg2Rad), -Mathf.Sin (angle * Mathf.Deg2Rad)) * this.force);
	}

	// Событие столкновения кометы с объектами
	private void OnTriggerEnter2D(Collider2D collider) {
		// Если комента столкнулась с планетой
		Planet planet = collider.GetComponent<Planet>();
		if (planet != null) {
			// Запускаем событие
			planet.OnComet(this.angle, this.size);
			// Запускаем звук
			this.GetComponent<AudioSource> ().Play ();
			// Деактивируем комету
			Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
			body.velocity = Vector2.zero;
			//
			this.Hide();
		}
	}

	// Показывает объект
	private void Show() {
		this.GetComponent<SpriteRenderer> ().enabled = true;
		this.GetComponent<CircleCollider2D> ().enabled = true;
	}

	// Скрывает объект
	private void Hide() {
		this.GetComponent<SpriteRenderer> ().enabled = false;
		this.GetComponent<CircleCollider2D> ().enabled = false;
	}

	// Событие запуска игры
	public void OnStartGame() {
		this.StartCoroutine (this.SpawnLoop());
	}

	// Событие окончания игры
	public void OnGameOver() {
		this.Hide ();
	}
		
}
