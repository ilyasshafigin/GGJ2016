using UnityEngine;
using System.Collections;

public class CometManager : MonoBehaviour {

	public float minTimeSpawn = 3;
	public float maxTimeSpawn = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine (Spawn());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator Spawn() {
		yield return new WaitForSeconds (Random.Range(this.minTimeSpawn, this.maxTimeSpawn));

		this.gameObject.SetActive (false);

		float angle = Random.Range (0, 360)*Mathf.Deg2Rad;
		this.transform.position =  new Vector3 (10*Mathf.Cos(angle), 10*Mathf.Sin(angle), 0);
		this.transform.rotation = Quaternion.LookRotation(this.transform.position);

		Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
		body.AddForce (new Vector2(-10*Mathf.Cos(angle), -10*Mathf.Sin(angle)));

		this.gameObject.SetActive (true);

		yield return Spawn ();
	}
}
