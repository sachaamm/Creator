using UnityEngine;
using System.Collections;

public class Excitate : MonoBehaviour {
	Vector3 origin;
	float mag =0.1f;
	// Use this for initialization
	void Start () {
		origin = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 u = v (rdF (mag), rdF (mag), rdF (mag));
		transform.position = origin + u;
	}

	Vector3 v(float x, float y, float z){
		return new Vector3 (x, y, z);
	}

	float rdF(float a){
		return Random.Range (-a,a);
	}
	float rdUF(float a){
		return Random.Range (0,a);
	}

	void OnDestroy(){
		//print ("bye bye");
		transform.position = origin;

	}
}
