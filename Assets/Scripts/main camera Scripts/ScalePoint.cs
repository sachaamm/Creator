using UnityEngine;
using System.Collections;

public class ScalePoint : MonoBehaviour {

	public GameObject xAxis, zAxis;

	public int scaleXValue = 2;
	public int scaleYValue = 2;
	public int scaleZValue = 2;

	public GameObject pointMesh;

	SelectObjectToCreate selectObject;
	InstantiateGrid grid;
	// Use this for initialization
	void Start () {

		selectObject = (SelectObjectToCreate)GetComponent (typeof(SelectObjectToCreate));
		grid = (InstantiateGrid)GetComponent (typeof(InstantiateGrid));


		grid.InitPointPos ();
	}

	void updateScalingValues(){

		GameObject currentObject = selectObject.returnCurrentObject ();
		Vector3 scaling = currentObject.transform.localScale;	
		//RECUPERER LE MESH FILTER
		MeshFilter mf = currentObject.GetComponent<MeshFilter> ();
		if (mf == null) {
			// PROBLEME ICI
			//GameObject meshUtilityInterface = GameObject.Find ("MeshUtilityInterface");
			mf = currentObject.transform.Find ("default").GetComponent<MeshFilter> ();
		}
		
		Mesh m = scaleMesh (mf.mesh,scaling.x, scaling.y, scaling.z);
		Vector3[] v = m.vertices;

		//CALCULER LA BOUNDING BOX QUI CONTIENT LE MESH
		float Xgap = getXGap (v);
		float Ygap = getYGap (v);
		float Zgap = getZGap (v);
		
		scaleXValue = (int)Xgap;
		scaleYValue = (int)Ygap;
		scaleZValue = (int)Zgap;
		
		pointMesh.GetComponent<MeshFilter> ().mesh = m;
		
		grid.calculateOffset ();
	}
	// que se passe t il ici
	public void updateScaling(){

		updateScalingValues ();
		
		MovePoint movePoint = (MovePoint)GetComponent (typeof(MovePoint));
		movePoint.calculateScalingValues ();

		movePoint.fixPointMeshPosition ();
		movePoint.updateAxisPosition();
	}




	Mesh scaleMesh(Mesh m,float xVal, float yVal, float zVal){
 
		Mesh nm = new Mesh ();
		Vector3[] v = m.vertices;
		int[] f = m.triangles;

		nm.vertices = v;
		nm.triangles = f;

		for (int i = 0; i < v.Length; i++) {

			Vector3 cv = v[i];
			v[i] = new Vector3(cv.x * xVal , cv.y * yVal , cv.z * zVal );

		}

		nm.vertices = v;

		nm.RecalculateBounds ();
		nm.RecalculateNormals ();

		return nm;

	}

	 float getXGap(Vector3[] v){
		float[] f = getVerticesValues (v, 0);
		float diff = getMax(f) - getMin (f); 
		return diff;
	}

	float getYGap(Vector3[] v){
		float[] f = getVerticesValues (v, 1);
		float diff = getMax(f) - getMin (f); 
		return diff;
	}

	float getZGap(Vector3[] v){
		float[] f = getVerticesValues (v, 2);
		float diff = getMax(f) - getMin (f); 
		return diff;
	}

	float getMin(float[] f){
		float min = f [0];

		for (int i = 0; i < f.Length; i++) {
			if(f[i]<min)min=f[i];
		}
		return min;
	}

	float getMax(float[] f){
		float max = f [0];

		for (int i = 0; i < f.Length; i++) {
			if(f[i]>max)max=f[i];
		}
		return max;
	}

	float[] getVerticesValues(Vector3[] v , int i){
		float[] t = new float[v.Length];
		for(int j = 0 ; j < t.Length ; j++){
			if(i==0)t[j]=v[j].x;
			if(i==1)t[j]=v[j].y;
			if(i==2)t[j]=v[j].z;

		}
		return t;
	}

	public int returnXScaling(){
		return scaleXValue;
	}
	
	public int returnYScaling(){
		return scaleYValue;
	}
	
	public int returnZScaling(){
		return scaleZValue;
	}


}
