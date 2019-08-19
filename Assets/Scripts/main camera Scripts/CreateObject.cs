using UnityEngine;
using System.Collections;

public class CreateObject : MonoBehaviour {
	MovePoint movePoint;
	SelectObjectToCreate selectObject;
	InstantiateGrid gridScript;

	public Material overMaterial;

	public GameObject sandbox;
	// Use this for initialization
	void Start () {

		movePoint = (MovePoint)GetComponent (typeof(MovePoint));
		selectObject = (SelectObjectToCreate)GetComponent (typeof(SelectObjectToCreate));
		gridScript = (InstantiateGrid)GetComponent (typeof(InstantiateGrid));

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {

			GameObject meshPoint = movePoint.returnMeshPoint();
			Vector3 currentPos = meshPoint.transform.position;
			Quaternion rotation = meshPoint.transform.rotation;

			InstantiateGameObject(selectObject.returnCurrentObject(),currentPos, rotation);
	
			gridScript.updateCubeIndex(movePoint.returnIndex());
		}


	}

	void InstantiateGameObject(GameObject o, Vector3 pos, Quaternion r){

		GameObject newGameObject = (GameObject)Instantiate (o);

		newGameObject.transform.position = pos;
		newGameObject.transform.rotation = r;
		ChangeMaterialOnMouseOver change = (ChangeMaterialOnMouseOver)newGameObject.GetComponent (typeof(ChangeMaterialOnMouseOver));

		if (change == null) {

			newGameObject.AddComponent<ChangeMaterialOnMouseOver> ();
			ChangeMaterialOnMouseOver onMouseOver = (ChangeMaterialOnMouseOver)newGameObject.GetComponent (typeof(ChangeMaterialOnMouseOver));
			onMouseOver.setMaterial (overMaterial);
		}

		newGameObject.transform.parent = sandbox.transform;



	
/*
 * // TESTER GET CENTER
 * 
 * 	GameObject meshUtilityInterface = GameObject.Find ("MeshUtilityInterface");
		MeshUtility meshUtility = (MeshUtility)meshUtilityInterface.GetComponent (typeof(MeshUtility));

        Mesh m = newGameObject.GetComponent<MeshFilter>().mesh;    
		Object ooo = Resources.Load ("Models/Geometrie/A Cube");
		GameObject g = ooo as GameObject;
		GameObject newCube = GameObject.Instantiate (g);
	    newCube.transform.position = meshUtility.getMeshCenter(m) + newGameObject.transform.position;
		newCube.transform.name = "testCube";
	*/
	}



}
