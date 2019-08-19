using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MeshUtility : MonoBehaviour {

	public GameObject empty;

	public GameObject test;

	void Start(){

		Mesh mf = returnOneJoinedMeshFilter (test);
		print (" empty mesh is now "+mf.vertexCount+"  vertices long ,  it get  "+mf.triangles.Length+ " triangles");

		MeshFilter mfa = empty.GetComponent<MeshFilter> () ;
		if (mfa == null) {
			empty.AddComponent<MeshFilter> ();
			empty.AddComponent<MeshRenderer> ();
		}

		MeshFilter mfe = empty.GetComponent<MeshFilter> () ;
		mfe.mesh = mf;

	}


	public Vector3 getMeshCenterMass(Mesh m){

		Vector3[] v = m.vertices;
		float xTotal = 0;
		float yTotal = 0;
		float zTotal = 0;


		int lg = v.Length;
		for (int i = 0; i < v.Length; i++) {
			xTotal+=v[i].x;
			yTotal+=v[i].y;
			zTotal+=v[i].z;

		}

		return new Vector3(xTotal / lg , yTotal / lg , zTotal / lg );

	}

	public Vector3 getMeshCenter(Mesh m ){

		Vector3[] v = m.vertices;

		float xMin = getMinCoordinate (v, 0);
		float xMax = getMaxCoordinate (v, 0);
			float yMin = getMinCoordinate (v, 1);
		float yMax = getMaxCoordinate (v, 1);
			float zMin = getMinCoordinate (v, 2);
		float zMax = getMaxCoordinate (v, 2);

		float xCenter = (xMin + xMax)/2;
			float yCenter =( yMin + yMax)/2;
		float zCenter = (zMin + zMax)/2;

		return new Vector3 (xCenter, yCenter, zCenter);

	}

	
	public Mesh returnOneJoinedMeshFilter(GameObject o){

		List<MeshFilter> lmf = new List<MeshFilter> ();
		List<Vector3> localPositions = new List<Vector3> ();

		seekMeshFilterChilds (lmf,localPositions,o.transform.position, o.transform);
		// on renvoie vide quand il n'y a aucun mesh filter
		if (lmf.Count == 0)
			return new Mesh ();

	
		List<int[]> lmfFaces = new List<int[]> ();

		List<Vector3[]> vert = new List<Vector3[]> ();

		for(int i = 0 ; i < lmf.Count ; i++){
			lmfFaces.Add (lmf[i].mesh.triangles);
			Vector3[] updateV = 
			updateVertices (lmf[i].mesh.vertices , localPositions[i]);
			vert.Add (updateV); 
			}


		for (int i = 1; i < lmfFaces.Count; i++) {
			int newIndex = getIntMax(lmfFaces[i-1]) +  1;
			lmfFaces[i] = updateMeshIndex(lmfFaces[i], newIndex );
		}

		Vector3[] finalVertices = joinVertices (vert);
			int[] finalFaces = joinTriangles(lmfFaces);


		Mesh finalMesh = new Mesh();
		finalMesh.vertices = finalVertices;
		finalMesh.triangles = finalFaces;
		finalMesh.RecalculateBounds ();
		finalMesh.RecalculateNormals ();


		return finalMesh;

	}


	void seekMeshFilterChilds(List<MeshFilter> l , List<Vector3> v, Vector3 origin, Transform t){
		foreach (Transform child in t) {
			MeshFilter mf = child.GetComponent<MeshFilter>();
			if(mf!=null){
				l.Add(mf);
				v.Add (child.position-origin);
			
			}

			seekMeshFilterChilds (l ,v, origin, child);


		}
	}


	
	int[] joinTriangles(List<int[]> ls){
		
		int lg = 0;
		for(int i = 0 ; i < ls.Count ; i++){
			lg+=ls[i].Length;
		}
		
		int[] joined = new int[lg];
		lg = 0;
		
		for (int i = 0; i < ls.Count; i++) {
			for(int j = 0 ; j < ls[i].Length ; j++){
				int[] a = ls[i];
				joined[j + lg] = a[j];
			}
			lg+= ls[i].Length;
		}
		
		return joined;
		
	}
	
	Vector3[] joinVertices(List<Vector3[]> ls){
		
		int lg = 0;
		for(int i = 0 ; i < ls.Count ; i++){
			lg+=ls[i].Length;
		}
		Vector3[] joined = new Vector3[lg];
		lg = 0;
		
		for (int i = 0; i < ls.Count; i++) {
			for(int j = 0 ; j < ls[i].Length ; j++){
				Vector3[] a = ls[i];
				joined[j + lg] = a[j];
			}
			lg+= ls[i].Length;
		}
		
		return joined;
		
	}
	
	
	int[] updateMeshIndex (int[] a,int indexStart){
		
		int[] na = new int[a.Length];
		for (int i = 0; i < na.Length; i++) {
			na[i] = a[i] + indexStart;
			
		}
		
		return na;
		
	}
	
	
	Vector3[] updateVertices(Vector3[] v, Vector3 u){
		
		for (int i = 0; i < v.Length; i++) {
			
			v[i]+=u;
			
		}
		
		return v;
	}
	


	float getMaxCoordinate(Vector3[] v , int dimension){
		
		float max = -Mathf.Infinity;
		
		float[] f = convertFromVectorToFloat (v, dimension);
		max = getMax(f);
		
		if (v.Length == 0)
			return 0;
		return max;
		
	}
	
	float getMinCoordinate(Vector3[] v , int dimension){
		
		float min = Mathf.Infinity;
		
		float[] f = convertFromVectorToFloat (v, dimension);
		min = getMin(f);
		
		if (v.Length == 0)
			return 0;
		return min;
	}
	
	float getMax(float[] f){
		
		float max = -Mathf.Infinity;
		
		for (int i = 0; i < f.Length; i++) {
			if(max < f[i])max = f[i];
			
		}
		
		
		return max;
	}
	
	float getMin(float[] f){
		
		float min = Mathf.Infinity;
		for (int i = 0; i < f.Length; i++) {
			if(min > f[i])min = f[i];
			
		}
		
		return min;
	}
	
	int getIntMax(int[] f){
		
		int max= -1000000;
		for (int i = 0; i < f.Length; i++) {
			if(max < f[i])max = f[i];
			
		}
		
		return max;
	}
	
	float[] convertFromVectorToFloat(Vector3[] v , int dimension){
		
		float[] f = new float[v.Length];
		
		for (int i = 0; i < v.Length; i++) {
			if(dimension==0)f[i] = v[i].x;
			if(dimension==1)f[i] = v[i].y;
			if(dimension==2)f[i] = v[i].z;
			
		}
		
		return f;
	}
	



}
