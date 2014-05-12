using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))]
public class GoToScene : MonoBehaviour {

	public string sceneName;
	
	void OnMouseUpAsButton () {
		Application.LoadLevel(sceneName);
	}
	
}
