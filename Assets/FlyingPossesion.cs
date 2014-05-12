using UnityEngine;
using System.Collections;

public class FlyingPossesion : PossesionController 
{
	void Update () 
	{
		float xMove = 0;
		float yMove = 0;
		
		xMove = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		yMove = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
	}
	
	void ResourceCollision(GameObject other)
	{
		if(Input.GetKey (KeyCode.B))
		{
			Resource resource = other.GetComponent<Resource>();
			resource.Consume(stateMachine, false);
		}
	}
}
