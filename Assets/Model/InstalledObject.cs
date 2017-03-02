using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// e.g. walls, doors, furniture

public class InstalledObject {

	// this represents BASE tile, but contingent on size object may occupy multiple tiles
	public Tile tile { get; protected set; }

	// this will be queried by visual system for rendering purposes
	public string objectType { get; protected set; }

	//	multiplier, so value of 2 would mean movespeed is halved. stacks. (e.g. 'rough' (2f) + table (3f) + fire (3f) = 8f)
	// 	if movement cost = 0, then tile cannot be moved through (e.g. wall)
	float movementCost; 

	int width;
	int height;

	Action<InstalledObject> cbOnChanged;

	// TODO: implement larger objects
	// TODO: implement object rotation

	static public InstalledObject CreatePrototype ( string objectType, float movementCost = 1f, int width = 1, int height = 1 ){
		InstalledObject obj = new InstalledObject ();

		obj.objectType = objectType;
		obj.movementCost = movementCost;
		obj.width = width;
		obj.height = height;

		return obj;
	}

	static public InstalledObject PlaceInstance ( InstalledObject proto, Tile tile ){
		InstalledObject obj = new InstalledObject ();

		obj.objectType = proto.objectType;
		obj.movementCost = proto.movementCost;
		obj.width = proto.width;
		obj.height = proto.height;

		obj.tile = tile;

		//FIXME: this assumes we are one-by-one
		if ( !tile.PlaceObject(obj) ) {
			// Weren't able to place object in this tile. Probably already occupied.

			// DON'T return newly instantiated object if it wasn't installed successfully. Instead:
			return null;
		}
		return obj;
	}

	public void RegisterOnChangedCallback(Action<InstalledObject> callbackFunc) {
		cbOnChanged += callbackFunc;
	}
	public void UnregisterOnChangedCallback(Action<InstalledObject> callbackFunc) {
		cbOnChanged -= callbackFunc;
	}
}
