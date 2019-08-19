using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {


	public GameObject mainCamera;
	public GameObject mainPoint;
	public GameObject explorer;
	public GameObject explorerSpawner;

	GameObject currentExplorer;

	ScalePoint scalePoint;
	MovePoint movePoint;
	RotateCamera rotateCamera;
	RotateMeshPoint rotateMesh;
	InstantiateGrid grid;
	ChangePointMesh changePoint;
	SelectObjectToCreate selectObject;
	CreateObject createObject;

	//// 
	int step = 0;
	string mode = "Explore Universe";


	Vector3 savePosition;
	Quaternion saveRotation;

	// Use this for initialization
	void Start () {

		scalePoint = (ScalePoint)mainCamera.GetComponent (typeof(ScalePoint));
		movePoint = (MovePoint)mainCamera.GetComponent (typeof(MovePoint));
		rotateCamera = (RotateCamera)mainCamera.GetComponent (typeof(RotateCamera));
		rotateMesh = (RotateMeshPoint)mainCamera.GetComponent (typeof(RotateMeshPoint));
		grid = (InstantiateGrid)mainCamera.GetComponent (typeof(InstantiateGrid));
		changePoint = (ChangePointMesh)mainCamera.GetComponent (typeof(ChangePointMesh));
		selectObject = (SelectObjectToCreate)mainCamera.GetComponent (typeof(SelectObjectToCreate));
		createObject = (CreateObject)mainCamera.GetComponent (typeof(CreateObject));


		saveRotation = mainCamera.transform.rotation;

	}

	void OnGUI(){

		float w = 100;
		float h = 20;

		if (GUI.Button (new Rect (Screen.width - w, Screen.height - h, w, h), mode)) {
			updateMode ();
		}

	}

	void updateMode(){
		step++;

		if (step % 2 == 0) {
			mode = "Explore Universe";

			mainCamera.transform.parent = mainPoint.transform;
			mainCamera.transform.position = savePosition;
			mainCamera.transform.rotation = saveRotation;
			Destroy (currentExplorer);
			switchMode(true);

		} else {

			mode = "Go Back To Edit Mode";
			savePosition = mainCamera.transform.position;
			mainCamera.transform.rotation = saveRotation;

			currentExplorer = GameObject.Instantiate(explorer);
			currentExplorer.transform.position = explorerSpawner.transform.position;
			mainCamera.transform.position = currentExplorer.transform.position 
				- currentExplorer.transform.forward * 4 + currentExplorer.transform.up * 2;

			mainCamera.transform.parent = currentExplorer.transform;
			switchMode(false);
		}

	}

	
	void switchMode (bool arg){
		
		scalePoint.enabled = arg;
		movePoint.enabled = arg;
		rotateCamera.enabled = arg;
		rotateMesh.enabled = arg;
		grid.enabled = arg;
		changePoint.enabled = arg;
		selectObject.enabled = arg;
		createObject.enabled = arg;
		mainPoint.SetActive (arg);
	}

	public int returnStep(){
		return step;
	}

}
