using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// e.g. walls, doors, furniture

public class InstalledObject {

	// this represents BASE tile, but contingent on size object may occupy multiple tiles
	Tile tile;

	// this will be queried by visual system for rendering purposes
	string objectType;

	//	multiplier, so value of 2 would mean movespeed is halved. stacks. (e.g. 'rough' (2f) + table (3f) + fire (3f) = 8f)
	// 	if movement cost = 0, then tile cannot be moved through (e.g. wall)
	float movementCost; 

	int width;
	int height;

	// used by object factory to create prototypical object
	public InstalledObject( string objectType, float movementCost = 1f, int width = 1, int height = 1 ){
		this.objectType = objectType;
		this.movementCost = movementCost;
		this.width = width;
		this.height = height;
	}

	protected InstalledObject( InstalledObject proto, Tile tile ){
		this.objectType = objectType;
		this.movementCost = movementCost;
		this.width = width;
		this.height = height;

		this.tile = tile;


	}

}
