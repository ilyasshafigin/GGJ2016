using UnityEngine;
using System.Collections;

public class RotationEarth : MonoBehaviour {
	private Vector2 previousMousePosition=Vector2.zero;
	private Vector2 currentMousePosition =Vector2.zero;
	private bool previousFrameMouseDown = false;
	public Transform Earth;
    private bool drag = false;
    private Vector2 previousTouchPosition = Vector2.zero;
    private Vector2 currentTouchPosition = Vector2.zero;

	void Update () {
		//Мышь
		Vector2 mousePos = new Vector2(Input.mousePosition.x-Screen.width/2, Input.mousePosition.y-Screen.height/2);
		if (Input.GetMouseButtonDown(0) && !previousFrameMouseDown){
			previousMousePosition = mousePos;
			currentMousePosition = mousePos;
			previousFrameMouseDown = true;
		}
		else if (Input.GetMouseButton(0) && previousFrameMouseDown){
			previousMousePosition = currentMousePosition;
			currentMousePosition = mousePos;
		}
		else if (!Input.GetMouseButton(0)){
			previousFrameMouseDown = false;	
		}
		Vector2 previousPositionVector = previousMousePosition;	
		Vector2 currentPositionVector = currentMousePosition ;
		if (previousPositionVector != currentPositionVector && previousFrameMouseDown)		{
			float rotationAmount = ReturnSignedAngleBetweenVectors(previousPositionVector,
				currentPositionVector);

			Earth.transform.RotateAroundLocal(Vector3.forward, rotationAmount * Time.deltaTime);
		}
		//Сенсор
		/*if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began) {
				previousTouchPosition = touch.position;
				currentTouchPosition = touch.position;
			} else if (touch.phase == TouchPhase.Moved) {
				previousTouchPosition = currentTouchPosition;
				currentTouchPosition = touch.position;
				drag = true;
			} else {
				drag = false;
			}
			Vector2 previousPositionVector2 = previousTouchPosition;	
			Vector2 currentPositionVector2 = currentTouchPosition ;
			if (previousPositionVector2 != currentPositionVector2 && drag){
				float rotationAmount = ReturnSignedAngleBetweenVectors(previousPositionVector2,currentPositionVector2);
				Earth.transform.RotateAroundLocal(Vector3.forward, rotationAmount * Time.deltaTime);
			}
		}*/
	}
    private float ReturnSignedAngleBetweenVectors(Vector2 vectorA, Vector2 vectorB)
	{
		Vector3 vector3A = new Vector3(vectorA.x, vectorA.y, 0f);
		Vector3 vector3B = new Vector3(vectorB.x, vectorB.y, 0f);

		if (vector3A == vector3B)
			return 0f;
        
		Vector3 refVector = Vector3.Cross(vector3A, Vector3.forward);
		float dotProduct = Vector3.Dot(refVector, vector3B);

		if (dotProduct > 0)
			return -Vector3.Angle(vector3A, vector3B);
		else if (dotProduct < 0)
			return Vector3.Angle(vector3A, vector3B);
		else
			throw new System.InvalidOperationException("the vectors are opposite");
	}
}
