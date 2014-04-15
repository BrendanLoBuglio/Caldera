/*using UnityEngine;
using System.Collections;

public class Phantom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 moveDist = new Vector2(Input.GetAxis("Horizontal")*3f*Time.deltaTime,Input.GetAxis("Vertical")*3f*Time.deltaTime);
		transform.position = new Vector2(transform.position.x + moveDist.x,transform.position.y + moveDist.y);
		
		//bufferTime += Time.deltaTime;
	}
	
	void OnCollisionStay2D(Collision2D other) {
		if (Input.GetKey (KeyCode.Space) && bufferTime >= 1f)
		{ 
			Debug.Log ("Halfway there!");
			if(other.gameObject.GetComponent<moveme>() != null)
			{
				moveThang.enabled = false;
				other.gameObject.GetComponent<moveme>().Reset();
			}
		}
	}
	
	void TransferConsciousness(GameObject otherCreature)
	{
		//Disable, hide the gameObject, and enable the player controle
	}
	
	void Reappear()
	{
	
	}
}
*/
