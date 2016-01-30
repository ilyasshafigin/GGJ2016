using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlantManager : MonoBehaviour {

	public static Object prefab = Resources.Load ("Plant");

	public enum Type {
		Tree, Weed, None
	}

	private Type type;

	// Use this for initialization
	void Start () {
	
	}

	void Initialize() {
		this.SetType (Type.None);
	}

	void SetType(Type type) {
		this.type = type;
	}

	bool IsNone() {
		return this.type == Type.None;
	}

	bool IsTree() {
		return this.type == Type.Tree;
	}

	bool IsWeed() {
		return this.type == Type.Weed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static PlantManager CreatePlant() {
		GameObject gameObject = (GameObject) Object.Instantiate (prefab);
		PlantManager plant = gameObject.GetComponent<PlantManager> ();
		plant.Initialize ();
		return plant;
	}
}
