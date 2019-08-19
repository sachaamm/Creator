using UnityEngine;
using System.Collections;

public class ChangeMaterialOnMouseOver : MonoBehaviour {

	public Material newMat;
	Material init;
	MeshRenderer mr;
	


	void Start () {

		mr = GetComponent<MeshRenderer> ();
		if (mr == null) {
			Transform child = transform.Find("default");
			child.gameObject.AddComponent<ChangeMaterialOnMouseOver>();
			ChangeMaterialOnMouseOver onMouseOver = (ChangeMaterialOnMouseOver)child.gameObject.GetComponent(typeof(ChangeMaterialOnMouseOver));
			onMouseOver.setMaterial (newMat);
			child.gameObject.AddComponent<MeshCollider>();

			Destroy (this);

			return;
		}
		init = mr.material;
	}


	void OnMouseEnter(){

		mr.material = newMat;

	}

	void OnMouseDown(){

		GameObject menu = GameObject.Find ("MenuInterface");
		MenuScript menuScript = (MenuScript)menu.GetComponent (typeof(MenuScript));
		menuScript.setSelected (this.gameObject);

	}


	void OnMouseOver(){

		if (Input.GetKeyDown (KeyCode.Delete)) {

			GameObject camera = GameObject.Find ("Main Camera");
			MovePoint movePoint = (MovePoint)camera.GetComponent(typeof(MovePoint));
			InstantiateGrid instantiateGrid = (InstantiateGrid)camera.GetComponent(typeof(InstantiateGrid));

			int currentIndex = movePoint.convertPosToIndex(transform.position);
			instantiateGrid.removeCube(currentIndex);
	
			Destroy(this.gameObject);
		}


	}

	void OnMouseExit(){
		mr.material = init;
	}


	public void setInit(Material m){
		
		init = m;
		
	}



	public void setMaterial(Material m){
		newMat = m;
		
	}


}
