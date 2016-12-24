using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	public GameObject circleCursor;
	Vector3 lastFramePosition;

	void Start () {
		
	}

	void Update () {

		Vector3 currFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currFramePosition.z = 0;

		//update cursor reticule position
		circleCursor.transform.position = currFramePosition;

		//screen dragging for camera movement
		if ( Input.GetMouseButton(1) || Input.GetMouseButton(2) ) { //rmb or mmb

			Vector3 diff = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate (diff);
			
		}

		lastFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		lastFramePosition.z = 0;

	}
}
