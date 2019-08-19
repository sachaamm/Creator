using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {
	
	int[] buildModes = {  2 , 3 , 4 , 5   };
	int[] scriptModes = { 11 };
	int[] materialModes = { 8, 9 , 13 , 14  };
	
    string[] menuNames = {
	//0
		"Main Menu",
	//1
		"Main Builder Menu",
	//2
		"Geometrie",
	//3
		"Architecture",
	//4
		"Environnement",
	//5
		"Lumiere",
	//6
		"Variables Environnement",
	//7
		"Angle Lumiere",
	//8
		"Skybox",
	//9
		"Ground",
	//10
		"Selection Menu",
	//11
		"Ajouter Script",
	//12
		"Selection_Material",
	//13
		"Basics",
	//14
		"Textures"

	};

	int efjn;

	// 0
	string[] mainMenu = {"Construire","Variables Environnement"};
	int[] mainMenuChildIds = {1,6};
	// 1 
	string[] mainBuilderMenu = {"Geometrie","Architecture","Environnement","Lumiere"};
	int[] mainBuilderMenuChildIds  = {2,3,4,5};


	//6
	string[] mainEnvironnementMenu = {"Angle Lumiere","Skybox","Ground"};
	int[] environnementChildIds = { 7 , 8 , 9 };
	

	//10
	string[] selectionMainMenu = { "Add Script","Change Material" };
	int[] selectionMainMenuChildIds = { 11 , 12 };

	//12
	string[] materialMainMenu = { "Basics" ,"Texture" };
	int[] materialMainMenuIds = { 13 , 14 };


	string[] emptyMenu = {};
	int[] emptyMenuId = {};



	/// selection
	SelectObjectToCreate select;
	GameMode gameMode;

	public GameObject mainCamera;
	public GameObject gameInterface;

	GameObject selected,ghost;
	public GameObject ghostModel;

	public Material selectionMaterial;
	
	
	public int currentIndexMenu = 0 ;
	

	
	List<MenuItem> allMenus;

	// Use this for initialization
	void Start () {

		InstantiateMenu ();

		select = (SelectObjectToCreate)mainCamera.GetComponent (typeof(SelectObjectToCreate));
		gameMode = (GameMode)gameInterface.GetComponent (typeof(GameMode));

	}




	void displayCurrentMenu(){
		
		allMenus [currentIndexMenu].displaySubmenu ();
		
	}





	void OnGUI(){

		// Don't display menu in the explore Mode
		if (gameMode.returnStep () % 2 != 0)
			return;
			
		string[] submenus = allMenus [currentIndexMenu].returnSubMenu ();
		int[] submenusIds = allMenus [currentIndexMenu].returnSubMenuIds ();

		float w = 120 ;
		float h = 30;
		float x = 120;
		float y = 20;
		float offset = h + 10;


		for(int i = 0 ; i < submenus.Length ; i++){
			
			if(GUI.Button (new Rect(x,y + i * offset , w,h),submenus[i])){
				setCurrentMenu(submenusIds[i]);
			
			}
			
		}
		
		if (!allMenus [currentIndexMenu].isRootMenu ()) {
			if (GUI.Button (new Rect (x, y + submenus.Length * offset, w, h), "Back")) {

				if(currentIndexMenu == 10)unselect();
				int parentId = allMenus [currentIndexMenu].returnParentId ();
				setCurrentMenu (parentId);

			}

		}

		int currentId = allMenus [currentIndexMenu].returnId ();
		GUI.Button (new Rect (0, Screen.height - h, w, h), menuNames [currentId]);



	}


	
	// selection du menu
	void setCurrentMenu(int index){

		//
		if (currentIndexMenu == 11) {
			print ("reset script values");
			select.resetScriptTypeValues();
		}


		currentIndexMenu = index;
		
		int currentId = allMenus [currentIndexMenu].returnId ();
		string menuName = menuNames [currentId];
		
		int modeValue = returnMenuModeVaue ();
		
		// Si c'est un menu terminal
		if (isTerminalMenu ()) {
			// selection des modeles 3d
			if (modeValue == 0) {
				select.initGameObjectsSelection ("Models/" + menuName);
				select.restartSelectionWithNewObject ();
			}
			//selection des materiaux
			if(modeValue == 1){
				string pathName = "Materials/" +  menuName;
				select.initMaterials(pathName);
			}

			if(modeValue == 2){
				string pathName = "Scripts";
				select.initScripts(pathName);
			}

			print ("set Current Menu ( terminal Menu ) with Mode Value: "+menuName);
			
		}
		
	}


	
	public void setSelected(GameObject o){

		select.resetScriptTypeValues ();

		selected = o;
		setCurrentMenu (10);
		
		if (ghost != null)
			Destroy (ghost);
		
		ghost = GameObject.Instantiate (ghostModel);
		
		MeshFilter mr = selected.GetComponent<MeshFilter> ();
		
		ghost.GetComponent<MeshFilter> ().mesh = mr.mesh;
		ghost.GetComponent<MeshRenderer> ().material = selectionMaterial;
		
		ghost.transform.position = o.transform.position;
		ghost.transform.rotation = o.transform.rotation;
		ghost.transform.localScale = o.transform.localScale * 1.001f;
		
		
	}

	
	public void unselectAfterRemove(GameObject o){
		if (selected == o)
			selected = null;
	}
	
	void unselect(){
		selected = null;
		if (ghost != null)
			Destroy (ghost);
	}


	public int returnMenuId(){
		return allMenus [currentIndexMenu].returnId ();
		
	}


	public bool isTerminalMenu(){
		return	allMenus [currentIndexMenu].returnIsFinal ();
	}



	void InstantiateMenu(){

		allMenus = new List<MenuItem>();
		
		
		// ROOT 
		allMenus.Add (new MenuItem(0, 0, mainMenu, mainMenuChildIds, false) );
		
		
		
		// BUILDER ROOT 
		allMenus.Add (new MenuItem(1, 0, mainBuilderMenu, mainBuilderMenuChildIds, false) );
		
		// GEOMETRIE 
		allMenus.Add (new MenuItem(2, 1, emptyMenu, emptyMenuId, true) );
		// ARCHITECTURE
		allMenus.Add (new MenuItem(3, 1, emptyMenu, emptyMenuId, true) );
		// ENVIRONNEMENT
		allMenus.Add (new MenuItem(4, 1, emptyMenu, emptyMenuId, true) );
		// LUMIERE
		allMenus.Add (new MenuItem(5, 1, emptyMenu, emptyMenuId, true) );
		
		
		// ENVIRONNEMENT ROOT
		allMenus.Add (new MenuItem(6, 0, mainEnvironnementMenu, environnementChildIds, false) );
		
		// ANGLE LUMIERE
		allMenus.Add (new MenuItem(7, 6, emptyMenu, emptyMenuId, true) );
		// SKYBOX 
		allMenus.Add (new MenuItem(8, 6, emptyMenu, emptyMenuId, true) );
		// LIGHT 
		allMenus.Add (new MenuItem(9, 6, emptyMenu, emptyMenuId, true) );
		
		
		// SELECTION ROOT
		allMenus.Add (new MenuItem(10, 0, selectionMainMenu, selectionMainMenuChildIds, false) );
		
		// ADD SCRIPTS
		allMenus.Add (new MenuItem(11, 10, emptyMenu, emptyMenuId, true) );
		// SELECT MATERIAL
		allMenus.Add (new MenuItem(12, 10,materialMainMenu, materialMainMenuIds, false) );

		// Basics mat
		allMenus.Add (new MenuItem(13, 12, emptyMenu, emptyMenuId, true) );

		// Textures mat
		allMenus.Add (new MenuItem(14, 12, emptyMenu, emptyMenuId, true) );






	}

	bool arrContains(int[] a, int b){

		for (int i = 0; i < a.Length; i++) {
			if(a[i] == b)return true;

		}

		return false;
	}

	// 0 = build // 1 = material // 2 = script
		public int returnMenuModeVaue(){

		int currentId = allMenus [currentIndexMenu].returnId ();
		if(arrContains(buildModes,currentId))return 0;
		if(arrContains(materialModes,currentId))return 1;
		if(arrContains(scriptModes,currentId))return 2;


		return -1;

	}

	public GameObject returnSelected(){

		return selected;

	}








	
	class MenuItem  {
		int parentId;
		int id;
		string[] submenus;
		int[] submenusId;
		bool isFinal;
		
		public MenuItem(int i, int pId, string[] subm, int[] chIds , bool isF){
			parentId = pId;
			id = i;
			submenus = subm;
			submenusId = chIds;
			isFinal = isF;
			
			
		}
		public string[] returnSubMenu(){
			return submenus;
		}
		
		public int[] returnSubMenuIds(){
			return submenusId;
			
		}
		
		public int returnParentId(){
			return parentId;
		}
		
				public int returnId(){
			return id;
		}
		
		public void displaySubmenu(){
			
			
		}
		
		
		
		public bool returnIsFinal(){
			return isFinal;
		}
		
		public bool isRootMenu(){
			if (parentId == id)
				return true;
			
			return false;
			
		}


	}
}



