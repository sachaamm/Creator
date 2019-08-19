using UnityEngine;
using System.Collections;

public class ChangePointMesh : MonoBehaviour {
	public GameObject pointMesh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void switchPointMesh(GameObject o){
	
		MeshFilter mr, mr2;
		mr = pointMesh.transform.GetComponent<MeshFilter> ();
	

		mr2 = o.transform.GetComponent<MeshFilter> ();
		if (mr2 == null) {
			Transform blenderDefault = o.transform.Find ("default");

			if(blenderDefault!=null)mr2 = blenderDefault.GetComponent<MeshFilter>();
		}


		if(mr2!=null)mr.mesh = mr2.mesh;

	
	}

}
