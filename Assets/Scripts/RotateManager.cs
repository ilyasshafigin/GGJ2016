using UnityEngine;
using System.Collections;

public class RotateManager : MonoBehaviour {

	public PlanetManager planet;

	private TargetJoint2D joint;
	private float prevAngle;
	private GameObject line = null;


	// Use this for initialization
	void Start () {
		this.joint = this.GetComponent<TargetJoint2D> ();

		this.line = new GameObject ();
		this.line.AddComponent<LineRenderer> ();
		this.line.transform.parent = this.transform.parent;
	}

	void OnMouseDown() {
		Vector3 p = this.GetMousePosition ();
		this.prevAngle = Mathf.Atan2 (p.y, p.x);
		this.joint.anchor = this.transform.TransformPoint(new Vector2(p.x, p.y));
		this.joint.target = new Vector2(p.x, p.y);
		this.joint.enabled = true;

		Debug.Log (this.planet.GetNearSector());
	}

	void OnMouseDrag() {
		Vector3 p = this.GetMousePosition ();
		
		this.joint.target = new Vector2(p.x, p.y);
	}

	void OnMouseUp() {
		this.joint.enabled = false;
	}

	Vector3 GetMousePosition() {
		//return new Vector3 (Input.GetAxis("Mouse X")- Camera.main.pixelRect.width/2, Input.GetAxis("Mouse Y") - Camera.main.pixelRect.height/2);
		Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return p;
	}

	// Update is called once per frame
	void Update () {
		this.planet.transform.rotation = this.transform.rotation;

		Vector2 p1 = this.transform.InverseTransformPoint(this.joint.anchor);
		Vector2 p2 = this.joint.target;
		LineRenderer lr = this.line.GetComponent<LineRenderer> ();
		lr.SetPosition (0, new Vector3(p1.x, p1.y, 0));
		lr.SetPosition (1, new Vector3(p2.x, p2.y, 0));
		lr.SetColors (new Color(1,0,0),new Color(1,0,0));
		lr.SetWidth (0.1f,0.1f);
	
	}

}
