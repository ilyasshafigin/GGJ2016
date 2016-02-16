using UnityEngine;
using System.Collections;

public class FallingStar : MonoBehaviour {

	private Animator animator;

	// Минимальное время респауна
	public float minTimeSpawn = 3;
	// Максимально время респауна
	public float maxTimeSpawn = 5;

	public float duration = 1.0f;
	private float elapsed = 0.0f;
	private Vector3 from;
	private Vector3 to;

	private float angle;
	private float elapsedTime;
	private float deltaTime;

	// Use this for initialization
	void Start () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
		this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
		this.deltaTime = 0.0f;
		// Спауним комету
		this.Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((this.deltaTime += Time.deltaTime) >= this.elapsedTime) {
			this.elapsedTime = Random.Range (this.minTimeSpawn, this.maxTimeSpawn);
			this.deltaTime = 0.0f;

			// Спауним комету
			this.Spawn ();
		} else {
			this.elapsed += Time.deltaTime;
			float progress = this.elapsed / this.duration;

			this.animator.Play ("FallingStar", 0, progress);
			this.transform.position = Vector3.Lerp (this.from, this.to, progress);
		}
	}

	// Респаун кометы
	private void Spawn() {
		this.elapsed = 0.0f;

		bool left = Random.Range (0f, 1f) > 0.5f;
		float fromX = left ? -10 : 10;
		float fromY = Random.Range (-1, 1);
		float toX = -fromX;
		float toY = Random.Range (-4, 4);

		this.from = new Vector3 (fromX, fromY, 0);
		this.to = new Vector3 (toX, toY, 0);

		// Устанавливаем угол прилета кометы
		this.angle = Mathf.Atan2(toY - fromY, toX - fromX)*Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
	}
}
