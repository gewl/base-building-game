﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	public Sprite floorSprite;

	World world;

	void Start () {
		//instantiate empty world
		world = new World ();

		//create a GameObject for each tile for rendering
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				Tile tile_data = world.GetTileAt (x, y);
				GameObject tile_go = new GameObject ();
				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (tile_data.X, tile_data.Y);

				//add spriterendered w/o sprite, as tiles are empty
				tile_go.AddComponent<SpriteRenderer> ();
			}
		}

		world.RandomizeTiles ();
	}

	float randomizeTileTimer = 2f;

	void Update () {
		randomizeTileTimer -= Time.deltaTime;

		if (randomizeTileTimer < 0) {
			world.RandomizeTiles ();
			randomizeTileTimer = 2f;
		}
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