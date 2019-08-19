using UnityEngine;
//using System;
using System.Collections;
using System.Reflection;

public class SelectObjectToCreate : MonoBehaviour {

	// les Modeles selectionnes 
	public GameObject[] objects;
	// les Materiaux selectionnes
	public Material[] materials;
	// les Scripts selectionnes
	public Object[] scripts;
	Object currentScript;


	// sauver Skybox
	Material initSkybox;

	// objet exemple
	GameObject currentObject;
	//pointMesh
	public GameObject pointMesh;
	//texture to draw sample example
	public RenderTexture tex;

	int step=0;
	//spawner
	public GameObject spawner;
	//menuInterface
	public GameObject menuObject;
	MenuScript menuScript;

	public GameObject ground;

	string typeName="";

	// Use this for initialization
	void Start () {
		//currentObject starts as a Cube
		currentObject = GameObject.Instantiate (objects [0]);
		currentObject.transform.position = spawner.transform.position;
		currentObject.transform.parent = spawner.transform;
		//this is the Menu Script
		menuScript = (MenuScript)menuObject.GetComponent (typeof(MenuScript));
		//save the default Skybox
		initSkybox = RenderSettings.skybox;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	

	}
	// init Models from menu selected
	public void initGameObjectsSelection(string path){
	
		Object[] newObjects = (Object[]) Resources.LoadAll(path);
		
		GameObject[] newGameObjects = new GameObject[newObjects.Length];

		for (int i = 0; i < newObjects.Length; i++) {
			newGameObjects[i] = newObjects[i] as GameObject;
		}

	objects = newGameObjects;

	}

	public void initMaterials(string path){

		Object[] newObjects = (Object[]) Resources.LoadAll(path);
		Material[] newMaterials = new Material[newObjects.Length];

		for (int i = 0; i < newObjects.Length; i++) {
			newMaterials[i] = newObjects[i] as Material;
		}

		materials = newMaterials;

	}

	public void initScripts(string path){
		Object[] newObjects = (Object[]) Resources.LoadAll(path);
		scripts = newObjects;
	}




	void OnGUI(){

		// si on est pas dans un menu terminal ( menu ou sont loades les exemples )
		if (menuScript.isTerminalMenu ()) {

			float x, y, w, h;
			x = 10;
			y = 10;
			w = 60;
			h = 60;
			float offset = 10;

			GUI.DrawTexture (new Rect (x, y, w, h), tex);

			float ww = 25;

			// Which type of Object are we selecting ? (Model ? Material ? Script ? )
			int menuMode = menuScript.returnMenuModeVaue();

			// GO TO NEXT OBJECT
			if (GUI.Button (new Rect (x, y + h + offset, ww, ww), "<<")){

				if(menuMode == 0)selectNewObject (-1);
				if(menuMode == 1)selectNewMaterial(-1);
				if(menuMode == 2)selectNewScript(-1);
			}

			if (GUI.Button (new Rect (x + ww + offset, y + h + offset, ww, ww), ">>")){

				if(menuMode == 0)selectNewObject (1);
				if(menuMode == 1)selectNewMaterial(1);
				if(menuMode == 2)selectNewScript(1);

			}

		}
		
		


		
	}

	//restart selection with first
	public void restartSelectionWithNewObject(){
		step = 0;
		if (currentObject != null) {
			Destroy (currentObject);
		}

		currentObject = GameObject.Instantiate (objects [step % objects.Length]);
		currentObject.transform.position = spawner.transform.position;
		currentObject.transform.name = "Exemple";
		//Je change le mesh de l'objet en cours
		ChangePointMesh changeMesh;
		changeMesh = (ChangePointMesh)GetComponent(typeof(ChangePointMesh));
		changeMesh.switchPointMesh(currentObject);

		//Je mets à jours les valeurs de scaling
		ScalePoint scalePoint = (ScalePoint)GetComponent(typeof(ScalePoint));
		scalePoint.updateScaling();
	}

	
	void selectNewObject(int i){

		step+=i;
		if(step<0)step+=objects.Length;
		
		//Je detruis l'ancien pour créer le nouveau
		Destroy (currentObject);
		currentObject = GameObject.Instantiate (objects [step % objects.Length]);
		currentObject.transform.position = spawner.transform.position;
		currentObject.transform.name = "Exemple";
		//Je change le mesh de l'objet en cours
		ChangePointMesh changeMesh;
		changeMesh = (ChangePointMesh)GetComponent(typeof(ChangePointMesh));
		changeMesh.switchPointMesh(currentObject);
				
		//Je mets à jours les valeurs de scaling
		ScalePoint scalePoint = (ScalePoint)GetComponent(typeof(ScalePoint));
		scalePoint.updateScaling();

	}



	void selectNewMaterial(int i){

		step += i;
		if(step<0)step+=materials.Length;
		int menuId = menuScript.returnMenuId ();
	
		// SKYBOX MATERIAL SELECTION
		if (menuId == 8) {
			RenderSettings.skybox = materials[step%materials.Length];
			if(step % materials.Length == materials.Length -1)RenderSettings.skybox = initSkybox;
			return;
		}

		// GROUND MATERIAL SELECTION
		if (menuId == 9) {
			MeshRenderer fmr = ground.GetComponent<MeshRenderer>();
			fmr.material = materials[step%materials.Length];
			return;
		}

		GameObject selected = menuScript.returnSelected ();

		//Je detruis l'ancien pour créer le nouveau
		Destroy (currentObject);
		currentObject = GameObject.Instantiate (selected);
		currentObject.transform.position = spawner.transform.position;
		MeshRenderer mr = currentObject.GetComponent<MeshRenderer> ();
		mr.material = materials[step% materials.Length];
		MeshRenderer mr2 = selected.GetComponent<MeshRenderer> ();
		mr2.material = materials[step% materials.Length];
		ChangeMaterialOnMouseOver change = (ChangeMaterialOnMouseOver)selected.GetComponent (typeof(ChangeMaterialOnMouseOver));
		change.setInit (materials [step % materials.Length]);

		//Je change le mesh de l'objet en cours
		ChangePointMesh changeMesh;
		changeMesh = (ChangePointMesh)GetComponent(typeof(ChangePointMesh));
		//switch pointMesh
		changeMesh.switchPointMesh(currentObject);
				
		//Je mets à jours les valeurs de scaling
		ScalePoint scalePoint = (ScalePoint)GetComponent(typeof(ScalePoint));
		scalePoint.updateScaling();


	}

	public void resetScriptTypeValues(){
		typeName = "";
	
	}



	void selectNewScript(int i){
		step += i;
		if(step<0)step+=scripts.Length;

		GameObject selected = menuScript.returnSelected ();

		if (typeName.Length > 0) {
			Destroy (currentObject.GetComponent(typeName));
			Destroy (selected.GetComponent(typeName));
		}

		currentScript = scripts [step % scripts.Length];
		typeName = currentScript.name;

		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (currentObject, "Assets/Scripts/main camera Scripts/SelectObjectToCreate.cs (228,3)", typeName);
		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent (selected, "Assets/Scripts/main camera Scripts/SelectObjectToCreate.cs (228,3)", typeName);

	}
	/*
	public static void AddUnknownComponent<T>(this GameObject gameObject) where T : BasicRotate{
		string type = "BasicRotate";


	}*/



	// return exemple
	public GameObject returnCurrentObject(){
		return currentObject;
	}


}
