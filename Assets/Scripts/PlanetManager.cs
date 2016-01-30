using UnityEngine;
using System.Collections;

public class PlanetManager : MonoBehaviour {

	public int plantCount = 36;
	public float radius;

	private PlantManager[] plants;

	private float sectorAngle;

	// Use this for initialization
	void Start () {
		this.plants = new PlantManager[this.plantCount];

		this.sectorAngle = 360f / this.plantCount;

		for (int i = 0; i < this.plantCount; i++) {
			float angle = i * this.sectorAngle;
			float x = this.radius * Mathf.Cos (angle * Mathf.Deg2Rad);
			float y = this.radius * Mathf.Sin (angle * Mathf.Deg2Rad);

			PlantManager plant = PlantManager.CreatePlant ();
			plant.transform.SetParent (this.transform);
			plant.transform.Translate (new Vector3(x, y, 0));
			plant.transform.Rotate (new Vector3(0, 0, angle-90));
			this.plants[i] = plant;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
