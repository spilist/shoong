using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class WallCreator : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public GameObject wallUnit;
	public GameObject wallScoreTextUnit;
	int maxWallUnitCount = 40;
	int currWallUnitCount = 40;
	int currBuildingWallUnitCount = 40;
	float wallHeight;
	float wallThickness;
	float floorThickness;
	Vector3 startPoint;
	Vector3 dragPoint;
	Vector3 endPoint;
	Wall currentWall;
	Dictionary<int, Wall> wallGroups;

	// Use this for initialization
	void Start () {
		// This disables touch input
		this.gameObject.GetComponent<BoxCollider>().enabled = false;
		GameInfoMaker.instance.maxWallUnitCount = maxWallUnitCount;
		GameInfoMaker.notifyUpdate();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void runWallCreator() {
		this.gameObject.GetComponent<BoxCollider>().enabled = true;
		wallGroups = new Dictionary<int, Wall>();
		//wallThickness = wallUnit.GetComponent<MeshFilter>().mesh.bounds.extents.z;
		wallThickness = wallUnit.GetComponent<Renderer>().bounds.size.z;
		floorThickness = 0;
		wallHeight = wallUnit.GetComponent<Renderer>().bounds.size.y;
	}

	public void destroyWall(GameObject wall, ContactPoint contact, float velocity) {
		Wall targetWall;
		wallGroups.TryGetValue(wall.GetHashCode(), out targetWall);
		wallGroups.Remove(wall.GetHashCode());
		currWallUnitCount += targetWall.currentWallLength;
		GameInfoMaker.instance.currWallUnitCount = currWallUnitCount;
		int score = GameInfoMaker.updateWallScore(velocity, targetWall.currentWallLength);
		GameInfoMaker.notifyUpdate();
		GameObject wallScoreText = (GameObject) Instantiate (wallScoreTextUnit, contact.point, Quaternion.Euler(new Vector3(90, 0, 0)));
		wallScoreText.GetComponent<TextMesh>().text = "+" + score.ToString();
		Hashtable args = new Hashtable();
		args.Add("alpha", 0);
		args.Add("delay", 1);
		args.Add("time", 0.5f);
		args.Add("oncomplete", "Destroy");
		//args.Add("oncompletetarget", );
		iTween.FadeTo(wallScoreText, args);
		targetWall.destroySelf();
	}


	public void OnPointerDown (PointerEventData eventData)
	{
		Debug.Log ("OnPointerDown called");
		if (currWallUnitCount == 0)
			return;
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		startPoint = hit.point;
		// thickness is cube unit
		startPoint.y += wallHeight / 2 + floorThickness;
		currentWall = new Wall(wallUnit);
	}

	public void OnDrag (PointerEventData eventData)
	{
		Debug.Log ("OnDrag called");	
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		dragPoint = hit.point;
		dragPoint.y += wallHeight / 2 + floorThickness;
		int usedWalLcount = currentWall.moveWall(startPoint, dragPoint, currWallUnitCount);
		currBuildingWallUnitCount = currWallUnitCount - usedWalLcount;
		GameInfoMaker.instance.currWallUnitCount = currBuildingWallUnitCount;
		GameInfoMaker.notifyUpdate();
	}
	
	public void OnPointerUp (PointerEventData eventData)
	{
		Debug.Log ("OnPointerUp called");
		/*
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		endPoint = hit.point;
		endPoint.y += wallHeight / 2 + floorThickness;
		*/
		currWallUnitCount = currBuildingWallUnitCount;
		currentWall.setCollider();
		wallGroups.Add(currentWall.wall.GetHashCode(), currentWall);
		currentWall = null;
	}


	class Wall {
		public int startTime;
		public int expirePeriod = 5;
		public int currentWallLength = 0;
		//public GameObject wallUnit;
		public GameObject wall;
		float wallThickness;
		public Wall(GameObject wallUnit) {
			wallThickness = wallUnit.GetComponent<Renderer>().bounds.size.z;
			wall = (GameObject) Instantiate (wallUnit, new Vector3(-5,-5,-5), Quaternion.identity);
		}
		public int moveWall(Vector3 startPoint, Vector3 endPoint, int currWallCount) {	
			float distance = Vector3.Distance (startPoint, endPoint);		
			//Vector3 position = Vector3.MoveTowards (startPoint, endPoint, distance / 2);
			Vector3 position = startPoint;
			Vector3 direction = (endPoint - startPoint).normalized;
			Quaternion rotation = Quaternion.LookRotation(direction);
			wall.transform.position = position;
			wall.transform.rotation = rotation;
			Debug.Log ("Wall thickness: " + wallThickness + ", distance: " + distance);
			int discreteUnitLength = (int)(distance / wallThickness) + 1;
			currentWallLength = (discreteUnitLength < currWallCount) ? discreteUnitLength : currWallCount;
			wall.transform.localScale = new Vector3(1.0f, 1.0f, currentWallLength);
			//Texture2D tex = Resources.Load("11") as Texture2D;
			//tex.
			//tex.width = (int) (tex.width / discreteUnitLength);
			//wall.GetComponent<Renderer>().material.mainTexture = tex;
			/*
			wall.GetComponent<Renderer>().material.mainTextureOffset =
				new Vector2(1.5f * discreteUnitLength, 1);
				*/
			return currentWallLength;
		}
		public void setCollider() {
			wall.AddComponent<BoxCollider>();
		}
		public void destroySelf() {
			// DO_SOMETHING
			Destroy (wall);
		}
	}

}
