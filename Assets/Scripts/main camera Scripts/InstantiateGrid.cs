using UnityEngine;
using System.Collections;

public class InstantiateGrid : MonoBehaviour {

	MovePoint movePoint;
	ScalePoint scalePoint;

	public GameObject point;
	public GameObject grille;

	public int multiplierGrille = 2;
	int module= 0;

	//scale Offset
	float scaleXOffset, scaleYOffset, scaleZOffset;
	
	// trouver un meilleur nom
	int[] cubeNumbers;


	//public GameObject cube;
	//public GameObject cubeFloor;

	// Use this for initialization
	void Start () {

		movePoint = (MovePoint)GetComponent (typeof(MovePoint));

		// INIT GRID SCALING
		Vector3 scale = grille.transform.localScale;
		float scaleX = scale.x;
		float scaleZ = scale.z;
		grille.transform.localScale = new Vector3(scaleX * multiplierGrille,1,scaleZ * multiplierGrille);

		// ADAPT TEXTURE SCALING TO THE GRID SCALING
		Material mat = grille.GetComponent<MeshRenderer> ().material;	 
		mat.mainTextureScale = new Vector2(multiplierGrille,multiplierGrille);

		scalePoint =(ScalePoint) GetComponent (typeof(ScalePoint));


		// CALCULATE MODULE ( THIS IS THE SIZE OF THE SQUARE EDGE )
		module = 4 * multiplierGrille;
		cubeNumbers = new int[module*module];

		//InstantiateCubes ();

	}
	void InstantiateCubes(){
		/*
		Vector3 origin = new Vector3(-2*multiplierGrille + 0.5f ,-1,-2*multiplierGrille + 0.5f);


		int i = 0;
		while( i < module * module){

			float x = i % module;
			float z = ( (i - x) / module);

		    GameObject newCube = GameObject.Instantiate (cube);

			Vector3 v = new Vector3(x,0,z);
			newCube.transform.position = origin + v;
			newCube.transform.parent = cubeFloor.transform;

			i++;
		}

*/
		}

	public void calculateOffset(){

		int scaleXValue = scalePoint.returnXScaling() - 1;
		int scaleYValue = scalePoint.returnYScaling() - 1;
		int scaleZValue = scalePoint.returnZScaling() - 1;

		if (scaleYValue < 0)
			scaleYValue = 0;

		scaleXOffset = 0.5f * scaleXValue;
		scaleYOffset = 0.5f * scaleYValue;
		scaleZOffset = 0.5f * scaleZValue;


		GameObject meshPoint = movePoint.returnMeshPoint ();

		if (meshPoint.tag == "Tree") {

			scaleYOffset = -10.5f;
		}

		//print ("scaleYvalue "+scaleYValue);

		
	}

	void Update () {
		// UPDATE POSITION ON THE GRID.

		//print ("scaleYoffset :"+scaleYOffset);


		Vector3 pos = point.transform.position;
		
		int yPos = cubeNumbers [movePoint.returnIndex ()];
		point.transform.position = new Vector3(pos.x, yPos + scaleYOffset  ,pos.z);


		//print ("scale Y offset " + scaleYOffset);
	}



	public void updateCubeIndex(int index){

		cubeNumbers [index]++;

	}
	
	public int getCubeHeightValue(int index){
		return cubeNumbers [index];
	}


	public void InitPointPos(){

		point.transform.Translate(new Vector3(-2*multiplierGrille + scaleXOffset ,0,-2*multiplierGrille + scaleZOffset ));
	}

	public int returnMultiplyer(){
		return multiplierGrille;
	}

	public int returnModule(){
		return module;
	}

	public void removeCube(int currentIndex){
		cubeNumbers [currentIndex]--;

	}
}
