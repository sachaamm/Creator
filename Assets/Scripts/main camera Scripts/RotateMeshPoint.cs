using UnityEngine;
using System.Collections;

public class RotateMeshPoint : MonoBehaviour {
	public GameObject meshPoint;

	MovePoint movePoint;
	int step=0;
	// Use this for initialization
	void Start () {
		movePoint = (MovePoint)GetComponent (typeof(MovePoint));
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.R)) {
			meshPoint.transform.Rotate(new Vector3(0,1,0) * 90);
			step++;

			movePoint.fixPointMeshPosition();
			movePoint.updateAxisPosition();

		}
	}

	public int returnStep(){
		return step;
	}
}
