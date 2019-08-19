using UnityEngine;
using System.Collections;

public class MovePoint : MonoBehaviour {

	InstantiateGrid gridScript;
	ScalePoint scalePoint;
	RotateCamera rotateCamera;
	RotateMeshPoint rotateMesh;


	public GameObject point;

	public GameObject xAxis,zAxis;
	public GameObject meshPoint;

	Vector3 origin;

	/// 
    int rightValue = 0;
	int forwardValue = 0;

	int[] rights, forwards;
	Vector3[] rightsV,forwardsV;
	
	Vector3 rightDirection;
	Vector3 forwardDirection;


	int rotateStep = 0;

	public int index = 0;
	
	// position
	int x = 0;
	int z = 0;
	// scaling
	int xScaling,zScaling;
	int module;
	

	// Use this for initialization
	void Start () {

		rotateMesh = (RotateMeshPoint)GetComponent (typeof(RotateMeshPoint));
		scalePoint = (ScalePoint)GetComponent (typeof(ScalePoint));
		calculateScalingValues ();

		gridScript = (InstantiateGrid)GetComponent (typeof(InstantiateGrid));
		rotateCamera = (RotateCamera)GetComponent (typeof(RotateCamera));
	
		module = 4 * gridScript.returnMultiplyer ();

		/////////////
		/// 
		// DIRECTIONS

		rights = new int[2];
		forwards = new int[2];

		rights [0] = 1;
		rights [1] = module;

		forwards [0] = rights [1];
		forwards [1] = rights [0];

		rightValue = rights [0];
		forwardValue = forwards [0];

		rightsV = new Vector3[2];
		forwardsV = new Vector3[2];

		Vector3 forward = new Vector3 (0, 0, 1);
		Vector3 right = new Vector3 (1, 0, 0);

		rightsV [0] = right;
		rightsV [1] = forward;

		forwardsV [0] = rightsV [1];
		forwardsV [1] = rightsV [0];	

		rightDirection = right;
		forwardDirection = forward;
	}
	//CALCULER LES VALEURS DE SCALING
	public void calculateScalingValues(){
		xScaling = scalePoint.returnXScaling () - 1;
		zScaling = scalePoint.returnZScaling () - 1;
	}
	
	// Update is called once per frame
	void Update () {

		// update Position
		x = index % (module);
		z = ((index - x) / module);

		if(Input.GetKeyDown(KeyCode.UpArrow) ){
			//MOVE TO FORWARD

			if(isValidIndex(forwardValue)){
			    movePosition(forwardValue);
				point.transform.Translate( forwardDirection );
				rotateCamera.saveCameraPosition();
			}
		}

		if(Input.GetKeyDown(KeyCode.DownArrow)){
			//MOVE TO BACKWARD
			if(isValidIndex(-forwardValue)){
				movePosition(-forwardValue);
				point.transform.Translate( -forwardDirection  );
				rotateCamera.saveCameraPosition();
			}

		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			//MOVE TO RIGHT
			if(isValidIndex(rightValue)){
				movePosition(rightValue);
				point.transform.Translate( rightDirection  );
				rotateCamera.saveCameraPosition();
			}
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			//MOVE TO LEFT
			if(isValidIndex(-rightValue)){
				movePosition(-rightValue);
				point.transform.Translate( -rightDirection  );
				rotateCamera.saveCameraPosition();
			}
		}


	}

	public void updateDirection(int i){

		rotateStep++;

		rightValue = rights [rotateStep%2];
		forwardValue = forwards [rotateStep%2];

		rightDirection = rightsV [rotateStep%2];
		forwardDirection = forwardsV [rotateStep%2];

			if (rotateStep % 4 >= 1 && rotateStep % 4 < 3) {
				rightDirection *= -1;
				rightValue *= -1;
			}
	

			if (rotateStep % 4 >= 2) {
				forwardDirection *= -1;
				forwardValue *= -1;
			}


		rotateAxis ();
	
	}
	// ROTATE AXIS
	 void rotateAxis(){

		updateAxisPosition ();

		xAxis.transform.RotateAround (meshPoint.transform.position, new Vector3 (0, 1, 0), 90);
		zAxis.transform.RotateAround (meshPoint.transform.position, new Vector3 (0, 1, 0), 90);

	}

