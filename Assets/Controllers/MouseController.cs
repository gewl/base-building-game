using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

	public GameObject circleCursorPrefab;

	bool 		  	buildModeIsObjects = false;
	Tile.TileType 	buildModeTile = Tile.TileType.Floor;
	string 		  	buildModeObjectType;

	Vector3 lastFramePosition;
	Vector3 currFramePosition;

	Vector3 dragStartPosition;
	List<GameObject> dragPreviewGameObjects;

	void Start () {
		dragPreviewGameObjects = new List<GameObject> ();
	}

	void Update () {
		currFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currFramePosition.z = 0;
		
		//UpdateCursor ();
		UpdateDragging ();
		UpdateCameraMovement ();

		lastFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		lastFramePosition.z = 0;
	}

//	void UpdateCursor() {
//		//update cursor reticule position
//		Tile tileUnderMouse = GetTileAtWorldCoord(currFramePosition);
//		if (tileUnderMouse != null) {
//			circleCursor.SetActive (true);
//			Vector3 cursorPosition = new Vector3 (tileUnderMouse.X, tileUnderMouse.Y, 0);
//			circleCursor.transform.position = cursorPosition;
//		} else {
//			circleCursor.SetActive (false);
//		}
//	}

	void UpdateDragging() {
		//if we are over a UI element, block behavior
		if ( EventSystem.current.IsPointerOverGameObject() ) {
			return;
		}

		// start drag
		if (Input.GetMouseButtonDown(0)) {
			dragStartPosition = currFramePosition;
		}

		int start_x = Mathf.FloorToInt (dragStartPosition.x);
		int end_x = Mathf.FloorToInt (currFramePosition.x);
		int start_y = Mathf.FloorToInt (dragStartPosition.y);
		int end_y = Mathf.FloorToInt (currFramePosition.y);

		if (end_x < start_x) {
			int tmp = end_x;
			end_x = start_x;
			start_x = tmp;
		}

		if (end_y < start_y) {
			int tmp = end_y;
			end_y = start_y;
			start_y = tmp;
		}

		// clean up old drag previews
		while (dragPreviewGameObjects.Count > 0) {
			GameObject go = dragPreviewGameObjects [0];
			dragPreviewGameObjects.RemoveAt (0);
			SimplePool.Despawn (go);
		}

		if (Input.GetMouseButton(0)) {
			// preview of drag area
			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.Instance.World.GetTileAt (x, y);
					if (t != null) {
						// display building hint on tile position
						GameObject go = SimplePool.Spawn( circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity );
						go.transform.SetParent (this.transform, true);
						dragPreviewGameObjects.Add (go);
					}
				}
			}
		}

		// end drag
		if (Input.GetMouseButtonUp(0)) {
			for (int x = start_x; x <= end_x; x++) {
				for (int y = start_y; y <= end_y; y++) {
					Tile t = WorldController.Instance.World.GetTileAt (x, y);
					if (t != null) {
						if (buildModeIsObjects) {
							// create the InstalledObject and assign it to the tile

							// FIXME: only applies to walls rn
							WorldController.Instance.World.PlaceInstalledObject( buildModeObjectType, t );

						} else {
							// tile-changing mode
							t.Type = buildModeTile;
						}							
					}
				}
			}
		}
	}

	void UpdateCameraMovement(){
		// screen dragging for camera movement
		if ( Input.GetMouseButton(1) || Input.GetMouseButton(2) ) { // rmb or mmb
			Vector3 diff = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate (diff);
		}

		// scrollwheel for zoom
		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");

		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 3f, 25f);
	}

	public void SetMode_BuildFloor() {
		buildModeIsObjects = false;
		buildModeTile = Tile.TileType.Floor;

	}

	public void SetMode_BuildInstalledObject( string objectType ) {
		buildModeIsObjects = true;
		buildModeObjectType = objectType;
	}

	public void SetMode_Bulldoze() {
		buildModeIsObjects = false;
		buildModeTile = Tile.TileType.Empty;
	}



	
}

