using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile {

	public enum TileType { Empty, Floor };
	TileType _type = TileType.Empty;
	LooseObject looseObject;
	InstalledObject installedObject;
	//action delegator initialized
	Action<Tile> cbTileTypeChanged;

	World world;
	public int X { get; protected set; }
	public int Y { get; protected set; }

	//setter & getter
	//setter handles tile change event to prevent need for polling
	public TileType Type {
		get {
			return _type;
		}
		set {
			TileType oldType = _type;
			_type = value;

			//call callback to update tile sprite
			//null check for safety
			if (cbTileTypeChanged != null && oldType != _type) {
				cbTileTypeChanged(this);	
			}

		}
	}
		
	public Tile( World world, int x, int y ) {
		this.world = world;
		this.X = x;
		this.Y = y;
	}

	//adds cb function to cbTileTypeChanged array, all of which fire
	//on fx call
	public void cbRegisterTileTypeChanged(Action<Tile> callback){
		cbTileTypeChanged += callback;
	}

	//removes cb function from array
	public void cbUnregisterTileTypeChanged(Action<Tile> callback){
		cbTileTypeChanged -= callback;
	}

}
