using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour {
	private float delTime=0;
	public float maxTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Mouse0) || Input.touches.Length != 0)
			SceneManager.LoadScene ("Scene");
	}
}
