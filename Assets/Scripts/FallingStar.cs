using UnityEngine;
using System.Collections;

public class FallingStar : MonoBehaviour {

	private Animator animator;

	// Минимальное время респауна
	public float minTimeSpawn = 3;
	// Максимально время респауна
	public float maxTimeSpawn = 5;
	// Время полета
	public float duration = 1.0f;

	private float elapsed = 0.0f;
	private Vector3 from;
	private Vector3 to;

	// 
	void Start () {
		this.animator = this.GetComponent<Animator> ();
		this.animator.speed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;
		float progress = this.elapsed / this.duration;

		this.animator.Play ("FallingStar", 0, progress);
		this.transform.position = Vector3.Lerp (this.from, this.to, progress);
	}

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
		this.elapsed = 0.0f;

		bool left = Random.Range (0f, 1f) > 0.5f;
		float fromX = left ? -10 : 10;
		float fromY = Random.Range (-3, 3);
		float toX = -fromX;
		float toY = Random.Range (-10, 10);

		this.from = new Vector3 (fromX, fromY, 0);
		this.to = new Vector3 (toX, toY, 0);

		float angle = Mathf.Atan2(toY - fromY, toX - fromX)*Mathf.Rad2Deg;
		this.transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
	}

	// Событие запуска игры
	public void OnStartGame() {
		this.StartCoroutine (this.SpawnLoop ());
	}

	// Событие окончания игры
	public void OnGameOver() {

	}

}
