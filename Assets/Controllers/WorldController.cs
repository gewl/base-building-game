using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	public static WorldController Instance { get; protected set; }

	public Sprite floorSprite;

	public World World { get; protected set; }

	void Start () {
		if (Instance != null) {
			Debug.LogError ("Duplicate world controllers.");
		}
		Instance = this;

		//instantiate empty world
		World = new World ();

		//create a GameObject for each tile for rendering
		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Tile tile_data = World.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();
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
}
