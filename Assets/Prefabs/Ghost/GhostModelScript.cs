using UnityEngine;
using System.Collections;

public class GhostModelScript : MonoBehaviour {

	MeshRenderer mr;
	float a = 1.0f;
	float acc = 0.02f;
	int step = 0;



	void Start () {
		mr = GetComponent<MeshRenderer> ();
		//mr.material

	}
	
	// Update is called once per frame
	void Update () {

		if (step % 2 == 0) {
			a-=acc;
		} else {
			a+=acc;
		}

		if (a < 0)
			step++;

		if (a > 1)
			step++;

		Color c = mr.material.color;

		//float a = (Time.frameCount % 255);
		Color nc = new Color (c.r, c.g, c.b, a);

		mr.material.color = nc;
	}
}