	public void updateAxisPosition(){

		xAxis.transform.position = transform.parent.transform.GetChild (0).position;
		zAxis.transform.position = transform.parent.transform.GetChild (0).position;
		
		int valueStep = rotateStep % 4 / 2;
		valueStep *= -1;
		if (valueStep == 0)
			valueStep++;
		
		int sX = scalePoint.returnXScaling ();
		int sZ = scalePoint.returnZScaling ();
		
		float scaleXValue = sX /2 + 0.5f;
		float scaleZValue = sZ /2 + 0.5f;
		
		if (sX % 2 != 0) scaleXValue += 0.5f;
		if (sZ % 2 != 0) scaleZValue += 0.5f;

		int step  = rotateMesh.returnStep ();

		if (step % 2 == 1) {
			float nz = scaleXValue;
			float nx = scaleZValue;
			scaleXValue = nx;
			scaleZValue = nz;
		}
		
		if (rotateStep % 2 == 0) {
			xAxis.transform.Translate (rightDirection * scaleXValue * valueStep);
			zAxis.transform.Translate (forwardDirection * scaleZValue * valueStep);
			
		} else {
			xAxis.transform.Translate (forwardDirection * scaleZValue * valueStep);
			zAxis.transform.Translate (rightDirection * scaleXValue * -valueStep);

		}

	}


	public void fixPointMeshPosition(){

		x = index % (module);
		z = ((index - x) / module);
		float xOffset = (xScaling - 1) * 0.5f;
		float zOffset = (zScaling - 1) * 0.5f;
	

		int step  = rotateMesh.returnStep ();

		if (step % 2 == 1) {
			float nx = zOffset;
			float nz = xOffset;
			xOffset = nx;
			zOffset = nz;
		}


		//print (xScaling);
		Vector3 fix = new Vector3 (x + xOffset, meshPoint.transform.position.y, z + zOffset);
		origin = new Vector3(-2*(module/4) + 1  ,0,-2*(module/4) + 1  );

		meshPoint.transform.position =  origin + fix;

	}


  
	public void movePosition(int i){
		
		index += i;
	}

	// Can we move point on this index ??




	
	public int convertPosToIndex(Vector3 pos){
		
		float xValue = pos.x;
		float zValue = pos.z;
		
		float xOrigin = (2 * gridScript.returnMultiplyer())-0.5f;
		float zOrigin = (2 * gridScript.returnMultiplyer())-0.5f;

		float currentX = xValue + xOrigin;
		float currentZ = zValue + zOrigin;
		
		int currentIndex = (int)currentX + (int)currentZ * module;

		return currentIndex;
	}

	public bool isValidIndex(int ind){
		
		int module = 4 * gridScript.returnMultiplyer();
		
		int xIndex = index % (4 * gridScript.returnMultiplyer());
		int zIndex = ((index - xIndex) / module);
		
		
		if (xIndex == 0 && ind == -1)
			return false;
		
		if (xIndex == module - 1 - xScaling && ind == 1)
			return false;
		
		if (zIndex == module - 1 - zScaling && ind == module)
			return false;
		
		
		if (index + ind < 0 || index + ind >= module * module ) {
			print ("false move "+(index+ind));
			
			return false;
		}
		
		
		return true;
	}




	public Vector3 returnOrigin(){
		return origin;
	}


	public Vector3 returnPointPosition(){
		return meshPoint.transform.position;
	}

	public GameObject returnMeshPoint(){
		return meshPoint;
	}

	public int returnX(){
		return x;
	}
	
	public int returnZ(){
		return z;
	}
	
	public int returnIndex(){
		return index;
	}

}
