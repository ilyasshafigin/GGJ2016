using UnityEngine;
using System.Collections;

public class TemperatureLigth : MonoBehaviour {

	// Префаб
	public static Object prefab = Resources.Load ("Prefabs/TemperatureLight");

	// Цвет нагретой Земли
	public Color heatColor = Color.red;
	// Цвет замезшей Земли
	public Color coolColor = Color.blue;
	// Максимальная интенсивность света
	public float maxIntensity = 2.0f;
	// Радиус размещения источников света
	public float radius = 2.0f;

	// Текущая температура
	private float temp = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (this.temp > 0) {
			this.GetComponent<Light> ().color = this.heatColor;
		} else {
			this.GetComponent<Light> ().color = this.coolColor;
		}

		this.GetComponent<Light> ().intensity = Mathf.Lerp (0, this.maxIntensity, Mathf.Abs(this.temp));
	}

	//
	public void SetTemperature(float temp) {
		this.temp = temp;
	}

	// Создает объект
	public static TemperatureLigth CreateLight () {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		TemperatureLigth light = gameObject.GetComponent<TemperatureLigth> ();
		return light;
	}
}
