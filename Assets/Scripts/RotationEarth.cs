using UnityEngine;
using System.Collections;

public class RotationEarth : MonoBehaviour {
	/*private Vector2 previousTouch;
	 * private Vector2 newTouch;
	 * private bool drag = false;
	 * public Transform Earth;
	*/
	private Vector2 previousMousePosition=Vector2.zero;
	private Vector2 currentMousePosition =Vector2.zero;
	bool previousFrameMouseDown = false;
	public Transform Earth;

	void Update () {
//		/*
//		 * Версия 2
//		 * if (Input.touchCount == 1) {
//		 * Touch touch0 = Input.GetTouch (0);
//		 * if (!drag) {
//		 * initialTouchPosition = touch0.position;
//				drag = true;
//			}
//			else
//			{
//				Vector2 delta = camera.ScreenToWorldPoint(touch0.position) -
//					camera.ScreenToWorldPoint(initialTouchPosition);
//			}
//		}
//Версия 1
//		if(Input.touchCount ==1){
//			if (touch.phase == TouchPhase.Began) {
//				newTouch.x = touch.position.x;
//				newTouch.y = touch.position.y;
//			}
//			if (touch.phase == TouchPhase.Moved) {
//				previousTouch.x = newTouch.x; previousTouch.y = newTouch.y;
//				newTouch.x = touch.position.x; newTouch.y = touch.position.y;
//				if (previousTouch.x < newTouch.x)
//					Earth.Rotat
//			}
//		}
//		*/
		Vector2 mousePos = new Vector2(Input.mousePosition.x-Screen.currentResolution.width/2, Input.mousePosition.y-Screen.currentResolution.height/2);
		if (Input.GetMouseButtonDown(0) && !previousFrameMouseDown)
		{
			previousMousePosition = mousePos;
			currentMousePosition = mousePos;
			previousFrameMouseDown = true;
		}
		else if (Input.GetMouseButton(0) && previousFrameMouseDown)
		{
			previousMousePosition = currentMousePosition;
			currentMousePosition = mousePos;
		}
		else if (!Input.GetMouseButton(0))
		{
			previousFrameMouseDown = false;	
		}

		Vector2 previousPositionVector = previousMousePosition;	
		Vector2 currentPositionVector = currentMousePosition ;

		if (previousPositionVector != currentPositionVector && previousFrameMouseDown)
		{
			float rotationAmount = ReturnSignedAngleBetweenVectors(previousPositionVector,
				currentPositionVector);

			Earth.transform.RotateAroundLocal(Vector3.forward, rotationAmount * Time.deltaTime);
		}
	}

	private float ReturnSignedAngleBetweenVectors(Vector2 vectorA, Vector2 vectorB)
	{
		Vector3 vector3A = new Vector3(vectorA.x, vectorA.y, 0f);
		Vector3 vector3B = new Vector3(vectorB.x, vectorB.y, 0f);

		if (vector3A == vector3B)
			return 0f;

		// refVector is a 90cw rotation of vector3A
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
