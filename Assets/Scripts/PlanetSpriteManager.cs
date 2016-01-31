using UnityEngine;
using System.Collections;

public class PlanetSpriteManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string[] sprites = { "Planet1", "Planet2", "Planet3", "Planet4" };
		this.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + sprites[Random.Range (0, 3)]);
	}
}
