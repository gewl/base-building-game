using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	public Sprite floorSprite; // FIXME
	public Sprite wallSprite; // FIXME

	Dictionary<Tile, GameObject> tileGameObjectMap;
	Dictionary<InstalledObject, GameObject> installedObjectGameObjectMap;

	public World World { get; protected set; }

	void Start () {
		if (Instance != null) {
			Debug.LogError ("Duplicate world controllers.");
		}
		Instance = this;

		//instantiate empty world
		World = new World ();

		World.RegisterInstalledObjectCreated (OnInstalledObjectCreated);

		tileGameObjectMap = new Dictionary<Tile, GameObject> ();
		installedObjectGameObjectMap = new Dictionary<InstalledObject, GameObject> ();

		//create a GameObject for each tile for rendering
		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Tile tile_data = World.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();

				// add tile/go pair to dict
				tileGameObjectMap.Add( tile_data, tile_go );

				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (tile_data.X, tile_data.Y);
				tile_go.transform.SetParent(this.transform, true);

				//add spriterendered w/o sprite, as tiles are empty
				tile_go.AddComponent<SpriteRenderer> ();

				tile_data.cbRegisterTileTypeChanged ( (tile) => { OnTileTypeChanged(tile, tile_go); } );
			}
		}

		World.RandomizeTiles ();
	}

	float randomizeTileTimer = 2f;

	void Update () {
		
	}

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
		if (tile_data.Type == Tile.TileType.Floor) {
			tile_go.GetComponent<SpriteRenderer> ().sprite = floorSprite;
		} else if (tile_data.Type == Tile.TileType.Empty) {
			tile_go.GetComponent<SpriteRenderer> ().sprite = null;
		} else {
			Debug.LogError ("Unexpected tiletype.");
		}
	}

	Tile GetTileAtWorldCoord(Vector3 coord) {
		int x = Mathf.FloorToInt (coord.x);
		int y = Mathf.FloorToInt (coord.y);

		return World.GetTileAt (x, y);
	}

	public void OnInstalledObjectCreated( InstalledObject obj ) {
		// Create a visual GameObject linked to this data.

		GameObject obj_go = new GameObject ();

		// add obj/go pair to dict
		installedObjectGameObjectMap.Add (obj, obj_go);

		obj_go.name = obj.objectType + "_" + obj.tile.X + "_" + obj.tile.Y;
		obj_go.transform.position = new Vector3 (obj.tile.X, obj.tile.Y);
		obj_go.transform.SetParent(this.transform, true);

		// FIXME: assumes object must be wall, hence hardcoded reference to wallSprite
		obj_go.AddComponent<SpriteRenderer> ().sprite = wallSprite; //FIXME

		// register callback so that GameObject gets updated when tile's type changes
		obj.RegisterOnChangedCallback ( OnInstalledObjectChanged );
	}

	void OnInstalledObjectChanged( InstalledObject obj )  {
		Debug.LogError ("OnInstalledObjectChanged -- NOT IMPLEMENTED");
	}
}