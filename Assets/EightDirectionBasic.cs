using UnityEngine;
using System.Collections;

public class EightDirectionBasic : MonoBehaviour {

	public float speed = 5;

	void Start () 
	{
	
	}
	

	void Update () 
	{
		float xMove = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
		float yMove = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
		
		transform.Translate (new Vector2(xMove, yMove));
	}
}
