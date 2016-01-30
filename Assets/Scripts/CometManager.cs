using UnityEngine;
using System.Collections;

public class CometManager : MonoBehaviour {

	public float minTimeSpawn = 3;
	public float maxTimeSpawn = 5;
	public float force = 10000f;
	public float radius = 10;

	private float angle;
	private float elapsedTime;
	private float deltaTime;

	// Use this for initialization
	void Start () {
		this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
		this.deltaTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.deltaTime += Time.deltaTime;
		if (this.deltaTime >= this.elapsedTime) {
			this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
			this.deltaTime = 0.0f;

			// Спауним комету
			this.Spawn ();
		}
	}

	private void Spawn() {
		this.gameObject.SetActive (true);

		this.angle = Random.Range (0, 360) * Mathf.Deg2Rad;
		this.transform.position =  new Vector3 (this.radius*Mathf.Cos(angle), this.radius*Mathf.Sin(angle), 0);
		this.transform.rotation = Quaternion.AngleAxis(angle*Mathf.Rad2Deg+90, Vector3.forward);

		Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
		body.velocity = Vector2.zero;
		body.AddForce (new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle)) * this.force);
	}

	// Событие столкновения кометы с объектами
	private void OnTriggerEnter2D(Collider2D collider) {
		// Если комента столкнулась с планетой
		if (collider.gameObject.name == "Planet") {
			// Запускаем событие
			collider.gameObject.SendMessage("OnComet", this.angle);
			// Деактивируем комету
			Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
			body.velocity = Vector2.zero;
			this.transform.position =  new Vector3 (-10, 0, 0);
		}
	}
}
