using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyByBoundary : MonoBehaviour {

    public GameObject go;
    public float timeSpawn;

    private float time;

    void Start() {
        time = timeSpawn;
    }

    void OnTrigerExit(Collider other) {
        //Destroy(other.gameObject);
    }

    void Update() {
		if (time < timeSpawn) {
			time += Time.deltaTime;
		} else {
            //if (transform.position.x >= 25)
            //    transform.Translate(new Vector3(-22, 0, 0));
            //transform.position = Vector3.Lerp(transform.position, new Vector3(1, 0, 0), 0.001f);
            time = 0;
            Instantiate(go, new Vector3(-30, 0, 2.5f), new Quaternion(0, 0, 0, 0));
        }
        //transform.position = Vector3.Lerp(transform.position, new Vector3(22, 0, 0), 0.001f);
    }
}
