using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public KeyCode rotateLeft;

	public GameObject point;
	MovePoint movePoint;

	Quaternion saveRotation;
	Vector3 savePosition;

	Vector3 axis = new Vector3 (0, 1, 0);

	bool rotateClick = false;



	void Start () {

		movePoint = (MovePoint)GetComponent (typeof(MovePoint));
		saveRotation = transform.rotation;
		savePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	

		if (Input.GetKeyDown (rotateLeft)) {

			transform.RotateAround(point.transform.position , axis, 90);

			movePoint.updateDirection(-1);

			saveRotation = transform.rotation;
			savePosition = transform.position;

			rotateClick = false;
		}

	
		/*
		//ROTATE AROUND
		if(Input.GetMouseButton(1)){

			transform.RotateAround(point.transform.position , axis, 1);
			rotationAngle++;

			if(rotationAngle==90){
				rotationAngle-=90;
				movePoint.updateDirection(-1);
			}

			rotateClick = true;

		}
		*/



		                          
	}


	public void resetRotation(){
		transform.rotation = saveRotation;
		transform.position = savePosition;
		rotateClick = false;
	}



	public void saveCameraPosition(){
		savePosition = transform.position;
	}


	public bool returnRotateClick(){
		return rotateClick;
	}



}
